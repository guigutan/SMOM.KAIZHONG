using SIE.ESop.Displays;
using SIE.Web.ESop.Displays.Commands;

namespace SIE.Web.ESop.Displays
{
    /// <summary>
    /// 显示点视图配置
    /// </summary>
    internal class DisplayPointViewConfig : WebViewConfig<DisplayPoint>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseImportCommands();
            View.UseCommand(typeof(InitESopCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Resource).UseEnterpriseResourceEditor();
                View.Property(p => p.PlayScreenNum).UseSpinEditor(p => { p.MinValue = 1; p.AllowDecimals = false; }).UseListSetting(p=>p.HeaderToolTip="1或不填");
                View.Property(p => p.Remark);
            }
        }

        /// <summary>
        /// 导入视图配置
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).ImportIndexer();
            View.Property(p => p.Name);
            View.PropertyRef(p => p.Resource.Code).HasLabel("资源");
            View.Property(p => p.PlayScreenNum);
            View.Property(p => p.Remark);
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
