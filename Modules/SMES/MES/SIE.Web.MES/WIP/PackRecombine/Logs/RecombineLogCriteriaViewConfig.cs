using SIE.MES.WIP.PackRecombine.Logs;

namespace SIE.Web.MES.WIP.PackRecombine.Logs
{
    /// <summary>
    /// 包装操作日志查询体视图配置
    /// </summary>
    internal class RecombineLogCriteriaViewConfig : WebViewConfig<RecombineLogCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageNo).Show(ShowInWhere.Detail);
                View.Property(p => p.ParentNo).Show(ShowInWhere.Detail);
                View.Property(p => p.ScanMode).UseEnumEditor(p => p.FilterCategoery = "PACKLOG").Show(ShowInWhere.Detail);
                View.Property(p => p.IsBatch).UseEnumEditor().Show(ShowInWhere.Detail);
            }
        }
    }
}
