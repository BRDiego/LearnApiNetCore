using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Data.EFContext.Repository;

namespace ApiNetCore.Api.Controllers
{
    [Route("api/bands")]
    public class BandsController : MainController
    {
        private readonly IBandService bandService;
        private readonly IMapper mapper;

        public BandsController(IBandService bandService,
                                      IMapper mapper,
                                      IAlertManager alertManager)
                                      : base(alertManager)
        {

            this.mapper = mapper;
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

        //TODO
        //Implement some filters like: Find Bands by MusicalStyle ot use the predicate List
        // [HttpGet]


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
                    AlertValidation("the id in the query doesn't match the object provided");
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