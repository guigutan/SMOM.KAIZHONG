using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Items;
using SIE.Wpf.Items;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 打印模板设置 视图配置
    /// </summary>
    internal class ProductLabelTemplateViewConfig : WPFViewConfig<LabelPrintTemplate>
    {
        /// <summary>
        /// ReadonlyView
        /// </summary>
        public const string ReadOnlyView = "ReadonlyView";

        /// <summary>
        /// WorkderView
        /// </summary>
        public const string WorkderView = "WorkderView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadOnlyView, WorkderView);
            View.UseDetail(columnCount: 2);
            if (ViewGroup == ReadOnlyView)
                ConfigReadOnly();
            else if (ViewGroup == WorkderView)
                ConfigWorkderView();
        }

        /// <summary>
        /// 工单对应视图配置
        /// </summary>
        void ConfigWorkderView()
        {
            View.FormEdit();
            //View.UseCommands(typeof(WorkOrders.Commands.TemplateSaveCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberRule).UseNumberRuleLookupEditor(p => p.DisplayMember = NumberRule.CodeProperty.Name).Show(ShowInWhere.Detail);
                View.Property(p => p.LabelTemplate).UseBarcodeModelLookupEditor(p => { p.DisplayMember = PrintTemplate.FileNameProperty.Name; p.ReloadDataOnPopping = true; }).Show(ShowInWhere.Detail);
                View.Property(p => p.PackingTemplate).UsePackageModelLookupEditor(p => { p.DisplayMember = PrintTemplate.FileNameProperty.Name; p.ReloadDataOnPopping = true; }).Show(ShowInWhere.Detail);
            }
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        void ConfigReadOnly()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberRule).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.LabelTemplate).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PackingTemplate).Show(ShowInWhere.All).Readonly();
            }
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.NumberRule).UsePagingLookUpEditor(p => p.DisplayMember = NumberRule.CodeProperty.Name)
                //    .UseDataSource((o, e, r) =>
                //    {
                //        return RT.Service.Resolve<NumberRuleController>().GetNumberRule(RuleType.Barcode);
                //    }).Show(ShowInWhere.All);
                //View.Property(p => p.LabelTemplate).UsePagingLookUpEditor(p => p.DisplayMember = PrintTemplate.FileNameProperty.Name)
                //    .UseDataSource((o, e, r) =>
                //{
                //    var template = o as LabelPrintTemplate;
                //    var templates = new EntityList<PrintTemplate>();
                //    if (template == null || template.NumberRule == null)
                //        return templates;
                //    template.NumberRule.TemplateList.ForEach(p => templates.Add(p.Template));
                //    return templates;
                //}).Show(ShowInWhere.All);
                //View.Property(p => p.PackingTemplate).UsePagingLookUpEditor(p => p.DisplayMember = PrintTemplate.FileNameProperty.Name)
                //    .UseDataSource((o, e, r) =>
                //{
                //    var template = o as LabelPrintTemplate;
                //    var templates = new EntityList<PrintTemplate>();
                //    if (template == null || template.NumberRule == null)
                //        return templates;
                //    template.NumberRule.TemplateList.ForEach(p => templates.Add(p.Template));
                //    return templates;
                //}).Show(ShowInWhere.All);

                View.Property(p => p.NumberRule).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.LabelTemplate).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PackingTemplate).Show(ShowInWhere.All).Readonly();
            }
        }
    }
}
