using System.Windows.Controls;
using SIE.Resources.WipResources;

namespace SIE.Wpf.Resources.WipResources.Controls
{
    /// <summary>
    /// ChildLayout.xaml 的交互逻辑
    /// </summary>
    public partial class ChildLayout : UserControl
    {
        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipResource { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public ChildLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新资源日历数据
        /// </summary>
        public void RefreshSchemeWeek()
        {
            resCalendarControl.ItemsSource = WipResource?.Scheme?.SchemeWeeks;
            calendar.WipeResource = WipResource;
            calendar.RefreshCalendar();
        }
    }
}
