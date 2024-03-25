using ApiNetCore.Application.CustomExceptions;
using ApiNetCore.Business.AlertsManagement;
using Microsoft.AspNetCore.Http;

namespace ApiNetCore.Application.Procedures.Files
{
    internal class ImageProcedures
    {
        private string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "assets/images");

        protected readonly IAlertManager alertManager;

        private ImageProcedures(IAlertManager alertManager)
        {
            this.alertManager = alertManager;

            Directory.CreateDirectory(imagesFolderPath);
        }

        public static ImageProcedures Create(IAlertManager alertManager)
        {
            return new ImageProcedures(alertManager);
        }

        private void ValidateImage(long fileLength)
        {
            if (FileSizeChecker.Instance.GetKilobytesSize(50) < fileLength)
            {
                alertManager.AddAlert("image surpass the 50kb limit");
                alertManager.CheckAlerts();
            }
        }

        public string SaveFileFromBase64(string file64, string uploadedImageName)
        {
            var fileDataByteArray = Convert.FromBase64String(file64);

            ValidateImage(fileDataByteArray.Length);

            var fileName = GenerateFileName(uploadedImageName);

            var saveFilePath = Path.Combine(imagesFolderPath, fileName);

            File.WriteAllBytes(saveFilePath, fileDataByteArray);

            return fileName;
        }

        internal string SaveFileFromStream(IFormFile imageUpload)
        {
            ValidateImage(imageUpload.Length);

            var fileName = GenerateFileName(imageUpload.FileName);

            var saveFilePath = Path.Combine(imagesFolderPath, fileName);

            using var stream = new FileStream(saveFilePath, FileMode.Create);

            Task.Run(async () =>
            {
                await imageUpload.CopyToAsync(stream);
            }
            ).Wait();

            return fileName;
        }

        private string GenerateFileName(string uploadedImageName)
        {
            var extension = uploadedImageName.Substring(uploadedImageName.LastIndexOf('.'));
            return $"{Guid.NewGuid()}_{DateTime.Now:ddMMyyHHmmss}{extension}";
        }

        internal void DeleteImage(string oldImageFileName)
        {
            if (!string.IsNullOrEmpty(oldImageFileName))
            {
                File.Delete(Path.Combine(imagesFolderPath, oldImageFileName));
            }
        }
    }
}
