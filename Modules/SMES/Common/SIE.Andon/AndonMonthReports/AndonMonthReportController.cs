using SIE.Andon.Andons;
using SIE.Andon.AndonStatisticsReports;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Andon.AndonMonthReports
{
    /// <summary>
    /// 
    /// </summary>
    public class AndonMonthReportController : DomainController
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual AndonMonthReportInfos GetReportData(AndonMonthViewModelCriteria criteria)
        {
            var query = Query<AndonManage>().Where(m => m.State != Andons.Enum.AndonManageState.Cancel);
            var andonMonthReportInfos = new AndonMonthReportInfos();
            if (criteria.AndonClass.HasValue)
            {
                query.Where(m => m.AndonManageClass == criteria.AndonClass);
            }
            if (criteria.AndonNameId.HasValue)
            {
                query.Where(m => m.AndonId == criteria.AndonNameId);
            }
            if (criteria.AndonTypeId.HasValue)
            {
                query.Where(m => m.AndonTypeId == criteria.AndonTypeId);
            }
            if (criteria.FactoryId.HasValue)
            {
                query.Where(m => m.FactoryId == criteria.FactoryId);
            }
            if (criteria.WorkShopId.HasValue)
            {
                query.Where(m => m.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.WipResourceId.HasValue)
            {
                query.Where(m => m.WipResourceId == criteria.WipResourceId);
            }
            if (criteria.EquipAccountId.HasValue)
            {
                query.Where(m => m.EquipAccountId == criteria.EquipAccountId);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(m => m.Andon.DepartmentId == criteria.DepartmentId);
            }
            var startDate = new DateTime(criteria.CreateTime.Year, 1, 1);
            var endDate = startDate.AddMonths(12);

            query.Where(m => m.CreateDate >= startDate && m.CreateDate < endDate);

            //先执行条件查询所有数据
            var res = query.ToList(null, new EagerLoadOptions().LoadWith(AndonManage.AbnormalCauseProperty).LoadWithViewProperty());
            var oprateList = new EntityList<AndonManageOperateLog>();
            if (res.Any())
            {
                var andonManageIds = res.Select(p => p.Id).ToList();
                oprateList = andonManageIds.SplitContains(ids =>
                {
                    return Query<AndonManageOperateLog>().Where(m => ids.Contains(m.AndonManageId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            }
            FiterGroupResultInfos(criteria, andonMonthReportInfos, res, oprateList);


            return andonMonthReportInfos;
        }
        private void FiterGroupResultInfos(AndonMonthViewModelCriteria criteria, AndonMonthReportInfos andonReportInfos,
           EntityList<AndonManage> res,
           EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            switch (criteria.GroupLevel)
            {
                //工厂、车间、产线、设备、责任部门、产品、触发人
                case GroupLevel.Factory:
                    GetFactory(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;

                case GroupLevel.ProductionLine:
                    ProductionLine(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Department:
                    GetDepartment(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Equipment:
                    GetEquipment(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Workshop:
                    GetWorkshop(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Product:
                    GetProduct(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Trigger:
                    GetTrigger(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
            }
        }
        /// <summary>
        /// 获取工厂分类
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetFactory(AndonMonthViewModelCriteria criteria, AndonMonthReportInfos andonReportInfos, EntityList<AndonManage> res,
            EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            var startDate = new DateTime(criteria.CreateTime.Year, 1, 1);
            var endDate = startDate.AddMonths(12);
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.FactoryId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "工厂".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯大类".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().FactoryName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonManageClass.ToLabel();
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.FactoryId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "工厂".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯编码".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().FactoryName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);


                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.FactoryId, p.AndonTypeId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "工厂".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯类型".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().FactoryName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonTypeName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
        }

        /// <summary>
        /// 获取产线
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void ProductionLine(AndonMonthViewModelCriteria criteria, AndonMonthReportInfos andonReportInfos, EntityList<AndonManage> res,
            EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            var startDate = new DateTime(criteria.CreateTime.Year, 1, 1);
            var endDate = startDate.AddMonths(12);
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.WipResourceId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "产线".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯大类".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().WipResourceName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonManageClass.ToLabel();
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.WipResourceId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "产线".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯编码".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().WipResourceName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);


                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.FactoryId, p.WipResourceId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "产线".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯类型".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().WipResourceName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonTypeName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
        }
        
        /// <summary>
        /// 获取责任部门分类
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetDepartment(AndonMonthViewModelCriteria criteria, AndonMonthReportInfos andonReportInfos, EntityList<AndonManage> res,
           EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            var startDate = new DateTime(criteria.CreateTime.Year, 1, 1);
            var endDate = startDate.AddMonths(12);
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.Andon.DepartmentId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "责任部门".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯大类".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().Andon.DepartmentName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonManageClass.ToLabel();
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.Andon.DepartmentId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "责任部门".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯编码".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().Andon.DepartmentName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);


                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonTypeId, p.Andon.DepartmentId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "责任部门".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯类型".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().Andon.DepartmentName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonTypeName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
        }

        /// <summary>
        /// 获取设备分类
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetEquipment(AndonMonthViewModelCriteria criteria, AndonMonthReportInfos andonReportInfos, EntityList<AndonManage> res,
           EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            var startDate = new DateTime(criteria.CreateTime.Year, 1, 1);
            var endDate = startDate.AddMonths(12);
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.EquipAccountId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "设备编码".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯大类".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().EquipAccountCode;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonManageClass.ToLabel();
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.EquipAccountId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "设备编码".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯编码".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().EquipAccountCode;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);


                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonTypeId, p.EquipAccountId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "设备编码".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯类型".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().EquipAccountCode;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonTypeName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
        }

        /// <summary>
        ///获取车间
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetWorkshop(AndonMonthViewModelCriteria criteria, AndonMonthReportInfos andonReportInfos, EntityList<AndonManage> res,
           EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            var startDate = new DateTime(criteria.CreateTime.Year, 1, 1);
            var endDate = startDate.AddMonths(12);
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.WorkShopId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "车间".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯大类".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().WorkShopName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonManageClass.ToLabel();
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.WorkShopId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "车间".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯编码".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().WorkShopName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);


                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonTypeId, p.WorkShopId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "车间".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯类型".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().WorkShopName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonTypeName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
        }

        private void GetProduct(AndonMonthViewModelCriteria criteria, AndonMonthReportInfos andonReportInfos, EntityList<AndonManage> res,
           EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            var startDate = new DateTime(criteria.CreateTime.Year, 1, 1);
            var endDate = startDate.AddMonths(12);
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.ProductCode }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "产品".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯大类".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().ProductCode;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonManageClass.ToLabel();
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.ProductCode }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "产品".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯编码".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().ProductCode;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);


                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonTypeId, p.ProductCode }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "产品".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯类型".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().ProductCode;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonTypeName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
        }

        private void GetTrigger(AndonMonthViewModelCriteria criteria, AndonMonthReportInfos andonReportInfos, EntityList<AndonManage> res,
           EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            var startDate = new DateTime(criteria.CreateTime.Year, 1, 1);
            var endDate = startDate.AddMonths(12);
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.TriggerId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "触发人".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯大类".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().TriggerByName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonManageClass.ToLabel();
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.TriggerId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "触发人".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯编码".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().TriggerByName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);


                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonTypeId, p.TriggerId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var andonMonthReportViewModel = new AndonMonthReportViewModel();
                    andonMonthReportViewModel.GroupNameTitle = "触发人".L10N();
                    andonMonthReportViewModel.SummaryDimensionTitle = "安灯类型".L10N();

                    for (var beginTime = startDate; beginTime < endDate; beginTime = beginTime.AddMonths(1))//循环12个月
                    {
                        var monthDatas = equipmentItemInfos[key].Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                        if (monthDatas.Any())
                        {
                            double andonStopLine = GetAndonStopLine(monthDatas);
                            var logs = andonManageOperateLogs.Where(m => m.CreateDate >= beginTime && m.CreateDate < beginTime.AddMonths(1)).ToList();
                            var modelItem = SetStatisticsItemValue(monthDatas, logs, andonStopLine);
                            SetResultData(andonMonthReportViewModel, beginTime, modelItem);
                            andonMonthReportViewModel.GroupName = monthDatas.First().TriggerByName;
                            andonMonthReportViewModel.SummaryDimension = monthDatas.First().AndonTypeName;
                        }
                    }
                    andonReportInfos.andonMonthReportViewModels.Add(andonMonthReportViewModel);
                }
            }
        }

        /// <summary>
        /// 设置每个月统计的结果
        /// </summary>
        /// <param name="andonMonthReportViewModel"></param>
        /// <param name="beginTime"></param>
        /// <param name="modelItem"></param>
        private void SetResultData(AndonMonthReportViewModel andonMonthReportViewModel, DateTime beginTime, AndonStatisticsViewModel modelItem)
        {
            var modelFields = typeof(AndonMonthReportViewModel).GetFields();
            //排除的字段
            var notMatchs = new List<string>(){
                        "GroupNameProperty",
                       "SummaryDimensionProperty",
                       "GroupNameTitleProperty",
                       "SummaryDimensionTitleProperty"
                };
            foreach (var field in modelFields)
            {
                var fieldName = field.Name;
                if (notMatchs.Contains(fieldName))
                {
                    continue;
                }

                string result = System.Text.RegularExpressions.Regex.Replace(field.Name, @"[^0-9]+", "");
                if (result==beginTime.Month.ToString())
                {

                    var setValue = 0m;
                    if (field.Name.Contains("AndonStopLine"))
                    {
                        setValue = (decimal)modelItem.AndonStopLine;
                    }
                    if (field.Name.Contains("AndonNum"))
                    {
                        setValue = (decimal)modelItem.AndonNum;
                    }
                    if (field.Name.Contains("AndonStopNum"))
                    {
                        setValue = (decimal)modelItem.AndonStopNum;
                    }
                    if (field.Name.Contains("AndonTime"))
                    {
                        setValue = (decimal)modelItem.AndonTime;
                    }
                    if (field.Name.Contains("TriggerAccuracy"))
                    {
                        setValue = (decimal)modelItem.TriggerAccuracy;
                    }
                    AnalyseCommonHelper.SetStatisticProperties(andonMonthReportViewModel, field.Name, setValue);
                }


            }
        }

        /// <summary>
        /// 获取停线时长
        /// </summary>
        /// <param name="equipmentItemInfos"></param>
        /// <returns></returns>
        private double GetAndonStopLine(List<AndonManage> equipmentItemInfos)
        {
            //计算停线时间
            var abnormalCauseList = equipmentItemInfos.Where(m => m.LineStop && m.AbnormalCauseId.HasValue).Select(m => m.AbnormalCause).ToList();
            var andonStopLine = 0d;
            abnormalCauseList.ForEach(item =>
            {
                var endtime = item.EndDate.HasValue ? item.EndDate.Value : DateTime.Now;
                andonStopLine += (endtime - item.BeginDate).TotalSeconds / 3600;//除3600
            });
            return andonStopLine;
        }

        /// <summary>
        /// 统计数据
        /// </summary>
        /// <param name="GroupListItemInfos"></param>
        /// <param name="andonManageOperateLogs"></param>
        /// <param name="andonStopLine"></param>
        /// <returns></returns>
        private AndonStatisticsViewModel SetStatisticsItemValue(List<AndonManage> GroupListItemInfos,
            List<AndonManageOperateLog> andonManageOperateLogs, double andonStopLine)
        {
            var oprateHadChangeNames = andonManageOperateLogs.Where(p => GroupListItemInfos.Select(m => m.Id).Contains(p.AndonManageId) && p.OperateType == Andons.Enum.AndonManageOperateType.AndonNameChange).ToList();
            var triggerAccuracy = oprateHadChangeNames.Count > 0 ? Math.Round((double)oprateHadChangeNames.Count / GroupListItemInfos.Count * 100, 2) : 0;

            var modelItem = new AndonStatisticsViewModel()
            {
                AndonNum = GroupListItemInfos.Count,
                AndonStopNum = GroupListItemInfos.Count(m => m.LineStop),
                AndonTime = Math.Round(GroupListItemInfos.Where(m => m.LastTime.HasValue).Sum(m => m.LastTime.Value), 2),
                AndonStopLine = Math.Floor(andonStopLine),//向上取整数
                TriggerAccuracy = triggerAccuracy,
            };
            return modelItem;
        }
    }
}
