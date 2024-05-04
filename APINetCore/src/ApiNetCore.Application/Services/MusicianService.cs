using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.DTOs.MappingConfig;
using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Application.Procedures.Files;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ApiNetCore.Application.Services
{
    public class MusicianService : EntityService<MusicianDTO, Musician>, IMusicianService
    {
        private readonly IMusicianRepository musicianRepository;

        public MusicianService(IMusicianRepository musicianRepository,
                            IAlertManager alertManager,
                            IMapper mapper,
                            IBusinessRules businessRules)
                            : base(alertManager, musicianRepository, mapper, businessRules)
        {
            this.musicianRepository = musicianRepository;
        }

        public async Task<IEnumerable<MusicianDTO>> SearchAsync(int? musicianAge, string? surname, string? nickname)
        {
            businessRules.ValidateMusicianAge(ref musicianAge);
            businessRules.ValidateMusicianSurname(ref surname);
            businessRules.ValidateMusicianNickname(ref nickname);

            alertManager.CheckAlerts();

            var possibleYearsOfBirth = new List<int>();
            var yearOfBirth = DateTime.Now.Year - musicianAge!.Value;

            possibleYearsOfBirth.Add(yearOfBirth);
            possibleYearsOfBirth.Add(yearOfBirth - 1);

            return MapToDto(await musicianRepository.ListAsync(m =>

                    musicianAge > 0 ? possibleYearsOfBirth.Contains(m.DateOfBirth.Year) : true
                    &&
                    m.Surnames.Contains(surname!)
                    &&
                    m.Nickname.Contains(nickname!)
                    ));
        }

        public async Task UpdateImageAsync(ushort id, IFormFile imageUpload)
        {
            var musician = await musicianRepository.FindByIdAsync(id);

            if (musician is null)
            {
                alertManager.AddAlert("could not load register for uploading image");
                alertManager.CheckAlerts();
            }

            var proc = ImageProcedures.Create(alertManager);

            var oldImageName = musician!.PictureFileName;

            musician.PictureFileName = proc.SaveFileFromStream(imageUpload);

            await musicianRepository.UpdateAsync(musician);

            proc.DeleteImage(oldImageName);
        }

        protected override bool ObjectIsValid(ref MusicianDTO mus)
        {
            var isValid = base.ObjectIsValid(ref mus);

            if (isValid)
            {
                var proc = ImageProcedures.Create(alertManager);

                var oldImageFileName = mus.PictureFileName;

                var newImage = false;
                if (!string.IsNullOrEmpty(mus.ImageUploadingBase64))
                {
                    newImage = true;
                    mus.PictureFileName = proc.SaveFileFromBase64(mus.ImageUploadingBase64, mus.ImageUploadingName);
                }
                else if (mus.ImageUploadStream is not null && mus.ImageUploadStream.Length > 0)
                {
                    newImage = true;
                    mus.PictureFileName = proc.SaveFileFromStream(mus.ImageUploadStream);
                }

                if (newImage)
                    proc.DeleteImage(oldImageFileName);
            }

            return isValid;
        }

        protected async override Task<bool> PassesDuplicityCheck(Musician entityModel)
        {
            var register = await musicianRepository.FindAsync(
                m => m.Name == entityModel.Name
                &&
                m.DateOfBirth == entityModel.DateOfBirth
                );

            return register is null;
        }
    }
}