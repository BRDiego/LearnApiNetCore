using ApiNetCore.Application.DTOs.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore.Application.DTOs.Models
{
    [ModelBinder(BinderType = typeof(MusicianDtoModelBinder))]
    public class MusicianImageDTO : MusicianDTO
    {
    }
}
