using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库位库存
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LocationOnhandCriteria))]
    [Label("库位库存")]
    [DisplayMember(nameof(Id))]
    public partial class LocationOnhand : BaseOnhand
    {
        #region 货主编码 StorerCode
        /// <summary>
        /// 货主编码
        /// </summary>
        [Label("货主编码")]
        public static readonly Property<string> StorerCodeProperty = P<LocationOnhand>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LocationOnhand>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 任务号 TaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> TaskNoProperty = P<LocationOnhand>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 库位库存 实体配置
    /// </summary>
    internal class LocationOnhandConfig : EntityConfig<LocationOnhand>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            //Meta.MapTable("INV_LOC_ONHAND").MapAllProperties();
            //Meta.EnablePhantoms();

            Func<IQuery> view = () => DB.Query<LotLpnOnhand>()
                .Select(p => new
                {
                    id = p.SQL<double>("0 id"),
                    Qty = p.Qty.SUM(),
                    Available_Qty = p.AvailableQty.SUM(),
                    Freezing_Qty = p.FreezingQty.SUM(),
                    Allotted_Qty = p.AllottedQty.SUM(),
                    Storer_Code = p.StorerCode,
                    Project_No = p.ProjectNo,
                    Task_No = p.TaskNo,
                    Warehouse_Id = p.WarehouseId,
                    Storage_Area_Id = p.StorageAreaId,
                    Storage_Location_Id = p.StorageLocationId,
                    Item_Id = p.ItemId,
                    Item_Ext_Prop_Name = p.ItemExtPropName,
                    Create_By = p.SQL<double>("0 Create_By"),                    
                    Update_By = p.SQL<double>("0 Update_By"),
                    Create_Date=p.SQL<string>("NULL Create_Date"),
                    Update_Date = p.SQL<string>("NULL Update_Date"),
                })
                .GroupBy(p => new { p.StorerCode, p.ProjectNo, p.TaskNo, p.WarehouseId, p.StorageAreaId, p.StorageLocationId, p.ItemId, p.ItemExtPropName })
               .ToQuery();

            Meta.MapView(view).MapAllProperties();
        }
    }
}