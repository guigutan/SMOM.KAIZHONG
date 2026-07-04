using System;
using System.Windows;
using System.Windows.Controls;

namespace Resources.Controls
{
    /// <summary>
    /// Copyright.xaml 的交互逻辑
    /// </summary>
    public partial class Copyright : UserControl
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public Copyright()
        {
            InitializeComponent();
            Loaded += Copyright_Loaded;
        }

        private void Copyright_Loaded(object sender, RoutedEventArgs e)
        {
            BtmYear.Text = string.Format("© {0} SIE All Rights Reserved.", DateTime.Now.Year);
        }
    }
}
