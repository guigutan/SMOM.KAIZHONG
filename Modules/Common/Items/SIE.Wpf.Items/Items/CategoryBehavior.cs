using DevExpress.Xpf.Grid;
using System;
using System.Linq;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 分类视图行为行为
    /// 1、展开所有节点；2、控制删除命令是否可执行
    /// </summary>
    class CategoryBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加行为展开所有节点
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            if (view.Control.View is TreeListView)
            {
                var treeView = view.Control.View as TreeListView;
                treeView.AutoExpandAllNodes = true;
            }

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

            var delCommand = View.Commands.FirstOrDefault(p => p.GetType() == typeof(CategoryDeleteCommand)) as CategoryDeleteCommand;
            if (delCommand != null)
            {
                delCommand.CanDelete = null;
            }
        }
    }
}
