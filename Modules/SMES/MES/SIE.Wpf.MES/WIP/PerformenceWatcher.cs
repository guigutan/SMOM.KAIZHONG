using SIE.Logging;
using System;
using System.Diagnostics;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 性能监控，并记录到Logger的Info Level.
    /// </summary>
    public class PerformenceWatcher : IDisposable
    {
        readonly ILog logger;
        readonly string message;
        readonly Stopwatch sw;

        /// <summary>
        /// 开始监控
        /// </summary>
        /// <param name="logger">信息记录到此logger</param>
        /// <param name="msg">记录信息</param>
        /// <returns></returns>
        public static PerformenceWatcher Start(ILog logger, string msg)
        {
            return new PerformenceWatcher(logger, msg);
        }

        /// <summary>
        /// 创建PerformenceWatcher
        /// </summary>
        /// <param name="logger">信息记录到此logger</param>
        /// <param name="msg">记录信息</param>
        public PerformenceWatcher(ILog logger, string msg)
        {
            if (logger != null && logger.IsInfoEnabled)
            {
                this.logger = logger;
                message = msg;
                sw = new Stopwatch();
                sw.Start();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (sw != null)
            {
                sw.Stop();
                logger.Info(message + " {0} ms".FormatArgs(sw.ElapsedMilliseconds));
            }
        }
    }
}
