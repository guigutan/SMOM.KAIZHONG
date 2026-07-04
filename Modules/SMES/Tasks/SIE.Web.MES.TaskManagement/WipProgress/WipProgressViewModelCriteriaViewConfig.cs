using SIE.MES.TaskManagement.WipProgress;

namespace SIE.Web.MES.TaskManagement.WipProgress
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WipProgressViewModelCriteriaViewConfig : WebViewConfig<WipProgressViewModelCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo);
                View.Property(p => p.BatchNo);
            }
        }
    }
}
