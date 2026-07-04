using SIE.Domain;
using SIE.Domain.Query;
using SIE.Inventory.Commom;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 批次库存
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LotOnhandCriteria))]
    [Label("批次库存")]
    [DisplayMember(nameof(Id))]
    public partial class LotOnhand : BaseOnhand
    {
        #region 货主编码 StorerCode
        /// <summary>
        /// 货主编码
        /// </summary>
        [Label("货主编码")]
        public static readonly Property<string> StorerCodeProperty = P<LotOnhand>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly IRefIdProperty LotIdProperty =
            P<LotOnhand>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

        /// <summary>
        /// 批次
        /// </summary>
        public double LotId
        {
            get { return (double)this.GetRefId(LotIdProperty); }
            set { this.SetRefId(LotIdProperty, value); }
        }

        /// <summary>
        /// 批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> LotProperty =
            P<LotOnhand>.RegisterRef(e => e.Lot, LotIdProperty);

        /// <summary>
        /// 批次
        /// </summary>
        public Lot Lot
        {
            get { return this.GetRefEntity(LotProperty); }
            set { this.SetRefEntity(LotProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<LotOnhand>.RegisterView(e => e.LotCode, p => p.Lot.Code);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LotOnhand>.Register(e => e.ProjectNo);

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
        public static readonly Property<string> TaskNoProperty = P<LotOnhand>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 批次属性01 LotAtt01
        /// <summary>
        /// 批次属性01
        /// </summary>
        [Label("批次属性01")]
        public static readonly Property<DateTime?> LotAtt01Property = P<LotOnhand>.RegisterView(e => e.LotAtt01, p => p.Lot.LotAtt01);

        /// <summary>
        /// 批次属性01
        /// </summary>
        public DateTime? LotAtt01
        {
            get { return this.GetProperty(LotAtt01Property); }
        }
        #endregion

        #region 批次属性02 LotAtt02
        /// <summary>
        /// 批次属性02
        /// </summary>
        [Label("批次属性02")]
        public static readonly Property<DateTime?> LotAtt02Property = P<LotOnhand>.RegisterView(e => e.LotAtt02, p => p.Lot.LotAtt02);

        /// <summary>
        /// 批次属性02
        /// </summary>
        public DateTime? LotAtt02
        {
            get { return this.GetProperty(LotAtt02Property); }
        }
        #endregion

        #region 批次属性03 LotAtt03
        /// <summary>
        /// 批次属性03
        /// </summary>
        [Label("批次属性03")]
        public static readonly Property<DateTime?> LotAtt03Property = P<LotOnhand>.RegisterView(e => e.LotAtt03, p => p.Lot.LotAtt03);

        /// <summary>
        /// 批次属性03
        /// </summary>
        public DateTime? LotAtt03
        {
            get { return this.GetProperty(LotAtt03Property); }
        }
        #endregion

        #region 批次属性04 LotAtt04
        /// <summary>
        /// 批次属性04
        /// </summary>
        [Label("批次属性04")]
        public static readonly Property<string> LotAtt04Property = P<LotOnhand>.RegisterView(e => e.LotAtt04, p => p.Lot.LotAtt04);

        /// <summary>
        /// 批次属性04
        /// </summary>
        public string LotAtt04
        {
            get { return this.GetProperty(LotAtt04Property); }
        }
        #endregion

        #region 批次属性05 LotAtt05
        /// <summary>
        /// 批次属性05
        /// </summary>
        [Label("批次属性05")]
        public static readonly Property<decimal?> LotAtt05Property = P<LotOnhand>.RegisterView(e => e.LotAtt05, p => p.Lot.LotAtt05);

        /// <summary>
        /// 批次属性05
        /// </summary>
        public decimal? LotAtt05
        {
            get { return this.GetProperty(LotAtt05Property); }
        }
        #endregion

        #region 批次属性06 LotAtt06
        /// <summary>
        /// 批次属性06
        /// </summary>
        [Label("批次属性06")]
        public static readonly Property<decimal?> LotAtt06Property = P<LotOnhand>.RegisterView(e => e.LotAtt06, p => p.Lot.LotAtt06);

        /// <summary>
        /// 批次属性06
        /// </summary>
        public decimal? LotAtt06
        {
            get { return this.GetProperty(LotAtt06Property); }
        }
        #endregion

        #region 批次属性07 LotAtt07
        /// <summary>
        /// 批次属性07
        /// </summary>
        [Label("批次属性07")]
        public static readonly Property<string> LotAtt07Property = P<LotOnhand>.RegisterView(e => e.LotAtt07, p => p.Lot.LotAtt07);

        /// <summary>
        /// 批次属性07
        /// </summary>
        public string LotAtt07
        {
            get { return this.GetProperty(LotAtt07Property); }
        }
        #endregion

        #region 批次属性08 LotAtt08
        /// <summary>
        /// 批次属性08
        /// </summary>
        [Label("批次属性08")]
        public static readonly Property<string> LotAtt08Property = P<LotOnhand>.RegisterView(e => e.LotAtt08, p => p.Lot.LotAtt08);

        /// <summary>
        /// 批次属性08
        /// </summary>
        public string LotAtt08
        {
            get { return this.GetProperty(LotAtt08Property); }
        }
        #endregion

        #region 批次属性09 LotAtt09
        /// <summary>
        /// 批次属性09
        /// </summary>
        [Label("批次属性09")]
        public static readonly Property<string> LotAtt09Property = P<LotOnhand>.RegisterView(e => e.LotAtt09, p => p.Lot.LotAtt09);

        /// <summary>
        /// 批次属性09
        /// </summary>
        public string LotAtt09
        {
            get { return this.GetProperty(LotAtt09Property); }
        }
        #endregion

        #region 批次属性10 LotAtt10
        /// <summary>
        /// 批次属性10
        /// </summary>
        [Label("批次属性10")]
        public static readonly Property<string> LotAtt10Property = P<LotOnhand>.RegisterView(e => e.LotAtt10, p => p.Lot.LotAtt10);

        /// <summary>
        /// 批次属性10
        /// </summary>
        public string LotAtt10
        {
            get { return this.GetProperty(LotAtt10Property); }
        }
        #endregion

        #region 批次属性11 LotAtt11
        /// <summary>
        /// 批次属性11
        /// </summary>
        [Label("批次属性11")]
        public static readonly Property<DateTime?> LotAtt11Property = P<LotOnhand>.RegisterView(e => e.LotAtt11, p => p.Lot.LotAtt11);

        /// <summary>
        /// 批次属性11
        /// </summary>
        public DateTime? LotAtt11
        {
            get { return this.GetProperty(LotAtt11Property); }
        }
        #endregion

        #region 批次属性12 LotAtt12
        /// <summary>
        /// 批次属性12
        /// </summary>
        [Label("批次属性12")]
        public static readonly Property<DateTime?> LotAtt12Property = P<LotOnhand>.RegisterView(e => e.LotAtt12, p => p.Lot.LotAtt12);

        /// <summary>
        /// 批次属性12
        /// </summary>
        public DateTime? LotAtt12
        {
            get { return this.GetProperty(LotAtt12Property); }
        }
        #endregion
    }

    /// <summary>
    /// 批次库存 实体配置
    /// </summary>
    internal class LotOnhandConfig : EntityConfig<LotOnhand>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            //Meta.MapTable("INV_LOT_ONHAND").MapAllProperties();
            //Meta.IndexGroupOnProperties(LotOnhand.StorageLocationIdProperty, LotOnhand.ItemIdProperty, LotOnhand.LotCodeProperty);
            //Meta.IndexGroupOnProperties(LotOnhand.StorageLocationIdProperty, LotOnhand.ItemIdProperty, LotOnhand.LotIdProperty);
            //Meta.EnablePhantoms();

            Func<IQuery> view = () => DB.Query<LotLpnOnhand>()
              .Select(p => new
              {
                  id = p.SQL<double>("0 id"),
                  Qty = p.Qty.SUM(),
                  Available_Qty = p.AvailableQty.SUM(),
                  Freezing_Qty = p.FreezingQty.SUM(),
                  Allotted_Qty = p.AllottedQty.SUM(),
                  Lot_Id = p.LotId,                     
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
                  Create_Date = p.SQL<string>("NULL Create_Date"),
                  Update_Date = p.SQL<string>("NULL Update_Date"),
              })
              .GroupBy(p => new { p.StorerCode, p.ProjectNo, p.TaskNo, p.WarehouseId, p.StorageAreaId, p.StorageLocationId, p.ItemId, p.ItemExtPropName,p.LotId })
             .ToQuery();

            Meta.MapView(view).MapAllProperties();
        }
    }
}