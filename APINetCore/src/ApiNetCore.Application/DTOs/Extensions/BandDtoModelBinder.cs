using ApiNetCore.Application.DTOs.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace ApiNetCore.Application.DTOs.Extensions
{
    public class BandDtoModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                if (bindingContext is null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }

                var serializeOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    PropertyNameCaseInsensitive = true
                };

                var band = JsonSerializer.Deserialize<BandImageDTO>(bindingContext!.ValueProvider.GetValue("bandDto").FirstOrDefault()!, serializeOptions);
                band!.ImageUploadStream = bindingContext.ActionContext.HttpContext.Request.Form.Files.FirstOrDefault();

                bindingContext.Result = ModelBindingResult.Success(band);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }
    }
}
