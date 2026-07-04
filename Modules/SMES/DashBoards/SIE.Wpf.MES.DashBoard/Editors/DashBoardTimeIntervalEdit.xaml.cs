using SIE.ObjectModel;
using SIE.Utils;
using SIE.Wpf.Common.Diagram;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.DashBoard.Editors
{
    /// <summary>
    /// TimeIntervalEditor.xaml 的交互逻辑
    /// </summary>
    public partial class DashBoardTimeIntervalEdit : UserControl
    {
        /// <summary>
        /// 时间间隔
        /// </summary>
        public TimeInterval TimeInterval
        {
            get { return (TimeInterval)GetValue(TimeIntervalProperty); }
            set { SetValue(TimeIntervalProperty, value); }
        }

        /// <summary>
        /// 时间间隔依赖属性
        /// </summary>      
        public static readonly DependencyProperty TimeIntervalProperty =
            DependencyProperty.Register("TimeInterval", typeof(TimeInterval), typeof(DashBoardTimeIntervalEdit), new PropertyMetadata(new TimeInterval { TimePart = TimePart.Hours, TimeValue = 1d }));

        /// <summary>
        /// 构造函数
        /// </summary>
        public DashBoardTimeIntervalEdit()
        {
            InitializeComponent();
            this.Loaded += (o, e) =>
            {
                this.timePart.ItemsSource = EnumViewModel.GetByEnumType(typeof(TimePart));

                var path = PropertyEditorBinder.GetPropertyName(this);
                this.SetBinding(TimeIntervalProperty, new Binding(path) { Mode = BindingMode.TwoWay });
                if (this.TimeInterval == null)
                    this.TimeInterval = new TimeInterval();
                this.contaolGrid.DataContext = this.TimeInterval;
            };
        }
    }

    /// <summary>
    /// 时间单位
    /// </summary>
    public enum TimePart
    {
        /// <summary>
        /// 小时
        /// </summary>
        [Label("小时")]
        Hours,

        /// <summary>
        /// 分
        /// </summary>
        [Label("分")]
        Minutes,

        /// <summary>
        /// 秒
        /// </summary>
        [Label("秒")]
        Seconds,

        /// <summary>
        /// 天
        /// </summary>
        [Label("天")]
        Days
    }

    /// <summary>
    /// 时间间隔类
    /// </summary>
    [Serializable]
    public class TimeInterval : ObservableObject
    {
        /// <summary>
        /// 时间类型
        /// </summary>
        public TimePart TimePart
        {
            get { return GetProperty<TimePart>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 时间值
        /// </summary>
        public double TimeValue
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
    }
}
