using System.Linq.Expressions;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;

namespace ApiNetCore.Application.Services
{
    public abstract class EntityService<MapSourceDtoType, MapDestinationEntityType> : BaseService, IEntityService<MapSourceDtoType, MapDestinationEntityType> where MapDestinationEntityType : Entity where MapSourceDtoType : EntityDTO, IValidDtoEntity<MapSourceDtoType>
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<MapDestinationEntityType> repository;
        
        protected readonly IBusinessRules businessRules;

        public EntityService(IAlertManager alertManager,
                                 IMapper mapper,
                                 IEntityRepository<MapDestinationEntityType> repository,
                                 IBusinessRules businessRules) : base(alertManager)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.businessRules = businessRules;
        }

        public void Dispose()
        {
            repository?.Dispose();
        }

        public async Task AddAsync(MapSourceDtoType entity)
        {
            if (ObjectIsValid(entity))
            {
                var entityModel = MapToModel(entity);

                if (await PassesDuplicityCheck(entityModel))
                {
                    await repository.AddAsync(entityModel);
                }
                else
                {
                    Alert("Duplicity exists!");
                }
            }
        }

        protected abstract Task<bool> PassesDuplicityCheck(MapDestinationEntityType entityModel);

        public async Task UpdateAsync(MapSourceDtoType entity)
        {
            if (ObjectIsValid(entity))
            {
                await repository.UpdateAsync(MapToModel(entity));
            }
        }

        public async Task DeleteAsync(ushort id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<MapSourceDtoType> FindByIdAsync(ushort id)
        {
            return MapToDto(
                await repository.FindByIdAsync(id)
            );
        }

        public async Task<IEnumerable<MapSourceDtoType>> ListAsync()
        {
            return MapToDto(
                await repository.ListAsync()
            );
        }

        public async Task<IEnumerable<MapSourceDtoType>> ListAsync(Expression<Func<MapDestinationEntityType, bool>> predicate)
        {
            return MapToDto(
                await repository.ListAsync(predicate)
            );
        }

        protected void Alert(string message)
        {
            alertManager.Handle(new Alert(message));
        }

        protected void Alert(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                Alert(error.ErrorMessage);
        }

        protected bool ObjectIsValid(MapSourceDtoType entity)
        {
            return ValidateObject(entity.GetFluentValidator(), entity);
        }

        protected bool ValidateObject<ValidatorType>(ValidatorType validation, MapSourceDtoType entity) where ValidatorType : AbstractValidator<MapSourceDtoType>
        {
            var validationResult = validation.Validate(entity);

            if (validationResult.IsValid) return true;

            Alert(validationResult);

            return false;
        }

        protected MapDestinationEntityType MapToModel(MapSourceDtoType entityDTO)
        {
            return mapper.Map<MapDestinationEntityType>(entityDTO);
        }

        protected MapSourceDtoType MapToDto(MapDestinationEntityType entity)
        {
            return mapper.Map<MapSourceDtoType>(entity);
        }

        protected IEnumerable<MapSourceDtoType> MapToDto(IEnumerable<MapDestinationEntityType> entity)
        {
            return mapper.Map<IEnumerable<MapSourceDtoType>>(entity);
        }
    }

}