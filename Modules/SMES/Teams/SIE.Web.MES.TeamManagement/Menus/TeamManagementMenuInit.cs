using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.OnLoans;
using SIE.MES.TeamManagement.RatedItems;
using SIE.MES.TeamManagement.ScoreRecords;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.MES.TeamManagement.SikllAuthentications;

namespace SIE.Web.MES.TeamManagement
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class TeamManagementMenuInit : IWebMenuInit
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
                Label = "员工考勤绩效",
                IsLeafNode = false,
            });
            const string mesTeamManagement = "MES.员工考勤绩效";
            res.Add(new MenuDto()
            {
                TreeKey = mesTeamManagement,
                Label = "考勤机管理",
                EntityType = typeof(ClockInMachine)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTeamManagement,
                Label = "排班表",
                EntityType = typeof(ShiftSchedule)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTeamManagement,
                Label = "评分项目",
                EntityType = typeof(RatedItem)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTeamManagement,
                Label = "评分记录",
                EntityType = typeof(ScoreRecord)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTeamManagement,
                Label = "借调明细表",
                EntityType = typeof(WorkGroupOnLoan)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTeamManagement,
                Label = "人员工时统计",
                EntityType = typeof(EmployeeClockInAttent)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "技能管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.技能管理",
                Label = "员工技能认证管理",
                EntityType = typeof(SkillAuthentication)
            });

            return res;
        }

    }
}
