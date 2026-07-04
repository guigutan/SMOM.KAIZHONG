using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Outsourcing
{
	/// <summary>
	/// 在制品委外出库
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("在制品委外出库")]
	public partial class ProcessingOutbound : DataEntity
	{
		#region 来源Id SourceId
		/// <summary>
		/// 来源Id
		/// </summary>
		[Label("来源Id")]
		public static readonly Property<double> SourceIdProperty = P<ProcessingOutbound>.Register(e => e.SourceId);

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
		public static readonly Property<string> SNProperty = P<ProcessingOutbound>.Register(e => e.SN);

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
		public static readonly Property<string> LotNoProperty = P<ProcessingOutbound>.Register(e => e.LotNo);

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
		public static readonly Property<decimal> QtyProperty = P<ProcessingOutbound>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 需求单 OutsourcingRequest
		/// <summary>
		/// 需求单Id
		/// </summary>
		[Label("需求单")]
		public static readonly IRefIdProperty OutsourcingRequestIdProperty = P<ProcessingOutbound>.RegisterRefId(e => e.OutsourcingRequestId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<OutsourcingRequest> OutsourcingRequestProperty = P<ProcessingOutbound>.RegisterRef(e => e.OutsourcingRequest, OutsourcingRequestIdProperty);

		/// <summary>
		/// 需求单
		/// </summary>
		public OutsourcingRequest OutsourcingRequest
		{
			get { return GetRefEntity(OutsourcingRequestProperty); }
			set { SetRefEntity(OutsourcingRequestProperty, value); }
		}
		#endregion

		#region 状态 State
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<OutsourcingDetailState> StateProperty = P<ProcessingOutbound>.Register(e => e.State);

		/// <summary>
		/// 状态
		/// </summary>
		public OutsourcingDetailState State
		{
			get { return GetProperty(StateProperty); }
			set { SetProperty(StateProperty, value); }
		}
		#endregion

		#region 需求单号 NO
		/// <summary>
		/// 需求单号
		/// </summary>
		[Label("需求单号")]
		public static readonly Property<string> NOProperty = P<ProcessingOutbound>.RegisterView(e => e.NO, p => p.OutsourcingRequest.NO);

		/// <summary>
		/// 需求单号
		/// </summary>
		public string NO
		{
			get { return this.GetProperty(NOProperty); }
		}
        #endregion

        #region 是否上传 IsUpload
        /// <summary>
        /// 是否上传
        /// </summary>
        [Label("是否上传")]
        public static readonly Property<bool?> IsUploadProperty = P<ProcessingOutbound>.Register(e => e.IsUpload);

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
		public static readonly Property<double?> OldIdProperty = P<ProcessingOutbound>.Register(e => e.OldId);

		/// <summary>
		/// 原来Id
		/// </summary>
		public double? OldId
		{
			get { return this.GetProperty(OldIdProperty); }
			set { this.SetProperty(OldIdProperty, value); }
		}
        #endregion

        #region 是否确认 IsConfirm
        /// <summary>
        /// 是否确认
        /// </summary>
        [Label("是否确认")]
		public static readonly Property<bool?> IsConfirmProperty = P<ProcessingOutbound>.Register(e => e.IsConfirm);

		/// <summary>
		/// 是否确认
		/// </summary>
		public bool? IsConfirm
        {
			get { return this.GetProperty(IsConfirmProperty); }
			set { this.SetProperty(IsConfirmProperty, value); }
		}
        #endregion

        #region 重传次数 ReLoadCount
        /// <summary>
        /// 重传次数
        /// </summary>
        [Label("重传次数")]
        public static readonly Property<int?> ReLoadCountProperty = P<ProcessingOutbound>.Register(e => e.ReLoadCount);

        /// <summary>
        /// 重传次数
        /// </summary>
        public int? ReLoadCount
        {
            get { return this.GetProperty(ReLoadCountProperty); }
            set { this.SetProperty(ReLoadCountProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
		public static readonly Property<string> WorkOrderNoProperty = P<ProcessingOutbound>.RegisterView(e => e.WorkOrderNo, p => p.OutsourcingRequest.WorkOrder.No);

		/// <summary>
		/// 工单号
		/// </summary>
		public string WorkOrderNo
		{
			get { return this.GetProperty(WorkOrderNoProperty); }
		}
		#endregion

		#region 产品编码 ProductCode
		/// <summary>
		/// 产品编码
		/// </summary>
		[Label("产品编码")]
		public static readonly Property<string> ProductCodeProperty = P<ProcessingOutbound>.RegisterView(e => e.ProductCode, p => p.OutsourcingRequest.WorkOrder.Product.Code);

		/// <summary>
		/// 产品编码
		/// </summary>
		public string ProductCode
		{
			get { return this.GetProperty(ProductCodeProperty); }
		}
		#endregion

		#region 产品名称 ProductName
		/// <summary>
		/// 产品名称
		/// </summary>
		[Label("产品名称")]
		public static readonly Property<string> ProductNameProperty = P<ProcessingOutbound>.RegisterView(e => e.ProductName, p => p.OutsourcingRequest.WorkOrder.Product.Name);

		/// <summary>
		/// 产品名称
		/// </summary>
		public string ProductName
		{
			get { return this.GetProperty(ProductNameProperty); }
		}
		#endregion

		#region 产品旧料号 ShortDescription
		/// <summary>
		/// 产品旧料号
		/// </summary>
		[Label("产品旧料号")]
		public static readonly Property<string> ShortDescriptionProperty = P<ProcessingOutbound>.RegisterView(e => e.ShortDescription, p => p.OutsourcingRequest.WorkOrder.Product.ShortDescription);

		/// <summary>
		/// 产品旧料号
		/// </summary>
		public string ShortDescription
        {
			get { return this.GetProperty(ShortDescriptionProperty); }
		}
        #endregion

        #region 起始工序 BeginProcessName
        /// <summary>
        /// 起始工序
        /// </summary>
        [Label("起始工序")]
		public static readonly Property<string> BeginProcessNameProperty = P<ProcessingOutbound>.RegisterView(e => e.BeginProcessName, p => p.OutsourcingRequest.BeginProcess.Process.Name);

        /// <summary>
        /// 起始工序
        /// </summary>
        public string BeginProcessName
        {
			get { return this.GetProperty(BeginProcessNameProperty); }
		}
		#endregion

		#region 委外工厂 OutFactory
		/// <summary>
		/// 委外工厂
		/// </summary>
		[Label("委外工厂")]
		public static readonly Property<string> OutFactoryProperty = P<ProcessingOutbound>.RegisterView(e => e.OutFactory, p => p.OutsourcingRequest.OutFactory);

		/// <summary>
		/// 委外工厂
		/// </summary>
		public string OutFactory
        {
			get { return this.GetProperty(OutFactoryProperty); }
		}
		#endregion

		#region 发起工厂 InitiatorFactory
		/// <summary>
		/// 发起工厂
		/// </summary>
		[Label("发起工厂")]
		public static readonly Property<string> InitiatorFactoryProperty = P<ProcessingOutbound>.RegisterView(e => e.InitiatorFactory, p => p.OutsourcingRequest.InitiatorFactory);

		/// <summary>
		/// 发起工厂
		/// </summary>
		public string InitiatorFactory
        {
			get { return this.GetProperty(InitiatorFactoryProperty); }
		}
		#endregion


		#endregion
	}

    /// <summary>
    /// 在制品委外出库 实体配置
    /// </summary>
    internal class ProcessingOutsourcingOutboundConfig : EntityConfig<ProcessingOutbound>
	{
        protected override void AddValidations(IValidationDeclarer rules)
        {
			rules.AddRule(new NotDuplicateRule
			{
				Properties = {
				ProcessingOutbound.SNProperty,
				ProcessingOutbound.OutsourcingRequestIdProperty
				},
				MessageBuilder = (e) =>
				{
					return "已存在相同产品条码".L10N();
				}
			});
            rules.AddRule(new NotDuplicateRule
            {
                Properties = {
                ProcessingOutbound.LotNoProperty,
                ProcessingOutbound.OutsourcingRequestIdProperty
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
			Meta.MapTable("PROC_OUT_OUTBOUND").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}