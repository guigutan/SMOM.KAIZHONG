using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.Statistics.Fpy;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 产品报表ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductReportViewModelCriteria))]
    [ArgsSetting(typeof(ProductModelFpySetting)), Label("产品直通率报表")]
    public class ProductReportViewModel : ReportBaseViewModel
    {
        private const string CRITERIA = "criteria";

        /// <summary>
        /// list
        /// </summary>
        public override EntityList DirectRateList
        {
            get
            {
                return this.ProdDirectRateList;
            }
        }

        #region 产品直通率数据列表 ProdDirectRateList
        /// <summary>
        /// 产品直通率数据列表
        /// </summary>
        [Label("产品直通率数据列表")]
        public static readonly ListProperty<EntityList<ProductDirectRateViewModel>> ProdDirectRateListProperty = P<ProductReportViewModel>.RegisterList(e => e.ProdDirectRateList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as ProductReportViewModel).LoadProductCategoryList()
        });

        /// <summary>
        /// 产品直通率数据列表
        /// </summary>
        public EntityList<ProductDirectRateViewModel> ProdDirectRateList
        {
            get { return this.GetLazyList(ProdDirectRateListProperty); }
        }

        private EntityList<ProductDirectRateViewModel> LoadProductCategoryList()
        {
            return new EntityList<ProductDirectRateViewModel>();
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
            if (fieldName == nameof(ProductDirectRateViewModel.ProductModel) && parentFieldName.IsNullOrEmpty())
            {
                return this.ProdDirectRateList.FirstOrDefault(p => p.ProductModel == fieldValue).ModelDirectRate;
            }
            else if (fieldName == nameof(ProductDirectRateViewModel.Product) && parentFieldName == nameof(ProductDirectRateViewModel.ProductModel) && parentFieldValue.IsNotEmpty())
            {
                return this.ProdDirectRateList.FirstOrDefault(p => p.Product == fieldValue && p.ProductModel == parentFieldValue).ProductDirectRate;
            }
            else
            {
                //
            }

            return new FpySetting();
        }

        /// <summary>
        /// 计算需添加样式规则数
        /// </summary>
        /// <returns>样式规则数</returns>
        public override int CountFormatConditions()
        {
            int count = 0;

            count += this.ProdDirectRateList.GroupBy(p => p.ProductModel).Count(p => p.ToList()[0].ModelDirectRateId > 0);
            count += this.ProdDirectRateList.GroupBy(p => p.ProductModel + p.Product).Count(p => p.ToList()[0].ProductDirectRateId > 0);
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

            if (criteria.RowFieldName == nameof(ProductDirectRateViewModel.ProductModel) && criteria.RowParentFieldName.IsNullOrEmpty())
            {
                return RT.Service.Resolve<FpyController>().GetDefectStatisticsForProd(dateRange, modelName: criteria.RowFieldValue);
            }
            else if (criteria.RowFieldName == nameof(ProductDirectRateViewModel.Product) && criteria.RowParentFieldName == nameof(ProductDirectRateViewModel.ProductModel))
            {
                return RT.Service.Resolve<FpyController>().GetDefectStatisticsForProd(dateRange, modelName: criteria.RowParentFieldValue, productName: criteria.RowFieldValue);
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

            if (criteria.RowFieldName == nameof(ProductDirectRateViewModel.ProductModel) && criteria.RowParentFieldName.IsNullOrEmpty())
            {
                return RT.Service.Resolve<FpyController>().GetProdcutFpyStatistics(criteria.RowFieldValue, dateRange: new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 });
            }
            else if (criteria.RowFieldName == nameof(ProductDirectRateViewModel.Product) && criteria.RowParentFieldName == nameof(ProductDirectRateViewModel.ProductModel))
            {
                return RT.Service.Resolve<FpyController>().GetProdcutFpyStatistics(criteria.RowParentFieldValue, criteria.RowFieldValue, dateRange: new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 });
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
            return fieldName == nameof(ProductDirectRateViewModel.ProductModel) || fieldName == nameof(ProductDirectRateViewModel.Product);
        }

        /// <summary>
        /// get custom summeries
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, decimal> GetCustomSummeries()
        {
            return RT.Service.Resolve<ProdReportViewModelController>().GetCustomSummeries();
        }
    }
}
