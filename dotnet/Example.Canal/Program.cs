using CanalSharp.Client.Impl;
using Com.Alibaba.Otter.Canal.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Example.Canal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            //canal 配置的 destination，默认为 example
            var destination = "example";

            //创建一个简单 CanalClient 连接对象（此对象不支持集群）传入参数分别为 canal 地址、端口、destination、用户名、密码
            var connector = CanalConnectors.NewSingleConnector("39.108.11.143", 11111, destination, "", "");
            //连接 Canal
            connector.Connect();

            //订阅，同时传入 Filter。Filter是一种过滤规则，通过该规则的表数据变更才会传递过来
            //允许所有数据 .*\\..*
            //允许某个库数据 库名\\..*
            //允许某些表 库名.表名,库名.表名
            connector.Subscribe(".*\\..*");

            while (true)
            {
                //获取数据 1024表示数据大小 单位为字节
                var message = connector.Get(1024);

                //批次id 可用于回滚
                var batchId = message.Id;
                if (batchId == -1 || message.Entries.Count <= 0)
                {
                    Thread.Sleep(300);
                    continue;
                }

                PrintEntry(message.Entries);
            }
        }

        /// <summary>
        /// 输出数据
        /// </summary>
        /// <param name="entrys">一个entry表示一个数据库变更</param>
        private static void PrintEntry(List<Entry> entrys)
        {
            foreach (var entry in entrys)
            {
                if (entry.EntryType == EntryType.Transactionbegin || entry.EntryType == EntryType.Transactionend)
                {
                    continue;
                }

                RowChange rowChange = null;

                try
                {
                    //获取行变更
                    rowChange = RowChange.Parser.ParseFrom(entry.StoreValue);
                }
                catch (Exception e)
                {
                    //_logger.LogError(e.ToString());
                    Console.WriteLine(e.Message);
                }

                if (rowChange != null)
                {
                    //变更类型 insert/update/delete 等等
                    EventType eventType = rowChange.EventType;

                    //输出binlog信息 表名 数据库名 变更类型
                    //_logger.LogInformation(
                    //    $"================> binlog[{entry.Header.LogfileName}:{entry.Header.LogfileOffset}] , name[{entry.Header.SchemaName},{entry.Header.TableName}] , eventType :{eventType}");

                    Console.WriteLine($"================> binlog[{entry.Header.LogfileName}:{entry.Header.LogfileOffset}] , name[{entry.Header.SchemaName},{entry.Header.TableName}] , eventType :{eventType}");

                    //输出 insert/update/delete 变更类型列数据
                    foreach (var rowData in rowChange.RowDatas)
                    {
                        if (eventType == EventType.Delete)
                        {
                            PrintColumn(rowData.BeforeColumns.ToList());
                        }
                        else if (eventType == EventType.Insert)
                        {
                            PrintColumn(rowData.AfterColumns.ToList());
                        }
                        else
                        {
                            //_logger.LogInformation("-------> before");
                            Console.WriteLine("-------> before");

                            PrintColumn(rowData.BeforeColumns.ToList());

                            //_logger.LogInformation("-------> after");
                            Console.WriteLine("-------> after");

                            PrintColumn(rowData.AfterColumns.ToList());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 输出每个列的详细数据
        /// </summary>
        /// <param name="columns"></param>
        private static void PrintColumn(List<Column> columns)
        {
            var dic = new Dictionary<string, object>();
            foreach (var column in columns)
            {
                dic.Add(column.Name, column.Value);

                //输出列明 列值 是否变更
                Console.WriteLine($"{column.Name} ： {column.Value}  update=  {column.Updated}");
            }

            var json = JsonConvert.SerializeObject(dic);
            Console.WriteLine(json);

            var unit = JsonConvert.DeserializeObject<Unit>(json);
            Console.WriteLine(unit.UnitName);
        }
    }

    /// <summary>
    /// 状态枚举 -1 删除 0 正常 1 禁用
    /// </summary>
    public enum EStatus
    {
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = -1,

        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 禁用
        /// </summary>
        Disable = 1
    }
    /// <summary>
    /// Guid扩展
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// 32位数字(不包含-) 00000000000000000000000000000000
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ToN(this Guid guid)
        {
            return guid.ToString("N");
        }
    }

    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class CommonEntity
    {
        /// <summary>
        /// 标识Id
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToN();

        /// <summary>
        /// 状态
        /// </summary>
        public EStatus Status { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Reviser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ReviseTime { get; set; }
    }

    public class Unit : CommonEntity
    {
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

}
