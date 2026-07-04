using SIE.Common.Menus;
using SIE.MetaModel;
using SIE.WorkBenchCommon.Workbench.Base;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.WorkBenchCommon
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class WorkBenchCommonMenuInit : IWebMenuInit
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
                TreeKey = "SNest.通用配置",
                Label = "布局管理".L10N(),
                EntityType = typeof(LayoutInfo),
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.通用配置",
                Label = "组件管理".L10N(),
                EntityType = typeof(ComponentInfo),
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.通用配置",
                Label = "工作台定义".L10N(),
                EntityType = typeof(WorkbenchDefinition),
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.通用配置",
                Label = "KPI目标设定".L10N(),
                EntityType = typeof(QuotaTargetSetting)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.通用配置",
                Label = "目标达成率预警设定".L10N(),
                EntityType = typeof(TargetWarnSetting)
            });

            return res;
        }

    }
}
