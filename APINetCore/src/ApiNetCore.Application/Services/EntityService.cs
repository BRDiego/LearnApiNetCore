using System.Linq.Expressions;
using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models.Generic;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;

namespace ApiNetCore.Application.Services
{
    public abstract class EntityService<TDto, TEntity> : BaseService, IEntityService<TDto, TEntity> where TEntity : Entity where TDto : EntityDTO, IValidDtoEntity<TDto>
    {
        private readonly IMapper autoMapper;
        private readonly IEntityRepository<TEntity> repository;

        protected readonly IBusinessRules businessRules;

        public EntityService(IAlertManager alertManager,
                                 IEntityRepository<TEntity> repository,
                                 IMapper mapper,
                                 IBusinessRules businessRules) : base(alertManager)
        {
            this.repository = repository;
            this.businessRules = businessRules;
            this.autoMapper = mapper;
        }

        public void Dispose()
        {
            repository?.Dispose();
        }

        public async Task AddAsync(TDto entity)
        {
            if (ObjectIsValid(ref entity))
            {
                var entityModel = MapToEntity(entity);

                await repository.AddAsync(entityModel);
            }
        }

        protected abstract Task<bool> PassesDuplicityCheck(TEntity entityModel);

        public async Task UpdateAsync(TDto entity)
        {
            if (ObjectIsValid(ref entity))
            {
                await repository.UpdateAsync(MapToEntity(entity));
            }
        }

        public async Task DeleteAsync(ushort id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<TDto?> FindByIdAsync(ushort id)
        {
            return MapToDto(
                await repository.FindByIdAsync(id)
            );
        }

        public async Task<IEnumerable<TDto>> ListAsync()
        {
            return MapToDto(
                await repository.ListAsync()
            );
        }

        public async Task<IEnumerable<TDto>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return MapToDto(
                await repository.ListAsync(predicate)
            );
        }

        protected void Alert(string message)
        {
            alertManager.AddAlert(message);
        }

        protected void Alert(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                Alert(error.ErrorMessage);
        }

        protected virtual bool ObjectIsValid(ref TDto entity)
        {
            var result = ValidateObject(entity.GetFluentValidator(), entity);

            if (result)
            {
                var model = MapToEntity(entity);

                Task.Run(async () =>
                {
                    if (!await PassesDuplicityCheck(model))
                    {
                        Alert("Duplicity exists!");
                        result = false;
                    }
                }).Wait();
            }

            return result;                
        }

        protected bool ValidateObject<ValidatorType>(ValidatorType validation, TDto entity) where ValidatorType : AbstractValidator<TDto>
        {
            var validationResult = validation.Validate(entity);

            if (validationResult.IsValid) return true;

            Alert(validationResult);

            return false;
        }

        protected TEntity MapToEntity(TDto entityDTO)
        {
            return autoMapper.Map<TEntity>(entityDTO);
        }

        protected TDto MapToDto(TEntity? entity)
        {
            return autoMapper.Map<TDto>(entity);
        }

        protected IEnumerable<TDto> MapToDto(IEnumerable<TEntity> entity)
        {
            return autoMapper.Map<IEnumerable<TDto>>(entity);
        }
    }

}