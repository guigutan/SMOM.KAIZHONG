using SIE.Core.Items;
using System;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 打印模板设置 视图配置
    /// </summary>
    internal class ProductLabelTemplateViewConfig : WebViewConfig<LabelPrintTemplate>
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
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberRule).HasLabel("条码规则".L10N()+"*").UseNumberRuleLookupEditor(p => p.XType = "TemplateBarcodeRule").Show(ShowInWhere.Detail)
                    .UseListSetting(e => { e.HelpInfo = "显示规则类型为产品条码的编码规则".L10N(); });
                View.Property(p => p.LabelTemplate).HasLabel("标签模板".L10N() + "*").UseBarcodeModelLookupEditor(p => p.XType = "TemplateLabel").Show(ShowInWhere.Detail)
                    .UseListSetting(e => e.HelpInfo = "说明:单件追溯，标签模板只能是产品条码实体类型。批次追溯，标签模板只能是批次条码实体类型".L10N());
                View.Property(p => p.PackingTemplate).UseBarcodeMoveModelLookupEditor(p => p.XType = "TemplateLabel").HasLabel("外标签打印模板".L10N() + "*").Show(ShowInWhere.Detail)
                    .UseListSetting(e => e.HelpInfo = "说明:外标签打印模板的实体类型为外标签条码".L10N());
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
                View.Property(p => p.NumberRule).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.LabelTemplate).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PackingTemplate).Show(ShowInWhere.All).Readonly();
            }
        }
    }
}
