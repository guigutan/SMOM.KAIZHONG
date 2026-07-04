using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.MES.Statistics.Entities;
using SIE.MES.Statistics.WIP;
using SIE.MES.WorkOrders;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace SIE.Wpf.MES.Workbench.WoStatus
{
    /// <summary>
    /// WoStatusControl.xaml 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class WoStatusControl : ComponentItem
    {
        /// <summary>
        /// 工单状态属性
        /// </summary>
        WoStatusProperty _p;

        /// <summary>
        /// 线别ID
        /// </summary>
        private double _lineId;

        /// <summary>
        /// 班次ID
        /// </summary>
        private double _shiftId;

        /// <summary>
        /// 工单状态构造函数
        /// </summary>
        public WoStatusControl()
        {
            InitializeComponent();
            _p = UseProperty<WoStatusProperty>();
            _p.LateTimeRate = 0.25f;
            _p.TargetCompleteRate = 0.75m;
            _p.TimeSpan = 5d;
            var input = UseInput<WoInput>();
            input.PropertyChanged += Input_PropertyChanged;
        }

        /// <summary>
        /// 输入变更事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Input_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var se = sender as WoInput;
            if (e.PropertyName == nameof(se.LineID) || e.PropertyName == nameof(se.ShiftId))
            {
                _lineId = se.LineID;
                _shiftId = se.ShiftId;
                RefreshData();
            }
        }

        #region 工单监控信息集合 WorkOrderInfos

        /// <summary>
        /// 工单监控信息集合
        /// </summary>
        private ObservableCollection<WoStatusEntity> _wostatusInfos;

        /// <summary>
        /// 工单监控信息集合
        /// </summary>
        public ObservableCollection<WoStatusEntity> WoStatusInfos
        {
            get
            {
                if (_wostatusInfos == null)
                {
                    _wostatusInfos = new ObservableCollection<WoStatusEntity>();
                }

                return _wostatusInfos;
            }
        }
        #endregion

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GetWOInfos();
            }));
        }

        /// <summary>
        /// 获取工单列表信息
        /// </summary>
        public void GetWOInfos()
        {
            WoStatusInfos.Clear();

            if (_lineId == 0 || _shiftId == 0)
                return;

            var resource = RF.GetById<WipResource>(_lineId);
            //改为获取数据库时间
            var repository = resource.GetRepository() as EntityRepository;
            var dbdate = repository.GetDbTime();
            var date = dbdate;

            var shiftEntity = RF.GetById<Shift>(_shiftId); //获取lineID
            if (shiftEntity == null)
                return;
            else
                SetShiftDateNowVaue(shiftEntity, date);


            var list = RT.Service.Resolve<WorkOrderController>().GetWoList(_lineId, shiftEntity.BeginTime, shiftEntity.EndTime);
            var statusDic = RT.Service.Resolve<WipStatisticsController>().GetWorkOrderStatusStatics(list.Select(p => p.Id).ToList()).GroupBy(p => p.WorkOrderId).ToDictionary(p => p.Key, p => p.ToList());
            Dictionary<int, WoStatusEntity> myDictionary = new Dictionary<int, WoStatusEntity>();
            if (list != null && list.Count > 0)
            {
                int realindex = 1;
                const int processBackWidth = 280; //进度条底色宽
                foreach (var wo in list)
                {
                    date = dbdate; //后面遇到完工会改这个值
                    WoStatusEntity entity = new WoStatusEntity() { ID = wo.Id };
                    entity.PlanIndex = 0;
                    entity.No = wo.No;
                    entity.RealIndex = realindex;
                    realindex++;
                    entity.ComAndPlan = wo.FinishQty + "/" + wo.PlanQty;
                    SetWorkOrderFpy(statusDic, entity);
                    decimal r = wo.FinishQty / wo.PlanQty;
                    entity.CompleteRate = (Math.Round(r, 3) * 100).ToString("0.#") + "%";
                    decimal processdec = (Math.Round(r, 3) * processBackWidth);
                    entity.ProcessValue = processdec.ToString("F1"); 
                    entity.PlanBeginDate = wo.PlanBeginDate;
                    entity.PlanEndDate = wo.PlanEndDate;
                    entity.ActulEndDate = wo.ActuFinishDate;
                    entity.ActulStartDate = wo.ActuStartDate;
                    entity.FinishQty = wo.FinishQty;
                    entity.ProductStatus = Utils.EnumViewModel.EnumToLabel(wo.State).L10N();
                    TimeSpan tsPlan = wo.PlanEndDate - wo.PlanBeginDate;
                    if (_p.TargetCompleteRate != 0 && _p.LateTimeRate != 0)
                    {
                        SetStatus(entity, date, wo, tsPlan, _p.TargetCompleteRate, _p.LateTimeRate);
                    }
                    else
                    {
                        SetStatus(entity, date, wo, tsPlan);
                    }

                    myDictionary.Add(realindex - 1, entity);
                }
            }

            SetWoStatusInfos(myDictionary);
            statusGridControl.ItemsSource = WoStatusInfos;
        }

        /// <summary>
        /// 设置工单监控信息
        /// </summary>
        /// <param name="myDictionary">序列集合</param>
        public void SetWoStatusInfos(Dictionary<int, WoStatusEntity> myDictionary)
        {
            List<KeyValuePair<int, WoStatusEntity>> lst = new List<KeyValuePair<int, WoStatusEntity>>(myDictionary);
            lst.Sort(delegate (KeyValuePair<int, WoStatusEntity> s1, KeyValuePair<int, WoStatusEntity> s2)
            {
                return s1.Value.PlanBeginDate.CompareTo(s2.Value.PlanBeginDate);
            });
            for (int i = 0; i < lst.Count; i++)
            {
                lst[i].Value.PlanIndex = i + 1;
            }

            lst.Sort(delegate (KeyValuePair<int, WoStatusEntity> s1, KeyValuePair<int, WoStatusEntity> s2)
            {
                return s1.Value.RealIndex.CompareTo(s2.Value.RealIndex);
            });
            myDictionary.Clear();
            foreach (var wo in lst)
            {
                WoStatusEntity entity = new WoStatusEntity();
                entity.PlanIndex = wo.Value.PlanIndex;
                entity.No = wo.Value.No;
                entity.RealIndex = wo.Value.RealIndex;
                entity.ComAndPlan = wo.Value.ComAndPlan;
                entity.PassRate = wo.Value.PassRate;
                entity.ProcessValue = wo.Value.ProcessValue;
                entity.PlanBeginDate = wo.Value.PlanBeginDate;
                entity.PlanEndDate = wo.Value.PlanEndDate;
                entity.ActulEndDate = wo.Value.ActulEndDate;
                entity.ActulStartDate = wo.Value.ActulStartDate;
                entity.FinishQty = wo.Value.FinishQty;
                entity.TargetQty = wo.Value.TargetQty;
                entity.ProductStatus = wo.Value.ProductStatus.L10N();
                entity.ProcessStatus = wo.Value.ProcessStatus.L10N();
                entity.CompleteRate = wo.Value.CompleteRate;
                entity.ProcessColor = wo.Value.ProcessColor;
                WoStatusInfos.Add(entity);
            }
        }

        /// <summary>
        /// 根据条件设置进度状态和颜色
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="dateInput">当前时间</param>
        /// <param name="wo">工单</param>
        /// <param name="tsPlan">计划时长</param>
        /// <param name="targetCompleteRate"></param>
        /// <param name="lateTimeRate"></param>
        /// <returns>带状态实体</returns>
        public void SetStatus(WoStatusEntity entity, DateTime dateInput, WorkOrder wo, TimeSpan tsPlan, decimal targetCompleteRate = 0.75m, float lateTimeRate = 0.25f)
        {
            const string colour = "#E33043";
            var date = dateInput;
            if (tsPlan.TotalMinutes > 0)
            {
                if (wo.ActuFinishDate != null && wo.State == Core.WorkOrders.WorkOrderState.Finish)
                {
                    date = (DateTime)wo.ActuFinishDate;
                }

                if (wo.FinishQty < wo.PlanQty)
                {
                    if (wo.FinishQty == 0)
                    {
                        if (date <= wo.PlanBeginDate)
                        {
                            /*没开始生产没到计划开始时间，不显示进度条*/
                            entity.CompleteRate = string.Empty;
                            entity.ProcessValue = "0";
                            entity.ProcessColor = "gray";
                            entity.ProcessStatus = "未开始";
                        }
                        else
                        {
                            /*超期还没开始*/
                            entity.CompleteRate = "0%";
                            entity.ProcessValue = "40";
                            entity.ProcessColor = colour;
                            entity.ProcessStatus = "滞后未开始";
                        }
                    }
                    else
                    {
                        decimal target = wo.PlanQty * decimal.Parse((((date - wo.PlanBeginDate).TotalMinutes * 1.0 / (tsPlan).TotalMinutes * 1.0).ToString("F2")));

                        if (target > wo.PlanQty)
                        {
                            target = wo.PlanQty;
                        }
                        else if (target < 0)
                        {
                            target = 0;
                        }

                        entity.TargetQty = target.ToString("0.##");
                        if (date <= wo.PlanEndDate)
                        {
                            if (target > 0)
                            {
                                decimal rr = wo.FinishQty / target;
                                if (rr >= 1)
                                {
                                    entity.ProcessColor = "#5CB65E";
                                    entity.ProcessStatus = "进度正常";
                                }
                                else if (rr >= targetCompleteRate)
                                {
                                    entity.ProcessColor = "#FF7200";
                                    entity.ProcessStatus = "进度滞后";
                                }
                                else
                                {
                                    entity.ProcessColor = colour;
                                    entity.ProcessStatus = "严重滞后";
                                }
                            }
                            else
                            {
                                entity.ProcessColor = "#5CB65E";
                                entity.ProcessStatus = "进度正常";
                            }
                        }
                        else
                        {
                            TimeSpan tsLate = (date - wo.PlanBeginDate - tsPlan);
                            if ((tsLate.TotalMinutes * 1.0 / tsPlan.TotalMinutes) <= lateTimeRate)
                            {
                                entity.ProcessColor = "#FF7200";
                                entity.ProcessStatus = "进度滞后";
                            }
                            else
                            {
                                entity.ProcessColor = colour;
                                entity.ProcessStatus = "严重滞后";
                            }
                        }
                    }
                }
            }
            else
            {
                entity.ProcessColor = colour;
                entity.ProcessStatus = "计划周期异常";
            }
        }

        /// <summary>
        /// 设置合格率
        /// </summary>
        /// <param name="statusDic">字典</param>
        /// <param name="entity">实体</param>
        private void SetWorkOrderFpy(Dictionary<double, List<WorkOrderFpyStatistics>> statusDic, WoStatusEntity entity)
        {
            if (statusDic.ContainsKey(entity.ID))
                entity.PassRate = "{0}%".FormatArgs(Math.Round(statusDic[entity.ID].FirstOrDefault().Fpy * 100, 1));
            else
                entity.PassRate = "{0}%".FormatArgs(0);
        }


        /// <summary>
        /// 修改班次的开始时间、结束时间的日期为当前日期
        /// </summary>
        /// <param name="shiftEntity">实体</param>
        /// <param name="date">日期</param>
        /// <returns>班次实体</returns>
        public Shift SetShiftDateNowVaue(Shift shiftEntity, DateTime date)
        {
            var shiftSpan = (shiftEntity.EndTime.Date - shiftEntity.BeginTime.Date).TotalDays; //考虑跨天 
            var shiftBeginHour = shiftEntity.BeginTime.Hour;
            var shiftBeginMinute = shiftEntity.BeginTime.Minute;
            var shiftBeginSecond = shiftEntity.BeginTime.Second;
            shiftEntity.BeginTime = new DateTime(date.Year, date.Month, date.Day, shiftBeginHour, shiftBeginMinute, shiftBeginSecond); //shiftEntity.BeginTime = DateTime.Parse(date.ToShortDateString() + " " + shiftEntity.BeginTime.ToShortTimeString());
            var shiftEndHour = shiftEntity.EndTime.Hour;
            var shiftEndMinute = shiftEntity.EndTime.Minute;
            var shiftEndSecond = shiftEntity.EndTime.Second;
            shiftEntity.EndTime = new DateTime(date.Year, date.Month, date.Day, shiftEndHour, shiftEndMinute, shiftEndSecond);
            shiftEntity.EndTime = shiftEntity.EndTime.AddDays(shiftSpan);

            return shiftEntity;
        }

        /// <summary>
        /// 选择表格事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void statusGridControl_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            var se = (GridControl)sender;
            if (se.SelectedItem != null)
            {
                var entity = se.SelectedItem as WoStatusEntity;
                ctlDetal.DataContext = entity;
                statusGridControl.Visibility = System.Windows.Visibility.Collapsed;
                ctlDetal.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// 明细页点击切换回列表
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ctlDetal_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            statusGridControl.Visibility = System.Windows.Visibility.Visible;
            ctlDetal.Visibility = System.Windows.Visibility.Collapsed;
            statusGridControl.SelectedItem = null;
        }

        /// <summary>
        /// 重写OnRun
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            TimerIni();
            ValidateRate();
            LoadResourceShift();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        void LoadResourceShift()
        {
            var resourceShift = SettingsHelper.GetResourceShift();
            _lineId = resourceShift.ResourceId;
            _shiftId = resourceShift.ShiftId;
            RefreshData();
        }

        /// <summary>
        /// 校验目标完工率参数和逾期时间比参数
        /// </summary>
        private void ValidateRate()
        {
            if (_p.TargetCompleteRate <= 0 || _p.TargetCompleteRate >= 1)
            {
                _p.TargetCompleteRate = 0.75m;
            }
            if (_p.LateTimeRate <= 0 || _p.LateTimeRate >= 1)
            {
                _p.LateTimeRate = 0.25f;
            }
        }

        /// <summary>
        /// 计时器
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        /// Timeer计时器初始化
        /// </summary>
        private void TimerIni()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer();
            }
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMinutes(_p.TimeSpan <= 0 ? 5 : _p.TimeSpan);
            timer.IsEnabled = true;
            timer.Start();
        }

        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            timer?.Stop();
            timer = null;
        }

        /// <summary>
        /// 计时器事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
            {
                RefreshData();
            });
        }
    }

    /// <summary>
    /// 工单状态属性
    /// </summary>
    public class WoStatusProperty : ComponentProperty<WoStatusControl>
    {
        /// <summary>
        /// 刷新时间（分钟）
        /// </summary>
        [DisplayName("刷新间隔(分钟)"), CategoryAttribute("自定义")]
        [Description("时间必须大于0,默认刷新时间为3分钟")]
        public double TimeSpan { get; set; }

        /// <summary>
        /// 目标完工率参数
        /// </summary>
        [DisplayName("目标完工率参数"), CategoryAttribute("自定义")]
        [Description("完工率高于此值并小于1时,显示黄色;低于此值时显示红色")]
        public decimal TargetCompleteRate { get; set; }

        /// <summary>
        /// 逾期时间比参数
        /// </summary>
        [DisplayName("逾期时间比参数"), CategoryAttribute("自定义")]
        [Description("逾期时间比高于此值时显示红色")]
        public float LateTimeRate { get; set; }
    }
}
