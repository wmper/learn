using System.Collections.Generic;

namespace Example.Upload.Model
{
    public class UploadSettings
    {
        /// <summary>
        /// 上传目录
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public IList<string> Ext { get; set; }

        /// <summary>
        /// 大小(字节)
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 远程url
        /// </summary>
        public string Url { get; set; }
    }
}
