using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Fixtures.Models;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Purchases.FixtureAcceptances
{
    /// <summary>
    /// 工治具验收查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工治具验收查询")]
    public class FixtureAcceptanceCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FixtureAcceptanceCriteria()
        {
            CreateDate = new DateRange();
            CreateDate.DateRangeType = DateRangeType.All;
        }

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty
            = P<FixtureAcceptanceCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty
            = P<FixtureAcceptanceCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty
            = P<FixtureAcceptanceCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty
            = P<FixtureAcceptanceCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<FixtureAcceptanceCriteria>.Register(e => e.FixtureEncodeCode);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
            set { this.SetProperty(FixtureEncodeCodeProperty, value); }
        }
        #endregion




        #region 采购订单号 No
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> NoProperty = P<FixtureAcceptanceCriteria>.Register(e => e.No);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion


        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty
            = P<FixtureAcceptanceCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty
            = P<FixtureAcceptanceCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty =
            P<FixtureAcceptanceCriteria>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)this.GetRefNullableId(CustomerIdProperty); }
            set { this.SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty =
            P<FixtureAcceptanceCriteria>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return this.GetRefEntity(CustomerProperty); }
            set { this.SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty
            = P<FixtureAcceptanceCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<FixtureAcceptanceCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FixtureAcceptancesController>().CriteriaFixtureAcceptances(this);
        }
    }
}
