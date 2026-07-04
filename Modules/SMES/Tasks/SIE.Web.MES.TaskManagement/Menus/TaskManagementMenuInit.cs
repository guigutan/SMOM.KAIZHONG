using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Specifications;
using SIE.MES.TaskManagement.StandardWorkHours;
using SIE.MetaModel;
using System;
using SIE.MES.TaskManagement.ProcessTaskLists;

namespace SIE.Web.MES.TaskManagement
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class TaskManagementMenuInit : IWebMenuInit
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
                TreeKey = "MES",
                Label = "任务单管理",
                IsLeafNode = false,
            });
            const string mesTaskManagement = "MES.任务单管理";
            res.Add(new MenuDto()
            {
                TreeKey = mesTaskManagement,
                Label = "产品规格件对照表",
                EntityType = typeof(ProductSpecification)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTaskManagement,
                Label = "规格件清单",
                EntityType = typeof(Specification)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTaskManagement,
                Label = "派工管理",
                EntityType = typeof(DispatchTask)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTaskManagement,
                Label = "报工管理",
                EntityType = typeof(ReportDispatchTask)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTaskManagement,
                Label = "报工记录审核",
                EntityType = typeof(ReportRecordExamine)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTaskManagement,
                Label = "产品标准工时维护",
                EntityType = typeof(StandardHourSet)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTaskManagement,
                Label = "工序任务清单",
                EntityType = typeof(ProcessTaskListViewModel)
            });
            return res;
        }

    }
}
