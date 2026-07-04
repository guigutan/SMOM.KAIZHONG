using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Common.Prints;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// 批次条码打印
    /// </summary>
    public class PrintHelper
    {
        /// <summary>
        /// 根据工单批次打印规则打印条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workOrderId">工单号</param>
        public void PrintBarcode(string barcode, double workOrderId)
        {
            if (barcode.IsNullOrEmpty())
            {
                throw new ValidationException("条码不能为空".L10N());
            }
            if (workOrderId <= 0)
            {
                throw new ValidationException("工单不能为空".L10N());
            }
            var setting = RT.Service.Resolve<BatchManageController>().GetOrCreateBatchPrintSetting(workOrderId);
            ValidateSetting(setting);
            PrintBarcode(barcode, setting.Printer, setting.PrintTemplate);
        }

        /// <summary>
        /// 打印批次条码
        /// </summary>
        /// <param name="barcode">批次条码</param>
        /// <param name="printer">打印机</param>
        /// <param name="template">打印模板</param>
        public void PrintBarcode(string barcode, string printer, PrintTemplate template)
        {
            if (template == null)
            {
                throw new ValidationException("打印模板为空，请在批次条码设置配置".L10N());
            }
            var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(template.Id);
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatches(new List<string> { barcode });
            report.Print(new WipBatchPrintable(), filePath, printer, () =>
            {
                return wipBatch;
            }, () =>
            {
            });
        }

        /// <summary>
        /// 验证批次打印设置
        /// </summary>
        /// <param name="setting">批次打印设置</param>
        private void ValidateSetting(BatchPrintSetting setting)
        {
            if (!setting.NumberRuleId.HasValue)
            {
                throw new ValidationException("批次打印设置未设置打印模板，请在批次条码设置配置".L10N());
            }
            if (setting.Printer.IsNullOrEmpty())
            {
                throw new ValidationException("批次打印设置未设置打印机，请在批次条码设置配置".L10N());
            }
        }
    }
}