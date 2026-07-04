using SIE.Defects;

namespace SIE.Web.Defects
{
	/// <summary>
	/// 缺陷等级视图配置
	/// </summary>
	internal class DefectGradeViewConfig : WebViewConfig<DefectGrade>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Name);
            View.Property(p => p.DefectSeverity);
        }

        /// <summary>
        /// 查询面板视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.DefectSeverity);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.DefectSeverity);
        }
    }
}
