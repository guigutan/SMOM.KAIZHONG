using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.Distributions
{
    /// <summary>
    /// 配送设置
    /// </summary>
    [QueryEntity, Serializable]
    [Label("配送设置查询实体")]
    public class DistributionSettingCriteria : Criteria
    {         
        #region 目标产线 ProductLine
        /// <summary>
        /// 目标产线Id
        /// </summary>
        [Label("目标产线")]
        public static readonly IRefIdProperty ProductLineIdProperty =
            P<DistributionSettingCriteria>.RegisterRefId(e => e.ProductLineId, ReferenceType.Normal);

        /// <summary>
        /// 目标产线Id
        /// </summary>
        public double? ProductLineId
        {
            get { return (double?)this.GetRefNullableId(ProductLineIdProperty); }
            set { this.SetRefNullableId(ProductLineIdProperty, value); }
        }

        /// <summary>
        /// 目标产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ProductLineProperty =
            P<DistributionSettingCriteria>.RegisterRef(e => e.ProductLine, ProductLineIdProperty);

        /// <summary>
        /// 目标产线
        /// </summary>
        public WipResource ProductLine
        {
            get { return this.GetRefEntity(ProductLineProperty); }
            set { this.SetRefEntity(ProductLineProperty, value); }
        }
        #endregion

        #region 来源仓库 Warehouse
        /// <summary>
        /// 来源仓库Id
        /// </summary>
        [Label("来源仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<DistributionSettingCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 来源仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 来源仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<DistributionSettingCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 来源仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<DistributionSettingCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<DistributionSettingCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 创建人 CreateBy
        /// <summary>
        /// 创建人Id
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty CreateByIdProperty = P<DistributionSettingCriteria>.RegisterRefId(e => e.CreateById, ReferenceType.Normal);

        /// <summary>
        /// 创建人Id
        /// </summary>
        public double? CreateById
        {
            get { return (double?)GetRefNullableId(CreateByIdProperty); }
            set { SetRefNullableId(CreateByIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreateByProperty = P<DistributionSettingCriteria>.RegisterRef(e => e.CreateBy, CreateByIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee CreateBy
        {
            get { return GetRefEntity(CreateByProperty); }
            set { SetRefEntity(CreateByProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DistributionController>().GetDistributionSettings(this);
        }
    }
}
