using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WIP.Reworks
{
    /// <summary>
    /// ReworkControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReworkControl : UserControl
    {
        /// <summary>
        /// 返工采集视图
        /// </summary>
        private ReworkViewModel _model;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collectDetails">采集信息</param>
        /// <param name="keyItems">关键件</param>
        /// <param name="model">返工采集视图</param>
        public ReworkControl(FrameworkElement collectDetails, FrameworkElement keyItems, ReworkViewModel model)
        {
            InitializeComponent();
            _model = model;

            gdReworkCtl.DataContext = _model;
            cclCollectDetails.Content = collectDetails;
            cclKeyItems.Content = keyItems;
        }

        /// <summary>
        /// CheckBox的Click事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void ChkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            bool curIsUnbound = _model.IsSelectedAll;
            foreach (var curItem in _model.WipKeyItems)
            {
                curItem.IsUnbound = curIsUnbound;
            }
        }

        /////// <summary>
        /////// 根据参数解绑关键件
        /////// </summary>
        /////// <param name="curIsUnbound">是否解绑</param>
        ////private void UpdateKeyItems(bool curIsUnbound)
        ////{
        ////    foreach (var curItem in _model.WipKeyItems)
        ////    {
        ////        curItem.IsUnbound = curIsUnbound;
        ////    }
        ////}
    }
}
