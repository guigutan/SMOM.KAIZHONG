using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.WorkBenchChartBase;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.ShiftProductions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SIE.Wpf.MES.Workbench.LineStates
{
    /// <summary>
    /// 产线状态 的交互逻辑
    /// </summary>
    [Category("过程分析")]
    public partial class LineStateControl : ComponentItem, IfaceKeyEvent
    {
        /// <summary>
        /// 车间Id--从车间组件获取输入值
        /// </summary>
        private double? _workShopId = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LineStateControl()
        {
            InitializeComponent();
            UseProperty<LineStatProperty>();
            LineStateControl owner = this;
            RT.EventBus.Subscribe<WorkShopChangedEvent>(owner, LineStateSubscribeHandle);

        }

        /// <summary>
        /// 订阅事件的处理方法
        /// </summary>
        /// <param name="obj">车间变更事件</param>
        private void LineStateSubscribeHandle(WorkShopChangedEvent obj)
        {
            _workShopId = obj.WorkShopId;
            var workShop = RF.GetById<Enterprise>(_workShopId);
            if (workShop == null) return;
            ControlManager();
        }

        /// <summary>
        /// 关闭后处理方法
        /// </summary>
        protected override void OnClose()
        {
            RT.EventBus.Unsubscribe<WorkShopChangedEvent>(this);
            base.OnClose();
        }

        /// <summary>
        /// 接口IfaceKeyEvent方法的实现
        /// </summary>
        public override void Refresh()
        {
            ControlManager();
        }

        /// <summary>
        /// 组件方法大总管
        /// </summary>
        private void ControlManager()
        {
            var _abnormalCauses = GetAllLineAbnormalCause();
            var productingLineStates = InitLineState(_abnormalCauses);
            InitChart(productingLineStates);
        }

        /// <summary>
        /// 获取所有产线的异常停线信息
        /// </summary>
        /// <returns>异常停线集合</returns>
        private EntityList<AbnormalCause> GetAllLineAbnormalCause()
        {
            var currentTime = RF.Find<AbnormalCause>().GetDbTime();
            EntityList<AbnormalCause> abnormalCauses = RT.Service.Resolve<AbnormalCauseController>().GetAllLineAbnormalCause(currentTime);
            return abnormalCauses;
        }

        /// <summary>
        /// 使用产线异常集合初始化ItemsControl
        /// </summary>
        /// <param name="abnormalCauses">异常停线集合</param>
        /// <returns>产线状态集合</returns>
        private ObservableCollection<LineState> InitLineState(EntityList<AbnormalCause> abnormalCauses)
        {
            ObservableCollection<LineState> productingLineStates = new ObservableCollection<LineState>();
            var allWipResources = GetWipResources();

            foreach (var itemWipResource in allWipResources)
            {
                var itemCause = abnormalCauses.FirstOrDefault(x => x.ResourceId == itemWipResource.Id);
                if (itemCause == null)
                    productingLineStates.Add(new LineState { LineName = itemWipResource.Name, State = ExceptionStopType.Normal });
                else
                    productingLineStates.Add(new LineState { LineName = itemWipResource.Name, State = itemCause.ExceptionStopType });
            }

            ctlLineState.ItemsSource = productingLineStates;
            return productingLineStates;
        }

        /// <summary>
        /// 获取生产资源
        /// 排除自定义资源、失效资源
        /// </summary>
        /// <returns>生产资源集合</returns>
        private EntityList<WipResource> GetWipResources()
        {
            ////var allLineResources = RT.Service.Resolve<EnterpriseController>().GetAllLineResouuces();
            var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
            var typeList = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
            var allWipResources = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, _workShopId, typeList, null, null);
            return allWipResources;
        }

        /// <summary>
        /// 使用产线异常比例信息初始化ChartControl
        /// </summary>
        /// <param name="productingLineStates">产线状态集合</param>
        void InitChart(ObservableCollection<LineState> productingLineStates)
        {
            var chartDataList = GetStatesPercent(productingLineStates);
            LineChart.DataSource = chartDataList;
            LineSeries.ArgumentDataMember = "Name";
            LineSeries.ValueDataMember = "Value";
        }

        /// <summary>
        /// 获取产线异常的比例信息: 正常、停线、保养
        /// </summary>
        /// <param name="productingLineStates">产线状态集合</param>
        /// <returns>产线状态圆环图数据集合</returns>
        private ObservableCollection<ChartData> GetStatesPercent(ObservableCollection<LineState> productingLineStates)
        {
            ObservableCollection<ChartData> chartDataList = new ObservableCollection<ChartData>();
            decimal totalCount = productingLineStates?.Count ?? 0;
            if (totalCount > 0)
            {
                decimal stateStopPercent = 0;
                decimal stateNormalPercent = 0;
                decimal stateMaintainPercent = 0;
                decimal stopCount = productingLineStates.Count(p => p.State == ExceptionStopType.StopLine);
                decimal normalCount = productingLineStates.Count(p => p.State == ExceptionStopType.Normal);
                stateStopPercent = Math.Round(stopCount / totalCount, 2);
                stateNormalPercent = Math.Round(normalCount / totalCount, 2);
                stateMaintainPercent = 1 - stateStopPercent - stateNormalPercent;

                chartDataList.Add(new ChartData { Name = ExceptionStopType.Maintain.ToLabel(), Value = stateMaintainPercent }); //#128BEF
                chartDataList.Add(new ChartData { Name = ExceptionStopType.StopLine.ToLabel(), Value = stateStopPercent }); //#FF7200
                chartDataList.Add(new ChartData { Name = ExceptionStopType.Normal.ToLabel(), Value = stateNormalPercent });  //#5BB65E
            }

            return chartDataList;
        }
    }

    /// <summary>
    /// 产线状态属性
    /// </summary>
    public class LineStatProperty : ComponentProperty<LineStateControl>
    {
    }
}