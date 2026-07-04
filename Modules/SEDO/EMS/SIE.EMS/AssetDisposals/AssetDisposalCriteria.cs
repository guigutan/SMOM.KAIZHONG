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

namespace SIE.EMS.AssetDisposals
{
    /// <summary>
    /// 资产处置查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("资产处置查询实体")]
    public partial class AssetDisposalCriteria : Criteria
    {
        #region 处置单号 No
        /// <summary>
        /// 处置单号
        /// </summary>		
        [Label("处置单号")]
        public static readonly Property<string> NoProperty = P<AssetDisposalCriteria>.Register(e => e.No);

        /// <summary>
        /// 处置单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 工厂 QureyFactory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty QureyFactoryIdProperty = P<AssetDisposalCriteria>.RegisterRefId(e => e.QureyFactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> QureyFactoryProperty = P<AssetDisposalCriteria>.RegisterRef(e => e.QureyFactory, QureyFactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise QureyFactory
        {
            get { return GetRefEntity(QureyFactoryProperty); }
            set { SetRefEntity(QureyFactoryProperty, value); }
        }
        #endregion

        #region 资产对象 AssetObject
        /// <summary>
        /// 资产对象
        /// </summary>
        [Label("资产对象")]
        public static readonly Property<AssetObject?> AssetObjectProperty = P<AssetDisposalCriteria>.Register(e => e.AssetObject);

        /// <summary>
        /// 资产对象
        /// </summary>
        public AssetObject? AssetObject
        {
            get { return this.GetProperty(AssetObjectProperty); }
            set { this.SetProperty(AssetObjectProperty, value); }
        }
        #endregion

        #region 管理部门 ManageDept
        /// <summary>
        /// 管理部门Id
        /// </summary>
        [Label("管理部门")]
        public static readonly IRefIdProperty ManageDeptIdProperty = P<AssetDisposalCriteria>.RegisterRefId(e => e.ManageDeptId, ReferenceType.Normal);

        /// <summary>
        /// 管理部门Id
        /// </summary>
        public double? ManageDeptId
        {
            get { return (double?)GetRefNullableId(ManageDeptIdProperty); }
            set { SetRefNullableId(ManageDeptIdProperty, value); }
        }

        /// <summary>
        /// 管理部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ManageDeptProperty = P<AssetDisposalCriteria>.RegisterRef(e => e.ManageDept, ManageDeptIdProperty);

        /// <summary>
        /// 管理部门
        /// </summary>
        public Enterprise ManageDept
        {
            get { return GetRefEntity(ManageDeptProperty); }
            set { SetRefEntity(ManageDeptProperty, value); }
        }
        #endregion

        #region 使用部门 UseDept
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty UseDeptIdProperty = P<AssetDisposalCriteria>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDeptId
        {
            get { return (double?)GetRefNullableId(UseDeptIdProperty); }
            set { SetRefNullableId(UseDeptIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<AssetDisposalCriteria>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDept
        {
            get { return GetRefEntity(UseDeptProperty); }
            set { SetRefEntity(UseDeptProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssetDisposalCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetDisposalCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
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
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<AssetDisposalCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
            set { this.SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 申请人 Applicant
        /// <summary>
        /// 申请人Id
        /// </summary>
        [Label("申请人")]
        public static readonly IRefIdProperty ApplicantIdProperty =
            P<AssetDisposalCriteria>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

        /// <summary>
        /// 申请人Id
        /// </summary>
        public double? ApplicantId
        {
            get { return (double?)this.GetRefNullableId(ApplicantIdProperty); }
            set { this.SetRefNullableId(ApplicantIdProperty, value); }
        }

        /// <summary>
        /// 申请人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplicantProperty =
            P<AssetDisposalCriteria>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

        /// <summary>
        /// 申请人
        /// </summary>
        public Employee Applicant
        {
            get { return this.GetRefEntity(ApplicantProperty); }
            set { this.SetRefEntity(ApplicantProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<AssetDisposalCriteria>.Register(e => e.CreateDate);

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
            return RT.Service.Resolve<AssetDisposalController>().GetAssetDisposalList(this);

        }
    }
}
