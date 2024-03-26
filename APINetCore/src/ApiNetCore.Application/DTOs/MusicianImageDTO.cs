using ApiNetCore.Application.DTOs.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore.Application.DTOs
{
    [ModelBinder(BinderType = typeof(MusicianDtoModelBinder))]
    public class MusicianImageDTO : MusicianDTO
    {
    }
}
