using AngleSharp.Dom;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.NumberRules;
using SIE.Core.Items;
using SIE.Domain;
using SIE.MES.BatchGeneration.Services;
using SIE.MES.WorkOrders;
using SIE.Packages.Boxs;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Web.Barcodes.WipBatchs.ViewModels;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Linq;
using System.Text;
using static IronPython.Modules._ast;

namespace SIE.Web.MES.BatchGeneration
{
    /// <summary>
    /// 客制查询数据处理
    /// </summary>
    public class WipBatchsDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取批次生成的模型初始数据
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>模型初始数据</returns>
        public EntityJson GetBatchGeneratingViewModel(double workOrderId)
        {
            EntityJson json = new EntityJson();
            var wo = RF.GetById<WorkOrder>(workOrderId, new EagerLoadOptions().LoadWithViewProperty());
            if (wo == null)
            {
                json.SetProperty("ErrorMsg", "没找到工单Id" + workOrderId);
                return json;
            }
            else
            {
                json.SetProperty("ErrorMsg", string.Empty);
            }
            var defaultProcesses = RT.Service.Resolve<WOBatchGenerationService>().GetProcessesByWoId(workOrderId, ProcessType.BatchAssembly);
            var firstProcess = RT.Service.Resolve<WOBatchGenerationService>().GetFirstProcessesByWoId(workOrderId, ProcessType.BatchAssembly);
            if (firstProcess != null)
            {
                json.SetProperty("ProcessId", firstProcess.Id);
                json.SetProperty("ProcessName", firstProcess.Name);
            }
            else
            {
                if (defaultProcesses.Count == 1 && defaultProcesses.Any())
                {
                    json.SetProperty("ProcessId", defaultProcesses.FirstOrDefault()?.Id);
                    json.SetProperty("ProcessName", defaultProcesses.FirstOrDefault()?.Name);
                }
            }
            json.SetProperty("ResourceId", wo.ResourceId);
            json.SetProperty("ResourceName", wo.ResourceName);
            string warn = string.Empty;
            json.SetProperty("BatchRule", "");
            json.SetProperty("BatchQty", 1);
            json.SetProperty("NumberRuleId", wo.Template?.NumberRuleId);
            json.SetProperty("NumberRuleName", wo.Template?.NumberRule?.Name);
            if (wo.Template?.LabelTemplate != null && wo.Template.LabelTemplate.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
            {
                json.SetProperty("TemplateId", null);
                json.SetProperty("TemplateName", null);
                if (!string.IsNullOrEmpty(warn))
                {
                    warn += "\n\r";
                }

                warn += "工单[{0}]打印设置的默认标签模板不是生产批次类型的，请重新选择打印模板".L10nFormat(wo.No);
            }
            else
            {
                json.SetProperty("TemplateId", wo.Template?.LabelTemplateId);
                json.SetProperty("TemplateName", wo.Template?.LabelTemplate?.FileName);
            }
            json.SetProperty("WarnMsg", warn);
            return json;
        }

        /// <summary>
        /// 根据工序资源
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public EntityJson getStationResourceId(double processId, double resourceId)
        {
            EntityJson json = new EntityJson();
            var stations = RT.Service.Resolve<StationController>().GetStationsByResourceId(resourceId, processId);
            if (stations.Any() && stations.Count == 1)
            {
                json.SetProperty("StationId", stations[0].Id);
                json.SetProperty("StationName", stations[0].Name);
            }
            return json;
        }


        #region 预览、打印控制
        /// <summary>
        /// 编码规则控制器对象
        /// </summary>
        readonly NumberRuleController controller = RT.Service.Resolve<NumberRuleController>();

        /// <summary>
        /// 获取开始结束条码
        /// </summary>
        /// <param name="generateingQty">生成数量</param>
        /// <param name="batchQty">批次数量</param>
        /// <param name="numberRuleId">条码规则Id</param>
        /// <returns>开始结束条码</returns>
        public EntityJson GetBeginSnEndSn(decimal generateingQty, decimal batchQty, double numberRuleId)
        {
            EntityJson json = new EntityJson();
            int printQty = (int)Math.Ceiling(generateingQty / batchQty);
            var beginSn = controller.GetStartSegment(numberRuleId);
            var endSn = controller.GetEndSegment(numberRuleId, printQty);
            json.SetProperty("BeginSn", beginSn);
            json.SetProperty("EndSn", endSn);
            return json;
        }

        /// <summary>
        /// 获取规则明细
        /// </summary>
        /// <param name="numberRuleId">条码规则Id</param>
        /// <returns>规则明细</returns>
        public string GetRuleDetail(double numberRuleId)
        {
            var numberRule = RF.GetById<NumberRule>(numberRuleId);
            StringBuilder sb = new StringBuilder();
            if (numberRule != null)
            {
                foreach (var ruleDtl in numberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取开始结束条码(子批)
        /// </summary>
        /// <param name="generateingQty">生成数量</param>
        /// <param name="batchQty">批次数量</param>
        /// <param name="numberRuleId">条码规则Id</param>
        /// <param name="childRuleId">子条码规则</param>
        /// <param name="childBatchQty">子批次数量</param>
        /// <returns>开始结束条码(子批)</returns>
        public EntityJson GetChildBeginSnEndSn(decimal generateingQty, decimal batchQty, double numberRuleId, double childRuleId, decimal childBatchQty)
        {
            EntityJson json = new EntityJson();
            int first = (int)Math.Ceiling(generateingQty / batchQty);
            int printQty = (int)Math.Ceiling(batchQty / childBatchQty) * first;

            var mantissaQty = (int)Math.Ceiling(generateingQty % batchQty / childBatchQty);
            if (mantissaQty != 0)
            {
                printQty = (int)Math.Ceiling(batchQty / childBatchQty) * (first - 1) + mantissaQty;
            }

            string childBeginSn = string.Empty;
            string childEndSn = string.Empty;
            if (childRuleId == numberRuleId)
            {
                childBeginSn = controller.GetEndSegment(childRuleId, first + 1);
                childEndSn = controller.GetEndSegment(childRuleId, printQty + first);
            }
            else
            {
                childBeginSn = controller.GetStartSegment(childRuleId);
                childEndSn = controller.GetEndSegment(childRuleId, printQty);
            }

            json.SetProperty("ChildBeginSn", childBeginSn);
            json.SetProperty("ChildEndSn", childEndSn);
            return json;
        }

        /// <summary>
        /// 获取批次数量
        /// </summary>
        /// <param name="batchRuleValue">批次规则</param>
        /// <param name="productId">产品Id</param>
        /// <param name="woId">工单Id</param>
        /// <returns>批次数量</returns>
        public decimal GetBatchQty(int batchRuleValue, double productId, double woId)
        {
            decimal batchQty = 1;
            if (batchRuleValue == (int)SIE.Core.Items.BatchRule.Product)
            {
                batchQty = RT.Service.Resolve<SIE.Items.ItemController>().GetBatchRule(productId)?.Qty ?? 1;
            }
            else if (batchRuleValue == (int)SIE.Core.Items.BatchRule.WorkOrder)
            {
                var qty = RT.Service.Resolve<Core.WorkOrders.WorkOrderController>().GetWipBatch(woId)?.Qty;
                batchQty = qty ?? 1;
            }
            else if (batchRuleValue == (int)SIE.Core.Items.BatchRule.Vehicle)
            {
                //var qty = RT.Service.Resolve<Packages.Boxs.BoxController>().GetDefaultProductCapacity(productId, null).FirstOrDefault()?.Capacity;
                //batchQty = qty ?? 1;
            }
            else
            {
                //
            }

            return batchQty;
        }

        /// <summary>
        /// 控制反打
        /// </summary>   
        /// <param name="generateingQty">生成数量</param>
        /// <param name="batchQty">批次数量</param>
        /// <param name="numberRuleId">条码规则Id</param>
        /// <param name="printControl">反打bool</param>
        /// <param name="childBatchQty">子批次数量</param>
        /// <param name="childNumberRuleId">子条码规则</param>
        /// <returns>开始结束条码</returns>
        public EntityJson PrintControlChange(decimal generateingQty, decimal batchQty, double numberRuleId, bool printControl, decimal childBatchQty, double? childNumberRuleId)
        {
            EntityJson json = new EntityJson();
            int printQty = (int)Math.Ceiling(generateingQty / batchQty);
            string beginSn = string.Empty;
            string endSn = string.Empty;
            if (!printControl) //默认为正打
            {
                beginSn = controller.GetStartSegment(numberRuleId);
                endSn = controller.GetEndSegment(numberRuleId, printQty);
            }
            else
            {
                endSn = controller.GetStartSegment(numberRuleId);
                beginSn = controller.GetEndSegment(numberRuleId, printQty);
            }

            json.SetProperty("BeginSn", beginSn);
            json.SetProperty("EndSn", endSn);
            string childBeginSn = string.Empty;
            string childEndSn = string.Empty;
            if (childNumberRuleId == null || generateingQty <= 0 || childBatchQty <= 0) //条码规则不空，打印数量打印0
            {
                json.SetProperty("ChildBeginSn", childBeginSn);
                json.SetProperty("ChildEndSn", childEndSn);
                return json;
            }

            int first = printQty;
            printQty = (int)Math.Ceiling(batchQty / childBatchQty) * first;
            var mantissaQty = (int)Math.Ceiling(generateingQty % batchQty / childBatchQty);
            if (mantissaQty != 0)
            {
                printQty = (int)Math.Ceiling(batchQty / childBatchQty) * (first - 1) + mantissaQty;
            }

            if (!printControl) //默认为正打
            {
                if (childNumberRuleId == numberRuleId)
                {
                    childBeginSn = controller.GetEndSegment(childNumberRuleId.Value, first + 1);
                    childEndSn = controller.GetEndSegment(childNumberRuleId.Value, printQty + first);
                }
                else
                {
                    childBeginSn = controller.GetStartSegment(childNumberRuleId.Value);
                    childEndSn = controller.GetEndSegment(childNumberRuleId.Value, printQty);
                }
            }
            else
            {
                if (childNumberRuleId == numberRuleId)
                {
                    childEndSn = controller.GetEndSegment(childNumberRuleId.Value, first + 1);
                    childBeginSn = controller.GetEndSegment(childNumberRuleId.Value, printQty + first);
                }
                else
                {
                    childEndSn = controller.GetStartSegment(childNumberRuleId.Value);
                    childBeginSn = controller.GetEndSegment(childNumberRuleId.Value, printQty);
                }
            }

            json.SetProperty("ChildBeginSn", childBeginSn);
            json.SetProperty("ChildEndSn", childEndSn);
            return json;
        }
        #endregion

        /// <summary>
        /// 获取批次规则
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>批次规则信息</returns>
        public BatchRuleReturn GetBatchRule(double productId, double workOrderId)
        {
            var batchRule = RT.Service.Resolve<SIE.Items.ItemController>().GetBatchRule(productId);
            BatchRuleReturn rst = new BatchRuleReturn();
            if (batchRule == null)
            {
                return rst;
            }

            if (batchRule.BatchRule == SIE.Core.Items.BatchRule.WorkOrder)
            {
                var qty = RT.Service.Resolve<Core.WorkOrders.WorkOrderController>().GetWipBatch(workOrderId)?.Qty;
                if (qty == null)
                {
                    rst.Warning = "该工单未设置生成批次".L10N();
                }

                batchRule.Qty = qty ?? 0;
            }
            else if (batchRule.BatchRule == SIE.Core.Items.BatchRule.Vehicle)
            {
                var qty = RT.Service.Resolve<BoxController>().GetDefaultProductCapacity(productId, null).FirstOrDefault()?.Capacity;
                if (qty == null)
                {
                    rst.Warning = "工单的物料未设置默认载具".L10N();
                }

                batchRule.Qty = qty ?? 0;
            }
            else
            {
                //
            }

            rst.BatchRule = batchRule.BatchRule;
            rst.Qty = batchRule.Qty;
            return rst;
        }

        /// <summary>
        /// 获取补打信息
        /// </summary>
        /// <param name="wipBatchId">生产批次ID</param>
        /// <returns>补打信息</returns>
        public BatchReprintViewModel GetReprintInfo(double wipBatchId)
        {
            var wipBatch = RF.GetById<WipBatch>(wipBatchId);
            Check.NotNull(wipBatch, nameof(WipBatch));
            var reprintInfo = new BatchReprintViewModel();
            var template = RT.Service.Resolve<BarcodeController>().GetPrintTemplateByWo(wipBatch.WorkOrderId);
            if (template?.EntityType == typeof(WipBatchPrintable).GetQualifiedName())
            {
                reprintInfo.Template = template;
            }

            return reprintInfo;
        }

        /// <summary>
        /// 获取补打信息
        /// </summary>
        /// <param name="subWipBatchId">子生产批次ID</param>
        /// <returns>补打信息</returns>
        public BatchReprintViewModel GetSubReprintInfo(double subWipBatchId)
        {
            var subWipBatch = RF.GetById<SubWipBatch>(subWipBatchId);
            Check.NotNull(subWipBatch, nameof(SubWipBatch));
            var reprintInfo = new BatchReprintViewModel();
            var template = RT.Service.Resolve<BarcodeController>().GetPrintTemplateByWo(subWipBatch.WorkOrderId);
            if (template?.EntityType == typeof(WipBatchPrintable).GetQualifiedName())
            {
                reprintInfo.Template = template;
            }

            return reprintInfo;
        }
    }

    /// <summary>
    /// 批次规则返回信息
    /// </summary>
    public class BatchRuleReturn
    {
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get; set;
        }

        /// <summary>
        /// 规则
        /// </summary>
        public BatchRule? BatchRule
        {
            get; set;
        }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string Warning
        {
            get; set;
        }
    }
}
