using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 发货计划验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("发货计划验证规则")]
    [System.ComponentModel.Description("发货计划验证规则")]
    public class DeliveryPlanNotNullRule : EntityRule<DeliveryPlan>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public DeliveryPlanNotNullRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }

            var plan = entity as DeliveryPlan;
            if (plan.RequireQty <= 0)
            {
                e.BrokenDescription = "发货计划需求数必须大于0".L10N();
            }
            if (plan.No.IsNullOrEmpty())
            {
                e.BrokenDescription = "发货计划单号不能为空".L10N();
            }
            if (plan.LineNo <= 0)
            {
                e.BrokenDescription = "发货计划明细行号必须填写".L10nFormat();
            }
            if (plan.OrderType == OrderType.SaleOut && plan.CustomerId == null)
            {
                e.BrokenDescription = "单据类型为销售出库时，客户不能为空！".L10N();
            }
            if (plan.OrderType == OrderType.SupplierReturn && plan.SupplierId == null)
            {
                e.BrokenDescription = "单据类型为供应商退货时，供应商不能为空！".L10N();
            }
            if ((plan.OrderType == OrderType.OutWorkFeed || plan.OrderType == OrderType.OutWorkFeed || plan.OrderType == OrderType.OutWorkFeed) && plan.SupplierId == null)
            {
                e.BrokenDescription = "单据类型为委外工单发料时，供应商不能为空！".L10N();
            }
            if ((plan.OrderType == OrderType.DirectAllocate || plan.OrderType == OrderType.TwoAllocate) && plan.TargetWarehouseId == null)
            {
                e.BrokenDescription = "单据类型为直接调拨、两步调拨时，目标仓库不能为空！".L10N();
            }
        }
    }

}