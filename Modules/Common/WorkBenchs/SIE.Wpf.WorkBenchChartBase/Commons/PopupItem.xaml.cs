using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// ControlGroup.xaml 的交互逻辑
    /// </summary>
    public partial class PopupItem : Grid
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PopupItem()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return Convert.ToString(lbTitle.Content);
            }

            set
            {
                lbTitle.Content = value;
            }
        }

        /// <summary>
        /// 内容信息
        /// </summary>
        public FrameworkElement InfoContent { get; set; }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            gridContent.Children.Clear();
            if (InfoContent != null)
                gridContent.Children.Add(InfoContent);
        }
    }
}
