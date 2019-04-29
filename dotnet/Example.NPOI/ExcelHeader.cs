using NPOI.SS.UserModel;

namespace Example.NPOI
{
    public class ExcelHeader
    {
        /// <summary>
        /// 标题名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据字段名
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 单元格类型
        /// </summary>
        public CellType CellType { get; set; } = CellType.String;

        /// <summary>
        /// 单元格对齐方式
        /// </summary>
        public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;
    }
}
