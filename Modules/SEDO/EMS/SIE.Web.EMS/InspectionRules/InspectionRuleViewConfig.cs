using SIE.EMS.InspectionRules;

namespace SIE.Web.EMS.InspectionRules
{
    /// <summary>
    /// 检验规程视图配置
    /// </summary>
    internal class InspectionRuleViewConfig : WebViewConfig<InspectionRule>
    {

        /// <summary>
        /// 显示宽度
        /// </summary>
        private const int CoulmnWidth = 20;
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(InspectionRule.NameProperty);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.InspectionRules.Behaviors.InspectionRuleBehavior");
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(CoulmnWidth * 10);
                View.Property(p => p.Name).ShowInList(CoulmnWidth * 10);
                View.Property(p => p.PeriodDays).UseSpinEditor(p =>
                {
                    p.AllowNegative = false;
                    p.AllowDecimals = false;
                    p.MinValue = 0;
                }).ShowInList(CoulmnWidth * 4);
                View.Property(p => p.WarningPeriod).UseSpinEditor(p =>
                {
                    p.AllowNegative = false;
                    p.AllowDecimals = false;
                    p.MinValue = 0;
                }).ShowInList(CoulmnWidth * 6);
                View.Property(p => p.InspectionRuleType).ShowInList(CoulmnWidth * 4);
                View.Property(p => p.CheckCategory).ShowInList(CoulmnWidth * 4);
            }
        }

        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(CoulmnWidth * 10);
                View.Property(p => p.Name).ShowInList(CoulmnWidth * 10);
                View.Property(p => p.PeriodDays).ShowInList(CoulmnWidth * 4);
                View.Property(p => p.WarningPeriod).ShowInList(CoulmnWidth * 6);
                //View.Property(p => p.InspectionRuleType).ShowInList(CoulmnWidth * 4); 
                View.Property(p => p.CheckCategory).ShowInList(CoulmnWidth * 4); 
            }
        }
    }
}