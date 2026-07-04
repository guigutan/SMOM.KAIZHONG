using SIE.Domain;
using SIE.MES.Statistics.Fpy;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.Wpf.MES.Workbench.MonthFpys
{
    /// <summary>
    /// 直通率 的交互逻辑
    /// </summary>
    [Category("目标管理")]
    [QuotaAttribute(_groupName, _title)]
    public partial class MonthFypControl : ComponentItem
    {
        /// <summary>
        /// 指标分类
        /// </summary>
        private const string _groupName = "效率类";

        /// <summary>
        /// 指标名称
        /// </summary>
        private const string _title = "日均直通率";

        /// <summary>
        /// 构造函数
        /// </summary>
        public MonthFypControl()
        {
            InitializeComponent();
            UseProperty<MonthFypProperty>();
            MonthFypControl owner = this;
            RT.RemotingEventBus.Subscribe<WorkShopChangedEvent>(owner, SubscribeHandle);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            decimal currentRate = 0.9M;
            decimal passRate = 1;
            decimal lastMonthReal = 0.8M;
            decimal lastMonthTarget = 0.8M;
            QuotaTargetDetail quotaTargetDetail = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaMonthTargetSetting(_groupName, _title, _dbTime.Value.Month, _workShopId);
            QuotaTargetDetail preQuotaTargetDetail = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaMonthTargetSetting(_groupName, _title, _dbTime.Value.Month - 1, _workShopId);
            var curMonth = DateTime.Parse(_dbTime.Value.AddMonths(-1).Year + "-" + _dbTime.Value.AddMonths(-1).Month + "-01");
            DateRange dr = new DateRange() { BeginValue = curMonth, EndValue = _dbTime };
            var shopFpyStatistics = RT.Service.Resolve<FpyController>().GetShopProductFpyStatistics(_workShop.Name, null, dr);
            currentRate = GetCurRate(shopFpyStatistics, new DateRange() { BeginValue = curMonth.AddMonths(1), EndValue = _dbTime.Value.AddDays(1) });
            lastMonthReal = GetCurRate(shopFpyStatistics, new DateRange() { BeginValue = curMonth, EndValue = curMonth.AddMonths(1) });
            if (quotaTargetDetail != null)
            {
                ////currentRate = quotaTargetDetail.Actual;
                passRate = quotaTargetDetail.Target;
            }

            if (preQuotaTargetDetail != null)
            {
                //// lastMonthReal = preQuotaTargetDetail.Actual;
                lastMonthTarget = preQuotaTargetDetail.Target;
            }

            CompletionRates.CompletionRatesEntity com = new CompletionRates.CompletionRatesEntity();
            com.CurMonthReal = (currentRate * 100).ToString("0.#") + "%";
            com.CurMonthTarget = (passRate).ToString("0.#") + "%";
            com.LastMonthReal = (lastMonthReal * 100).ToString("0.#") + "%";
            com.LastMonthTarget = (lastMonthTarget).ToString("0.#") + "%";
            GridContent.DataContext = com;
        }

        /// <summary>
        /// 汇总产线每天的直通率
        /// </summary>
        /// <param name="shopFpyStatistics">直通率数据集合</param>
        /// <param name="dr">时间范围</param>
        /// <returns>车间月度直通率</returns>
        public decimal GetCurRate(EntityList<ProductFpyStatistics> shopFpyStatistics, DateRange dr)
        {
            var dics = new Dictionary<DateTime, decimal>();
            var dayNums = 0;
            shopFpyStatistics.Where(p => p.CollectedDate >= dr.BeginValue && p.CollectedDate < dr.EndValue).GroupBy(p => p.CollectedDate)
             .ForEach(p =>
                    {
                        decimal processRate = 1;
                        dayNums++;
                        processRate = (p.Sum(x => x.InputQty) - p.Sum(x => x.FailedQty)) / p.Sum(x => x.InputQty);
                        dics.Add(p.Key, processRate);
                    });
            if (dayNums == 0) return 0;
            return dics.Sum(p => p.Value) / dayNums;
        }

        #region 初始化车间、数据库时间
        /// <summary>
        /// 车间Id
        /// </summary>
        private double _workShopId;

        /// <summary>
        /// 车间
        /// </summary>
        private Enterprise _workShop;

        /// <summary>
        /// 订阅事件处理
        /// </summary>
        /// <param name="e">e</param>
        private void SubscribeHandle(WorkShopChangedEvent e)
        {
            _workShopId = e.WorkShopId;
            GetDbTime();
            if (_dbTime.HasValue)
                LoadData();
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
            _workShop = RF.GetById<Enterprise>(_workShopId);
            if (_workShop == null) return;
            var repository = _workShop.GetRepository() as EntityRepository;
            _dbTime = repository.GetDbTime();
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        protected override void OnClose()
        {
            RT.RemotingEventBus.Subscribe<WorkShopChangedEvent>(this, SubscribeHandle);
            base.OnClose();
        }
        #endregion
    }

    /// <summary>
    /// 直通率属性
    /// </summary>
    public class MonthFypProperty : ComponentProperty<MonthFypControl>
    {
    }
}