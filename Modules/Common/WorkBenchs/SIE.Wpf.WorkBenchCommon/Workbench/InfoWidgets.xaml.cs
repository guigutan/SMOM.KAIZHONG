using SIE.Common.Announcements;
using SIE.Common.Messages;
using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.Concerns;
using SIE.Wpf.Common.Diagram;
using System;
using System.Windows;

namespace SIE.Wpf.WorkBenchCommon.Workbench
{
    /// <summary>
    /// InfoWidgets.xaml 的交互逻辑
    /// </summary>
    public partial class InfoWidgets : ComponentItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InfoWidgets()
        {
            InitializeComponent();
            UseProperty<ComponentProperty>();
        }

        /// <summary>
        /// 运行后
        /// </summary>
        protected override void OnRun()
        {
            LoadData();
        }

        void LoadData()
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                msgListBox.ItemsSource = RT.Service.Resolve<MessagesController>().GetMyMessages();
                announcementListBox.ItemsSource = RT.Service.Resolve<AnnouncementController>().GetList();
                concernsListBox.ItemsSource = RT.Service.Resolve<ConcernsController>().GetMyConcerns();
            }));            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void CancelConcerns_Click(object sender, RoutedEventArgs e)
        {
            var selected = concernsListBox.SelectedItem as ConcernsInfo;
            if (selected != null && CRT.MessageService.AskQuestion("你确定要取消关注吗?".L10N()))
            {
                selected.PersistenceStatus = SIE.Domain.PersistenceStatus.Deleted;
                RF.Save(selected);
            }
        }
    }
}
