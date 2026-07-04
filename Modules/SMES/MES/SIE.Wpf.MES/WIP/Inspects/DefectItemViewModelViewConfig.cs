using SIE.Wpf.MES.WIP.Inspects.Commands;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 缺陷录入 视图配置
    /// </summary>
    public class DefectItemViewModelViewConfig : WPFViewConfig<DefectItemViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CollectionUITemplate.CollectionUIViewGroup);
            if (ViewGroup == CollectionUITemplate.CollectionUIViewGroup)
            {
                ConfigCollectionView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigCollectionView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(DefectItemDeleteCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Defect.DefectCategory.Code).HasLabel("缺陷分类代码").Show(ShowInWhere.All);
                View.Property(p => p.Defect.DefectCategory.Description).HasLabel("缺陷分类描述").Show(ShowInWhere.All);
                View.Property(p => p.Defect.Code).HasLabel("缺陷代码").Show(ShowInWhere.All);
                View.Property(p => p.Defect.Description).HasLabel("缺陷描述").Show(ShowInWhere.All);
                View.Property(p => p.ModelInspectionItem.Name).HasLabel("检验项描述").Show(ShowInWhere.All);
            }
        }
    }
}
