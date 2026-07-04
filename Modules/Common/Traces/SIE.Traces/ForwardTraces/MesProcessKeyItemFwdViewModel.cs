using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Traces.ForwardTraces
{

    /// <summary>
	/// 工序采集记录追溯
	/// </summary>
	[RootEntity, Serializable]
    [Label("工序采集记录")]
    public partial class MesProcessKeyItemFwdViewModel : ViewModel
    {

        #region 采集条码 CollectSn
        /// <summary>
        /// 采集条码
        /// </summary>
        [Label("采集条码")]
        public static readonly Property<string> CollectSnProperty = P<MesProcessKeyItemFwdViewModel>.Register(e => e.CollectSn);
        /// <summary>
        /// 采集条码
        /// </summary>
        public string CollectSn
        {
            get { return GetProperty(CollectSnProperty); }
            set { SetProperty(CollectSnProperty, value); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("采集工位")]
        public static readonly Property<string> StationNameProperty = P<MesProcessKeyItemFwdViewModel>.Register(e => e.StationName);
        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return GetProperty(StationNameProperty); }
            set { SetProperty(StationNameProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("采集工序")]
        public static readonly Property<string> ProcessNameProperty = P<MesProcessKeyItemFwdViewModel>.Register(e => e.ProcessName);
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 使用数量 Qty
        /// <summary>
        /// 使用数量
        /// </summary>
        [Label("使用数量")]
        public static readonly Property<decimal> QtyProperty = P<MesProcessKeyItemFwdViewModel>.Register(e => e.Qty);
        /// <summary>
        /// 使用数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 采集时间 CollectTime
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateTime?> CollectTimeProperty = P<MesProcessKeyItemFwdViewModel>.Register(e => e.CollectTime);
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? CollectTime
        {
            get { return GetProperty(CollectTimeProperty); }
            set { SetProperty(CollectTimeProperty, value); }
        }
        #endregion

        #region 操作人 CollectBy
        /// <summary>
        /// 操作人
        /// </summary>
        [Label("操作人")]
        public static readonly Property<string> CollectByProperty = P<MesProcessKeyItemFwdViewModel>.Register(e => e.CollectBy);
        /// <summary>
        /// 操作人
        /// </summary>
        public string CollectBy
        {
            get { return GetProperty(CollectByProperty); }
            set { SetProperty(CollectByProperty, value); }
        }
        #endregion
    }

}
