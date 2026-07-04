using SIE.Andon.AndonMonthReports;
using SIE.Andon.Andons;
using SIE.Andon.AndonStatisticsReports;
using SIE.Common.Menus;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.Andon.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class AndonMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "安灯管理",
                IsLeafNode = false
            });

            const string mesAndonManage = "MES.安灯管理";
            res.Add(new MenuDto()
            {
                TreeKey = mesAndonManage,
                EntityType = typeof(AndonType),
                Label = "安灯类型维护"
            });
            
            res.Add(new MenuDto()
            {
                TreeKey = mesAndonManage,
                EntityType = typeof(SIE.Andon.Andons.Andon),
                Label = "安灯维护"
            });

            res.Add(new MenuDto()
            {
                TreeKey = mesAndonManage,
                EntityType = typeof(AndonManage),
                Label = "安灯管理"
            });

            res.Add(new MenuDto()
            {
                TreeKey = mesAndonManage,
                EntityType = typeof(AndonExperience),
                Label = "安灯经验库"
            });

            res.Add(new MenuDto()
            {
                TreeKey = mesAndonManage,
                Label = "安灯统计报表".L10N(),
                EntityType = typeof(AndonStatisticsViewModel)
            });

            res.Add(new MenuDto()
            {
                TreeKey = mesAndonManage,
                Label = "安灯统计报表(月度)".L10N(),
                EntityType = typeof(AndonMonthReportViewModel)
            });

            return res;
        }
    }
}
