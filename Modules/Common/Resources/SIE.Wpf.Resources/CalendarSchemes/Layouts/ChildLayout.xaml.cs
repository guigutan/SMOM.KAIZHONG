using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.ShiftTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.Resources.CalendarSchemes.Layouts
{
    /// <summary>
    /// ChildLayout.xaml 的交互逻辑
    /// </summary>
    public partial class ChildLayout : UserControl
    {
        /// <summary>
        /// 班制字典
        /// </summary>
        Dictionary<string, List<Shift>> _dic;

        /// <summary>
        /// The start point
        /// </summary>
        private Point startPoint;

        /// <summary>
        /// The start drag
        /// </summary>
        private bool startDrag;

        /// <summary>
        /// The current row handle
        /// </summary>
        private int currentRowHandle;

        /// <summary>
        /// 构造方法
        /// </summary>
        public ChildLayout()
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="mainView">日历方案主视图</param>
        public ChildLayout(LogicalView mainView)
        {
            InitializeComponent();
            mainView.CurrentChanged += (s, e) => MainView_CurrentChanged(s, e, mainView);
        }

        /// <summary>
        /// 主表事件变更后更新例外
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        /// <param name="mainView">主逻辑视图</param>
        private void MainView_CurrentChanged(object sender, EventArgs e, LogicalView mainView)
        {
            calendar.TransferMainView(mainView, exceptionControl);
            var calendarScheme = mainView.Current as CalendarScheme;
            if (calendarScheme == null)
            {
                return;
            }

            var except = calendarScheme.Excepts.OfType<CalendarSchemeExcept>().OrderBy(w => w.CalendarDay);
            exceptionControl.ItemsSource = except;

            InitShiftType();

            calendar.RefreshCalendar();
        }

        /// <summary>
        /// 初始化班制
        /// </summary>
        private void InitShiftType()
        {
            var calendarCtl = RT.Service.Resolve<ShiftTypeController>();
            EntityList<ShiftType> shiftTypes = calendarCtl.GetShiftTypes();

            EntityList<Shift> shifts = calendarCtl.GetShifts();
            var dicShifts = shifts.GroupBy(x => x.ShiftTypeId).ToDictionary(x => x.Key);

            InitShift(shiftTypes, dicShifts);

            shiftTypeControl.ItemsSource = shiftTypes;
        }

        /// <summary>
        /// 获取所有班制的所有班次并按照班制名称存字典
        /// </summary>
        /// <param name="shiftTypes">班制</param>
        /// <param name="dicShifts">班次</param>
        /// <returns>
        /// 字典
        /// </returns>
        private void InitShift(
            EntityList<ShiftType> shiftTypes, Dictionary<double, IGrouping<double, Shift>> dicShifts)
        {
            _dic = new Dictionary<string, List<Shift>>();
            foreach (var type in shiftTypes)
            {
                var shifts = new List<Shift>();

                IGrouping<double, Shift> shiftGroup = null;

                dicShifts.TryGetValue(type.Id, out shiftGroup);

                if (shiftGroup != null)
                {
                    shifts = shiftGroup.OrderBy(x => x.BeginTime).ToList();
                }

                _dic.Add(type.Name, shifts);
            }
        }

        /// <summary>
        /// 鼠标进入班制触发事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void ShiftTypeControl_MouseEnter(object sender, MouseEventArgs e)
        {
            var control = sender as GridControl;

            if (control.CurrentItem == null)
                return;

            control.ToolTip = null;

            var shiftType = new ShiftType();

            if (control.CurrentItem.GetType() == typeof(ShiftType))
            {
                shiftType = control.CurrentItem as ShiftType;
            }

            if (shiftType == null)
                return;

            foreach (var item in _dic[shiftType.Name])
            {
                var begin = item.BeginTime;
                var end = item.EndTime;
                control.ToolTip += "{0}:{1}--{2}:{3}\r\n"
                    .FormatArgs(begin.Hour, begin.Minute, end.Hour, end.Minute);
            }
        }

        /// <summary>
        /// Handles the PreviewMouseDown event of the tableView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void TableView1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            this.startPoint = e.GetPosition(null);
            this.startDrag = IsGridRowAvailable(e);
            this.currentRowHandle = shiftTypeControl.View.GetRowHandleByMouseEventArgs(e);
        }

        /// <summary>
        /// Handles the PreviewMouseMove event of the tableView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        void TableView1_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(null);

            if (this.startDrag && e.LeftButton == MouseButtonState.Pressed && IsGridRowAvailable(e) && (Math.Abs(position.X - this.startPoint.X) > 1 || Math.Abs(position.Y - this.startPoint.Y) > 1))
            {
                this.startDrag = false;

                var currentRow = shiftTypeControl.GetRow(currentRowHandle);
                if (currentRow != null && currentRow.GetType() == typeof(ShiftType))
                {
                    var shiftType = currentRow as ShiftType;
                    DataObject dataObject = new DataObject(shiftType);
                    DragDrop.DoDragDrop(shiftTypeControl, dataObject, DragDropEffects.Copy);
                }
            }

            this.startPoint = e.GetPosition(null);
        }

        /// <summary>
        /// Handles the SelectedItemChanged event of the ExceptionControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs"/> instance containing the event data.</param>
        private void ExceptionControl_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            var control = sender as GridControl;
            control.ToolTip = null;
            ShiftType shiftType;
            if (control.CurrentItem.GetType() == typeof(ShiftType))
                shiftType = control.CurrentItem as ShiftType;
            else
                shiftType = (control.CurrentItem as CalendarSchemeExcept).ShiftType;

            if (shiftType == null)
                return;
            foreach (var item in _dic[shiftType.Name])
            {
                if (item.IsOverDay)
                {
                    control.ToolTip += "{0}--{1}\r\n\n".FormatArgs(item.BeginTime, item.EndTime.AddDays(1));
                }
                else
                {
                    control.ToolTip += "{0}--{1}\r\n\n".FormatArgs(item.BeginTime, item.EndTime);
                }
            }
        }

        /// <summary>
        /// Determines whether [is grid row available] [the specified e].
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <returns>
        ///   <c>true</c> if [is grid row available] [the specified e]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsGridRowAvailable(MouseEventArgs e)
        {
            int rowHandle = shiftTypeControl.View.GetRowHandleByMouseEventArgs(e);
            TableViewHitInfo hitInfo = tableView1.CalcHitInfo(e.OriginalSource as DependencyObject);

            return shiftTypeControl.GetRow(rowHandle) != null && (hitInfo.HitTest == TableViewHitTest.RowIndicator
                || hitInfo.HitTest == TableViewHitTest.RowCell);
        }
    }
}
