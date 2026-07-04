using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Barcodes.WipBatchs.ViewModels;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;

namespace SIE.Web.Barcodes.WipBatchs.Commands
{
    /// <summary>
    /// 生成并打印命令
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.WipBatchs.GenerateAndPrintCommand")]
    public class GenerateAndPrintCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">序列化参数</param>
        /// <param name="scope">类型</param>
        /// <returns>模板BASE6字符串</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var rstPrint = new RstBarcodePrint();
            rstPrint.ErrMsg = string.Empty;

            try
            {
                var model = args.Data.ToJsonObject<BatchGeneratingViewModel>();
                var brokenList = model.Validate();

                if (brokenList.Count > 0)
                {
                    throw new ValidationException(brokenList.ToString(";\r\n", MetaModel.RuleLevel.Error));
                }

                if (model.Template == null)
                {
                    throw new ValidationException("打印模板不能为空".L10N());
                }

                rstPrint.Type = model.Template.Type;

                BatchBarcodeInfo info = new BatchBarcodeInfo
                {
                    WorkOrderId = (double)model.BatchWoId,
                    NumberRuleId = (double)model.NumberRuleId,
                    BatchQty = model.BatchQty,
                    GeneratingQty = model.GenerateingQty,
                    GenerateChild = model.GenerateChildren,
                    ChildNumberRuleId = model.ChildNumberRuleId ?? 0,
                    ChildBatchQty = model.ChildBatchQty
                };

                Tuple<BatchWorkOrder, EntityList<WipBatch>> batchWoAndBarcodes = null;
                batchWoAndBarcodes = RT.Service.Resolve<WipBatchController>().BatchsGenerating(info);
                RT.Service.Resolve<WipBatchController>().SavePrintedBarcodes(batchWoAndBarcodes.Item1, batchWoAndBarcodes.Item2);

                try
                {
                    if (info.GenerateChild)
                    {
                        rstPrint.Url = PrintChildBarcodes(model, batchWoAndBarcodes.Item2);
                    }
                    else
                    {
                        rstPrint.Url = PrintBarcodes(model, batchWoAndBarcodes.Item2);
                    }
                }
                catch (Exception ex)
                {
                    RT.Service.Resolve<WipBatchController>().RemoveBarcodesOnFailedPrint(batchWoAndBarcodes.Item1, batchWoAndBarcodes.Item2);
                    throw new ValidationException(ex.Message);
                }
            }
            catch (Exception exc)
            {
                rstPrint.ErrMsg = exc.Message;
            }

            return rstPrint;
        }

        /// <summary>
        /// 打印批次条码
        /// </summary>
        /// <param name="entity">批次生成ViewModel</param>
        /// <param name="barcodes">生成条码列表</param>
        private string PrintBarcodes(BatchGeneratingViewModel entity, EntityList<WipBatch> barcodes)
        {
            var template = entity.Template;
            if (template.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
            {
                throw new ValidationException("打印模板的类型必须是生产批次".L10N());
            }

            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            var printable = new WipBatchPrintable();

            return report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return barcodes;
            });
        }

        /// <summary>
        /// 打印子批次条码
        /// </summary>
        /// <param name="entity">批次生成ViewModel</param>
        /// <param name="barcodes">生成条码列表</param>
        private string PrintChildBarcodes(BatchGeneratingViewModel entity, EntityList<WipBatch> barcodes)
        {
            var template = entity.Template;
            if (template.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
            {
                throw new ValidationException("生成子批次时，打印模板的类型必须是生产子批次".L10N());
            }

            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            var printable = new WipBatchPrintable();

            return report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return barcodes;
            });
        }
    }
}
