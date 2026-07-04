using SIE.Core.Items;
using SIE.Wpf.Items.Editors;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 打印模板设置视图配置
    /// </summary>
    public class LabelPrintTemplateViewConfig : WPFViewConfig<LabelPrintTemplate>
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
            View.ClearCommands();
            ////View.UseCommands(typeof(TemplateSaveCommand));
            View.UseDetail(columnCount: 2);
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberRule).UseEditor(NumberRuleLookupEditor.EditorName).Show(ShowInWhere.All);
                View.Property(p => p.LabelTemplate).UseEditor(BarcodeModelLookupEditor.EditorName).Show(ShowInWhere.All);
                View.Property(p => p.PackingTemplate).UseEditor(PackageModelLookupEditor.EditorName).Show(ShowInWhere.All);
            }
        }
    }
}
