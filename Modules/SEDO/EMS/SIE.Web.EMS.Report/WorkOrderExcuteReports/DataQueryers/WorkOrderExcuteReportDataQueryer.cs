using SIE.EMS.Report.WorkOrderExcuteReports;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Report.WorkOrderExcuteReports.DataQueryers
{
    /// <summary>
    /// 工单执行报表查询器
    /// </summary>
   public class WorkOrderExcuteReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public WorkOrderExcuteReportInfo GetWorkOrderExcuteReportData(WorkOrderExcuteReportViewModelCriteria criteria)
        {
            WorkOrderExcuteReportInfo info = new WorkOrderExcuteReportInfo();
            if (!criteria.FactoryId.HasValue)
            {
                info.err = "查询条件【工厂】不能为空，请选择！".L10N();
                info.IsSuccess = false;
                return info;
            }
            if (!criteria.BeginMonth.HasValue)
            {
                info.err = "查询条件【开始月份】不能为空，请选择！".L10N();
                info.IsSuccess = false;
                return info;
            }
            if (!criteria.EndMonth.HasValue)
            {
                info.err = "查询条件【结束月份】不能为空，请选择！".L10N();
                info.IsSuccess = false;
                return info;
            }
            if (criteria.EndMonth < criteria.BeginMonth)
            {
                info.err = "【开始月份】须小于【结束月份】，请确认！".L10N();
                info.IsSuccess = false;
                return info;
            }
            if (criteria.EndMonth >= criteria.BeginMonth.Value.AddYears(1))
            {
                info.err = "【开始月份】【结束月份】必须为一年区间，请确认！".L10N();
                info.IsSuccess = false;
                return info;
            }
            return RT.Service.Resolve<WorkOrderExcuteReportViewModelController>().GetSpartPartMitReport(criteria);
        }
    }
}
