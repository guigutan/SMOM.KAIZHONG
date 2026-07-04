namespace SIE.Core.Common
{
    /// <summary>
    /// 性能日志工具类
    /// 正式发布版本将此类所有业务代码注释掉
    /// </summary>
    public class PerformanceLog
    {
        //private string msgFormat ="{0},{1}.{2},{3}, 耗时：{4} \r\n";
        //private string fixedInfo = string.Empty;
        //private StringBuilder msgSb = null;
        //string tmp = string.Empty;
        //private System.Diagnostics.Stopwatch stopwatch = null;
        //private long elapsed = 0;
        //private long lastElapsed = 0;

        /// <summary>
        /// 构造函数，new实例之后马上启动StopWatch开始计时
        /// </summary>
        public PerformanceLog()
        {
            //this.fixedInfo = string.Empty;
            //stopwatch = new System.Diagnostics.Stopwatch();
            //msgSb = new StringBuilder();
            //stopwatch.Start();
        }

        /// <summary>
        /// 构造函数，new实例之后马上启动StopWatch开始计时
        /// </summary>
        /// <param name="_fixedInfo">固定信息</param>
        public PerformanceLog(string _fixedInfo)
        {
            //this.fixedInfo = _fixedInfo;
            //stopwatch = new System.Diagnostics.Stopwatch();
            //msgSb = new StringBuilder();
            //stopwatch.Start();
        }

        /// <summary>
        /// 输出一行日志
        /// </summary>
        /// <param name="msg"></param>
        public void Write(string msg = "")
        {
            //elapsed = stopwatch.ElapsedMilliseconds - lastElapsed;
            //tmp = string.Format(msgFormat, DateTime.Now.ToString("yy-MM-dd dd:mm:ss:fff"), System.Reflection.MethodBase.GetCurrentMethod().Name, fixedInfo, msg, elapsed);
            //msgSb.Append(tmp);
            //System.Diagnostics.Debug.WriteLine(tmp);
            //lastElapsed = stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// 输出全部日志
        /// </summary>
        public void WriteAllAndStop()
        {
            //System.Diagnostics.Debug.WriteLine(msgSb.ToString());
            //StopAndClear();
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            //stopwatch.Start();
        }

        /// <summary>
        /// 停止并重置
        /// </summary>
        public void StopAndClear()
        {
            //stopwatch.Stop();
            //msgSb.Clear();
            //lastElapsed = 0;
            //elapsed = 0;
        }
    }
}
