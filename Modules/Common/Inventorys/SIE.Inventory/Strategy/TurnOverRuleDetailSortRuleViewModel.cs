using SIE.Domain;
using SIE.ObjectModel;
using SIE.Inventory.Commom;
using System;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 周转规则明细排序字段
    /// </summary>
    [RootEntity, Serializable]
    [Label("排序规则")]
    public class TurnOverRuleDetailSortRuleViewModel : ViewModel
    {
        #region 排序名称 SortName
        /// <summary>
        /// 排序名称
        /// </summary>
        [Label("排序名称")]
        public static readonly Property<string> SortNameProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.SortName);

        /// <summary>
        /// 排序名称
        /// </summary>
        public string SortName
        {
            get { return this.GetProperty(SortNameProperty); }
            set { this.SetProperty(SortNameProperty, value); }
        }
        #endregion

        #region 排序字段 SortField
        /// <summary>
        /// 排序字段
        /// </summary>
        [Label("排序字段")]
        public static readonly Property<SortField?> SortFieldProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.SortField);

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField? SortField
        {
            get { return GetProperty(SortFieldProperty); }
            set { SetProperty(SortFieldProperty, value); }
        }
        #endregion

        #region 字段类型 FieldType
        /// <summary>
        /// 字段类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<DataType?> FieldTypeProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.FieldType);

        /// <summary>
        /// 字段类型
        /// </summary>
        public DataType? FieldType
        {
            get { return GetProperty(FieldTypeProperty); }
            set { SetProperty(FieldTypeProperty, value); }
        }
        #endregion

        #region 排序方式 SortType
        /// <summary>
        /// 排序方式
        /// </summary>
        [Label("排序方式")]
        public static readonly Property<SortType?> SortTypeProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.SortType);

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType? SortType
        {
            get { return GetProperty(SortTypeProperty); }
            set { SetProperty(SortTypeProperty, value); }
        }
        #endregion

        #region 相等值 EqualValue
        /// <summary>
        /// 相等值
        /// </summary>
        [Label("相等值")]
        public static readonly Property<string> EqualValueProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.EqualValue);

        /// <summary>
        /// 相等值
        /// </summary>
        public string EqualValue
        {
            get { return GetProperty(EqualValueProperty); }
            set { SetProperty(EqualValueProperty, value); }
        }
        #endregion

        #region 下限值 LowerLimit
        /// <summary>
        /// 下限值
        /// </summary>
        [Label("下限值")]
        public static readonly Property<decimal?> LowerLimitProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.LowerLimit);

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? LowerLimit
        {
            get { return GetProperty(LowerLimitProperty); }
            set { SetProperty(LowerLimitProperty, value); }
        }
        #endregion

        #region 上限值 UpperLimit
        /// <summary>
        /// 上限值
        /// </summary>
        [Label("上限值")]
        public static readonly Property<decimal?> UpperLimitProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.UpperLimit);

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? UpperLimit
        {
            get { return GetProperty(UpperLimitProperty); }
            set { SetProperty(UpperLimitProperty, value); }
        }
        #endregion

        #region 下限天数 LowerLimitDay
        /// <summary>
        /// 下限天数
        /// </summary>
        [Label("下限天数")]
        public static readonly Property<decimal?> LowerLimitDayProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.LowerLimitDay);

        /// <summary>
        /// 下限天数
        /// </summary>
        public decimal? LowerLimitDay
        {
            get { return GetProperty(LowerLimitDayProperty); }
            set { SetProperty(LowerLimitDayProperty, value); }
        }
        #endregion

        #region 上限天数 UpperLimitDay
        /// <summary>
        /// 上限天数
        /// </summary>
        [Label("上限天数")]
        public static readonly Property<decimal?> UpperLimitDayProperty = P<TurnOverRuleDetailSortRuleViewModel>.Register(e => e.UpperLimitDay);

        /// <summary>
        /// 上限天数
        /// </summary>
        public decimal? UpperLimitDay
        {
            get { return GetProperty(UpperLimitDayProperty); }
            set { SetProperty(UpperLimitDayProperty, value); }
        }
        #endregion        
    }
}
