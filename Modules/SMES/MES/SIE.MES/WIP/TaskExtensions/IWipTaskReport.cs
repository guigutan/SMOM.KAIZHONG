using SIE.Domain;
using SIE.Services;

namespace SIE.MES.WIP.TaskExtensions
{
    /// <summary>
    /// 任务
    /// </summary>
    [Service(FallbackType = typeof(DefaultAutoTaskReport))]
    public interface IWipTaskReport
    {
        /// <summary>
        /// 验证工单任务自动报工
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="employeeId">员工Id</param>
        /// <param name="processId">工序ID</param>
        /// <returns>验证通过返回true，失败抛异常</returns>
        bool ValidateAutoReport(double workOrderId, double employeeId, double processId);

        /// <summary>
        /// 获取工单报工任务
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="retrospectType">追溯方式</param>
        /// <param name="processId">工序Id</param>
        /// <returns>报工任务列表</returns>
        EntityList<ReportTaskViewModel> GetReportTasks(double employeeId, Core.Items.RetrospectType retrospectType, double processId);

        /// <summary>
        /// 是否任务单生产模式
        /// </summary>
        /// <returns>任务单生产模式返回true，否则返回false</returns>
        bool IsTaskWip();

        /// <summary>
        /// 工单是否自动报工
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>自动报工返回true，否则返回false</returns>
        int? GetWorkOrdeReportModel(double workOrderId);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    class DefaultAutoTaskReport : IWipTaskReport
    {
        /// <summary>
        /// 获取工单报工任务
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="retrospectType">追溯方式</param>
        /// <param name="processId">工序Id</param>
        /// <returns>报工任务列表</returns>
        public EntityList<ReportTaskViewModel> GetReportTasks(double employeeId, Core.Items.RetrospectType retrospectType, double processId)
        {
            return new EntityList<ReportTaskViewModel>();
        }

        /// <summary>
        /// 是否任务单生产模式
        /// </summary>
        /// <returns>任务单生产模式返回true，否则返回false</returns>
        public bool IsTaskWip()
        {
            return false;
        }

        /// <summary>
        /// 工单是否自动报工
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>自动报工返回true，否则返回false</returns>
        public int? GetWorkOrdeReportModel(double workOrderId)
        {
            return null;
        }

        /// <summary>
        /// 验证工单任务自动报工
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="employeeId">员工Id</param>
        /// <param name="processId">工序ID</param>
        /// <returns>验证通过返回true，失败抛异常</returns>
        public bool ValidateAutoReport(double workOrderId, double employeeId, double processId)
        {
            return true;
        }
    }
}