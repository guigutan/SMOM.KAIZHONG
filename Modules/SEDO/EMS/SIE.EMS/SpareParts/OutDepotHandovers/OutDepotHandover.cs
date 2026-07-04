using SIE;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.OutDepotHandovers
{
	/// <summary>
	/// 备件交接单
	/// </summary>
	[RootEntity, Serializable]
	[ConditionQueryType(typeof(OutDepotHandoverCriteria))]
	[EntityWithConfig(typeof(NoConfig), "备件交接单单号生成规则配置项", "备件交接单单号生成规则")]
	[Label("备件交接单")]
	public partial class OutDepotHandover : DataEntity
	{
		#region 交接单号 HandoverNo
		/// <summary>
		/// 交接单号
		/// </summary>
		[Label("交接单号")]
		public static readonly Property<string> HandoverNoProperty = P<OutDepotHandover>.Register(e => e.HandoverNo);

		/// <summary>
		/// 交接单号
		/// </summary>
		public string HandoverNo
		{
			get { return GetProperty(HandoverNoProperty); }
			set { SetProperty(HandoverNoProperty, value); }
		}
		#endregion

		#region 备件出库单 OutDepot
		/// <summary>
		/// 备件出库单Id
		/// </summary>
		public static readonly IRefIdProperty OutDepotIdProperty = P<OutDepotHandover>.RegisterRefId(e => e.OutDepotId, ReferenceType.Normal);

		/// <summary>
		/// 备件出库单Id
		/// </summary>
		public double OutDepotId
		{
			get { return (double)GetRefId(OutDepotIdProperty); }
			set { SetRefId(OutDepotIdProperty, value); }
		}

		/// <summary>
		/// 备件出库单
		/// </summary>
		public static readonly RefEntityProperty<OutDepot> OutDepotProperty = P<OutDepotHandover>.RegisterRef(e => e.OutDepot, OutDepotIdProperty);

		/// <summary>
		/// 备件出库单
		/// </summary>
		public OutDepot OutDepot
		{
			get { return GetRefEntity(OutDepotProperty); }
			set { SetRefEntity(OutDepotProperty, value); }
		}
		#endregion

		#region 出库日期 OutDepotDate
		/// <summary>
		/// 出库日期
		/// </summary>
		[Label("出库日期")]
        public static readonly Property<DateTime> OutDepotDateProperty = P<OutDepotHandover>.Register(e => e.OutDepotDate);

        /// <summary>
        /// 出库日期
        /// </summary>
        public DateTime OutDepotDate
        {
            get { return this.GetProperty(OutDepotDateProperty); }
            set { this.SetProperty(OutDepotDateProperty, value); }
        }
		#endregion

		#region 交接状态 HandOverStatus
		/// <summary>
		/// 交接状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<HandOverStatus> HandOverStatusProperty = P<OutDepotHandover>.Register(e => e.HandOverStatus);

		/// <summary>
		/// 交接状态
		/// </summary>
		public HandOverStatus HandOverStatus
		{
			get { return GetProperty(HandOverStatusProperty); }
			set { SetProperty(HandOverStatusProperty, value); }
		}
		#endregion

		#region 备件交接明细 OutDepotHandoverDetailList
		/// <summary>
		/// 备件交接明细
		/// </summary>
		[Label("接收明细")]
		public static readonly ListProperty<EntityList<OutDepotHandoverDetail>> OutDepotHandoverDetailListProperty = P<OutDepotHandover>.RegisterList(e => e.OutDepotHandoverDetailList);

		/// <summary>
		/// 备件交接明细
		/// </summary>
		public EntityList<OutDepotHandoverDetail> OutDepotHandoverDetailList
		{
			get { return this.GetLazyList(OutDepotHandoverDetailListProperty); }
		}
		#endregion

		#region 视图属性

		#region 备件出库单号 OutDepotNo
		/// <summary>
		/// 备件出库单号
		/// </summary>
		[Label("备件出库单号")]
        public static readonly Property<string> OutDepotNoProperty = P<OutDepotHandover>.RegisterView(e => e.OutDepotNo, p => p.OutDepot.No);

        /// <summary>
        /// 备件出库单号
        /// </summary>
        public string OutDepotNo
		{
            get { return this.GetProperty(OutDepotNoProperty); }
        }
		#endregion

		#region 领用部门 GetDepartmentName
		/// <summary>
		/// 领用部门
		/// </summary>
		[Label("领用部门")]
        public static readonly Property<string> GetDepartmentNameProperty = P<OutDepotHandover>.RegisterView(e => e.GetDepartmentName, p => p.OutDepot.GetDepartment.Name);

        /// <summary>
        /// 领用部门
        /// </summary>
        public string GetDepartmentName
		{
            get { return this.GetProperty(GetDepartmentNameProperty); }
        }
		#endregion

		#endregion

		#region 不映射数据库的属性

		#region 接收明细查询关键字 HandoverDetailKeyWord
		/// <summary>
		/// 接收明细查询关键字
		/// </summary>
		[Label("接收明细查询关键字")]
		public static readonly Property<string> HandoverDetailKeyWordProperty = P<OutDepotHandover>.Register(e => e.HandoverDetailKeyWord);

		/// <summary>
		/// 接收明细查询关键字
		/// </summary>
		public string HandoverDetailKeyWord
		{
			get { return this.GetProperty(HandoverDetailKeyWordProperty); }
			set { this.SetProperty(HandoverDetailKeyWordProperty, value); }
		}
		#endregion

		#region 扫描值 ScanValue
		/// <summary>
		/// 扫描值
		/// </summary>
		[Label("扫描值")]
		public static readonly Property<string> ScanValueProperty = P<OutDepotHandover>.Register(e => e.ScanValue);

		/// <summary>
		/// 扫描值
		/// </summary>
		public string ScanValue
		{
			get { return this.GetProperty(ScanValueProperty); }
			set { this.SetProperty(ScanValueProperty, value); }
		}
		#endregion

		#region 提示消息 Message
		/// <summary>
		/// 提示消息
		/// </summary>
		[Label("提示消息")]
		public static readonly Property<string> MessageProperty = P<OutDepotHandover>.Register(e => e.Message);

		/// <summary>
		/// 提示消息
		/// </summary>
		public string Message
		{
			get { return this.GetProperty(MessageProperty); }
			set { this.SetProperty(MessageProperty, value); }
		}
		#endregion

		#region 备件交接单 OutDepotHandoverBill
		/// <summary>
		/// 备件交接单Id
		/// </summary>
		[Label("备件交接单")]
        public static readonly IRefIdProperty OutDepotHandoverBillIdProperty =
            P<OutDepotHandover>.RegisterRefId(e => e.OutDepotHandoverBillId, ReferenceType.Normal);

		/// <summary>
		/// 备件交接单Id
		/// </summary>
		public double? OutDepotHandoverBillId
        {
            get { return (double?)this.GetRefNullableId(OutDepotHandoverBillIdProperty); }
            set { this.SetRefNullableId(OutDepotHandoverBillIdProperty, value); }
        }

		/// <summary>
		/// 备件交接单
		/// </summary>
		public static readonly RefEntityProperty<OutDepotHandover> OutDepotHandoverBillProperty =
            P<OutDepotHandover>.RegisterRef(e => e.OutDepotHandoverBill, OutDepotHandoverBillIdProperty);

		/// <summary>
		/// 备件交接单
		/// </summary>
		public OutDepotHandover OutDepotHandoverBill
        {
            get { return this.GetRefEntity(OutDepotHandoverBillProperty); }
            set { this.SetRefEntity(OutDepotHandoverBillProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<OutDepotHandover>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)this.GetRefNullableId(SparePartIdProperty); }
            set { this.SetRefNullableId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<OutDepotHandover>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

		#region 备件编码 SparePartCode
		/// <summary>
		/// 备件编码
		/// </summary>
		[Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<OutDepotHandover>.Register(e => e.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
		{
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
		public static readonly Property<string> SparePartNameProperty = P<OutDepotHandover>.Register(e => e.SparePartName);

		/// <summary>
		/// 备件名称
		/// </summary>
		public string SparePartName
		{
			get { return this.GetProperty(SparePartNameProperty); }
			set { this.SetProperty(SparePartNameProperty, value); }
		}
		#endregion

		#region 管控方式 ControlMethod
		/// <summary>
		/// 管控方式
		/// </summary>
		[Label("管控方式")]
		public static readonly Property<ControlMethod> ControlMethodProperty = P<OutDepotHandover>.Register(e => e.ControlMethod);

		/// <summary>
		/// 管控方式
		/// </summary>
		public ControlMethod ControlMethod
		{
			get { return this.GetProperty(ControlMethodProperty); }
			set { this.SetProperty(ControlMethodProperty, value); }
		}
		#endregion

		#region 发料数 Qty
		/// <summary>
		/// 发料数
		/// </summary>
		[Label("发料数")]
		public static readonly Property<int?> QtyProperty = P<OutDepotHandover>.Register(e => e.Qty);

		/// <summary>
		/// 发料数
		/// </summary>
		public int? Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 已接收数 ReceiveQty
		/// <summary>
		/// 已接收数
		/// </summary>
		[Label("已接收数")]
		public static readonly Property<int?> ReceiveQtyProperty = P<OutDepotHandover>.Register(e => e.ReceiveQty);

		/// <summary>
		/// 已接收数
		/// </summary>
		public int? ReceiveQty
		{
			get { return GetProperty(ReceiveQtyProperty); }
			set { SetProperty(ReceiveQtyProperty, value); }
		}
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("扫描条码")]
        public static readonly Property<string> BarcodeProperty = P<OutDepotHandover>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 是否手动选择备件 IsSelectSparePart
        /// <summary>
        /// 是否手动选择备件
        /// </summary>
        [Label("是否手动选择备件")]
        public static readonly Property<bool> IsSelectSparePartProperty = P<OutDepotHandover>.Register(e => e.IsSelectSparePart);

        /// <summary>
        /// 是否手动选择备件
        /// </summary>
        public bool IsSelectSparePart
        {
            get { return this.GetProperty(IsSelectSparePartProperty); }
            set { this.SetProperty(IsSelectSparePartProperty, value); }
        }
        #endregion


        #endregion
    }

	/// <summary>
	/// 备件出库交接 实体配置
	/// </summary>
	internal class OutDepotHandoverConfig : EntityConfig<OutDepotHandover>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_SP_HANDOVER").MapAllProperties();
			Meta.Property(OutDepotHandover.HandoverDetailKeyWordProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.ScanValueProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.MessageProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.OutDepotHandoverBillIdProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.OutDepotHandoverBillProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.SparePartIdProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.SparePartProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.SparePartCodeProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.SparePartNameProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.ControlMethodProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.QtyProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.ReceiveQtyProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.BarcodeProperty).DontMapColumn();
			Meta.Property(OutDepotHandover.IsSelectSparePartProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}
	}
}