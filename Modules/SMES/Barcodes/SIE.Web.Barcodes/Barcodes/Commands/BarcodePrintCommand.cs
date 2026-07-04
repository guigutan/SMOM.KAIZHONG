using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Security;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using SIE.Web.Json;
using System;
using System.Text;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 单体打印按钮
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.BarcodePrintCommand")]
    public class BarcodePrintCommand : ListViewCommand //ViewCommand
    {
        /// <summary>
        /// 执行的方法
        /// </summary>
        /// <param name="args">序列化参数</param>
        /// <param name="scope">类型</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var obj = args.Data.ToJsonObject<PrintTemplate>();
            return obj;
        }
    }

    /// <summary>
    /// 获取报废数量的方法
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.GetRuleAndScrapedQtyCommand")]
    public class GetRuleAndScrapedQtyCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">序列化参数</param>
        /// <param name="scope">类型</param>
        /// <returns>报废数量</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            int scrapedQty = -1;
            var data = args.Data.ToJsonObject<BarcodePrintData>();
            EntityJson json = new EntityJson();
            if (data != null && data.WorkOrderId > 0)
            {
                var wo = RF.GetById<WorkOrder>(data.WorkOrderId);
                if (wo != null)
                {
                    json.SetProperty("NumberRuleId", wo.Template?.NumberRuleId);
                    json.SetProperty("NumberRuleName", wo.Template?.NumberRule?.Name);
                    json.SetProperty("TemplateId", wo.Template?.LabelTemplateId);
                    json.SetProperty("TemplateName", wo.Template?.LabelTemplate?.FileName);
                }

                var controller = RT.Service.Resolve<BarcodeController>();
                scrapedQty = controller.GetScrapBarcodeCount(data.WorkOrderId);
                json.SetProperty("ScrapedQty", scrapedQty);
            }
            return json;
        }
    }

    /// <summary>
    /// 获取条码打印的起始条码、结束条码
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.GetBeginSnEndSnCommand")]
    public class GetBeginSnEndSnCommand : ViewCommand
    {
        /// <summary>
        /// 执行的方法
        /// </summary>
        /// <param name="args">序列化参数</param>
        /// <param name="scope">类型</param>
        /// <returns>BarcodePrintData类型的对象</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var barcodePrintData = args.Data.ToJsonObject<BarcodePrintData>();
            if (barcodePrintData != null && barcodePrintData.NumberRuleId > 0 && barcodePrintData.WorkOrderId > 0
                && barcodePrintData.PrintQty > 0 && barcodePrintData.SingleQty > 0)
            {
                var workOrder = RF.GetById<PrintWorkOrder>(barcodePrintData.WorkOrderId);
                NumberRuleController controller = RT.Service.Resolve<NumberRuleController>();
                int fullBoxCount = barcodePrintData.PrintQty / barcodePrintData.SingleQty;
                var printQty = barcodePrintData.PrintQty % barcodePrintData.SingleQty == 0 ? fullBoxCount : fullBoxCount + 1;
                if (!barcodePrintData.PrintControl) ////默认为正打
                {
                    barcodePrintData.BeginSn = controller.GetStartSegment(barcodePrintData.NumberRuleId.Value, workOrder);
                    barcodePrintData.EndSn = controller.GetEndSegment(barcodePrintData.NumberRuleId.Value, printQty, workOrder);
                }
                else ////反打
                {
                    barcodePrintData.EndSn = controller.GetStartSegment(barcodePrintData.NumberRuleId.Value, workOrder);
                    barcodePrintData.BeginSn = controller.GetEndSegment(barcodePrintData.NumberRuleId.Value, printQty, workOrder);
                }
            }

            return barcodePrintData;
        }
    }

    /// <summary>
    /// 获取条码打印的条码规则明细
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.GetBarcodeRuleDtlCommand")]
    public class GetBarcodeRuleDtlCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">序列化参数</param>
        /// <param name="scope">类型</param>
        /// <returns>条码规则明细</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            StringBuilder sb = new StringBuilder();
            var barcodePrintData = args.Data.ToJsonObject<BarcodePrintData>();
            if (barcodePrintData != null && barcodePrintData.NumberRuleId > 0)
            {
                var numberRule = RF.GetById<NumberRule>(barcodePrintData.NumberRuleId);
                if (numberRule != null)
                {
                    foreach (var ruleDtl in numberRule.DetailList)
                    {
                        sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                    }
                }
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// 条码打印
    /// </summary>
    [AllowAnonymous]
    [JsCommand("SIE.Web.Barcodes.PrintCommand")]
    public class PrintCommand : DetailViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">序列化参数</param>
        /// <param name="scope">类型</param>
        /// <returns>模板BASE6字符串</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            try
            {
                var model = args.Data.ToJsonObject<BarcodePrintViewModel>();
                ValidatePrint(model);
                var info = new PrinterInfo(model.WorkOrderId, model.NumberRuleId.Value, model.TemplateId.Value, model.PrintQty, model.SingleQty, model.PrintedQty);
                var result = RT.Service.Resolve<BarcodeController>().PrintBarcodes(info); // 先保存创建的条码
                return result.Item1.Length > 0 ? throw new ValidationException(result.Item1) : PrintBarcodes(model, result.Item2);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
                throw new ValidationException(errMsg);
            }
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="entity">条码打印视图模型</param>
        /// <param name="barcodes">生成条码列表</param>
        private object PrintBarcodes(BarcodePrintViewModel entity, EntityList<Barcode> barcodes)
        {
            var template = entity.Template;
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            var printable = new BarcodePrintable();
            return report.PrintProcess(printable, template.Id, template.Content, () =>
             {
                 return barcodes;
             });
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
    }

    /// <summary>
    /// 条码打印序列化类
    /// 用于"Web页面数据刷新"
    /// </summary>
    [Serializable]
    public class BarcodePrintData
    {
        /// <summary>
        /// 条码规则Id
        /// </summary>
        public double? NumberRuleId { get; set; }

        /// <summary>
        /// 条码规则名称
        /// </summary>

        public string NumberRuleName { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>

        public string TemplateName { get; set; }

        /// <summary>
        /// 模板实体类型
        /// </summary>
        public string TemplateEntityType { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 打印控制(反打)
        /// </summary>
        public bool PrintControl { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintQty { get; set; }

        /// <summary>
        /// 单张数量
        /// </summary>
        public int SingleQty { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public int DumpingQty { get; set; }

        /// <summary>
        /// 起始条码
        /// </summary>
        public string BeginSn { get; set; }

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn { get; set; }

        /// <summary>
        /// 条码规则明细
        /// </summary>
        public string BarcodeRuleDtl { get; set; }
    }
}
