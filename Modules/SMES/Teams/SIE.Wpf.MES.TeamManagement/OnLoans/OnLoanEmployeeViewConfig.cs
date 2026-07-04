using SIE.MES.TeamManagement.OnLoans;

namespace SIE.Wpf.MES.TeamManagement
{
    /// <summary>
    /// 借调单选择人员视图配置
    /// </summary>
    internal class OnLoanEmployeeViewConfig : WPFViewConfig<OnLoanEmployee>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Employee).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}