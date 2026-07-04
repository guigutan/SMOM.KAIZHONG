using System;
using System.Diagnostics;

namespace SIE.MES.CollectTest
{
    /// <summary>
    /// 任务池并发模式
    /// </summary>
    public class TaskPoolRunMode : IRunMode
    {
        public string Help => "参数：任务数量，线程数据，默认1000,20";

        public string Name => "任务池并发执行";

        public void Run(string[] args)
        {
            var tasks = 1000;
            var threads = 20;
            if (args.Length > 0)
                tasks = Utils.ToInt(args[0], 1000);
            if (args.Length > 1)
                threads = Utils.ToInt(args[1], 20);

            var task = new CollectTask();
            var barcodes = task.Init(tasks);

            var st = new Stopwatch();
            st.Start();
            var pool = new TaskPool();
            for (int i = 0; i < barcodes.Count; i++)
            {
                pool.Add(ctx =>
                {
                    task.Run(ctx);
                }, new TaskContext(i, barcodes[i]));
            }
            int errors = 0;
            pool.Run(threads, () =>
            {
                st.Stop();
                var msg = $"完成任务数[{tasks}],线程数[{threads}],耗时[{st.ElapsedMilliseconds}]ms,错误数[{errors}]";
                Console.WriteLine(msg);
                Logger.LogInfo(msg);
            }, (ctx, exc) =>
            {
                errors++;
                var context = (TaskContext)ctx;
                context.Stopwatch.Stop();
                var msg = $"错误->ID[{context.Id}],条码[{context.BarCode}]{context.Stopwatch.ElapsedMilliseconds}ms:{exc.Message}";
                Console.WriteLine(msg);
                Logger.LogError(msg);
            });
        }
    }
}
