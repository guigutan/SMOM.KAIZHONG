using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.API.APIModels;
using SIE.EMS.Report.EquipmentIntegrateStatistics;
using SIE.Equipments.EquipAccounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Report.MttrAndMtbfReports
{
    /// <summary>
    /// MTTR/MTBF统计报表控制器
    /// </summary>
    public partial class MttrAndMtbfReportViewModelController : DomainController
    {
        /// <summary>
        /// 获取设备故障统计报告
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual Dictionary<int, List<EquipmentFaultStatistics>> GetFaultStatisticsReport(MttrAndMtbfReportViewModelCriteria criteria)
        {
            var query = Query<EquipAccount>();

            if (criteria.EquipTypeId.HasValue && criteria.EquipTypeId != 0)
            {
                query.Where(w => w.EquipModel.EquipTypeId == criteria.EquipTypeId);
            }
            if (criteria.EquipModelId.HasValue && criteria.EquipModelId != 0)
            {
                query.Where(w => w.EquipModelId == criteria.EquipModelId);
            }
            if (criteria.EquipCodeId.HasValue && criteria.EquipCodeId != 0)
            {
                query.Where(w => w.Id == criteria.EquipCodeId);
            }
            if (criteria.UseDepartmentId.HasValue && criteria.UseDepartmentId != 0)
            {
                query.Where(w => w.UseDepartmentId == criteria.UseDepartmentId);
            }
            if (criteria.WorkShopId.HasValue && criteria.WorkShopId != 0)
            {
                query.Where(w => w.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.WipResourceId.HasValue && criteria.WipResourceId != 0)
            {
                query.Where(w => w.ResourceId == criteria.WipResourceId);
            }

            if (criteria.Year == new DateTime(2000, 1, 1))
            {
                throw new ValidationException("查询条件【年份】不能为空，请检查！".L10N());
            }

            EntityList<EquipAccount> accounts = query.ToList();
            Dictionary<int, List<EquipmentFaultStatistics>> dic = new Dictionary<int, List<EquipmentFaultStatistics>>();

            if (accounts.Any())
            {
                var statiList = GetFaultStatisticsReport(accounts, criteria.Year.Year);
                dic[accounts.Count] = statiList;
            }
            else
            {
                dic[accounts.Count] = new List<EquipmentFaultStatistics>();
            }
            return dic;
        }

        /// <summary>
        /// 获取设备故障统计报告
        /// </summary>
        /// <returns></returns>
        public virtual List<EquipmentFaultStatistics> GetFaultStatisticsReport(EntityList<EquipAccount> accounts, int currenYear)
        {
            var dic = new Dictionary<int, EquipmentFaultStatistics>();
            var startDate = new DateTime(currenYear, 1, 1);
            var endDate = new DateTime(currenYear, 12, 31);
            if (endDate > DateTime.Now)
            {
                endDate = DateTime.Now.Date.AddDays(1);
            }
            for (int i = 1; i <= 12; i++)
            {
                dic.Add(i, new EquipmentFaultStatistics()
                {
                    Month = currenYear + "-" + i
                });
            }
            var results = RT.Service.Resolve<EquipmentIntegrateStatisticController>().GetStatistics(accounts.Select(m => m.Id).ToList(), startDate, endDate);
            if (results.Any())
            {
                for (int i = 1; i <= 12; i++)
                {
                    var equipMentFailureTime = (double)results.Where(m => m.StatisticDate.Month == i).Sum(m => m.EquipMentFailureTime);
                    var runningTime = (double)results.Where(m => m.StatisticDate.Month == i).Sum(m => m.RunningTime);
                    var times = (double)results.Where(m => m.StatisticDate.Month == i).Sum(m => m.NumberOfFailures);

                    dic[i].EquipMentFailureTime = Math.Round(equipMentFailureTime, 1);
                    dic[i].RunningTime = Math.Round(runningTime, 1);
                    dic[i].Mtbf = Math.Round(times != 0 ? runningTime / times : 0, 1);
                    dic[i].Mttr = Math.Round(times != 0 ? equipMentFailureTime / times : 0, 1);//故障时长/故障次数
                    dic[i].RepairTimeTotal = Math.Round((double)results.Where(m => m.StatisticDate.Month == i).Sum(m => m.RepairTime), 1);
                    dic[i].Count = (int)times;
                }
            }
            return dic.Values.ToList();
        }
    }
}
