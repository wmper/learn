using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Upload.Model
{
    public class Result
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 远程url
        /// </summary>
        public string RemoteUrl { get; set; }

        /// <summary>
        /// 文件长度字节
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string Message { get; set; }
    }
}
