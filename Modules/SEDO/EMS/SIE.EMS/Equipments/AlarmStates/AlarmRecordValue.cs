using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.AlarmStates
{
	/// <summary>
	/// 设备报警记录值
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("设备报警记录值")]
	public partial class AlarmRecordValue : DataEntity
	{
		#region TAG全称 FullTagName
		/// <summary>
		/// TAG全称
		/// </summary>
		[Label("TAG全称")]
		public static readonly Property<string> FullTagNameProperty = P<AlarmRecordValue>.Register(e => e.FullTagName);

		/// <summary>
		/// TAG全称
		/// </summary>
		public string FullTagName
		{
			get { return GetProperty(FullTagNameProperty); }
			set { SetProperty(FullTagNameProperty, value); }
		}
		#endregion

		#region 报警值  AlarmValue
		/// <summary>
		/// 报警值
		/// </summary>
		[Required]
		[Label("报警值")]
		public static readonly Property<double> AlarmValueProperty = P<AlarmRecordValue>.Register(e => e.AlarmValue);

		/// <summary>
		/// 报警值
		/// </summary>
		public double AlarmValue
		{
			get { return GetProperty(AlarmValueProperty); }
			set { SetProperty(AlarmValueProperty, value); }
		}
		#endregion

		#region 恢复值  RecoveryValue
		/// <summary>
		/// 恢复值
		/// </summary>
		[Label("恢复值")]
		public static readonly Property<double?> RecoveryValueProperty = P<AlarmRecordValue>.Register(e => e.RecoveryValue);

		/// <summary>
		/// 恢复值
		/// </summary>
		public double? RecoveryValue
		{
			get { return GetProperty(RecoveryValueProperty); }
			set { SetProperty(RecoveryValueProperty, value); }
		}
		#endregion

		#region 设备报警记录 EquipAlarmRecord
		/// <summary>
		/// 设备报警记录Id
		/// </summary>
		[Label("设备报警记录")]
		public static readonly IRefIdProperty EquipAlarmRecordIdProperty = P<AlarmRecordValue>.RegisterRefId(e => e.EquipAlarmRecordId, ReferenceType.Parent);

		/// <summary>
		/// 设备报警记录Id
		/// </summary>
		public double EquipAlarmRecordId
		{
			get { return (double)GetRefId(EquipAlarmRecordIdProperty); }
			set { SetRefId(EquipAlarmRecordIdProperty, value); }
		}

		/// <summary>
		/// 设备报警记录
		/// </summary>
		public static readonly RefEntityProperty<EquipAlarmRecord> EquipAlarmRecordProperty = P<AlarmRecordValue>.RegisterRef(e => e.EquipAlarmRecord, EquipAlarmRecordIdProperty);

		/// <summary>
		/// 设备报警记录
		/// </summary>
		public EquipAlarmRecord EquipAlarmRecord
		{
			get { return GetRefEntity(EquipAlarmRecordProperty); }
			set { SetRefEntity(EquipAlarmRecordProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 设备报警记录值 实体配置
	/// </summary>
	internal class AlarmRecordValueConfig : EntityConfig<AlarmRecordValue>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_EQP_ALM_REC_VAL").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}