using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using System;

namespace SIE.Equipments.EquipModels
{

    #region 设备型号存在设备台账引用不能删除
    /// <summary>
    /// 设备型号存在设备台账引用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("设备型号存在设备台账引用不能删除")]
    [System.ComponentModel.Description("设备型号存在设备台账引用不能删除")]
    public class EquipModelReferencedRule : NoReferencedRule<EquipModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipModelReferencedRule()
        {
            Properties.Add(EquipAccount.EquipModelIdProperty);

            MessageBuilder = (o, e) =>
            {
                return "不能删除，设备型号被设备台账引用".L10N();
            };
        }
    }
    #endregion
}