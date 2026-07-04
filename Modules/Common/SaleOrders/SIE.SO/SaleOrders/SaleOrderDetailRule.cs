using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单明细验证规则
    /// </summary>
    [DisplayName("重复验证")]
    [Description("销售明细行号不能重复")]
    public class SaleOrderDetailRule : NotDuplicateRule<SaleOrderDetail>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public SaleOrderDetailRule()
        {
            Properties.Add(SaleOrderDetail.SaleOrderIdProperty);
            Properties.Add(SaleOrderDetail.LineNoProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as SaleOrderDetail;
                return "销售订单明细中已经存在[行号]是{0}的明细".L10nFormat(r.LineNo);
            };
        }
    }

    /*
    /// <summary>
    /// 销售订单明细验证规则
    /// </summary>
    [DisplayName("版本号验证")]
    [Description("版本号输入限制")]
    public class SaleOrderDetailLineNoRule : EntityRule<SaleOrderDetail>
    {
        /// <summary>
        /// 验证范围
        /// </summary>
        public SaleOrderDetailLineNoRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 版本号输入限制
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var detail = entity as SaleOrderDetail;
            var reg = new Regex(@"^[A-Z]{1}\d{2}$");
            var result = reg.IsMatch(detail.Version);
            if (!result)
            {
                e.BrokenDescription = "销售订单明细版本号限制,请正确输入例:A10!".L10N();
            }
        }

    }

    */

    #region 单位验证
    /// <summary>
    /// 单位已关联销售订单物料不能删除
    /// </summary>
    [DisplayName("验证规则")]
    [Description("单位已关联销售订单物料不能删除")]
    public class UndeleteInvolveSaleOrderItemProperty : EntityRule<Unit>
    {
        /// <summary>
        /// 验证范围
        /// </summary>
        public UndeleteInvolveSaleOrderItemProperty()
        {
            Scope = EntityStatusScopes.Delete;
        }
        /// <summary>
        /// 单位已关联销售订单物料不能删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var unit = entity as Unit;
            var count = RT.Service.Resolve<SaleOrderDetailController>().ValidateInvolveSaleOrderItem(unit.Id);
            if (count > 0)
            {
                e.BrokenDescription = "[单位:{0}]已经被[{1}]引用了{2}次!".L10nFormat(unit.Name, "销售订单", count);
            }
        }
    }
    #endregion

    #region 单个面积验证
    /// <summary>
    /// 单个面积验证不为0
    /// </summary>
    [DisplayName("验证规则")]
    [Description("单个面积验证不为0")]
    public class SingleAreaProperty : EntityRule<SaleOrderDetail>
    {
        /// <summary>
        /// 验证范围
        /// </summary>
        public SingleAreaProperty()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 单个面积验证不为0
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var sale = entity as SaleOrderDetail;
            if (sale.SingleArea <= 0)
            {
                e.BrokenDescription = "行号【{0}】物料【{1}】单个面积为0,请重新维护数量或总面积!".L10nFormat(sale.LineNo, sale.Item.Name);
            }
        }
    }
    #endregion

}
