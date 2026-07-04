using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using SIE.Tech.Processs;
using SIE.Wpf.Tech.Processs;
using SIE.Wpf.Windows;
using System.Windows.Media;

namespace SIE.Wpf.Tech.Routings
{
    /// <summary>
    /// 工序树 工艺路线版本树图片选择器
    /// </summary>
    public class TreeInfoImageSelector : TreeListNodeImageSelector
    {
        /// <summary>
        /// 选择图片，树节点展开收缩时触发
        /// </summary>
        /// <param name="rowData">当前行数据</param>
        /// <returns>图片</returns>
        public override ImageSource Select(TreeListRowData rowData)
        {
            TreeInfo treeInfo = (rowData.Row as TreeInfo);
            if (treeInfo.Type == nameof(Process))
            {
                return IconManager.GetImageSource("pack://application:,,,/SIE.Wpf.Tech;component/Routings/Images/file.png");
            }
            else
            {
                return IconManager.GetImageSource("pack://application:,,,/SIE.Wpf.Tech;component/Routings/Images/folder.png");
            }
        }
    }
}
