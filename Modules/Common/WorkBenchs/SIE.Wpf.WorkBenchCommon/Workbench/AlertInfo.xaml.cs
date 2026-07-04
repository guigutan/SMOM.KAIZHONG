using SIE.Common.Messages;
using SIE.Wpf.Common.Diagram;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace SIE.Wpf.WorkBenchCommon.Workbench
{
    /// <summary>
    /// AlertInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AlertInfo : ComponentItem
    {
        DispatcherTimer timer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AlertInfo()
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
            msgListBox.ItemsSource = RT.Service.Resolve<MessagesController>().GetMyMessages(1000);
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AlertLevelColorConvertMarkupExtension : MarkupExtension, IValueConverter
    {
        static AlertLevelColorConvertMarkupExtension Instance = new AlertLevelColorConvertMarkupExtension();

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
            var task = value as Message;
            if (task.AlertLevel == SIE.Common.Alert.AlertLevel.Grave)
                return Application.Current.FindResource(new Themes.ColorBrushesKeyExtension { ResourceKey = Themes.ColorBrushesKeys.RedBrush });
            else
                return Application.Current.FindResource(new Themes.ColorBrushesKeyExtension { ResourceKey = Themes.ColorBrushesKeys.DarkYellowBrush });
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
            return Instance;
        }
    }
}
