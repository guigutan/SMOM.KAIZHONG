using SIE.Barcodes.Printables;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using SIE.Web.Common.Prints;
using SIE.Barcodes.WipBatchs;
using SIE.Barcodes.Barcodes.ViewModels;

namespace SIE.Web.Barcodes.WipBatchs.Commands
{
    /// <summary>
    /// 批次信息补打
    /// </summary>
    public class ReprintBatchCommand : ListViewCommand
    {
        /// <summary>
        /// 补打命令执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>补打结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var rstPrint = new RstBarcodePrint();
            rstPrint.ErrMsg = string.Empty;

            try
            {
                var entity = args.Data.ToJsonObject<BatchReprintDataModel>();
                var template = RF.GetById<PrintTemplate>(entity.TemplateId);
                Check.NotNull(template, nameof(PrintTemplate));

                if (template.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板的类型必须是生产批次".L10N());
                }

                rstPrint.Type = template.Type;

                var wipBatchs = RT.Service.Resolve<WipBatchController>().ReprintWipBatch(entity.WipBatchIds);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                
                var printable = new WipBatchPrintable();

                rstPrint.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return wipBatchs;
                });
            }
            catch (Exception exc)
            {
                rstPrint.ErrMsg = exc.Message;
            }

            return rstPrint;
        }
    }

    /// <summary>
    /// 批次补打数据类
    /// </summary>
    public class BatchReprintDataModel
    {
        /// <summary>
        /// 选中条码Id列表
        /// </summary>
        public List<double> WipBatchIds { get; set; }

        /// <summary>
        /// 补打模板Id
        /// </summary>
        public double TemplateId { get; set; }
    }
}
