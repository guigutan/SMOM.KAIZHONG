using DevExpress.Xpf.Grid;
using SIE.Common;
using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.Wpf.Resources.Enterprises.Commands;
using System;
using System.Linq;

namespace SIE.Wpf.Resources.Enterprises
{
    /// <summary>
    /// 企业层级视图行为
    /// </summary>
    public class EnterpriseLevelBehavior : ViewBehavior
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseLevelBehavior()
        {
            AutoExpandAllNodes = true;
        }

        /// <summary>
        /// 是否自动展开所有树节点
        /// </summary>
        public bool AutoExpandAllNodes
        {
            get;
            set;
        }

        /// <summary>
        /// 附加视图行为
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            if (view.Control.View is TreeListView)
            {
                var treeView = view.Control.View as TreeListView;
                treeView.AutoExpandAllNodes = AutoExpandAllNodes;
            }

            view.Data = view.Data.OfType<Entity>().Where(f => f.GetInvOrgId() == RT.InvOrg).AsEntityList();
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

            var delLevelCommand = View.Commands.FirstOrDefault(p => p.GetType() == typeof(DeleteLevelCommand)) as DeleteLevelCommand;
            if (delLevelCommand != null)
            {
                delLevelCommand.CanDelete = null;
            }
        }
    }
}
