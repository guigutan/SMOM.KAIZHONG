using DevExpress.Xpf.Grid;
using System;
using System.Linq;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 分类层级视图行为行为
    /// 1、展开所有节点；2、控制删除命令是否可执行
    /// </summary>
    class CategoryLevelBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加行为展开所有节点
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            var treeView = view.Control.View as TreeListView;
            treeView.AutoExpandAllNodes = true;
            view.CurrentChanged += View_CurrentChanged;
        }

        /// <summary>
        /// 当前选中变更事件
        /// </summary>
        /// <param name="sender">逻辑视图</param>
        /// <param name="e">参数</param>
        private void View_CurrentChanged(object sender, EventArgs e)
        {
            if (View.Current == null)
            {
                return;
            }

            var delCommand = View.Commands.FirstOrDefault(p => p.GetType() == typeof(CategoryLevelDeleteCommand)) as CategoryLevelDeleteCommand;
            if (delCommand != null)
            {
                delCommand.CanDelete = null;
            }
        }
    }
}
