using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts.Tab
{
    /// <summary>
    /// 特种设备型号润滑项目
    /// </summary>
    [RootEntity, Serializable]
    [Label("特种设备台账润滑项目")]
    public class SpecialEquipAccountLubricationProject : EquipAccountLubricationProject
    {
        #region 润滑项目列表 EquipModel
        /// <summary>
        /// 润滑项目列表Id
        /// </summary>
        public static new readonly IRefIdProperty EquipAccountIdProperty = P<SpecialEquipAccountLubricationProject>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 润滑项目列表Id
        /// </summary>
        public new double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 润滑项目列表
        /// </summary>
        public static new readonly RefEntityProperty<SpecialEquipmentAccount> EquipModelProperty = P<SpecialEquipAccountLubricationProject>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 润滑项目列表
        /// </summary>
        public new SpecialEquipmentAccount EquipAccount
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion
    }
}
