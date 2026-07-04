using SIE.MES.Workbench.EmployeeMarks;
using SIE.Resources.Employees;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SIE.Wpf.MES.Workbench.EmployeeMarks
{
    /// <summary>
    /// EmployeeMarkControl.xaml 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class EmployeeMarkControl : ComponentItem
    {
        /// <summary>
        /// //金杯最小分值
        /// </summary>
        private const int GoldenMark = 95;

        /// <summary>
        /// 金杯图片
        /// </summary>
        private const string GoldenCupImg = @"../Images/GoldenCup1.png";

        /// <summary>
        /// 银杯最小分值
        /// </summary>
        private const int SilverMark = 85; //银杯最小分值

        /// <summary>
        /// 银杯图片
        /// </summary>
        private const string SliverCupImg = @"../Images/GoldenCup2.png";

        /// <summary>
        /// 铜杯最小分值
        /// </summary>
        private const int CopperMark = 75; //铜杯最小分值

        /// <summary>
        /// 铜杯图片
        /// </summary>
        private const string CopperCupImg = @"../Images/GoldenCup3.png";

        /// <summary>
        /// 小于铜杯最小分值的图片
        /// </summary>
        private const string ComeOnCupImg = @"../Images/Come.png";

        /// <summary>
        /// 定时器
        /// </summary>
        private DispatcherTimer _timer;

        /// <summary>
        /// 定时器轮询值
        /// </summary>
        private int _timerLoop = -1;

        /// <summary>
        /// 当前用户的班次Id
        /// </summary>
        private double _meShiftId = 1;

        /// <summary>
        /// 当前用户的资源Id(产线Id)
        /// </summary>
        private double _meResourceId = -1;

        /// <summary>
        /// 当前用户的员工Id
        /// </summary>
        private double _meEmployeeId = -1;

        /// <summary>
        /// 综合评分输入参数
        /// </summary>
        private EmployeeMarkInput _input;

        /// <summary>
        /// 综合评分属性
        /// </summary>
        private EmployeeMarkProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeMarkControl()
        {
            InitializeComponent();
            _input = UseInput<EmployeeMarkInput>();
            _input.PropertyChanged += EmployeeMarkinput_PropertyChanged;
            _property = UseProperty<EmployeeMarkProperty>();
        }

        /// <summary>
        /// 组件Close事件
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            _timer?.Stop();
            _timer = null;
        }

        /// <summary>
        /// OnRun方法
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            GetComponentCommuData();
            GetFormData();
            TimerIni();
        }

        /// <summary>
        /// 组件通信的Input属性变更事件
        /// </summary>
        /// <param name="sender">属性变更事件发送对象</param>
        /// <param name="e">事件参数</param>
        private void EmployeeMarkinput_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GetComponentCommuData();
            GetFormData();
        }

        /// <summary>
        /// Timeer计时器初始化
        /// </summary>
        private void TimerIni()
        {
            if (_timer == null)
                _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromMinutes(_property.TimeSpan <= 0 ? 3 : _property.TimeSpan);
            _timer.IsEnabled = true;
            _timer.Start();
        }

        /// <summary>
        /// Timer计时器的轮询处理事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">事件参数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetTimerRefreshData();
                }));
            });
        }

        /// <summary>
        /// 定时器轮询方法
        /// </summary>
        private void GetTimerRefreshData()
        {
            GetComponentCommuData();
            if (_meEmployeeId > 0 && _meResourceId > 0)
            {
                List<double?> shiftIds = RT.Service.Resolve<EmployeeMarksController>().GetEmpMarkShiftIds(_meResourceId);
                if (shiftIds != null && shiftIds.Count > 0)
                {
                    if (_timerLoop < shiftIds.Count - 1)
                        _timerLoop++;
                    else
                        _timerLoop = 0;

                    var curShiftId = (double)shiftIds[_timerLoop];
                    CalculationEmployeesMark(curShiftId);
                }
            }
        }

        /// <summary>
        /// 获取通信组件
        /// </summary>
        private void GetComponentCommuData()
        {
            /*var employee = GetEmployeeByUserId(_input.EmployeeId);
            _meEmployeeId = employee?.Id ?? 0;*/
            _meEmployeeId = _input.EmployeeId;
            _meResourceId = _input.ResourceId;
        }

        /// <summary>
        /// 界面控件的Context初始化
        /// </summary>
        private void SetDataContext()
        {
            gcScoreTop5.ItemsSource = new ObservableCollection<EmployeeMarkViewData>();
            spBottom.DataContext = new EmployeeMarkViewData();
            spMeScore.DataContext = new EmployeeMarkViewData();
        }

        /// <summary>
        /// 获取通信组件的数据--员工Id、资源Id
        /// </summary>
        private void GetFormData()
        {
            SetDataContext();
            if (_meEmployeeId > 0 && _meResourceId > 0)
            {
                var shiftIds = RT.Service.Resolve<EmployeeMarksController>().GetEmpMarkShiftIds(_meResourceId);
                if (shiftIds != null && shiftIds.Count > 0)
                {
                    _meShiftId = (double)shiftIds.LastOrDefault();
                    GetTop5EmpMark(_meShiftId, _meResourceId);
                    GetMeMarkData(_meEmployeeId, _meShiftId, _meResourceId);
                }
            }
        }

        /// <summary>
        /// 计算当前资源的员工的评分信息
        /// </summary>
        /// <param name="curShiftId">班次Id</param>
        private void CalculationEmployeesMark(double curShiftId)
        {
            SetDataContext();
            if (_meEmployeeId > 0 && _meResourceId > 0)
            {
                GetTop5EmpMark(curShiftId, _meResourceId);
                GetMeMarkData(_meEmployeeId, curShiftId, _meResourceId);
            }
        }

        /// <summary>
        /// 获取个人评分的前5名评分信息
        /// </summary>
        /// <param name="shiftId">班次Id</param>
        /// <param name="resourceId">资源Id</param>
        private void GetTop5EmpMark(double shiftId, double resourceId)
        {
            var top5Marks = RT.Service.Resolve<EmployeeMarksController>().GetTopEmpMark(0, 5, shiftId, resourceId);
            var curIndex = 1;
            ObservableCollection<EmployeeMarkViewData> topMarkDatas = new ObservableCollection<EmployeeMarkViewData>();
            foreach (var item in top5Marks)
            {
                var curMarkData = new EmployeeMarkViewData();
                curMarkData.EmployeeName = item.Employee.Name;
                curMarkData.CurScore = item.Mark.ToString();
                curMarkData.CurRank = curIndex.ToString();
                topMarkDatas.Add(curMarkData);
                curIndex++;
            }

            gcScoreTop5.ItemsSource = topMarkDatas;
        }

        /// <summary>
        /// 获取MySelf的评分信息(姓名、评分、排名、奖杯)
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="shiftId">班次Id</param>
        /// <param name="resourceId">资源Id</param>
        private void GetMeMarkData(double employeeId, double shiftId, double resourceId)
        {
            EmployeeMarkViewData meMarkData = null;
            var empMark = RT.Service.Resolve<EmployeeMarksController>().GetEmpMark(employeeId, shiftId, resourceId);

            if (empMark != null)
            {
                meMarkData = new EmployeeMarkViewData();
                var curRank = RT.Service.Resolve<EmployeeMarksController>().GetRankingOfMe(employeeId, shiftId, resourceId);
                meMarkData.EmployeeName = empMark.Employee.Name;
                meMarkData.CurScore = empMark.Mark.ToString();
                meMarkData.ImgCup = GetMySelfEmpMarkCup(empMark.Mark);
                if (curRank > 0)
                    meMarkData.CurRank = curRank.ToString();
            }

            spBottom.DataContext = meMarkData;
            spMeScore.DataContext = meMarkData;
        }

        /// <summary>
        /// 获取当前用户的奖杯图片
        /// </summary>
        /// <param name="mark">当前用户的评分</param>
        /// <returns>图片对象</returns>
        private BitmapImage GetMySelfEmpMarkCup(decimal mark)
        {
            BitmapImage cupImage;
            if (mark >= GoldenMark)
            {
                cupImage = new BitmapImage(new Uri(GoldenCupImg, UriKind.Relative));
            }
            else if (mark >= SilverMark)
            {
                cupImage = new BitmapImage(new Uri(SliverCupImg, UriKind.Relative));
            }
            else if (mark >= CopperMark)
            {
                cupImage = new BitmapImage(new Uri(CopperCupImg, UriKind.Relative));
            }
            else
            {
                cupImage = new BitmapImage(new Uri(ComeOnCupImg, UriKind.Relative));
            }

            return cupImage;
        }
    }

    /// <summary>
    /// 员工评分属性
    /// </summary>
    public class EmployeeMarkProperty : ComponentProperty<EmployeeMarkControl>
    {
        /// <summary>
        /// 刷新时间(分钟)
        /// </summary>
        [DisplayName("刷新间隔(分钟)"), CategoryAttribute("自定义")]
        [Description("时间必须大于0,默认刷新时间为3分钟")]
        public double TimeSpan { get; set; }
    }
}