using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// 设备综合统计
    /// </summary>
    [RootEntity, Serializable]
    public class EquipmentIntegrateStatisticViewModel : ViewModel
    {
        #region 序号 IndexSeq
        /// <summary>
        /// 序号
        /// </summary>
        [Label("序号")]
        public static readonly Property<int> IndexSeqProperty
            = P<EquipmentIntegrateStatisticViewModel>.Register(e => e.IndexSeq);

        /// <summary>
        /// 序号
        /// </summary>
        public int IndexSeq
        {
            get { return this.GetProperty(IndexSeqProperty); }
            set { this.SetProperty(IndexSeqProperty, value); }
        }
        #endregion


        #region 日期 StatisticDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<string> StatisticDateProperty
            = P<EquipmentIntegrateStatisticViewModel>.Register(e => e.StatisticDate);

        /// <summary>
        /// 日期
        /// </summary>
        public string StatisticDate
        {
            get { return this.GetProperty(StatisticDateProperty); }
            set { this.SetProperty(StatisticDateProperty, value); }
        }
        #endregion

        #region 值 Value
        /// <summary>
        /// 值
        /// </summary>
        [Label("值")]
        public static readonly Property<string> ValueProperty = P<EquipmentIntegrateStatisticViewModel>.Register(e => e.Value);

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return this.GetProperty(ValueProperty); }
            set { this.SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 标题 ValueTitle
        /// <summary>
        /// 标题
        /// </summary>
        [Label("标题")]
        public static readonly Property<string> ValueTitleProperty = P<EquipmentIntegrateStatisticViewModel>.Register(e => e.ValueTitle);

        /// <summary>
        /// 标题
        /// </summary>
        public string ValueTitle
        {
            get { return this.GetProperty(ValueTitleProperty); }
            set { this.SetProperty(ValueTitleProperty, value); }
        }
        #endregion

    }
}
