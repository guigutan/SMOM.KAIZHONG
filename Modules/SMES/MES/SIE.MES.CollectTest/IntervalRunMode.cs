using System;
using System.Threading;
using SIE.Logging;

namespace SIE.MES.CollectTest
{
    /// <summary>
    /// 固定间隔执行模式
    /// </summary>
    public class IntervalRunMode : IRunMode
    {
        public string Help => "参数：任务数量，时间间隔(毫秒)，默认1000,200";

        public string Name => "固定间隔执行";

        ILog logger = Logging.LogManager.GetLogger("wip");

        public void Run(string[] args)
        {
            var tasks = 1000;
            var interval = 200;
            if (args.Length > 0)
                tasks = Utils.ToInt(args[0], 1000);
            if (args.Length > 1)
                interval = Utils.ToInt(args[1], 200);

            var task = new CollectTask();
            var barcodes = task.Init(tasks);

            for (int i = 0; i < barcodes.Count; i++)
            {
                new Thread(x =>
                {
                    TaskContext context = (TaskContext)x;
                    try
                    {
                        task.Run(context);
                    }
                    catch (Exception exc)
                    {
                        context.Stopwatch.Stop();
                        var msg = $"错误->ID[{context.Id}],条码[{context.BarCode}]{context.Stopwatch.ElapsedMilliseconds}ms:{exc.Message}";
                        Console.WriteLine(msg);
                        logger.Info(msg);
                        // Logger.LogError(msg);
                    }
                    Thread.CurrentThread.Abort();
                }).Start(new TaskContext(i, barcodes[i]));
                Thread.Sleep(interval);
            }
        }
    }
}
