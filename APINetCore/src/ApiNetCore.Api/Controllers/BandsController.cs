using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;

namespace ApiNetCore.Api.Controllers
{
    [Route("api/bands")]
    public class BandsController : MainController
    {
        private readonly IBandService bandService;

        public BandsController(IBandService bandService,
                                      IAlertManager alertManager)
                                      : base(alertManager)
        {
            this.bandService = bandService;
        }

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

        [HttpGet("list-by-musician-age")]
        public async Task<ActionResult<IEnumerable<BandDTO>>> ListByMusiciansAge([FromForm] int? minimumMusicianAge, [FromForm] int? maximumMusicianAge)
        {
            try
            {
                return CustomResponse(await bandService.ListByMusiciansAgeAsync(minimumMusicianAge, maximumMusicianAge));
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }


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


        [HttpGet("{id:int}")]
        public async Task<ActionResult<BandDTO>> FindById(int id)
        {
            try
            {
                var parsedId = (ushort)id;
                var band = await bandService.FindByIdAsync(parsedId);

                IsRegisterLoaded(band);

                return CustomResponse(band);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [HttpGet("{id:int}/members")]
        public async Task<ActionResult<BandDTO>> GetBandWithMembers(int id)
        {
            var parsedId = (ushort)id;
            var band = await bandService.GetBandWithMembers(parsedId);
            
            IsRegisterLoaded(band);

            return CustomResponse(band);
        }

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

        [HttpPut("{id:int}")]
        public async Task<ActionResult<BandDTO>> Update(int id, BandDTO bandDTO)
        {
            try
            {
                if (id != bandDTO.Id)
                {
                    AlertValidation("the id doesn't match the object provided");
                    return CustomResponse(bandDTO);
                }

                if (!ModelState.IsValid) return CustomResponse(ModelState);
                await bandService.UpdateAsync(bandDTO);

                return CustomResponse(bandDTO);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

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