using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.PanelBindings
{
    /// <summary>
    /// MES工单条码绑定记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PanelBindingRecordCriteria))]
    [Label("MES工单条码绑定记录")]
    public class PanelBindingRecord : Entity<double>
    {
        #region MES工单号 No
        /// <summary>
        /// MES工单号
        /// </summary>
        [Label("MES工单号")]
        public static readonly Property<string> NoProperty = P<PanelBindingRecord>.Register(e => e.No);

        /// <summary>
        /// MES工单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double?> ProductIdProperty = P<PanelBindingRecord>.Register(e => e.ProductId);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double? ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<PanelBindingRecord>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<PanelBindingRecord>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<PanelBindingRecord>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 拼板数 PanelQty
        /// <summary>
        /// 拼板数
        /// </summary>
        [Label("拼板数")]
        public static readonly Property<decimal?> PanelQtyProperty = P<PanelBindingRecord>.Register(e => e.PanelQty);

        /// <summary>
        /// 拼板数
        /// </summary>
        public decimal? PanelQty
        {
            get { return this.GetProperty(PanelQtyProperty); }
            set { this.SetProperty(PanelQtyProperty, value); }
        }
        #endregion

        #region 已绑定数量 BindQty
        /// <summary>
        /// 已绑定数量
        /// </summary>
        [Label("已绑定数量")]
        public static readonly Property<int> BindQtyProperty = P<PanelBindingRecord>.Register(e => e.BindQty);

        /// <summary>
        /// 已绑定数量
        /// </summary>
        public int BindQty
        {
            get { return this.GetProperty(BindQtyProperty); }
            set { this.SetProperty(BindQtyProperty, value); }
        }
        #endregion

        //#region 未绑定数量 UnBindSnQty
        ///// <summary>
        ///// 未绑定数量
        ///// </summary>
        //[Label("未绑定数量")]
        //public static readonly Property<int> UnBindSnQtyProperty = P<PanelBindingRecord>.Register(e => e.UnBindSnQty);

        ///// <summary>
        ///// 未绑定数量
        ///// </summary>
        //public int UnBindSnQty
        //{
        //    get { return this.GetProperty(UnBindSnQtyProperty); }
        //    set { this.SetProperty(UnBindSnQtyProperty, value); }
        //}
        //#endregion

        #region 车间Id WorkShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double?> WorkShopIdProperty = P<PanelBindingRecord>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return this.GetProperty(WorkShopIdProperty); }
            set { this.SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<PanelBindingRecord>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 资源Id ResourceId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double?> ResourceIdProperty = P<PanelBindingRecord>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
            set { this.SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<PanelBindingRecord>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<Core.WorkOrders.WorkOrderState> StateProperty = P<PanelBindingRecord>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public Core.WorkOrders.WorkOrderState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<PanelBindingRecord>.Register(e => e.PlanBeginDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class PanelBindingRecordConfig : EntityConfig<PanelBindingRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<WorkOrder>("w")
            .LeftJoin<Item>((w, i) => w.ProductId == i.Id)
            .LeftJoin<Enterprise>((w, e) => w.WorkShopId == e.Id)
            .LeftJoin<WipResource>((w, r) => w.ResourceId == r.Id)
            .LeftJoin<PanelBindingBarcode>("b1", (w, b1) => w.Id == b1.WorkOrderId)
            .Select<Item, Enterprise, WipResource, PanelBindingBarcode, PanelBindingBarcode>(
                (w, i, e, r, b1, b2) => new
                {
                    w.Id,
                    No = w.No,
                    Product_Id = i.Id,
                    Product_Code = i.Code,
                    Product_Name = i.Name,
                    Plan_Qty = w.PlanQty,
                    Panel_Qty = w.SQL<int?>("w.Panel_Qty"),
                    Printed_Qty = w.PrintedQty,
                    Bind_Qty = b1.Qty,
                    Work_Shop_Id = e.Id,
                    Work_Shop_Name = e.Name,
                    Resource_Id = r.Id,
                    Resource_Name = r.Name,
                    State = w.State,
                    Plan_Begin_Date = w.PlanBeginDate
                })
            .ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.DisablePhantoms();
        }
    }
}
