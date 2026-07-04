using SIE.Domain;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.Statistics.Fpy;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.MES.DashBoard.Reports.Commons
{
    /// <summary>
    /// 报表基类ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public abstract class ReportBaseViewModel : ViewModel
    {
        #region 布局文件名 LayoutFileName
        /// <summary>
        /// 布局文件名
        /// </summary>
        [Label("布局文件名")]
        public static readonly Property<string> LayoutFileNameProperty = P<ReportBaseViewModel>.Register(e => e.LayoutFileName);

        /// <summary>
        /// 布局文件名
        /// </summary>
        public string LayoutFileName
        {
            get { return this.GetProperty(LayoutFileNameProperty); }
            set { this.SetProperty(LayoutFileNameProperty, value); }
        }
        #endregion

        #region 直通率配置类型 SettingType
        /// <summary>
        /// 直通率配置类型
        /// </summary>
        [Label("直通率配置类型")]
        public static readonly Property<Type> SettingTypeProperty = P<ReportBaseViewModel>.Register(e => e.SettingType);

        /// <summary>
        /// 直通率配置类型
        /// </summary>
        public Type SettingType
        {
            get { return this.GetProperty(SettingTypeProperty); }
            set { this.SetProperty(SettingTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public abstract EntityList DirectRateList { get; }

        /// <summary>
        /// 获取直通率设置
        /// </summary>
        /// <param name="fieldName">栏位名称</param>
        /// <param name="fieldValue">栏位值</param>
        /// <param name="parentFieldName">父栏位名称</param>
        /// <param name="parentFieldValue">父栏位值</param>
        /// <returns>直通率设置</returns>
        public abstract FpySetting GetFpySetting(string fieldName, string fieldValue, string parentFieldName = null, string parentFieldValue = null);

        /// <summary>
        /// 计算需添加样式规则数
        /// </summary>
        /// <returns>样式规则数</returns>
        public abstract int CountFormatConditions();

        /// <summary>
        /// 获取工序缺陷Top5数据源
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>缺陷统计列表</returns>
        public abstract EntityList<DefectStatistics> GetDefectTopDataSource(DefectStatisticsCriteria criteria);

        /// <summary>
        /// 获取工序直通率统计数据源
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>直通率统计列表</returns>
        public abstract EntityList<ProcessFpyStatistics> GetProcessFpyDataSource(DefectStatisticsCriteria criteria);

        /// <summary>
        /// 根据属性名判断是否打开缺陷统计报表界面
        /// </summary>
        /// <param name="fieldName">属性名</param>
        /// <returns>true/false</returns>
        public abstract bool IsOpenDefectStatisticsChart(string fieldName);

        /// <summary>
        /// 求日期属性(年、月、 周、日)的范围
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>日期组（开始时间，结束日期时间）</returns>
        protected Tuple<DateTime, DateTime> ProcessDateTime(DefectStatisticsCriteria criteria)
        {

            switch (criteria.ColumnFieldName)
            {
                case nameof(DirectRateBaseViewModel.Year):
                    return Tuple.Create(DateTime.Parse(criteria.ColumnFieldValue + "年"), DateTime.Parse(criteria.ColumnFieldValue + "年").AddYears(1));

                case nameof(DirectRateBaseViewModel.Month):
                    return Tuple.Create(DateTime.Parse(criteria.ColumnFieldValue), DateTime.Parse(criteria.ColumnFieldValue).AddMonths(1));

                case nameof(DirectRateBaseViewModel.Week):
                    {
                        int year = int.Parse(criteria.ColumnFieldValue.Substring(0, 4));
                        int week = int.Parse(criteria.ColumnFieldValue.Substring(6, 2));
                        return RT.Service.Resolve<CommonController>().GetFirstEndDayOfWeek(year, week);
                    }

                case nameof(DirectRateBaseViewModel.Date):
                default:
                    return Tuple.Create(DateTime.Parse(criteria.ColumnFieldValue), DateTime.Parse(criteria.ColumnFieldValue).AddDays(1));
            }
        }

        /// <summary>
        /// 获取行自定义求和值
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, decimal> GetCustomSummeries()
        {
            return new Dictionary<string, decimal>();
        }
    }

    /// <summary>
    /// 缺陷统计查询实体
    /// </summary>
    [Serializable]
    public class DefectStatisticsCriteria
    {
        /// <summary>
        /// 当前行属性名称
        /// </summary>
        public string RowFieldName { get; set; }

        /// <summary>
        /// 当前行属性值
        /// </summary>
        public string RowFieldValue { get; set; }

        /// <summary>
        /// 当前行父层级属性名称
        /// </summary>
        public string RowParentFieldName { get; set; }

        /// <summary>
        /// 当前行父层级值
        /// </summary>
        public string RowParentFieldValue { get; set; }

        /// <summary>
        /// 当前列属性名称
        /// </summary>
        public string ColumnFieldName { get; set; }

        /// <summary>
        /// 当前列属性值
        /// </summary>
        public string ColumnFieldValue { get; set; }
    }
}

