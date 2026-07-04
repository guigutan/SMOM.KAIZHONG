using SIE.Barcodes.Panels.Configs;
using SIE.Common.Prints;
using SIE.Domain;
using System.Linq;

namespace SIE.Web.Elec.MES.Panels.Configs
{
    /// <summary>
    /// 拼板码打印规则配置
    /// </summary>
    internal class PanelPrintConfigValueViewConfig : WebViewConfig<PanelPrintConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BacodeRule).Cascade(p => p.LabelTemplate, null).Show(ShowInWhere.All);
                View.Property(p => p.LabelTemplate).UseDataSource((e, c, r) =>
                {
                    var configValue = e as PanelPrintConfigValue;
                    var templates = new EntityList<PrintTemplate>();
                    if (configValue == null || configValue.BacodeRule == null)
                        return templates;
                    configValue.BacodeRule.TemplateList.Where(p => p.Template.State == State.Enable).ForEach(p => templates.Add(p.Template));
                    return templates;
                }).UsePagingLookUpEditor().Show(ShowInWhere.All);
            }
        }
    }
}
