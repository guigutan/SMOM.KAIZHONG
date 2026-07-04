using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    #region 一个接收单的行号不能重复
    /// <summary>
    /// 一个接收单的行号不能重复
    /// </summary>
    [DisplayName("一个接收单的行号不能重复")]
    [Description("一个接收单的行号不能重复")]
    public class EquipmentReceiveDetailNotDuplicateRule : NotDuplicateRule<EquipmentReceiveDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentReceiveDetailNotDuplicateRule()
        {
            Properties.Add(EquipmentReceiveDetail.EquipmentReceiveIdProperty);
            Properties.Add(EquipmentReceiveDetail.LineNoProperty);
            MessageBuilder = (e) => { return "一个接收单的行号不能重复".L10N(); };
        }
    }
    #endregion

    #region 接收明细非重复验证规则
    /// <summary>
    /// 接收明细非重复验证规则
    /// </summary>
    [DisplayName("接收明细非重复验证规则")]
    [Description("接收明细非重复验证规则")]
    public class EquipReceiveDetailNotDuplicateRule : NotDuplicateRule<EquipmentReceiveDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipReceiveDetailNotDuplicateRule()
        {
            Properties.Add(EquipmentReceiveDetail.EquipmentReceiveIdProperty);
            Properties.Add(EquipmentReceiveDetail.GiveawayProperty);
            Properties.Add(EquipmentReceiveDetail.PurchaseOrderIdProperty);
            Properties.Add(EquipmentReceiveDetail.PurchaseOrderItemIdProperty);
            Properties.Add(EquipmentReceiveDetail.EquipModelIdProperty);
            Properties.Add(EquipmentReceiveDetail.SupplierIdProperty);
            Properties.Add(EquipmentReceiveDetail.CustomerIdProperty);
            Properties.Add(EquipmentReceiveDetail.WarehouseIdProperty);
            MessageBuilder = (e) => { return "同一个接收单下的接收明细【赠品+采购订单+采购订单行+设备型号+供应商+客户+接收仓库】唯一".L10N(); };
        }
    }
    #endregion
}
