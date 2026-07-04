using SIE.ESop.Displays;
using SIE.Wpf.ESop.Displays.Command;
using SIE.Wpf.Resources;

namespace SIE.Wpf.ESop.Displays
{
    /// <summary>
    /// 显示点视图配置
    /// </summary>
    internal class DisplayPointViewConfig : WPFViewConfig<DisplayPoint>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.UseCommands(typeof(InitESopCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Resource).UseEnterpriseResourceEditor();
                View.Property(p => p.PlayScreenNum).UseSpinEditor(p => { p.MinValue = 1; p.Decimals = 0; p.Increment = 1; }).UseListSetting(p => p.HeaderToolTip = "1或不填"); ;
                View.Property(p => p.Remark);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Resource).UseEnterpriseResourceEditor();
                View.Property(p => p.PlayScreenNum);
                View.Property(p => p.Remark);
            }
        }
    }
}