using SIE.MES.TeamManagement.ClockingIns;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 考勤机管理视图配置
    /// </summary>
    internal class ClockInMachineViewConfig : WebViewConfig<ClockInMachine>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseImportCommands();
            View.Property(p => p.Name).ShowInList(150);
            View.Property(p => p.IpAddress).ShowInList(150);
            View.Property(p => p.Port);
            View.Property(p => p.Model).Show(ShowInWhere.Hide);
            View.Property(p => p.SN).ShowInList(150).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommands("SIE.Web.MES.TeamManagement.ClockingIns.Commands.MachineSaveCommand", "SIE.Web.MES.TeamManagement.ClockingIns.Commands.MachineLinkCommand");
            View.Property(p => p.Name);
            View.Property(p => p.IpAddress);
            View.Property(p => p.Port);
            View.Property(p => p.Model).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.SN).Readonly().Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.IpAddress);
            View.Property(p => p.Port);
            View.Property(p => p.Model).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.SN).Readonly().Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Name).ImportIndexer();
            View.Property(p => p.IpAddress);
            View.Property(p => p.Port);
        }
    }
}
