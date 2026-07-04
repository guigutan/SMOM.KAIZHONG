using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 工单达成率报表ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkOrderReachCriteria))]
    [Label("工单达成率报表")]
    public class WoReachReportViewModel : ViewModel
    {
        #region 工单达成率数据列表 ProdDirectRateList
        /// <summary>
        /// 工单达成率数据列表
        /// </summary>
        [Label("工单达成率数据列表")]
        public static readonly ListProperty<EntityList<WorkOrderReachViewModel>> WorkOrderReachListProperty = P<WoReachReportViewModel>.RegisterList(e => e.WorkOrderReachList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as WoReachReportViewModel).LoadWorkOrderReachList()
        });

        /// <summary>
        /// 工单达成率数据列表
        /// </summary>
        public EntityList<WorkOrderReachViewModel> WorkOrderReachList
        {
            get { return this.GetLazyList(WorkOrderReachListProperty); }
        }

        /// <summary>
        /// 加载数据列表
        /// </summary>
        /// <returns>数据列表</returns>
        private EntityList<WorkOrderReachViewModel> LoadWorkOrderReachList()
        {
            return new EntityList<WorkOrderReachViewModel>();
        }
        #endregion      

        #region 布局文件名 LayoutFileName
        /// <summary>
        /// 布局文件名
        /// </summary>
        [Label("布局文件名")]
        public static readonly Property<string> LayoutFileNameProperty = P<WoReachReportViewModel>.Register(e => e.LayoutFileName);

        /// <summary>
        /// 布局文件名
        /// </summary>
        public string LayoutFileName
        {
            get { return this.GetProperty(LayoutFileNameProperty); }
            set { this.SetProperty(LayoutFileNameProperty, value); }
        }
        #endregion

        #region 查询条件 Criteria
        /// <summary>
        /// 查询条件
        /// </summary>
        [Label("查询条件")]
        public static readonly Property<WorkOrderReachCriteria> CriteriaProperty = P<WoReachReportViewModel>.Register(e => e.Criteria);

        /// <summary>
        /// 查询条件
        /// </summary>
        public WorkOrderReachCriteria Criteria
        {
            get { return this.GetProperty(CriteriaProperty); }
            set { this.SetProperty(CriteriaProperty, value); }
        }
        #endregion       
    }
}
