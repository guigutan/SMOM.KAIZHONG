using SIE;
using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.ViceTransfers
{
	/// <summary>
	/// 备件需求清单
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("备件需求清单")]
	public partial class ViceTransferSparePart : DataEntity
	{
		#region 行号 LineNo
		/// <summary>
		/// 行号
		/// </summary>
		[Label("行号")]
		public static readonly Property<string> LineNoProperty = P<ViceTransferSparePart>.Register(e => e.LineNo);

		/// <summary>
		/// 行号
		/// </summary>
		public string LineNo
		{
			get { return GetProperty(LineNoProperty); }
			set { SetProperty(LineNoProperty, value); }
		}
		#endregion

		#region 需求数量 Qty
		/// <summary>
		/// 需求数量
		/// </summary>
		[Label("需求数量")]
		[Required]
		public static readonly Property<decimal> QtyProperty = P<ViceTransferSparePart>.Register(e => e.Qty);

		/// <summary>
		/// 需求数量
		/// </summary>
		public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 调拨数量 TransferQty
		/// <summary>
		/// 调拨数量
		/// </summary>
		[Label("调拨数量")]
		public static readonly Property<decimal> TransferQtyProperty = P<ViceTransferSparePart>.Register(e => e.TransferQty);

		/// <summary>
		/// 调拨数量
		/// </summary>
		public decimal TransferQty
		{
			get { return GetProperty(TransferQtyProperty); }
			set { SetProperty(TransferQtyProperty, value); }
		}
		#endregion

		#region 备件 SparePart
		/// <summary>
		/// 备件Id
		/// </summary>
		[Label("备件编码")]
		public static readonly IRefIdProperty SparePartIdProperty = P<ViceTransferSparePart>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<SparePart> SparePartProperty = P<ViceTransferSparePart>.RegisterRef(e => e.SparePart, SparePartIdProperty);

		/// <summary>
		/// 备件
		/// </summary>
		public SparePart SparePart
		{
			get { return GetRefEntity(SparePartProperty); }
			set { SetRefEntity(SparePartProperty, value); }
		}
		#endregion

		#region 备件需求清单 ViceTransfer
		/// <summary>
		/// 备件需求清单Id
		/// </summary>
		[Label("备件需求清单")]
		public static readonly IRefIdProperty ViceTransferIdProperty = P<ViceTransferSparePart>.RegisterRefId(e => e.ViceTransferId, ReferenceType.Parent);

		/// <summary>
		/// 备件需求清单Id
		/// </summary>
		public double ViceTransferId
		{
			get { return (double)GetRefId(ViceTransferIdProperty); }
			set { SetRefId(ViceTransferIdProperty, value); }
		}

		/// <summary>
		/// 备件需求清单
		/// </summary>
		public static readonly RefEntityProperty<ViceTransfer> ViceTransferProperty = P<ViceTransferSparePart>.RegisterRef(e => e.ViceTransfer, ViceTransferIdProperty);

		/// <summary>
		/// 备件需求清单
		/// </summary>
		public ViceTransfer ViceTransfer
		{
			get { return GetRefEntity(ViceTransferProperty); }
			set { SetRefEntity(ViceTransferProperty, value); }
		}
		#endregion


		#region 质量状态 QualityStatus
		/// <summary>
		/// 质量状态
		/// </summary>
		[Label("质量状态")]
		public static readonly Property<QualityStatus> QualityStatusProperty = P<ViceTransferSparePart>.Register(e => e.QualityStatus);

		/// <summary>
		/// 质量状态
		/// </summary>
		public QualityStatus QualityStatus
		{
			get { return GetProperty(QualityStatusProperty); }
			set { SetProperty(QualityStatusProperty, value); }
		}
		#endregion


		#region 仓库库存 WhInventory
		/// <summary>
		/// 仓库库存(不映射数据库字段)
		/// </summary>
		[Label("仓库库存")]
        public static readonly Property<decimal> WhInventoryProperty = P<ViceTransferSparePart>.Register(e => e.WhInventory);

		/// <summary>
		/// 仓库库存
		/// </summary>
		public decimal WhInventory
		{
            get { return this.GetProperty(WhInventoryProperty); }
            set { this.SetProperty(WhInventoryProperty, value); }
        }
        #endregion


        #region 视图属性
        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
		public static readonly Property<string> SparePartCodeProperty = P<ViceTransferSparePart>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

		/// <summary>
		/// 备件编码
		/// </summary>
		public string SparePartCode
		{
			get { return this.GetProperty(SparePartCodeProperty); }
		}
		#endregion

		#region 备件名称 SparePartName
		/// <summary>
		/// 备件名称
		/// </summary>
		[Label("备件名称")]
		public static readonly Property<string> SparePartNameProperty = P<ViceTransferSparePart>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

		/// <summary>
		/// 备件编码
		/// </summary>
		public string SparePartName
		{
			get { return this.GetProperty(SparePartNameProperty); }
		}
		#endregion

		#region 规格型号 Specification
		/// <summary>
		/// 规格型号
		/// </summary>
		[Label("规格型号")]
		public static readonly Property<string> SpecificationProperty = P<ViceTransferSparePart>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

		/// <summary>
		/// 规格型号
		/// </summary>
		public string Specification
		{
			get { return this.GetProperty(SpecificationProperty); }
		}
		#endregion

		#region 类型 SparePartType
		/// <summary>
		/// 类型
		/// </summary>
		[Label("类型")]
		public static readonly Property<SparePartType> SparePartTypeProperty = P<ViceTransferSparePart>.RegisterView(e => e.SparePartType, p => p.SparePart.SpartType);

		/// <summary>
		/// 类型
		/// </summary>
		public SparePartType SparePartType
		{
			get { return this.GetProperty(SparePartTypeProperty); }
		}
		#endregion

		#region 管控方式 ControlMethod
		/// <summary>
		/// 管控方式
		/// </summary>
		[Label("管控方式")]
		public static readonly Property<ControlMethod> ControlMethodProperty = P<ViceTransferSparePart>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

		/// <summary>
		/// 管控方式
		/// </summary>
		public ControlMethod ControlMethod
		{
			get { return this.GetProperty(ControlMethodProperty); }
		}
		#endregion

		#region 单位 UnitName
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
		public static readonly Property<string> UnitNameProperty = P<ViceTransferSparePart>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

		/// <summary>
		/// 单位
		/// </summary>
		public string UnitName
		{
			get { return this.GetProperty(UnitNameProperty); }
		}
		#endregion


		#endregion


		#region 来源仓库Id	 WarehouseId
		/// <summary>
		/// 来源仓库Id
		/// </summary>
		[Label("来源仓库Id")]
        public static readonly Property<double> WarehouseIdProperty = P<ViceTransferSparePart>.RegisterView(e => e.WarehouseId, p => p.ViceTransfer.WarehouseId);

		/// <summary>
		/// 来源仓库Id
		/// </summary>
		public double WarehouseId
		{
            get { return this.GetProperty(WarehouseIdProperty); }
        }
		#endregion

		#region 目标仓库Id	 TargetWarehouseId
		/// <summary>
		/// 目标仓库Id
		/// </summary>
		[Label("目标仓库Id")]
		public static readonly Property<double> TargetWarehouseIdProperty = P<ViceTransferSparePart>.RegisterView(e => e.TargetWarehouseId, p => p.ViceTransfer.TargetWarehouseId);

		/// <summary>
		/// 目标仓库Id
		/// </summary>
		public double TargetWarehouseId
		{
			get { return this.GetProperty(TargetWarehouseIdProperty); }
		}
		#endregion
	}

	/// <summary>
	/// 备件需求清单 实体配置
	/// </summary>
	internal class ViceTransferSparePartConfig : EntityConfig<ViceTransferSparePart>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_VICE_TRAN_SP").MapAllPropertiesExcept(ViceTransferSparePart.WhInventoryProperty);
			Meta.EnablePhantoms();
		}
	}
}