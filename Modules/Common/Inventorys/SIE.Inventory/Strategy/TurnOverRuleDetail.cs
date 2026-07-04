using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Inventory.Transactions;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 周转规则明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("周转规则明细")]
    public partial class TurnOverRuleDetail : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<TurnOverRuleDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 周转定义列表 DefinitionList
        /// <summary>
        /// 周转定义列表
        /// </summary>
        public static readonly ListProperty<EntityList<TurnOverDefinition>> DefinitionListProperty = P<TurnOverRuleDetail>.RegisterList(e => e.DefinitionList);
        /// <summary>
        /// 周转定义列表
        /// </summary>
        public EntityList<TurnOverDefinition> DefinitionList
        {
            get { return this.GetLazyList(DefinitionListProperty); }
        }
        #endregion

        #region 排序字段 SortField1
        /// <summary>
        /// 排序字段
        /// </summary>
        [Label("排序字段1")]
        public static readonly Property<SortField?> SortField1Property = P<TurnOverRuleDetail>.Register(e => e.SortField1);

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField? SortField1
        {
            get { return GetProperty(SortField1Property); }
            set { SetProperty(SortField1Property, value); }
        }
        #endregion

        #region 字段类型 FieldType1
        /// <summary>
        /// 字段类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<DataType?> FieldType1Property = P<TurnOverRuleDetail>.Register(e => e.FieldType1);

        /// <summary>
        /// 字段类型
        /// </summary>
        public DataType? FieldType1
        {
            get { return GetProperty(FieldType1Property); }
            set { SetProperty(FieldType1Property, value); }
        }
        #endregion

        #region 排序方式 SortType1
        /// <summary>
        /// 排序方式
        /// </summary>
        [Label("排序方式")]
        public static readonly Property<SortType?> SortType1Property = P<TurnOverRuleDetail>.Register(e => e.SortType1);

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType? SortType1
        {
            get { return GetProperty(SortType1Property); }
            set { SetProperty(SortType1Property, value); }
        }
        #endregion

        #region 相等值 EqualValue1
        /// <summary>
        /// 相等值
        /// </summary>
        [Label("相等值")]
        public static readonly Property<string> EqualValue1Property = P<TurnOverRuleDetail>.Register(e => e.EqualValue1);

        /// <summary>
        /// 相等值
        /// </summary>
        public string EqualValue1
        {
            get { return GetProperty(EqualValue1Property); }
            set { SetProperty(EqualValue1Property, value); }
        }
        #endregion

        #region 下限值 LowerLimit1
        /// <summary>
        /// 下限值
        /// </summary>
        [Label("下限值")]
        public static readonly Property<decimal?> LowerLimit1Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimit1);

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? LowerLimit1
        {
            get { return GetProperty(LowerLimit1Property); }
            set { SetProperty(LowerLimit1Property, value); }
        }
        #endregion

        #region 上限值 UpperLimit1
        /// <summary>
        /// 上限值
        /// </summary>
        [Label("上限值")]
        public static readonly Property<decimal?> UpperLimit1Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimit1);

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? UpperLimit1
        {
            get { return GetProperty(UpperLimit1Property); }
            set { SetProperty(UpperLimit1Property, value); }
        }
        #endregion

        #region 下限天数 LowerLimitDay1
        /// <summary>
        /// 下限天数
        /// </summary>
        [Label("下限天数")]
        public static readonly Property<decimal?> LowerLimitDay1Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimitDay1);

        /// <summary>
        /// 下限天数
        /// </summary>
        public decimal? LowerLimitDay1
        {
            get { return GetProperty(LowerLimitDay1Property); }
            set { SetProperty(LowerLimitDay1Property, value); }
        }
        #endregion

        #region 上限天数 UpperLimitDay1
        /// <summary>
        /// 上限天数
        /// </summary>
        [Label("上限天数")]
        public static readonly Property<decimal?> UpperLimitDay1Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimitDay1);

        /// <summary>
        /// 上限天数
        /// </summary>
        public decimal? UpperLimitDay1
        {
            get { return GetProperty(UpperLimitDay1Property); }
            set { SetProperty(UpperLimitDay1Property, value); }
        }
        #endregion
        
        #region 排序字段 SortField2
        /// <summary>
        /// 排序字段
        /// </summary>
        [Label("排序字段2")]
        public static readonly Property<SortField?> SortField2Property = P<TurnOverRuleDetail>.Register(e => e.SortField2);

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField? SortField2
        {
            get { return GetProperty(SortField2Property); }
            set { SetProperty(SortField2Property, value); }
        }
        #endregion

        #region 相等值 EqualValue2
        /// <summary>
        /// 相等值
        /// </summary>
        [Label("相等值")]
        public static readonly Property<string> EqualValue2Property = P<TurnOverRuleDetail>.Register(e => e.EqualValue2);

        /// <summary>
        /// 相等值
        /// </summary>
        public string EqualValue2
        {
            get { return GetProperty(EqualValue2Property); }
            set { SetProperty(EqualValue2Property, value); }
        }
        #endregion

        #region 字段类型 FieldType2
        /// <summary>
        /// 字段类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<DataType?> FieldType2Property = P<TurnOverRuleDetail>.Register(e => e.FieldType2);

        /// <summary>
        /// 字段类型
        /// </summary>
        public DataType? FieldType2
        {
            get { return GetProperty(FieldType2Property); }
            set { SetProperty(FieldType2Property, value); }
        }
        #endregion

        #region 排序方式 SortType2
        /// <summary>
        /// 排序方式
        /// </summary>
        [Label("排序方式")]
        public static readonly Property<SortType?> SortType2Property = P<TurnOverRuleDetail>.Register(e => e.SortType2);

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType? SortType2
        {
            get { return GetProperty(SortType2Property); }
            set { SetProperty(SortType2Property, value); }
        }
        #endregion

        #region 下限值 LowerLimit2
        /// <summary>
        /// 下限值
        /// </summary>
        [Label("下限值")]
        public static readonly Property<decimal?> LowerLimit2Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimit2);

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? LowerLimit2
        {
            get { return GetProperty(LowerLimit2Property); }
            set { SetProperty(LowerLimit2Property, value); }
        }
        #endregion

        #region 上限值 UpperLimit2
        /// <summary>
        /// 上限值
        /// </summary>
        [Label("上限值")]
        public static readonly Property<decimal?> UpperLimit2Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimit2);

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? UpperLimit2
        {
            get { return GetProperty(UpperLimit2Property); }
            set { SetProperty(UpperLimit2Property, value); }
        }
        #endregion

        #region 下限天数 LowerLimitDay2
        /// <summary>
        /// 下限天数
        /// </summary>
        [Label("下限天数")]
        public static readonly Property<decimal?> LowerLimitDay2Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimitDay2);

        /// <summary>
        /// 下限天数
        /// </summary>
        public decimal? LowerLimitDay2
        {
            get { return GetProperty(LowerLimitDay2Property); }
            set { SetProperty(LowerLimitDay2Property, value); }
        }
        #endregion

        #region 上限天数 UpperLimitDay2
        /// <summary>
        /// 上限天数
        /// </summary>
        [Label("上限天数")]
        public static readonly Property<decimal?> UpperLimitDay2Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimitDay2);

        /// <summary>
        /// 上限天数
        /// </summary>
        public decimal? UpperLimitDay2
        {
            get { return GetProperty(UpperLimitDay2Property); }
            set { SetProperty(UpperLimitDay2Property, value); }
        }
        #endregion
        
        #region 排序字段 SortField3
        /// <summary>
        /// 排序字段
        /// </summary>
        [Label("排序字段")]
        public static readonly Property<SortField?> SortField3Property = P<TurnOverRuleDetail>.Register(e => e.SortField3);

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField? SortField3
        {
            get { return GetProperty(SortField3Property); }
            set { SetProperty(SortField3Property, value); }
        }
        #endregion

        #region 字段类型 FieldType3
        /// <summary>
        /// 字段类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<DataType?> FieldType3Property = P<TurnOverRuleDetail>.Register(e => e.FieldType3);

        /// <summary>
        /// 字段类型
        /// </summary>
        public DataType? FieldType3
        {
            get { return GetProperty(FieldType3Property); }
            set { SetProperty(FieldType3Property, value); }
        }
        #endregion

        #region 排序方式 SortType3
        /// <summary>
        /// 排序方式
        /// </summary>
        [Label("排序方式")]
        public static readonly Property<SortType?> SortType3Property = P<TurnOverRuleDetail>.Register(e => e.SortType3);

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType? SortType3
        {
            get { return GetProperty(SortType3Property); }
            set { SetProperty(SortType3Property, value); }
        }
        #endregion

        #region 相等值 EqualValue3
        /// <summary>
        /// 相等值
        /// </summary>
        [Label("相等值")]
        public static readonly Property<string> EqualValue3Property = P<TurnOverRuleDetail>.Register(e => e.EqualValue3);

        /// <summary>
        /// 相等值
        /// </summary>
        public string EqualValue3
        {
            get { return GetProperty(EqualValue3Property); }
            set { SetProperty(EqualValue3Property, value); }
        }
        #endregion

        #region 上限值 UpperLimit3
        /// <summary>
        /// 上限值
        /// </summary>
        [Label("上限值")]
        public static readonly Property<decimal?> UpperLimit3Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimit3);

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? UpperLimit3
        {
            get { return GetProperty(UpperLimit3Property); }
            set { SetProperty(UpperLimit3Property, value); }
        }
        #endregion

        #region 下限值 LowerLimit3
        /// <summary>
        /// 下限值
        /// </summary>
        [Label("下限值")]
        public static readonly Property<decimal?> LowerLimit3Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimit3);

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? LowerLimit3
        {
            get { return GetProperty(LowerLimit3Property); }
            set { SetProperty(LowerLimit3Property, value); }
        }
        #endregion

        #region 上限天数 UpperLimitDay3
        /// <summary>
        /// 上限天数
        /// </summary>
        [Label("上限天数")]
        public static readonly Property<decimal?> UpperLimitDay3Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimitDay3);

        /// <summary>
        /// 上限天数
        /// </summary>
        public decimal? UpperLimitDay3
        {
            get { return GetProperty(UpperLimitDay3Property); }
            set { SetProperty(UpperLimitDay3Property, value); }
        }
        #endregion

        #region 下限天数 LowerLimitDay3
        /// <summary>
        /// 下限天数
        /// </summary>
        [Label("下限天数")]
        public static readonly Property<decimal?> LowerLimitDay3Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimitDay3);

        /// <summary>
        /// 下限天数
        /// </summary>
        public decimal? LowerLimitDay3
        {
            get { return GetProperty(LowerLimitDay3Property); }
            set { SetProperty(LowerLimitDay3Property, value); }
        }
        #endregion
        
        #region 排序字段 SortField4
        /// <summary>
        /// 排序字段
        /// </summary>
        [Label("排序字段")]
        public static readonly Property<SortField?> SortField4Property = P<TurnOverRuleDetail>.Register(e => e.SortField4);

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField? SortField4
        {
            get { return GetProperty(SortField4Property); }
            set { SetProperty(SortField4Property, value); }
        }
        #endregion

        #region 字段类型 FieldType4
        /// <summary>
        /// 字段类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<DataType?> FieldType4Property = P<TurnOverRuleDetail>.Register(e => e.FieldType4);

        /// <summary>
        /// 字段类型
        /// </summary>
        public DataType? FieldType4
        {
            get { return GetProperty(FieldType4Property); }
            set { SetProperty(FieldType4Property, value); }
        }
        #endregion

        #region 排序方式 SortType4
        /// <summary>
        /// 排序方式
        /// </summary>
        [Label("排序方式")]
        public static readonly Property<SortType?> SortType4Property = P<TurnOverRuleDetail>.Register(e => e.SortType4);

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType? SortType4
        {
            get { return GetProperty(SortType4Property); }
            set { SetProperty(SortType4Property, value); }
        }
        #endregion

        #region 相等值 EqualValue4
        /// <summary>
        /// 相等值
        /// </summary>
        [Label("相等值")]
        public static readonly Property<string> EqualValue4Property = P<TurnOverRuleDetail>.Register(e => e.EqualValue4);

        /// <summary>
        /// 相等值
        /// </summary>
        public string EqualValue4
        {
            get { return GetProperty(EqualValue4Property); }
            set { SetProperty(EqualValue4Property, value); }
        }
        #endregion

        #region 上限值 UpperLimit4
        /// <summary>
        /// 上限值
        /// </summary>
        [Label("上限值")]
        public static readonly Property<decimal?> UpperLimit4Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimit4);

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? UpperLimit4
        {
            get { return GetProperty(UpperLimit4Property); }
            set { SetProperty(UpperLimit4Property, value); }
        }
        #endregion

        #region 下限值 LowerLimit4
        /// <summary>
        /// 下限值
        /// </summary>
        [Label("下限值")]
        public static readonly Property<decimal?> LowerLimit4Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimit4);

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? LowerLimit4
        {
            get { return GetProperty(LowerLimit4Property); }
            set { SetProperty(LowerLimit4Property, value); }
        }
        #endregion

        #region 上限天数 UpperLimitDay4
        /// <summary>
        /// 上限天数
        /// </summary>
        [Label("上限天数")]
        public static readonly Property<decimal?> UpperLimitDay4Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimitDay4);

        /// <summary>
        /// 上限天数
        /// </summary>
        public decimal? UpperLimitDay4
        {
            get { return GetProperty(UpperLimitDay4Property); }
            set { SetProperty(UpperLimitDay4Property, value); }
        }
        #endregion

        #region 下限天数 LowerLimitDay4
        /// <summary>
        /// 下限天数
        /// </summary>
        [Label("下限天数")]
        public static readonly Property<decimal?> LowerLimitDay4Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimitDay4);

        /// <summary>
        /// 下限天数
        /// </summary>
        public decimal? LowerLimitDay4
        {
            get { return GetProperty(LowerLimitDay4Property); }
            set { SetProperty(LowerLimitDay4Property, value); }
        }
        #endregion

        

        #region 排序字段 SortField5
        /// <summary>
        /// 排序字段
        /// </summary>
        [Label("排序字段")]
        public static readonly Property<SortField?> SortField5Property = P<TurnOverRuleDetail>.Register(e => e.SortField5);

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField? SortField5
        {
            get { return GetProperty(SortField5Property); }
            set { SetProperty(SortField5Property, value); }
        }
        #endregion

        #region 字段类型 FieldType5
        /// <summary>
        /// 字段类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<DataType?> FieldType5Property = P<TurnOverRuleDetail>.Register(e => e.FieldType5);

        /// <summary>
        /// 字段类型
        /// </summary>
        public DataType? FieldType5
        {
            get { return GetProperty(FieldType5Property); }
            set { SetProperty(FieldType5Property, value); }
        }
        #endregion

        #region 排序方式 SortType5
        /// <summary>
        /// 排序方式
        /// </summary>
        [Label("排序方式")]
        public static readonly Property<SortType?> SortType5Property = P<TurnOverRuleDetail>.Register(e => e.SortType5);

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType? SortType5
        {
            get { return GetProperty(SortType5Property); }
            set { SetProperty(SortType5Property, value); }
        }
        #endregion

        #region 相等值 EqualValue5
        /// <summary>
        /// 相等值
        /// </summary>
        [Label("相等值")]
        public static readonly Property<string> EqualValue5Property = P<TurnOverRuleDetail>.Register(e => e.EqualValue5);

        /// <summary>
        /// 相等值
        /// </summary>
        public string EqualValue5
        {
            get { return GetProperty(EqualValue5Property); }
            set { SetProperty(EqualValue5Property, value); }
        }
        #endregion

        #region 上限值 UpperLimit5
        /// <summary>
        /// 上限值
        /// </summary>
        [Label("上限值")]
        public static readonly Property<decimal?> UpperLimit5Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimit5);

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? UpperLimit5
        {
            get { return GetProperty(UpperLimit5Property); }
            set { SetProperty(UpperLimit5Property, value); }
        }
        #endregion

        #region 下限值 LowerLimit5
        /// <summary>
        /// 下限值
        /// </summary>
        [Label("下限值")]
        public static readonly Property<decimal?> LowerLimit5Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimit5);

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? LowerLimit5
        {
            get { return GetProperty(LowerLimit5Property); }
            set { SetProperty(LowerLimit5Property, value); }
        }
        #endregion

        #region 上限天数 UpperLimitDay5
        /// <summary>
        /// 上限天数
        /// </summary>
        [Label("上限天数")]
        public static readonly Property<decimal?> UpperLimitDay5Property = P<TurnOverRuleDetail>.Register(e => e.UpperLimitDay5);

        /// <summary>
        /// 上限天数
        /// </summary>
        public decimal? UpperLimitDay5
        {
            get { return GetProperty(UpperLimitDay5Property); }
            set { SetProperty(UpperLimitDay5Property, value); }
        }
        #endregion

        #region 下限天数 LowerLimitDay5
        /// <summary>
        /// 下限天数
        /// </summary>
        [Label("下限天数")]
        public static readonly Property<decimal?> LowerLimitDay5Property = P<TurnOverRuleDetail>.Register(e => e.LowerLimitDay5);

        /// <summary>
        /// 下限天数
        /// </summary>
        public decimal? LowerLimitDay5
        {
            get { return GetProperty(LowerLimitDay5Property); }
            set { SetProperty(LowerLimitDay5Property, value); }
        }
        #endregion

        

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门名称")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<TurnOverRuleDetail>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<TurnOverRuleDetail>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 事务定义 Transaction
        /// <summary>
        /// 事务定义Id
        /// </summary>
        [Label("单据小类")]
        public static readonly IRefIdProperty TransactionIdProperty = P<TurnOverRuleDetail>.RegisterRefId(e => e.TransactionId, ReferenceType.Normal);

        /// <summary>
        /// 事务定义Id
        /// </summary>
        public double? TransactionId
        {
            get { return (double?)GetRefNullableId(TransactionIdProperty); }
            set { SetRefNullableId(TransactionIdProperty, value); }
        }

        /// <summary>
        /// 事务定义
        /// </summary>
        public static readonly RefEntityProperty<Transaction> TransactionProperty = P<TurnOverRuleDetail>.RegisterRef(e => e.Transaction, TransactionIdProperty);

        /// <summary>
        /// 事务定义
        /// </summary>
        public Transaction Transaction
        {
            get { return GetRefEntity(TransactionProperty); }
            set { SetRefEntity(TransactionProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<TurnOverRuleDetail>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType? OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户名称")]
        public static readonly IRefIdProperty CustomerIdProperty = P<TurnOverRuleDetail>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)GetRefNullableId(CustomerIdProperty); }
            set { SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<TurnOverRuleDetail>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 周转规则 TurnOverRule
        /// <summary>
        /// 周转规则Id
        /// </summary>
        public static readonly IRefIdProperty TurnOverRuleIdProperty = P<TurnOverRuleDetail>.RegisterRefId(e => e.TurnOverRuleId, ReferenceType.Parent);

        /// <summary>
        /// 周转规则Id
        /// </summary>
        public double TurnOverRuleId
        {
            get { return (double)GetRefId(TurnOverRuleIdProperty); }
            set { SetRefId(TurnOverRuleIdProperty, value); }
        }

        /// <summary>
        /// 周转规则
        /// </summary>
        public static readonly RefEntityProperty<TurnOverRule> TurnOverRuleProperty = P<TurnOverRuleDetail>.RegisterRef(e => e.TurnOverRule, TurnOverRuleIdProperty);

        /// <summary>
        /// 周转规则
        /// </summary>
        public TurnOverRule TurnOverRule
        {
            get { return GetRefEntity(TurnOverRuleProperty); }
            set { SetRefEntity(TurnOverRuleProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<TurnOverRuleDetail>.RegisterView(e => e.CustomerName, e => e.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        /// <summary>
        /// 排序字段1值改变
        /// </summary>
        /// <param name="dicSortField">字段批次属性数据类型字典</param>
        private void SortField1Change(Dictionary<string, DataType> dicSortField)
        {
            if (SortField1 != null && SortField1.HasValue && dicSortField.Count > 0)
            {
                FieldType1 = dicSortField[SortField1.ToLabel()];
                if (FieldType1.Value == DataType.Date)
                {
                    LowerLimit1 = null;
                    UpperLimit1 = null;
                    EqualValue1 = string.Empty;
                }
                else if (FieldType1.Value == DataType.Text)
                {
                    LowerLimitDay1 = null;
                    UpperLimitDay1 = null;
                    LowerLimit1 = null;
                    UpperLimit1 = null;
                }
                else if (FieldType1.Value == DataType.Numerical)
                {
                    LowerLimitDay1 = null;
                    UpperLimitDay1 = null;
                    EqualValue1 = string.Empty;
                }
            }
            else
            {
                FieldType1 = null;
                SortType1 = null;
                EqualValue1 = string.Empty;
                LowerLimit1 = null;
                UpperLimit1 = null;
                LowerLimitDay1 = null;
                UpperLimitDay1 = null;
            }
        }

        /// <summary>
        /// 排序字段2值改变
        /// </summary>
        /// <param name="dicSortField">字段批次属性数据类型字典</param>
        private void SortField2Change(Dictionary<string, DataType> dicSortField)
        {
            if (SortField2 != null && SortField2.HasValue && dicSortField.Count > 0)
            {
                FieldType2 = dicSortField[SortField2.ToLabel()];
                if (FieldType2.Value == DataType.Date)
                {
                    LowerLimit2 = null;
                    UpperLimit2 = null;
                    EqualValue2 = string.Empty;
                }
                else if (FieldType2.Value == DataType.Text)
                {
                    LowerLimitDay2 = null;
                    UpperLimitDay2 = null;
                    LowerLimit2 = null;
                    UpperLimit2 = null;
                }
                else if (FieldType2.Value == DataType.Numerical)
                {
                    LowerLimitDay2 = null;
                    UpperLimitDay2 = null;
                    EqualValue2 = string.Empty;
                }
            }
            else
            {
                FieldType2 = null;
                SortType2 = null;
                EqualValue2 = string.Empty;
                LowerLimit2 = null;
                UpperLimit2 = null;
                LowerLimitDay2 = null;
                UpperLimitDay2 = null;
            }
        }

        /// <summary>
        /// 排序字段3值改变
        /// </summary>
        /// <param name="dicSortField">字段批次属性数据类型字典</param>
        private void SortField3Change(Dictionary<string, DataType> dicSortField)
        {
            if (SortField3 != null && SortField3.HasValue && dicSortField.Count > 0)
            {
                FieldType3 = dicSortField[SortField3.ToLabel()];
                if (FieldType3.Value == DataType.Date)
                {
                    LowerLimit3 = null;
                    UpperLimit3 = null;
                    EqualValue3 = string.Empty;
                }
                else if (FieldType3.Value == DataType.Text)
                {
                    LowerLimitDay3 = null;
                    UpperLimitDay3 = null;
                    LowerLimit3 = null;
                    UpperLimit3 = null;
                }
                else if (FieldType3.Value == DataType.Numerical)
                {
                    LowerLimitDay3 = null;
                    UpperLimitDay3 = null;
                    EqualValue3 = string.Empty;
                }
            }
            else
            {
                FieldType3 = null;
                SortType3 = null;
                EqualValue3 = string.Empty;
                LowerLimit3 = null;
                UpperLimit3 = null;
                LowerLimitDay3 = null;
                UpperLimitDay3 = null;
            }
        }

        /// <summary>
        /// 排序字段4值改变
        /// </summary>
        /// <param name="dicSortField">字段批次属性数据类型字典</param>
        private void SortField4Change(Dictionary<string, DataType> dicSortField)
        {
            if (SortField4 != null && SortField4.HasValue && dicSortField.Count > 0)
            {
                FieldType4 = dicSortField[SortField4.ToLabel()];
                if (FieldType4.Value == DataType.Date)
                {
                    LowerLimit4 = null;
                    UpperLimit4 = null;
                    EqualValue4 = string.Empty;
                }
                else if (FieldType4.Value == DataType.Text)
                {
                    LowerLimitDay4 = null;
                    UpperLimitDay4 = null;
                    LowerLimit4 = null;
                    UpperLimit4 = null;
                }
                else if (FieldType4.Value == DataType.Numerical)
                {
                    LowerLimitDay4 = null;
                    UpperLimitDay4 = null;
                    EqualValue4 = string.Empty;
                }
            }
            else
            {
                FieldType4 = null;
                SortType4 = null;
                EqualValue4 = string.Empty;
                LowerLimit4 = null;
                UpperLimit4 = null;
                LowerLimitDay4 = null;
                UpperLimitDay4 = null;
            }
        }

        /// <summary>
        /// 排序字段5值改变
        /// </summary>
        /// <param name="dicSortField">字段批次属性数据类型字典</param>
        private void SortField5Change(Dictionary<string, DataType> dicSortField)
        {
            if (SortField5 != null && SortField5.HasValue && dicSortField.Count > 0)
            {
                FieldType5 = dicSortField[SortField5.ToLabel()];
                if (FieldType5.Value == DataType.Date)
                {
                    LowerLimit5 = null;
                    UpperLimit5 = null;
                    EqualValue5 = string.Empty;
                }
                else if (FieldType5.Value == DataType.Text)
                {
                    LowerLimitDay5 = null;
                    UpperLimitDay5 = null;
                    LowerLimit5 = null;
                    UpperLimit5 = null;
                }
                else if (FieldType5.Value == DataType.Numerical)
                {
                    LowerLimitDay5 = null;
                    UpperLimitDay5 = null;
                    EqualValue5 = string.Empty;
                }
            }
            else
            {
                FieldType5 = null;
                SortType5 = null;
                EqualValue5 = string.Empty;
                LowerLimit5 = null;
                UpperLimit5 = null;
                LowerLimitDay5 = null;
                UpperLimitDay5 = null;
            }
        }
        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Source == ManagedPropertyChangedSource.FromUIOperating && e.NewValue != e.OldValue)
            {
                if (e.Property == TurnOverRuleDetail.OrderTypeProperty)
                {
                    TransactionId = 0;
                    Transaction = null;
                }

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

                if (e.Property == TurnOverRuleDetail.SortField1Property)
                {
                    SortField1Change(dicSortField);
                }

                if (e.Property == TurnOverRuleDetail.SortField2Property)
                {
                    SortField2Change(dicSortField);
                }

                if (e.Property == TurnOverRuleDetail.SortField3Property)
                {
                    SortField3Change(dicSortField);
                }

                if (e.Property == TurnOverRuleDetail.SortField4Property)
                {
                    SortField4Change(dicSortField);
                }

                if (e.Property == TurnOverRuleDetail.SortField5Property)
                {
                    SortField5Change(dicSortField);
                }
            }
        }
    }

    /// <summary>
    /// 周转规则明细 实体配置
    /// </summary>
    internal class TurnOverRuleDetailConfig : EntityConfig<TurnOverRuleDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TURN_OVER_RULE_DTL").MapAllProperties();
            Meta.EnableSort();
            Meta.EnablePhantoms();
        }
    }
}