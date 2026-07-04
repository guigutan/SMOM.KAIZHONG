using SIE;
using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.OutDepotHandovers
{
	/// <summary>
	/// 备件交接明细
	/// </summary>
	[ChildEntity, Serializable]
	[Label("备件交接明细")]
	public partial class OutDepotHandoverDetail : DataEntity
	{
		#region 备件交接单 OutDepotHandover
		/// <summary>
		/// 备件交接单Id
		/// </summary>
		public static readonly IRefIdProperty OutDepotHandoverIdProperty = P<OutDepotHandoverDetail>.RegisterRefId(e => e.OutDepotHandoverId, ReferenceType.Parent);

		/// <summary>
		/// 备件交接单Id
		/// </summary>
		public double OutDepotHandoverId
		{
			get { return (double)GetRefId(OutDepotHandoverIdProperty); }
			set { SetRefId(OutDepotHandoverIdProperty, value); }
		}

		/// <summary>
		/// 备件交接单
		/// </summary>
		public static readonly RefEntityProperty<OutDepotHandover> OutDepotHandoverProperty = P<OutDepotHandoverDetail>.RegisterRef(e => e.OutDepotHandover, OutDepotHandoverIdProperty);

		/// <summary>
		/// 备件交接单
		/// </summary>
		public OutDepotHandover OutDepotHandover
		{
			get { return GetRefEntity(OutDepotHandoverProperty); }
			set { SetRefEntity(OutDepotHandoverProperty, value); }
		}
		#endregion

		#region 备件基础数据 SparePart
		/// <summary>
		/// 备件基础数据Id
		/// </summary>
		public static readonly IRefIdProperty SparePartIdProperty = P<OutDepotHandoverDetail>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

		/// <summary>
		/// 备件基础数据Id
		/// </summary>
		public double SparePartId
		{
			get { return (double)GetRefId(SparePartIdProperty); }
			set { SetRefId(SparePartIdProperty, value); }
		}

		/// <summary>
		/// 备件基础数据
		/// </summary>
		public static readonly RefEntityProperty<SparePart> SparePartProperty = P<OutDepotHandoverDetail>.RegisterRef(e => e.SparePart, SparePartIdProperty);

		/// <summary>
		/// 备件基础数据
		/// </summary>
		public SparePart SparePart
		{
			get { return GetRefEntity(SparePartProperty); }
			set { SetRefEntity(SparePartProperty, value); }
		}
		#endregion

		#region 批次号 BatchNo
		/// <summary>
		/// 批次号
		/// </summary>
		[Label("批次号")]
		public static readonly Property<string> BatchNoProperty = P<OutDepotHandoverDetail>.Register(e => e.BatchNo);

		/// <summary>
		/// 批次号
		/// </summary>
		public string BatchNo
		{
			get { return this.GetProperty(BatchNoProperty); }
			set { this.SetProperty(BatchNoProperty, value); }
		}
		#endregion

		#region 序列号 SeriaNo
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("序列号")]
		public static readonly Property<string> SeriaNoProperty = P<OutDepotHandoverDetail>.Register(e => e.SeriaNo);

		/// <summary>
		/// 序列号
		/// </summary>
		public string SeriaNo
		{
			get { return this.GetProperty(SeriaNoProperty); }
			set { this.SetProperty(SeriaNoProperty, value); }
		}
		#endregion

		#region 发料数 Qty
		/// <summary>
		/// 发料数
		/// </summary>
		[Label("发料数")]
		public static readonly Property<int> QtyProperty = P<OutDepotHandoverDetail>.Register(e => e.Qty);

		/// <summary>
		/// 发料数
		/// </summary>
		public int Qty
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
		public static readonly Property<int> ReceiveQtyProperty = P<OutDepotHandoverDetail>.Register(e => e.ReceiveQty);

		/// <summary>
		/// 已接收数
		/// </summary>
		public int ReceiveQty
		{
			get { return GetProperty(ReceiveQtyProperty); }
			set { SetProperty(ReceiveQtyProperty, value); }
		}
		#endregion

		#region 状态 HandOverStatus
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<HandOverStatus> HandOverStatusProperty = P<OutDepotHandoverDetail>.Register(e => e.HandOverStatus);

		/// <summary>
		/// 状态
		/// </summary>
		public HandOverStatus HandOverStatus
		{
			get { return GetProperty(HandOverStatusProperty); }
			set { SetProperty(HandOverStatusProperty, value); }
		}
		#endregion

		#region 视图属性

		#region 交接单号 HandoverNo
		/// <summary>
		/// 交接单号
		/// </summary>
		[Label("交接单号")]
        public static readonly Property<string> HandoverNoProperty = P<OutDepotHandoverDetail>.RegisterView(e => e.HandoverNo, p => p.OutDepotHandover.HandoverNo);

		/// <summary>
		/// 交接单号
		/// </summary>
		public string HandoverNo
        {
            get { return this.GetProperty(HandoverNoProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<OutDepotHandoverDetail>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

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
        public static readonly Property<string> SparePartNameProperty = P<OutDepotHandoverDetail>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
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
		public static readonly Property<string> SpecificationProperty = P<OutDepotHandoverDetail>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

		/// <summary>
		/// 规格型号
		/// </summary>
		public string Specification
		{
			get { return GetProperty(SpecificationProperty); }
		}
		#endregion

		#region 类型 SpartType
		/// <summary>
		/// 类型
		/// </summary>
		[Label("类型")]
		public static readonly Property<SparePartType> SpartTypeProperty = P<OutDepotHandoverDetail>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

		/// <summary>
		/// 类型
		/// </summary>
		public SparePartType SpartType
		{
			get { return this.GetProperty(SpartTypeProperty); }
		}
		#endregion

		#region 管控方式 ControlMethod
		/// <summary>
		/// 管控方式
		/// </summary>
		[Label("管控方式")]
		public static readonly Property<ControlMethod> ControlMethodProperty = P<OutDepotHandoverDetail>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

		/// <summary>
		/// 管控方式
		/// </summary>
		public ControlMethod ControlMethod
		{
			get { return this.GetProperty(ControlMethodProperty); }
		}
		#endregion

		#region 以旧换新 IsReplacement
		/// <summary>
		/// 以旧换新
		/// </summary>
		[Label("以旧换新")]
		public static readonly Property<bool> IsReplacementProperty = P<OutDepotHandoverDetail>.RegisterView(e => e.IsReplacement, p => p.SparePart.IsReplacement);

		/// <summary>
		/// 以旧换新
		/// </summary>
		public bool IsReplacement
		{
			get { return this.GetProperty(IsReplacementProperty); }
		}
		#endregion

		#endregion

	}

	/// <summary>
	/// 备件出库交接明细 实体配置
	/// </summary>
	internal class OutDepotHandoverDetailConfig : EntityConfig<OutDepotHandoverDetail>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_SP_HANDOVER_DTL").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}