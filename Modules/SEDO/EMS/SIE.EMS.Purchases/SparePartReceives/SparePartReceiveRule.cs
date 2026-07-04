using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.SparePartReceives
{
    #region 备件接收明细
    /// <summary>
    /// 一个接收单的行号不能重复
    /// </summary>
    [DisplayName("一个接收单的行号不能重复")]
    [Description("一个接收单的行号不能重复")]
    public class SparePartReceiveDetailNotDuplicateRule : NotDuplicateRule<SparePartReceiveDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SparePartReceiveDetailNotDuplicateRule()
        {
            Properties.Add(SparePartReceiveDetail.SparePartReceiveIdProperty);
            Properties.Add(SparePartReceiveDetail.LineNoProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                return "一个接收单的行号不能重复".L10N();
            };
        }
    }

    /// <summary>
    /// 同一个接收单下，采购订单行+仓库唯一
    /// </summary>
    [DisplayName("同一个接收单下，采购订单行+仓库唯一")]
    [Description("同一个接收单下，采购订单行+仓库唯一")]
    public class SparePartReceiveDetailPurNotDuplicateRule : NotDuplicateRule<SparePartReceiveDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SparePartReceiveDetailPurNotDuplicateRule()
        {
            Properties.Add(SparePartReceiveDetail.SparePartReceiveIdProperty);
            Properties.Add(SparePartReceiveDetail.PurchaseOrderItemIdProperty);
            Properties.Add(SparePartReceiveDetail.WarehouseIdProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                return "同一个接收单下，采购订单行+仓库唯一".L10N();
            };
        }
    }

    /// <summary>
    /// 备件接收明细验证规则
    /// </summary>
    [DisplayName("备件接收明细验证规则")]
    [Description("备件接收明细验证规则")]
    public class SparePartReceiveDetailRule : EntityRule<SparePartReceiveDetail>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">参数明细</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var detail = entity as SparePartReceiveDetail;
            if (detail.RecivedQty > detail.Qty)
            {
                e.BrokenDescription = "【已接收数量】不能大于【接收数量】".L10N();
            }
        }
    }
    #endregion

    #region 序列号明细
    /// <summary>
    /// 序列号明细验证规则
    /// </summary>
    [DisplayName("序列号明细验证规则")]
    [Description("序列号明细验证规则")]
    public class SparePartReceiveSnRule : EntityRule<SparePartReceiveSn>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">参数明细</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var sn = entity as SparePartReceiveSn;
            if (!sn.OriginalSn.IsNullOrWhiteSpace())
            {
                var check = RT.Service.Resolve<SparePartReceiveController>().GetReceiveSnOriginalSnQty(sn.OriginalSn, sn.Id);
                if (check > 0)
                {
                    e.BrokenDescription = "原厂序列号:{0}已存在".L10nFormat(sn.OriginalSn);
                }
            }
        }
    }
    #endregion
}
