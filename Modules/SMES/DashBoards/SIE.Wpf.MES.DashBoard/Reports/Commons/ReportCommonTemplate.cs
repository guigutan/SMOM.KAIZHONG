using SIE.MetaModel.View;

namespace SIE.Wpf.MES.DashBoard.Reports.Commons
{
    /// <summary>
    /// 报表通用模板
    /// </summary>
    /// <typeparam name="T">报表直通率ViewModel类型</typeparam>
    public class ReportCommonTemplate : ListUITemplate
    {

        /// <summary>
        /// 块定义
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(FPYReportLayout));
            return blocks;
        }

        protected override void OnUIGenerated(ControlResult ui)
        {
            base.OnUIGenerated(ui);

            ui.MainView?.QueryView?.TryExecuteQuery();
        }
    }
}
