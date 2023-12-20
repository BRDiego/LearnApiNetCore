using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Api.CustomExceptions;
using Microsoft.AspNetCore.WebUtilities;

namespace ApiNetCore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
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
        public async Task<ActionResult<IEnumerable<BandDTO>>> ListByAge([FromForm] int musicianAge)
        {
            try
            {
                if (musicianAge > 0 && (musicianAge < 18 || musicianAge > (DateTime.Now.Date.Year - 1920)))
                    throw new InvalidRequestValueException("Invalid age provided for filtering");

                var birthYear = DateTime.Now.Year - musicianAge;

                return CustomResponse(
                    await bandService.ListAsync(
                        b => 
                        musicianAge > 0 ? b.Musicians.Any(m => m.DateOfBirth.Year == birthYear) : true
                ));
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }


        [HttpGet]
        public async Task<ActionResult<BandDTO>> FindByName([FromForm]string name)
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


        [HttpGet("{id:int}")]
        public async Task<ActionResult<BandDTO>> FindById(int id)
        {
            try
            {
                var parsedId = (ushort)id;
                var band = await bandService.FindAsync(parsedId);

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
        public async Task<ActionResult<BandDTO>> Excluir(int id)
        {
            var parsedId = (ushort)id;
            var band = await bandService.FindAsync(parsedId);

            if (band is null) return NotFound();

            await bandService.DeleteAsync(parsedId);

            return CustomResponse(band);
        }

        [HttpGet("members/{id:int}")]
        public async Task<ActionResult<BandDTO>> GetBandMembers(int id)
        {
            var parsedId = (ushort)id;
            var band = await bandService.GetBandMembers(parsedId);

            if (band is null) return NotFound();
            
            return CustomResponse(band);
        }
    }
}