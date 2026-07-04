using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 工单验证器
    /// </summary>
    public class WorkOrderValidator
    {
        /// <summary>
        /// 打印配置设置值
        /// </summary>
        private readonly PrintTemplateConfigValue printTemplateConfig;

        /// <summary>
        /// 工单下达验证器
        /// </summary>
        private readonly TaskReleaseValidator taskReleaseValidator;

        /// <summary>
        /// 工单下达的原材料物料数据拥有者
        /// </summary>
        private readonly ItemDataOwner itemDataOwner;

        /// <summary>
        /// 工单验证器构造函数
        /// </summary>
        public WorkOrderValidator(TaskReleaseValidator taskReleaseValidator, ItemDataOwner itemDataOwner)
        {
            //打印配置设置值
            printTemplateConfig = ConfigService.GetConfig(new PrintTemplateConfig(), typeof(WorkOrder));

            this.taskReleaseValidator = taskReleaseValidator;
            this.itemDataOwner = itemDataOwner;
        }

        /// <summary>
        /// 检验待保存的工单的合法性
        /// </summary>
        /// <param name="workOrder">待保存的工单</param>        
        /// <param name="releasePlanDetail">下达计划明细数据</param>
        /// <param name="releasePlanResult">下达结果</param>
        public void ValidateSavingWorkOrder(WorkOrder workOrder, ReleasePlanDetail releasePlanDetail, ReleasePlanResult releasePlanResult)
        {
            if (workOrder is null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (releasePlanDetail is null)
            {
                throw new ArgumentNullException(nameof(releasePlanDetail));
            }

            if (releasePlanResult is null)
            {
                throw new ArgumentNullException(nameof(releasePlanResult));
            }

            StringBuilder cursbMsg = new StringBuilder();

            if (workOrder.PlanEndDate < workOrder.PlanBeginDate)
            {
                cursbMsg.Append("计划完成时间必须大于计划开始时间".L10N());
            }

            if (workOrder.OrderQty <= 0)
            {
                cursbMsg.Append("工单订单数量必须大于0".L10N());
            }

            if (workOrder.PlanQty <= 0)
            {
                cursbMsg.Append("工单计划数量必须大于0".L10N());
            }

            if (workOrder.PackageRuleDetailList.Count(p => p.IsInStockLabel) > 1)
            {
                cursbMsg.Append("包装规则入库标签只能选择一个".L10N());
            }

            if (printTemplateConfig.IsNeed)
            {
                if (workOrder.Template.NumberRuleId == null)
                {
                    cursbMsg.Append("条码规则不能为空".L10N());
                }

                if (workOrder.Template.LabelTemplateId == null)
                {
                    cursbMsg.Append("标签模板不能为空".L10N());
                }

                if (workOrder.Template.PackingTemplateId == null)
                {
                    cursbMsg.Append("包装模板不能为空".L10N());
                }
            }

            if (!workOrder.VersionId.HasValue)
            {
                cursbMsg.Append("工单工艺路线版本不能为空".L10N());
            }
            //先保存工单工序清单
            var routingProcesses = workOrder.RoutingProcessList;

            if (!routingProcesses.Any())
            {
                cursbMsg.Append("工单工艺路线工序为空，请检查工艺路线".L10N());
            }

            if (routingProcesses.GroupBy(x => x.Index).Any(g => g.Count() > 1))
            {
                cursbMsg.Append("工单工艺路线工序存在重复，请检查工艺路线".L10N());
            }

            if (routingProcesses.Any(p => p.ParameterList.Count <= 0))
            {
                cursbMsg.Append("工单工艺路线工序参数为空，请检查工艺路线".L10N());
            }

            var product = this.taskReleaseValidator.Products.FirstOrDefault(x => x.Id == workOrder.ProductId);

            if (product == null)
            {
                cursbMsg.Append("工单的产品【{0}】找不到".L10nFormat(workOrder.ProductId));
            }
            else
            {
                if (product.EnableExtendProperty && workOrder.ItemExtProp.IsNullOrEmpty())
                {
                    cursbMsg.Append("物料【{0}】启用扩展属性，必须输入扩展属性！".L10nFormat(product.Code));
                }
            }

            //工单工序BOM清单的列表
            var processBoms = workOrder.ProcessBomList;

            foreach (var processBom in processBoms)
            {
                var processBomItem = this.itemDataOwner.GetItem(processBom.ItemId);

                if (processBomItem == null)
                {
                    cursbMsg.Append("工单的工序BOM的物料【{0}】找不到".L10nFormat(processBom.ItemId));
                }
                else
                {
                    if (processBomItem.EnableExtendProperty && processBom.ItemExtProp.IsNullOrEmpty())
                    {
                        cursbMsg.Append("物料【{0}】启用扩展属性，必须输入扩展属性！".L10nFormat(processBomItem.Code));
                    }
                }
            }

            foreach (var bom in workOrder.BomList)
            {
                var bomItem = this.itemDataOwner.GetItem(bom.ItemId);

                if (bomItem == null)
                {
                    cursbMsg.Append("工单BOM的物料【{0}】找不到".L10nFormat(bom.ItemId));
                }
                else
                {
                    if (bomItem.EnableExtendProperty && bom.ItemExtProp.IsNullOrEmpty())
                    {
                        cursbMsg.Append("物料【{0}】启用扩展属性，必须输入扩展属性！".L10nFormat(bomItem.Code));
                    }
                }
            }

            if (cursbMsg.Length > 0)
            {
                var curDetailResult = TaskReleaseHelper.CreateReleaseDetailResult(releasePlanDetail.DetailId,
                    releasePlanDetail.ProcessTechOrderCode, cursbMsg.ToString(), string.Empty);
                releasePlanResult.Details.Add(curDetailResult);
                TaskReleaseHelper.SetReleasePlanMainResult(releasePlanResult, false, string.Empty);
            }
        }

        /// <summary>
        /// 验证工单号是否重复
        /// </summary>
        /// <param name="workOrders"></param>
        /// <exception cref="ValidationException"></exception>
        public static void ValidateNoIfDuplicate(IList<WorkOrder> workOrders)
        {
            var woNos = workOrders.Select(x => x.No).Distinct().ToList();
            var workOrdersExists = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(woNos);
            workOrders.ForEach(wo =>
            {
                if (workOrdersExists.Any(x => x.No == wo.No && x.Id != wo.Id))
                {
                    throw new ValidationException("已经存在工单号为【{0}】的工单！".L10nFormat(wo.No));
                }
            });
        }
    }
}
