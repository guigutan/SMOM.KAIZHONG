using SIE.Common.Announcements;
using SIE.Wpf.Common.Diagram;
using System;
using System.Windows;
using System.Windows.Threading;

namespace SIE.Wpf.WorkBenchCommon.Workbench
{
    /// <summary>
    /// Announcement.xaml 的交互逻辑
    /// </summary>
    public partial class Announcement : ComponentItem
    {
        DispatcherTimer timer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Announcement()
        {
            InitializeComponent();
            UseProperty<ComponentProperty>();
        }

        /// <summary>
        /// 运行后
        /// </summary>
        protected override void OnRun()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 5, 0);
            timer.Start();
            timer.Tick += delegate
            {
                SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
                {
                    LoadData();
                });
            };
            LoadData();
        }
        void LoadData()
        {
            announcementListBox.ItemsSource = RT.Service.Resolve<AnnouncementController>().GetList();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
