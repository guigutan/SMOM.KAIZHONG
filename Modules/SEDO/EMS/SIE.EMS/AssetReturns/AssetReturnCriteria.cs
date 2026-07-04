using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.AssetReturns
{
    /// <summary>
    /// 资产归还查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("资产归还查询实体")]
    public partial class AssetReturnCriteria : Criteria
    {
        #region 归还单号 ReturnNo
        /// <summary>
        /// 归还单号
        /// </summary>
        [Label("归还单号")]
        public static readonly Property<string> ReturnNoProperty = P<AssetReturnCriteria>.Register(e => e.ReturnNo);

        /// <summary>
        /// 归还单号
        /// </summary>
        public string ReturnNo
        {
            get { return GetProperty(ReturnNoProperty); }
            set { SetProperty(ReturnNoProperty, value); }
        }
        #endregion

        #region 工厂 QureyFactory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty QureyFactoryIdProperty = P<AssetReturnCriteria>.RegisterRefId(e => e.QureyFactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> QureyFactoryProperty = P<AssetReturnCriteria>.RegisterRef(e => e.QureyFactory, QureyFactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise QureyFactory
        {
            get { return GetRefEntity(QureyFactoryProperty); }
            set { SetRefEntity(QureyFactoryProperty, value); }
        }
        #endregion

        #region 借用单号 RequisitionNo
        /// <summary>
        /// 借用单号
        /// </summary>		
        [Label("借用单号")]
        public static readonly Property<string> RequisitionNoProperty = P<AssetReturnCriteria>.Register(e => e.RequisitionNo);

        /// <summary>
        /// 借用单号
        /// </summary>
        public string RequisitionNo
        {
            get { return GetProperty(RequisitionNoProperty); }
            set { SetProperty(RequisitionNoProperty, value); }
        }
        #endregion

        #region 归还对象 AssetObject
        /// <summary>
        /// 归还对象
        /// </summary>
        [Label("归还对象")]
        public static readonly Property<AssetObject?> AssetObjectProperty = P<AssetReturnCriteria>.Register(e => e.AssetObject);

        /// <summary>
        /// 归还对象
        /// </summary>
        public AssetObject? AssetObject
        {
            get { return this.GetProperty(AssetObjectProperty); }
            set { this.SetProperty(AssetObjectProperty, value); }
        }
        #endregion

        #region 归还部门 ApplyDepartment
        /// <summary>
        /// 归还部门Id
        /// </summary>
        [Label("归还部门")]
        public static readonly IRefIdProperty ApplyDepartmentIdProperty =
            P<AssetReturnCriteria>.RegisterRefId(e => e.ApplyDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 归还部门Id
        /// </summary>
        public double? ApplyDepartmentId
        {
            get { return (double?)this.GetRefNullableId(ApplyDepartmentIdProperty); }
            set { this.SetRefNullableId(ApplyDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 归还部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ApplyDepartmentProperty =
            P<AssetReturnCriteria>.RegisterRef(e => e.ApplyDepartment, ApplyDepartmentIdProperty);

        /// <summary>
        /// 归还部门
        /// </summary>
        public Enterprise ApplyDepartment
        {
            get { return this.GetRefEntity(ApplyDepartmentProperty); }
            set { this.SetRefEntity(ApplyDepartmentProperty, value); }
        }
        #endregion

        #region 接收部门 LendingDepartment
        /// <summary>
        /// 接收部门Id
        /// </summary>
        [Label("接收部门")]
        public static readonly IRefIdProperty LendingDepartmentIdProperty =
            P<AssetReturnCriteria>.RegisterRefId(e => e.LendingDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 接收部门Id
        /// </summary>
        public double? LendingDepartmentId
        {
            get { return (double?)this.GetRefNullableId(LendingDepartmentIdProperty); }
            set { this.SetRefNullableId(LendingDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 接收部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> LendingDepartmentProperty =
            P<AssetReturnCriteria>.RegisterRef(e => e.LendingDepartment, LendingDepartmentIdProperty);

        /// <summary>
        /// 接收部门
        /// </summary>
        public Enterprise LendingDepartment
        {
            get { return this.GetRefEntity(LendingDepartmentProperty); }
            set { this.SetRefEntity(LendingDepartmentProperty, value); }
        }
        #endregion

        #region 接收仓库 Warehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssetReturnCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetReturnCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 接收仓库
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
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<AssetReturnCriteria>.Register(e => e.ApprovalStatus);

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
        public static readonly Property<DateRange> CreateDateProperty = P<AssetReturnCriteria>.Register(e => e.CreateDate);

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
            return RT.Service.Resolve<AssetReturnController>().GetAssetReturnList(this);

        }
    }
}
