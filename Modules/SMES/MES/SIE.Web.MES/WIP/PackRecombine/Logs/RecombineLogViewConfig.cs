using SIE.MES.WIP.PackRecombine.Logs;

namespace SIE.Web.MES.WIP.PackRecombine.Logs
{
    /// <summary>
    /// 包装操作日志视图配置
    /// </summary>
    internal class RecombineLogViewConfig : WebViewConfig<RecombineLog>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.PackageNo).ShowInList(width: 150).Readonly();
            View.Property(p => p.PackageUnitName).ShowInList(width: 100).Readonly();
            View.Property(p => p.ParentNo).ShowInList(width: 150).Readonly();
            View.Property(p => p.ParentUnitName).ShowInList(width: 100).Readonly();
            View.Property(p => p.ScanMode).UseEnumEditor().Readonly();
            View.Property(p => p.IsBatch).Readonly();
        }
    }
}
