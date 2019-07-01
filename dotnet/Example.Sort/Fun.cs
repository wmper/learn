using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Sort
{
    public class Fun
    {
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="arr"></param>
        public static int[] Sort(int[] arr)
        {
            // 3, 6, 9, 1, 4, 0, 7, 2, 5, 8 
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - 1 - i; j++)
                {
                    int temp = 0;
                    if (arr[j] > arr[j + 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }

            return arr;
        }

        public static int[] SelectSort(int[] arr)
        {
            // 3, 6, 9, 1, 4, 0, 7, 2, 5, 8 
            for (int i = 0; i < arr.Length; i++)
            {
                int min = i;
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[j] < arr[min])
                    {
                        min = j;
                    }
                }

                int temp = arr[i];
                arr[i] = arr[min];
                arr[min] = temp;
            }

            return arr;
        }

        public static int[] InsertSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; ++i)
            {
                int value = arr[i];
                int j = 0;
                for (j = i - 1; j >= 0; j--)
                {
                    if (arr[j] > value)
                    {
                        arr[j + 1] = arr[j];
                    }
                    else
                    {
                        break;
                    }
                }
                arr[j + 1] = value;
            }

            return arr;
        }
    }
}
