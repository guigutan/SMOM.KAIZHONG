using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.AssetIssues
{
    /// <summary>
    /// 资产发放查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("资产发放查询实体")]
    public partial class AssetIssueCriteria : Criteria
    {
        #region 发放单号 IssueNo
        /// <summary>
        /// 发放单号
        /// </summary>
        [Label("发放单号")]
        public static readonly Property<string> IssueNoProperty = P<AssetIssueCriteria>.Register(e => e.IssueNo);

        /// <summary>
        /// 发放单号
        /// </summary>
        public string IssueNo
        {
            get { return this.GetProperty(IssueNoProperty); }
            set { this.SetProperty(IssueNoProperty, value); }
        }
        #endregion

        #region 领用单号 RequisitionNo
        /// <summary>
        /// 领用单号
        /// </summary>		
        [Label("领用单号")]
        public static readonly Property<string> RequisitionNoProperty = P<AssetIssueCriteria>.Register(e => e.RequisitionNo);

        /// <summary>
        /// 领用单号
        /// </summary>
        public string RequisitionNo
        {
            get { return GetProperty(RequisitionNoProperty); }
            set { SetProperty(RequisitionNoProperty, value); }
        }
        #endregion

        #region 工厂 QureyFactory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty QureyFactoryIdProperty = P<AssetIssueCriteria>.RegisterRefId(e => e.QureyFactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? QureyFactoryId
        {
            get { return (double?)GetRefNullableId(QureyFactoryIdProperty); }
            set { SetRefNullableId(QureyFactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> QureyFactoryProperty = P<AssetIssueCriteria>.RegisterRef(e => e.QureyFactory, QureyFactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise QureyFactory
        {
            get { return GetRefEntity(QureyFactoryProperty); }
            set { SetRefEntity(QureyFactoryProperty, value); }
        }
        #endregion

        #region 业务类型 RequisitionType
        /// <summary>
        /// 业务类型
        /// </summary>
        [Label("业务类型")]
        public static readonly Property<RequisitionType?> RequisitionTypeProperty = P<AssetIssueCriteria>.Register(e => e.RequisitionType);

        /// <summary>
        /// 业务类型
        /// </summary>
        public RequisitionType? RequisitionType
        {
            get { return GetProperty(RequisitionTypeProperty); }
            set { SetProperty(RequisitionTypeProperty, value); }
        }
        #endregion

        #region 资产对象 AssetObject
        /// <summary>
        /// 资产对象
        /// </summary>
        [Label("资产对象")]
        public static readonly Property<AssetObject?> AssetObjectProperty = P<AssetIssueCriteria>.Register(e => e.AssetObject);

        /// <summary>
        /// 资产对象
        /// </summary>
        public AssetObject? AssetObject
        {
            get { return this.GetProperty(AssetObjectProperty); }
            set { this.SetProperty(AssetObjectProperty, value); }
        }
        #endregion

        #region 申请部门 ApplyDepartment
        /// <summary>
        /// 申请部门Id
        /// </summary>
        [Label("申请部门")]
        public static readonly IRefIdProperty ApplyDepartmentIdProperty =
            P<AssetIssueCriteria>.RegisterRefId(e => e.ApplyDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 申请部门Id
        /// </summary>
        public double? ApplyDepartmentId
        {
            get { return (double?)this.GetRefNullableId(ApplyDepartmentIdProperty); }
            set { this.SetRefNullableId(ApplyDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 申请部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ApplyDepartmentProperty =
            P<AssetIssueCriteria>.RegisterRef(e => e.ApplyDepartment, ApplyDepartmentIdProperty);

        /// <summary>
        /// 申请部门
        /// </summary>
        public Enterprise ApplyDepartment
        {
            get { return this.GetRefEntity(ApplyDepartmentProperty); }
            set { this.SetRefEntity(ApplyDepartmentProperty, value); }
        }
        #endregion

        #region 借出部门 LendingDepartment
        /// <summary>
        /// 借出部门Id
        /// </summary>
        [Label("借出部门")]
        public static readonly IRefIdProperty LendingDepartmentIdProperty =
            P<AssetIssueCriteria>.RegisterRefId(e => e.LendingDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 借出部门Id
        /// </summary>
        public double? LendingDepartmentId
        {
            get { return (double?)this.GetRefNullableId(LendingDepartmentIdProperty); }
            set { this.SetRefNullableId(LendingDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 借出部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> LendingDepartmentProperty =
            P<AssetIssueCriteria>.RegisterRef(e => e.LendingDepartment, LendingDepartmentIdProperty);

        /// <summary>
        /// 借出部门
        /// </summary>
        public Enterprise LendingDepartment
        {
            get { return this.GetRefEntity(LendingDepartmentProperty); }
            set { this.SetRefEntity(LendingDepartmentProperty, value); }
        }
        #endregion

        #region 发放仓库 Warehouse
        /// <summary>
        /// 发放仓库Id
        /// </summary>
        [Label("发放仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssetIssueCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发放仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发放仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetIssueCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发放仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<AssetIssueCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
            set { this.SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<AssetIssueCriteria>.Register(e => e.CreateDate);

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
        /// 重写实体查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AssetIssueController>().GetAssetIssueList(this);

        }
    }
}
