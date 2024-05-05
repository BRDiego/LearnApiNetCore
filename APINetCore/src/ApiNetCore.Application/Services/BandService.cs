using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Application.Procedures.Files;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Interfaces;
using ApiNetCore.Business.Models;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ApiNetCore.Application.Services
{
    public class BandService : EntityService<BandDTO, Band>, IBandService
    {
        private readonly IBandRepository bandRepository;
        private readonly IUser user;

        public BandService(IBandRepository bandRepository,
                            IAlertManager alertManager,
                            IBusinessRules businessRules,
                            IMapper autoMapper,
                            IUser user)
                            : base(alertManager, bandRepository, autoMapper, businessRules)
        {
            this.bandRepository = bandRepository;
            this.user = user;
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