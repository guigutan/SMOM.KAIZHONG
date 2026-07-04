using DotLiquid.Util;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Common;
using SIE.MES.ItemChecker;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemEquipAccount.Configs;
using SIE.MES.ItemFixture;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WorkOrders.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.PreStartupSetupRecords
{
    public class PreStartupSetupRecordsController : DomainController
    {
        /// <summary>
        /// 开机准备记录查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<PreStartupSetupRecord> CriteriaPreStartupSetupRecord(PreStartupSetupRecordCriteria criteria)
        {
            var q = Query<PreStartupSetupRecord>();
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.WorkOrder.No.Contains(criteria.WorkOrderNo));
            if (!criteria.TaskNo.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.No.Contains(criteria.TaskNo));
            if (!criteria.Process.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Process.Code.Contains(criteria.Process) || p.DispatchTask.Process.Name.Contains(criteria.Process));
            if (!criteria.ResourceCode.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Resource.Code.Contains(criteria.ResourceCode));
            if (!criteria.ResourceName.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Resource.Name.Contains(criteria.ResourceName));
            if (!criteria.ItemCode.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Product.Code.Contains(criteria.ItemCode));
            if (!criteria.ItemName.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Product.Name.Contains(criteria.ItemName));
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Product.ShortDescription.Contains(criteria.ShortDescription));
            if (!criteria.ToolCode.IsNullOrEmpty())
                q.Where(p => p.ToolCode.Contains(criteria.ToolCode));
            if (!criteria.ToolName.IsNullOrEmpty())
                q.Where(p => p.ToolName.Contains(criteria.ToolName));
            if (criteria.CheckerFixtureType != null)
                q.Where(p => p.CheckerFixtureType == criteria.CheckerFixtureType);
            if (criteria.CreateDate.BeginValue != null)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue != null)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            if (!criteria.DrawingNo.IsNullOrEmpty())
                q.Where(p => p.DrawingNo.Contains(criteria.DrawingNo));

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据机台获取最新一条开机记录
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="checkerFixtureType"></param>
        /// <returns></returns>
        public virtual PreStartupSetupRecord GetLastPreStartupSetupRecord(double resourceId, CheckerFixtureType checkerFixtureType)
        {
            var record = Query<PreStartupSetupRecord>().Where(p => p.DispatchTask.ResourceId == resourceId && p.CheckerFixtureType == checkerFixtureType).OrderByDescending(p => p.CreateDate).FirstOrDefault();
            return record;

        }

        /// <summary>
        /// 校验任务单开机准备
        /// </summary>
        /// <param name="task"></param>
        public virtual void ValidateStartupSetupPrepare(DispatchTask task)
        {
            if (task == null) return;

            //任务单报工校验开机准备时，当机台报工产品和上一个任务报工产品相同，则不需要进行开机准备；相同产品多个任务单连续进行报工时只需要做一次开机准备：报工第二个任务单时，在开机准备记录查询中查看此机台最新的开机准备产品是否与本任务单一致，若一致，则不进行开机准备也能报工
            var lastRecord = Query<PreStartupSetupRecord>().Where(p => p.DispatchTask.ResourceId == task.ResourceId).OrderByDescending(p => p.CreateDate).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (lastRecord != null && lastRecord.ItemCode == task.ProductCode)
                return;

            var config = ConfigService.GetConfig(new Configs.PreStartupSetupRecordConfig(), typeof(PreStartupSetupRecord));
            //if (config.IsValidateStartupSetupPrepare != true)
            //    return;
            if (config.IsValidCheckerItem != true && config.IsValidEquipAccountItem != true && config.IsValidFixtureItem != true)
                return;
            var records = Query<PreStartupSetupRecord>().Where(p => p.DispatchTaskId == task.Id).ToList();
            var list = new List<string>();
            if (config.IsValidFixtureItem == true)
            {
                //工装
                var fixtureItems = Query<FixtureItem>().Where(p => p.ItemId == task.ProductId && p.ProcessId == task.ProcessId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (fixtureItems.Count > 0)
                {
                    var draws = fixtureItems.Where(p => !p.Drawn.IsNullOrEmpty()).Select(p => p.Drawn).Distinct().ToList();
                    foreach (var draw in draws)
                    {
                        if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Fixture && p.DrawingNo == draw))
                        {
                            list.Add(CheckerFixtureType.Fixture.ToLabel());
                            break;
                        }
                    }
                    //当没有图号的需要校验所有的
                    foreach (var item in fixtureItems.Where(p => p.Drawn.IsNullOrEmpty()))
                    {
                        if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Fixture && p.ToolCode == item.FixtureCode))
                        {
                            list.Add(CheckerFixtureType.Fixture.ToLabel());
                            break;
                        }
                    }
                }
            }
            //foreach (var item in fixtureItems)
            //{
            //    if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Fixture && p.ToolCode == item.FixtureCode))
            //    {
            //        list.Add(CheckerFixtureType.Fixture.ToLabel());
            //        break;
            //    }
            //}
            //检具
            if (config.IsValidCheckerItem == true)
            {
                var checkerItems = Query<CheckerItem>().Where(p => p.ItemId == task.ProductId && p.ProcessId == task.ProcessId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (checkerItems.Count > 0)
                {
                    var drawingNos = checkerItems.Where(p => !p.DrawingNo.IsNullOrEmpty()).Select(p => p.DrawingNo).Distinct().ToList();
                    //有图号的时候，每个图号必须要有一个开机记录，且只要一个就行
                    foreach (var drawingNo in drawingNos)
                    {
                        if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Checker && p.DrawingNo == drawingNo))
                        {
                            list.Add(CheckerFixtureType.Checker.ToLabel());
                            break;
                        }
                    }
                    //当没有图号的需要校验所有的
                    foreach (var item in checkerItems.Where(p => p.DrawingNo.IsNullOrEmpty()))
                    {
                        if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Checker && p.ToolCode == item.CheckerCode))
                        {
                            list.Add(CheckerFixtureType.Checker.ToLabel());
                            break;
                        }
                    }
                }
            }
            //foreach (var item in checkerItems)
            //{
            //    if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Checker && p.ToolCode == item.CheckerCode))
            //    {
            //        list.Add(CheckerFixtureType.Checker.ToLabel());
            //        break;
            //    }
            //}
            //模具
            if (config.IsValidEquipAccountItem == true)
            {
                var equipItems = Query<EquipAccountItem>().Where(p => p.ItemId == task.ProductId && p.ProcessId == task.ProcessId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (equipItems.Count > 0)
                {
                    var draws = equipItems.Where(p => !p.UniqueCode.IsNullOrEmpty()).Select(p => p.UniqueCode).Distinct().ToList();
                    //有唯一码的时候，每个唯一码必须要有一个开机记录且只要一个就行
                    foreach (var draw in draws)
                    {
                        if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Mold && p.UniqueCode == draw))
                        {
                            list.Add(CheckerFixtureType.Mold.ToLabel());
                            break;
                        }
                    }
                    //当没有唯一码的需要校验所有
                    foreach (var item in equipItems.Where(p => p.UniqueCode.IsNullOrEmpty()))
                    {
                        if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Mold && p.ToolCode == item.EquipAccountCode))
                        {
                            list.Add(CheckerFixtureType.Mold.ToLabel());
                            break;
                        }
                    }
                }
            }
            //foreach (var item in equipItems)
            //{
            //    if (!records.Any(p => p.CheckerFixtureType == CheckerFixtureType.Mold && p.ToolCode == item.EquipAccountCode))
            //    {
            //        list.Add(CheckerFixtureType.Mold.ToLabel());
            //    }
            //}
            if (list.Count > 0)
                throw new ValidationException("任务单[{0}]开机准备[{1}]未完成,请检查".L10nFormat(task?.No, list.Concat("、")));
        }
    }
}
