using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUSINESS.Concrete
{
    public static class ImageSaver
    {
        private static Account account = new Account(
               "df70ifqzq",
               "991312396234646",
                "sgSBwEpi-68kJQQG5jHsx4bnrHA");

        private static Cloudinary _cloudinary = new Cloudinary(account);

        public static async Task<string> SaveImage(IFormFile image)
        {
            if (image == null)
                return null;

            var filePath = Path.GetTempFileName();//gecici dosya yolu oluşturur ve yolunu verir.

            using (var stream = File.Create(filePath))
            {
                await image.CopyToAsync(stream);//gelen image'i gecici açtığı dosyanın stream'ine kopyalıyor.
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath),//dosya yolunu veriyor
                PublicId = image.FileName//dosya ismi
            };

            var uploadResult = _cloudinary.Upload(uploadParams);//resmi yükler
            return uploadResult.Url.ToString();//resmin sonucunda json içindeki url'i çeker.
        }

    }
}
