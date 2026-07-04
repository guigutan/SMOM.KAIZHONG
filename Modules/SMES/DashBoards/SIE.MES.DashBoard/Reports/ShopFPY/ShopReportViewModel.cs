using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.Statistics.Fpy;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.Reports.ShopFPY
{
    /// <summary>
    /// 车间报表ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ShopReportViewModelCriteria))]
    [ArgsSetting(typeof(ShopFpySetting)), Label("车间直通率报表")]
    public class ShopReportViewModel : ReportBaseViewModel
    {
        private const string CRITERIA = "criteria";

        /// <summary>
        /// 直通率列表
        /// </summary>
        public override EntityList DirectRateList
        {
            get
            {
                return this.ShopDirectRateList;
            }
        }

        #region 车间直通率数据列表 ShopDirectRateList
        /// <summary>
        /// 车间直通率数据列表
        /// </summary>
        [Label("车间直通率数据列表")]
        public static readonly ListProperty<EntityList<ShopDirectRateViewModel>> ShopDirectRateListProperty = P<ShopReportViewModel>.RegisterList(e => e.ShopDirectRateList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as ShopReportViewModel).LoadProductCategoryList()
        });

        /// <summary>
        /// 车间直通率数据列表
        /// </summary>
        public EntityList<ShopDirectRateViewModel> ShopDirectRateList
        {
            get { return this.GetLazyList(ShopDirectRateListProperty); }
        }

        private EntityList<ShopDirectRateViewModel> LoadProductCategoryList()
        {
            return new EntityList<ShopDirectRateViewModel>();
        }
        #endregion
  
        /// <summary>
        /// 获取直通率设置
        /// </summary>
        /// <param name="fieldName">选项名称</param>
        /// <param name="fieldValue">选项值</param>
        /// <param name="parentFieldName">父选项名称</param>
        /// <param name="parentFieldValue">父选项值</param>
        /// <returns>直通率设置</returns>
        public override FpySetting GetFpySetting(string fieldName, string fieldValue, string parentFieldName = null, string parentFieldValue = null)
        {
            if (fieldName == nameof(ShopDirectRateViewModel.ShopName) && parentFieldName.IsNullOrEmpty())
            {
                return ShopDirectRateList.FirstOrDefault(p => p.ShopName == fieldValue).ShopDirectRate;
            }
            else if (fieldName == nameof(ShopDirectRateViewModel.LineName) && parentFieldName == nameof(ShopDirectRateViewModel.ShopName) && parentFieldValue.IsNotEmpty())
            {
                return ShopDirectRateList.FirstOrDefault(p => p.LineName == fieldValue && p.ShopName == parentFieldValue).LineDirectRate;
            }
            else
            {
                return new FpySetting();
            }
        }

        /// <summary>
        /// 计算需添加样式规则数
        /// </summary>
        /// <returns>样式规则数</returns>
        public override int CountFormatConditions()
        {
            int count = 0;

            count += this.ShopDirectRateList.GroupBy(p => p.ShopName).Count(p => p.ToList()[0].ShopDirectRateId > 0);
            count += this.ShopDirectRateList.GroupBy(p => p.ShopName + p.LineName).Count(p => p.ToList()[0].LineDirectRateId > 0);
            return count * 4;
        }

        /// <summary>
        /// 获取工序缺陷Top5数据源
        /// </summary>
        /// <returns>缺陷统计列表</returns>
        public override EntityList<DefectStatistics> GetDefectTopDataSource(DefectStatisticsCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(CRITERIA);
            }

            var dateRange = ProcessDateTime(criteria);

            if (criteria.RowFieldName == nameof(ShopDirectRateViewModel.LineName))
            {
                return RT.Service.Resolve<FpyController>().GetDefectStatisticsForLine(dateRange, lineName: criteria.RowFieldValue);
            }

            return new EntityList<DefectStatistics>();
        }

        /// <summary>
        /// 获取工序直通率统计数据源
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>直通率统计列表</returns>
        public override EntityList<ProcessFpyStatistics> GetProcessFpyDataSource(DefectStatisticsCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(CRITERIA);
            }

            var dateRange = ProcessDateTime(criteria);

            if (criteria.RowFieldName == nameof(ShopDirectRateViewModel.ShopName) && criteria.RowParentFieldName.IsNullOrEmpty())
            {
                return RT.Service.Resolve<FpyController>().GetShopFpyStatistics(criteria.RowFieldValue, dateRange: new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 });
            }
            else if (criteria.RowFieldName == nameof(ShopDirectRateViewModel.LineName) && criteria.RowParentFieldName == nameof(ShopDirectRateViewModel.ShopName))
            {
                return RT.Service.Resolve<FpyController>().GetShopFpyStatistics(criteria.RowParentFieldValue, criteria.RowFieldValue, new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 });
            }
            else
            {
                //
            }

            return new EntityList<ProcessFpyStatistics>();
        }

        /// <summary>
        /// get custom summeries
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, decimal> GetCustomSummeries()
        {
            return RT.Service.Resolve<ShopReportViewModelController>().GetCustomSummeries();
        }

        /// <summary>
        /// 根据属性名判断是否打开缺陷统计报表界面
        /// </summary>
        /// <param name="fieldName">属性名</param>
        /// <returns>true/false</returns>
        public override bool IsOpenDefectStatisticsChart(string fieldName)
        {
            return fieldName == nameof(ShopDirectRateViewModel.LineName);
        }
    }
}
