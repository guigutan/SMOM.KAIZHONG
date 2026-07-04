using SIE.Barcodes.Panels;
using SIE.Barcodes.Panels.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Web.Barcodes.Barcodes.ViewModels;
using SIE.Web.Barcodes.Panels.Commands;
using SIE.Web.Barcodes.Panels.ViewModels;
using SIE.Web.Data;
using System;
using System.Text;

namespace SIE.Web.Barcodes.Panels.DataQueryers
{
    /// <summary>
    /// 拼板码打印查询器
    /// </summary>
    public class PanelPrintDataQuery : DataQueryer
    {
        /// <summary>
        /// 加载拼板码打印的初始数据
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>拼板码打印数据</returns>
        public PanelPrintData GetBarcodePrintData(double workOrderId)
        {
            var workOrder = RT.Service.Resolve<PanelController>().GetPanelWorkOrder(workOrderId);
            var scrapedQty = RT.Service.Resolve<PanelController>().GetScrapPanelCount(workOrderId);
            var config = new PanelPrintConfigValue();
            try
            {
                config = RT.Service.Resolve<PanelController>().GetPanelPrintConfig();
            }
            catch (Exception)
            {
                return null;
            }
            var printedQty = workOrder.PanelPrintQty;
            var residualQty = 0;
            if (workOrder.PanelQty > 0)
            {
                int fullBoxCount = (int)Math.Ceiling(workOrder.PlanQty / workOrder.PanelQty - printedQty);

                //if(workOrder.IsPanelWorkOrder)
                //    fullBoxCount = (int)(workOrder.PlanQty - printedQty);

                residualQty = fullBoxCount;
            }
            var printData = new PanelPrintData();
            printData.PanelQty = workOrder.PanelQty;
            printData.DumpingQty = scrapedQty;
            printData.NumberRuleId = config.BacodeRule?.Id;
            printData.NumberRuleName = config.BacodeRule?.Name;
            printData.TemplateId = config.LabelTemplate?.Id;
            printData.TemplateName = config.LabelTemplate?.FileName;
            if (config.BacodeRule != null)
            {
                if (config.LabelTemplate != null)
                    printData.TemplateEntityType = config.LabelTemplate.EntityType;
                var numberRule = RF.GetById<NumberRule>(config.BacodeRule.Id);
                StringBuilder sb = new StringBuilder();
                foreach (var ruleDtl in numberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
                printData.BarcodeRuleDtl = sb.ToString();
                if (residualQty > 0)
                {
                    GetPrintData(residualQty, false, config.BacodeRule.Id, workOrder, printData);
                }
            }
            return printData;
        }

        /// <summary>
        /// 获取拼板码打印数据
        /// </summary>
        /// <param name="panelPrintVM">拼板码打印ViewModel</param>
        /// <returns>拼板码打印数据</returns>
        public PanelPrintData GetPrintData(PanelPrintViewModel panelPrintVM)
        {
            var printData = new PanelPrintData();
            var workOrder = RT.Service.Resolve<PanelController>().GetPanelWorkOrder(panelPrintVM.WorkOrderId);
            if (panelPrintVM.NumberRuleId != null)
            {
                if (panelPrintVM.Template != null)
                    printData.TemplateEntityType = panelPrintVM.Template.EntityType;
                var numberRule = RF.GetById<NumberRule>(panelPrintVM.NumberRuleId.Value);
                StringBuilder sb = new StringBuilder();
                foreach (var ruleDtl in numberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
                printData.BarcodeRuleDtl = sb.ToString();
                if (panelPrintVM.PrintQty > 0)
                    GetPrintData(panelPrintVM.PrintQty, panelPrintVM.PrintControl, panelPrintVM.NumberRuleId.Value, workOrder, printData);
            }
            return printData;
        }

        /// <summary>
        /// 获取拼板码打印数据
        /// </summary>
        /// <param name="printQty">可打印数量</param>
        /// <param name="printControl">是否反打</param>
        /// <param name="numberRuleId">条码规则Id</param>
        /// <param name="workOrder">工单</param>
        /// <param name="printData">打印数据</param>
        private void GetPrintData(int printQty, bool printControl, double numberRuleId, PanelWorkOrder workOrder, PanelPrintData printData)
        {
            var ruleCt = RT.Service.Resolve<NumberRuleController>();
            if (!printControl) //默认为正打
            {
                printData.BeginSn = ruleCt.GetStartSegment(numberRuleId, workOrder);
                printData.EndSn = ruleCt.GetEndSegment(numberRuleId, printQty, workOrder);
            }
            else
            {
                printData.EndSn = ruleCt.GetStartSegment(numberRuleId, workOrder);
                printData.BeginSn = ruleCt.GetEndSegment(numberRuleId, printQty, workOrder);
            }
        }

        /// <summary>
        /// 获取拼板码补打ViewModel
        /// </summary>
        /// <returns>拼板码补打ViewModel</returns>
        public ReprintInfo GetReprintInfo()
        {
            var reprintInfo = new ReprintInfo() { Times = 1 };
            reprintInfo.Template = RT.Service.Resolve<PanelController>().GetPanelPrintConfig().LabelTemplate;
            return reprintInfo;
        }
    }
}
