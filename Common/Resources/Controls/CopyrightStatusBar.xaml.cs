using System;
using System.Windows;
using System.Windows.Controls;

namespace Resources.Controls
{
    /// <summary>
    /// CopyrightStatusBar.xaml 的交互逻辑
    /// </summary>
    public partial class CopyrightStatusBar : UserControl
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public CopyrightStatusBar()
        {
            InitializeComponent();
            Loaded += CopyrightStatusBar_Loaded;
        }

        private void CopyrightStatusBar_Loaded(object sender, RoutedEventArgs e)
        {
            txt.Text = string.Format("Copyright© {0}  广州赛意信息科技股份有限公司", DateTime.Now.Year);
        }
    }
}
