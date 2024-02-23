using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;

namespace ApiNetCore.Api.Controllers
{
    
    [Route("api/[controller]/[action]")]
    public class MusiciansController : MainController
    {
        private readonly IMusicianService musicianService;

        public MusiciansController(IMusicianService musicianService,
                                      IAlertManager alertManager)
                                      : base(alertManager)
        {
            this.musicianService = musicianService;
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicianDTO>>> Search([FromForm] int musicianAge, string surname)
        {
            try
            {
                return CustomResponse(await musicianService.SearchAsync(musicianAge, surname));
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicianDTO>>> ListByNickname([FromForm]string nickname)
        {
            try
            {
                return CustomResponse(await musicianService.ListByNicknameAsync(nickname));
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<MusicianDTO>> FindById(int id)
        {
            try
            {
                var parsedId = (ushort)id;
                var musician = await musicianService.FindByIdAsync(parsedId);

                if (musician is null) return NotFound();

                return CustomResponse(musician);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

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

        [HttpPut("{id:int}")]
        public async Task<ActionResult<MusicianDTO>> Update(int id, MusicianDTO musicianDTO)
        {
            try
            {
                if (id != musicianDTO.Id)
                {
                    AlertValidation("the id doesn't match the object provided");
                    return CustomResponse(musicianDTO);
                }

                if (!ModelState.IsValid) return CustomResponse(ModelState);
                await musicianService.UpdateAsync(musicianDTO);

                return CustomResponse(musicianDTO);
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<MusicianDTO>> Delete(int id)
        {
            var parsedId = (ushort)id;
            var musician = await musicianService.FindByIdAsync(parsedId);

            if (musician is null) return NotFound();

            await musicianService.DeleteAsync(parsedId);

            return CustomResponse(musician);
        }

        [HttpGet("bands/{id:int}")]
        public async Task<ActionResult<MusicianDTO>> GetMusicianWithBands(int id)
        {
            var parsedId = (ushort)id;
            var musician = await musicianService.GetMusicianWithBands(parsedId);

            if (musician is null) return NotFound();
            
            return CustomResponse(musician);
        }
    }
}