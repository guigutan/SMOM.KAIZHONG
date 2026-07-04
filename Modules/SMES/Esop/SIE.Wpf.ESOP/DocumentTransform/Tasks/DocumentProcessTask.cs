using SIE.Security;
using SIE.Threading;
using SIE.Wpf.ESop.DocumentTransform.ProcessDocuments;
using System;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace SIE.Wpf.ESop.DocumentTransform
{
    /// <summary>
    /// 文档处理任务
    /// </summary>
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    public class DocumentProcessTask : IProcessTask, IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        /// <summary>
        /// 日记对象
        /// </summary>
        private static readonly LogWriter Logwriter = HostLogger.Get("DocumentTransformService");

        /// <summary>
        /// 监控文件任务
        /// </summary>
        private Task _monitoringTask;

        /// <summary>
        /// 转换文件任务
        /// </summary>
        private Task _converterTask;

        /// <summary>
        /// 是否运行
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// 用户名
        /// </summary>
        private readonly string _userName;

        /// <summary>
        /// 密码
        /// </summary>
        private readonly string _passWord;

        /// <summary>
        /// 转换文件任务取消资源对象
        /// </summary>
        private readonly CancellationTokenSource _converterTaskCancelTokenSource;

        /// <summary>
        /// 监控文件任务取消资源对象
        /// </summary>
        private readonly CancellationTokenSource _monitoringWaitCancelTokenSource;

        /// <summary>
        /// 文档转换对象
        /// </summary>
        public IDocumentConvert DocumentConverter { get; private set; }

        /// <summary>
        /// 文档处理对象
        /// </summary>
        public IProcessDocument ProcessDocumenter { get; private set; }

        /// <summary>
        /// 初始化任务调度对象
        /// </summary>
        /// <param name="processDocumenter">文档处理对象</param>
        /// <param name="converter">文档转换对象</param>
        public DocumentProcessTask(IProcessDocument processDocumenter, IDocumentConvert converter)
        {
            _converterTaskCancelTokenSource = new CancellationTokenSource();
            _monitoringWaitCancelTokenSource = new CancellationTokenSource();
            _userName = AppRuntime.Config.Get("rbac.userName", string.Empty);
            _passWord = AppRuntime.Config.Get("rbac.passWord", string.Empty);
            DocumentConverter = converter;
            ProcessDocumenter = processDocumenter;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Star()
        {
            Logwriter.Info("DocumentProcessTask Started");
            var service = RT.Service.Resolve<IAuthenticationService>();
            LoginInfo info = new LoginInfo()
            {
                OperatingSystem = "ESOP",
                SystemVersion = string.Empty,
                DeviceName = "ESOP",
                Ram = string.Empty,
                IpAdress = string.Empty,
                Mac = string.Empty,
                SoftVersion = string.Empty,
                Remarks = string.Empty,
                CpuModel = string.Empty,
                SystemType = string.Empty,
                DisplayMetrics = string.Empty,
                Model = string.Empty
            };
            service.Login(_userName, _passWord, info);

            ////监控文件变动
            _monitoringTask = new Task(new Action(Process).WithCurrentThreadContext(), _monitoringWaitCancelTokenSource.Token);
            ////转换
            _converterTask = new Task(new Action(DocumentConvert).WithCurrentThreadContext(), _converterTaskCancelTokenSource.Token);
            _monitoringTask.Start();
            _converterTask.Start(new StaScheduler(1));
            isRunning = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            DocumentConverter.Stop();
            ProcessDocumenter.Stop();
            if (_monitoringWaitCancelTokenSource.IsCancellationRequested)
            {
                return;
            }

            _monitoringWaitCancelTokenSource.Cancel();
            if (_monitoringTask.Status == TaskStatus.Running)
            {
#pragma warning disable S4462 // Calls to "async" methods should not be blocking
                _monitoringTask.Wait();
#pragma warning restore S4462 // Calls to "async" methods should not be blocking
            }

            if (_monitoringTask.IsCanceled || _monitoringTask.IsCompleted || _monitoringTask.IsFaulted)
            {
#pragma warning disable S2952 // Classes should "Dispose" of members from the classes' own "Dispose" methods
                _monitoringTask.Dispose();
#pragma warning restore S2952 // Classes should "Dispose" of members from the classes' own "Dispose" methods
            }

            if (_converterTaskCancelTokenSource.IsCancellationRequested)
            {
                return;
            }

            _converterTaskCancelTokenSource.Cancel();
            if (_converterTask.Status == TaskStatus.Running)
            {
#pragma warning disable S4462 // Calls to "async" methods should not be blocking
                _converterTask.Wait();
#pragma warning restore S4462 // Calls to "async" methods should not be blocking
            }

            if (_converterTask.IsCanceled || _converterTask.IsCompleted || _converterTask.IsFaulted)
#pragma warning disable S2952 // Classes should "Dispose" of members from the classes' own "Dispose" methods
                _converterTask.Dispose();
#pragma warning restore S2952 // Classes should "Dispose" of members from the classes' own "Dispose" methods
            isRunning = false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            // 暂时留空
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            // 暂时留空
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (isRunning)
            {
                Dispose(true);
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            Stop();
        }

        /// <summary>
        /// 文档转换任务
        /// </summary>
        private void DocumentConvert()
        {
            while (!_converterTaskCancelTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    DocumentConverter.Convert();
                }
                catch (Exception e)
                {
                    Logwriter.Error(e);
                }
            }
        }

        /// <summary>
        /// 文档处理任务
        /// </summary>
        private void Process()
        {
            while (!_monitoringWaitCancelTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    ProcessDocumenter.Process();
                }
                catch (Exception e)
                {
                    Logwriter.Error(e);
                }
            }
        }
    }
}
