using SIE.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Pressure
{
    /// <summary>
    /// 时间监听
    /// </summary>
    public class TimeWacther : IDisposable
    {
        DateTime dtStart = new DateTime();
        DateTime dtLastMark = new DateTime();
        StringBuilder sbContent = new StringBuilder();
        string WatchKey = "";
        Hashtable hsStartMark = new Hashtable();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="watchKey"></param>
        public TimeWacther(string watchKey)
        {
            WatchKey = watchKey;
            dtStart = DateTime.Now;
            dtLastMark = DateTime.Now;
        }

        /// <summary>
        /// 开始标记，调用了StartMark的标记，在Mark时，以StartMark记录的时间算耗时
        /// </summary>
        /// <param name="markKey"></param>
        public void StartMark(string markKey)
        {
            hsStartMark[markKey] = DateTime.Now;
        }

        /// <summary>
        /// 标记
        /// </summary>
        /// <param name="markKey"></param>
        /// <param name="markTime"></param>
        public void Mark(string markKey, bool markTime = true)
        {
            Mark(markKey, "", markTime);
        }
        /// <summary>
        /// 标记
        /// </summary>
        /// <param name="markKey"></param>
        /// <param name="info"></param>
        /// <param name="markTime"></param>
        public void Mark(string markKey, string info, bool markTime = true)
        {
            string content = "Key={0},MarkTime={1}{2}";
            string usedTime = "";
            bool hasStartMark = hsStartMark.ContainsKey(markKey);
            if (markTime)
            {

                TimeSpan timeSpan = new TimeSpan();
                if (hasStartMark)
                {
                    //有开始标记的，则以开始标准的时间计算耗时
                    var startTime = (DateTime)hsStartMark[markKey];
                    timeSpan = DateTime.Now - startTime;

                }
                else
                {
                    //没有开始标记的，则以上次Mark的时间计算耗时
                    timeSpan = DateTime.Now - dtLastMark;
                    dtLastMark = DateTime.Now;
                }
                usedTime = ",UsedTime=" + timeSpan.TotalMilliseconds;
            }
            //删除开始标记
            if (hasStartMark)
                hsStartMark.Remove(markKey);

            if (!string.IsNullOrEmpty(info))
                content += "\r\ninfo=>" + info;
            content = string.Format(content, markKey, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), usedTime);
            WriteLog(content);
        }


        /// <summary>
        /// 结束
        /// </summary>
        private void MarkEnd()
        {
            TimeSpan ts = DateTime.Now - dtStart;
            //指定大于XX毫秒才记录,测试时可修改为0
            if (ts.TotalMilliseconds >= 0)
            {
                //结束日志
                WriteLog("", true);
                WriteLog(string.Format("UsedTime:{0}", ts.TotalMilliseconds), true);
                WriteLog(string.Format("EndTime:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")), true);
                WriteLog(string.Format("StartTime:{0}", dtStart.ToString("yyyy-MM-dd HH:mm:ss.ffff")), true);
                WriteLog(string.Format("---------------------------SpanWatcherStart[{0}]---------------------------", WatchKey), true);
                WriteLog("", true);
                WriteLog(string.Format("---------------------------SpanWatcherEnd[{0}]---------------------------", WatchKey));
                WriteLog("");
                //输出日志到文件
                var log = SIE.Logging.LogManager.GetLogger("SpanWatcher");
                log.Info(sbContent.ToString());
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="InsertAtFirst"></param>
        private void WriteLog(string content, bool InsertAtFirst = false)
        {
            if (InsertAtFirst)
                sbContent.Insert(0, content + "\r\n");
            else
                sbContent.AppendLine(content);
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            MarkEnd();
        }
    }
}
