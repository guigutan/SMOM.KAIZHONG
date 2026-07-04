using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 设备台账维修定标
    /// </summary>
    [RootEntity, Serializable]	
	[Label("设备台账维修定标")]
	public partial class MeteringEquipAccountRepairStandard : EquipAccountRepairStandardBase
    {
		#region 设备台账 EquipAccount
		/// <summary>
		/// 设备台账Id
		/// </summary>
		[Label("设备台账")]
		public static readonly IRefIdProperty EquipAccountIdProperty = P<MeteringEquipAccountRepairStandard>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

		/// <summary>
		/// 设备台账Id
		/// </summary>
		public double EquipAccountId
		{
			get { return (double)GetRefId(EquipAccountIdProperty); }
			set { SetRefId(EquipAccountIdProperty, value); }
		}

		/// <summary>
		/// 设备台账
		/// </summary>
		public static readonly RefEntityProperty<MeteringEquipmentAccount> EquipAccountProperty = P<MeteringEquipAccountRepairStandard>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

		/// <summary>
		/// 设备台账
		/// </summary>
		public MeteringEquipmentAccount EquipAccount
		{
			get { return GetRefEntity(EquipAccountProperty); }
			set { SetRefEntity(EquipAccountProperty, value); }
		}
		#endregion


    }

	/// <summary>
	/// 设备台账维修定标 实体配置
	/// </summary>
	internal class EquipAccountRepairStandardConfig : EntityConfig<MeteringEquipAccountRepairStandard>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_EQP_RUN_STD").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}