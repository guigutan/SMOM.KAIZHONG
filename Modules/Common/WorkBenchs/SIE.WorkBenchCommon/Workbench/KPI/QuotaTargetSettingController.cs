using DocumentFormat.OpenXml.Bibliography;
using SIE.Domain;
using SIE.JDynamics.Reflections;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Domain.Validation;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标目标定义控制器
    /// </summary>
    public class QuotaTargetSettingController : DomainController
    {
        /// <summary>
        /// 获取当月的绩效分析
        /// </summary>
        /// <param name="codeList">指标分类编号，以逗号分割</param>
        /// <returns>返回当月的绩效分析</returns>
        public virtual List<KpiAnalysisData> GetCurrMonthKpiData(List<string> codeList)
        {
            List<KpiAnalysisData> result = new List<KpiAnalysisData>();
            int currMonth = DateTime.Now.Month;
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(QuotaTargetSetting.QuotaTargetDetailListProperty);
            var query = Query<QuotaTargetSetting>().Join<QuotaTargetDetail>((a, b) => a.Id == b.QuotaTargetSettingId)
                .Where<QuotaTargetDetail>((p, q) => p.DataType == DateType.MONTH && q.Month == currMonth && q.State == State.Enable);
            if (codeList.Count > 0)
            {
                query = query.Where<QuotaTargetDetail>((p, q) => codeList.Contains(p.Code));
            }

            EntityList<QuotaTargetSetting> quotaTargetSettingList = query.ToList(null, elo);
            List<string> nameList = quotaTargetSettingList.Select(p => p.Name).ToList();
            EntityList<TargetWarnSetting> targetWarnSettingList = RT.Service.Resolve<TargetWarnSettingController>().GetTargetWarnSetting(nameList);
            quotaTargetSettingList.ForEach(p =>
            {
                QuotaTargetDetail detail = p.QuotaTargetDetailList.FirstOrDefault(q => q.Month == currMonth);
                if (detail != null)
                {
                    TargetWarnSetting twsetting = targetWarnSettingList.FirstOrDefault(tw => tw.Name == p.Name);
                    KpiAnalysisData kpiData = new KpiAnalysisData();
                    kpiData.Name = p.Name;
                    kpiData.GoalFormat = detail.Target.ToString() + "%";
                    kpiData.ActualFormat = detail.Actual.ToString() + "%";
                    if (twsetting != null)
                    {
                        foreach (TargetWarnDetail twDetal in twsetting.TargetWarnDetailList)
                        {
                            if (twDetal.TargetOpetators == TargetOpetators.GreaterOrEqual && detail.Actual >= twDetal.MaxValue)
                            {
                                kpiData.KpiGrade = KpiGrade.Great;
                                break;
                            }
                            else if (twDetal.TargetOpetators == TargetOpetators.LessOrEqual && detail.Actual <= twDetal.MinValue)
                            {
                                kpiData.KpiGrade = KpiGrade.Poor;
                                break;
                            }
                            else if (twDetal.TargetOpetators == TargetOpetators.Between && detail.Actual > twDetal.MinValue && detail.Actual < twDetal.MaxValue)
                            {
                                kpiData.KpiGrade = KpiGrade.Good;
                                break;
                            }
                        }
                    }

                    result.Add(kpiData);
                }
            });

            return result;
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询视图</param>
        /// <returns>返回查询数据</returns>
        public virtual EntityList<QuotaTargetSetting> GetQuota(QuotaTargetSettingCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));
            var q = Query<QuotaTargetSetting>();
            if (criteria.Code.IsNotEmpty())
                q.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                q.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.Dimension.HasValue)
                q.Where(p => p.Dimension == criteria.Dimension);
            if (criteria.EntType.HasValue)
                q.Where(p => p.EntType == criteria.EntType);
            if (criteria.EnterpriseId.HasValue)
                q.Where(p => p.EnterpriseId == criteria.EnterpriseId);
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(QuotaTargetSetting.QuotaTargetDetailListProperty);
            elo.LoadWithViewProperty();
            return q.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 根据指标分类、指标名称、月份获取目标值设定
        /// </summary>
        /// <param name="code">指标分类</param>
        /// <param name="name">指标名称</param>
        /// <param name="month">月份</param>
        /// <param name="enterpriseId">企业模型Id</param>
        /// <returns>返回目标值设定</returns>
        public virtual QuotaTargetDetail GetQuotaMonthTargetSetting(string code, string name, int month, double enterpriseId = 0)
        {
            var query = Query<QuotaTargetDetail>().Join<QuotaTargetSetting>((a, b) => a.QuotaTargetSettingId == b.Id)
                .Where<QuotaTargetSetting>((a, b) => a.Month == month && b.Code == code && b.Name == name && a.State == State.Enable);
            if (enterpriseId > 0)
                query.Where<QuotaTargetSetting>((a, b) => b.Dimension == KPIDimension.Enterprise && b.EnterpriseId == enterpriseId);
            else
                query.Where<QuotaTargetSetting>((a, b) => b.Dimension == KPIDimension.InvOrg);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取时间周期为年的重复数据量
        /// </summary>
        /// <param name="id">指标目标定义明细ID</param>
        /// <param name="year">指标目标定义明细年份</param>
        /// <param name="quotatargetsettingid">指标目标定义ID</param>
        /// <returns>返回符合条件的数据量</returns>
        public virtual int GetQuotaTargetDetailYear(double id, int? year, double quotatargetsettingid)
        {
            return Query<QuotaTargetDetail>().Where(p => p.Id != id && p.Year == year && p.QuotaTargetSettingId == quotatargetsettingid && p.State == State.Enable).Count();
        }

        /// <summary>
        /// 获取时间周期为月的重复数据量
        /// </summary>
        /// <param name="id">指标目标定义明细ID</param>
        /// <param name="year">指标目标定义明细年份</param>
        /// <param name="month">指标目标定义明细月份</param>
        /// <param name="quotatargetsettingid">指标目标定义ID</param>
        /// <returns>返回符合条件的数据量</returns>
        public virtual int GetQuotaTargetDetailMonth(double id, int? year, int? month, double quotatargetsettingid)
        {
            return Query<QuotaTargetDetail>().Where(p => p.Id != id && p.Year == year && p.Month == month && p.QuotaTargetSettingId == quotatargetsettingid && p.State == State.Enable).Count();
        }

        /// <summary>
        /// 获取时间周期为周的重复数据量
        /// </summary>
        /// <param name="id">指标目标定义明细ID</param>
        /// <param name="year">指标目标定义明细年份</param>
        /// <param name="week">指标目标定义明细周</param>
        /// <param name="quotatargetsettingid">指标目标定义ID</param>
        /// <returns>返回符合条件的数据量</returns>
        public virtual int GetQuotaTargetDetailWeek(double id, int? year, int? week, double quotatargetsettingid)
        {
            return Query<QuotaTargetDetail>().Where(p => p.Id != id && p.Year == year && p.Week == week && p.QuotaTargetSettingId == quotatargetsettingid && p.State == State.Enable).Count();
        }
        /// <summary>
        /// 获取指标分类字典
        /// </summary>
        /// <returns></returns>

        public virtual Dictionary<string, string> GetQuotaTargetSettingCodeDic()
        {
            Dictionary<string, string> list = QuotaTargetCategoryHelper.GetCodeDic();
            return list;
        }

        /// <summary>
        /// 获取指标名称字典
        /// </summary>
        /// <returns></returns>

        public virtual Dictionary<string, string> GetQuotaTargetSettingNameDic(string code)
        {
            Dictionary<string, string> list = QuotaTargetCategoryHelper.GetNameDic(code);
            return list;
        }


        /// <summary>
        /// 获取年字典
        /// </summary>
        /// <returns></returns>

        public virtual Dictionary<string, string> GetYearDic()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            for (int i = DateTime.Now.Year; i <= DateTime.Now.Year + 5; i++)
            {
                list.Add(i.ToString(), i.ToString() + "年");
            }
            return list;
        }

        /// <summary>
        /// 获取月字典
        /// </summary>
        /// <returns></returns>

        public virtual Dictionary<string, string> GetMonthDic()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            for (int i = 1; i <= 12; i++)
            {
                list.Add(i.ToString(), i.ToString() + "月");
            }
            return list;
        }

        /// <summary>
        /// 获取周字典
        /// </summary>
        /// <returns></returns>

        public virtual Dictionary<string, string> GetWeekDic()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            for (int i = 1; i <= 53; i++)
            {
                list.Add(i.ToString(), i.ToString() + "周");
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        public virtual bool Exist(QuotaTargetSetting qt)
        {
            var query = Query<QuotaTargetSetting>().Where(c => c.Id != qt.Id && c.Code == qt.Code && c.Name == qt.Name && c.DataType == qt.DataType && c.Dimension == qt.Dimension && c.EnterpriseId == qt.EnterpriseId && c.EntType == qt.EntType);
            var entity = query.FirstOrDefault();
            return entity != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qt"></param>
        public virtual void Save(QuotaTargetSetting qt)
        {

            if (qt.Dimension == KPIDimension.Enterprise && qt.Code == "品质类" || qt.Code == "持续改进")
            {
                throw new ValidationException("指标维度为企业层级，指标分类不能是[品质类]、[持续改进]".L10N());
            }
            RF.Save(qt);
        }
    }

}
