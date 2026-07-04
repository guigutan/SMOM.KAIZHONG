using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.Traces.ForwardTraces
{
    /// <summary>
    /// 过程追溯
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("过程信息追溯")]
	public partial class MesFwdTraceViewModel : ViewModel
    {

        #region 生产通用报表主表Id WipProductVersionId
        /// <summary>
        /// 生产通用报表主表Id
        /// </summary>
        public static readonly Property<double> WipProductVersionIdProperty = P<MesFwdTraceViewModel>.Register(e => e.WipProductVersionId);
        /// <summary>
        /// 生产通用报表主表Id
        /// </summary>
        public double WipProductVersionId
        {
            get { return GetProperty(WipProductVersionIdProperty); }
            set { SetProperty(WipProductVersionIdProperty, value); }
        }
        #endregion

        #region 关联产品Id RelatedProductId
        /// <summary>
        /// 关联产品Id
        /// </summary>
        public static readonly Property<double> RelatedProductIdProperty = P<MesFwdTraceViewModel>.Register(e => e.RelatedProductId);
        /// <summary>
        /// 关联产品Id
        /// </summary>
        public double RelatedProductId
        {
            get { return GetProperty(RelatedProductIdProperty); }
            set { SetProperty(RelatedProductIdProperty, value); }
        }
        #endregion

        #region 关联产品条码 RelatedProductSn
        /// <summary>
        /// 关联产品条码
        /// </summary>
        [Label("关联产品条码")]
		public static readonly Property<string> RelatedProductSnProperty = P<MesFwdTraceViewModel>.Register(e => e.RelatedProductSn);

		/// <summary>
		/// 关联产品条码
		/// </summary>
		public string RelatedProductSn
		{
			get { return GetProperty(RelatedProductSnProperty); }
			set { SetProperty(RelatedProductSnProperty, value); }
		}
		#endregion

		#region 关联产品批次 RelatedProductLot
		/// <summary>
		/// 关联产品批次
		/// </summary>
		[Label("关联产品批次")]
		public static readonly Property<string> RelatedProductLotProperty = P<MesFwdTraceViewModel>.Register(e => e.RelatedProductLot);

		/// <summary>
		/// 关联产品批次
		/// </summary>
		public string RelatedProductLot
		{
			get { return GetProperty(RelatedProductLotProperty); }
			set { SetProperty(RelatedProductLotProperty, value); }
		}
		#endregion

		#region 关联产品编码 RelatedProductCode
		/// <summary>
		/// 关联产品编码
		/// </summary>
		[Label("关联产品编码")]
		public static readonly Property<string> RelatedProductCodeProperty = P<MesFwdTraceViewModel>.Register(e => e.RelatedProductCode);

		/// <summary>
		/// 关联产品编码
		/// </summary>
		public string RelatedProductCode
		{
			get { return GetProperty(RelatedProductCodeProperty); }
			set { SetProperty(RelatedProductCodeProperty, value); }
		}
		#endregion

		#region 关联产品名称 RelatedProductName
		/// <summary>
		/// 关联产品名称
		/// </summary>
		[Label("关联产品名称")]
		public static readonly Property<string> RelatedProductNameProperty = P<MesFwdTraceViewModel>.Register(e => e.RelatedProductName);

		/// <summary>
		/// 关联产品名称
		/// </summary>
		public string RelatedProductName
		{
			get { return GetProperty(RelatedProductNameProperty); }
			set { SetProperty(RelatedProductNameProperty, value); }
		}
		#endregion
			
		#region 关联工单号 RelatedWorkOrderNo
		/// <summary>
		/// 关联工单号
		/// </summary>
		[Label("关联工单号")]
		public static readonly Property<string> RelatedWorkOrderNoProperty = P<MesFwdTraceViewModel>.Register(e => e.RelatedWorkOrderNo);

		/// <summary>
		/// 关联工单号
		/// </summary>
		public string RelatedWorkOrderNo
		{
			get { return GetProperty(RelatedWorkOrderNoProperty); }
			set { SetProperty(RelatedWorkOrderNoProperty, value); }
		}
        #endregion

        #region 关联工单Id RelatedWorkOrderId
        /// <summary>
        /// 关联工单Id
        /// </summary>
        [Label("关联工单Id")]
        public static readonly Property<double> RelatedWorkOrderIdProperty = P<MesFwdTraceViewModel>.Register(e => e.RelatedWorkOrderId);

        /// <summary>
        /// 关联工单Id
        /// </summary>
        public double RelatedWorkOrderId
        {
            get { return GetProperty(RelatedWorkOrderIdProperty); }
            set { SetProperty(RelatedWorkOrderIdProperty, value); }
        }
        #endregion

        #region 产品扩展属性 ProductExtProp
        /// <summary>
        /// 产品扩展属性
        /// </summary>
        [Label("产品扩展属性")]
        public static readonly Property<string> ProductExtPropProperty = P<MesFwdTraceViewModel>.Register(e => e.ProductExtProp);

        /// <summary>
        /// 产品扩展属性
        /// </summary>
        public string ProductExtProp
        {
            get { return GetProperty(ProductExtPropProperty); }
            set { SetProperty(ProductExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MesFwdTraceViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly Property<double> ItemIdProperty = P<MesFwdTraceViewModel>.Register(e => e.ItemId);
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return GetProperty(ItemIdProperty); }
            set { SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 物料来源条码 ItemSourceCode
        /// <summary>
        /// 物料来源条码
        /// </summary>
        [Label("物料来源条码")]
        public static readonly Property<string> ItemSourceCodeProperty = P<MesFwdTraceViewModel>.Register(e => e.ItemSourceCode);

        /// <summary>
        /// 物料来源条码
        /// </summary>
        public string ItemSourceCode
        {
            get { return GetProperty(ItemSourceCodeProperty); }
            set { SetProperty(ItemSourceCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  配置
    /// </summary>
    public class MesFwdTraceViewModelConfig : EntityConfig<MesFwdTraceViewModel>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.SupportTree();
        }
    }
}