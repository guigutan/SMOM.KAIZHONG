using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 任务工序BOM视图配置
    /// </summary>
    internal class TaskProcessBomViewConfig : WebViewConfig<TaskProcessBom>
    {
        /// <summary>
        /// 报工管理工序BOM视图
        /// </summary>
        public const string reportDispatchView = "ReportDispatchView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(reportDispatchView);
            if (ViewGroup == reportDispatchView)
                ReportDispatchView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Qty);
            View.Property(p => p.UseQty);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }

        /// <summary>
        /// 报工管理BOM视图
        /// </summary>
        void ReportDispatchView()
        {
            View.AssignAuthorize(typeof(ReportDispatchTask));
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessName).ShowInList().Readonly().HasLabel("工序");
                View.Property(p => p.ItemCode).ShowInList().Readonly();
                View.Property(p => p.ItemName).ShowInList().Readonly();
                View.Property(p => p.Qty).ShowInList().Readonly();
                View.Property(p => p.UnitName).ShowInList().Readonly();
                View.Property(p => p.UseQty).ShowInList().Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}