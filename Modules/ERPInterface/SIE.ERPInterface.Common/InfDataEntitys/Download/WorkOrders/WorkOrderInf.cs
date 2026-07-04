using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 工单中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工单中间表")]
    public partial class WorkOrderInf : DownloadBaseEntity
    {
        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<WorkOrderInf>.Register(e => e.No);

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
        public static readonly Property<DateTime> PlanBeginDateProperty = P<WorkOrderInf>.Register(e => e.PlanBeginDate);

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
        public static readonly Property<DateTime> PlanEndDateProperty = P<WorkOrderInf>.Register(e => e.PlanEndDate);

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
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WorkOrderInf>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 客户订单号 CustomerOrderNo
        /// <summary>
        /// 客户订单号
        /// </summary>
        [Label("客户订单号")]
        public static readonly Property<string> CustomerOrderNoProperty = P<WorkOrderInf>.Register(e => e.CustomerOrderNo);

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerOrderNo
        {
            get { return GetProperty(CustomerOrderNoProperty); }
            set { SetProperty(CustomerOrderNoProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<WorkOrderInf>.Register(e => e.CustomerCode);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerCode
        {
            get { return GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 订单数量 OrderQty
        /// <summary>
        /// 订单数量
        /// </summary>
        [Label("订单数量")]
        public static readonly Property<decimal> OrderQtyProperty = P<WorkOrderInf>.Register(e => e.OrderQty);

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal OrderQty
        {
            get { return GetProperty(OrderQtyProperty); }
            set { SetProperty(OrderQtyProperty, value); }
        }
        #endregion

        #region 制单时间 MakeDate
        /// <summary>
        /// 制单时间
        /// </summary>
        [Label("制单时间")]
        public static readonly Property<DateTime> MakeDateProperty = P<WorkOrderInf>.Register(e => e.MakeDate);

        /// <summary>
        /// 制单时间
        /// </summary>
        public DateTime MakeDate
        {
            get { return GetProperty(MakeDateProperty); }
            set { SetProperty(MakeDateProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<WorkOrderInf>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return GetProperty(SaleOrderNoProperty); }
            set { SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WorkOrderInf>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 车间编码 WorkshopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkshopCodeProperty = P<WorkOrderInf>.Register(e => e.WorkshopCode);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkshopCode
        {
            get { return GetProperty(WorkshopCodeProperty); }
            set { SetProperty(WorkshopCodeProperty, value); }
        }
        #endregion

        #region 更新人编码 UpdateByCode
        /// <summary>
        /// 更新人编码
        /// </summary>
        [Label("更新人编码")]
        public static readonly Property<string> UpdateByCodeProperty = P<WorkOrderInf>.Register(e => e.UpdateByCode);

        /// <summary>
        /// 更新人编码
        /// </summary>
        public string UpdateByCode
        {
            get { return GetProperty(UpdateByCodeProperty); }
            set { SetProperty(UpdateByCodeProperty, value); }
        }
        #endregion

        #region 关闭编码 ClosedCode
        /// <summary>
        /// 关闭编码
        /// </summary>
        [Label("关闭编码")]
        public static readonly Property<string> ClosedCodeProperty = P<WorkOrderInf>.Register(e => e.ClosedCode);

        /// <summary>
        /// 关闭编码
        /// </summary>
        public string ClosedCode
        {
            get { return GetProperty(ClosedCodeProperty); }
            set { SetProperty(ClosedCodeProperty, value); }
        }
        #endregion

        #region 工单类型 WorkOrderType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<string> WorkOrderTypeProperty = P<WorkOrderInf>.Register(e => e.WorkOrderType);

        /// <summary>
        /// 工单类型
        /// </summary>
        public string WorkOrderType
        {
            get { return GetProperty(WorkOrderTypeProperty); }
            set { SetProperty(WorkOrderTypeProperty, value); }
        }
        #endregion

        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<string> WorkOrderStateProperty = P<WorkOrderInf>.Register(e => e.WorkOrderState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public string WorkOrderState
        {
            get { return this.GetProperty(WorkOrderStateProperty); }
            set { this.SetProperty(WorkOrderStateProperty, value); }
        }
        #endregion

        #region 建单人编码 MakerCode
        /// <summary>
        /// 建单人编码
        /// </summary>
        [Label("建单人编码")]
        public static readonly Property<string> MakerCodeProperty = P<WorkOrderInf>.Register(e => e.MakerCode);

        /// <summary>
        /// 建单人编码
        /// </summary>
        public string MakerCode
        {
            get { return this.GetProperty(MakerCodeProperty); }
            set { this.SetProperty(MakerCodeProperty, value); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<WorkOrderInf>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单中间表 实体配置
    /// </summary>
    internal class WorkOrderInfConfig : EntityConfig<WorkOrderInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_WO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}