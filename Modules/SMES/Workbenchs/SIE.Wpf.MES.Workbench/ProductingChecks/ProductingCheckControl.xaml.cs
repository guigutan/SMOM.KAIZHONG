using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Workbench.ProductingChecks;
using SIE.Resources.Employees;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace SIE.Wpf.MES.Workbench.ProductingChecks
{
    /// <summary>
    /// 开班点检 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class ProductingCheckControl : ComponentItem, IDisposable
    {
        public static RoutedUICommand Prev { get; } = new RoutedUICommand();
        public static RoutedUICommand Next { get; } = new RoutedUICommand();

      readonly  ObservableCollection<ProductingCheck> CheckList;
        ProductingCheckProperty _property;
        double? workGroupId;
        double? WorkGroupId { get { return workGroupId ?? GetWorkGroupId(); } }
        ProductingCheckContoller _controller { get; } = RT.Service.Resolve<ProductingCheckContoller>();
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductingCheckControl()
        {
            InitializeComponent();
            _property = UseProperty<ProductingCheckProperty>();
            CheckList = new ObservableCollection<ProductingCheck>();
            ctlStation.ItemsSource = CheckList;
        }

        Timer timer;
        /// <summary>
        /// 
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            var interval = _property.RefeshTime <= 0 ? 180000 : _property.RefeshTime * 60000;
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = interval;
            timer.Enabled = true;
            timer.Start();
            RefeshCheck();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            timer?.Stop();
            timer?.Close();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnClose();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
            {
                RefeshCheck();
            });
        }

        void InitStationCheck()
        {
            CheckList.Clear();
            var date = RF.Find<Employee>().GetDbTime();
            var checkList = _controller.GetProductingChecks(WorkGroupId.Value, date.Date);
            if (checkList != null)
                checkList.ForEach(e => { CheckList.Add(e); });
        }

        double? GetWorkGroupId()
        {
            var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByUserId(RT.IdentityId);
            if (employee == null)
                throw new ValidationException("员工未关联当前用户".L10N());
            if (!employee.WorkGroupId.HasValue)
                throw new ValidationException("员工[{0}]未分配班组".L10nFormat(employee.Name));
            return employee.WorkGroupId;
        }
        DXWindow window;
        private void ChangeOnduty_Click(object sender, RoutedEventArgs e)
        {
            var check = (sender as SimpleButton).DataContext as ProductingCheck;
            var control = new ChangeOnDutyControl(this, check);
            window = new DXWindow() { Topmost = true, WindowStyle = WindowStyle.None, Content = control, Padding = new Thickness(0) };
            window.Width = 255;
            window.Height = 160;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            window.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseOnDutyWindow()
        {
            window?.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefeshCheck()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                InitStationCheck();
            }));
        }

        private void Prev_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Prev_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //
        }
    }

    /// <summary>
    /// 开班点检属性
    /// </summary>
    public class ProductingCheckProperty : ComponentProperty<ProductingCheckControl>
    {
        /// <summary>
        /// 数据刷新时间
        /// </summary>
        [DisplayName("刷新间隔（分钟）")]
        [Description("时间必须大于0,默认刷新时间为3分钟")]
        [Category("自定义")]
        public double RefeshTime { get; set; }
    }
}