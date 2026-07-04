using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单验证规则
    /// </summary>
    [DisplayName("明细验证")]
    [Description("销售明细行不能为空")]
    public class SalesOrderRule : EntityRule<SaleOrder>
    {
        /// <summary>
        /// 验证范围
        /// </summary>
        public SalesOrderRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 销售明细行不能为空
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var salesOrder = entity as SaleOrder;
            var count = RT.Service.Resolve<SaleOrderController>().ValidateSalesOrderDetail(salesOrder.Id);
            if (count == salesOrder.SaleOrderDetailList.DeletedList.Count && salesOrder.SaleOrderDetailList.Count <= 0)
            {
                e.BrokenDescription = "销售订单明细不能为空!".L10N();
            }
        }
    }

    /// <summary>
    /// 销售订单明细已经引用物料，不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("物料删除验证规则")]
    [System.ComponentModel.Description("销售订单明细已经引用物料，不允许删除")]
    public class DeleteSoItemRule : EntityRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeleteSoItemRule()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (RT.Service.Resolve<SaleOrderController>().IsExistsItem(item.Id))
            {
                e.BrokenDescription = "物料已被销售订单引用，不能删除".L10N();
            }
        }
    }
}
