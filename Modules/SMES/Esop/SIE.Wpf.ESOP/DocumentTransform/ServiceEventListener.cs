using SIE.Event.Bus;
using SIE.Wpf.ESop.DocumentTransform.ProcessDocuments;
using Topshelf.Logging;

namespace SIE.Wpf.ESop.DocumentTransform
{
    /// <summary>
    /// 服务订阅
    /// </summary>
    public class ServiceEventListener
    {
        /// <summary>
        /// 日记对象
        /// </summary>
        private static readonly LogWriter Logwriter = HostLogger.Get("DocumentTransformService");

        /// <summary>
        /// 服务实例
        /// </summary>
        public static readonly ServiceEventListener Instance = new ServiceEventListener();

        /// <summary>
        /// 文档处理任务对象
        /// </summary>
        private readonly IProcessTask Task = new DocumentProcessTask(new ProcessExcelDocumenter(), DocumentCollectConverter.Instance);

        /// <summary>
        /// 注册开始服务
        /// </summary>
        public void Start()
        {
            Logwriter.Info("Subscribe ServiceStartEvent");
            RT.EventBus.Subscribe<ServiceStartEvent>(this, e =>
            {
                Task.Star();
            });
        }

        /// <summary>
        /// 注册停止服务
        /// </summary>
        public void Stop()
        {
            Logwriter.Info("Subscribe ServiceStopEvent");
            RT.EventBus.Subscribe<ServiceStopEvent>(this, e =>
            {
                Task.Stop();
            });
        }

        /// <summary>
        /// 注册暂停服务
        /// </summary>
        public void Pause()
        {
            Logwriter.Info("Subscribe ServicePauseEvent");
            RT.EventBus.Subscribe<ServicePauseEvent>(this, e =>
            {
                Task.Pause();
            });
        }

        /// <summary>
        /// 注册恢复服务
        /// </summary>
        public void Resume()
        {
            Logwriter.Info("Subscribe ServiceResumeEvent");
            RT.EventBus.Subscribe<ServiceResumeEvent>(this, e =>
            {
                Task.Continue();
            });
        }
    }
}
