using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 接口下载异常报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DownloadExcViewModelCriteria))]
    [Label("接口下载异常报表")]
    public partial class DownloadExcReportViewModel : ViewModel
    {
        #region 接口下载异常列表 DownloadExcList
        /// <summary>
        /// 接口下载异常列表
        /// </summary>
        [Label("接口下载异常列表")]
        public static readonly ListProperty<EntityList<DownloadExcViewModel>> DownloadExcListProperty = P<DownloadExcReportViewModel>.RegisterList(e => e.DownloadExcList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as DownloadExcReportViewModel).LoadWorkOrderReachList()
        });

        /// <summary>
        /// 接口下载异常列表
        /// </summary>
        public EntityList<DownloadExcViewModel> DownloadExcList
        {
            get { return this.GetLazyList(DownloadExcListProperty); }
        }

        /// <summary>
        /// 加载数据列表
        /// </summary>
        /// <returns>数据列表</returns>
        private EntityList<DownloadExcViewModel> LoadWorkOrderReachList()
        {
            return new EntityList<DownloadExcViewModel>();
        }
        #endregion      

        #region 布局文件名 LayoutFileName
        /// <summary>
        /// 布局文件名
        /// </summary>
        [Label("布局文件名")]
        public static readonly Property<string> LayoutFileNameProperty = P<DownloadExcReportViewModel>.Register(e => e.LayoutFileName);

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
        public static readonly Property<DownloadExcViewModelCriteria> CriteriaProperty = P<DownloadExcReportViewModel>.Register(e => e.Criteria);

        /// <summary>
        /// 查询条件
        /// </summary>
        public DownloadExcViewModelCriteria Criteria
        {
            get { return this.GetProperty(CriteriaProperty); }
            set { this.SetProperty(CriteriaProperty, value); }
        }
        #endregion       

    }
}
