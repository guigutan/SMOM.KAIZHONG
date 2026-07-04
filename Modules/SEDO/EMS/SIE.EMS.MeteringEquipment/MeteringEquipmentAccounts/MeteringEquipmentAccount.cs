using SIE.Common.Configs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab;
using SIE.Equipments.Configs;
using SIE.Equipments.DataAuth;
using SIE.Equipments.DeviceControls.Configs;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 计量设备台账
    /// </summary>
    [RootEntity, Serializable]
	[DisplayMember(nameof(Code))]
	[EntityWithConfig(typeof(SmdcUrlConfig))]
	[ConditionQueryType(typeof(MeteringEquipmentAccountCriteria))]
	[EquipAccountAuth(nameof(EquipModelId), nameof(UseDepartmentId), true)]
	[EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	[Label("计量设备台账")]
	public partial class MeteringEquipmentAccount : EquipAccountBase
    {
		#region 计量校验规程 EquipAccountCalibrationList
		/// <summary>
		/// 计量校验规程
		/// </summary>
		public static readonly ListProperty<EntityList<EquipAccountCalibration>> EquipAccountCalibrationListProperty = P<MeteringEquipmentAccount>.RegisterList(e => e.EquipAccountCalibrationList);
		/// <summary>
		/// 计量校验规程
		/// </summary>
		public EntityList<EquipAccountCalibration> EquipAccountCalibrationList
		{
			get { return this.GetLazyList(EquipAccountCalibrationListProperty); }
		}
		#endregion

		#region  基础子标签New新集合

		#region 缸槽列表 PcbSlotList
		/// <summary>
		/// 缸槽列表
		/// </summary>
		public static readonly ListProperty<EntityList<MeterEquipAccountSlot>> PcbSlotListProperty = P<MeteringEquipmentAccount>.RegisterList(e => e.PcbSlotList);
		/// <summary>
		/// 缸槽列表
		/// </summary>
		public EntityList<MeterEquipAccountSlot> PcbSlotList
		{
			get { return this.GetLazyList(PcbSlotListProperty); }
		}
		#endregion

		#region 设备履历列表 ResumeList
		/// <summary>
		/// 设备履历列表
		/// </summary>
		[Label("设备履历列表")]
		public static readonly ListProperty<EntityList<MeterEquipAccountResume>> ResumeListProperty = P<MeteringEquipmentAccount>.RegisterList(e => e.ResumeList);

		/// <summary>
		/// 设备履历列表
		/// </summary>
		public EntityList<MeterEquipAccountResume> ResumeList
		{
			get { return this.GetLazyList(ResumeListProperty); }
		}
		#endregion

		#region 工序列表 ProcessList
		/// <summary>
		/// 工序列表
		/// </summary>
		[Label("工序列表")]
		public static readonly ListProperty<EntityList<MeterEquipAccountProcess>> ProcessListProperty = P<MeteringEquipmentAccount>.RegisterList(e => e.ProcessList);

		/// <summary>
		/// 工序列表
		/// </summary>
		public EntityList<MeterEquipAccountProcess> ProcessList
		{
			get { return this.GetLazyList(ProcessListProperty); }
		}
		#endregion

		#region 设备位置列表 EquipAccountLocationList
		/// <summary>
		/// 设备位置列表
		/// </summary>
		[Label("设备位置列表")]
		public static readonly ListProperty<EntityList<MeterEquipAccountLocation>> EquipAccountLocationListProperty = P<MeteringEquipmentAccount>.RegisterList(e => e.EquipAccountLocationList);

		/// <summary>
		/// 设备位置列表
		/// </summary>
		public EntityList<MeterEquipAccountLocation> EquipAccountLocationList
		{
			get { return this.GetLazyList(EquipAccountLocationListProperty); }
		}
		#endregion

		#region 设备物联列表 EquipAccountPhysicalUnionList
		/// <summary>
		/// 设备物联列表
		/// </summary>
		[Label("设备物联列表")]
		public static readonly ListProperty<EntityList<MeterEquipAccountPhysicalUnion>> EquipAccountPhysicalUnionListProperty = P<MeteringEquipmentAccount>.RegisterList(e => e.EquipAccountPhysicalUnionList);

		/// <summary>
		/// 设备物联列表
		/// </summary>
		public EntityList<MeterEquipAccountPhysicalUnion> EquipAccountPhysicalUnionList
		{
			get { return this.GetLazyList(EquipAccountPhysicalUnionListProperty); }
		}
        #endregion

        #region 附加列表 AttachmentList
        /// <summary>
        /// 附加列表
        /// </summary>
        public static readonly ListProperty<EntityList<MeteringEquipAccountAttachment>> AttachmentListProperty
            = P<MeteringEquipmentAccount>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 附加列表
        /// </summary>
        public EntityList<MeteringEquipAccountAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 计量设备台账 实体配置
    /// </summary>
    internal class MeteringEquipmentAccountConfig : EntityConfig<MeteringEquipmentAccount>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_EQUIP_ACCOUNT").MapAllProperties();
            Meta.Property(MeteringEquipmentAccount.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.FactoryIdProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.ManageDepartmentIdProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.UseDepartmentIdProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.EquipModelIdProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(MeteringEquipmentAccount.ReasonProperty).DontMapColumn();
            Meta.Property(MeteringEquipmentAccount.IsCalibrationProperty).DontMapColumn();
            Meta.SupportTree();
			Meta.EnablePhantoms();
		}
	}
}