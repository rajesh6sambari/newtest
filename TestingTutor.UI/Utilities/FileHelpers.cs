using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TestingTutor.UI.Utilities
{
    public class FileHelpers
    {
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".zip", "application/zip" },
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        public static TestingTutorFile ProcessFormFile(IFormFile formFile,ModelStateDictionary modelState)
        {
            var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));

            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name,
                    $"The file ({fileName}) is empty.");
            }
            else if (formFile.Length > 10485760)
            {
                modelState.AddModelError(formFile.Name,
                    $"The file ({fileName}) exceeds 100 MB.");
            }
            else
            {
                try
                {
                    var file = new TestingTutorFile();
                    using (var streamReader = new StreamReader(formFile.OpenReadStream()))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            streamReader.BaseStream.CopyTo(memoryStream);
                            file.FileName = formFile.FileName;
                            file.FileBytes = memoryStream.ToArray();
                        }
                    }

                    if (file.FileBytes.Length > 0)
                    {
                        return file;
                    }
                    else
                    {
                        modelState.AddModelError(formFile.Name,
                            $"The file ({fileName}) is empty.");
                    }
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(formFile.Name,
                        $"The file ({fileName}) upload failed. " +
                        $"Please contact the Help Desk for support. Error: {ex.Message}");
                    // Log the exception
                }
            }

            return null;
        }
    }
}
