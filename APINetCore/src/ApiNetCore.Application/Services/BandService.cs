using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.Procedures.Files;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ApiNetCore.Application.Services
{
    public class BandService : EntityService<BandDTO, Band>, IBandService
    {
        private readonly IBandRepository bandRepository;

        public BandService(IBandRepository bandRepository,
                            IAlertManager alertManager,
                            IMapper mapper,
                            IBusinessRules businessRules)
                            : base(alertManager, mapper, bandRepository, businessRules)
        {
            this.bandRepository = bandRepository;
        }

        public async Task<BandDTO> GetBandWithMembers(ushort id)
        {
            return MapToDto(await bandRepository.GetBandWithMembers(id));
        }

        public async Task<IEnumerable<BandDTO>> ListByMusiciansAgeAsync(int? minimumMusicianAge, int? maximumMusicianAge)
        {

            businessRules.ValidateMusicianAge(ref minimumMusicianAge);
            businessRules.ValidateMusicianAge(ref maximumMusicianAge);

            alertManager.CheckAlerts();

            var minBirthYear = DateTime.Now.Year - minimumMusicianAge;
            var maxBirthYear = DateTime.Now.Year - maximumMusicianAge;

            IEnumerable<Band> result;

            if (minimumMusicianAge > 0 && maximumMusicianAge > 0)
                result = await bandRepository.ListAsync(
                    b => b.Musicians.Any(mus => mus.DateOfBirth.Year >= minBirthYear && mus.DateOfBirth.Year <= maxBirthYear));
            else if (minimumMusicianAge > 0)
                result = await bandRepository.ListAsync(
                    b => b.Musicians.Any(mus => mus.DateOfBirth.Year >= minBirthYear));
            else if (maximumMusicianAge > 0)
                result = await bandRepository.ListAsync(
                    b => b.Musicians.Any(mus => mus.DateOfBirth.Year <= maxBirthYear));
            else
                result = await bandRepository.ListAsync();

            return MapToDto(result);
        }

        public async Task<BandDTO?> FindByNameAsync(string? name)
        {
            businessRules.ValidateBandName(ref name);

            alertManager.CheckAlerts();

            var register = await bandRepository.FindAsync(b => b.Name.Contains(name!));

            return MapToDto(register);
        }

        protected async override Task<bool> PassesDuplicityCheck(Band entityModel)
        {
            var register = await bandRepository.FindAsync(
                b => entityModel.Name == b.Name
                );

            return register is null;
        }

        protected override bool ObjectIsValid(ref BandDTO band)
        {
            var isValid = base.ObjectIsValid(ref band);

            if (isValid)
            {
                var proc = ImageProcedures.Create(alertManager);

                var oldImageFileName = band.ImageFileName;

                var newImage = false;
                if (!string.IsNullOrEmpty(band.ImageUploadingBase64))
                {
                    newImage = true;
                    band.ImageFileName = proc.SaveFileFromBase64(band.ImageUploadingBase64, band.ImageUploadingName);
                }
                else if (band.ImageUploadStream is not null && band.ImageUploadStream.Length > 0)
                {
                    newImage = true;
                    band.ImageFileName = proc.SaveFileFromStream(band.ImageUploadStream);
                }

                if (newImage)
                    proc.DeleteImage(oldImageFileName);
            }

            return isValid;
        }

        public async Task UpdateImageAsync(ushort id, IFormFile imageUpload)
        {
            var band = await bandRepository.FindByIdAsync(id);

            if (band is null)
            {
                alertManager.AddAlert("could not load register for uploading image");
                alertManager.CheckAlerts();
            }

            var proc = ImageProcedures.Create(alertManager);

            var oldImageName = band!.ImageFileName;

            band.ImageFileName = proc.SaveFileFromStream(imageUpload);

            await bandRepository.UpdateAsync(band);

            proc.DeleteImage(oldImageName);
        }
    }
}