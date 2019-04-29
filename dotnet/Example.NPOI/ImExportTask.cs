using SDK.Infrastructure.Model;
using ServiceStack.DataAnnotations;

namespace Example.NPOI
{
    [EnumAsInt]
    public enum ETaskType
    {
        Import = 0,
        Export = 1
    }

    [EnumAsInt]
    public enum EUploadType
    {
        Remote = 0,
        Local = 1
    }

    [EnumAsInt]
    public enum ETaskStatus
    {
        Unknown = 0,
        Success = 1,
        Failure = 2
    }

    public class ImExportTask : CommonEntity
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public ETaskType TaskType { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// excel标题头(json格式)
        /// </summary>
        public string ExcelHeader { get; set; }

        /// <summary>
        /// 数据(json格式)
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 上传类型
        /// </summary>
        public EUploadType UploadType { get; set; }

        /// <summary>
        /// 上传地址
        /// </summary>
        public string UploadFile { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadFile { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public ETaskStatus TaskStatus { get; set; } = ETaskStatus.Unknown;

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; } = 3;
    }
}
