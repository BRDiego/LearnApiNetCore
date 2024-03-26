using Microsoft.AspNetCore.Http;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IEntityImageService
    {
        Task UpdateImageAsync(ushort id, IFormFile imageUpload);
    }
}
