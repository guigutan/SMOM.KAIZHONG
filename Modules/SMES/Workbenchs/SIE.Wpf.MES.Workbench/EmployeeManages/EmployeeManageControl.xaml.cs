using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Workbench.EmployeeManages;
using SIE.Resources.Employees;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.Helper;
using SIE.Wpf.MES.Workbench.Properties;
using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;

namespace SIE.Wpf.MES.Workbench.EmployeeManages
{
    /// <summary>
    /// EmployeeManageControl.xaml 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class EmployeeManageControl : ComponentItem, IDisposable
    {
        EmployeeManageOutput _output;
        EmployeeManage _manage;
        EmployeeManageProperty _property;
        double? workGroupId;
        double? WorkGroupId { get { return workGroupId ?? GetWorkGroupId(); } }

        EmployeeController _controller { get; } = RT.Service.Resolve<EmployeeController>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeManageControl()
        {
            InitializeComponent();
            _manage = new EmployeeManage();
            ctlManage.DataContext = _manage;
            _property = UseProperty<EmployeeManageProperty>();
            _output = UseOutput<EmployeeManageOutput>();
        }

        #region 按钮命令
        private void Setting_MouseDown(object sender, System.Windows.RoutedEventArgs e)
        {
            btnYesNo.Visibility = System.Windows.Visibility.Visible;
            btnSetting.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Yes_MouseDown(object sender, System.Windows.RoutedEventArgs e)
        {
            ValidationLineShift(_manage.ResourceId, _manage.ShiftId);
            _manage.OldResourceId = _manage.ResourceId;
            _manage.OldShiftId = _manage.ShiftId;
            _output.ResourceId = _manage.ResourceId;
            _output.ShiftId = _manage.ShiftId;
            SetBtnVisibility();
            SaveResourceShift();
        }

        private void ValidationLineShift(double resourceId, double shifitId)
        {
            if (resourceId == 0)
                throw new ValidationException("产线不能为空".L10N());
            if (shifitId == 0)
                throw new ValidationException("班次不能为空".L10N());
        }

        private void No_MouseDown(object sender, System.Windows.RoutedEventArgs e)
        {
            _manage.ResourceId = _manage.OldResourceId;
            _manage.ShiftId = _manage.OldShiftId;
            SetBtnVisibility();
        }
        #endregion

        double? GetWorkGroupId()
        {
            var employee = _controller.GetEmployeeByUserId(RT.IdentityId);
            if (employee == null)
                throw new ValidationException("员工未关联当前用户".L10N());
            if (!employee.WorkGroupId.HasValue)
                throw new ValidationException("员工[{0}]未分配班组".L10nFormat(employee.Name));
            return employee.WorkGroupId;
        }

        private void AddOnLoanEmployee(object sender, System.Windows.RoutedEventArgs e)
        {
            var filter = _manage.Employees.Select(p => p.Id).ToArray();
            var control = new AddEmployeeControl(filter);
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), control, (w) =>
            {
                w.Commands.Clear();
                w.Commands.Add("取消".L10N());
                w.Commands.Add("确定".L10N());
                w.Title = "选择员工".L10N();
                w.Height = 407;
                w.Width = 839;
            });
        }

        void SetBtnVisibility()
        {
            btnYesNo.Visibility = System.Windows.Visibility.Hidden;
            btnSetting.Visibility = System.Windows.Visibility.Visible;
        }

        Timer timer;
        /// <summary>
        /// 运行
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            LoadResourceShift();
            var interval = _property.RefeshTime <= 0 ? 180000 : _property.RefeshTime * 60000;
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = interval;
            timer.Enabled = true;
            timer.Start();
            LoadEmployees();
        }
        /// <summary>
        /// 关闭后
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
                LoadEmployees();
            });
        }

        /// <summary>
        /// 加载产线班次 
        /// </summary>
        void LoadResourceShift()
        {
            var resourceShift = SettingsHelper.GetResourceShift();
            _manage.ResourceId = _manage.OldResourceId = resourceShift.ResourceId;
            _manage.ShiftId = _manage.OldShiftId = resourceShift.ShiftId;
        }

        void SaveResourceShift()
        {
            var result = "{0};{1}".FormatArgs(_manage.ResourceId, _manage.ShiftId);
            Settings.Default.ResourceShift = result;
            Settings.Default.Save();
        }

        private void LoadEmployees()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ResetDate();
                var date = RF.Find<Employee>().GetDbTime();
                var manage = RT.Service.Resolve<EmployeeManageController>().GetEmployeeManage(WorkGroupId.Value, date.Date);
                _manage.DueQty = manage.DueQty;
                _manage.ArrivedQty = manage.ArrivedQty;
                _manage.AbsenteeismQty = manage.AbsenteeismQty;
                _manage.EmployeeType = manage.EmployeeType;
                manage.Employees.ForEach(emp => { _manage.Employees.Add(emp); });
                manage.OnLoanEmployees.ForEach(emp => { _manage.OnLoanEmployees.Add(emp); });
            }));
        }

        void ResetDate()
        {
            _manage.ArrivedQty = _manage.DueQty = _manage.AbsenteeismQty = 0;
            _manage.Employees.Clear();
            _manage.OnLoanEmployees.Clear();
        }

        /// <summary>
        /// 删除组内员工
        /// </summary>
        /// <param name="sender">SimpleButton</param>
        /// <param name="e">参数</param>
        private void DeleteEmployee(object sender, System.Windows.RoutedEventArgs e)
        {
            var employee = (sender as SimpleButton).DataContext as EmployeeInfo;
            if (employee == null)
            {
                return;
            }
            LoadEmployees();
        }
    }

    /// <summary>
    /// 员工管理输出参数
    /// </summary>
    public class EmployeeManageOutput : ComponentOutput<EmployeeManageControl>
    {
        /// <summary>
        /// 产线ID
        /// </summary>
        [DisplayName("产线ID")]
        [Description("员工管理组件选择产线后输出")]
        public virtual double ResourceId { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        [DisplayName("班次ID")]
        [Description("员工管理组件选择班次后输出")]
        public virtual double ShiftId { get; set; }
    }

    public class ResourceShift
    {
        public double ResourceId { get; set; }

        public double ShiftId { get; set; }
    }

    /// <summary>
    /// 员工管理参数
    /// </summary>
    public class EmployeeManageProperty : ComponentProperty<EmployeeManageControl>
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