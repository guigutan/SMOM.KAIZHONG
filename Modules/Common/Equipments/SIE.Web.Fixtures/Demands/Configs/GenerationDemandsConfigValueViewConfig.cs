using SIE.Fixtures.FixtureDemands.Configs;

namespace SIE.Web.Fixtures.Demands.Configs
{

    /// <summary>
    /// 配置项
    /// </summary>
    public class GenerationDemandsConfigValueViewConfig : WebViewConfig<GenerationDemandsConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Leadtime).Show().UseSpinEditor(m => { m.MinValue = 0; m.DecimalPrecision = 2; }).HasLabel("前置时间(小时)");
            View.Property(p => p.AutomaticGeneration).ShowInDetail().UseFormSetting(p => p.HelpInfo = "勾选后请前往【调度任务设置】菜单配置调度")
                .HasLabel("工单自动生成工治具需求单");
        }
    }
}
