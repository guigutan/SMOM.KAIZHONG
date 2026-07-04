using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Traces.ReverseTraces
{

    /// <summary>
	/// 反向追溯-工序采集-关键件记录追溯
	/// </summary>
	[RootEntity, Serializable]
    [Label("关键件")]
    public partial class MesProcessCollectKeyItemViewModel : ViewModel
    {
        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<MesProcessCollectKeyItemViewModel>.Register(e => e.ItemId);
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
        public static readonly Property<string> ItemCodeProperty = P<MesProcessCollectKeyItemViewModel>.Register(e => e.ItemCode);
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
        public static readonly Property<string> ItemNameProperty = P<MesProcessCollectKeyItemViewModel>.Register(e => e.ItemName);
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 来源条码 SourceCode
        /// <summary>
        /// 来源条码
        /// </summary>
        [Label("来源条码")]
        public static readonly Property<string> SourceCodeProperty = P<MesProcessCollectKeyItemViewModel>.Register(e => e.SourceCode);
        /// <summary>
        /// 来源条码
        /// </summary>
        public string SourceCode
        {
            get { return GetProperty(SourceCodeProperty); }
            set { SetProperty(SourceCodeProperty, value); }
        }
        #endregion

        #region 用料数 Qty
        /// <summary>
        /// 用料数
        /// </summary>
        [Label("用料数")]
        public static readonly Property<decimal?> QtyProperty = P<MesProcessCollectKeyItemViewModel>.Register(e => e.Qty);
        /// <summary>
        /// 用料数
        /// </summary>
        public decimal? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MesProcessCollectKeyItemViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion       
    }

}
