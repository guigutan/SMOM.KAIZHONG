using System.Windows.Controls;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// TwoListControl.xaml 的交互逻辑
    /// </summary>
    public partial class TwoListControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="first">第一个List</param>
        /// <param name="firstHeader">第一个List的Header</param>
        /// <param name="second">第二个List</param>
        /// <param name="secondHeader">第二个List的Header</param>
        public TwoListControl(ControlResult first, string firstHeader, ControlResult second, string secondHeader)
        {
            InitializeComponent();
            inputControl.Content = first.Control;
            inputControl.Header = firstHeader;
            outputControl.Content = second.Control;
            outputControl.Header = secondHeader;
        }
    }
}