using SIE.Domain;
using SIE.Resources.ProcessTechTypes;
using SIE.Resources.WipResources;

namespace SIE.Web.Resources.WipResources
{
    /// <summary>
    /// 生产资源查询视图配置类
    /// </summary>
    public class WipResourceCriteriaViewConfig : WebViewConfig<WipResourceCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.SourceType).Show(ShowInWhere.All);
                View.Property(p => p.State).Show(ShowInWhere.All);
                View.Property(p => p.ProcessTechTypeId).Show(ShowInWhere.All)
                    .UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var result = RT.Service.Resolve<ProcessTechTypeController>().GetProcessTechTypeList(pagingInfo, keyword);
                        if (result == null) return new EntityList<ProcessTechType>();
                        return result;
                    });
                View.Property(p => p.CalendarScheme).UseSchemeLookUpEditor().Show(ShowInWhere.All);
                View.Property(p => p.WorkShop).UseShopEditor().Show(ShowInWhere.All)
                    .UseListSetting(e => { e.HelpInfo = "显示类型为车间的企业模型"; });
                View.Property(p => p.Factory).UsePlantEditor().Show(ShowInWhere.All);
            }
        }
    }
}
