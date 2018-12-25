using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MyCommute.Utilities
{
    public class ImageHelper : IImageHelper
    {
        private readonly IHostingEnvironment environment;

        public ImageHelper(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public string UploadImage(IFormFile image, string imageName)
        {
            if (image != null && image.Length > 0 && (image.ContentType == "image/png" || image.ContentType == "image/jpeg"))
            {
                string rootPath = environment.WebRootPath;
                string extension = image.ContentType == "image/png" ? "png" : "jpg";
                string imagePath = $"{rootPath}\\user_files\\images\\{imageName}.{extension}";
                string savedPath = $"~/user_files/images/{imageName}.{extension}";

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyToAsync(stream);
                    return savedPath;
                }
            }

            return string.Empty;
        }
    }
}
