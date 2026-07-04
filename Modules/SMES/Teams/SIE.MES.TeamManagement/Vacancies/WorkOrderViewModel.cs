using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.Vacancies
{
    /// <summary>
    /// 工单带班组
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单")]
    public class WorkOrderViewModel : ViewModel
    {
        #region 工单编号 No
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> NoProperty = P<WorkOrderViewModel>.Register(e => e.No);

        /// <summary>
        /// 工单编号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WorkOrderViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品机型 ProductModel
        /// <summary>
        /// 产品机型
        /// </summary>
        [Label("产品机型")]
        public static readonly Property<string> ProductModelProperty = P<WorkOrderViewModel>.Register(e => e.ProductModel);

        /// <summary>
        /// 产品机型
        /// </summary>
        public string ProductModel
        {
            get { return this.GetProperty(ProductModelProperty); }
            set { this.SetProperty(ProductModelProperty, value); }
        }
        #endregion

        #region 产品机型Id ProductModelId
        /// <summary>
        /// 产品机型Id 
        /// </summary>
        [Label("产品机型Id")]
        public static readonly Property<double?> ProductModelIdProperty = P<WorkOrderViewModel>.Register(e => e.ProductModelId);

        /// <summary>
        /// 产品机型Id 
        /// </summary>
        public double? ProductModelId
        {
            get { return this.GetProperty(ProductModelIdProperty); }
            set { this.SetProperty(ProductModelIdProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WorkOrderViewModel>.Register(e => e.ProductName);

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
        public static readonly Property<decimal> PlanQtyProperty = P<WorkOrderViewModel>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<WorkOrderViewModel>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 工单状态 StateName
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<string> StateNameProperty = P<WorkOrderViewModel>.Register(e => e.StateName);

        /// <summary>
        /// 工单状态
        /// </summary>
        public string StateName
        {
            get { return this.GetProperty(StateNameProperty); }
            set { this.SetProperty(StateNameProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<WorkOrderViewModel>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
            set { this.SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划结束时间 PlanEndDate
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<WorkOrderViewModel>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return this.GetProperty(PlanEndDateProperty); }
            set { this.SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<WorkOrderViewModel>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<WorkOrderViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 缺编Id VacancyId
        /// <summary>
        /// 缺编Id
        /// </summary>
        [Label("缺编Id")]
        public static readonly Property<double> VacancyIdProperty = P<WorkOrderViewModel>.Register(e => e.VacancyId);

        /// <summary>
        /// 缺编Id
        /// </summary>
        public double VacancyId
        {
            get { return this.GetProperty(VacancyIdProperty); }
            set { this.SetProperty(VacancyIdProperty, value); }
        }
        #endregion

        #region 出勤员工集合 EmployeeIds
        /// <summary>
        /// 出勤员工集合
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> EmployeeIdsProperty = P<WorkOrderViewModel>.Register(e => e.EmployeeIds);

        /// <summary>
        /// 出勤员工集合
        /// </summary>
        public string EmployeeIds
        {
            get { return this.GetProperty(EmployeeIdsProperty); }
            set { this.SetProperty(EmployeeIdsProperty, value); }
        }
        #endregion        
    }
}
