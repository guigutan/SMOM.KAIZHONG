using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;

namespace SIE.Web.Barcodes.WipBatchs.Commands
{
    /// <summary>
    /// 子批次信息补打
    /// </summary>
    public class ReprintSubWipBatchCommand : ListViewCommand
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
                    throw new ValidationException("打印模板的类型必须是生产子批次".L10N());
                }

                rstPrint.Type = template.Type;


                var subWipBatchs = RT.Service.Resolve<WipBatchController>().ReprintSubWipBatch(entity.WipBatchIds);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var printable = new WipBatchPrintable();

                rstPrint.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return subWipBatchs;
                });
            }
            catch (Exception exc)
            {
                rstPrint.ErrMsg = exc.Message;
            }

            return rstPrint;
        }
    }
}
