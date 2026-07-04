using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Fixtures.Repairs
{
    /// <summary>
	/// 工治具报修
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureRepairCriteria))]
    [EntityWithConfig(typeof(NoConfig))]
    [Label("工治具报修")]
    public partial class FixtureRepair : DataEntity
    {
        #region 维修单号 No
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> NoProperty = P<FixtureRepair>.Register(e => e.No);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 维修完成时间 RepairDate
        /// <summary>
        /// 维修完成时间
        /// </summary>
        [Label("维修完成时间")]
        public static readonly Property<DateTime?> RepairDateProperty = P<FixtureRepair>.Register(e => e.RepairDate);

        /// <summary>
        /// 维修完成时间
        /// </summary>
        public DateTime? RepairDate
        {
            get { return GetProperty(RepairDateProperty); }
            set { SetProperty(RepairDateProperty, value); }
        }
        #endregion

        #region 报修时间 ApplyDate
        /// <summary>
        /// 报修时间
        /// </summary>
        [Label("报修时间")]
        public static readonly Property<DateTime> ApplyDateProperty = P<FixtureRepair>.Register(e => e.ApplyDate);

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateTime ApplyDate
        {
            get { return GetProperty(ApplyDateProperty); }
            set { SetProperty(ApplyDateProperty, value); }
        }
        #endregion

        #region 治具异常详情 Details
        /// <summary>
        /// 治具异常详情
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureRepairDetail>> DetailsProperty = P<FixtureRepair>.RegisterList(e => e.Details);
        /// <summary>
        /// 治具异常详情
        /// </summary>
        public EntityList<FixtureRepairDetail> Details
        {
            get { return this.GetLazyList(DetailsProperty); }
        }
        #endregion

        #region 报修人 ApplyBy
        /// <summary>
        /// 报修人Id
        /// </summary>
        public static readonly IRefIdProperty ApplyByIdProperty = P<FixtureRepair>.RegisterRefId(e => e.ApplyById, ReferenceType.Normal);

        /// <summary>
        /// 报修人Id
        /// </summary>
        public double ApplyById
        {
            get { return (double)GetRefId(ApplyByIdProperty); }
            set { SetRefId(ApplyByIdProperty, value); }
        }

        /// <summary>
        /// 报修人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplyByProperty = P<FixtureRepair>.RegisterRef(e => e.ApplyBy, ApplyByIdProperty);

        /// <summary>
        /// 报修人
        /// </summary>
        public Employee ApplyBy
        {
            get { return GetRefEntity(ApplyByProperty); }
            set { SetRefEntity(ApplyByProperty, value); }
        }
        #endregion

        #region 维修人 RepairBy
        /// <summary>
        /// 维修人Id
        /// </summary>
        public static readonly IRefIdProperty RepairByIdProperty = P<FixtureRepair>.RegisterRefId(e => e.RepairById, ReferenceType.Normal);

        /// <summary>
        /// 维修人Id
        /// </summary>
        public double? RepairById
        {
            get { return (double?)GetRefNullableId(RepairByIdProperty); }
            set { SetRefNullableId(RepairByIdProperty, value); }
        }

        /// <summary>
        /// 维修人
        /// </summary>
        public static readonly RefEntityProperty<Employee> RepairByProperty = P<FixtureRepair>.RegisterRef(e => e.RepairBy, RepairByIdProperty);

        /// <summary>
        /// 维修人
        /// </summary>
        public Employee RepairBy
        {
            get { return GetRefEntity(RepairByProperty); }
            set { SetRefEntity(RepairByProperty, value); }
        }
        #endregion

        #region 单据状态 RepairState
        /// <summary>
        /// 单据状态
        /// </summary>
        [Label("单据状态")]
        public static readonly Property<RepairState> RepairStateProperty = P<FixtureRepair>.Register(e => e.RepairState);

        /// <summary>
        /// 单据状态
        /// </summary>
        public RepairState RepairState
        {
            get { return GetProperty(RepairStateProperty); }
            set { SetProperty(RepairStateProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 报修人 ApplyByName
        /// <summary>
        /// 报修人
        /// </summary>
        [Label("报修人")]
        public static readonly Property<string> ApplyByNameProperty = P<FixtureRepair>.RegisterView(e => e.ApplyByName, p => p.ApplyBy.Name);

        /// <summary>
        /// 报修人
        /// </summary>
        public string ApplyByName
        {
            get { return this.GetProperty(ApplyByNameProperty); }
            set { this.SetProperty(ApplyByNameProperty, value); }
        }

        #endregion

        #region 维修人 RepairByName
        /// <summary>
        /// 维修人
        /// </summary>
        [Label("维修人")]
        public static readonly Property<string> RepairByNameProperty = P<FixtureRepair>.RegisterView(e => e.RepairByName, p => p.RepairBy.Name);

        /// <summary>
        /// 维修人
        /// </summary>
        public string RepairByName
        {
            get { return this.GetProperty(RepairByNameProperty); }
            set { this.SetProperty(RepairByNameProperty, value); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 工治具报修 实体配置
    /// </summary>
    internal class FixtureRepairConfig : EntityConfig<FixtureRepair>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_FIX_REPAIR").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
