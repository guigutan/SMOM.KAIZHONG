using SIE.MetaModel.View;
using SIE.Tech.Processs;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序缺陷视图配置
    /// </summary>
    public class ProcessDefectViewConfig : WebViewConfig<ProcessDefect>
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
            View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessDefectSelectCommand", WebCommandNames.Delete);
            View.Property(p => p.Defect).HasLabel("编码").Show();
            View.Property(p => p.DefectDescription).HasLabel("描述").Show();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessDefectSelectCommand", WebCommandNames.Delete);
            View.Property(p => p.Defect).HasLabel("编码");
            View.Property(p => p.DefectDescription).HasLabel("描述");
            View.Property(p => p.DefectLevel);
            View.Property(p => p.QualityType);
            View.Property(p => p.CategoryCode).HasLabel("分类编码");
            View.Property(p => p.CategoryDescription);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Defect).HasLabel("编码");
            View.Property(p => p.DefectDescription).HasLabel("描述");
            View.Property(p => p.DefectLevel);
            View.Property(p => p.QualityType);
            View.Property(p => p.CategoryCode).HasLabel("分类编码");
            View.Property(p => p.CategoryDescription);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Defect).HasLabel("编码");
            View.Property(p => p.DefectDescription).HasLabel("描述");
            View.Property(p => p.DefectLevel);
            View.Property(p => p.QualityType);
            View.Property(p => p.CategoryCode).HasLabel("分类编码");
            View.Property(p => p.CategoryDescription);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Defect).HasLabel("编码");
            View.Property(p => p.DefectDescription).HasLabel("描述");
            View.Property(p => p.DefectLevel);
            View.Property(p => p.QualityType);
            View.Property(p => p.CategoryCode).HasLabel("分类编码");
            View.Property(p => p.CategoryDescription);
        }
    }
}