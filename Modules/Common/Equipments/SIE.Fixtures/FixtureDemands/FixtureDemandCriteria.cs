using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.Fixtures.FixtureDemands
{
    /// <summary>
	/// 工治具需求清单查询体
	/// </summary>
	[QueryEntity, Serializable]
    [Label("工治具需求清单查询体")]
    public partial class FixtureDemandCriteria : Criteria
    {
        #region No
        /// <summary>
        /// 需求单号
        /// </summary>
        [Label("需求单号")]
        public static readonly Property<string> NoProperty = P<FixtureDemandCriteria>.Register(e => e.No);

        /// <summary>
        /// 需求单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<FixtureDemandCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        public static readonly IRefIdProperty WorkShopIdProperty = P<FixtureDemandCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<FixtureDemandCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<FixtureDemandCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<FixtureDemandCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region ProductCode
        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        public static readonly Property<string> ProductCodeProperty = P<FixtureDemandCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 领用状态 ReceiveState
        /// <summary>
        /// 领用状态
        /// </summary>
        [Label("领用状态")]
        public static readonly Property<ReceiveState?> ReceiveStateProperty = P<FixtureDemandCriteria>.Register(e => e.ReceiveState);

        /// <summary>
        /// 领用状态
        /// </summary>
        public ReceiveState? ReceiveState
        {
            get { return GetProperty(ReceiveStateProperty); }
            set { SetProperty(ReceiveStateProperty, value); }
        }
        #endregion

        #region 出库状态 DemandState
        /// <summary>
        /// 出库状态
        /// </summary>
        [Label("出库状态")]
        public static readonly Property<DemandState?> DemandStateProperty = P<FixtureDemandCriteria>.Register(e => e.DemandState);

        /// <summary>
        /// 出库状态
        /// </summary>
        public DemandState? DemandState
        {
            get { return GetProperty(DemandStateProperty); }
            set { SetProperty(DemandStateProperty, value); }
        }
        #endregion

        #region 创建人 Employee
        /// <summary>
        /// 创建人Id
        /// </summary>
        public static readonly IRefIdProperty EmployeeIdProperty = P<FixtureDemandCriteria>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 创建人Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<FixtureDemandCriteria>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<FixtureDemandCriteria>.Register(e => e.CreateDate);

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
        /// 获取工治具需求清单列表
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ElecFixtureController>().GetFixtureDemands(this);
        }
    }
}
