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
using System.Drawing;
using System.Drawing.Imaging;

namespace Example.Upload.Controllers
{
    [Route("api/[controller]")]
    public class Upload : Controller
    {
        private readonly UploadSettings settings = ConfigurationManager.Configuration.GetSection("upload").Get<UploadSettings>();

        [HttpPost]
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

                    //using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    //{
                    //    file.CopyTo(fs);
                    //}

                    ImageFormat format(string input)
                    {
                        switch (input)
                        {
                            case ".bmp":
                                return ImageFormat.Bmp;
                            case ".emf":
                                return ImageFormat.Emf;
                            case ".exif":
                                return ImageFormat.Exif;
                            case ".gif":
                                return ImageFormat.Gif;
                            case ".icon":
                                return ImageFormat.Icon;
                            case ".jpg":
                            case ".jpeg":
                                return ImageFormat.Jpeg;
                            case ".memorybmp":
                                return ImageFormat.MemoryBmp;
                            case ".png":
                                return ImageFormat.Png;
                            case ".tiff":
                                return ImageFormat.Tiff;
                            case ".wmf":
                                return ImageFormat.Wmf;
                            default:
                                return ImageFormat.Jpeg;
                        }
                    }

                    // 去除 EXIF信息
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);

                        var image = Image.FromStream(stream);
                        Bitmap bitmap = new Bitmap(image);

                        bitmap.Save(path, format(ext));
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
