using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;

namespace ApiNetCore.Business.Services
{
    public class EntityBaseService<EntityModelType> where EntityModelType : Entity
    {
        private readonly IAlertManager alertManager;
        private readonly IMapper mapper; 

        public EntityBaseService(IAlertManager alertManager, IMapper mapper)
        {
            this.alertManager = alertManager;
            this.mapper = mapper;
        }
        
        protected void Alert(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                Alert(error.ErrorMessage);
        }

        protected void Alert(string message)
        {
            alertManager.Handle(new Alert(message));
        }

        protected bool ExecuteValidation<ValidatorType, EntityType>(ValidatorType validation, EntityType entity) where ValidatorType : AbstractValidator<EntityType> where EntityType : EntityDTO
        {
            var validationResult = validation.Validate(entity);

            if (validationResult.IsValid) return true;

            Alert(validationResult);

            return false;
        }

        protected EntityModelType Map<DtoType>(DtoType entityDTO) where DtoType : EntityDTO
        {
            return mapper.Map<EntityModelType>(entityDTO);
        }
    }
}