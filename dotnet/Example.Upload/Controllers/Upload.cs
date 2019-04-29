using Example.Upload.Model;
using Example.Upload.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SDK.Extensions;
using SDK.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Example.Upload.Controllers
{
    [Route("api/[controller]")]
    public class Upload : Controller
    {
        private readonly UploadSettings settings = ConfigurationManager.Configuration.GetSection("upload").Get<UploadSettings>();

        public string Post(IEnumerable<IFormFile> files)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory + settings.Dir;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var result = new List<Result>();

            files.Each(file =>
            {
                var rs = new Result()
                {
                    FileName = file.FileName,
                    Length = file.Length
                };

                try
                {
                    Verify.If(file.Length > settings.Size, $"上传文件大于{settings.Size}字节");

                    var ext = rs.FileName.Substring(rs.FileName.LastIndexOf(".")).ToLower();

                    Verify.If(!settings.Ext.Contains(ext), "上传文件类型不允许");

                    var newFileName = $"{Guid.NewGuid().ToN()}{ext}";
                    var path = dir + newFileName;
                    using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fs);
                    }

                    rs.RemoteUrl = $"{settings.Url}/{newFileName}";
                    rs.Message = "success";
                }
                catch (Exception ex)
                {
                    rs.Message = ex.Message;
                }

                result.Add(rs);
            });

            return JsonConvert.SerializeObject(result);
        }
    }
}
