using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Example.Picture
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory + "/temp/";

            var drawingFont = new DrawingFont
            {
                OriginPath = dir + "1.jpg",
                PurposePath = dir + "temp.jpg",
                ImageFormat = ImageFormat.Jpeg,
                Fonts = new List<Fonts>() {
                    new Fonts(){
                        Text ="测试新增字体",
                        Font = new Font(FontFamily.GenericSerif,20,FontStyle.Bold),
                        Brush = new SolidBrush(Color.Red),
                        X = 100,
                        Y = 100
                    }
                }
            };

            using (Image image = Image.FromFile(drawingFont.OriginPath))
            {
                Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                Graphics graphics = Graphics.FromImage(bitmap);

                foreach (var item in drawingFont.Fonts)
                {
                    graphics.DrawString(item.Text, item.Font, item.Brush, item.X, item.Y);
                }

                bitmap.Save(drawingFont.PurposePath, drawingFont.ImageFormat);
                bitmap.Dispose();
            }

            Console.WriteLine("Drawing.");
            Console.ReadKey();
        }
    }

    public class DrawingFont
    {
        /// <summary>
        /// 图片源地址
        /// </summary>
        public string OriginPath { get; set; }

        /// <summary>
        /// 新图片地址
        /// </summary>
        public string PurposePath { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        /// <summary>
        /// 新增字体
        /// </summary>
        public IEnumerable<Fonts> Fonts { get; set; }
    }

    public class Fonts
    {
        /// <summary>
        /// 字体
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 字体样式
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Brush字体
        /// </summary>
        public Brush Brush { get; set; }

        /// <summary>
        /// x轴
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// y轴
        /// </summary>
        public float Y { get; set; }
    }
}
