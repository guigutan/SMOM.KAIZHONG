using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// ERP工单
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("ERP工单")]
    [DisplayMember(nameof(No))]
    public partial class ErpWorkOrder : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpWorkOrder()
        {
            this.PlanQty = 0;
            this.PlanBeginDate = DateTime.Today;
            this.PlanEndDate = DateTime.Today;
            this.ActualFinishDate = DateTime.Today.AddDays(1);
        }
        #endregion

        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("ERP工单号")]
        public static readonly Property<string> NoProperty = P<ErpWorkOrder>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 计划开始日期 PlanBeginDate
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [Label("计划开始日期")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<ErpWorkOrder>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始日期
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划完成日期 PlanEndDate
        /// <summary>
        /// 计划完成日期
        /// </summary>
        [Label("计划完成日期")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<ErpWorkOrder>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划完成日期
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [MinValue(0)]
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<ErpWorkOrder>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 实际完成时间 ActualFinishDate
        /// <summary>
        /// 实际完成时间
        /// </summary>
        [Label("实际完成时间")]
        public static readonly Property<DateTime> ActualFinishDateProperty = P<ErpWorkOrder>.Register(e => e.ActualFinishDate);

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime ActualFinishDate
        {
            get { return GetProperty(ActualFinishDateProperty); }
            set { SetProperty(ActualFinishDateProperty, value); }
        }
        #endregion

        #region ERP订单 ErpSaleOrder
        /// <summary>
        /// ERP订单Id
        /// </summary>
        [Label("ERP订单")]
        public static readonly IRefIdProperty ErpSaleOrderIdProperty = P<ErpWorkOrder>.RegisterRefId(e => e.ErpSaleOrderId, ReferenceType.Normal);

        /// <summary>
        /// ERP订单Id
        /// </summary>
        public double ErpSaleOrderId
        {
            get { return (double)GetRefId(ErpSaleOrderIdProperty); }
            set { SetRefId(ErpSaleOrderIdProperty, value); }
        }

        /// <summary>
        /// ERP订单
        /// </summary>
        public static readonly RefEntityProperty<ErpSaleOrder> ErpSaleOrderProperty = P<ErpWorkOrder>.RegisterRef(e => e.ErpSaleOrder, ErpSaleOrderIdProperty);

        /// <summary>
        /// ERP订单
        /// </summary>
        public ErpSaleOrder ErpSaleOrder
        {
            get { return GetRefEntity(ErpSaleOrderProperty); }
            set { SetRefEntity(ErpSaleOrderProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ErpWorkOrder>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// ERP工单 实体配置
    /// </summary>
    internal class ErpWorkOrderConfig : EntityConfig<ErpWorkOrder>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ERP_WO").MapAllProperties();
            Meta.Property(ErpWorkOrder.NoProperty).ColumnMeta.HasIndex();
            Meta.IndexGroupOnProperties(ErpWorkOrder.ProductCodeProperty, ErpWorkOrder.PlanBeginDateProperty);
            Meta.EnablePhantoms();
        }
    }
}