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
    [Route("api/v{version:apiVersion}/bands")]
    //[DisableCors]
    public class BandsController : MainController
    {
        private readonly IBandService bandService;
        private readonly IBandMembersService bandMembersService;

        public BandsController(IBandService bandService,
                                 IAlertManager alertManager,
                                 IBandMembersService bandMembersService,
                                 IUser user)
                                : base(alertManager, user)
        {
            this.bandService = bandService;
            this.bandMembersService = bandMembersService;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<BandDTO>>> List()
        {
            try
            {
                return CustomResponse(await bandService.ListAsync());
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        //[EnableCors("Development")]
        [HttpGet("list-by-musician-age")]
        public async Task<ActionResult<IEnumerable<BandMembersDTO>>> ListByMusiciansAge([FromForm] int? minimumMusicianAge, [FromForm] int? maximumMusicianAge)
        {
            try
            {
                return CustomResponse(await bandMembersService.ListByMusiciansAgeAsync(minimumMusicianAge, maximumMusicianAge));
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Band", "R")]
        [HttpGet("find-by-name")]
        public async Task<ActionResult<BandDTO>> FindByName([FromForm] string? name)
        {
            try
            {
                return CustomResponse(await bandService.FindByNameAsync(name));
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }


        [ClaimsAuthorization("Band", "R")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BandDTO>> FindById(int id)
        {
            try
            {
                var parsedId = (ushort)id;
                var band = await bandService.FindByIdAsync(parsedId);

                return CustomResponse(band);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Band", "R")]
        [HttpGet("{id:int}/members")]
        public async Task<ActionResult<BandMembersDTO>> GetBandWithMembers(int id)
        {
            var parsedId = (ushort)id;
            var band = await bandMembersService.GetBandWithMembers(parsedId);

            return CustomResponse(band);
        }

        [ClaimsAuthorization("Band", "C")]
        [HttpPost]
        public async Task<ActionResult<BandDTO>> Create(BandDTO bandDTO)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                await bandService.AddAsync(bandDTO);

                return CustomResponse(bandDTO);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Band", "C")]
        [HttpPost("create-with-image")]
        public async Task<ActionResult<BandDTO>> CreateWithImage(
            [ModelBinder(BinderType = typeof(BandDtoModelBinder))] BandImageDTO bandDtoWithImage)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                await bandService.AddAsync(bandDtoWithImage);

                return CustomResponse(bandDtoWithImage);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Band", "U")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<BandDTO>> Update(int id, BandDTO bandDTO)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                if (id != bandDTO.Id)
                {
                    AlertValidation("the id doesn't match the object provided");
                    return CustomResponse(bandDTO);
                }

                await bandService.UpdateAsync(bandDTO);

                return CustomResponse(bandDTO);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Band", "U")]
        [HttpPut("{id:int}/updateImage")]
        public async Task<ActionResult<BandDTO>> UpdateImage(int id, [FromForm] IFormFile imageUpload)
        {
            try
            {
                var parsedId = (ushort)id;
                await bandService.UpdateImageAsync(parsedId, imageUpload);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [ClaimsAuthorization("Band", "D")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BandDTO>> Delete(int id)
        {
            var parsedId = (ushort)id;
            var band = await bandService.FindByIdAsync(parsedId);

            if (IsRegisterLoaded(band))
                await bandService.DeleteAsync(parsedId);

            return CustomResponse(band);
        }
    }
}