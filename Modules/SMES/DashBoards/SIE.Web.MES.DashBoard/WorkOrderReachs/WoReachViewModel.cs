using SIE.Domain;
using SIE.ObjectModel;
using SIE.Web.Json;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.DashBoard.WorkOrderReachs
{
    public class ReachDataViewModel : ViewModel
    {
        #region 达成率数据 ReachData
        /// <summary>
        /// 达成率数据
        /// </summary>
        [Label("达成率数据")]
        public static readonly Property<decimal> ReachDataProperty = P<ReachDataViewModel>.Register(e => e.ReachData);

        /// <summary>
        /// 达成率数据
        /// </summary>
        public decimal ReachData
        {
            get { return this.GetProperty(ReachDataProperty); }
            set { this.SetProperty(ReachDataProperty, value); }
        }
        #endregion

        #region 是否为率 IsRate
        /// <summary>
        /// 是否为率
        /// </summary>
        [Label("是否为率")]
        public static readonly Property<bool> IsRateProperty = P<ReachDataViewModel>.Register(e => e.IsRate);

        /// <summary>
        /// 是否为率
        /// </summary>
        public bool IsRate
        {
            get { return this.GetProperty(IsRateProperty); }
            set { this.SetProperty(IsRateProperty, value); }
        }
        #endregion

        #region 采集日期 Date
        /// <summary>
        /// 采集日期
        /// </summary>
        [Label("采集日期")]
        public static readonly Property<DateTime> DateProperty = P<ReachDataViewModel>.Register(e => e.Date);

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion

        #region 工单属性 WoInfo
        /// <summary>
        /// 工单属性
        /// </summary>
        [Label("工单属性")]
        public static readonly Property<string> WoInfoProperty = P<ReachDataViewModel>.Register(e => e.WoInfo);

        /// <summary>
        /// 工单属性
        /// </summary>
        public string WoInfo
        {
            get { return this.GetProperty(WoInfoProperty); }
            set { this.SetProperty(WoInfoProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 通信数据
    /// </summary>
    public class WoReachViewModel
    {
        public List<ReachDataViewModel> ReachDataList { get; } = new List<ReachDataViewModel>();

        public List<EntityJson> ChartJsonData { get; } = new List<EntityJson>();
    }


}
