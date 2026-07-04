using SIE.Core.Barcodes;
using SIE.Wpf.MES.BatchWIP.Commands;

namespace SIE.Wpf.MES.BatchWIP.ViewBehaviors
{
    /// <summary>
    /// 转出批次视图行为
    /// </summary>
    public class OutputViewBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加行为
        /// </summary>
        protected override void OnAttach()
        {
            var listView = View as ListLogicalView;
            listView.DataChanged += (o, e) =>
            {
                var command = listView.Commands.Find(typeof(Command.ListEditCommand));
                if (command != null)
                    command.IsVisible = false;
            };
            RT.EventBus.Subscribe<ProcessChangedEvent>(this, HideCommand);
            View.Closed += View_Closed;
        }

        /// <summary>
        /// 视图关闭后事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void View_Closed(object sender, System.EventArgs e)
        {
            RT.EventBus.Unsubscribe<ProcessChangedEvent>(this);
        }

        /// <summary>
        /// 启用载具管理的时生成批次和打印命令不可见
        /// </summary>
        /// <param name="obj">工序变更事件</param>
        private void HideCommand(ProcessChangedEvent obj)
        {
            var generateCommand = View.Commands.Find(typeof(GenerateChildBatchCommand));
            if (generateCommand != null)
                generateCommand.IsVisible = obj.type == BarcodeType.BatchBarocde;
            var printCommand = View.Commands.Find(typeof(PrintChildBatchCommand));
            if (printCommand != null)
                printCommand.IsVisible = obj.type == BarcodeType.BatchBarocde;
        }
    }
}