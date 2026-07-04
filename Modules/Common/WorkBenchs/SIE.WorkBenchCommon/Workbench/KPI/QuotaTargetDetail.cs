using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 目标值设定
    /// </summary>
    [ChildEntity, Serializable]
    [Label("目标值设定")]
    public class QuotaTargetDetail : DataEntity, IStateEntity
    {
        #region 年 Year
        /// <summary>
        /// 年
        /// </summary>
        [Label("年")]
        public static readonly Property<int?> YearProperty = P<QuotaTargetDetail>.Register(e => e.Year);

        /// <summary>
        /// 年
        /// </summary>
        public int? Year
        {
            get { return GetProperty(YearProperty); }
            set { SetProperty(YearProperty, value); }
        }
        #endregion

        #region 月 Month
        /// <summary>
        /// 月
        /// </summary>
        [Label("月")]
        public static readonly Property<int?> MonthProperty = P<QuotaTargetDetail>.Register(e => e.Month);

        /// <summary>
        /// 月
        /// </summary>
        public int? Month
        {
            get { return GetProperty(MonthProperty); }
            set { SetProperty(MonthProperty, value); }
        }
        #endregion

        #region 周 Week
        /// <summary>
        /// 周
        /// </summary>
        [Label("周")]
        public static readonly Property<int?> WeekProperty = P<QuotaTargetDetail>.Register(e => e.Week);

        /// <summary>
        /// 周
        /// </summary>
        public int? Week
        {
            get { return GetProperty(WeekProperty); }
            set { SetProperty(WeekProperty, value); }
        }
        #endregion

        #region 目标值 Target
        /// <summary>
        /// 目标值
        /// </summary>
        [Label("目标值")]
        public static readonly Property<decimal> TargetProperty = P<QuotaTargetDetail>.Register(e => e.Target);

        /// <summary>
        /// 目标值
        /// </summary>
        public decimal Target
        {
            get { return GetProperty(TargetProperty); }
            set { SetProperty(TargetProperty, value); }
        }
        #endregion

        #region 实际值 Actual
        /// <summary>
        /// 实际值
        /// </summary>
        [Label("目标值")]
        public static readonly Property<decimal> ActualProperty = P<QuotaTargetDetail>.Register(e => e.Actual);

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal Actual
        {
            get { return GetProperty(ActualProperty); }
            set { SetProperty(ActualProperty, value); }
        }
        #endregion

        #region 绩效运算符 KpiOperators
        /// <summary>
        /// 绩效运算符
        /// </summary>
        [Label("绩效运算符")]
        public static readonly Property<KpiOperators> KpiOperatorsProperty = P<QuotaTargetDetail>.Register(e => e.KpiOperators);

        /// <summary>
        /// 绩效运算符
        /// </summary>
        public KpiOperators KpiOperators
        {
            get { return GetProperty(KpiOperatorsProperty); }
            set { SetProperty(KpiOperatorsProperty, value); }
        }
        #endregion

        #region 周期类型 DataType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<DateType> DataTypeProperty = P<QuotaTargetDetail>.Register(e => e.DataType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public DateType DataType
        {
            get { return GetProperty(DataTypeProperty); }
            set { SetProperty(DataTypeProperty, value); }
        }
        #endregion

        #region KPI目标设定 QuotaTargetSetting
        /// <summary>
        /// KPI目标设定Id
        /// </summary>
        public static readonly IRefIdProperty QuotaTargetSettingIdProperty = P<QuotaTargetDetail>.RegisterRefId(e => e.QuotaTargetSettingId, ReferenceType.Parent);

        /// <summary>
        /// KPI目标设定Id
        /// </summary>
        public double QuotaTargetSettingId
        {
            get { return (double)GetRefId(QuotaTargetSettingIdProperty); }
            set { SetRefId(QuotaTargetSettingIdProperty, value); }
        }

        /// <summary>
        /// KPI目标设定
        /// </summary>
        public static readonly RefEntityProperty<QuotaTargetSetting> QuotaTargetSettingProperty = P<QuotaTargetDetail>.RegisterRef(e => e.QuotaTargetSetting, QuotaTargetSettingIdProperty);

        /// <summary>
        /// KPI目标设定
        /// </summary>
        public QuotaTargetSetting QuotaTargetSetting
        {
            get { return GetRefEntity(QuotaTargetSettingProperty); }
            set { SetRefEntity(QuotaTargetSettingProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<QuotaTargetDetail>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 目标值格式 QuotaTargetSettingValueType
        /// <summary>
        /// 目标值格式
        /// </summary>
        [Label("目标值格式")]
        public static readonly Property<string> QuotaTargetSettingValueTypeProperty = P<QuotaTargetDetail>.RegisterView(e => e.QuotaTargetSettingValueType, p => p.QuotaTargetSetting.ValueType);

        /// <summary>
        /// 目标值格式
        /// </summary>
        public string QuotaTargetSettingValueType
        {
            get { return this.GetProperty(QuotaTargetSettingValueTypeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 目标值设定实体配置
    /// </summary>
    internal class QuotaTargetDetailConfig : EntityConfig<QuotaTargetDetail>
    {
        /// <summary>
        /// 数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QUOTA_TARGET_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
