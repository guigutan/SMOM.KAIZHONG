using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace SIE.MES.CollectTest
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface IActionTask
    {
        void Run();

        object Context { get; }
    }
    /// <summary>
    /// 泛型任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionTask<T> : IActionTask
    {
        public Action<T> Action { get; set; }
        public T Context { get; set; }
        object IActionTask.Context { get { return Context; } }

        public void Run()
        {
            Action(Context);
        }
    }
    /// <summary>
    /// 任务池
    /// </summary>
    public class TaskPool
    {
        ConcurrentQueue<IActionTask> Actions { get; } = new ConcurrentQueue<IActionTask>();
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="state"></param>
        public void Add<T>(Action<T> action, T state)
        {
            Actions.Enqueue(new ActionTask<T> { Action = action, Context = state });
        }
        /// <summary>
        /// 使用指定线程数目完成任务列表
        /// </summary>
        /// <param name="threads"></param>
        /// <param name="onComplete"></param>
        /// <param name="onError"></param>
        public void Run(int threads, Action onComplete = null, Action<object, Exception> onError = null)
        {
            var count = Math.Max(1, threads);
            AutoResetEvent autoRest = new AutoResetEvent(false);
            ConcurrentBag<Thread> threadFinished = new ConcurrentBag<Thread>();
            for (int i = 0; i < count; i++)
            {
                var thread = new Thread(x =>
                {
                    while (Actions.TryDequeue(out IActionTask action))
                    {
                        try
                        {
                            action.Run();
                        }
                        catch (Exception exc)
                        {
                            try
                            {
                                onError?.Invoke(action.Context, exc);
                            }
                            catch { }
                        }
                    }
                    threadFinished.Add(Thread.CurrentThread);
                });
                thread.Start(i);
            }
            new Thread(() =>
            {
                while (threadFinished.Count != count)
                {
                    Thread.Sleep(1);
                }
                onComplete?.Invoke();
                threadFinished.ToList().ForEach(p => p.Abort());
                autoRest.Set();
            }).Start();
            autoRest.WaitOne();
        }
    }
}
