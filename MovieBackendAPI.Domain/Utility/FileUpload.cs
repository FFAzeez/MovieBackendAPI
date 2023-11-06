using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MovieBackendAPI.Domain.Const;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBackendAPI.Domain.Utility
{
    public class FileUpload : IFileUpload
    {

        private readonly IHostingEnvironment environment;
        public FileUpload(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task<string> SaveImageAsync(IFormFile Image)
        {
            string ImagePath = string.Empty;
            var uniqueFileName = GetUniqueFileName(Image.FileName);
            var uploads = Path.Combine(environment.ContentRootPath,Configs.Folder);
            var filePath = Path.Combine(uploads, uniqueFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await Image.CopyToAsync(new FileStream(filePath, FileMode.Create));
            ImagePath = filePath;
            return ImagePath;
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return string.Concat(Path.GetFileNameWithoutExtension(fileName)
                                , "_"
                                , Guid.NewGuid().ToString().AsSpan(0, 4)
                                , Path.GetExtension(fileName));

        }


    }
    public interface IFileUpload
    {
        Task<string> SaveImageAsync(IFormFile Image);
    }
}
