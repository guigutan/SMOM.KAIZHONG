using SIE.Barcodes;
using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Barcodes.Printables;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.Web.Barcodes.Barcodes.ViewModels;
using SIE.Web.Barcodes.Utils;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.Barcodes.Barcodes.DataQuery
{
    /// <summary>
    /// 条码数据查询器
    /// </summary>  
    public class BarcodeDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取条码补打ViewModel，没有则默认生成数据
        /// </summary>
        /// <returns>条码补打ViewModel</returns>
        public ReprintInfo GetReprintInfo(double barcodeId)
        {
            var barcode = RF.GetById<Barcode>(barcodeId);
            var reprintInfo = new ReprintInfo()
            {
                Times = 1,
            };

            reprintInfo.Template = RT.Service.Resolve<BarcodeController>().GetPrintTemplateByWo(barcode.WorkOrder.Id);
            return reprintInfo;
        }

        /// <summary>
        /// 加载条码打印的初始数据
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>条码打印数据</returns>
        public BarcodePrintData GetBarcodePrintData(double workOrderId)
        {
            var barcodeCt = RT.Service.Resolve<BarcodeController>();
            var workOrder = barcodeCt.GetPrintWorkOrder(workOrderId);
            var scrapedQty = barcodeCt.GetScrapBarcodeCount(workOrderId);
            var printedQty = workOrder.PrintedQty;
            int residualQty = (int)workOrder.PlanQty - printedQty;

            var printData = new BarcodePrintData();
            printData.DumpingQty = scrapedQty;
            printData.NumberRuleId = workOrder.Template?.NumberRuleId;
            printData.NumberRuleName = workOrder.Template?.NumberRule?.Name;
            printData.TemplateId = workOrder.Template?.LabelTemplateId;
            printData.TemplateName = workOrder.Template?.LabelTemplate?.FileName;
            StringBuilder sb = new StringBuilder();
            if (workOrder.Template?.NumberRule != null)
            {
                if (workOrder.Template.LabelTemplate != null)
                    printData.TemplateEntityType = workOrder.Template.LabelTemplate.EntityType;
                var numberRule = RF.GetById<NumberRule>(workOrder.Template.NumberRuleId.Value);
                foreach (var ruleDtl in numberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
                printData.BarcodeRuleDtl = sb.ToString();
                if (residualQty > 0)
                {
                    GetPrintData(residualQty, 1, false, workOrder.Template.NumberRuleId.Value, workOrder, printData);
                }
            }

            return printData;
        }

        /// <summary>
        /// 获取条码打印数据
        /// </summary>
        /// <param name="barcodePrintVM">条码打印ViewModel</param>
        /// <returns>条码打印数据</returns>
        public BarcodePrintData GetPrintData(BarcodePrintViewModel barcodePrintVM)
        {
            var printData = new BarcodePrintData();
            var barcodeCt = RT.Service.Resolve<BarcodeController>();
            var workOrder = barcodeCt.GetPrintWorkOrder(barcodePrintVM.WorkOrderId);
            StringBuilder sb = new StringBuilder();
            if (barcodePrintVM.NumberRuleId != null)
            {
                if (barcodePrintVM.Template != null)
                    printData.TemplateEntityType = barcodePrintVM.Template.EntityType;
                var numberRule = RF.GetById<NumberRule>(barcodePrintVM.NumberRuleId.Value);
                foreach (var ruleDtl in numberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
                printData.BarcodeRuleDtl = sb.ToString();
                if (barcodePrintVM.PrintQty > 0 && barcodePrintVM.SingleQty > 0)
                    GetPrintData(barcodePrintVM.PrintQty, barcodePrintVM.SingleQty, barcodePrintVM.PrintControl, barcodePrintVM.NumberRuleId.Value, workOrder, printData);
            }

            return printData;
        }

        /// <summary>
        /// 获取条码打印数据
        /// </summary>
        /// <param name="printQty">可打印数量</param>
        /// <param name="singleQty">单张数量</param>
        /// <param name="printControl">是否反打</param>
        /// <param name="numberRuleId">条码规则Id</param>
        /// <param name="workOrder">工单</param>
        /// <param name="printData">打印数据</param>
        private void GetPrintData(int printQty, int singleQty, bool printControl, double numberRuleId, PrintWorkOrder workOrder, BarcodePrintData printData)
        {
            var ruleCt = RT.Service.Resolve<NumberRuleController>();
            int fullBoxCount = printQty / singleQty;
            var restQty = printQty % singleQty == 0 ? fullBoxCount : fullBoxCount + 1;
            if (!printControl) //默认为正打
            {
                printData.BeginSn = ruleCt.GetStartSegment(numberRuleId, workOrder);
                printData.EndSn = ruleCt.GetEndSegment(numberRuleId, restQty, workOrder);
            }
            else
            {
                printData.EndSn = ruleCt.GetStartSegment(numberRuleId, workOrder);
                printData.BeginSn = ruleCt.GetEndSegment(numberRuleId, restQty, workOrder);
            }
        }

        /// <summary>
        /// 获取打印返回结果
        /// </summary>
        /// <param name="model">条码打印ViewModel</param>
        /// <returns>打印返回结果</returns>
        public RstBarcodePrint Print(BarcodePrintViewModel model)
        {
            var rstPrint = new RstBarcodePrint();
            rstPrint.ErrMsg = string.Empty;
            try
            {
                ValidatePrint(model);
                var info = new PrinterInfo(model.WorkOrderId, model.NumberRuleId.Value, model.TemplateId.Value, model.PrintQty, model.SingleQty, model.PrintedQty);
                var result = RT.Service.Resolve<BarcodeController>().PrintBarcodes(info); // 先保存创建的条码

                if (result.Item1.Length > 0)
                    throw new ValidationException(result.Item1);
                else 
                {
                    return PrintBarcodes(model, result.Item2);
                }
                    
            }
            catch (Exception exc)
            {
                rstPrint.ErrMsg = exc.Message;
            }

            return rstPrint;
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="entity">条码打印视图模型</param>
        /// <param name="barcodes">生成条码列表</param>
        private RstBarcodePrint PrintBarcodes(BarcodePrintViewModel entity, EntityList<Barcode> barcodes)
        {
            var rstPrint = new RstBarcodePrint();
            rstPrint.ErrMsg = string.Empty;
            var template = entity.Template;
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            rstPrint.Type = template.Type;
            var printable = new BarcodePrintable();
            rstPrint.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                if (entity.PrintControl)
                    return barcodes.OrderByDescending(p => p.Id).ToList();
                return barcodes;
            }, (short)entity.PageCount);

            return rstPrint;
        }

        /// <summary>
        /// 验证打印信息
        /// </summary>
        /// <param name="vm">条码打印视图模型</param>
        /// <exception cref="ValidationException">验证失败异常</exception>
        private void ValidatePrint(BarcodePrintViewModel vm)
        {
            var curTemplate = RF.GetById<PrintTemplate>(vm.TemplateId);
            if (curTemplate == null)
                throw new EntityNotFoundException(typeof(PrintTemplate), vm.TemplateId);
            var broken = vm.Validate();
            if (broken.Count > 0)
                throw new ValidationException(broken.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcodeIds"></param>
        /// <param name="reprintInfo"></param>
        /// <returns></returns>
        public RstBarcodePrint Reprint(List<double> barcodeIds, ReprintInfo reprintInfo)
        {
            var rstPrint = new RstBarcodePrint();
            rstPrint.ErrMsg = string.Empty;

            try
            {
                PrintHelper helper = new PrintHelper();
                var barcodeList = RT.Service.Resolve<BarcodeController>().GetBarcodesByIds(barcodeIds);
                var template = RF.GetById<PrintTemplate>(reprintInfo.TemplateId);
                if (template.EntityType != typeof(BarcodePrintable).GetQualifiedName())
                    throw new ValidationException("打印模板错误，请配置【条码】类型的模板！".L10N());
                rstPrint.Type = template.Type;
                rstPrint.Url = helper.PrintBarcodes(barcodeList, template, string.Empty, (short)reprintInfo.Times, () =>
                 {
                     rstPrint.ErrMsg = RT.Service.Resolve<BarcodeController>().ReprintBarcode(barcodeList, BarcodeLogType.Remedy, reprintInfo.Reason, reprintInfo.Times);
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
