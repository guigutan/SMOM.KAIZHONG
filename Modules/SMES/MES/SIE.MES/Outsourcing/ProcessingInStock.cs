using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Outsourcing
{
	/// <summary>
	/// 在制品委外入库
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("在制品委外入库")]
	public partial class ProcessingInStock : DataEntity
	{
        #region 来源Id SourceId
        /// <summary>
        /// 来源Id
        /// </summary>
        [Label("来源Id")]
        public static readonly Property<double> SourceIdProperty = P<ProcessingInStock>.Register(e => e.SourceId);

        /// <summary>
        /// 来源Id
        /// </summary>
        public double SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 产品条码 SN
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
		public static readonly Property<string> SNProperty = P<ProcessingInStock>.Register(e => e.SN);

		/// <summary>
		/// 产品条码
		/// </summary>
		public string SN
		{
			get { return GetProperty(SNProperty); }
			set { SetProperty(SNProperty, value); }
		}
		#endregion

		#region 批次号 LotNo
		/// <summary>
		/// 批次号
		/// </summary>
		[Label("批次号")]
		public static readonly Property<string> LotNoProperty = P<ProcessingInStock>.Register(e => e.LotNo);

		/// <summary>
		/// 批次号
		/// </summary>
		public string LotNo
		{
			get { return GetProperty(LotNoProperty); }
			set { SetProperty(LotNoProperty, value); }
		}
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
		public static readonly Property<decimal> QtyProperty = P<ProcessingInStock>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 合格数 PassQty
		/// <summary>
		/// 合格数
		/// </summary>
		[Label("合格数")]
		public static readonly Property<decimal> PassQtyProperty = P<ProcessingInStock>.Register(e => e.PassQty);

		/// <summary>
		/// 合格数
		/// </summary>
		public decimal PassQty
		{
			get { return GetProperty(PassQtyProperty); }
			set { SetProperty(PassQtyProperty, value); }
		}
		#endregion

		#region 不合格数 NgQty
		/// <summary>
		/// 不合格数
		/// </summary>
		[Label("不合格数")]
		public static readonly Property<decimal> NgQtyProperty = P<ProcessingInStock>.Register(e => e.NgQty);

		/// <summary>
		/// 不合格数
		/// </summary>
		public decimal NgQty
		{
			get { return GetProperty(NgQtyProperty); }
			set { SetProperty(NgQtyProperty, value); }
		}
		#endregion

		#region 状态 State
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<OutsourcingDetailState> StateProperty = P<ProcessingInStock>.Register(e => e.State);

		/// <summary>
		/// 状态
		/// </summary>
		public OutsourcingDetailState State
		{
			get { return GetProperty(StateProperty); }
			set { SetProperty(StateProperty, value); }
		}
		#endregion

		#region 委外出库 Outbound
		/// <summary>
		/// 委外出库Id
		/// </summary>
		[Label("委外出库")]
		public static readonly IRefIdProperty OutboundIdProperty =
			P<ProcessingInStock>.RegisterRefId(e => e.OutboundId, ReferenceType.Normal);

		/// <summary>
		/// 委外出库Id
		/// </summary>
		public double? OutboundId
		{
			get { return (double?)this.GetRefNullableId(OutboundIdProperty); }
			set { this.SetRefNullableId(OutboundIdProperty, value); }
		}

		/// <summary>
		/// 委外出库
		/// </summary>
		public static readonly RefEntityProperty<ProcessingOutbound> OutboundProperty =
			P<ProcessingInStock>.RegisterRef(e => e.Outbound, OutboundIdProperty);

		/// <summary>
		/// 委外出库
		/// </summary>
		public ProcessingOutbound Outbound
		{
			get { return this.GetRefEntity(OutboundProperty); }
			set { this.SetRefEntity(OutboundProperty, value); }
		}
		#endregion

		#region 是否上传 IsUpload
		/// <summary>
		/// 是否上传
		/// </summary>
		[Label("是否上传事务交易")]
		public static readonly Property<bool?> IsUploadProperty = P<ProcessingInStock>.Register(e => e.IsUpload);

		/// <summary>
		/// 是否上传
		/// </summary>
		public bool? IsUpload
		{
			get { return this.GetProperty(IsUploadProperty); }
			set { this.SetProperty(IsUploadProperty, value); }
		}
        #endregion

        #region 原来Id OldId
        /// <summary>
        /// 原来Id
        /// </summary>
        [Label("原来Id")]
        public static readonly Property<double?> OldIdProperty = P<ProcessingInStock>.Register(e => e.OldId);

        /// <summary>
        /// 原来Id
        /// </summary>
        public double? OldId
        {
            get { return this.GetProperty(OldIdProperty); }
            set { this.SetProperty(OldIdProperty, value); }
        }
        #endregion

        #region 类型 ProcessingType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
		public static readonly Property<ProcessingType> ProcessingTypeProperty = P<ProcessingInStock>.Register(e => e.ProcessingType);

		/// <summary>
		/// 类型
		/// </summary>
		public ProcessingType ProcessingType
        {
			get { return this.GetProperty(ProcessingTypeProperty); }
			set { this.SetProperty(ProcessingTypeProperty, value); }
		}
        #endregion

        #region 重传次数 ReLoadCount
        /// <summary>
        /// 重传次数
        /// </summary>
        [Label("重传次数")]
        public static readonly Property<int?> ReLoadCountProperty = P<ProcessingInStock>.Register(e => e.ReLoadCount);

        /// <summary>
        /// 重传次数
        /// </summary>
        public int? ReLoadCount
        {
            get { return this.GetProperty(ReLoadCountProperty); }
            set { this.SetProperty(ReLoadCountProperty, value); }
        }
        #endregion



        #region 需求单 OutsourcingRequest
        /// <summary>
        /// 需求单Id
        /// </summary>
        [Label("需求单")]
		public static readonly IRefIdProperty OutsourcingRequestIdProperty = P<ProcessingInStock>.RegisterRefId(e => e.OutsourcingRequestId, ReferenceType.Parent);

		/// <summary>
		/// 需求单Id
		/// </summary>
		public double OutsourcingRequestId
		{
			get { return (double)GetRefId(OutsourcingRequestIdProperty); }
			set { SetRefId(OutsourcingRequestIdProperty, value); }
		}

		/// <summary>
		/// 需求单
		/// </summary>
		public static readonly RefEntityProperty<OutsourcingRequest> OutsourcingRequestProperty = P<ProcessingInStock>.RegisterRef(e => e.OutsourcingRequest, OutsourcingRequestIdProperty);

		/// <summary>
		/// 需求单
		/// </summary>
		public OutsourcingRequest OutsourcingRequest
		{
			get { return GetRefEntity(OutsourcingRequestProperty); }
			set { SetRefEntity(OutsourcingRequestProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 在制品委外入库 实体配置
	/// </summary>
	internal class ProcessingInStockConfig : EntityConfig<ProcessingInStock>
	{
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule
            {
                Properties = {
                ProcessingInStock.SNProperty,
                ProcessingInStock.OutsourcingRequestIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同产品条码".L10N();
                }
            });
            rules.AddRule(new NotDuplicateRule
            {
                Properties = {
                ProcessingInStock.LotNoProperty,
                ProcessingInStock.OutsourcingRequestIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同批次号".L10N();
                }
            });
            base.AddValidations(rules);
        }

		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("PROC_OUT_INSTOCK").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}