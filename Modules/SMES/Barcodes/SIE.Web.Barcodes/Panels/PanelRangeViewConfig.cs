using SIE.Barcodes.Panels;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.ObjectModel;

namespace SIE.Web.Barcodes.Panels
{
    /// <summary>
    /// 拼板码范围视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class PanelRangeViewConfig : WebViewConfig<PanelRange>
    {
        #region 条码范围
        /// <summary>
        /// 条码范围
        /// </summary>
        [Label("条码范围")]
        public static readonly Property<string> CodeRangeProperty = P<PanelRange>.RegisterExtensionReadOnly("CodeRange", typeof(PanelRangeViewConfig), GetCodeRange, PanelRange.StartNoProperty, PanelRange.EndNoProperty);

        /// <summary>
        /// 格式化条码范围
        /// </summary>
        /// <param name="me">条码范围</param>
        /// <returns>格式化条码范围String</returns>
        public static string GetCodeRange(PanelRange me)
        {
            return me.StartNo + "-" + me.EndNo;
        }
        #endregion 
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, "SIE.Web.Barcodes.Panels.ReceivePanelCommand");
            View.Property(p => p.WONo).ShowInList(150).HasLabel("工单号");
            View.Property(p => p.ProductName).HasLabel("产品名称");
            View.Property(CodeRangeProperty).ShowInList(250).HasLabel("条码范围");
            View.Property(p => p.PrintQty);
            View.Property(p => p.ScrapedQty);
            View.Property(p => p.State);
            View.Property(p => p.ReceiveBy);
            View.Property(p => p.ReceiveDate).ShowInList(150);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置视图
        }
    }
}