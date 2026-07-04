using SIE;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.MES.WorkOrders.Commands;
using SIE.Wpf.MES.WorkOrders.ViewModels;
using System;

namespace SIE.Wpf.MES.WorkOrders.ViewModels
{
    /// <summary>
    /// 导入工单 实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("导入失败数据")]
    public class WorkOrderCheckDataViewModel : ViewModel
    {
        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> NoProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        public static readonly Property<string> ProductProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.Product);

        /// <summary>
        /// 产品
        /// </summary>
        public string Product
        {
            get { return GetProperty(ProductProperty); }
            set { SetProperty(ProductProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<string> PlanQtyProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public string PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 工单类型 Type
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<string> TypeProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public string Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<string> PlanBeginDateProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public string PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndDate
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<string> PlanEndDateProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public string PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopProperty =
            P<WorkOrderCheckDataViewModel>.Register(e => e.WorkShop);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop
        {
            get { return this.GetProperty(WorkShopProperty); }
            set { this.SetProperty(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.Resource);

        /// <summary>
        /// 资源
        /// </summary>
        public string Resource
        {
            get { return GetProperty(ResourceProperty); }
            set { SetProperty(ResourceProperty, value); }
        }
        #endregion

        #region 上一级工单 ParentId
        /// <summary>
        /// 上一级工单
        /// </summary>
        [Label("上级工单编码")]
        public static readonly Property<string> ParentCodeProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.ParentCode);

        /// <summary>
        /// 上一级工单
        /// </summary>
        public string ParentCode
        {
            get { return GetProperty(ParentCodeProperty); }
            set { SetProperty(ParentCodeProperty, value); }
        }
        #endregion

        #region ERP工单 ErpWorkOrder
        /// <summary>
        /// ERP工单
        /// </summary>
        [Label("ERP工单")]
        public static readonly Property<string> ErpWorkOrderProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.ErpWorkOrder);

        /// <summary>
        /// ERP工单
        /// </summary>
        public string ErpWorkOrder
        {
            get { return GetProperty(ErpWorkOrderProperty); }
            set { SetProperty(ErpWorkOrderProperty, value); }
        }
        #endregion

        #region 客户订单号 CustomerOrderNo
        /// <summary>
        /// 客户订单号
        /// </summary>
        [Label("客户订单号")]
        public static readonly Property<string> CustomerOrderNoProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.CustomerOrderNo);

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerOrderNo
        {
            get { return GetProperty(CustomerOrderNoProperty); }
            set { SetProperty(CustomerOrderNoProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return GetProperty(SaleOrderNoProperty); }
            set { SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 订单数量 OrderQty
        /// <summary>
        /// 订单数量
        /// </summary>
        [Label("订单数量")]
        public static readonly Property<string> OrderQtyProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.OrderQty);

        /// <summary>
        /// 订单数量
        /// </summary>
        public string OrderQty
        {
            get { return GetProperty(OrderQtyProperty); }
            set { SetProperty(OrderQtyProperty, value); }
        }
        #endregion

        #region ErrorMessage 导入失败原因
        /// <summary>
        /// 导入失败原因
        /// </summary>
        [Label("导入失败原因")]
        public static readonly Property<string> ErrorMessageProperty = P<WorkOrderCheckDataViewModel>.Register(e => e.ErrorMessage);

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public string ErrorMessage
        {
            get { return this.GetProperty(ErrorMessageProperty); }
            set { this.SetProperty(ErrorMessageProperty, value); }
        }
        #endregion
    }
}

/// <summary>
/// 导入工单 视图配置
/// </summary>
class WorkOrderCheckDataViewModelConfig : WPFViewConfig<WorkOrderCheckDataViewModel>
{
    /// <summary>
    /// 默认视图
    /// </summary>
    protected override void ConfigView()
    {
        View.AssignAuthorize(typeof(ImportWorkOrderCheckViewModel));
    }

    /// <summary>
    /// 列表视图
    /// </summary>
    protected override void ConfigListView()
    {
        View.ClearCommands();
        View.UseCommands(typeof(ExportFailedDataCommand));
        View.Property(p => p.ErrorMessage);
        View.Property(p => p.No);
        View.Property(p => p.Product);
        View.Property(p => p.PlanQty);
        View.Property(p => p.Type);
        View.Property(p => p.PlanBeginDate);
        View.Property(p => p.PlanEndDate);
        View.Property(p => p.WorkShop);
        View.Property(p => p.Resource);
        View.Property(p => p.ParentCode);
        View.Property(p => p.ErpWorkOrder);
        View.Property(p => p.CustomerOrderNo);
        View.Property(p => p.SaleOrderNo);
        View.Property(p => p.OrderQty);
    }
}