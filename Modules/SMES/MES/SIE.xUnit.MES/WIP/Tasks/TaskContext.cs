using System.Diagnostics;

namespace SIE.xUnit.MES.WIP.Tasks
{
    /// <summary>
    /// 任务上下文
    /// </summary>
    public class TaskContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="barcode"></param>
        public TaskContext(int id, string barcode)
        {
            Id = id;
            BarCode = barcode;
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// 采集条码
        /// </summary>
        public string BarCode { get; }
        /// <summary>
        /// 秒表
        /// </summary>
        public Stopwatch Stopwatch { get; } = new Stopwatch();
    }
}
