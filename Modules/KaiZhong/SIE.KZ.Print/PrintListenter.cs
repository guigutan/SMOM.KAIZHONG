using SIE.Logging;
using SIE.KZ.Print.Common;
using SIE.Threading;
using System;
using System.Threading.Tasks;

namespace SIE.KZ.Print
{
    /// <summary>
    /// 标签打印 监听器
    /// </summary>
    public class PrintListenter
    {
        private ILog log = Logging.LogManager.GetLogger("print_logger");
        /// <summary>
        /// 单例
        /// </summary>
        public static PrintListenter Instance = new PrintListenter();

        /// <summary>
        /// 启动监听
        /// </summary>
        public void Start()
        {
            var EnablePrint = RT.Config.Get<bool>("pda.enablePrint", false);
            var msg = "打印服务监听: {0}".FormatArgs(EnablePrint);
            System.Console.WriteLine(msg);
            log.Info(msg);

            if (EnablePrint) { 
                RT.RemotingEventBus.Subscribe<WipBatchData>(this, e =>
                {
                    Task.Run(new Action(() =>
                    {
                        RT.Service.Resolve<PrinterController>().PrintDatas(e);
                    }).WithCurrentThreadContext());
                });

                var printers = RT.Service.Resolve<PrinterController>().GetPrinters();
                msg = "可用打印机: \r\n{0}".FormatArgs(string.Join(", \r\n", printers));
                System.Console.WriteLine(msg);
                log.Info(msg);
            }
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop()
        {
            var EnablePrint = RT.Config.Get<bool>("pda.enablePrint", false);
            if (EnablePrint)
                RT.RemotingEventBus.Unsubscribe<WipBatchData>(this);
        }
    }
}