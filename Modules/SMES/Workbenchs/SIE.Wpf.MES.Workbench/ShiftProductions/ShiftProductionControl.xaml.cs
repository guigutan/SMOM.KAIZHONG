using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.MES.Statistics.WIP;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.WorkBenchChartBase;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace SIE.Wpf.MES.Workbench.ShiftProductions
{
    /// <summary>
    /// 当班产量 的交互逻辑
    /// </summary>
    [Category("过程分析")]
    public partial class ShiftProductionControl : ComponentItem, IfaceKeyEvent
    {
        /// <summary>
        /// 车间Id
        /// </summary>
        private double _workShopId;

        /// <summary>
        /// 数据库时间
        /// </summary>
        private DateTime? _dbTime;

        /// <summary>
        /// 柱状图初始高度
        /// </summary>
        private decimal _barHeight = 210;

        /// <summary>
        /// 柱状图
        /// </summary>
        private List<SeriesPoint> lsBarPoint = new List<SeriesPoint>();

        #region 当班产量集合 WorkOrderInfos
        /// <summary>
        /// 当班产量集合
        /// </summary>
        private ObservableCollection<ShiftProductEntity> _shiftProductOrders;

        /// <summary>
        /// 当班产量集合
        /// </summary>
        public ObservableCollection<ShiftProductEntity> ShiftProductOrders
        {
            get
            {
                if (_shiftProductOrders == null)
                {
                    _shiftProductOrders = new ObservableCollection<ShiftProductEntity>();
                }

                return _shiftProductOrders;
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftProductionControl()
        {
            InitializeComponent();
            UseProperty<ShiftProductionProperty>();
            shiftGridControl.ItemsSource = ShiftProductOrders;
            ShiftProductionControl owner = this;
            RT.EventBus.Subscribe<WorkShopChangedEvent>(owner, SubscribeHandle);
        }

        /// <summary>
        /// 页面刷新方法
        /// </summary>
        public override void Refresh()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GetDbTime();
            }));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        protected void GetData()
        {
            ShiftProductOrders.Clear();

            DateRange dr = new DateRange() { BeginValue = _dbTime.Value.Date, EndValue = _dbTime.Value.Date.AddDays(1) };
            var workOrderLists = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByDateRange(_workShopId, dr);
            var workOrders = workOrderLists.Where(x => x.Resource.ResourceState != ResourceState.Diseffect);
            var workOrderStatisticsList = RT.Service.Resolve<WipStatisticsController>().GetMonthStatics(_workShopId, dr);
            var workOrderStatisticsGroups = workOrderStatisticsList.GroupBy(p => p.ResourceId);

            foreach (var wosgItem in workOrderStatisticsGroups)
            {
                var curResourceId = wosgItem.Key;
                var curWipResource = RF.GetById<WipResource>(curResourceId);
                if (curWipResource == null)
                    continue;

                var curWosList = wosgItem.ToList();
                var woIds = curWosList.Select(x => x.WorkOrderId).Distinct();
                var curWorkOrders = workOrders.Where(x => woIds.Contains(x.Id));

                decimal planQty = curWorkOrders.Sum(p => p.PlanQty);
                decimal actualQty = curWosList.Sum(p => p.QtyPass);
                decimal failedQty = curWosList.Sum(p => p.QtyFailed);
                decimal fpy = actualQty / (actualQty + failedQty);

                ShiftProductEntity entity = new ShiftProductEntity() { LineName = curWipResource.Name };
                entity.PlanQty = planQty.ToString();
                entity.FinishQty = actualQty.ToString();
                entity.PassRateDecimal = (double)fpy;
                if (planQty > 0)
                {
                    var percent = Math.Round(actualQty / planQty, 3);
                    entity.PassRate = ((double)fpy).ToString("P0");
                    entity.CompleteRate = percent.ToString("P0");
                    decimal lessqty = (planQty - actualQty);
                    entity.LessQty = (lessqty < 0 ? 0 : lessqty).ToString();
                    decimal processdec = (Math.Round(percent, 3) * 160);
                    entity.ProcessValue = processdec.ToString();
                }
                else
                {
                    entity.PassRate = "/";
                    entity.ProcessValue = "0";
                    entity.CompleteRate = string.Empty;
                }

                ShiftProductOrders.Add(entity);
            }

            if (ShiftProductOrders.Count > 0)
            {
                BindBarChart();
                BindChartPanel();
            }
        }

        /// <summary>
        /// 绑定柱状图数据
        /// 总体完成率--总体合格率
        /// </summary>       
        protected void BindBarChart()
        {
            var sumPass = ShiftProductOrders.Sum(p => p.PassRateDecimal) / ShiftProductOrders.Count;
            lsBarPoint.Clear();
            BarChart.Points.Clear();
            var sumPlan = ShiftProductOrders.Sum(p => decimal.Parse(p.PlanQty));
            var sumFinish = ShiftProductOrders.Sum(p => decimal.Parse(p.FinishQty));
            string value = "0";
            if (sumPlan > 0)
                value = (100 * sumFinish / sumPlan).ToString("0.#");
            lsBarPoint.Add(new SeriesPoint() { Argument = "总体完成率".L10N(), Value = double.Parse(value), Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4668A5")) });
            lsBarPoint.Add(new SeriesPoint() { Argument = "总体合格率".L10N(), Value = double.Parse((sumPass * 100).ToString("0.#")), Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A54671")) });
            BarChart.Points.AddRange(lsBarPoint);
            diagram.SetAxisXZoomRatio(0.5);
            BarChart.Animate();
        }

        /// <summary>
        /// 绑定"计划合计/产量合计"
        /// </summary>
        protected void BindChartPanel()
        {
            var sumPlan = ShiftProductOrders.Sum(p => decimal.Parse(p.PlanQty));
            var sumFinish = ShiftProductOrders.Sum(p => decimal.Parse(p.FinishQty));
            var bl = "0";
            if (sumPlan > 0)
                bl = (100 * sumFinish / sumPlan).ToString("0.#");
            var comRate = bl + "%";
            var less = sumPlan - sumFinish;
            if (less == 0)
            {
                LaFinish.Content = "100%";
            }
            else
            {
                LaFinish.Content = string.Empty;
            }

            toopTipsPlan.Text = "计划{0}".L10nFormat(sumPlan.ToString("0.#"));
            toopTipsFin.Text = "已完成{0}({1})".L10nFormat(sumFinish, comRate);
            toopTipsLess.Text = "剩余{0}".L10nFormat(less);
            laPlanQty.Content = sumPlan.ToString("0.#");
            laFinishQty.Content = sumFinish.ToString("0.#");
            laLessQty.Content = less.ToString("0.#");
            laCompleteRate.Content = comRate;
            int finishHeight = 0;
            if (sumPlan > 0)
                finishHeight = int.Parse((_barHeight * sumFinish / sumPlan).ToString("F0"));
            if (finishHeight < 20) finishHeight = 20;
            laCompleteRate.Height = (double)_barHeight - finishHeight;
            LaFinish.Height = finishHeight;
            laLessQty.Height = laCompleteRate.Height;
            laFinishQty.Height = finishHeight;
        }

        /// <summary>
        /// Grid大小改变事件的处理方法
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void Grid_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
            {
                var de = e.NewSize;
                _barHeight = int.Parse(de.Height.ToString("F0")) * 6 / 7 - 20;
                Refresh();
            }
        }

        #region 初始化车间、数据库时间
        /// <summary>
        /// 订阅事件处理
        /// </summary>
        /// <param name="e">e</param>
        private void SubscribeHandle(WorkShopChangedEvent e)
        {
            _workShopId = e.WorkShopId;
            GetDbTime();
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        private void GetDbTime()
        {
            var workShop = RF.GetById<Enterprise>(_workShopId);
            if (workShop == null) return;

            var repository = workShop.GetRepository() as EntityRepository;
            _dbTime = repository.GetDbTime();
            if (_dbTime.HasValue)
                GetData();
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        protected override void OnClose()
        {
            RT.EventBus.Unsubscribe<WorkShopChangedEvent>(this);
            base.OnClose();
        }
        #endregion
    }

    /// <summary>
    /// 工单状态实体
    /// </summary>
    public class ShiftProductEntity : ObservableObject
    {
        /// <summary>
        /// 产线名称
        /// </summary>
        public string LineName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 合格率
        /// </summary>
        public string PassRate
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 完成率
        /// </summary>
        public string CompleteRate
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 完成进度条数值
        /// </summary>
        public string ProcessValue
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public string FinishQty
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 计划数量
        /// </summary>
        public string PlanQty
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public string LessQty
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 实际完成占比高度
        /// </summary>
        public int FinishHeight
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 剩余完成占比高度
        /// </summary>
        public int LessHeight
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 完成比率double
        /// </summary>
        public double CompleteRateDecimal
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 合格比率double
        /// </summary>
        public double PassRateDecimal
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
    }

    /// <summary>
    /// 当班产量属性
    /// </summary>
    public class ShiftProductionProperty : ComponentProperty<ShiftProductionControl>
    {
    }
}