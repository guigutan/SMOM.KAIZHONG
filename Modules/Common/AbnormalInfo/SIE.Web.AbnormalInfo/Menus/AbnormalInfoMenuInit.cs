using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports;
using SIE.Common.Menus;
using System.Collections.Generic;
using AbnormalSource = SIE.AbnormalInfo.AbnormalMonitors.AbnormalSource;

namespace SIE.Web.AbnormalInfo.Menus
{
    /// <summary>
    /// IQC菜单初始化
    /// </summary>
    public class AbnormalInfoMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            const string strAbnormalInfo = "SNest.异常管理";
            var res = new List<MenuDto>() {
                 new MenuDto()
                {
                    TreeKey = "SNest",
                    IsLeafNode = false,
                    Label = "异常管理",
                },
                new MenuDto()
                {
                    TreeKey = strAbnormalInfo,
                    EntityType = typeof(AbnormalSource),
                    Label = "异常来源",
                },
                new MenuDto()
                {
                    TreeKey = strAbnormalInfo,
                    EntityType = typeof(AbnormalDefine),
                    Label = "异常定义",
                },
                new MenuDto()
                {
                    TreeKey = strAbnormalInfo,
                    EntityType = typeof(AbnormalDecisionRule),
                    Label = "异常判定规则",
                },
                new MenuDto()
                {
                    TreeKey = strAbnormalInfo,
                    EntityType = typeof(TimelinessAbnormityReportsViewModel),
                    Label = "异常时效看板",
                },
                new MenuDto()
                {
                    TreeKey = strAbnormalInfo,
                    EntityType = typeof(AbnormalMonitorInventory),
                    Label = "异常清单",
                },
                new MenuDto()
                {
                    TreeKey = strAbnormalInfo,
                    EntityType = typeof(AbnormalMonitorTask),
                    Label = "异常任务",
                },
                new MenuDto()
                {
                    TreeKey = strAbnormalInfo,
                    EntityType = typeof(AbnormalWarnDefine),
                    Label = "异常预警定义",
                }
            };
            return res;
        }
    }
}
