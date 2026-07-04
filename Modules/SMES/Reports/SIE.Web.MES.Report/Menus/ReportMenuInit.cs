using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.MES.Report.WipProducts;
using SIE.MES.Report.BatchWipProducts;
using SIE.MES.Report.EmployeeReports.ClockingIns;
using SIE.MES.Report.EmployeeReports.Vacancies;

namespace SIE.Web.MES.Report
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class ReportMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产报表",
                Label = "生产通用报表",
                EntityType = typeof(WipProductVersionReport)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产报表",
                Label = "批次生产通用报表",
                EntityType = typeof(BatchWipProductVersionReport)
            });
            
            res.Add(new MenuDto()
            {
                TreeKey = "MES.员工考勤绩效",
                Label = "员工出勤",
                EntityType = typeof(EmployeeClockInReport)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.员工考勤绩效",
                Label = "班组缺编统计",
                EntityType = typeof(WorkGroupVacancyReport)
            });

            return res;
        }

    }
}
