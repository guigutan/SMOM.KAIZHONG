using DevExpress.Xpf.Grid;
using SIE.Packages.ItemLabels;

namespace SIE.Wpf.Packages.ItemLabels.ViewBehaviors
{
    /// <summary>
    /// 
    /// </summary>
    public class PackingLabelBehavior : ViewBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            var grid = view.Control as GridControl;
            if (view.Control.View is TableView)
            {
                var treeView = new TreeListView();
                treeView.KeyFieldName = PackingLabel.NoProperty.Name;
                treeView.ParentFieldName = PackingLabel.PackageNoProperty.Name;
                treeView.AutoExpandAllNodes = true;
                grid.View = treeView;
            }
        }
    }
}
