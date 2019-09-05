using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Example.Bit62
{
    class Program
    {
        public static string EncodeStr(long num)
        {
            int scale = 62;
            StringBuilder sb = new StringBuilder();
            char[] charArray = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            long remainder = 0;

            do
            {
                remainder = num % scale;
                sb.Append(charArray[remainder]);
                num = num / scale;
            }
            while (num > scale - 1);

            sb.Append(charArray[num]);

            char[] chars = sb.ToString().ToCharArray();
            Array.Reverse(chars);
            string result = new string(chars);

            // 6 bits are required, add leading zeros when the encoded string < 6 long
            return result.PadLeft(6, '0');
        }

        public static long DecodeNum(string str)
        {
            int scale = 62;
            string charArray = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Trim the leading zeros first            
            str = Regex.Replace(str, "^0*", "");

            long num = 0;
            int index = 0;
            for (int i = 0; i < str.Length; i++)
            {
                index = charArray.IndexOf(str[i]);
                num += (long)(index * (Math.Pow(scale, str.Length - i - 1)));
            }

            return num;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // 指定长度数值转短码
            var num = 236513265495;

            Console.WriteLine(EncodeStr(num));
            Console.Read();
        }
    }
}
