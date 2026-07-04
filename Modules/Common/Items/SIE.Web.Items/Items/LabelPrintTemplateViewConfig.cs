using SIE.Core.Items;
using SIE.Web.Items._Extentions_;

namespace SIE.Web.Items
{
    /// <summary>
    /// 打印模板设置视图配置
    /// </summary>
    public class LabelPrintTemplateViewConfig : WebViewConfig<LabelPrintTemplate>
    {
        /// <summary>
        /// 视图名称
        /// </summary>
        public const string LabelPrintTemplateView = "LabelPrintTemplateViewConfig";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(LabelPrintTemplateView);

            if (ViewGroup == LabelPrintTemplateView)
            {
                ItemLabelPrintTemplateView();
            }
        }

        /// <summary>
        /// 物料标签打印模板视图配置
        /// </summary>
        void ItemLabelPrintTemplateView()
        {
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberRule).Cascade(p => p.LabelTemplate, null).Cascade(p => p.PackingTemplate, null).Show();
                View.Property(p => p.LabelTemplate).UseLabelPrintTemplateEditor().UseListSetting(e => { e.HelpInfo = "编码规则下对应的可用标签模板"; }).Show();
                View.Property(p => p.PackingTemplate).UseLabelPrintTemplateEditor().UseListSetting(e => { e.HelpInfo = "编码规则下对应的可用包装模板"; }).Show();
            }
        }
    }
}
