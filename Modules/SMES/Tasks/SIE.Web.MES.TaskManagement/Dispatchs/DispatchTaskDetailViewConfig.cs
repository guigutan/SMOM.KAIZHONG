using SIE.MES.TaskManagement.Dispatchs;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工任务明细视图配置
    /// </summary>
    internal class DispatchTaskDetailViewConfig : WebViewConfig<DispatchTaskDetail>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.AdoId);
            View.Property(p => p.AdoName);
            View.Property(p => p.ProcessMatchDegree);
            View.Property(p => p.AdoType);
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
    }
}