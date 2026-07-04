using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 周转定义
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("周转定义")]
    public partial class TurnOverDefinition : DataEntity
    {
        #region 顺序号 Sequence
        /// <summary>
        /// 顺序号
        /// </summary>
        [Label("顺序号")]
        public static readonly Property<int> SequenceProperty = P<TurnOverDefinition>.Register(e => e.Sequence);

        /// <summary>
        /// 顺序号
        /// </summary>
        public int Sequence
        {
            get { return GetProperty(SequenceProperty); }
            set { SetProperty(SequenceProperty, value); }
        }
        #endregion

        #region 上限值 UpperLimit
        /// <summary>
        /// 上限值
        /// </summary>
        [Label("上限值")]
        public static readonly Property<decimal?> UpperLimitProperty = P<TurnOverDefinition>.Register(e => e.UpperLimit);

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? UpperLimit
        {
            get { return GetProperty(UpperLimitProperty); }
            set { SetProperty(UpperLimitProperty, value); }
        }
        #endregion

        #region 下限值 LowerLimit
        /// <summary>
        /// 下限值
        /// </summary>
        [Label("下限值")]
        public static readonly Property<decimal?> LowerLimitProperty = P<TurnOverDefinition>.Register(e => e.LowerLimit);

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? LowerLimit
        {
            get { return GetProperty(LowerLimitProperty); }
            set { SetProperty(LowerLimitProperty, value); }
        }
        #endregion

        #region 上限天数 UpperLimitDay
        /// <summary>
        /// 上限天数
        /// </summary>
        [Label("上限天数")]
        public static readonly Property<decimal?> UpperLimitDayProperty = P<TurnOverDefinition>.Register(e => e.UpperLimitDay);

        /// <summary>
        /// 上限天数
        /// </summary>
        public decimal? UpperLimitDay
        {
            get { return GetProperty(UpperLimitDayProperty); }
            set { SetProperty(UpperLimitDayProperty, value); }
        }
        #endregion

        #region 下限天数 LowerLimitDay
        /// <summary>
        /// 下限天数
        /// </summary>
        [Label("下限天数")]
        public static readonly Property<decimal?> LowerLimitDayProperty = P<TurnOverDefinition>.Register(e => e.LowerLimitDay);

        /// <summary>
        /// 下限天数
        /// </summary>
        public decimal? LowerLimitDay
        {
            get { return GetProperty(LowerLimitDayProperty); }
            set { SetProperty(LowerLimitDayProperty, value); }
        }
        #endregion

        #region 字段类型 FieldType
        /// <summary>
        /// 字段类型
        /// </summary>
        [Label("字段类型")]
        public static readonly Property<DataType?> FieldTypeProperty = P<TurnOverDefinition>.Register(e => e.FieldType);

        /// <summary>
        /// 字段类型
        /// </summary>
        public DataType? FieldType
        {
            get { return GetProperty(FieldTypeProperty); }
            set { SetProperty(FieldTypeProperty, value); }
        }
        #endregion

        #region 排序字段 SortField
        /// <summary>
        /// 排序字段
        /// </summary>
        [Label("排序字段")]
        public static readonly Property<SortField?> SortFieldProperty = P<TurnOverDefinition>.Register(e => e.SortField);

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField? SortField
        {
            get { return GetProperty(SortFieldProperty); }
            set { SetProperty(SortFieldProperty, value); }
        }
        #endregion

        #region 排序方式 SortType
        /// <summary>
        /// 排序方式
        /// </summary>
        [Label("排序方式")]
        public static readonly Property<SortType?> SortTypeProperty = P<TurnOverDefinition>.Register(e => e.SortType);

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
        public static readonly Property<string> EqualValueProperty = P<TurnOverDefinition>.Register(e => e.EqualValue);

        /// <summary>
        /// 相等值
        /// </summary>
        public string EqualValue
        {
            get { return GetProperty(EqualValueProperty); }
            set { SetProperty(EqualValueProperty, value); }
        }
        #endregion

        #region 周转规则明细 TurnOverRuleDetail
        /// <summary>
        /// 周转规则明细Id
        /// </summary>
        public static readonly IRefIdProperty TurnOverRuleDetailIdProperty = P<TurnOverDefinition>.RegisterRefId(e => e.TurnOverRuleDetailId, ReferenceType.Parent);

        /// <summary>
        /// 周转规则明细Id
        /// </summary>
        public double TurnOverRuleDetailId
        {
            get { return (double)GetRefId(TurnOverRuleDetailIdProperty); }
            set { SetRefId(TurnOverRuleDetailIdProperty, value); }
        }

        /// <summary>
        /// 周转规则明细
        /// </summary>
        public static readonly RefEntityProperty<TurnOverRuleDetail> TurnOverRuleDetailProperty = P<TurnOverDefinition>.RegisterRef(e => e.TurnOverRuleDetail, TurnOverRuleDetailIdProperty);

        /// <summary>
        /// 周转规则明细
        /// </summary>
        public TurnOverRuleDetail TurnOverRuleDetail
        {
            get { return GetRefEntity(TurnOverRuleDetailProperty); }
            set { SetRefEntity(TurnOverRuleDetailProperty, value); }
        }
        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Source == ManagedPropertyChangedSource.FromUIOperating && e.NewValue != e.OldValue && e.Property == TurnOverDefinition.SortFieldProperty)
            {
                string[] strArrName = new string[] { "批次号","生产日期", "失效日期", "收货日期", "生产批次", "批次属性05"
                                                , "批次属性06", "批次属性07", "批次属性08", "批次属性09"
                                                , "批次属性10", "批次属性11", "批次属性12"};
                DataType[] arrDataType = new DataType[] { DataType.Text,DataType.Date, DataType.Date, DataType.Date,DataType.Text,DataType.Numerical,
                                                DataType.Numerical,DataType.Text,DataType.Text,DataType.Text,DataType.Text,DataType.Date,DataType.Date};

                Dictionary<string, DataType> dicSortField = new Dictionary<string, DataType>();
                for (int i = 0; i < strArrName.Length; i++)
                {
                    if (!dicSortField.ContainsKey(strArrName[i]))
                    {
                        dicSortField.Add(strArrName[i], arrDataType[i]);
                    }
                }

                if (SortField != null && SortField.HasValue && dicSortField.Count > 0)
                {
                    FieldType = dicSortField[SortField.ToLabel()];
                    if (FieldType.Value == DataType.Date)
                    {
                        LowerLimit = null;
                        UpperLimit = null;
                        EqualValue = string.Empty;
                    }
                    else if (FieldType.Value == DataType.Text)
                    {
                        LowerLimitDay = null;
                        UpperLimitDay = null;
                        LowerLimit = null;
                        UpperLimit = null;
                    }
                    else if (FieldType.Value == DataType.Numerical)
                    {
                        LowerLimitDay = null;
                        UpperLimitDay = null;
                        EqualValue = string.Empty;
                    }
                }
                else
                {
                    FieldType = null;
                    SortType = null;
                    EqualValue = string.Empty;
                    LowerLimit = null;
                    UpperLimit = null;
                    LowerLimitDay = null;
                    UpperLimitDay = null;
                }

            }
        }
    }

    /// <summary>
    /// 周转定义 实体配置
    /// </summary>
    internal class TurnOverDefinitionConfig : EntityConfig<TurnOverDefinition>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TURN_OVER_DEF").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}