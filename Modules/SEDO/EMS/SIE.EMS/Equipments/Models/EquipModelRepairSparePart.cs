using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using SIE.EMS.SpareParts;

namespace SIE.EMS.Equipments.Models
{
	/// <summary>
	/// 维修备件
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("维修备件")]
	public partial class EquipModelRepairSparePart : DataEntity
	{
		#region 数量 Qty
		/// <summary>
		/// 数量
		/// </summary>
		[Label("数量")]
		public static readonly Property<decimal> QtyProperty = P<EquipModelRepairSparePart>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 备件 SparePart
		/// <summary>
		/// 备件Id
		/// </summary>
		[Label("备件")]
		public static readonly IRefIdProperty SparePartIdProperty = P<EquipModelRepairSparePart>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

		/// <summary>
		/// 备件Id
		/// </summary>
		public double SparePartId
		{
			get { return (double)GetRefId(SparePartIdProperty); }
			set { SetRefId(SparePartIdProperty, value); }
		}

		/// <summary>
		/// 备件
		/// </summary>
		public static readonly RefEntityProperty<SparePart> SparePartProperty = P<EquipModelRepairSparePart>.RegisterRef(e => e.SparePart, SparePartIdProperty);

		/// <summary>
		/// 备件
		/// </summary>
		public SparePart SparePart
		{
			get { return GetRefEntity(SparePartProperty); }
			set { SetRefEntity(SparePartProperty, value); }
		}
		#endregion

		#region 设备型号维修项目 EquipModelRepairProject
		/// <summary>
		/// 设备型号维修项目Id
		/// </summary>
		[Label("设备型号维修项目")]
		public static readonly IRefIdProperty EquipModelRepairProjectIdProperty = P<EquipModelRepairSparePart>.RegisterRefId(e => e.EquipModelRepairProjectId, ReferenceType.Parent);

		/// <summary>
		/// 设备型号维修项目Id
		/// </summary>
		public double EquipModelRepairProjectId
		{
			get { return (double)GetRefId(EquipModelRepairProjectIdProperty); }
			set { SetRefId(EquipModelRepairProjectIdProperty, value); }
		}

		/// <summary>
		/// 设备型号维修项目
		/// </summary>
		public static readonly RefEntityProperty<EquipModelRepairProject> EquipModelRepairProjectProperty = P<EquipModelRepairSparePart>.RegisterRef(e => e.EquipModelRepairProject, EquipModelRepairProjectIdProperty);

		/// <summary>
		/// 设备型号维修项目
		/// </summary>
		public EquipModelRepairProject EquipModelRepairProject
		{
			get { return GetRefEntity(EquipModelRepairProjectProperty); }
			set { SetRefEntity(EquipModelRepairProjectProperty, value); }
		}
		#endregion

		#region 备件名称 SparePartName
		/// <summary>
		/// 备件名称
		/// </summary>
		[Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<EquipModelRepairSparePart>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 注释
        /// </summary>
        public string SparePartName
		{
            get { return this.GetProperty(SparePartNameProperty); }
        }
		#endregion

		#region 单位 UnitName
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<EquipModelRepairSparePart>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

		/// <summary>
		/// 单位
		/// </summary>
		public string UnitName
		{
            get { return this.GetProperty(UnitNameProperty); }
        }
		#endregion

		#region 规格型号 Specification
		/// <summary>
		/// 规格型号
		/// </summary>
		[Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<EquipModelRepairSparePart>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

		/// <summary>
		/// 规格型号
		/// </summary>
		public string Specification
		{
            get { return this.GetProperty(SpecificationProperty); }
        }
		#endregion
		

    }

	/// <summary>
	/// 维修备件 实体配置
	/// </summary>
	internal class EquipModelRepairSparePartConfig : EntityConfig<EquipModelRepairSparePart>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_MODEL_REPA_PRJ_SP").MapAllProperties();

			Meta.EnablePhantoms();
		}
	}
}