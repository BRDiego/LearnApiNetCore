using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Application.DTOs.Extensions;
using Microsoft.AspNetCore.Authorization;
using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Application.DTOs.Authentication;
using ApiNetCore.Business.Interfaces;
using ApiNetCore.Api.Controllers;
using Asp.Versioning;

namespace APINetCore.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/musicians")]
    public class MusiciansController : MainController
    {
        private readonly IMusicianService musicianService;

        public MusiciansController(IMusicianService musicianService,
                                    IAlertManager alertManager,
                                    IUser user)
                                    : base(alertManager, user)
        {
            this.musicianService = musicianService;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<MusicianDTO>>> List()
        {
            try
            {
                return CustomResponse(await musicianService.ListAsync());
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MusicianDTO>>> Search([FromForm] int? musicianAge, [FromForm] string? surname, [FromForm] string? nickname)
        {
            try
            {
                return CustomResponse(
                    await musicianService.SearchAsync(musicianAge, surname, nickname)
                    );
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Musician", "R")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MusicianDTO>> FindById(int id)
        {
            try
            {
                var parsedId = (ushort)id;
                return CustomResponse(
                    await musicianService.FindByIdAsync(parsedId)
                    );
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Musician", "R")]
        [HttpGet("{id:int}/bands")]
        public async Task<ActionResult<MusicianDTO>> GetMusicianWithBands(int id)
        {
            var parsedId = (ushort)id;

            return CustomResponse(
                await musicianService.GetMusicianWithBands(parsedId)
                );
        }

        [ClaimsAuthorization("Musician", "C")]
        [HttpPost]
        public async Task<ActionResult<MusicianDTO>> Create(MusicianDTO musicianDTO)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                await musicianService.AddAsync(musicianDTO);

                return CustomResponse(musicianDTO);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Musician", "C")]
        [HttpPost("create-with-image")]
        public async Task<ActionResult<MusicianDTO>> CreateWithImage(
            [ModelBinder(BinderType = typeof(MusicianDtoModelBinder))] MusicianImageDTO musicianDtoWithImage)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                await musicianService.AddAsync(musicianDtoWithImage);

                return CustomResponse(musicianDtoWithImage);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Musician", "U")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<MusicianDTO>> Update(int id, MusicianDTO musicianDTO)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                if (id != musicianDTO.Id)
                {
                    AlertValidation("the id doesn't match the object provided");
                    return CustomResponse(musicianDTO);
                }

                await musicianService.UpdateAsync(musicianDTO);

                return CustomResponse(musicianDTO);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Musician", "U")]
        [HttpPut("{id:int}/updateImage")]
        public async Task<ActionResult<MusicianDTO>> UpdateImage(int id, [FromForm] IFormFile imageUpload)
        {
            try
            {
                var parsedId = (ushort)id;
                await musicianService.UpdateImageAsync(parsedId, imageUpload);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Musician", "D")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<MusicianDTO>> Delete(int id)
        {
            var parsedId = (ushort)id;
            var musician = await musicianService.FindByIdAsync(parsedId);

            if (IsRegisterLoaded(musician))
                await musicianService.DeleteAsync(parsedId);

            return CustomResponse(musician);
        }
    }
}