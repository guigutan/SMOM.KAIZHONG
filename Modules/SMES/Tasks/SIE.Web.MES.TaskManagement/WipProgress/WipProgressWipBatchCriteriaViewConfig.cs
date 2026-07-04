using SIE.MES.TaskManagement.WipProgress;

namespace SIE.Web.MES.TaskManagement.WipProgress
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WipProgressWipBatchCriteriaViewConfig : WebViewConfig<WipProgressWipBatchCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Readonly();
                View.Property(p => p.PreProcessCode).Readonly();
                View.Property(p => p.ProcessCode).Readonly();
                View.Property(p => p.ProcessName).Readonly();
                View.Property(p => p.BatchNo);
            }
        }
    }
}
