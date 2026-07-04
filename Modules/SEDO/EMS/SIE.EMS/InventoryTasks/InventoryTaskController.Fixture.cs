using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Crypto;
using SIE.Common;
using SIE.Common.Import;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点-工治具
    /// </summary>
    public partial class InventoryTaskController : DomainController
    {
        /// <summary>
        /// 保存工治具任务列表
        /// </summary>
        /// <param name="taskList"></param>
        private void SaveFixtureTaskList(EntityList<InventoryTask> taskList)
        {
            var taskIds = taskList.Select(p => p.Id).ToList();

            //取出当次保存的明细 先保存 再捞起统计明细数据

            var fixtureEncodeList = taskList.SelectMany(m => m.InventoryTaskFixtureEncodeList).ToList();
            var fixtureEncodeIDList = taskList.SelectMany(m => m.InventoryTaskFixtureIdAccountList).ToList();
            var now = RF.Find<InventoryTask>().GetDbTime();

            var fixtureEncodeChangeList = new EntityList<InventoryTaskFixtureEncode>();
            var fixtureEncodeIDChangeList = new EntityList<InventoryTaskFixtureIdAccount>();


            fixtureEncodeIDList.ForEach(item =>
            {
                CheckFixtureEncodeIDList(item);
                fixtureEncodeIDChangeList.Add(item);
            });
            var sapartIds = fixtureEncodeIDChangeList.Select(p => p.FixtureEncodeId).ToList();
            fixtureEncodeList.ForEach(item =>
            {
                if (sapartIds.Contains(item.FixtureEncodeId))
                {
                    fixtureEncodeChangeList.Add(item);
                }
            });

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(taskList);

                var newTaskList = Query<InventoryTask>().Where(m => taskIds.Contains(m.Id)).ToList();
                //重新捞起数据
                var inventoryTaskFixtureEncodeNewList = Query<InventoryTaskFixtureEncode>().Where(m => taskIds.Contains(m.InventoryTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var inventoryTaskFixtureEncodeIDNewList = Query<InventoryTaskFixtureIdAccount>().Where(m => taskIds.Contains(m.InventoryTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                foreach (var task in newTaskList)
                {
                    var newFixtureEncodeRecord = inventoryTaskFixtureEncodeNewList.Where(m => m.InventoryTaskId == task.Id).AsEntityList();
                    var newFixtureEncodeIDRecord = inventoryTaskFixtureEncodeIDNewList.Where(m => m.InventoryTaskId == task.Id).AsEntityList();
                    //ID编码序列号
                    foreach (var re in newFixtureEncodeIDRecord)//ID明细
                    {
                        FixtureIdCodeInv(now, fixtureEncodeIDChangeList, task, re);
                    }
                    foreach (var re in newFixtureEncodeRecord)//编码明细
                    {
                        //初盘
                        if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                        {
                            FixtureCodeFirstInv(re, now, fixtureEncodeChangeList, newFixtureEncodeIDRecord);

                        }
                        //初盘完成或复盘中
                        if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
                        {
                            FixtureCodeSecondInv(re, task, now, fixtureEncodeChangeList, newFixtureEncodeIDRecord);
                        }
                    }
                }

                RF.Save(newTaskList);
                RF.Save(inventoryTaskFixtureEncodeNewList);
                RF.Save(inventoryTaskFixtureEncodeIDNewList);

                UpdateFixturePercentage(newTaskList);
                trans.Complete();
            }
        }

        /// <summary>
        ///  保存PDA端的工治具数据
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="fixtureEncodes"></param>
        /// <param name="taskFixtureIdAccounts"></param>
        public virtual void SaveFixtureTaskListPda(double taskId, EntityList<InventoryTaskFixtureEncode> fixtureEncodes, EntityList<InventoryTaskFixtureIdAccount> taskFixtureIdAccounts)
        {

            var now = RF.Find<InventoryTaskFixtureEncode>().GetDbTime();
            var task = RF.GetById<InventoryTask>(taskId);

            // 当前提交的与数据库内的序列号管控数据
            var exceptIds = taskFixtureIdAccounts.Select(p => p.Id).ToList();
            var dbFixtureEncodeIDRecord = Query<InventoryTaskFixtureIdAccount>().Where(m => taskId == m.InventoryTaskId && !exceptIds.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //ID编码序列号
                foreach (var re in taskFixtureIdAccounts)//ID明细
                {
                    FixtureIdCodeInv(now, taskFixtureIdAccounts, task, re);
                }
                foreach (var re in fixtureEncodes)//编码明细
                {
                    //初盘
                    if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                    {
                        FixtureCodeFirstInv(re, now, fixtureEncodes, dbFixtureEncodeIDRecord, taskFixtureIdAccounts, true);

                    }
                    //初盘完成或复盘中
                    if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
                    {
                        FixtureCodeSecondInv(re, task, now, fixtureEncodes, dbFixtureEncodeIDRecord, taskFixtureIdAccounts, true);
                    }
                }
                RF.Save(task);
                RF.Save(fixtureEncodes);
                RF.Save(taskFixtureIdAccounts);

                UpdateFixturePercentage(new EntityList<InventoryTask>() { task });
                trans.Complete();
            }

        }

        /// <summary>
        /// 工治具ID类编码盘点
        /// </summary>
        /// <param name="now"></param>
        /// <param name="fixtureEncodeIDChangeList"></param>
        /// <param name="task"></param>
        /// <param name="re"></param>
        private void FixtureIdCodeInv(DateTime now, EntityList<InventoryTaskFixtureIdAccount> fixtureEncodeIDChangeList, InventoryTask task, InventoryTaskFixtureIdAccount re)
        {
            if (fixtureEncodeIDChangeList.FirstOrDefault(m => m.Id == re.Id) != null)//变更集存在该数据则变更
            {
                if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                {
                    re.FirstCounterId = RT.IdentityId;
                    re.FirstDateTime = now;
                    re.InventoryStatus = re.FirstResult.HasValue ? InventoryStatus.Done : InventoryStatus.Not;
                }
                if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
                {
                    re.SecondCounterId = RT.IdentityId;
                    re.SecondDateTime = now;
                }
            }
        }

        /// <summary>
        /// 编码明细工治具复盘
        /// </summary>
        /// <param name="re"></param>
        /// <param name="task"></param>
        /// <param name="now"></param>
        /// <param name="fixtureEncodeChangeList"></param>
        /// <param name="dbFixtureEncodeIDRecord"></param>
        /// <param name="newFixtureEncodeIDRecord">前端提交</param>
        /// <param name="isPda">是否PDA</param>
        private static void FixtureCodeSecondInv(InventoryTaskFixtureEncode re, InventoryTask task, DateTime now, EntityList<InventoryTaskFixtureEncode> fixtureEncodeChangeList, EntityList<InventoryTaskFixtureIdAccount> dbFixtureEncodeIDRecord, EntityList<InventoryTaskFixtureIdAccount> newFixtureEncodeIDRecord = null, bool isPda = false)
        {
            if (newFixtureEncodeIDRecord == null)
            {
                newFixtureEncodeIDRecord = new EntityList<InventoryTaskFixtureIdAccount>();
            }

            var dbIdList = dbFixtureEncodeIDRecord.Where(m => m.FixtureEncodeId == re.FixtureEncodeId).ToList();
            var newIdList = newFixtureEncodeIDRecord.Where(m => m.FixtureEncodeId == re.FixtureEncodeId).ToList();
            if (re.ManageMode == Fixtures.ManageMode.Number)//ID类管控需获取序列号列表后统计
            {
                int secondStock = 0;
                int secondOnline = 0;
                re.SecondOnline = re.SecondOnline == null ? 0 : re.SecondOnline;
                re.SecondStock = re.SecondStock == null ? 0 : re.SecondStock;
                if (isPda)
                {
                    secondStock = newIdList.Count(m => m.SecondStatus == FixtureStatus.InStorage && m.SecondResult.HasValue);
                    secondOnline = newIdList.Count(m => m.SecondStatus == FixtureStatus.OnLine && m.SecondResult.HasValue);
                    var allRecordList = new List<InventoryTaskFixtureIdAccount>();
                    allRecordList.AddRange(dbIdList);
                    allRecordList.AddRange(newIdList);
                    re.SecondOnline += secondOnline;
                    re.SecondStock += secondStock;
                }
                else
                {
                    secondStock = dbIdList.Count(m => m.SecondStatus == FixtureStatus.InStorage && m.SecondResult.HasValue);
                    secondOnline = dbIdList.Count(m => m.SecondStatus == FixtureStatus.OnLine && m.SecondResult.HasValue);
                    re.SecondOnline = secondOnline;
                    re.SecondStock = secondStock;
                }
                
                re.SecondTotal = re.SecondOnline + re.SecondStock;
                re.SecondDiff = re.SecondTotal - re.Total;
                re.SecondCounterId = RT.IdentityId;
                re.SecondDateTime = now;
                if (re.Total > re.SecondTotal)
                {
                    re.SecondResult = InventoryResult.Loss;
                }
                if (re.Total < re.SecondTotal)
                {
                    re.SecondResult = InventoryResult.Profit;
                }
                if (re.Total == re.SecondTotal)
                {
                    re.SecondResult = re.SecondStock == re.StockQty && re.SecondOnline == re.Online ? InventoryResult.Normal : InventoryResult.InfoChange;
                }
                task.InventoryTaskStatus = InventoryTaskStatus.ScondDoing;
            }
            if (re.ManageMode == Fixtures.ManageMode.Code && fixtureEncodeChangeList.FirstOrDefault(m => m.Id == re.Id) != null)
            {
                //re.SecondTotal = re.SecondOnline + re.SecondStock; js中已计算
                //re.SecondDiff = re.SecondTotal - re.Total;
                re.SecondCounterId = RT.IdentityId;
                re.SecondDateTime = now;
                re.InventoryStatus = InventoryStatus.Done;
                if (re.Total > re.SecondTotal)
                {
                    re.SecondResult = InventoryResult.Loss;
                }
                if (re.Total < re.SecondTotal)
                {
                    re.SecondResult = InventoryResult.Profit;
                }
                if (re.Total == re.SecondTotal)
                {
                    re.SecondResult = re.StockQty == re.SecondStock && re.SecondOnline == re.Online ? InventoryResult.Normal : InventoryResult.InfoChange;
                }
                task.InventoryTaskStatus = InventoryTaskStatus.ScondDoing;
            }
        }

        /// <summary>
        /// 编码明细工治具初盘
        /// </summary>
        /// <param name="re"></param>
        /// <param name="now"></param>
        /// <param name="fixtureEncodeChangeList"></param>
        /// <param name="dbFixtureEncodeIDRecord"></param>
        /// <param name="newFixtureEncodeIDRecord">前端提交</param>
        /// <param name="isPda">是否PDA</param>
        private void FixtureCodeFirstInv(InventoryTaskFixtureEncode re, DateTime now, EntityList<InventoryTaskFixtureEncode> fixtureEncodeChangeList, EntityList<InventoryTaskFixtureIdAccount> dbFixtureEncodeIDRecord, EntityList<InventoryTaskFixtureIdAccount> newFixtureEncodeIDRecord = null, bool isPda = false)
        {
            if (newFixtureEncodeIDRecord == null)
            {
                newFixtureEncodeIDRecord = new EntityList<InventoryTaskFixtureIdAccount>();
            }

            var dbIdList = dbFixtureEncodeIDRecord.Where(p => p.FixtureEncodeId == re.FixtureEncodeId).ToList();
            var newIdList = newFixtureEncodeIDRecord.Where(p => p.FixtureEncodeId == re.FixtureEncodeId).ToList();
            if (re.ManageMode == Fixtures.ManageMode.Number)//ID类管控需获取序列号列表后统计
            {
                //判断是否PDA提交
                UpdateCodeRecordByIdRecord(now, re, dbIdList, newIdList, isPda);
            }
            if (re.ManageMode == Fixtures.ManageMode.Code && fixtureEncodeChangeList.FirstOrDefault(m => m.FixtureEncodeId == re.FixtureEncodeId) != null)//已变更过的编码数据
            {
                //编码管控的保存【初盘XX】时更新为已盘点
                re.FirstCounterId = RT.IdentityId;
                re.InventoryDateTime = now;
                re.InventoryStatus = InventoryStatus.Done;
                if (re.Total > re.FirstTotal)
                {
                    re.FirstResult = InventoryResult.Loss;
                }
                if (re.Total < re.FirstTotal)
                {
                    re.FirstResult = InventoryResult.Profit;
                }
                if (re.Total == re.FirstTotal)
                {
                    re.FirstResult = re.FirstStock == re.StockQty && re.FirstOnline == re.Online ? InventoryResult.Normal : InventoryResult.InfoChange;
                }
            }
        }

        /// <summary>
        ///根据初盘ID纪录更新初盘编码记录 
        /// </summary>
        /// <param name="now"></param>
        /// <param name="re"></param>
        /// <param name="dbFixtureEncodeIDRecord"></param>
        /// <param name="newFixtureEncodeIDRecord">前端新增</param>
        /// <param name="isPda"></param>
        private void UpdateCodeRecordByIdRecord(DateTime now, InventoryTaskFixtureEncode re, List<InventoryTaskFixtureIdAccount> dbFixtureEncodeIDRecord, List<InventoryTaskFixtureIdAccount> newFixtureEncodeIDRecord = null, bool isPda = false)
        {
            int firstStock = 0;
            int firstOnline = 0;
            InventoryStatus state = InventoryStatus.Not;
            re.FirstOnline = re.FirstOnline == null ? 0 : re.FirstOnline;
            re.FirstStock = re.FirstStock == null ? 0 : re.FirstStock;
            if (isPda) // 前端提交
            {
                firstStock = newFixtureEncodeIDRecord.Count(m => m.FirstStatus == FixtureStatus.InStorage && m.FirstResult.HasValue);
                firstOnline = newFixtureEncodeIDRecord.Count(m => m.FirstStatus == FixtureStatus.OnLine && m.FirstResult.HasValue);
                var allRecordList = new List<InventoryTaskFixtureIdAccount>();
                allRecordList.AddRange(dbFixtureEncodeIDRecord);
                allRecordList.AddRange(newFixtureEncodeIDRecord);
                state = allRecordList.All(m => m.InventoryStatus == InventoryStatus.Done) ? InventoryStatus.Done : InventoryStatus.Part;
                re.FirstOnline += firstOnline;
                re.FirstStock += firstStock;
            }
            else
            {
                firstStock =  dbFixtureEncodeIDRecord.Count(m => m.FirstStatus == FixtureStatus.InStorage && m.FirstResult.HasValue);
                firstOnline = dbFixtureEncodeIDRecord.Count(m => m.FirstStatus == FixtureStatus.OnLine && m.FirstResult.HasValue);
                state = dbFixtureEncodeIDRecord.All(m => m.InventoryStatus == InventoryStatus.Done) ? InventoryStatus.Done : InventoryStatus.Part;
                re.FirstOnline = firstOnline;
                re.FirstStock = firstStock;
            }
            
            re.FirstTotal = re.FirstStock + re.FirstOnline;
            re.FirstDiff = re.FirstTotal - re.Total;
            re.InventoryStatus = state;
            re.FirstCounterId = RT.IdentityId;
            re.InventoryDateTime = now;
            //保存【初盘XX】时根据数量和当前值对比取值
            //正常（在库合格数，在库不合格数，在线数三个相等）、盘盈（只看总数）、盘亏（只看总数），
            //信息变动（在库合格数，在库不合格数，在线数 任意一个不等
            if (re.Total > re.FirstTotal)
            {
                re.FirstResult = InventoryResult.Loss;
            }
            if (re.Total < re.FirstTotal)
            {
                re.FirstResult = InventoryResult.Profit;
            }
            if (re.Total == re.FirstTotal)
            {
                re.FirstResult = re.FirstStock == re.StockQty && re.FirstOnline == re.Online ? InventoryResult.Normal : InventoryResult.InfoChange;
            }
        }



        /// <summary>
        /// 校验序列号变更数据
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="ValidationException"></exception>
        private void CheckFixtureEncodeIDList(InventoryTaskFixtureIdAccount item)
        {

            if (item.SecondResult == InventoryResult.InfoChange)
            {
                if (!item.SecondStatus.HasValue)
                {
                    throw new ValidationException("复盘结果为信息变动,【复盘状态】必输".L10N());
                }
            }
            if (item.FirstResult == InventoryResult.InfoChange)
            {
                if (!item.FirstStatus.HasValue)
                {
                    throw new ValidationException("初盘结果为信息变动,【初盘状态】必输".L10N());
                }
            }

        }

        /// <summary>
        /// 更新工治具盘点进度
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="inventoryTaskFixtureEncodeList">外部传入 如不传入会从数据库重新取数</param>
        /// <param name="inventoryTaskFixtureEncodeIDList">外部传入 如不传入会从数据库重新取数</param>
        private void UpdateFixturePercentage(EntityList<InventoryTask> taskList,
            EntityList<InventoryTaskFixtureEncode> inventoryTaskFixtureEncodeList = null,
            EntityList<InventoryTaskFixtureIdAccount> inventoryTaskFixtureEncodeIDList = null)

        {
            var planIds = taskList.Select(p => p.InventoryPlanId).Distinct().ToList();
            var plans = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlansByIds(planIds);
            var taskIds = taskList.Select(p => p.Id).ToList();
            if (inventoryTaskFixtureEncodeList == null)
            {
                inventoryTaskFixtureEncodeList = Query<InventoryTaskFixtureEncode>().Where(m => taskIds.Contains(m.InventoryTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
            if (inventoryTaskFixtureEncodeIDList == null)
            {
                inventoryTaskFixtureEncodeIDList = Query<InventoryTaskFixtureIdAccount>().Where(m => taskIds.Contains(m.InventoryTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
            foreach (var task in taskList)
            {
                //这个盘点单下所有来源为【账内资产】的数据中，初盘结果有值的数量除以所有来源为【账内资产】的数量，保留2位小数
                decimal allFixtureEncodeQty = inventoryTaskFixtureEncodeList.Count(p => p.InventoryTaskId == task.Id && p.InventoryAssetSource == InventoryAssetSource.Account);
                decimal allFixtureEncodeIdQty = inventoryTaskFixtureEncodeIDList.Count(p => p.InventoryTaskId == task.Id && p.InventoryAssetSource == InventoryAssetSource.Account);

                decimal fixtureEncodeQty = inventoryTaskFixtureEncodeList.Count(p => p.InventoryTaskId == task.Id && p.InventoryAssetSource == InventoryAssetSource.Account && p.FirstResult.HasValue);
                decimal fixtureIdEncodeQty = inventoryTaskFixtureEncodeIDList.Count(p => p.InventoryTaskId == task.Id && p.InventoryAssetSource == InventoryAssetSource.Account && p.FirstResult.HasValue);

                var allQty = allFixtureEncodeQty + allFixtureEncodeIdQty;
                var qty = fixtureEncodeQty + fixtureIdEncodeQty;
                if (allQty > 0)
                {
                    var percentage = Math.Floor(qty / allQty * 100);
                    DB.Update<InventoryTask>().Set(p => p.Percentage, percentage).Where(p => p.Id == task.Id).Execute();
                }
            }
            foreach (var plan in plans)
            {
                //盘点计划关联的盘点单下所有来源为【账内资产】的数据中，初盘结果有值的数量除以所有来源为【账内资产】的数量，保留2位小数
                decimal allEncodeQty = inventoryTaskFixtureEncodeList.Count(p => p.InventoryPlanId == plan.Id && p.InventoryAssetSource == InventoryAssetSource.Account);
                decimal allEncodeIDqty = inventoryTaskFixtureEncodeIDList.Count(p => p.InventoryPlanId == plan.Id && p.InventoryAssetSource == InventoryAssetSource.Account);
                var allQty = allEncodeQty + allEncodeIDqty;

                decimal encodeQty = inventoryTaskFixtureEncodeList.Count(p => p.InventoryPlanId == plan.Id && p.InventoryAssetSource == InventoryAssetSource.Account && p.FirstResult.HasValue);
                decimal encodeIDqty = inventoryTaskFixtureEncodeIDList.Count(p => p.InventoryPlanId == plan.Id && p.InventoryAssetSource == InventoryAssetSource.Account && p.FirstResult.HasValue);
                var qty = encodeQty + encodeIDqty;

                if (allQty > 0)
                {
                    var percentage = Math.Floor(qty / allQty * 100);
                    DB.Update<InventoryPlan>().Set(p => p.Percentage, percentage).Where(p => p.Id == plan.Id).Execute();
                }
            }
        }
        /// <summary>
        /// 获取工治具盘点编码明细
        /// </summary>
        /// <param name="task"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="sortInfo"></param>
        /// <returns></returns>
        public virtual EntityList<InventoryTaskFixtureEncode> GetInventoryTaskFixtureEncodeList(InventoryTask task, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var seacherCode = task.FixtureCodeSnNotMap;
            var curId = RT.IdentityId;

            List<InventoryResult?> resultList = GetInventoryResultForSeach(task);

            var counter = Query<InventoryTaskFixtureCounter>().Where(p => p.InventoryTaskId == task.Id && p.EmployeeId == curId).FirstOrDefault();
            var encodeList = Query<InventoryTaskFixtureEncode>().Where(p => p.InventoryTaskId == task.Id)
                .WhereIf(!seacherCode.IsNullOrEmpty(), p => p.FixtureEncode.Code.Contains(seacherCode))
                .WhereIf(resultList.Any(), m => resultList.Contains(m.FirstResult) || resultList.Contains(m.SecondResult))
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            //用户在盘点人中有初盘/复盘权限才可编辑
            encodeList.ForEach(p =>
            {
                p.FirstPower = counter != null && counter.First;
                p.SecondPower = counter != null && counter.Second;

            });
            return encodeList;
        }

        /// <summary>
        /// 为查询获取盘点结果
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private List<InventoryResult?> GetInventoryResultForSeach(InventoryTask task)
        {
            var resultList = new List<InventoryResult?>();
            if (task.InventorySeachResult.HasValue)
            {
                switch (task.InventorySeachResult)
                {
                    case InventorySeachResult.ProfitOrLoss:
                        resultList.Add(InventoryResult.Profit);
                        resultList.Add(InventoryResult.Loss);
                        break;
                    case InventorySeachResult.Loss:
                        resultList.Add(InventoryResult.Loss);
                        break;
                    case InventorySeachResult.Profit:
                        resultList.Add(InventoryResult.Profit);
                        break;
                    case InventorySeachResult.Normal:
                        resultList.Add(InventoryResult.Normal);
                        break;
                    case InventorySeachResult.InfoChange:
                        resultList.Add(InventoryResult.InfoChange);
                        break;
                    default:
                        break;
                }
            }

            return resultList;
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="task"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="sortInfo"></param>
        /// <returns></returns>
        public virtual EntityList<InventoryTaskFixtureIdAccount> GetInventoryTaskFixtureIdAccountList(InventoryTask task, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {

            var curId = RT.IdentityId;
            var seacherCode = task.FixtureCodeSnNotMap;
            List<InventoryResult?> resultList = GetInventoryResultForSeach(task);

            var counter = Query<InventoryTaskFixtureCounter>().Where(p => p.InventoryTaskId == task.Id && p.EmployeeId == curId).FirstOrDefault();
            var encodeList = Query<InventoryTaskFixtureIdAccount>().Where(p => p.InventoryTaskId == task.Id)
                 .WhereIf(resultList.Any(), m => resultList.Contains(m.FirstResult) || resultList.Contains(m.SecondResult))
                .WhereIf(!seacherCode.IsNullOrEmpty(), p => p.Sn.Contains(seacherCode))
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            encodeList.ForEach(p =>
            {
                p.FirstPower = counter != null && counter.First;
                p.SecondPower = counter != null && counter.Second;

            });
            return encodeList;
        }

        #region 工治具新增盘盈


        /// <summary>
        /// 新增盘盈
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void AddFixtureProfit(AddFixtureProfitViewModel model)
        {
            var task = RF.GetById<InventoryTask>(model.InventoryTaskId);
            if (task == null)
            {
                throw new ValidationException("盘点任务信息丢失".L10N());
            }
            var now = RF.Find<InventoryTaskFixtureEncode>().GetDbTime();
            var encodeList = Query<InventoryTaskFixtureEncode>().Where(p => p.InventoryTaskId == task.Id).ToList();
            if (model.ManageMode == Fixtures.ManageMode.Number)//序列号管控
            {
                const int qty = 1;
                if ((int)model.FixtureStatus == 0)
                {
                    throw new ValidationException("库存状态必填".L10N());
                }
                if (model.GenerateSn)
                {
                    model.Sn = RT.Service.Resolve<CommonController>().GetNo<FixtureIDAccount>("工治具ID");
                }
                var exsitedSn = Query<FixtureAccount>().Where(m => m.Code == model.Sn).FirstOrDefault();
                if (exsitedSn != null)
                {
                    throw new ValidationException("序列号{0}已存在".L10nFormat(exsitedSn.Code));
                }
                //创建新的序列号数据
                FixtureProfitSN(model, task, now, encodeList, qty);
            }

            if (model.ManageMode == Fixtures.ManageMode.Code)//序列号管控
            {
                FixtureProfitCode(model, task, now, encodeList);

            }
        }


        /// <summary>
        /// 工治具盘盈-编码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="task"></param>
        /// <param name="now"></param>
        /// <param name="encodeList"></param>
        /// <exception cref="ValidationException"></exception>
        private void FixtureProfitCode(AddFixtureProfitViewModel model, InventoryTask task, DateTime now, EntityList<InventoryTaskFixtureEncode> encodeList)
        {
            if (model.Online == 0 && model.StockQty == 0)
            {
                throw new ValidationException("在线数、在库数,至少一个不为0".L10N());
            }
            if (encodeList.Any(m => m.FixtureEncodeId == model.FixtureEncodeId))
            {
                throw new ValidationException("工治具编码{0}已存在编码明细中，不能新增".L10nFormat(model.FixtureEncode.Code));
            }
            var encode = new InventoryTaskFixtureEncode();
            encode.InventoryTaskId = task.Id;
            encode.InventoryAssetSource = InventoryAssetSource.Profit;
            encode.InventoryStatus = InventoryStatus.Done;
            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
            {
                encode.FirstOnline += model.Online;
                encode.FirstStock += model.StockQty;
                encode.FirstTotal = encode.FirstOnline + encode.FirstStock;
                encode.FirstDiff = encode.FirstTotal - encode.Total;
                encode.FirstResult = InventoryResult.Profit;
                encode.InventoryDateTime = now;
                encode.FirstCounterId = RT.IdentityId;
            }
            if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
            {
                encode.SecondOnline += model.Online;
                encode.SecondStock += model.StockQty;
                encode.SecondTotal = encode.SecondOnline + encode.SecondStock;
                encode.SecondDiff = encode.SecondTotal - encode.Total;
                encode.SecondResult = InventoryResult.Profit;
                encode.SecondDateTime = now;
                encode.SecondCounterId = RT.IdentityId;
            }
            RF.Save(encode);
        }

        /// <summary>
        /// 工治具盘盈-序列号
        /// </summary>
        /// <param name="model"></param>
        /// <param name="task"></param>
        /// <param name="now"></param>
        /// <param name="encodeList"></param>
        /// <param name="qty"></param>
        private static void FixtureProfitSN(AddFixtureProfitViewModel model, InventoryTask task, DateTime now, EntityList<InventoryTaskFixtureEncode> encodeList, int qty)
        {
            InventoryTaskFixtureEncode encode = null;
            var inventoryTaskFixtureIdAccount = new InventoryTaskFixtureIdAccount();
            inventoryTaskFixtureIdAccount.InventoryTaskId = task.Id;
            inventoryTaskFixtureIdAccount.FixtureEncodeId = model.FixtureEncodeId;
            inventoryTaskFixtureIdAccount.InventoryAssetSource = InventoryAssetSource.Profit;
            inventoryTaskFixtureIdAccount.Sn = model.Sn;
            inventoryTaskFixtureIdAccount.InventoryStatus = InventoryStatus.Done;

            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
            {
                inventoryTaskFixtureIdAccount.FirstStatus = model.FixtureStatus;
                inventoryTaskFixtureIdAccount.FirstDateTime = now;
                inventoryTaskFixtureIdAccount.FirstCounterId = RT.IdentityId;
                inventoryTaskFixtureIdAccount.FirstResult = InventoryResult.Profit;
            }
            if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
            {
                inventoryTaskFixtureIdAccount.SecondStatus = model.FixtureStatus;
                inventoryTaskFixtureIdAccount.SecondDateTime = now;
                inventoryTaskFixtureIdAccount.SecondCounterId = RT.IdentityId;
                inventoryTaskFixtureIdAccount.SecondResult = InventoryResult.Profit;
            }

            encode = encodeList.FirstOrDefault(m => m.FixtureEncodeId == model.FixtureEncodeId);
            if (encode != null)
            {
                if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                {
                    encode.FirstOnline += model.FixtureStatus == FixtureStatus.OnLine ? qty : 0;
                    encode.FirstStock += (model.FixtureStatus == FixtureStatus.InStorage) ? qty : 0;
                    encode.FirstTotal = encode.FirstOnline + encode.FirstStock;
                    encode.FirstDiff = encode.FirstTotal - encode.Total;

                }
                if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
                {

                    encode.SecondOnline += model.FixtureStatus == FixtureStatus.OnLine ? qty : 0;
                    encode.SecondStock += (model.FixtureStatus == FixtureStatus.InStorage) ? qty : 0;
                    encode.SecondTotal = encode.SecondOnline + encode.SecondStock;
                    encode.SecondDiff = encode.SecondTotal - encode.Total;
                }
            }
            else
            {
                encode = new InventoryTaskFixtureEncode();
                encode.InventoryTaskId = task.Id;
                encode.InventoryAssetSource = InventoryAssetSource.Profit;
                if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                {
                    encode.FirstOnline += model.FixtureStatus == FixtureStatus.OnLine ? qty : 0;
                    encode.FirstStock += model.FixtureStatus == FixtureStatus.InStorage ? qty : 0;
                    encode.FirstTotal = encode.FirstOnline + encode.FirstStock;
                    encode.FirstDiff = encode.FirstTotal - encode.Total;
                    encode.InventoryDateTime = now;
                }
                if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
                {
                    encode.SecondOnline += model.FixtureStatus == FixtureStatus.OnLine ? qty : 0;
                    encode.SecondStock += model.FixtureStatus == FixtureStatus.InStorage ? qty : 0;
                    encode.SecondTotal = encode.SecondOnline + encode.SecondStock;
                    encode.SecondDiff = encode.SecondTotal - encode.Total;
                    encode.SecondDateTime = now;
                }
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(inventoryTaskFixtureIdAccount);
                RF.Save(encode);
                trans.Complete();
            }
        }

        #endregion

        /// <summary>
        /// 一键盘点
        /// </summary>
        /// <param name="taskId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OneKeyPass(double taskId)
        {
            var inventoryEncodeList = Query<InventoryTaskFixtureEncode>().Where(m => m.InventoryTaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var inventoryIDList = Query<InventoryTaskFixtureIdAccount>().Where(m => m.InventoryTaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var now = RF.Find<InventoryTask>().GetDbTime();
            inventoryIDList.ForEach(m =>
            {
                if (m.InventoryStatus != InventoryStatus.Done)
                {
                    m.FirstDateTime = DateTime.Now;
                    m.FirstCounterId = RT.IdentityId;
                    m.InventoryStatus = InventoryStatus.Done;
                    m.FirstResult = InventoryResult.Normal;
                    m.FirstStatus = m.FixtureStatus;
                }
            });
            inventoryEncodeList.ForEach(m =>
            {
                if (m.ManageMode == Fixtures.ManageMode.Number)
                {
                    var IdList = inventoryIDList.Where(k => k.FixtureEncodeId == m.FixtureEncodeId).ToList();
                    UpdateCodeRecordByIdRecord(now, m, IdList);
                }
            });

            if (inventoryIDList.Any())
            {
                using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(inventoryIDList);
                    RF.Save(inventoryEncodeList);
                    var task = RF.GetById<InventoryTask>(taskId);
                    UpdateFixturePercentage(new EntityList<InventoryTask>() { task }, inventoryEncodeList, inventoryIDList);
                    trans.Complete();
                }
            }


        }


        #region  导入
        /// <summary>
        /// 导入工治具任务
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual IEnumerable<ImportMessageResult> ImportTaskFixture(IList<RowData> batch)
        {
            List<ImportMessageResult> messageList = new List<ImportMessageResult>();
            var taskFixtureIds = batch.Select(p => p.Entity as InventoryTaskFixtureIdAccount).ToList();
            IEnumerable<ImportFixtureModel> importModelList = null;
            List<double> taskIds = GetImportData(batch, taskFixtureIds, ref importModelList);
            if (importModelList == null)
            {
                throw new ValidationException("转换数据失败".L10N());
            }
            var encodeIds = RT.Service.Resolve<InventoryPlanController>().GetTaskFixtureEncodeId(taskIds.FirstOrDefault());
            var allEncodeList = Query<FixtureEncode>().Where(p => encodeIds.Contains(p.Id)).ToList();
            var inventoryTaskFixtureEncodeList = Query<InventoryTaskFixtureEncode>().Where(m => taskIds.Contains(m.InventoryTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var inventoryTaskFixtureEncodeIDList = Query<InventoryTaskFixtureIdAccount>().Where(m => taskIds.Contains(m.InventoryTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var now = RF.Find<FixtureEncode>().GetDbTime();
            const InventoryTask task = null;
            if (!CheckImportTask(taskIds, messageList, task))
            {
                return messageList;
            }
            foreach (var row in importModelList)
            {
                //校验是否符合盘点范围的【工治具类型、工治具型号、工治具编码、管控方式】不符合时报错
                if (row.FixtureEncode == null)
                {
                    messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "工治具编码必填".L10N() });
                    return messageList;
                }

                if (!allEncodeList.Any(m => m.Id == row.FixtureEncodeId))
                {
                    messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "工治具编码{0}不在盘点范围内".L10nFormat(row.FixtureEncode.Code) });
                    return messageList;
                }
                if (row.FixtureEncode.FixtureModel.ManageMode == Fixtures.ManageMode.Code) //编码类型
                {
                    var exsitedRecord = inventoryTaskFixtureEncodeList.FirstOrDefault(m => m.FixtureEncodeId == row.FixtureEncodeId);
                    if (exsitedRecord != null)//编码清单能获取到数据且编码管控的为账内资产，导入后修改获取的那条数据 
                    {
                        UpdateImportFixtureEncode(now, row, task, exsitedRecord);
                    }
                    else//否则为盘盈
                    {
                        if (row.OnlineQty == 0 && row.NgQty == 0 && row.PassQty == 0)
                        {
                            messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "新增盘盈的数据【实盘合格数、实盘不合格数、实盘在线数】不能全为0".L10N() });
                            return messageList;
                        }
                        GetInventoryAdd(inventoryTaskFixtureEncodeList, now, task, row);
                    }
                }
                else//ID类
                {
                    if (row.Sn.IsNullOrEmpty())
                    {
                        messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "工治具编码的管控方式为【ID管控】时序列号必输".L10N() });
                        return messageList;
                    }
                    //实盘合格数、实盘不合格数、实盘在线数 ID管控的数据校验只能为1或0，且实盘合格数和实盘不合格数只能一个为1，一个为0
                    if (row.OnlineQty > 1 && row.NgQty > 1 && row.PassQty > 1)
                    {
                        messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "工治具编码的管控方式为【ID管控】时,实盘合格数、实盘不合格数、实盘在线数只能为0或1".L10N() });
                        return messageList;
                    }
                    if (row.NgQty == 1 && row.PassQty == 1)
                    {
                        messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "工治具编码的管控方式为【ID管控】时,实盘合格数和实盘不合格数只能一个为1，一个为0".L10N() });
                        return messageList;
                    }
                    var exsitedRecord = inventoryTaskFixtureEncodeIDList.FirstOrDefault(m => m.Sn == row.Sn);
                    var fixtureQualityState = row.PassQty == 1 ? FixtureQualityState.Pass : FixtureQualityState.Ng;
                    var fixtureStatus = row.OnlineQty == 1 ? FixtureStatus.OnLine : FixtureStatus.InStorage;
                    if (exsitedRecord != null)//编码清单能获取到数据且ID管控的为账内资产，导入后修改获取的那条数据 
                    {
                        //序列号的通过实盘合格和实盘不合格，判定盘点质量状态是合格还是不合格；通过实盘在线数为1还是为0判断状态为在线还是在库
                        UpdateExsitedSn(now, task, exsitedRecord, fixtureQualityState, fixtureStatus);
                    }
                    else//盘盈
                    {
                        var exsitedSn = Query<FixtureIDAccount>().Where(p => p.Code == row.Sn).FirstOrDefault();
                        if (exsitedSn != null)
                        {
                            messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "工治具{0}已存在".L10nFormat(row.Sn) });
                            return messageList;
                        }
                        UpdateNewSnInfo(inventoryTaskFixtureEncodeIDList, now, task, row, fixtureQualityState, fixtureStatus);
                    }
                }
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(inventoryTaskFixtureEncodeIDList);
                RF.Save(inventoryTaskFixtureEncodeList);
                trans.Complete();
            }
            return messageList;
        }

        /// <summary>
        ///校验导入的盘点任务状态
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="messageList"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private bool CheckImportTask(List<double> taskIds, List<ImportMessageResult> messageList, InventoryTask task)
        {
            if (taskIds.Count != 1)
            {
                messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "一次只能导入一个盘点任务的数据".L10N() });
                return false;
            }
            task = GetById<InventoryTask>(taskIds.FirstOrDefault());
            if (task == null)
            {
                messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "盘点任务不存在".L10N() });
                return false;
            }
            //盘点状态为【盘点中、初盘完成、复盘中】才允许导入
            if (task.InventoryTaskStatus != InventoryTaskStatus.Doing && task.InventoryTaskStatus != InventoryTaskStatus.FirstDone
                && task.InventoryTaskStatus != InventoryTaskStatus.ScondDoing)
            {
                messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "盘点状态为【盘点中、初盘完成、复盘中】才允许导入".L10N() });
                return false;
            }
            return false;
        }

        /// <summary>
        /// 获取盘盈的工治具编码记录
        /// </summary>
        /// <param name="inventoryTaskFixtureEncodeList"></param>
        /// <param name="now"></param>
        /// <param name="task"></param>
        /// <param name="row"></param>
        private void GetInventoryAdd(EntityList<InventoryTaskFixtureEncode> inventoryTaskFixtureEncodeList, DateTime now, InventoryTask task, ImportFixtureModel row)
        {
            var newRecord = new InventoryTaskFixtureEncode();
            newRecord.FixtureEncodeId = row.FixtureEncodeId;
            newRecord.FixtureEncode = row.FixtureEncode;
            newRecord.InventoryTaskId = task.Id;
            newRecord.InventoryStatus = InventoryStatus.Done;
            newRecord.PersistenceStatus = PersistenceStatus.New;
            UpdateImportFixtureEncode(now, row, task, newRecord);
            inventoryTaskFixtureEncodeList.Add(newRecord);
        }

        /// <summary>
        /// 获取导入数据
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="taskFixtureIds"></param>
        /// <param name="importModelList"></param>
        /// <returns></returns>
        private List<double> GetImportData(IList<RowData> batch, List<InventoryTaskFixtureIdAccount> taskFixtureIds, ref IEnumerable<ImportFixtureModel> importModelList)
        {
            var taskIds = new List<double>();
            if (taskFixtureIds.Any())//不存在则可能不是该类型
            {
                taskFixtureIds.Select(p => p.InventoryTaskId).Distinct().ToList();
                importModelList = taskFixtureIds.Select(p => new ImportFixtureModel()
                {
                    FixtureEncodeId = p.FixtureEncodeId,
                    FixtureEncode = p.FixtureEncode,
                    NgQty = p.NgQty,
                    OnlineQty = p.OnlineQty,
                    PassQty = p.PassQty,
                    Sn = p.Sn

                });
            }
            else
            {
                var taskFixtures = batch.Select(p => p.Entity as InventoryTaskFixtureEncode).ToList();
                if (taskFixtures.Any())
                {
                    taskFixtures.Select(p => p.InventoryTaskId).Distinct().ToList();
                    importModelList = taskFixtures.Select(p => new ImportFixtureModel()
                    {
                        FixtureEncodeId = p.FixtureEncodeId,
                        FixtureEncode = p.FixtureEncode,
                        NgQty = p.NgQty,
                        OnlineQty = p.OnlineQty,
                        PassQty = p.PassQty,
                        Sn = p.Sn

                    });
                }
            }

            return taskIds;
        }

        /// <summary>
        /// 设置导入的新SN的数据
        /// </summary>
        /// <param name="inventoryTaskFixtureEncodeIDList"></param>
        /// <param name="now"></param>
        /// <param name="task"></param>
        /// <param name="row"></param>
        /// <param name="fixtureQualityState"></param>
        /// <param name="fixtureStatus"></param>
        private void UpdateNewSnInfo(EntityList<InventoryTaskFixtureIdAccount> inventoryTaskFixtureEncodeIDList, DateTime now, InventoryTask task, ImportFixtureModel row, FixtureQualityState fixtureQualityState, FixtureStatus fixtureStatus)
        {
            var newRecord = new InventoryTaskFixtureIdAccount();
            newRecord.InventoryTaskId = task.Id;
            newRecord.InventoryAssetSource = InventoryAssetSource.Profit;
            newRecord.FixtureEncodeId = row.FixtureEncodeId;
            newRecord.PersistenceStatus = PersistenceStatus.New;
            newRecord.InventoryStatus = InventoryStatus.Done;
            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)//初盘中 更新初盘数值
            {
                newRecord.FirstStatus = fixtureStatus;
                newRecord.FirstResult = InventoryResult.Profit;
                newRecord.FirstCounterId = RT.IdentityId;
                newRecord.FirstDateTime = now;
            }
            if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
            {
                newRecord.SecondStatus = fixtureStatus;
                newRecord.SecondResult = InventoryResult.Profit;
                newRecord.SecondCounterId = RT.IdentityId;
                newRecord.SecondDateTime = now;
            }
            inventoryTaskFixtureEncodeIDList.Add(newRecord);
        }

        /// <summary>
        /// 更新已存序列号的记录
        /// </summary>
        /// <param name="now"></param>
        /// <param name="task"></param>
        /// <param name="exsitedRecord"></param>
        /// <param name="fixtureQualityState"></param>
        /// <param name="fixtureStatus"></param>
        private void UpdateExsitedSn(DateTime now, InventoryTask task, InventoryTaskFixtureIdAccount exsitedRecord, FixtureQualityState fixtureQualityState, FixtureStatus fixtureStatus)
        {
            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)//初盘中 更新初盘数值
            {
                exsitedRecord.FirstStatus = fixtureStatus;
                var noChanged = exsitedRecord.FirstStatus == exsitedRecord.FixtureStatus;
                exsitedRecord.FirstResult = noChanged ? InventoryResult.Normal : InventoryResult.InfoChange;
                exsitedRecord.FirstCounterId = RT.IdentityId;
                exsitedRecord.FirstDateTime = now;
            }
            if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
            {
                exsitedRecord.SecondStatus = fixtureStatus;
                var noChanged = exsitedRecord.SecondStatus == exsitedRecord.FixtureStatus;
                exsitedRecord.SecondResult = noChanged ? InventoryResult.Normal : InventoryResult.InfoChange;

                exsitedRecord.SecondCounterId = RT.IdentityId;
                exsitedRecord.SecondDateTime = now;
            }
        }

        /// <summary>
        /// 更新导入的编码明细
        /// </summary>
        /// <param name="now"></param>
        /// <param name="row"></param>
        /// <param name="task"></param>
        /// <param name="exsitedRecord"></param>
        private void UpdateImportFixtureEncode(DateTime now, ImportFixtureModel row, InventoryTask task, InventoryTaskFixtureEncode exsitedRecord)
        {
            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)//初盘中 更新初盘数值
            {
                exsitedRecord.FirstStock = row.PassQty + row.NgQty;
                exsitedRecord.FirstOnline = row.OnlineQty;
                exsitedRecord.FirstTotal = exsitedRecord.FirstStock + exsitedRecord.FirstOnline;
                exsitedRecord.FirstDiff = exsitedRecord.FirstTotal - exsitedRecord.Total;
                exsitedRecord.FirstCounterId = RT.IdentityId;
                exsitedRecord.InventoryDateTime = now;
                if (exsitedRecord.Total > exsitedRecord.FirstTotal)
                {
                    exsitedRecord.FirstResult = InventoryResult.Loss;
                }
                if (exsitedRecord.Total < exsitedRecord.FirstTotal)
                {
                    exsitedRecord.FirstResult = InventoryResult.Profit;
                }
                if (exsitedRecord.Total == exsitedRecord.FirstTotal)
                {
                    exsitedRecord.FirstResult = exsitedRecord.FirstStock == exsitedRecord.StockQty && exsitedRecord.FirstOnline == exsitedRecord.Online ? InventoryResult.Normal : InventoryResult.InfoChange;
                }

            }
            if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone || task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
            {
                exsitedRecord.SecondStock = row.NgQty + row.PassQty;
                exsitedRecord.SecondOnline = row.OnlineQty;
                exsitedRecord.SecondTotal = exsitedRecord.SecondStock + exsitedRecord.SecondOnline;
                exsitedRecord.SecondDiff = exsitedRecord.SecondTotal - exsitedRecord.Total;
                exsitedRecord.SecondCounterId = RT.IdentityId;
                exsitedRecord.InventoryDateTime = now;
                if (exsitedRecord.Total > exsitedRecord.SecondTotal)
                {
                    exsitedRecord.SecondResult = InventoryResult.Loss;
                }
                if (exsitedRecord.Total < exsitedRecord.SecondTotal)
                {
                    exsitedRecord.SecondResult = InventoryResult.Profit;
                }
                if (exsitedRecord.Total == exsitedRecord.SecondTotal)
                {
                    exsitedRecord.SecondResult = exsitedRecord.SecondStock == exsitedRecord.StockQty && exsitedRecord.SecondOnline == exsitedRecord.Online ? InventoryResult.Normal : InventoryResult.InfoChange;
                }
            }
        }
        #endregion
    }
}
