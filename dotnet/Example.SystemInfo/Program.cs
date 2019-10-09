using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Example.SystemInfo {
    class Program {
        static void Main (string[] args) {
            Console.WriteLine ("Hello World!");

            Process[] p = Process.GetProcesses (); //获取进程信息
                  
            Int64 totalMem = 0;
            string info = "";
            foreach (Process pr in p) {
                totalMem += pr.WorkingSet64 / 1024;
                info += pr.ProcessName + "内存：-----------" + (pr.WorkingSet64 / 1024).ToString () + "KB\r\n"; //得到进程内存
                      
            }
            //Console.WriteLine(info);
            Console.WriteLine ("总内存totalmem:" + totalMem / 1024 + "M");
            Console.WriteLine ("判断是否为Windows Linux OSX");
            Console.WriteLine ($"Linux:{RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}");
            Console.WriteLine ($"OSX:{RuntimeInformation.IsOSPlatform(OSPlatform.OSX)}");
            Console.WriteLine ($"Windows:{RuntimeInformation.IsOSPlatform(OSPlatform.Windows)}");
            Console.WriteLine ($"系统架构：{RuntimeInformation.OSArchitecture}");
            Console.WriteLine ($"系统名称：{RuntimeInformation.OSDescription}");
            Console.WriteLine ($"进程架构：{RuntimeInformation.ProcessArchitecture}");
            Console.WriteLine ($"是否64位操作系统：{Environment.Is64BitOperatingSystem}");
            Console.WriteLine ("CPU CORE:" + Environment.ProcessorCount);
            Console.WriteLine ("HostName:" + Environment.MachineName);
            Console.WriteLine ("Version:" + Environment.OSVersion);

            Console.WriteLine ("内存相关的:" + Environment.WorkingSet);
            string[] LogicalDrives = Environment.GetLogicalDrives ();
            for (int i = 0; i < LogicalDrives.Length; i++) {
                Console.WriteLine ("驱动:" + LogicalDrives[i]);
            }

            ////创建一个ProcessStartInfo对象 使用系统shell 指定命令和参数 设置标准输出
            //var psi = new ProcessStartInfo("top", " -b -n 1") { RedirectStandardOutput = true };
                   ////启动
                   //var proc = Process.Start(psi);

            ////   psi = new ProcessStartInfo("", "1") { RedirectStandardOutput = true };
            ////启动
            //// proc = Process.Start(psi);

            //if (proc == null)
            //{
            //    Console.WriteLine("Can not exec.");
            //}
            //else
            //{
            //    Console.WriteLine("-------------Start read standard output-------cagy-------");
                   //    //开始读取
                   //    using (var sr = proc.StandardOutput)
            //    {
            //        while (!sr.EndOfStream)
            //        {
            //            Console.WriteLine(sr.ReadLine());
            //        }

            //        if (!proc.HasExited)
            //        {
            //            proc.Kill();
            //        }
            //    }
            //    Console.WriteLine("---------------Read end-----------cagy-------");
            //    Console.WriteLine($"Total execute time :{(proc.ExitTime - proc.StartTime).TotalMilliseconds} ms");
            //    Console.WriteLine($"Exited Code ： {proc.ExitCode}");
            //}

            GetCPUUsage.OnStartup ();

            do {
                //var proc = Process.GetCurrentProcess();
                //var mem = proc.WorkingSet64;
                //var cpu = proc.TotalProcessorTime;

                //Console.WriteLine("进程使用内存: {0:n2} M of working set and CPU {1:n} msec",
                //   mem / (1024 * 1024), cpu.TotalMilliseconds);

                Thread.Sleep (1000);

                GetCPUUsage.CallCPU ();

                //var startTime = DateTime.UtcNow;
                //var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

                for (var i = 0; i < 10; i++) {
                    var t = i.ToString ();
                }
                //Thread.Sleep(500);

                //var endTime = DateTime.UtcNow;
                //var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

                //var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                //var totalMsPassed = (endTime - startTime).TotalMilliseconds;

                //var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

                //var useage = cpuUsageTotal * 100;
                //Console.WriteLine($"CPU使用率:{useage}");

                Console.WriteLine (GetCPUInfo.GetInfoMinute ());
                Console.WriteLine (GetCPUInfo.GetInfoTotal ());

            } while (true);

            // Console.Read();
        }
    }

    public class GetCPUUsage {
        static TimeSpan start;

        public static double CPUUsageTotal {
            get;

            private set;
        }

        public static double CPUUsageLastMinute {
            get;

            private set;
        }

        static TimeSpan oldCPUTime = new TimeSpan (0);

        static DateTime lastMonitorTime = DateTime.UtcNow;

        public static DateTime StartTime = DateTime.UtcNow;

        //Call it once everything is ready

        public static void OnStartup () {
            start = Process.GetCurrentProcess ().TotalProcessorTime;
        }

        //Call this every 30 seconds

        public static void CallCPU () {
            TimeSpan newCPUTime = Process.GetCurrentProcess ().TotalProcessorTime - start;

            CPUUsageLastMinute = (newCPUTime - oldCPUTime).TotalSeconds / (Environment.ProcessorCount * DateTime.UtcNow.Subtract (lastMonitorTime).TotalSeconds);

            lastMonitorTime = DateTime.UtcNow;

            CPUUsageTotal = newCPUTime.TotalSeconds / (Environment.ProcessorCount * DateTime.UtcNow.Subtract (StartTime).TotalSeconds);
            oldCPUTime = newCPUTime;
        }
    }

    public class GetCPUInfo {
        public static string GetInfoMinute () {
            return String.Format ("{0:0.0}", GetCPUUsage.CPUUsageLastMinute * 100);
        }

        public static string GetInfoTotal () {
            return String.Format ("{0:0.0}", GetCPUUsage.CPUUsageTotal * 100);
        }
    }
}