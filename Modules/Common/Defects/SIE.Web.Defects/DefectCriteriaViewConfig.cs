using SIE.Defects.Defects;

namespace SIE.Web.Defects
{
    /// <summary>
    /// 缺陷代码查询条件视图配置
    /// </summary>
    public class DefectCriteriaViewConfig : WebViewConfig<DefectCriteria>
    {
        private const string DEFECT_TYPE = "缺陷等级非快码类型";

        /// <summary>
        /// 查询面板视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.DefectCategory);
            View.Property(p => p.QualityType);
            View.Property(p => p.DefectGrade) .UseListSetting(e => { e.HelpInfo = DEFECT_TYPE; });
        }
    }
}
