using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.MES.WorkOrders._ERP_
{
    /// <summary>
    /// 被引用的编码段规则
    /// </summary>
    [DisplayName("ERP引用验证")]
    [Description("ERP订单被引用时不允许删除")]
    public class ErpSaleOrderNoReferencedRule : NoReferencedRule<ErpSaleOrder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpSaleOrderNoReferencedRule()
        {
            Properties.Add(ErpWorkOrder.ErpSaleOrderIdProperty);
            Properties.Add(WorkOrder.ErpSaleOrderIdProperty);
        }
    }

    /// <summary>
    /// 被引用的编码段规则
    /// </summary>
    [DisplayName("ERP引用验证")]
    [Description("ERP工单被引用时不允许删除")]
    public class ErpWorkOrderNoReferencedRule : NoReferencedRule<ErpWorkOrder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpWorkOrderNoReferencedRule()
        {
            Properties.Add(WorkOrder.ErpWorkOrderIdProperty);
        }
    }

    /// <summary>
    /// ERP工单的计划开始日期不能大于计划完成日期
    /// </summary>
    [DisplayName("ERP工单属性计划开始日期验证")]
    [Description("ERP工单属性计划开始日期验证规则")]
    public class ErpWorkOrderPrptyPlanBeginDateRule : PropertyRule<ErpWorkOrder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpWorkOrderPrptyPlanBeginDateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 托管属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return ErpWorkOrder.PlanBeginDateProperty;
            }
        }

        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">ERP工单实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var erpWorkOrder = entity as ErpWorkOrder;
            if (erpWorkOrder != null )
            {
                if (erpWorkOrder.PlanBeginDate > erpWorkOrder.PlanEndDate)
                    e.BrokenDescription = "计划开始日期不能大于计划完成日期".L10N();
            }
        }
    }

    /// <summary>
    /// ERP工单的计划完成日期不能小于计划开始日期
    /// </summary>
    [DisplayName("ERP工单属性计划完成日期验证")]
    [Description("ERP工单属性验计划完成日期证规则")]
    public class ErpWorkOrderPrptyPlanEndDateRule : PropertyRule<ErpWorkOrder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpWorkOrderPrptyPlanEndDateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 托管属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return ErpWorkOrder.PlanEndDateProperty;
            }
        }

        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">ERP工单实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var erpWorkOrder = entity as ErpWorkOrder;
            if (erpWorkOrder != null )
            {
                if (erpWorkOrder.PlanEndDate < erpWorkOrder.PlanBeginDate)
                    e.BrokenDescription = "计划完成日期不能小于计划开始日期".L10N();
            }
        }
    }
}
