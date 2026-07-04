using System;
using System.Diagnostics;

namespace SIE.Resources
{
    /// <summary>
    /// 追踪帮助类
    /// </summary>
    public static class TraceHelper
    {
        /// <summary>
        /// 输出文字到缓冲区
        /// </summary>
        /// <param name="msg">信息</param>
        public static void TraceMsg(string msg)
        {
            Trace.WriteLine(msg);
        }

        /// <summary>
        /// 输出操作耗时
        /// </summary>
        /// <param name="action">操作</param>
        /// <param name="lastOp">开始时间</param>
        /// <param name="endOp">结束时间</param>
        public static void TraceCostMsg(string action, DateTime lastOp, DateTime endOp)
        {
            Trace.WriteLine(string.Format("操作:{0},开始时间:{1},结束时间:{2},耗时(秒):{3}",
                action, lastOp.ToString(), endOp.ToString(), (endOp - lastOp).TotalSeconds));
        }
    }
}
