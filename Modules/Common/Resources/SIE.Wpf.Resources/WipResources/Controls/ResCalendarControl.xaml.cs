using SIE.Diagnostics;
using SIE.Domain;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BrushConverter = System.Windows.Media.BrushConverter;

namespace SIE.Wpf.Resources.WipResources.Controls
{
    /// <summary>
    /// ResCalendarControl.xaml 的交互逻辑
    /// </summary>
    public partial class ResCalendarControl : UserControl
    {
        #region 属性&字段
        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipeResource { get; set; }

        /// <summary>
        /// 当前生产资源是否改变，避免不断刷新数据导致程序卡死
        /// </summary>
        bool _isResChanged;

        /// <summary>
        /// 是否切换月份
        /// </summary>
        bool _monthChanged;

        /// <summary>
        /// 日历视图中日期对应的内容
        /// </summary>
        List<Grid> _dayButtonGrids = new List<Grid>();

        /// <summary>
        /// 内容资源名称，用于获取日历内容
        /// </summary>
        private string contentName = "CalendarDayButtonContent";

        /// <summary>
        /// 暂停图标控件
        /// </summary>
        private string contentHoliday = "contentHoliday";

        /// <summary>
        /// 默认日期颜色
        /// </summary>
        Brush _defaultBackground = (Brush)new BrushConverter().ConvertFromString("#EBECEF".L10N()); ////默认色：灰色

        /// <summary>
        /// 资源启用且日期大于等于当前日历颜色
        /// </summary>
        Brush _activedBackground = (Brush)new BrushConverter().ConvertFromString("#7DD1F3".L10N()); ////已启用状态，且是未来时间，则显示为绿色 

        /// <summary>
        /// 班制信息
        /// </summary>
        Dictionary<double, string> _dicShift = new Dictionary<double, string>();

        /// <summary>
        /// 工作日信息
        /// </summary>
        Dictionary<DateTime, ShiftTypeInfo> dicInfo = new Dictionary<DateTime, ShiftTypeInfo>();
        #endregion  属性&字段

        /// <summary>
        /// 资源日历控件构造函数
        /// </summary>
        public ResCalendarControl()
        {
            InitializeComponent();
            this.Loaded += ResCalendarControl_Loaded;
        }

        /// <summary>
        /// 资源日历控件Loaded方法
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void ResCalendarControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshCalendar();
        }

        /// <summary>
        /// 刷新日历方案控件
        /// </summary>
        public void RefreshCalendar()
        {
            if (WipeResource == null || WipeResource.PersistenceStatus == PersistenceStatus.New)
            {
                ClearCalendar();
                return;
            }

            if (!_isResChanged)
            {
                _isResChanged = true;
                try
                {
                    using (DebugTrace.Start("ResCalendarControl.RefreshCalendar".L10N()))
                    {
                        InitShiftTypeInfo();
                        foreach (var grid in _dayButtonGrids)
                        {
                            BindingDayButtonDayInfo(grid);
                        }
                    }
                }
                finally
                {
                    _isResChanged = false;
                }
            }
        }

        /// <summary>
        /// 清除日历信息
        /// </summary>
        public void ClearCalendar()
        {
            ShiftTypeInfo info = new ShiftTypeInfo();
            foreach (var grid in _dayButtonGrids)
            {
                var textBlock = grid.FindName(contentName) as TextBlock;
                textBlock.Text = info.Content;
                grid.Background = info.IsActived ? _activedBackground : _defaultBackground;
                grid.Tag = info;
                var image = grid.FindName(contentHoliday) as ContentControl;
                image.Content = null;
            }
        }

        /// <summary>
        /// 重置日历高度
        /// </summary>
        /// <param name="arrangeBounds">日历高度对象</param>
        /// <returns>日历高度</returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            calendarControl.Height = arrangeBounds.Height - 10;
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <summary>
        /// 日历方案加载，添加日历事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void DayButton_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                {
                    var grid = sender as Grid;
                    BindingDayButtonDayInfo(grid);
                    grid.MouseRightButtonDown += Grid_MouseRightButtonDown;
                    grid.DataContextChanged += Grid_DataContextChanged;
                    grid.PreviewMouseUp += Grid_PreviewMouseUp;
                    grid.MouseEnter += Grid_MouseEnter;
                    _dayButtonGrids.Add(grid);
                }));
            });
        }

        /// <summary>
        /// 工作日信息初始化
        /// </summary>
        private void InitShiftTypeInfo()
        {
            var firstDate = calendarControl.DisplayDate;
            var monthFirstDate = new DateTime(firstDate.Year, firstDate.Month, 1);
            DateTime calendarFirst;
            DateTime calendarLast;
            if (monthFirstDate.DayOfWeek == DayOfWeek.Monday)
                calendarFirst = monthFirstDate.AddDays(-7);
            else
            {
                int days = monthFirstDate.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)monthFirstDate.DayOfWeek - 1;
                calendarFirst = monthFirstDate.AddDays(-days);
            }

            calendarLast = calendarFirst.AddDays(41);
            if (dicInfo != null)
                dicInfo.Clear();
            dicInfo = RT.Service.Resolve<WipResourceController>().GetShiftTypeInfo(WipeResource, calendarFirst.Date, calendarLast.Date);
        }

        /// <summary>
        /// 根据传进来的Grid的DataContentext信息和班制Id设置DayInfo
        /// </summary>
        /// <param name="grid">DayButton中的Grid</param> 
        private void BindingDayButtonDayInfo(Grid grid)
        {
            if (WipeResource == null || WipeResource.PersistenceStatus == PersistenceStatus.New)
            {
                return;
            }

            DateTime date = (DateTime)grid.DataContext;
            var info = GetShiftTypeInfo(date);
            var textBlock = grid.FindName(contentName) as TextBlock;
            textBlock.Text = info.Content;
            grid.Background = info.IsActived ? _activedBackground : _defaultBackground;
            grid.Tag = info;
            var image = grid.FindName(contentHoliday) as ContentControl;
            if (info.IsHoliday)
            {
                var picture = new Image();
                picture.Source =
                    new BitmapImage(new Uri("/SIE.Wpf.Resources;component/Images/休.png", UriKind.RelativeOrAbsolute));
                image.Content = picture;
            }
            else
            {
                image.Content = null;
            }
        }

        /// <summary>
        /// 获取工作日信息
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>工作日信息</returns>
        ShiftTypeInfo GetShiftTypeInfo(DateTime date)
        {
            if (dicInfo != null && dicInfo.ContainsKey(date))
            {
                return dicInfo[date];
            }

            return new ShiftTypeInfo();
        }

        /// <summary>
        /// 日历日期右键事件，设置选中日期值
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            DateTime date = (DateTime)grid.DataContext;
            calendarControl.SelectedDate = date;
        }

        /// <summary>
        /// DayButton 的 DataContext Changed 事件处理。
        /// 切换月的事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((DateTime)e.NewValue == (DateTime)e.OldValue)
            {
                return;
            }
        }

        /// <summary>
        /// Grid点击事件,禁掉点击日期自动跨月
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            DateTime? date = (DateTime)grid.DataContext;
            if (date == null || date.Value.Month != calendarControl.DisplayDate.Month || date.Value.Year != calendarControl.DisplayDate.Year)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 鼠标进入日期控件事件，显示工作日信息
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = sender as Grid;
            var info = grid.Tag as ShiftTypeInfo;
            if (info == null || info.ShiftType == null)
            {
                return;
            }

            var toolTip = GetToolTipShiftType(info.ShiftType);
            grid.ToolTip = toolTip;
        }

        /// <summary>
        /// 获取悬浮班制显示信息
        /// </summary>
        /// <param name="type">班制</param>
        /// <returns>显示信息</returns>
        string GetToolTipShiftType(ShiftType type)
        {
            if (_dicShift.ContainsKey(type.Id))
            {
                return _dicShift[type.Id];
            }
            else
            {
                var shiftInfo = new StringBuilder();
                shiftInfo.Append(type.Name);
                foreach (Shift shift in type.ShiftList)
                {
                    shiftInfo.Append("\r\n{0} {1} - {2}".FormatArgs(shift.Name, shift.BeginTime.ToString("t"), shift.EndTime.ToString("t")));
                }
                var rst = shiftInfo.ToString();
                _dicShift[type.Id] = rst;
                return rst;
            }
        }

        /// <summary>
        /// 月切换
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void CalendarControl_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            if (!_monthChanged)
            {
                _monthChanged = true;
                try
                {
                    using (DebugTrace.Start("ResCalendarControl.MonthChanged".L10N()))
                    {
                        InitShiftTypeInfo();
                        foreach (var grid in _dayButtonGrids)
                        {
                            BindingDayButtonDayInfo(grid);
                        }
                    }
                }
                finally
                {
                    _monthChanged = false;
                }
            }
        }
    }
}