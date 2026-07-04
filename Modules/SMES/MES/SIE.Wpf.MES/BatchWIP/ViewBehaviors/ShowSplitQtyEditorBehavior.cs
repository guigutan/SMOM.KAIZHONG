namespace SIE.Wpf.MES.BatchWIP.ViewBehaviors
{
    /// <summary>
    /// 显示拆分数量编辑器
    /// </summary>
    public class ShowSplitQtyEditorBehavior : ViewBehavior
    {
        /// <summary>
        /// 行为执行方法
        /// </summary>
        protected override void OnAttach()
        {
            var listView = View as ListLogicalView;
            var tableView = listView.Control.View as DevExpress.Xpf.Grid.TableView;
            tableView.EditorButtonShowMode = DevExpress.Xpf.Grid.EditorButtonShowMode.ShowOnlyInEditor;
            tableView.EditorShowMode = DevExpress.Xpf.Core.EditorShowMode.MouseDown;

            listView.DataChanged += (o, e) =>
            {
                var command = listView.Commands.Find(typeof(Command.ListEditCommand));
                if (command == null) return;
                command.IsVisible = false;
                command.TryExecute();
                tableView.ShowEditor(false);
                return;
            };
        }
    }
}
