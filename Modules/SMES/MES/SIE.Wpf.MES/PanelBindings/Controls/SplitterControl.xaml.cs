using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.PanelBindings.Controls
{
    /// <summary>
    /// SplitterControl.xaml 的交互逻辑
    /// </summary>
    public partial class SplitterControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="leftControl">左边控件</param>
        /// <param name="rightControl">右边控件</param>
        public SplitterControl(FrameworkElement leftControl, FrameworkElement rightControl)
        {
            InitializeComponent();
            leftContent.Content = leftControl;
            rightContent.Content = rightControl;
        }
    }
}