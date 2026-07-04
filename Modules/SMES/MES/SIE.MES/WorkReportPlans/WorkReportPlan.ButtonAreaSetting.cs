using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.MES.WorkReportPlans
{

    /// <summary>
    /// 报工按钮区域设置
    /// </summary>
    public partial class WorkReportPlan
    {
        #region 派工任务 IsDispatchTask
        /// <summary>
        /// 派工任务
        /// </summary>
        [Label("派工任务")]
        public static readonly Property<bool> IsDispatchTaskProperty = P<WorkReportPlan>.Register(e => e.IsDispatchTask);

        /// <summary>
        /// 派工任务
        /// </summary>
        public bool IsDispatchTask
        {
            get { return this.GetProperty(IsDispatchTaskProperty); }
            set { this.SetProperty(IsDispatchTaskProperty, value); }
        }
        #endregion



        #region 生产报工 IsProductionReport
        /// <summary>
        /// 生产报工
        /// </summary>
        [Label("生产报工")]
        public static readonly Property<bool> IsProductionReportProperty = P<WorkReportPlan>.Register(e => e.IsProductionReport);

        /// <summary>
        /// 生产报工
        /// </summary>
        public bool IsProductionReport
        {
            get { return this.GetProperty(IsProductionReportProperty); }
            set { this.SetProperty(IsProductionReportProperty, value); }
        }
        #endregion
        

        #region 设备点检 IsDeviceInspection
        /// <summary>
        /// 设备点检
        /// </summary>
        [Label("设备点检")]
        public static readonly Property<bool> IsDeviceInspectionProperty = P<WorkReportPlan>.Register(e => e.IsDeviceInspection);

        /// <summary>
        /// 设备点检
        /// </summary>
        public bool IsDeviceInspection
        {
            get { return this.GetProperty(IsDeviceInspectionProperty); }
            set { this.SetProperty(IsDeviceInspectionProperty, value); }
        }
        #endregion

        #region 模具操作 IsMoldOperation
        /// <summary>
        /// 模具操作
        /// </summary>
        [Label("模具操作")]
        public static readonly Property<bool> IsMoldOperationProperty = P<WorkReportPlan>.Register(e => e.IsMoldOperation);

        /// <summary>
        /// 模具操作
        /// </summary>
        public bool IsMoldOperation
        {
            get { return this.GetProperty(IsMoldOperationProperty); }
            set { this.SetProperty(IsMoldOperationProperty, value); }
        }
        #endregion

        #region 物料操作 IsMaterialOperation
        /// <summary>
        /// 物料操作
        /// </summary>
        [Label("物料操作")]
        public static readonly Property<bool> IsMaterialOperationProperty = P<WorkReportPlan>.Register(e => e.IsMaterialOperation);

        /// <summary>
        /// 物料操作
        /// </summary>
        public bool IsMaterialOperation
        {
            get { return this.GetProperty(IsMaterialOperationProperty); }
            set { this.SetProperty(IsMaterialOperationProperty, value); }
        }
        #endregion

        #region 异常呼叫 ExceptionCall
        /// <summary>
        /// 异常呼叫
        /// </summary>
        [Label("异常呼叫")]
        public static readonly Property<bool> ExceptionCallProperty = P<WorkReportPlan>.Register(e => e.ExceptionCall);

        /// <summary>
        /// 异常呼叫
        /// </summary>
        public bool ExceptionCall
        {
            get { return this.GetProperty(ExceptionCallProperty); }
            set { this.SetProperty(ExceptionCallProperty, value); }
        }
        #endregion

        #region 异常响应 ExceptionResponse
        /// <summary>
        /// 异常响应
        /// </summary>
        [Label("异常响应")]
        public static readonly Property<bool> ExceptionResponseProperty = P<WorkReportPlan>.Register(e => e.ExceptionResponse);

        /// <summary>
        /// 异常响应
        /// </summary>
        public bool ExceptionResponse
        {
            get { return this.GetProperty(ExceptionResponseProperty); }
            set { this.SetProperty(ExceptionResponseProperty, value); }
        }
        #endregion


        #region 首件报检 IsShowFirstInsp
        /// <summary>
        /// 首件报价
        /// </summary>
        [Label("首件报检")]
        public static readonly Property<bool> IsShowFirstInspProperty = P<WorkReportPlan>.Register(e => e.IsShowFirstInsp);

        /// <summary>
        /// 首件报检
        /// </summary>
        public bool IsShowFirstInsp
        {
            get { return this.GetProperty(IsShowFirstInspProperty); }
            set { this.SetProperty(IsShowFirstInspProperty, value); }
        }
        #endregion

    }
}
