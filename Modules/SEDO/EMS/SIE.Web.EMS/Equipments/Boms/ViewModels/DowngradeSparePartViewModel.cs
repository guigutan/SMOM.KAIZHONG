using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Boms;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Equipments.Boms.ViewModels
{
    /// <summary>
    /// 设备台账降级ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class DowngradeSparePartViewModel : ViewModel
    {
        #region 设备BOM明细 EquipBomDetail
        /// <summary>
        /// 设备BOM明细Id
        /// </summary>
        public static readonly IRefIdProperty EquipBomDetailIdProperty =
            P<DowngradeSparePartViewModel>.RegisterRefId(e => e.EquipBomDetailId, ReferenceType.Normal);

        /// <summary>
        /// 设备BOM明细Id
        /// </summary>
        public double? EquipBomDetailId
        {
            get { return (double?)this.GetRefId(EquipBomDetailIdProperty); }
            set { this.SetRefId(EquipBomDetailIdProperty, value); }
        }

        /// <summary>
        /// 设备BOM明细
        /// </summary>
        public static readonly RefEntityProperty<EquipBomDetail> EquipBomDetailProperty =
            P<DowngradeSparePartViewModel>.RegisterRef(e => e.EquipBomDetail, EquipBomDetailIdProperty);

        /// <summary>
        /// 设备BOM明细
        /// </summary>
        public EquipBomDetail EquipBomDetail
        {
            get { return this.GetRefEntity(EquipBomDetailProperty); }
            set { this.SetRefEntity(EquipBomDetailProperty, value); }
        }
        #endregion

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<DowngradeSparePartViewModel>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<DowngradeSparePartViewModel>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备台账降级ViewModel ViewConfig
    /// </summary>
    public class DowngradeSparePartViewModelViewConfig : WebViewConfig<DowngradeSparePartViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.EquipBomDetail)
                .HasLabel("目标父备件").Show(ShowInWhere.All);
        }
    }
}
