using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using Employee = SIE.Resources.Employee;

namespace SIE.EMS.ViceTransfers
{
    /// <summary>
    /// 副资产调拨查询
    /// </summary>
    [QueryEntity, Serializable]
    public class ViceTransferCriteria : Criteria
    {


        #region 调拨单号 TransferNo
        /// <summary>
        /// 调拨单号
        /// </summary>
        [Label("调拨单号")]
        public static readonly Property<string> TransferNoProperty = P<ViceTransferCriteria>.Register(e => e.TransferNo);

        /// <summary>
        /// 调拨单号
        /// </summary>
        public string TransferNo
        {
            get { return this.GetProperty(TransferNoProperty); }
            set { this.SetProperty(TransferNoProperty, value); }
        }
        #endregion

        #region 工厂 QureyFactory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty QureyFactoryIdProperty = P<ViceTransferCriteria>.RegisterRefId(e => e.QureyFactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> QureyFactoryProperty = P<ViceTransferCriteria>.RegisterRef(e => e.QureyFactory, QureyFactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise QureyFactory
        {
            get { return GetRefEntity(QureyFactoryProperty); }
            set { SetRefEntity(QureyFactoryProperty, value); }
        }
        #endregion


        #region 资产对象 ViceAssetObject
        /// <summary>
        /// 资产对象
        /// </summary>
        [Label("资产对象")]
        public static readonly Property<ViceAssetObject?> ViceAssetObjectProperty = P<ViceTransferCriteria>.Register(e => e.ViceAssetObject);

        /// <summary>
        /// 资产对象
        /// </summary>
        public ViceAssetObject? ViceAssetObject
        {
            get { return this.GetProperty(ViceAssetObjectProperty); }
            set { this.SetProperty(ViceAssetObjectProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<ViceTransferCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion


        #region 目标仓库 TargetWareHouse
        /// <summary>
        /// 目标仓库Id
        /// </summary>
        [Label("目标仓库")]
        public static readonly IRefIdProperty TargetWareHouseIdProperty =
            P<ViceTransferCriteria>.RegisterRefId(e => e.TargetWareHouseId, ReferenceType.Normal);

        /// <summary>
        /// 目标仓库Id
        /// </summary>
        public double? TargetWareHouseId
        {
            get { return (double?)this.GetRefNullableId(TargetWareHouseIdProperty); }
            set { this.SetRefNullableId(TargetWareHouseIdProperty, value); }
        }

        /// <summary>
        /// 目标仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> TargetWareHouseProperty =
            P<ViceTransferCriteria>.RegisterRef(e => e.TargetWareHouse, TargetWareHouseIdProperty);

        /// <summary>
        /// 目标仓库
        /// </summary>
        public Warehouse TargetWareHouse
        {
            get { return this.GetRefEntity(TargetWareHouseProperty); }
            set { this.SetRefEntity(TargetWareHouseProperty, value); }
        }
        #endregion




        #region 来源仓库 OriginWareHouse
        /// <summary>
        /// 来源仓库Id
        /// </summary>
        [Label("来源仓库")]
        public static readonly IRefIdProperty OriginWareHouseIdProperty =
            P<ViceTransferCriteria>.RegisterRefId(e => e.OriginWareHouseId, ReferenceType.Normal);

        /// <summary>
        /// 来源仓库Id  
        /// </summary>
        public double? OriginWareHouseId
        {
            get { return (double?)this.GetRefNullableId(OriginWareHouseIdProperty); }
            set { this.SetRefNullableId(OriginWareHouseIdProperty, value); }
        }

        /// <summary>
        /// 来源仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> OriginWareHouseProperty =
            P<ViceTransferCriteria>.RegisterRef(e => e.OriginWareHouse, OriginWareHouseIdProperty);

        /// <summary>
        /// 来源仓库
        /// </summary>
        public Warehouse OriginWareHouse
        {
            get { return this.GetRefEntity(OriginWareHouseProperty); }
            set { this.SetRefEntity(OriginWareHouseProperty, value); }
        }
        #endregion



        #region 申请人 applicant
        /// <summary>
        /// 申请人Id
        /// </summary>
        [Label("申请人")]
        public static readonly IRefIdProperty ApplicantIdProperty = P<ViceTransferCriteria>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

        /// <summary>
        /// 申请人Id
        /// </summary>
        public double? ApplicantId
        {
            get { return (double?)GetRefNullableId(ApplicantIdProperty); }
            set { SetRefNullableId(ApplicantIdProperty, value); }
        }

        /// <summary>
        /// 申请人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplicantProperty = P<ViceTransferCriteria>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

        /// <summary>
        /// 申请人
        /// </summary>
        public Employee Applicant
        {
            get { return GetRefEntity(ApplicantProperty); }
            set { SetRefEntity(ApplicantProperty, value); }
        }
        #endregion


        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<ViceTransferCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion


        #region 调拨状态 TransferStatus
        /// <summary>
        /// 调拨状态
        /// </summary>
        [Label("调拨状态")]
        public static readonly Property<TransferStatus?> TransferStatusProperty = P<ViceTransferCriteria>.Register(e => e.TransferStatus);

        /// <summary>
        /// 调拨状态
        /// </summary>
        public TransferStatus? TransferStatus
        {
            get { return GetProperty(TransferStatusProperty); }
            set { SetProperty(TransferStatusProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写实体查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ViceTransferController>().Fetch(this);

        }
    }
}
