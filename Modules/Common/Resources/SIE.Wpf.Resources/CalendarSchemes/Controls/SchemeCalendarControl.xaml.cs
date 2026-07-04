using DevExpress.Xpf.Grid;
using SIE.Diagnostics;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.ShiftTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Resources.CalendarSchemes.Controls
{
    /// <summary>
    /// CalendarControl.xaml 的交互逻辑
    /// </summary>
    public partial class SchemeCalendarControl : UserControl
    {
        /// <summary>
        /// EventHandler
        /// </summary>
        public event EventHandler CalendarEvent;

        /// <summary>
        /// 日历方案主视图
        /// </summary>
        LogicalView _mainView;

        /// <summary>
        /// 例外表格控件
        /// </summary>
        GridControl _exceptionControl;

        /// <summary>
        /// 工作日信息
        /// </summary>
        Dictionary<DateTime, ShiftType> dicInfo = new Dictionary<DateTime, ShiftType>();

        /// <summary>
        /// 是否切换月份
        /// </summary>
        bool _monthChanged;

        /// <summary>
        /// 构造方法
        /// </summary>
        public SchemeCalendarControl()
        {
            InitializeComponent();

            this.Loaded += SchemeCalendarControl_Loaded;
        }

        /// <summary>
        /// 日历加载事件，重新刷新日历
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void SchemeCalendarControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshCalendar();
        }

        /// <summary>
        /// 刷新日历方案控件
        /// </summary>
        public void RefreshCalendar()
        {
            if (_mainView == null || _mainView.Current == null || _mainView.Current.PersistenceStatus == PersistenceStatus.New)
            {
                dicInfo.Clear();
                return;
            }

            using (DebugTrace.Start("SchemeCalendarControl.RefreshCalendar".L10N()))
            {
                InitShiftTypeInfo();
            }
        }

        /// <summary>
        /// 初始化当期日历时间的工作日信息
        /// </summary>
        private void InitShiftTypeInfo()
        {
            var calendarScheme = _mainView?.Current as CalendarScheme;
            if (calendarScheme == null)
            {
                return;
            }

            var firstDate = calendarControl.DisplayDate;
            var monthFirstDate = new DateTime(firstDate.Year, firstDate.Month, 1);
            DateTime calendarFirst;
            DateTime calendarLast;
            if (monthFirstDate.DayOfWeek == DayOfWeek.Monday)
            {
                calendarFirst = monthFirstDate.AddDays(-7);
            }
            else
            {
                int days = monthFirstDate.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)monthFirstDate.DayOfWeek - 1;
                calendarFirst = monthFirstDate.AddDays(-days);
            }

            calendarLast = calendarFirst.AddDays(41);
            dicInfo.Clear();
            dicInfo = RT.Service.Resolve<CalendarSchemeController>()
                .GetShiftTypesByCalendarSchemeAndDataRange(calendarScheme, calendarFirst, calendarLast);
        }

        /// <summary>
        /// 日历控件配置
        /// </summary>
        /// <param name="arrangeBounds">结构(参数)</param>
        /// <returns>结构(返回值)</returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            calendarControl.Height = arrangeBounds.Height - 10;
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <summary>
        /// 传递主视图以及例外控件
        /// </summary>
        /// <param name="mainView">主视图吧</param>
        /// <param name="exceptionControl">例外表格控件</param>
        internal void TransferMainView(LogicalView mainView, GridControl exceptionControl)
        {
            _mainView = mainView;
            _exceptionControl = exceptionControl;
        }

        /// <summary>
        /// 表格鼠标进入事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CalendarEvent?.Invoke(sender, EventArgs.Empty);
            var grid = sender as Grid;

            if (_mainView == null)
            {
                return;
            }

            DateTime date = (DateTime)grid.DataContext;

            ShiftType shiftType = null;

            dicInfo.TryGetValue(date, out shiftType);
            grid.ToolTip = null;
            if (shiftType == null)
            {
                return;
            }

            grid.ToolTip += "班制：{0}\r\n".L10nFormat(shiftType.Name);

            foreach (var item in shiftType.ShiftList)
            {
                var begin = item.BeginTime;
                var end = item.EndTime;
                grid.ToolTip += "班次：{0} 时间段：{1}:{2}--{3}:{4}\r\n"
                    .L10N()
                    .FormatArgs(item.Name, begin.Hour, begin.Minute, end.Hour, end.Minute);
            }
        }

        /// <summary>
        /// 班制向日历中放下事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void Grid_Drop(object sender, DragEventArgs e)
        {
            var calendarScheme = _mainView.Current as CalendarScheme;
            if (calendarScheme == null)
            {
                return;
            }

            var grid = sender as Grid;
            var shiftType = e.Data.GetData(typeof(ShiftType)) as ShiftType;

            DateTime date = (DateTime)grid.DataContext;

            if (date.Date <= DateTime.Now.Date)
            {
                throw new ValidationException("例外生效日期必须大于今天".L10N());
            }

            //取当天原来的班制
            ShiftType originalShiftType = null;
            dicInfo.TryGetValue(date, out originalShiftType);

            if (originalShiftType == null)
            {
                throw new ValidationException("原有班制为空".L10N());
            }

            if (originalShiftType.Id == shiftType.Id)
            {
                throw new ValidationException("班制与原有班制相同，不能添加例外。".L10N());
            }

            var exceptList = calendarScheme.Excepts;

            var oldExcept = exceptList.FirstOrDefault(p => p.CalendarDay.Date == date.Date);

            exceptList.Remove(oldExcept);

            var newCalendarSchemeExcept = new CalendarSchemeExcept()
            {
                CalendarDay = date,
                ShiftTypeId = shiftType.Id,
                SchemeId = calendarScheme.Id,
                CreateDate = DateTime.Now
            };

            exceptList.Add(newCalendarSchemeExcept);
            RF.Save(exceptList);

            ////更新引用该日历方案的已启用资源的时间片断
            //RT.Service.Resolve<TimeSliceController>().UpdateTimeSliceByCalendarSchemeException(
            //    newCalendarSchemeExcept.SchemeId, newCalendarSchemeExcept.CalendarDay, newCalendarSchemeExcept.ShiftTypeId);

            //更新字班制字典中的班制
            dicInfo[date] = shiftType;

            RT.Service.Resolve<CalendarSchemeController>().SendCalendarSchemeExceptionModifyMessage(
                       newCalendarSchemeExcept);

            var except = calendarScheme.Excepts.OfType<CalendarSchemeExcept>().OrderBy(w => w.CalendarDay);
            _exceptionControl.ItemsSource = except;
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