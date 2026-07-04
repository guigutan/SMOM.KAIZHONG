using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.Statistics.Fpy;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.Reports.LineFPY
{
    /// <summary>
    /// 产线报表ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LineReportViewModelCriteria))]
    [ArgsSetting(typeof(ShopFpySetting)), Label("产线直通率报表")]
    public class LineReportViewModel : ReportBaseViewModel
    {
        private const string CRITERIA = "criteria";
        /// <summary>
        /// 直通率列表
        /// </summary>
        public override EntityList DirectRateList
        {
            get
            {
                return this.LineDirectRateList;
            }
        }

        #region 产线直通率数据列表 LineDirectRateList
        /// <summary>
        /// 产线直通率数据列表
        /// </summary>
        [Label("产线直通率数据列表")]
        public static readonly ListProperty<EntityList<LineDirectRateViewModel>> LineDirectRateListProperty = P<LineReportViewModel>.RegisterList(e => e.LineDirectRateList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as LineReportViewModel).LoadProductCategoryList()
        });

        /// <summary>
        /// 产线直通率数据列表
        /// </summary>
        public EntityList<LineDirectRateViewModel> LineDirectRateList
        {
            get { return this.GetLazyList(LineDirectRateListProperty); }
        }

        private EntityList<LineDirectRateViewModel> LoadProductCategoryList()
        {
            return new EntityList<LineDirectRateViewModel>();
        }
        #endregion
        /// <summary>
        /// 获取直通率设置
        /// </summary>
        /// <param name="fieldName">Field名称</param>
        /// <param name="fieldValue"></param>
        /// <param name="parentFieldName"></param>
        /// <param name="parentFieldValue"></param>
        /// <returns>直通率设置</returns>
        public override FpySetting GetFpySetting(string fieldName, string fieldValue, string parentFieldName = null, string parentFieldValue = null)
        {
            if (fieldName == nameof(LineDirectRateViewModel.Shift) && parentFieldName == nameof(LineDirectRateViewModel.LineName))
            {
                return this.LineDirectRateList.FirstOrDefault(p => p.Shift == fieldValue && p.LineName == parentFieldValue).LineDirectRate;
            }
            else if (fieldName == nameof(LineDirectRateViewModel.LineName) && parentFieldName.IsNullOrEmpty() && parentFieldValue.IsNullOrEmpty())
            {
                return this.LineDirectRateList.FirstOrDefault(p => p.LineName == fieldValue).LineDirectRate;
            }
            else
            {
                //
            }

            return new LineFpySetting();
        }

        /// <summary>
        /// 计算需添加样式规则数
        /// </summary>
        /// <returns>样式规则数</returns>
        public override int CountFormatConditions()
        {
            int count = 0;

            count += this.LineDirectRateList.GroupBy(p => p.LineName).Count(p => p.ToList()[0].LineDirectRateId > 0);
            count += this.LineDirectRateList.GroupBy(p => p.LineName + p.Shift).Count(p => p.ToList()[0].LineDirectRateId > 0);
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
                throw new ArgumentNullException(CRITERIA.L10N());
            }

            var dateRange = ProcessDateTime(criteria);

            if (criteria.RowFieldName == nameof(LineDirectRateViewModel.LineName) && criteria.RowParentFieldName.IsNullOrEmpty())
            {
                return RT.Service.Resolve<FpyController>().GetDefectStatisticsForLine(dateRange, lineName: criteria.RowFieldValue);
            }

            else if (criteria.RowFieldName == nameof(LineDirectRateViewModel.Shift) && criteria.RowParentFieldName == nameof(LineDirectRateViewModel.LineName))
            {
                return RT.Service.Resolve<FpyController>().GetDefectStatisticsForLine(dateRange, lineName: criteria.RowParentFieldValue, shiftName: criteria.RowFieldValue);
            }
            else
            {
                //
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

            if (criteria.RowFieldName == nameof(LineDirectRateViewModel.LineName) && criteria.RowParentFieldName.IsNullOrEmpty())
            {
                return RT.Service.Resolve<FpyController>().GetLineFpyStatistics(criteria.RowFieldValue, dateRange: new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 });
            }
            else if (criteria.RowFieldName == nameof(LineDirectRateViewModel.Shift) && criteria.RowParentFieldName == nameof(LineDirectRateViewModel.LineName))
            {
                return RT.Service.Resolve<FpyController>().GetLineFpyStatistics(criteria.RowParentFieldValue, criteria.RowFieldValue, new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 });
            }
            else
            {
                // 
            }

            return new EntityList<ProcessFpyStatistics>();
        }

        /// <summary>
        /// 根据属性名判断是否打开缺陷统计报表界面
        /// </summary>
        /// <param name="fieldName">属性名</param>
        /// <returns>true/false</returns>
        public override bool IsOpenDefectStatisticsChart(string fieldName)
        {
            return fieldName == nameof(LineDirectRateViewModel.LineName) || fieldName == nameof(LineDirectRateViewModel.Shift);
        }

        /// <summary>
        /// Get custom summeries
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, decimal> GetCustomSummeries()
        {
            return RT.Service.Resolve<LineReportViewModelController>().GetCustomSummeries();
        }
    }
}
