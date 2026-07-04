using SIE.Tech.Processs;
using SIE.Wpf.Command;
using SIE.Wpf.Tech.Processs.Commands;

namespace SIE.Wpf.Tech.Processs
{
    /// <summary>
    /// 缺陷信息视图配置
    /// </summary>
    class ProcessDefectViewConfig : WPFViewConfig<ProcessDefect>
    {
        /// <summary>
        /// 工艺路线新增工序ViewGroup
        /// </summary>
        public static readonly string SelProcessDefectViewGroup = "SelProcessDefectView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == SelProcessDefectViewGroup)
            {
                SelProcessDefectView();
            }
        }

        /// <summary>
        /// 工艺路线新增工序视图
        /// </summary>
        protected void SelProcessDefectView()
        {
            View.FormEdit();
            View.UseCommands(typeof(SelProcessDefectCommand), typeof(DelProcessDefectCommand));
            View.Property(x => x.Defect.Code).Show(ShowInWhere.All);
            View.Property(x => x.Defect.Description).Show(ShowInWhere.All);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(ProcessDefectCommand), typeof(ListDeleteCommand));
            View.Property(x => x.Defect).HasLabel("编码");
            View.Property(x => x.Defect.Description).HasLabel("描述");
            View.Property(x => x.DefectLevel).HasLabel("缺陷等级");
            View.Property(x => x.QualityType).HasLabel("质量类型").UseEnumEditor();
            View.Property(x => x.CategoryCode).HasLabel("分类编码");
            View.Property(x => x.CategoryDescription).HasLabel("分类描述");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.UseCommands(typeof(ProcessDefectCommand), typeof(ListDeleteCommand));
            View.Property(x => x.Defect);
            View.Property(x => x.DefectDescription).HasLabel("缺陷描述");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(x => x.Defect);
            View.Property(x => x.DefectDescription);
        }
    }
}