using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Api.CustomExceptions;
using Microsoft.AspNetCore.WebUtilities;

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
        public async Task<ActionResult<IEnumerable<MusicianDTO>>> FilteredList([FromForm] int musicianAge, string surname)
        {
            try
            {
                if (musicianAge > 0 && (musicianAge < 18 || musicianAge > (DateTime.Now.Date.Year - 1920)))
                    throw new InvalidRequestValueException("Invalid age provided for filtering");

                    
                if (!string.IsNullOrWhiteSpace(surname))
                    if (surname.Length > 50)
                        throw new InvalidRequestValueException("The maximum characters amount for Musician Surname is 50");
                else 
                    surname = "";
                
                return CustomResponse(
                    await musicianService.ListAsync(
                        m => 
                        musicianAge > 0 ? m.Age == musicianAge : true
                        &&
                        m.Surnames.Contains(surname)
                ));
            }
            catch (Exception ex)
            {
                AlertException(ex);
                return CustomResponse();
            }
        }


        [HttpGet]
        public async Task<ActionResult<MusicianDTO>> FindByNickname([FromForm]string nickname)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(nickname))
                    if (nickname.Length > 20)
                        throw new InvalidRequestValueException("The maximum characters amount for Musician Name is 20");
                else 
                    nickname = "";
                
                var musician = await musicianService.FindAsync(
                    m => 
                    m.Nickname == nickname
                );

                if (musician is null) return NotFound();

                return CustomResponse(musician);
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
                var musician = await musicianService.FindAsync(parsedId);

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
        public async Task<ActionResult<MusicianDTO>> Excluir(int id)
        {
            var parsedId = (ushort)id;
            var musician = await musicianService.FindAsync(parsedId);

            if (musician is null) return NotFound();

            await musicianService.DeleteAsync(parsedId);

            return CustomResponse(musician);
        }

        [HttpGet("bands/{id:int}")]
        public async Task<ActionResult<MusicianDTO>> GetMusicianBands(int id)
        {
            var parsedId = (ushort)id;
            var musician = await musicianService.GetMusicianBands(parsedId);

            if (musician is null) return NotFound();
            
            return CustomResponse(musician);
        }
    }
}