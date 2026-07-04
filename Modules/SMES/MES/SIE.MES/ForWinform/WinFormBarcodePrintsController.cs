using SIE.Api;
using SIE.Barcodes;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Items;
using SIE.Domain;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.ForWinform
{
    /// <summary>
    /// 条码打印-客户端
    /// </summary>
    public class WinFormBarcodePrintsController : BarcodeController
    {
        /// <summary>
        /// 获取条码打印的工单
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ApiService("获取条码打印的工单")]
        [return: ApiReturn("工单信息")]
        public virtual Tuple<int, List<WorkOrder>> GetPrintWorkOrders([ApiParameter("工单号")] string key, [ApiParameter("页码")] int pageNum = 1, [ApiParameter("每页行数")] int pageSize = 20)
        {

            PrintWorkOrderCriteria criteria = new PrintWorkOrderCriteria();
            criteria.No = key;
            criteria.PagingInfo = new PagingInfo() { IsNeedCount = true, PageNumber = pageNum, PageSize = pageSize };

            var query = Query<WorkOrder>().Where(p => p.State != Core.WorkOrders.WorkOrderState.CancelRelease);
            if (!criteria.No.IsNullOrWhiteSpace())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            query.Join<ItemBatchRule>((x, y) => x.ProductId == y.ItemId && y.RetrospectType == Core.Items.RetrospectType.Single);
            query.Where(p => !p.IsPanelWorkOrder);
            var list = query.Distinct().ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return new Tuple<int, List<WorkOrder>>(list.TotalCount, list.ToList());
        }

        /// <summary>
        /// 获取工单的条码明细
        /// </summary>
        /// <param name="workorderId"></param>
        /// <returns></returns>

        [ApiService("获取工单的条码打印明细")]
        [return: ApiReturn("条码明细")]
        public virtual List<Barcode> GetBarcodes([ApiParameter("工单Id")] double workorderId)
        {
            return RT.Service.Resolve<BarcodeController>().GetBarcodes(workorderId).ToList();
        }
        /// <summary>
        /// 获取工单记录打印模板
        /// </summary>
        /// <returns></returns>
        [ApiService("获取条码打印模板")]
        [return: ApiReturn("获取条码打印模板 不分页")]
        public virtual List<PrintTemplate> GetPrintTemplates()
        {
            var labelPrintName = typeof(SIE.Barcodes.Printables.BarcodePrintable).GetQualifiedName();
            return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(labelPrintName, null, "").ToList();
        }

        /// <summary>
        /// 获取条码打印数据
        /// </summary>
        /// <param name="barcodePrintVM">条码打印ViewModel</param>
        /// <returns>条码打印数据</returns>

        [ApiService("获取条码打印数据")]
        [return: ApiReturn("条码打印数据")]
        public virtual BarcodePrintData GetPrintData([ApiParameter("打印模型数据")] XPBarcodePrintViewModel barcodePrintVM)
        {
            var printData = new BarcodePrintData();
            var barcodeCt = RT.Service.Resolve<BarcodeController>();
            var workOrder = barcodeCt.GetPrintWorkOrder(barcodePrintVM.WorkOrderId);
            StringBuilder sb = new StringBuilder();
            if (barcodePrintVM.NumberRuleId != null)
            {
                if (barcodePrintVM.TemplateId.HasValue)
                {
                    barcodePrintVM.Template = RF.GetById<PrintTemplate>(barcodePrintVM.TemplateId);
                    printData.TemplateEntityType = barcodePrintVM.Template.EntityType;
                    printData.TemplateName = barcodePrintVM.Template.FileName;
                }
                var numberRule = RF.GetById<NumberRule>(barcodePrintVM.NumberRuleId.Value);
                foreach (var ruleDtl in numberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
                printData.NumberRuleName = numberRule.Name;
                printData.PrintQty = barcodePrintVM.PrintQty;
                printData.ResidualQty = workOrder.PlanQty - workOrder.PrintedQty;
                printData.PrintedQty = workOrder.PrintedQty;
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
        /// 获取条码规则
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [ApiService("获取条码规则")]
        [return: ApiReturn("条码规则集合")]
        public virtual EntityList<NumberRule> GetNumberRules([ApiParameter("搜索关键词")] string keyWord)
        {

            return RT.Service.Resolve<NumberRuleController>().GetNumberRule(RuleType.Barcode, keyWord, null);
        }
        /// <summary>
        /// 条码号打印
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [ApiService("条码号打印")]
        [return: ApiReturn("条码号打印 ")]
        public virtual List<Barcode> BarcodePrint([ApiParameter("打印信息")] PrinterInfo info)
        {
            return RT.Service.Resolve<BarcodeController>().Print(info).ToList();
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
        /// 已打印数
        /// </summary>
        public int PrintedQty { get; set; }

        /// <summary>
        /// 剩余数
        /// </summary>
        public decimal ResidualQty { get; set; }

        /// <summary>
        /// 条码规则明细
        /// </summary>
        public string BarcodeRuleDtl { get; set; }
    }
}
