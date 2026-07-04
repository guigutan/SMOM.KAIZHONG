using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.MES.Statistics.WIP;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SIE.Wpf.MES.Workbench.CompletionRates
{
    /// <summary>
    /// Top3客户完成/计划 的交互逻辑
    /// </summary>
    [Category("目标管理")]
    public partial class TopThreeCusCompletionRateControl : ComponentItem
    {
        #region Top3Infos信息集合 Top3Infos
        /// <summary>
        /// 直通率柱状图信息集合
        /// </summary>
        private ObservableCollection<TopTreeEntity> _top3Infos;

        /// <summary>
        /// 计划柱子显示名称
        /// </summary>
        public string _planSerDisName { get; set; } = "计划";

        /// <summary>
        /// 完成柱子显示名称
        /// </summary>
        public string _actualSerDisName { get; set; } = "已完成";

        /// <summary>
        /// 客户名称集合
        /// </summary>
        string[] sname;

        /// <summary>
        /// 完成数量集合
        /// </summary>
        decimal[] valComplete;

        /// <summary>
        /// 计划数量集合
        /// </summary>
        decimal[] valPlan;

        /// <summary>
        /// 客户数量
        /// </summary>
        int cusNum = 3;

        /// <summary>
        /// 直通率柱状图信息集合
        /// </summary>
        public ObservableCollection<TopTreeEntity> Top3Infos
        {
            get
            {
                if (_top3Infos == null)
                {
                    _top3Infos = new ObservableCollection<TopTreeEntity>();
                }

                return _top3Infos;
            }
        }
        #endregion

        /// <summary>
        /// 产能分布
        /// </summary>
        public TopThreeCusCompletionRateControl()
        {
            InitializeComponent();
            planQtySerie.DisplayName = _planSerDisName.L10N();
            finishQtySerie.DisplayName = _actualSerDisName.L10N();
            UseProperty<TopThreeCusCompletionRateProperty>();
            TopThreeCusCompletionRateControl owner = this;
            RT.EventBus.Subscribe<WorkShopChangedEvent>(owner, SubscribeHandle);
        }

        /// <summary>
        /// 初始化数据级图表
        /// </summary>
        private void InitActualPlanChartSeries()
        {
            SetPlanActualSeriesData();
            planQtySerie.Animate();
            finishQtySerie.Animate();
        }

        /// <summary>
        /// 图表数据赋值
        /// </summary>
        private void SetPlanActualSeriesData()
        {
            //获取客户Top3再循环
            finishQtySerie.Points.Clear(); //赋值前清空之前Add的Points
            planQtySerie.Points.Clear();
            if (LoadData())
            {
                for (int i = cusNum; i >= 0; i--)
                {
                    sname[i] = sname[i].Length > 4 ? sname[i].Substring(1, 4) : sname[i];
                    TopTreeEntity entity = new TopTreeEntity();
                    entity.Name = sname[i];
                    entity.PlanQty = valPlan[i];
                    entity.FinishQty = valComplete[i];
                    Top3Infos.Add(entity);
                    var virCom = (valComplete[i] * 100 / valPlan[i]);

                    finishQtySerie.AddPoint(sname[i], 0, (double)virCom);
                    planQtySerie.AddPoint(sname[i], 0, 100);
                }
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        private bool LoadData()
        {
            if (!_dbTime.HasValue)
            {
                return false;
            }
            var woList = RT.Service.Resolve<WorkOrderController>().GetMonthWorkOrders(_workShopId, _dbTime.Value);
            if (woList.Count == 0)
            {
                return false;
            }
            var rst = woList.Where(p => !string.IsNullOrEmpty(p.CustomerName)).GroupBy(p => p.CustomerName).ToDictionary(p => p.Key, p => p.Sum(o => o.PlanQty)).OrderByDescending(p => p.Value);
            var emptyCusQty = woList.Where(p => string.IsNullOrEmpty(p.CustomerName)).Sum(p => p.PlanQty);
            int i = 0;
            cusNum = rst.Count(p => true);
            if (cusNum > 3)
            {
                cusNum = 3;
            }
            sname = new string[cusNum + 1];
            valPlan = new decimal[cusNum + 1];
            valComplete = new decimal[cusNum + 1];
            foreach (var item in rst)
            {
                if (i > 3)
                {
                    break;
                }
                else if (i == 3)
                {
                    sname[i] = "其他";
                    valPlan[i] = rst.Skip(3).Sum(p => p.Value) + emptyCusQty;
                }
                else
                {
                    sname[i] = item.Key;
                    valPlan[i] = item.Value;
                }

                i++;
            }


            ////客户数量不到4个
            if (cusNum < 4)
            {
                sname[cusNum] = "其他";
                valPlan[cusNum] = emptyCusQty;
            }

            var woIds = woList.Select(p => p.Id).ToList();
            var staticDatas = RT.Service.Resolve<WipStatisticsController>().GetMonthStatics(woIds, _dbTime.Value);
            for (int j = 0; j < sname.Length; j++)
            {
                if (j == sname.Length - 1)
                {
                    valComplete[j] = staticDatas.Where(p => !sname.Contains(p.CustomerName)).Sum(p => p.QtyPass);
                }
                else
                {
                    valComplete[j] = staticDatas.Where(p => p.CustomerName == sname[j]).Sum(p => p.QtyPass);
                }
            }

            return true;
        }

        /// <summary>
        /// 客制鼠标放上去的tips显示
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void chartControl_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            foreach (CrosshairElement element in e.CrosshairElements)
            {
                SeriesPoint point = element.SeriesPoint;
                var entity = Top3Infos.FirstOrDefault(p => p.Name == point.Argument);
                if (entity != null)
                {
                    if (point.Series.DisplayName == _actualSerDisName.L10N())
                        element.LabelElement.Text = "已完成 {0}({1:P2})".L10nFormat(entity.FinishQty, (entity.FinishQty / entity.PlanQty).ToString("P2"));
                    if (point.Series.DisplayName == _planSerDisName.L10N())
                    {
                        element.LabelElement.Text = "计划 {0}".L10nFormat(entity.PlanQty);
                    }
                }
            }
        }

        /// <summary>
        /// 客制柱状图的label
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void chartControl_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            var entity = _top3Infos.FirstOrDefault(p => p.Name == e.SeriesPoint.Argument);
            if (e.Series.DisplayName == _planSerDisName.L10N() && entity != null)
            {
                e.LabelsTexts[0] = entity.Name;
                e.LabelsTexts[1] = "{0}/{1}".FormatArgs(entity.FinishQty, entity.PlanQty);
            }
            else if (e.Series.DisplayName == _actualSerDisName.L10N() && entity != null)
            {
                var per = entity.FinishQty / entity.PlanQty;
                if (per == 0) e.LabelText = string.Empty;
                else
                    e.LabelText = (per * 100).ToString("0.#") + "%";
                if (per < 0.2M)
                    e.Series.Label.Padding = new System.Windows.Thickness() { Left = 80, Right = 0, Top = 0, Bottom = 0 };
                else if (per < 0.4M)
                    e.Series.Label.Padding = new System.Windows.Thickness() { Left = 40, Right = 0, Top = 0, Bottom = 0 };
            }
        }

        #region 初始化车间、数据库时间
        /// <summary>
        /// 车间Id
        /// </summary>
        private double _workShopId;

        /// <summary>
        /// 订阅事件处理
        /// </summary>
        /// <param name="e">e</param>
        private void SubscribeHandle(WorkShopChangedEvent e)
        {
            _workShopId = e.WorkShopId;
            GetDbTime();
            if (_dbTime.HasValue)
                InitActualPlanChartSeries();
        }

        /// <summary>
        /// 数据库时间
        /// </summary>
        private DateTime? _dbTime;

        /// <summary>
        /// 获取数据库时间
        /// </summary>     
        private void GetDbTime()
        {
            var workShop = RF.GetById<Enterprise>(_workShopId);
            if (workShop == null) return;
            var repository = workShop.GetRepository() as EntityRepository;
            _dbTime = repository.GetDbTime();
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
    /// 实体
    /// </summary>
    public class TopTreeEntity : ObservableObject
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishQty
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }
    }

    /// <summary>
    /// Top3客户完成/计划属性
    /// </summary>
    public class TopThreeCusCompletionRateProperty : ComponentProperty<TopThreeCusCompletionRateControl>
    {
    }
}