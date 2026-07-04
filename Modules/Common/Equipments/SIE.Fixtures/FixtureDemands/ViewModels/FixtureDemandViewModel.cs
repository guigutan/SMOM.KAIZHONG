using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.FixtureDemands.ViewModels
{
    /// <summary>
    /// 工治具需求清单ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("工治具需求清单信息")]
    public class FixtureDemandViewModel : Entity<double>
    {
        #region 需求单号 No
        /// <summary>
        /// 需求单号
        /// </summary>
        [Label("需求单号")]
        public static readonly Property<string> NoProperty = P<FixtureDemandViewModel>.Register(e => e.No);

        /// <summary>
        /// 需求单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<FixtureDemandViewModel>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
            set { SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 产线 ResourceName
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResourceNameProperty = P<FixtureDemandViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<FixtureDemandViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品 ProductCode
        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        public static readonly Property<string> ProductCodeProperty = P<FixtureDemandViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<FixtureDemandViewModel>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<FixtureDemandViewModel>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion
    }
}
