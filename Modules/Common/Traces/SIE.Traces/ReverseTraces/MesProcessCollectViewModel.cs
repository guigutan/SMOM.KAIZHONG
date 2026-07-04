using SIE.Domain;
using SIE.ObjectModel;
using SIE.Traces.ForwardTraces;

namespace SIE.Traces.ReverseTraces
{

    /// <summary>
	/// 反向追溯-工序采集记录追溯
	/// </summary>
	[RootEntity, Serializable]
    [Label("工序采集记录")]
    public partial class MesProcessCollectViewModel : ViewModel
    {
        #region 工序采集Id ReportProcessId
        /// <summary>
        /// 工序采集Id
        /// </summary>
        [Label("工序采集Id")]
        public static readonly Property<double> ReportProcessIdProperty = P<MesProcessCollectViewModel>.Register(e => e.ReportProcessId);
        /// <summary>
        /// 工序采集Id
        /// </summary>
        public double ReportProcessId
        {
            get { return GetProperty(ReportProcessIdProperty); }
            set { SetProperty(ReportProcessIdProperty, value); }
        }
        #endregion

        #region 采集条码 CollectSn
        /// <summary>
        /// 采集条码
        /// </summary>
        [Label("采集条码")]
        public static readonly Property<string> CollectSnProperty = P<MesProcessCollectViewModel>.Register(e => e.CollectSn);
        /// <summary>
        /// 采集条码
        /// </summary>
        public string CollectSn
        {
            get { return GetProperty(CollectSnProperty); }
            set { SetProperty(CollectSnProperty, value); }
        }
        #endregion

        #region 状态 StateName
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> StateNameProperty = P<MesProcessCollectViewModel>.Register(e => e.StateName);
        /// <summary>
        /// 状态
        /// </summary>
        public string StateName
        {
            get { return GetProperty(StateNameProperty); }
            set { SetProperty(StateNameProperty, value); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位名称")]
        public static readonly Property<string> StationNameProperty = P<MesProcessCollectViewModel>.Register(e => e.StationName);
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
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<MesProcessCollectViewModel>.Register(e => e.ProcessName);
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 资源 ResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<MesProcessCollectViewModel>.Register(e => e.ResourceName);
        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 采集结果 Result
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        public static readonly Property<string> ResultProperty = P<MesProcessCollectViewModel>.Register(e => e.Result);

        /// <summary>
        /// 采集结果
        /// </summary>
        public string Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 采集时间 CollectTime
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateTime?> CollectTimeProperty = P<MesProcessCollectViewModel>.Register(e => e.CollectTime);
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
        public static readonly Property<string> CollectByProperty = P<MesProcessCollectViewModel>.Register(e => e.CollectBy);
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
