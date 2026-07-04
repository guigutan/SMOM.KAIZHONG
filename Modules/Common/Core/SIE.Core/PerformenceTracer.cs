using System;
using System.Diagnostics;

namespace SIE.Core
{
    /// <summary>
    /// 性能追踪
    /// </summary>
    public sealed class PerformenceTracer : IDisposable
    {
        private readonly string message;
        private readonly Stopwatch sw;

        /// <summary>
        /// 开始追踪
        /// </summary>        
        /// <param name="msg">记录信息</param>
        /// <returns></returns>
        public static PerformenceTracer Start(string msg)
        {
            return new PerformenceTracer(msg);
        }

        /// <summary>
        /// 创建PerformenceTrace
        /// </summary>        
        /// <param name="msg">记录信息</param>
        private PerformenceTracer(string msg)
        {
            if (Debugger.IsAttached)
            {
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
                Trace.WriteLine(string.Format(message + " {0} 豪秒".L10N(), sw.ElapsedMilliseconds));
            }
        }
    }
}
