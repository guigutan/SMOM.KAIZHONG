using SIE.Common.Menus;
using SIE.Wpf.MES.TaskManagement.Reports;
using System.Collections.Generic;

namespace SIE.Wpf.MES.TaskManagement.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class TaskManagementMenuInit : IWpfMenuInit
    {
        /// <summary>
        /// 菜单初始化
        /// </summary>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            const string mesCollection = "MES(CS).生产采集";

            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "任务单报工",
                EntityType = typeof(TaskReportViewModel),
            });
            return res;
        }
    }
}
