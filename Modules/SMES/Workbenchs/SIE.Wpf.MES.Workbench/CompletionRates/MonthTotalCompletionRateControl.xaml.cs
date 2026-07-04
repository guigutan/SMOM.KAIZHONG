using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.MES.Statistics.WIP;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Common.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.CompletionRates
{
    /// <summary>
    /// 月度总量达成 的交互逻辑
    /// </summary>
    [Category("目标管理")]
    [QuotaAttribute(_groupName, _title)]
    public partial class MonthTotalCompletionRateControl : ComponentItem
    {
        /// <summary>
        /// 指标分类
        /// </summary>
        private const string _groupName = "效率类";

        /// <summary>
        /// 指标名称
        /// </summary>
        private const string _title = "月度总量达成";

        /// <summary>
        /// 构造函数
        /// </summary>
        public MonthTotalCompletionRateControl()
        {
            InitializeComponent();
            UseProperty<MonthTotalCompletionRateProperty>();
            MonthTotalCompletionRateControl owner = this;
            RT.EventBus.Subscribe<WorkShopChangedEvent>(owner, SubscribeHandle);
        }

        /// <summary>
        /// SeriesPoint
        /// </summary>
        private List<SeriesPoint> lsChartPoint = new List<SeriesPoint>();

        /// <summary>
        /// 刷新读数
        /// </summary>
        public override void Refresh()
        {
            LoadData();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            if (!_dbTime.HasValue) return;
            QuotaTargetDetail quotaTargetDetail = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaMonthTargetSetting(_groupName, _title, _dbTime.Value.Month, _workShopId);
            if (quotaTargetDetail == null) return;
            var target = quotaTargetDetail.Target;
            decimal complete = RT.Service.Resolve<WipStatisticsController>().GetMonthTotal(_workShopId, _dbTime.Value);
            laComplete.Content = complete.ToString("0.#");
            laTarget.Content = target.ToString("0.#");
            lsChartPoint.Clear();
            nestedChart.Points.Clear();
            lsChartPoint.Add(new SeriesPoint() { Argument = "未完成".L10N(), Value = double.Parse((target - complete).ToString("0.#")) });
            lsChartPoint.Add(new SeriesPoint() { Argument = "已完成".L10N(), Value = double.Parse((complete).ToString("0.#")) });
            nestedChart.Points.AddRange(lsChartPoint);
            if (target > 0)
                laPercent.Content = ((complete / target) * 100).ToString("0.#") + "%";
            else
                laPercent.Content = "0%";
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
    /// 月度总量达成属性
    /// </summary>
    public class MonthTotalCompletionRateProperty : ComponentProperty<MonthTotalCompletionRateControl>
    {
    }
}