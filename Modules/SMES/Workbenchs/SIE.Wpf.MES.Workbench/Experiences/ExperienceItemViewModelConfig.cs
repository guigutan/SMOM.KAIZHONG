using SIE.MES.Workbench.Experiences;

namespace SIE.Wpf.MES.Workbench.Experiences
{
    /// <summary>
    /// 弹出物料显示视图
    /// </summary>
    public class ExperienceItemViewModelConfig : WPFViewConfig<ExperienceItemViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
        }

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.ClearCommands();
            View.Property(p => p.Item.Code).HasLabel("编码").Show(ShowInWhere.All);
            View.Property(p => p.Item.Name).HasLabel("名称").Show();
            View.Property(p => p.Item.Type).HasLabel("类型").Show();
            View.Property(p => p.Item.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.Item.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.Item.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.Item.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        ///  查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            // 配置视图
        }
    }
}
