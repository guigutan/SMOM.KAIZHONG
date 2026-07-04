using System;
using System.IO;

namespace SIE.MES.CollectTest
{
    /// <summary>
    /// 日志
    /// </summary>
    public class Logger
    {
        public static void LogError(string message)
        {
            using (var sw = new StreamWriter("error.log", true))
                sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},{message}");
        }
        public static void LogInfo(string message)
        {
            using (var sw = new StreamWriter("info.log", true))
                sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},{message}");
        }
    }
}
