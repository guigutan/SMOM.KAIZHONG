using DevExpress.Xpf.Core;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Reworks;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WIP.Reworks
{
    /// <summary>
    /// ReworkKeyItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReworkKeyItemControl : UserControl
    {
        /// <summary>
        /// 返工采集视图
        /// </summary>
        private ReworkViewModel _model;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="model">返工采集视图</param>
        public ReworkKeyItemControl(ReworkViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            ctlKeyItem.ItemsSource = model.WipKeyItems;
            _model = model;
        }

        /// <summary>
        /// 关健件单击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void BtnKeyItem_Click(object sender, RoutedEventArgs e)
        {
            if (_model.ReworkOperate == ReworkOperate.PermuteUnbound)
            {
                var curBtn = sender as SimpleButton;
                var curKeyItem = curBtn.Tag as WipProductProcessKeyItem;
                if (e.OriginalSource.GetType().FullName== "System.Windows.Controls.CheckBox")
                {
                    if (curKeyItem.IsUnbound)
                        curKeyItem.IsUnbound = true;
                    else
                        curKeyItem.IsUnbound = false;
                }
                else
                {
                    if (curKeyItem.IsUnbound)
                        curKeyItem.IsUnbound = false;
                    else
                        curKeyItem.IsUnbound = true;
                }

                SetIsSelectedAll();
            }
        }

        /// <summary>
        /// 设置返工采集视图的属性值IsSelectedAll
        /// </summary>
        private void SetIsSelectedAll()
        {
            if (_model.WipKeyItems.All(x => x.IsUnbound))
                _model.IsSelectedAll = true;
            else
                _model.IsSelectedAll = false;
        }
    }
}
