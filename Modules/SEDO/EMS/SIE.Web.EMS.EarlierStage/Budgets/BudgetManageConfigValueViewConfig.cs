using SIE.EMS.EarlierStage.Budgets.Configs;

namespace SIE.Web.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算管理配置-界面
    /// </summary>
    internal class BudgetManageConfigValueViewConfig : WebViewConfig<BudgetManageConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.FiscalYearEndDate).UseDateEditor(p => { p.Format = "m-d H:i:s"; p.XType = "datetimefield"; });
        }
    }
}
