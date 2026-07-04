//using DevExpress.XtraPrinting;
//using DevExpress.XtraReports.Security;
//using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using SIE.Common.Prints;

namespace SIE.KZ.Print
{
    /// <summary>
    /// sie报表模板扩展类
    /// </summary>
    public static class SieReportExtension
    {
        /// <summary>
        /// 模板打印（支持客户端调用）
        /// </summary>
        /// <param name="sieReport"><see cref="IReport"/>sie报表模板接口.</param>
        /// <param name="printable"><see cref="Common.Prints.IPrintable"/>实现此接口的一个实例.</param>
        /// <param name="content"><see cref="byte[]"/>模板内容字节</param>
        /// <param name="printerName">打印机名称</param>
        /// <param name="datas">模板数据内容(实体)</param>
        /// <param name="completeCallBack">打印后回调事件</param>
        /// <param name="copies">打印份数(默认1份)</param>
        /// <example>
        /// 调用者示例，参考关键代码如下
        /// <code>
        /// // 1.根据配置项得到对应模板
        // var templateId = config.TemplateId;
        // var template = RF.GetById<PrintTemplate>(templateId);
        // // 1.1模板验证规则
        // if (template.State == State.Disable)
        //    throw new ValidationException("该模板已被禁用，请选择启用的模板".L10N());
        // // 2.根据类型获取报表类型
        // var report = ReportFactory.Current.GetReportByExtension(template.Type);
        // // 3.要打印的数据对象实例
        // var printable = new BarcodePrintable();
        // // 4.根据配置获取的打印机名称
        // var printName = "Adobe PDF";
        // // 5.调用直接打印api
        // report.Print(printable, template.Content, printName, () => { return barcodeList; });
        /// </code>
        /// </example>
        public static void Print(this IHostReport sieReport, SIE.Common.Prints.IPrintable printable, byte[] content, string printerName,
            Func<IEnumerable<object>> datas, Action completeCallBack = null, short copies = 1, int marginLeft = 0, int marginTop = 0, int marginRight = 0, int marginBottom = 0)
        {
            // 入参检查
            Check.NotNull(printable, nameof(printable));
            Check.NotNull(content, nameof(content));
            Check.NotNullOrWhiteSpace(printerName, nameof(printerName));
            Check.NotNull(datas, nameof(datas));

            var errLog = Logging.LogManager.GetLogger("error_logger");
            try
            {
                if (sieReport is SieDevHostReport)
                {
                    new SieDevHostReport().PrintProcess(printable, content, printerName, datas, completeCallBack, copies, marginLeft, marginTop, marginRight, marginBottom);
                }
                else
                {
                    throw new Exception("暂不支持该模板格式");
                }
            }
            catch (Exception ex)
            {
                errLog.Error($"打印异常 Print：{ex}");
                throw new PrintException(ex.Message, ex.GetBaseException());
            }
            
        }


        /// <summary>
        /// 导出模板为图片数据流
        /// </summary>
        /// <param name="sieReport"></param>
        /// <param name="printable"></param>
        /// <param name="content"></param>
        /// <param name="printerName"></param>
        /// <param name="datas"></param>
        /// <param name="resolution"></param>
        /// <param name="marginLeft"></param>
        /// <param name="marginTop"></param>
        /// <param name="marginRight"></param>
        /// <param name="marginBottom"></param>
        /// <returns></returns>
        /// <exception cref="PrintException"></exception>
        public static byte[] ExportToImage(this IHostReport sieReport, SIE.Common.Prints.IPrintable printable, byte[] content, string printerName,
            Func<IEnumerable<object>> datas, int resolution = 96, int marginLeft = 0, int marginTop = 0, int marginRight = 0, int marginBottom = 0)
        {
            // 入参检查
            Check.NotNull(printable, nameof(printable));
            Check.NotNull(content, nameof(content));
            Check.NotNull(datas, nameof(datas));

            var errLog = Logging.LogManager.GetLogger("error_logger");
            
            try
            {
                if (sieReport is SieDevHostReport)
                {
                    return new SieDevHostReport().ExportToImage(printable, content, printerName, datas, resolution, marginLeft, marginTop, marginRight, marginBottom);
                }
                else
                {
                    throw new Exception("暂不支持该模板格式");
                }
            }
            catch (Exception ex)
            {
                errLog.Error($"打印异常 ExportToImage：", ex);
                throw new PrintException(ex.Message, ex.GetBaseException());
            }
        }
    }
}
