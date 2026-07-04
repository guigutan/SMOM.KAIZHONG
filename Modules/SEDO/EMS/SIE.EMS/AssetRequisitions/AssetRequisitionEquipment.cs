using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Text;

namespace SIE.EMS.AssetRequisitions
{
    /// <summary>
    /// 领用申请设备清单
    /// </summary>
    [ChildEntity, Serializable]
	[Label("领用申请设备清单")]
	public partial class AssetRequisitionEquipment : DataEntity
	{
		#region 行号 LineNo
		/// <summary>
		/// 行号
		/// </summary>
		[Label("行号")]
		public static readonly Property<string> LineNoProperty = P<AssetRequisitionEquipment>.Register(e => e.LineNo);

		/// <summary>
		/// 行号
		/// </summary>
		public string LineNo
		{
			get { return GetProperty(LineNoProperty); }
			set { SetProperty(LineNoProperty, value); }
		}
		#endregion

		#region 类型名称 EquipTypeName
		/// <summary>
		/// 类型名称
		/// </summary>
		[Label("类型名称")]
		public static readonly Property<string> EquipTypeNameProperty = P<AssetRequisitionEquipment>.Register(e => e.EquipTypeName);

		/// <summary>
		/// 类型名称
		/// </summary>
		public string EquipTypeName
		{
			get { return GetProperty(EquipTypeNameProperty); }
			set { SetProperty(EquipTypeNameProperty, value); }
		}
		#endregion

		#region 型号名称 EquipModelName
		/// <summary>
		/// 型号名称
		/// </summary>
		[Label("型号名称")]
		public static readonly Property<string> EquipModelNameProperty = P<AssetRequisitionEquipment>.Register(e => e.EquipModelName);

		/// <summary>
		/// 型号名称
		/// </summary>
		public string EquipModelName
		{
			get { return GetProperty(EquipModelNameProperty); }
			set { SetProperty(EquipModelNameProperty, value); }
		}
		#endregion

		#region 技术规格 Specifications
		/// <summary>
		/// 技术规格
		/// </summary>
		[Label("技术规格")]
		public static readonly Property<string> SpecificationsProperty = P<AssetRequisitionEquipment>.Register(e => e.Specifications);

		/// <summary>
		/// 技术规格
		/// </summary>
		public string Specifications
		{
			get { return GetProperty(SpecificationsProperty); }
			set { SetProperty(SpecificationsProperty, value); }
		}
        #endregion

		#region 设备台账 EquipAccount
		/// <summary>
		/// 设备台账Id
		/// </summary>
		[Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<AssetRequisitionEquipment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

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
            P<AssetRequisitionEquipment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 申请数量 Qty
        /// <summary>
        /// 申请数量
        /// </summary>
        [Label("申请数量")]
		[MinValue(1)]
		public static readonly Property<int> QtyProperty = P<AssetRequisitionEquipment>.Register(e => e.Qty);

		/// <summary>
		/// 申请数量
		/// </summary>
		public int Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 预估价值 EstimatedAmount
		/// <summary>
		/// 预估价值
		/// </summary>
		[Label("预估价值")]
		public static readonly Property<decimal?> EstimatedAmountProperty = P<AssetRequisitionEquipment>.Register(e => e.EstimatedAmount);

		/// <summary>
		/// 预估价值
		/// </summary>
		public decimal? EstimatedAmount
		{
			get { return GetProperty(EstimatedAmountProperty); }
			set { SetProperty(EstimatedAmountProperty, value); }
		}
		#endregion

		#region 发放数量 IssuedQty
		/// <summary>
		/// 发放数量
		/// </summary>
		[Label("发放数量")]
		public static readonly Property<int> IssuedQtyProperty = P<AssetRequisitionEquipment>.Register(e => e.IssuedQty);

		/// <summary>
		/// 发放数量
		/// </summary>
		public int IssuedQty
		{
			get { return GetProperty(IssuedQtyProperty); }
			set { SetProperty(IssuedQtyProperty, value); }
		}
		#endregion

		#region 实物归还数量 ReturnQty
		/// <summary>
		/// 实物归还数量
		/// </summary>
		[Label("实物归还数量")]
		public static readonly Property<int> ReturnQtyProperty = P<AssetRequisitionEquipment>.Register(e => e.ReturnQty);

		/// <summary>
		/// 实物归还数量
		/// </summary>
		public int ReturnQty
		{
			get { return GetProperty(ReturnQtyProperty); }
			set { SetProperty(ReturnQtyProperty, value); }
		}
		#endregion

		#region 无实物归还数量 NoGoodsReturnQty
		/// <summary>
		/// 无实物归还数量
		/// </summary>
		[Label("无实物归还数量")]
		public static readonly Property<int> NoGoodsReturnQtyProperty = P<AssetRequisitionEquipment>.Register(e => e.NoGoodsReturnQty);

		/// <summary>
		/// 无实物归还数量
		/// </summary>
		public int NoGoodsReturnQty
		{
			get { return GetProperty(NoGoodsReturnQtyProperty); }
			set { SetProperty(NoGoodsReturnQtyProperty, value); }
		}
		#endregion

		#region 位置 Location
		/// <summary>
		/// 位置
		/// </summary>
		[Label("位置")]
		public static readonly Property<string> LocationProperty = P<AssetRequisitionEquipment>.Register(e => e.Location);

		/// <summary>
		/// 位置
		/// </summary>
		public string Location
		{
			get { return GetProperty(LocationProperty); }
			set { SetProperty(LocationProperty, value); }
		}
		#endregion

		#region 拣货数量 PickedQty
		/// <summary>
		/// 拣货数量
		/// </summary>
		[Label("拣货数量")]
		public static readonly Property<int> PickedQtyProperty = P<AssetRequisitionEquipment>.Register(e => e.PickedQty);

		/// <summary>
		/// 拣货数量
		/// </summary>
		public int PickedQty
		{
			get { return GetProperty(PickedQtyProperty); }
			set { SetProperty(PickedQtyProperty, value); }
		}

		#endregion

		#region 设备型号 EquipModel
		/// <summary>
		/// 设备型号Id
		/// </summary>
		[Label("设备型号")]
		public static readonly IRefIdProperty EquipModelIdProperty = P<AssetRequisitionEquipment>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

		/// <summary>
		/// 设备型号Id
		/// </summary>
		public double? EquipModelId
		{
			get { return (double?)GetRefNullableId(EquipModelIdProperty); }
			set { SetRefNullableId(EquipModelIdProperty, value); }
		}

		/// <summary>
		/// 设备型号
		/// </summary>
		public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<AssetRequisitionEquipment>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

		/// <summary>
		/// 设备型号
		/// </summary>
		public EquipModel EquipModel
		{
			get { return GetRefEntity(EquipModelProperty); }
			set { SetRefEntity(EquipModelProperty, value); }
		}
		#endregion

		#region 设备类型 EquipType
		/// <summary>
		/// 设备类型Id
		/// </summary>
		[Label("设备类型")]
		public static readonly IRefIdProperty EquipTypeIdProperty = P<AssetRequisitionEquipment>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

		/// <summary>
		/// 设备类型Id
		/// </summary>
		public double? EquipTypeId
		{
			get { return (double?)GetRefNullableId(EquipTypeIdProperty); }
			set { SetRefNullableId(EquipTypeIdProperty, value); }
		}

		/// <summary>
		/// 设备类型
		/// </summary>
		public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<AssetRequisitionEquipment>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

		/// <summary>
		/// 设备类型
		/// </summary>
		public EquipType EquipType
		{
			get { return GetRefEntity(EquipTypeProperty); }
			set { SetRefEntity(EquipTypeProperty, value); }
		}
		#endregion

		#region 保管人 Depositary
		/// <summary>
		/// 保管人Id
		/// </summary>
		[Label("保管人")]
		public static readonly IRefIdProperty DepositaryIdProperty = P<AssetRequisitionEquipment>.RegisterRefId(e => e.DepositaryId, ReferenceType.Normal);

		/// <summary>
		/// 保管人Id
		/// </summary>
		public double? DepositaryId
		{
			get { return (double?)GetRefNullableId(DepositaryIdProperty); }
			set { SetRefNullableId(DepositaryIdProperty, value); }
		}

		/// <summary>
		/// 保管人
		/// </summary>
		public static readonly RefEntityProperty<Employee> DepositaryProperty = P<AssetRequisitionEquipment>.RegisterRef(e => e.Depositary, DepositaryIdProperty);

		/// <summary>
		/// 保管人
		/// </summary>
		public Employee Depositary
		{
			get { return GetRefEntity(DepositaryProperty); }
			set { SetRefEntity(DepositaryProperty, value); }
		}
		#endregion

		#region 产线 Resource
		/// <summary>
		/// 产线Id
		/// </summary>
		[Label("产线")]
		public static readonly IRefIdProperty ResourceIdProperty = P<AssetRequisitionEquipment>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

		/// <summary>
		/// 产线Id
		/// </summary>
		public double? ResourceId
		{
			get { return (double?)GetRefNullableId(ResourceIdProperty); }
			set { SetRefNullableId(ResourceIdProperty, value); }
		}

		/// <summary>
		/// 产线
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<AssetRequisitionEquipment>.RegisterRef(e => e.Resource, ResourceIdProperty);

		/// <summary>
		/// 产线
		/// </summary>
		public Enterprise Resource
		{
			get { return GetRefEntity(ResourceProperty); }
			set { SetRefEntity(ResourceProperty, value); }
		}
		#endregion

		#region 车间 WorkShop
		/// <summary>
		/// 车间Id
		/// </summary>
		[Label("车间")]
		public static readonly IRefIdProperty WorkShopIdProperty = P<AssetRequisitionEquipment>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

		/// <summary>
		/// 车间Id
		/// </summary>
		public double? WorkShopId
		{
			get { return (double?)GetRefNullableId(WorkShopIdProperty); }
			set { SetRefNullableId(WorkShopIdProperty, value); }
		}

		/// <summary>
		/// 车间
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<AssetRequisitionEquipment>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

		/// <summary>
		/// 车间
		/// </summary>
		public Enterprise WorkShop
		{
			get { return GetRefEntity(WorkShopProperty); }
			set { SetRefEntity(WorkShopProperty, value); }
		}
		#endregion

        #region 资产领用 AssetRequisition
        /// <summary>
        /// 资产领用Id
        /// </summary>
        [Label("资产领用")]
		public static readonly IRefIdProperty AssetRequisitionIdProperty = P<AssetRequisitionEquipment>.RegisterRefId(e => e.AssetRequisitionId, ReferenceType.Parent);

		/// <summary>
		/// 资产领用Id
		/// </summary>
		public double AssetRequisitionId
		{
			get { return (double)GetRefId(AssetRequisitionIdProperty); }
			set { SetRefId(AssetRequisitionIdProperty, value); }
		}

		/// <summary>
		/// 资产领用
		/// </summary>
		public static readonly RefEntityProperty<AssetRequisition> AssetRequisitionProperty = P<AssetRequisitionEquipment>.RegisterRef(e => e.AssetRequisition, AssetRequisitionIdProperty);

		/// <summary>
		/// 资产领用
		/// </summary>
		public AssetRequisition AssetRequisition
		{
			get { return GetRefEntity(AssetRequisitionProperty); }
			set { SetRefEntity(AssetRequisitionProperty, value); }
		}
		#endregion

		#region 视图属性

		#region 设备编码 EquipAccountCode
		/// <summary>
		/// 设备编码
		/// </summary>
		[Label("设备编码")]
		public static readonly Property<string> EquipAccountCodeProperty = P<AssetRequisitionEquipment>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

		/// <summary>
		/// 设备编码
		/// </summary>
		public string EquipAccountCode
		{
			get { return this.GetProperty(EquipAccountCodeProperty); }
			set { this.SetProperty(EquipAccountCodeProperty, value); }
		}
		#endregion

		#region 设备名称 EquipAccountName
		/// <summary>
		/// 设备名称
		/// </summary>
		[Label("设备名称")]
		public static readonly Property<string> EquipAccountNameProperty = P<AssetRequisitionEquipment>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipAccountName
		{
			get { return this.GetProperty(EquipAccountNameProperty); }
			set { this.SetProperty(EquipAccountNameProperty, value); }
		}
		#endregion

		#region 管理状态 UseState
		/// <summary>
		/// 管理状态
		/// </summary>
		[Label("管理状态")]
		public static readonly Property<AccountUseState?> UseStateProperty = P<AssetRequisitionEquipment>.RegisterView(e => e.UseState, p => p.EquipAccount.UseState);

		/// <summary>
		/// 管理状态
		/// </summary>
		public AccountUseState? UseState
		{
			get { return this.GetProperty(UseStateProperty); }
			set { this.SetProperty(UseStateProperty, value); }
		}
		#endregion

		#region 设备别名 Alias
		/// <summary>
		/// 设备别名
		/// </summary>
		[Label("设备别名")]
		public static readonly Property<string> AliasProperty = P<AssetRequisitionEquipment>.RegisterView(e => e.Alias, p => p.EquipAccount.Alias);

		/// <summary>
		/// 设备别名
		/// </summary>
		public string Alias
		{
			get { return this.GetProperty(AliasProperty); }
			set { this.SetProperty(AliasProperty, value); }
		}
		#endregion

		#region 设备型号 EquipModelCode
		/// <summary>
		/// 设备型号
		/// </summary>
		[Label("设备型号")]
		public static readonly Property<string> EquipModelCodeProperty = P<AssetRequisitionEquipment>.RegisterView(e => e.EquipModelCode, p => p.EquipModel.Code);

		/// <summary>
		/// 设备型号
		/// </summary>
		public string EquipModelCode
		{
			get { return this.GetProperty(EquipModelCodeProperty); }
			set { this.SetProperty(EquipModelCodeProperty, value); }
		}
		#endregion

		#region 设备类型 EquipTypeCode
		/// <summary>
		/// 设备类型
		/// </summary>
		[Label("设备类型")]
		public static readonly Property<string> EquipTypeCodeProperty = P<AssetRequisitionEquipment>.RegisterView(e => e.EquipTypeCode, p => p.EquipType.TypeCode);

		/// <summary>
		/// 设备类型
		/// </summary>
		public string EquipTypeCode
		{
			get { return this.GetProperty(EquipTypeCodeProperty); }
			set { this.SetProperty(EquipTypeCodeProperty, value); }
		}
		#endregion

		#region 资产领用仓库 AssetRequisitionWarehouseId
		/// <summary>
		/// 资产领用仓库
		/// </summary>
		[Label("资产领用仓库")]
		public static readonly Property<double?> AssetRequisitionWarehouseIdProperty = P<AssetRequisitionEquipment>.RegisterView(e => e.AssetRequisitionWarehouseId, p => p.AssetRequisition.WarehouseId);

		/// <summary>
		/// 资产领用仓库
		/// </summary>
		public double? AssetRequisitionWarehouseId
		{
			get { return this.GetProperty(AssetRequisitionWarehouseIdProperty); }
		}
		#endregion

		#endregion

		#region 不映射数据库的属性

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<AssetRequisitionEquipment>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<AssetRequisitionEquipment>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 借出部门 LendingDepartment
        /// <summary>
        /// 借出部门Id
        /// </summary>
        [Label("借出部门")]
		public static readonly IRefIdProperty LendingDepartmentIdProperty =
			P<AssetRequisitionEquipment>.RegisterRefId(e => e.LendingDepartmentId, ReferenceType.Normal);

		/// <summary>
		/// 借出部门Id
		/// </summary>
		public double? LendingDepartmentId
		{
			get { return (double?)this.GetRefNullableId(LendingDepartmentIdProperty); }
			set { this.SetRefNullableId(LendingDepartmentIdProperty, value); }
		}

		/// <summary>
		/// 借出部门
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> LendingDepartmentProperty =
			P<AssetRequisitionEquipment>.RegisterRef(e => e.LendingDepartment, LendingDepartmentIdProperty);

		/// <summary>
		/// 借出部门
		/// </summary>
		public Enterprise LendingDepartment
		{
			get { return this.GetRefEntity(LendingDepartmentProperty); }
			set { this.SetRefEntity(LendingDepartmentProperty, value); }
		}
		#endregion

		#endregion
	}

	/// <summary>
	/// 领用申请设备清单 实体配置
	/// </summary>
	internal class AssetRequisitionEquipmentConfig : EntityConfig<AssetRequisitionEquipment>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_REQ_EQP").MapAllProperties();
			Meta.Property(AssetRequisitionEquipment.FactoryIdProperty).DontMapColumn();
			Meta.Property(AssetRequisitionEquipment.FactoryProperty).DontMapColumn();
			Meta.Property(AssetRequisitionEquipment.LendingDepartmentIdProperty).DontMapColumn();
			Meta.Property(AssetRequisitionEquipment.LendingDepartmentProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}
	}
}