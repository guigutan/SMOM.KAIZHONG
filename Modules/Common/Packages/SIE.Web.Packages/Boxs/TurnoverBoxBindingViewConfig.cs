using SIE.MetaModel.View;
using SIE.Packages.Boxs;

namespace SIE.WPF.Packages.Boxs
{
    /// <summary>
    /// 周转工具绑定明细视图配置
    /// </summary>
    internal class TurnoverBoxBindingViewConfig : WebViewConfig<TurnoverBoxBinding>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Sn);
            View.Property(p => p.Qty);
            View.Property(p => p.IsUnbinding);
            View.Property(p => p.BindingDate);
            View.Property(p => p.UnBindingDate);
            View.Property(p => p.IsBindFinish);
            View.Property(p => p.Item);
            View.Property(p => p.BarcodeType);
            View.Property(p => p.BindingOperatorId);
            View.Property(p => p.UnbindingOperatorId);
        }
    }
}