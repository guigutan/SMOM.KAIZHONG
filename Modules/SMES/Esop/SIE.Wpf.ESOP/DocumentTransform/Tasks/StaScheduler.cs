using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIE.Wpf.ESop.DocumentTransform
{
    /// <summary>
    /// 任务调度
    /// </summary>
    public sealed class StaScheduler : TaskScheduler, IDisposable
    {
        /// <summary>
        /// 线程
        /// </summary>
        private readonly List<Thread> _threads;

        /// <summary>
        /// 线程安全集合
        /// </summary>
        private BlockingCollection<Task> tasks;

        /// <summary>
        /// 初始化任务调度数据
        /// </summary>
        /// <param name="numberOfThreads">线程数量</param>
        public StaScheduler(int numberOfThreads)
        {
            if (numberOfThreads < 1) throw new ArgumentOutOfRangeException("concurrencyLevel");

            tasks = new BlockingCollection<Task>();

            _threads = Enumerable.Range(0, numberOfThreads).Select(i =>
            {
                var thread = new Thread(() =>
                {
                    //// 阻塞循环任务集合
                    foreach (var t in tasks.GetConsumingEnumerable())
                    {
                        TryExecuteTask(t);
                    }
                });
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                return thread;
            }).ToList();

            _threads.ForEach(t => t.Start());
        }

        /// <summary>
        /// 将任务对象排队到调度器
        /// </summary>
        /// <param name="task">任务对象</param>
        protected override void QueueTask(Task task)
        {
            tasks.Add(task);
        }

        /// <summary>
        /// 获取当前调度在该调度器上的任务的枚举。
        /// </summary>
        /// <returns>返回任务调度集合</returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return tasks.ToArray();
        }

        /// <summary>
        /// 在当前线程上执行指定任务的尝试。
        /// </summary>
        /// <param name="task">执行任务</param>
        /// <param name="taskWasPreviouslyQueued">一个布尔值，表示以前已排入任务。 如果此参数为 True，则该任务可能已以前排队 （计划）;如果为 False，则已知该任务无法已排队，以便执行内联任务，而队列不进行此调用它。</param>
        /// <returns>一个布尔值，该值指示该任务以内联方式执行。</returns>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return
                Thread.CurrentThread.GetApartmentState() == ApartmentState.STA &&
                TryExecuteTask(task);
        }

        /// <summary>
        /// 获取此调度器支持的最大并发级别。
        /// </summary>
        public override int MaximumConcurrencyLevel
        {
            get { return _threads.Count; }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (tasks != null)
            {
                tasks.CompleteAdding();
                foreach (var thread in _threads) thread.Join();
                tasks.Dispose();
                tasks = null;
            }
        }
    }
}