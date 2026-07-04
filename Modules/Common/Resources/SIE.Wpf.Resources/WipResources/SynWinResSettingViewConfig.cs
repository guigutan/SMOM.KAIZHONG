using SIE.Resources.WipResources;

namespace SIE.Wpf.Resources.WipResources
{
    /// <summary>
    /// 资源同步视图配置
    /// </summary>
    public class SynWinResSettingViewConfig : WPFViewConfig<SynWipResSetting>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            
            base.ConfigView();
        }

        /// <summary>
        /// 表格试图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(WPFCommandNames.ListEdit);

            using (View.OrderProperties())
            {
                View.Property(p => p.Name).Readonly();
                View.Property(p => p.Type).Readonly();
                View.Property(p => p.AssenblyName).Readonly();
                View.Property(p => p.IsSyn);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Name);
                View.Property(p => p.Type);
                View.Property(p => p.AssenblyName).Readonly();
                View.Property(p => p.IsSyn);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
