using SIE.Resources.WipResources;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SIE.Wpf.MES.DashBoard.DashBoards.Editors
{
    /// <summary>
    /// 产线下拉框
    /// </summary>
    public partial class DashBoardLineLookupEdit : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DashBoardLineLookupEdit()
        {
            InitializeComponent();
            var lineList = RT.Service.Resolve<WipResourceController>().GetWipResources(new List<ResourceState>() { ResourceState.Actived }, new List<SyncSourceType>() { SyncSourceType.Enterprise }, null, string.Empty);
            this.lineLookupEdit.ItemsSource = lineList;
        }
    }
}
