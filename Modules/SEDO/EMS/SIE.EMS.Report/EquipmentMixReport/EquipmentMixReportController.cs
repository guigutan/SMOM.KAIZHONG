using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Report.EquipmentIntegrateStatistics;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Report.EquipmentMixReport
{
    /// <summary>
    /// 设备综合统计报表控制器
    /// </summary>
    public class EquipmentMixReportController : DomainController
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EmsMixReportInfo GetReportData(EquipmentMixReportMonViewModelCriteria criteria)
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
            if (!criteria.EquipName.IsNullOrEmpty() && !criteria.EquipCodeId.HasValue)
            {
                query.Where(w => w.Name == criteria.EquipName);
            }
            var equipments = query.ToList();
            if (!equipments.Any())
            {
                throw new ValidationException("查询无设备".L10N());
            }
            var equipmentIds = equipments.Select(p => p.Id).ToList();

            if (criteria.UtilizationCriteria <= 0)
            {
                throw new ValidationException("查询【利用率标准】输入不正确,请输入正确".L10N());
            }
            if (equipmentIds.Count <= 0 && (criteria.EquipTypeId.HasValue || criteria.EquipModelId.HasValue || criteria.EquipCodeId.HasValue
                || criteria.UseDepartmentId.HasValue || criteria.WorkShopId.HasValue || criteria.WipResourceId.HasValue))
            {
                throw new ValidationException("筛选条件未能找到任何设备!请检查".L10N());
            }
            if (!criteria.Year.HasValue)
            {
                throw new ValidationException("请输入年份!".L10N());
            }
            //设置开始时间和结束时间
            var startDate = new DateTime(criteria.Year.Value.Year, 1, 1);
            var endDate = new DateTime(criteria.Year.Value.Year, 12, 31);

            if (endDate > DateTime.Now)
            {
                endDate = DateTime.Now.Date.AddDays(1);
            }
            //获取统计信息
            var equipmentIntegrateStatisticList = RT.Service.Resolve<EquipmentIntegrateStatisticController>().GetStatistics(equipmentIds, startDate, endDate);
            var reportInfo = new EmsMixReportInfo();
            reportInfo.EsdPassRateReport = new EsdPassRateReportInfo();
            reportInfo.EsdNgList = new Domain.EntityList<EquipmentMixReportMonViewModel>();
            reportInfo.Year = criteria.Year.Value.Year.ToString();
            reportInfo.EquipmentCount = equipmentIds.Count.ToString();

            const int decimals = 1;
            ComputeData(criteria, equipmentIntegrateStatisticList, reportInfo, decimals);

            //为了减少计算误差 上面计算时候不取舍小数点
            reportInfo.EsdNgList.ForEach(it =>
            {
                if (it.FirstColumn != "设备利用率".L10N())
                {
                    it.January = Math.Round(Convert.ToDecimal(it.January), decimals).ToString();
                    it.February = Math.Round(Convert.ToDecimal(it.February), decimals).ToString();
                    it.March = Math.Round(Convert.ToDecimal(it.March), decimals).ToString();

                    it.April = Math.Round(Convert.ToDecimal(it.April), decimals).ToString();
                    it.May = Math.Round(Convert.ToDecimal(it.May), decimals).ToString();
                    it.July = Math.Round(Convert.ToDecimal(it.July), decimals).ToString();

                    it.June = Math.Round(Convert.ToDecimal(it.June), decimals).ToString();
                    it.August = Math.Round(Convert.ToDecimal(it.August), decimals).ToString();
                    it.September = Math.Round(Convert.ToDecimal(it.September), decimals).ToString();

                    it.October = Math.Round(Convert.ToDecimal(it.October), decimals).ToString();
                    it.November = Math.Round(Convert.ToDecimal(it.November), decimals).ToString();
                    it.December = Math.Round(Convert.ToDecimal(it.December), decimals).ToString();
                }
            });

            return reportInfo;
        }

        /// <summary>
        /// 组合数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="equipmentIntegrateStatisticList"></param>
        /// <param name="reportInfo"></param>
        /// <param name="decimals"></param>
        private void ComputeData(EquipmentMixReportMonViewModelCriteria criteria,
            EntityList<EquipmentIntegrateStatistic> equipmentIntegrateStatisticList,
            EmsMixReportInfo reportInfo, int decimals)
        {
            if (equipmentIntegrateStatisticList.IsNotEmpty())
            {
                //组合数据
                var fields = typeof(EquipmentIntegrateStatistic).GetFields();
                var notMatchFields = new List<string>(){
                    "EquipAccountIdProperty",
                    "EquipAccountProperty",
                        "StatisticDateProperty",
                };

                EquipmentMixReportMonViewModel runningTimeProperty = null; // 运行时长
                EquipmentMixReportMonViewModel numberOfFailuresProperty = null;//故障次数
                EquipmentMixReportMonViewModel failureTimeProperty = null; // 故障时长
                EquipmentMixReportMonViewModel planningTimeProperty = null; // 计划时间

                foreach (var field in fields)
                {
                    var fieldName = field.Name;

                    if (notMatchFields.Contains(fieldName))
                    {
                        continue;
                    }

                    EquipmentMixReportMonViewModel eMRM = CreateEquipmentMixReportMonViewModel(criteria, equipmentIntegrateStatisticList, field);

                    if ("RunningTimeProperty" == fieldName)
                    {
                        runningTimeProperty = eMRM;
                    }

                    if ("NumberOfFailuresProperty" == fieldName)
                    {
                        numberOfFailuresProperty = eMRM;
                    }

                    if ("EquipMentFailureTimeProperty" == fieldName)
                    {
                        failureTimeProperty = eMRM;
                    }

                    if ("PlanningTimeProperty" == fieldName)
                    {
                        planningTimeProperty = eMRM;
                    }

                    reportInfo.EsdNgList.Add(eMRM);
                }

                //MTBF（h）运行时长/故障次数
                var MTBF = new EquipmentMixReportMonViewModel();
                var modelFields = typeof(EquipmentMixReportMonViewModel).GetFields();
                var notMatchs = new List<string>(){
                    "FirstColumnProperty"
                };

                //计算MTBF
                ComputeMtbf(runningTimeProperty, numberOfFailuresProperty, MTBF, modelFields, notMatchs);

                var MTTR = new EquipmentMixReportMonViewModel();
                MTTR.FirstColumn = "MTTR（h)";
                foreach (var field in modelFields)
                {
                    var fieldName = field.Name;
                    if (notMatchs.Contains(fieldName))
                    {
                        continue;
                    }
                    var numberOfFailuresValue = AnalyseCommonHelper.GetStatisticProperties(numberOfFailuresProperty, field.Name);
                    var failureTimeValue = AnalyseCommonHelper.GetStatisticProperties(failureTimeProperty, field.Name);
                    var setValue = numberOfFailuresValue > 0 ? decimal.Round(failureTimeValue / numberOfFailuresValue, 1) : 0;
                    AnalyseCommonHelper.SetStatisticProperties(MTTR, field.Name, setValue.ToString());
                }
                reportInfo.EsdNgList.AddRange(new List<EquipmentMixReportMonViewModel> { MTTR, MTBF });

                var utilizationRate = reportInfo.EsdNgList.FirstOrDefault(m => m.FirstColumn == "设备利用率".L10N());
                if (utilizationRate != null)
                {
                    var reportFields = typeof(EquipmentMixReportMonViewModel).GetFields();
                    var notSetFields = new List<string>() { "FirstColumnProperty" };

                    foreach (var reportField in reportFields)
                    {
                        if (notSetFields.Contains(reportField.Name))
                        {
                            continue;
                        }
                        var rate = new EsdPassRateDataInfo();
                        rate.Month = GetLabel(reportField.Name);
                        var planningTime = AnalyseCommonHelper.GetStatisticProperties(planningTimeProperty, reportField.Name);
                        rate.UtilizationRate = planningTime > 0 ? AnalyseCommonHelper.GetStatisticProperties(runningTimeProperty, reportField.Name) / planningTime : 0.0m;

                        AnalyseCommonHelper.SetStatisticProperties(utilizationRate, reportField.Name, rate.UtilizationRate == 0.0m ? "0.0%" :
                            (Math.Round(rate.UtilizationRate * 100, decimals)).ToString() + "%");
                        if (criteria.UtilizationCriteria > 0)
                        {
                            rate.UtilizationStandards = Convert.ToDecimal(criteria.UtilizationCriteria) / 100;
                        }
                        else
                        {
                            rate.UtilizationStandards = 0m;
                        }
                        reportInfo.EsdPassRateReport.EsdPassRateDatas.Add(rate);
                    }
                }
            }
        }

        /// <summary>
        /// 计算MTBF
        /// </summary>
        /// <param name="runningTimeProperty"></param>
        /// <param name="numberOfFailuresProperty"></param>
        /// <param name="MTBF"></param>
        /// <param name="modelFields"></param>
        /// <param name="notMatchs"></param>
        private void ComputeMtbf(EquipmentMixReportMonViewModel runningTimeProperty,
            EquipmentMixReportMonViewModel numberOfFailuresProperty,
            EquipmentMixReportMonViewModel MTBF,
            System.Reflection.FieldInfo[] modelFields, List<string> notMatchs)
        {
            if (modelFields == null)
            {
                return;
            }
            MTBF.FirstColumn = "MTBF（h）";
            foreach (var field in modelFields)
            {
                var fieldName = field.Name;
                if (notMatchs.Contains(fieldName))
                {
                    continue;
                }
                var numberOfFailuresValue = AnalyseCommonHelper.GetStatisticProperties(numberOfFailuresProperty, field.Name);
                var runningTimeValue = AnalyseCommonHelper.GetStatisticProperties(runningTimeProperty, field.Name);
                var setValue = numberOfFailuresValue > 0 ? decimal.Round(runningTimeValue / numberOfFailuresValue, 1) : 0;
                AnalyseCommonHelper.SetStatisticProperties(MTBF, field.Name, setValue.ToString());
            }
        }

        private EquipmentMixReportMonViewModel CreateEquipmentMixReportMonViewModel(EquipmentMixReportMonViewModelCriteria criteria,
            EntityList<EquipmentIntegrateStatistic> equipmentIntegrateStatisticList, System.Reflection.FieldInfo field)
        {
            var eMRM = new EquipmentMixReportMonViewModel();
            var label = field.CustomAttributes.FirstOrDefault(m => m.AttributeType.FullName == typeof(LabelAttribute).FullName);
            eMRM.FirstColumn = label != null ? label.ConstructorArguments.First().Value.ToString().L10N() : "";

            eMRM.January = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 1 && m.StatisticDate.Year == criteria.Year.Value.Year)
            .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.February = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 2 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.March = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 3 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.April = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 4 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.May = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 5 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.June = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 6 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.July = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 7 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.August = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 8 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.September = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 9 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.October = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 10 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.November = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 11 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();

            eMRM.December = equipmentIntegrateStatisticList.Where(m => m.StatisticDate.Month == 12 && m.StatisticDate.Year == criteria.Year.Value.Year)
                .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)).ToString();
            return eMRM;
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private string GetLabel(string field)
        {
            var fieldInfo = typeof(EquipmentMixReportMonViewModel).GetFields().FirstOrDefault(m => m.Name == field);
            var label = fieldInfo.CustomAttributes.FirstOrDefault(m => m.AttributeType.FullName == typeof(LabelAttribute).FullName);
            return label.ConstructorArguments.First().Value.ToString();

        }
    }
}
