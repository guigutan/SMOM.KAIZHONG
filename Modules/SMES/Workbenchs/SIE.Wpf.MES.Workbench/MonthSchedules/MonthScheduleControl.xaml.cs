using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.ShiftProductions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.MonthSchedules
{
    /// <summary>
    /// 月度排产 的交互逻辑
    /// </summary> 
    [Category("过程分析")]
    public partial class MonthScheduleControl : ComponentItem, IfaceKeyEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MonthScheduleControl()
        {
            InitializeComponent();
            UseProperty<MonthScheduleProperty>();
            MonthScheduleControl owner = this;
            RT.EventBus.Subscribe<WorkShopChangedEvent>(owner, SubscribeHandle);
        }

        #region 月度排产信息集合
        /// <summary>
        /// 月度排产信息集合
        /// </summary>
        private ObservableCollection<MonthScheduleInfo> _entityinfos;

        /// <summary>
        /// 月度排产信息集合
        /// </summary>
        public ObservableCollection<MonthScheduleInfo> EntityInfos
        {
            get
            {
                if (_entityinfos == null)
                {
                    _entityinfos = new ObservableCollection<MonthScheduleInfo>();
                }

                return _entityinfos;
            }
        }
        #endregion

        /// <summary>
        /// 数据刷新
        /// </summary>
        public override void Refresh()
        {
            LoadMonthScheduleInfos();
        }

        /// <summary>
        /// 加载月度计划
        /// </summary>
        private void LoadMonthScheduleInfos()
        {
            EntityInfos.Clear();
            if (!_dbTime.HasValue) return;
            var entityList = RT.Service.Resolve<WorkOrderController>().GetMonthWorkOrders(_workShopId, _dbTime.Value);
            if (entityList.Count > 0)
            {
                int i = 1;
                Dictionary<int, MonthScheduleInfo> myDictionary = new Dictionary<int, MonthScheduleInfo>();
                foreach (var item in entityList)
                {
                    MonthScheduleInfo entity = new MonthScheduleInfo();
                    entity.RowNum = i;
                    entity.PlanStartDate = item.PlanBeginDate.ToString("yyyy/MM/dd");
                    entity.LastFinishDate = item.PlanEndDate.ToString("yyyy/MM/dd");
                    entity.WorkOrderNo = item.No;
                    entity.ItemCode = item.Product.Code;
                    entity.PlanQty = item.PlanQty;
                    entity.FinishQty = item.FinishQty.ToString();
                    entity.CustomerName = item.CustomerName;
                    entity.CurrentState = Utils.EnumViewModel.EnumToLabel(item.State).L10N();
                    entity.PlanResource = item.Resource.Name;

                    #region 设置底色类型
                    /*红色0：生产中，上线时间晚于计划开始时间
                      绿色1：生产中，提前或者按计划开始时间上线；
                      灰色8：未开始
                      灰白色9：已完成 / 已关闭
                    顺序：点击状态，按照红 - 绿 - 灰 - 灰白顺序，由上至下排序工单，默认以工单开始时间从先到后由上至下排序*/

                    if (item.State == Core.WorkOrders.WorkOrderState.Close || item.State == Core.WorkOrders.WorkOrderState.Finish)
                    {
                        entity.RowColorType = 9;
                    }
                    else if (item.State == Core.WorkOrders.WorkOrderState.Release)
                    {
                        entity.RowColorType = 8;
                    }
                    else
                    {
                        ////生产中
                        if (item.ActuStartDate > item.PlanBeginDate)
                        {
                            entity.RowColorType = 0;
                        }
                        else
                        {
                            entity.RowColorType = 1;
                        }
                    }
                    #endregion                    
                    myDictionary.Add(i, entity);
                    i++;
                }

                //排序
                List<KeyValuePair<int, MonthScheduleInfo>> lst = new List<KeyValuePair<int, MonthScheduleInfo>>(myDictionary);
                lst.Sort(delegate (KeyValuePair<int, MonthScheduleInfo> s1, KeyValuePair<int, MonthScheduleInfo> s2)
                {
                    return s1.Value.RowColorType.CompareTo(s2.Value.RowColorType);
                });

                i = 1;
                foreach (var item in lst)
                {
                    MonthScheduleInfo entity = new MonthScheduleInfo();
                    entity.RowNum = i;
                    i++;
                    entity.PlanStartDate = item.Value.PlanStartDate;
                    entity.LastFinishDate = item.Value.LastFinishDate;
                    entity.WorkOrderNo = item.Value.WorkOrderNo;
                    entity.ItemCode = item.Value.ItemCode;
                    entity.PlanQty = item.Value.PlanQty;
                    entity.FinishQty = item.Value.FinishQty;
                    entity.CustomerName = item.Value.CustomerName;
                    entity.CurrentState = item.Value.CurrentState;
                    entity.PlanResource = item.Value.PlanResource;
                    entity.RowColorType = item.Value.RowColorType;
                    EntityInfos.Add(entity);
                }

                myDictionary.Clear();
            }

            monthSchedule.ItemsSource = EntityInfos;
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
                LoadMonthScheduleInfos();
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
    /// 月度排产属性
    /// </summary>
    public class MonthScheduleProperty : ComponentProperty<MonthScheduleControl>
    {
    }

    /// <summary>
    /// 页面数据实体
    /// </summary>
    public class MonthScheduleInfo : ObjectModel.ObservableObject
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 计划开始
        /// </summary>
        public string PlanStartDate
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 最晚完工
        /// </summary>
        public string LastFinishDate
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 计划生产数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty<decimal>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 完工数量
        /// </summary>
        public string FinishQty
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string CurrentState
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 计划执行资源
        /// </summary>
        public string PlanResource
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 行颜色类型
        /// </summary>
        public int RowColorType
        {
            get; set;
        }
    }
}