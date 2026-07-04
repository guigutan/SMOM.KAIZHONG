using SIE.Defects;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 缺陷代码视图配置
    /// </summary>
    internal class DefectViewConfig : WPFViewConfig<Defect>
    {
        private const string TYPE_CODE = "分类编码";

        public const string ReadOnlyListView = "ReadOnlyListView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
            View.DeclareExtendViewGroup(ReadOnlyListView);
            if (ViewGroup == ReadOnlyListView)
            {
                ConfigReadOnlyListView();
            }
        }

        /// <summary>
        /// 配置只读列表视图
        /// </summary>
        private void ConfigReadOnlyListView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(150);
                View.Property(p => p.Description).ShowInList();
                View.Property(p => p.DefectGrade).ShowInList();
                View.Property(p => p.QualityType).ShowInList();
                View.Property(p => p.DefectCategoryId).UseEditor(WPFEditorNames.PagingLookUp).HasLabel(TYPE_CODE).ShowInList();
                View.Property(p => p.CategoryDescription).HasLabel("分类描述").ShowInList();
            }
        }
    }
}