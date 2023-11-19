using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Api.CustomExceptions;
using Microsoft.AspNetCore.WebUtilities;

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

        [HttpGet]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BandDTO>>> List([FromForm] ushort musicianAge)
        {
            try
            {
                if (musicianAge > 0 && (musicianAge < 18 || musicianAge > (DateTime.Now.Date.Year - 1920)))
                    throw new InvalidRequestValueException("Invalid age provided for filtering");
             
                return CustomResponse(
                    await bandService.ListAsync(
                        b => 
                        musicianAge > 0 ? b.Musicians.Any(m => m.Age == musicianAge) : true
                ));
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }


        [HttpGet]
        public async Task<ActionResult<BandDTO>> Find([FromForm]string name)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(name))
                    if (name.Length > 20)
                        throw new InvalidRequestValueException("The maximum characters amount for Band Name is 20");
                else 
                    name = "";
                
                var band = await bandService.FindAsync(
                    b => 
                    b.MusicalStyles.Contains(name)
                    // musicalStyle == "" ? true : b.MusicalStyles.Contains(musicalStyle)
                );

                if (band is null) return NotFound();

                return CustomResponse(band);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }


        [HttpGet("{id:ushort}")]
        public async Task<ActionResult<BandDTO>> Find(ushort id)
        {
            try
            {
                var band = await bandService.FindAsync(id);

                if (band is null) return NotFound();

                return CustomResponse(band);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [HttpPost]
        public async Task<ActionResult<BandDTO>> Create(BandDTO bandDTO)
        {
            try
            {
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

        [HttpPut("{id:ushort}")]
        public async Task<ActionResult<BandDTO>> Update(ushort id, BandDTO bandDTO)
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

        [HttpDelete("{id:ushort}")]
        public async Task<ActionResult<BandDTO>> Excluir(ushort id)
        {
            var band = await bandService.FindAsync(id);

            if (band is null) return NotFound();

            await bandService.DeleteAsync(id);

            return CustomResponse(band);
        }

        [HttpGet("members/{id:ushort}")]
        public async Task<ActionResult<BandDTO>> GetBandMembers(ushort id)
        {
            var band = await bandService.GetBandMembers(id);

            if (band is null) return NotFound();
            
            return CustomResponse(band);
        }
    }
}