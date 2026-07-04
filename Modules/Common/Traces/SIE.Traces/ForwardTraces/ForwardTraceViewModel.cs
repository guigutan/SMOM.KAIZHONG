using SIE.Domain;
using SIE.EventMessages.Common.Traces;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.Traces.ForwardTraces
{
    /// <summary>
    /// 正向追溯
    /// </summary>
    [RootEntity, Serializable]
	[ConditionQueryType(typeof(ForwardTraceViewModelCriteria))]
	public partial class ForwardTraceViewModel : ViewModel
	{
        #region 追溯类型 TraceType
        /// <summary>
        /// 追溯类型
        /// </summary>
        public static readonly Property<TraceType> TraceTypeProperty = P<ForwardTraceViewModel>.Register(e => e.TraceType);
        /// <summary>
        /// 追溯类型
        /// </summary>
        public TraceType TraceType
        {
            get { return GetProperty(TraceTypeProperty); }
            set { SetProperty(TraceTypeProperty, value); }
        }
        #endregion       

        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<ForwardTraceViewModel>.Register(e => e.ItemId);
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return GetProperty(ItemIdProperty); }
            set { SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
		public static readonly Property<string> ItemCodeProperty = P<ForwardTraceViewModel>.Register(e => e.ItemCode);
		/// <summary>
		/// 物料编码
		/// </summary>
		public string ItemCode
		{
			get { return GetProperty(ItemCodeProperty); }
			set { SetProperty(ItemCodeProperty, value); }
		}
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ForwardTraceViewModel>.Register(e => e.ItemName);
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
		{
			get { return GetProperty(ItemNameProperty); }
			set { SetProperty(ItemNameProperty, value); }
		}
        #endregion

        #region 物料批次 ItemLot
        /// <summary>
        /// 物料批次
        /// </summary>
        [Label("物料批次")]
        public static readonly Property<string> ItemLotProperty = P<ForwardTraceViewModel>.Register(e => e.ItemLot);
        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemLot
        {
			get { return GetProperty(ItemLotProperty); }
			set { SetProperty(ItemLotProperty, value); }
		}
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<ForwardTraceViewModel>.Register(e => e.ItemExtPropName);
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion
        
        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<ForwardTraceViewModel>.Register(e => e.Sn);
		/// <summary>
		/// 序列号
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
        #endregion
	}
}