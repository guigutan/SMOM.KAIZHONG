using SIE.WorkBenchCommon.Workbench.Chatting;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.Services;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace SIE.Wpf.WorkBenchCommon.Workbench
{
    /// <summary>
    /// MyTask.xaml 的交互逻辑
    /// </summary>
    public partial class Chat : ComponentItem
    {
        DispatcherTimer _timer;

        /// <summary>
        /// 是否有新消息
        /// </summary>
        public bool HasNewChat
        {
            get { return (bool)GetValue(HasNewChatProperty); }
            set { SetValue(HasNewChatProperty, value); }
        }

        /// <summary>
        /// 新消息
        /// </summary>
        public static readonly DependencyProperty HasNewChatProperty =
            DependencyProperty.Register("HasNewChat", typeof(bool), typeof(Chat), new PropertyMetadata(false));

        static Chat()
        {
            EventManager.RegisterClassHandler(typeof(Chat), Mouse.LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCaptureEvent));
            EventManager.RegisterClassHandler(typeof(Chat), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
        }

        static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            Chat chat = (Chat)sender;
            if (Mouse.Captured == chat && e.OriginalSource == chat)
            {
                chat.btnChat.IsChecked = false;
                chat.ReleaseMouseCapture();
            }
            if (Mouse.Captured == chat && e.Source is Button)
            {
                chat.SendButton_Click(e.Source, e);
            }
        }

        static void OnLostMouseCaptureEvent(object sender, MouseEventArgs e)
        {
            Chat chat = (Chat)sender;
            if (Mouse.Captured == null && chat.btnChat.IsChecked == true)
                Mouse.Capture(chat, CaptureMode.SubTree);
            if (Mouse.Captured is FrameworkElement && new Rect(new Point(), chat.RenderSize).Contains(e.GetPosition(chat)))
            {
                Mouse.Capture(chat, CaptureMode.SubTree);
                System.Diagnostics.Debug.WriteLine("OnLostMouseCaptureEvent");
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Chat()
        {
            InitializeComponent();
            DataContext = this;
            UseProperty<ComponentProperty>();
            popup.Opened += Popup_Opened;
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
            ReciveChat();
            txtMsg.Focus();
        }

        void ReciveChat()
        {
            chatListBox.ItemsSource = RT.Service.Resolve<ChatController>().ReciveChat();
            HasNewChat = false;
            UpdateTimer();
        }

        /// <summary>
        /// 运行后
        /// </summary>
        protected override void OnRun()
        {
            NetworkChangeMonitor();
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 30);
            _timer.Tick += _timer_Tick;
            CheckNewChat();
        }

        /// <summary>
        /// 网络监听
        /// </summary>
        private void NetworkChangeMonitor()
        {
            var log = SIE.Logging.LogManager.Logger;
            var networkChangeMonitor = RT.Service.Resolve<INetworkChangeMonitor>();
            networkChangeMonitor.NetworkAvailableEvent += (sender, e) =>
            {
                log.Debug($"module NetworkAvailableEvent");
                //定时器start
                _timer.Start();
            };
            networkChangeMonitor.NetworkNotAvailableEvent += (sender, e) =>
            {
                log.Debug($"module NetworkAvailableEvent");
                //定时器stop
                _timer.Stop();
            };
        }

        void CheckNewChat()
        {
            if (!HasNewChat)
                HasNewChat = RT.Service.Resolve<ChatController>().HasNewChat();
            UpdateTimer();
        }

        void UpdateTimer()
        {
            if (!HasNewChat)
                _timer.Start();
            else
                _timer.Stop();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
            {
                CheckNewChat();
            });
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtMsg.Text.IsNotEmpty())
            {
                RT.Service.Resolve<ChatController>().SendChat(txtMsg.Text);
                txtMsg.Clear();
                ReciveChat();
            }
        }
    }

    /// <summary>
    /// 文本转换类
    /// </summary>
    public class HasTextConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().IsNotEmpty();
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// 当在派生类中实现时，返回一个对象，该对象作为此标记扩展的目标属性的值。
        /// </summary>
        /// <param name="serviceProvider">标记扩展服务的服务提供者</param>
        /// <returns>对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// 图形水平垂直的转换类
    /// </summary>
    public class ChatHorizontalAlignmentConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (object.Equals(value, RT.IdentityId))
                return HorizontalAlignment.Right;
            return HorizontalAlignment.Left;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// 当在派生类中实现时，返回一个对象，该对象作为此标记扩展的目标属性的值。
        /// </summary>
        /// <param name="serviceProvider">标记扩展服务的服务提供者</param>
        /// <returns>对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
