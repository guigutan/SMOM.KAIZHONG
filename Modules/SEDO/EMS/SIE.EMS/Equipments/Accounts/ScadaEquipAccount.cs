using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
	/// SCADA设备台账
	/// </summary>
	[RootEntity, Serializable]
    [Label("SCADA设备台账")]
    public partial class ScadaEquipAccount : EquipAccount
    {
        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<ScadaEquipAccount>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion
    }
}