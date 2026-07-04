using SIE.MetaModel.View;

namespace SIE.Wpf.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 模板
    /// </summary>
    public class WorkOrderReachTemplate : ListUITemplate
    {

        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(WorkOrderReachReportLayout));
            return blocks;
        }

        /// <summary>
        /// UI生成
        /// </summary>
        /// <param name="ui">ui</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            base.OnUIGenerated(ui);
            ui.MainView?.QueryView?.TryExecuteQuery();
        }
    }
}
