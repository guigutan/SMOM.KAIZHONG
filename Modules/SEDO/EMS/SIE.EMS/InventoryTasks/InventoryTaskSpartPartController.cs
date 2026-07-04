using SIE.Common.Import;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 备件备件任务控制器
    /// </summary>
    public class InventoryTaskSpartPartController : DomainController
    {
        /// <summary>
        /// 查询盘点任务下的备件清单
        /// </summary>
        /// <param name="task">盘点任务</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns></returns>
        public virtual EntityList<InventoryTaskSparePartDetail> GetInventoryTaskSparePartDetails(InventoryTask task, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var dbInventoryTask = RF.GetById<InventoryTask>(task.Id);
            if (dbInventoryTask == null)
            {
                throw new ValidationException("找不到盘点任务的信息".L10N());
            }

            var list = Query<InventoryTaskSparePartDetail>()
                .Where(x => x.InventoryTaskId == task.Id)
                .WhereIf(!task.SparePartDetailKeyWord.IsNullOrEmpty(),
                    x => x.SparePart.SparePartCode.Contains(task.SparePartDetailKeyWord)
                    || x.LotNo.Contains(task.SparePartDetailKeyWord)
                    || x.Sn.Contains(task.SparePartDetailKeyWord))
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            //用户在盘点人中有初盘/复盘权限才可编辑
            if (dbInventoryTask.ResponsibleId == RT.IdentityId)
            {
                //当前用户是盘点责任人，则具有初盘和复盘权限
                list.ForEach(p =>
                {
                    p.FirstPower = true;
                    p.SecondPower = true;
                });
            }
            else
            {
                var counter = Query<InventoryTaskSparePartCounter>()
                    .Where(p => p.InventoryTaskId == task.Id && p.EmployeeId == RT.IdentityId)
                    .FirstOrDefault();

                if (counter != null)
                {
                    list.ForEach(p =>
                    {
                        p.FirstPower = counter.First;
                        p.SecondPower = counter.Second;
                    });
                }
                else
                {
                    list.ForEach(p =>
                    {
                        p.FirstPower = false;
                        p.SecondPower = false;
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// 查询盘点任务下的备件清单
        /// </summary>
        /// <param name="taskId">盘点任务Id</param>
        /// <returns></returns>
        public virtual EntityList<InventoryTaskSparePartDetail> GetInventoryTaskSparePartDetails(double taskId)
        {
            var sparePartDetails = Query<InventoryTaskSparePartDetail>()
                .Where(x => x.InventoryTaskId == taskId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            return sparePartDetails;
        }

        /// <summary>
        /// 查询盘点任务下的备件清单
        /// </summary>
        /// <param name="task">盘点任务</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns></returns>
        public virtual EntityList<InventoryTaskSparePartDetail> GetInventoryTaskSparePartDetailsForBalance(InventoryTask task, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var sparePartDetails = Query<InventoryTaskSparePartDetail>()
                .Where(x => x.InventoryTaskId == task.Id)
                .WhereIf(!task.SparePartDetailKeyWord.IsNullOrEmpty(),
                    x => x.SparePart.SparePartCode.Contains(task.SparePartDetailKeyWord)
                    || x.LotNo.Contains(task.SparePartDetailKeyWord)
                    || x.Sn.Contains(task.SparePartDetailKeyWord))
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return sparePartDetails;
        }

        /// <summary>
        /// 新增备件盘盈
        /// </summary>
        /// <param name="model">备件盘盈视图</param>        
        public virtual double AddProfit(AddSparePartProfitViewModel model)
        {
            var task = RF.GetById<InventoryTask>(model.InventoryTaskId);

            if (task == null)
            {
                throw new ValidationException("盘点任务信息丢失".L10N());
            }

            if (model.ControlMethod == ControlMethod.ItemCode || model.ControlMethod == ControlMethod.Batch)
            {
                if (model.GoodQty == 0 && model.NgQty == 0)
                {
                    throw new ValidationException("【编码管控】或【批次管控】的备件新增盘盈良品数量和不良品数量不能同时都为0".L10N());
                }

                if (model.ControlMethod == ControlMethod.Batch && !model.GenerateLotNo && model.LotNo.IsNullOrEmpty())
                {
                    throw new ValidationException("【批次管控】的备件新增盘盈时，未勾选【自动生成批次号】时，批次号必须输入".L10N());
                }
            }
            else
            {
                //序列号管控
                if (!model.GenerateSn && model.Sn.IsNullOrEmpty())
                {
                    throw new ValidationException("【序列号管控】的备件新增盘盈时，未勾选【自动生成序列号】时，序列号必须输入".L10N());
                }

                if (model.IsGood && model.IsNg)
                {
                    throw new ValidationException("【序列号管控】的备件新增盘盈时，不能同时勾选良品和不良品".L10N());
                }

                if (!model.IsGood && !model.IsNg)
                {
                    throw new ValidationException("【序列号管控】的备件新增盘盈时，请勾选良品和不良品".L10N());
                }
            }

            //获取盘点范围
            var inventoryTaskSparePartScope = Query<InventoryTaskSparePartScope>()
                .Where(x => x.InventoryTaskId == model.InventoryTaskId)
                .FirstOrDefault();

            var sparePart = Query<SparePart>().Where(x => x.Id == model.SparePartId).FirstOrDefault();

            ValidationInventoryScope(inventoryTaskSparePartScope, sparePart);

            var now = RF.Find<InventoryTaskFixtureEncode>().GetDbTime();

            InventoryTaskSparePartDetail inventoryTaskSparePartDetail = new InventoryTaskSparePartDetail()
            {
                InventoryTaskId = task.Id,
                InventoryStatus = InventoryStatus.Done,                
                SparePartId = model.SparePartId,
                GoodQty = 0,
                NgQty = 0,
                Total = 0,
                InventoryAssetSource = InventoryAssetSource.Profit,
                FirstCounterId = RT.IdentityId,
                FirstDateTime = now,
                SecondCounterId = RT.IdentityId,
                SecondDateTime = now,
                WarehouseId = model.WarehouseId,
                StorageLocationId = model.StorageLocationId,
            };

            var goodQty = model.GoodQty;
            var ngQty = model.NgQty;
            var totalQty = model.GoodQty + model.NgQty;

            switch (sparePart.ControlMethod)
            {
                case ControlMethod.ItemCode:
                    break;
                case ControlMethod.Batch:
                    {
                        ProcessIfBatch(model.GenerateLotNo, model.LotNo, sparePart, inventoryTaskSparePartDetail);
                    }
                    break;
                case ControlMethod.Sn:
                    {
                        ProcessIfSn(model.GenerateSn, model.Sn, sparePart, inventoryTaskSparePartDetail);

                        if (model.IsGood)
                        {
                            goodQty = 1;
                            ngQty = 0;
                        }
                        else
                        {
                            goodQty = 0;
                            ngQty = 1;
                        }

                        totalQty = 1;
                    }
                    break;
                default:
                    break;
            }

            UpdateInventoryResultWhenAddProfit(task, inventoryTaskSparePartDetail, goodQty, ngQty, totalQty);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(inventoryTaskSparePartDetail);

                if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone)
                {
                    DB.Update<InventoryTask>()
                        .Set(x => x.InventoryTaskStatus, InventoryTaskStatus.ScondDoing)
                        .Where(x => x.Id == task.Id)
                        .Execute();
                }

                trans.Complete();
            }

            return inventoryTaskSparePartDetail.Id;
        }

        /// <summary>
        /// 更新盘点结果
        /// </summary>
        /// <param name="task"></param>
        /// <param name="inventoryTaskSparePartDetail"></param>
        /// <param name="goodQty"></param>
        /// <param name="ngQty"></param>
        /// <param name="totalQty"></param>
        private void UpdateInventoryResultWhenAddProfit(InventoryTask task, InventoryTaskSparePartDetail inventoryTaskSparePartDetail, int goodQty, int ngQty, int totalQty)
        {
            switch (task.InventoryTaskStatus)
            {
                case InventoryTaskStatus.NotBegin:
                    break;
                case InventoryTaskStatus.Doing:
                    {
                        inventoryTaskSparePartDetail.FirstResult = InventoryResult.Profit;
                        inventoryTaskSparePartDetail.FirstGood = goodQty;
                        inventoryTaskSparePartDetail.FirstNg = ngQty;
                        inventoryTaskSparePartDetail.FirstTotal = totalQty;
                        inventoryTaskSparePartDetail.FirstDiff = totalQty - inventoryTaskSparePartDetail.Total;
                    }
                    break;
                case InventoryTaskStatus.FirstDone:
                case InventoryTaskStatus.ScondDoing:
                    {
                        inventoryTaskSparePartDetail.SecondResult = InventoryResult.Profit;
                        inventoryTaskSparePartDetail.SecondGoodQty = goodQty;
                        inventoryTaskSparePartDetail.SecondNgQty = ngQty;
                        inventoryTaskSparePartDetail.SecondTotal = totalQty;
                        inventoryTaskSparePartDetail.SecondDiff = totalQty - inventoryTaskSparePartDetail.Total;
                    }
                    break;
                case InventoryTaskStatus.Completed:
                    break;
                case InventoryTaskStatus.Closed:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 序列号时处理
        /// </summary>
        /// <param name="generateSn">自动产生SN</param>
        /// <param name="sn">序列号</param>
        /// <param name="sparePart">备件</param>
        /// <param name="inventoryTaskSparePartDetail"></param>
        /// <param name="inventoryTaskSparePartDetails">备件盘点明细列表</param>
        /// <param name="storeSummaryDetails">备件明细列表</param>
        /// <exception cref="ValidationException"></exception>
        private void ProcessIfSn(bool generateSn, string sn, SparePart sparePart, InventoryTaskSparePartDetail inventoryTaskSparePartDetail,
            EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails = null,
            EntityList<StoreSummaryDetail> storeSummaryDetails = null)
        {
            //	 管控方式为【序列号】
            if (generateSn)
            {
                inventoryTaskSparePartDetail.Sn = RT.Service.Resolve<SparePartController>().GetSnNumber();
            }
            else
            {
                if (sn.IsNullOrEmpty())
                {
                    throw new ValidationException("序列号没有填写".L10N());
                }

                inventoryTaskSparePartDetail.Sn = sn;

                //根据【序列号】到备件清单获取数据，能获取到时报错：【序列号XXX数据已存在】，获取不到时根据序列号获取备件，能获取到时，校验序列号的备件与填写
                if (inventoryTaskSparePartDetails == null)
                {
                    var existsCount = Query<InventoryTaskSparePartDetail>().Where(x => x.Sn == sn).Count();
                    if (existsCount > 0)
                    {
                        throw new ValidationException("序列号{0}数据已存在".L10nFormat(sn));
                    }
                }
                else
                {
                    var existsCount = inventoryTaskSparePartDetails.Count(x => x.Sn == sn);
                    if (existsCount > 0)
                    {
                        throw new ValidationException("序列号{0}数据已存在".L10nFormat(sn));
                    }
                }

                //获取不到时根据序列号获取备件，能获取到时，校验序列号的备件与填写的备件是否一致，不一致时报错：序列号XXX已存在且所属备件编码为XXX，请确认
                StoreSummaryDetail storeSummaryDetail = null;

                if (storeSummaryDetails != null)
                {
                    storeSummaryDetail = storeSummaryDetails.FirstOrDefault(x => x.OrderNumberCode == sn);
                }
                else
                {
                    storeSummaryDetail = RT.Service.Resolve<SparePartController>()
                        .GetStoreSummaryDetailBySns(new List<string> { sn }).FirstOrDefault();
                }

                if (storeSummaryDetail != null)
                {
                    if (storeSummaryDetail.SparePartId != sparePart.Id)
                    {
                        throw new ValidationException("序列号{0}已存在且所属备件编码为{1}，请确认"
                            .L10nFormat(sn, storeSummaryDetail.SparePartCode));
                    }

                    inventoryTaskSparePartDetail.Total = 1;

                    switch (storeSummaryDetail.OdNbStatus)
                    {
                        case OdNbStatus.NoGoodProduct:
                            {
                                inventoryTaskSparePartDetail.GoodQty = 0;
                                inventoryTaskSparePartDetail.NgQty = 1;
                            }
                            break;
                        case OdNbStatus.GoodProduct:
                            {
                                inventoryTaskSparePartDetail.GoodQty = 1;
                                inventoryTaskSparePartDetail.NgQty = 0;
                            }
                            break;
                        default:
                            break;
                    }

                    inventoryTaskSparePartDetail.WarehouseId = storeSummaryDetail.WarehouseId;
                    inventoryTaskSparePartDetail.StorageLocationId = storeSummaryDetail.StorageLocationId;
                }
            }
        }

        /// <summary>
        /// 备件盘点差异
        /// </summary>
        /// <param name="task"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="sortInfo"></param>
        /// <returns></returns>
        public virtual EntityList<InventoryTaskSparePartDiff> GetInventoryTaskSparePartDiffs(InventoryTask task, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var sparePartDetails = Query<InventoryTaskSparePartDiff>()
                 .Where(x => x.InventoryTaskId == task.Id)
                 .OrderBy(sortInfo)
                 .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return sparePartDetails;
        }

        /// <summary>
        /// 验证盘点范围
        /// </summary>        
        /// <param name="inventoryTaskSparePartScope">盘点范围</param>
        /// <param name="sparePart">备件基础资料</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private void ValidationInventoryScope(InventoryTaskSparePartScope inventoryTaskSparePartScope, SparePart sparePart)
        {
            if (inventoryTaskSparePartScope == null)
            {
                throw new ValidationException("盘点任务的盘点范围信息找不到".L10N());
            }

            //	备件编码校验是否符合盘点范围的【类型、分类层级、备件编码、管控方式】
            if (inventoryTaskSparePartScope.SparePartId.HasValue && inventoryTaskSparePartScope.SparePartId != sparePart.Id)
            {
                throw new ValidationException("增加盘盈的备件[备件编码]与盘点范围不符合".L10N());
            }

            if (sparePart == null)
            {
                throw new ValidationException("增加盘盈的备件信息找不到".L10N());
            }

            if (inventoryTaskSparePartScope.PartType.HasValue && inventoryTaskSparePartScope.PartType != sparePart.SpartType)
            {
                throw new ValidationException("增加盘盈的备件[类型]与盘点范围不符合".L10N());
            }

            if (inventoryTaskSparePartScope.ItemCategoryId.HasValue && inventoryTaskSparePartScope.ItemCategoryId != sparePart.ItemCategoryId)
            {
                throw new ValidationException("增加盘盈的备件[分类层级]与盘点范围不符合".L10N());
            }

            if (inventoryTaskSparePartScope.ControlMethod.HasValue && inventoryTaskSparePartScope.ControlMethod != sparePart.ControlMethod)
            {
                throw new ValidationException("增加盘盈的备件[管控方式]与盘点范围不符合".L10N());
            }
        }

        /// <summary>
        /// 处理批次
        /// </summary>
        /// <param name="generateLotNo">自动生成批次号</param>
        /// <param name="lotNo">批次号</param>
        /// <param name="sparePart"></param>
        /// <param name="inventoryTaskSparePartDetail"></param>
        /// <param name="storeSummaryLots"></param>
        /// <exception cref="ValidationException"></exception>
        private void ProcessIfBatch(bool generateLotNo, string lotNo, SparePart sparePart, InventoryTaskSparePartDetail inventoryTaskSparePartDetail,
            EntityList<StoreSummaryLot> storeSummaryLots = null)
        {
            //	管控方式为【批次】且填写批次号时，根据批次号获取备件，能获取到时，校验批次的备件与填写的备件是否一致，不一致时报错：批次号XXX已存在且所属备件编码为XXX，请确认
            if (generateLotNo)
            {
                inventoryTaskSparePartDetail.LotNo = RT.Service.Resolve<SparePartController>().GetLotNumber();
            }
            else
            {
                if (lotNo.IsNullOrEmpty())
                {
                    throw new ValidationException("批次号没有填写".L10N());
                }

                inventoryTaskSparePartDetail.LotNo = lotNo;
                StoreSummaryLot storeSummaryLot = null;
                if (storeSummaryLots != null)
                {
                    storeSummaryLot = storeSummaryLots.FirstOrDefault(x => x.BatchNumber == lotNo);
                }
                else
                {
                    storeSummaryLot = RT.Service.Resolve<SparePartController>()
                        .GetStoreSummaryLotByLotNos(new List<string> { lotNo }).FirstOrDefault();
                }

                if (storeSummaryLot != null && storeSummaryLot.SparePartId != sparePart.Id)
                {
                    throw new ValidationException("批次号{0}已存在且所属备件编码为{1}，请确认"
                        .L10nFormat(lotNo, storeSummaryLot.SparePartCode));
                }
            }
        }

        /// <summary>
        /// 查询备件的盘点范围
        /// </summary>
        /// <param name="inventoryTaskId">盘点任务ID</param>
        /// <returns></returns>
        public virtual InventoryTaskSparePartScope GetInventoryTaskSparePartScope(double inventoryTaskId)
        {
            return Query<InventoryTaskSparePartScope>().Where(x => x.InventoryTaskId == inventoryTaskId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询盘点任务下的备件汇总
        /// </summary>
        /// <param name="task">盘点任务</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns></returns>
        public virtual EntityList<InventoryTaskSparePart> GetInventoryTaskSpareParts(InventoryTask task, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<InventoryTaskSparePart>()
                .Where(x => x.InventoryTaskId == task.Id)
                .WhereIf(!task.SparePartCode.IsNullOrEmpty(), x => x.SparePart.SparePartCode.Contains(task.SparePartCode))
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存备件盘点
        /// </summary>
        /// <param name="taskList">任务列表</param>
        /// <param name="isPDAExec">是否PDA调用</param>
        /// <param name="inventoryTaskSparePartDetails">PDA调用时 传入前端变更列表</param>
        public virtual void SaveSparePartTaskList(EntityList<InventoryTask> taskList, bool isPDAExec = false, EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails = null)
        {
            var taskIds = taskList.Select(p => p.Id).ToList();
            var dbInventoryTaskSparePartDetails = GetInventoryTaskSparePartDetailsByTaskIds(taskIds);

            //验证盘点人不能重复
            var sparePartCounters = taskIds.SplitContains(tempIds =>
            {
                return Query<InventoryTaskSparePartCounter>().Where(x => tempIds.Contains(x.InventoryTaskId)).ToList();
            });

            foreach (var inventoryTask in taskList)
            {
                foreach (var counter in inventoryTask.InventoryTaskSparePartCounterList)
                {
                    if (inventoryTask.InventoryTaskSparePartCounterList.Count(x => x.EmployeeId == counter.EmployeeId) > 1)
                    {
                        throw new ValidationException("盘点任务【{0}】盘点人重复".L10nFormat(inventoryTask.TaskNo));
                    }

                    if (sparePartCounters.Any(x => x.InventoryTaskId == inventoryTask.Id
                        && x.EmployeeId == counter.EmployeeId
                        && x.Id != counter.Id))
                    {
                        throw new ValidationException("盘点任务【{0}】盘点人重复".L10nFormat(inventoryTask.TaskNo));
                    }
                }
            }

            var saveDetailList = new EntityList<InventoryTaskSparePartDetail>();
            var now = RF.Find<InventoryTask>().GetDbTime();
            if (isPDAExec)
            {
                var task = taskList.First();
                UpdateInvResult(inventoryTaskSparePartDetails, task);
                SetInventorySparePartInfo(dbInventoryTaskSparePartDetails, now, inventoryTaskSparePartDetails, taskList.First());
                saveDetailList.AddRange(inventoryTaskSparePartDetails);
            }
            else
            {
                foreach (var task in taskList)
                {
                    SetInventorySparePartInfo(dbInventoryTaskSparePartDetails, now, task.InventoryTaskSparePartDetailList, task);
                    saveDetailList.AddRange(task.InventoryTaskSparePartDetailList);
                }
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存界面数据
                RF.Save(taskList);
                RF.Save(saveDetailList);
                var planIds = taskList.Select(p => p.InventoryPlanId).Distinct().ToList();

                //更新盘点任务和盘点计划的盘点进度
                UpdatePercentage(planIds, taskIds);

                //更新备件汇总数据
                UpdateSparePartGroup(taskIds);

                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 更新前端盘点结果
        /// </summary>
        /// <param name="inventoryTaskSparePartDetails"></param>
        /// <param name="task"></param>
        private void UpdateInvResult(EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails, InventoryTask task)
        {
            foreach (var sparePartDetail in inventoryTaskSparePartDetails)
            {

                if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                {
                    if (sparePartDetail.FirstDiff > 0)
                    {
                        sparePartDetail.FirstResult = InventoryResult.Profit;
                    }
                    else if (sparePartDetail.FirstDiff < 0)
                    {
                        sparePartDetail.FirstResult = InventoryResult.Loss;
                    }
                    else if (sparePartDetail.FirstDiff == 0
                        && sparePartDetail.FirstGood == sparePartDetail.GoodQty
                        && sparePartDetail.FirstNg == sparePartDetail.NgQty)
                    {
                        sparePartDetail.FirstResult = InventoryResult.Normal;
                    }
                    else
                    {
                        sparePartDetail.FirstResult = InventoryResult.InfoChange;
                    }
                }
                else if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone ||
             task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
                {
                    if (sparePartDetail.SecondDiff > 0)
                    {
                        sparePartDetail.SecondResult = InventoryResult.Profit;
                    }
                    else if (sparePartDetail.SecondDiff < 0)
                    {
                        sparePartDetail.SecondResult = InventoryResult.Loss;
                    }
                    else if (sparePartDetail.SecondDiff == 0
                        && sparePartDetail.SecondGoodQty == sparePartDetail.GoodQty
                        && sparePartDetail.SecondNgQty == sparePartDetail.NgQty)
                    {
                        sparePartDetail.SecondResult = InventoryResult.Normal;
                    }
                    else
                    {
                        sparePartDetail.SecondResult = InventoryResult.InfoChange;
                    }
                }
            }
        }

        /// <summary>
        /// 设置备件盘点信息
        /// </summary>
        /// <param name="dbInventoryTaskSparePartDetails"></param>
        /// <param name="now"></param>
        /// <param name="changedInventoryTaskSparePartDetailList">前端变更的列表</param>
        /// <param name="task"></param>
        private void SetInventorySparePartInfo(EntityList<InventoryTaskSparePartDetail> dbInventoryTaskSparePartDetails, DateTime now,
            EntityList<InventoryTaskSparePartDetail> changedInventoryTaskSparePartDetailList,
            InventoryTask task)
        {
            foreach (var sparePartDetail in changedInventoryTaskSparePartDetailList)
            {
                if (sparePartDetail.ControlMethod == ControlMethod.Sn
                    && sparePartDetail.FirstTotal > 1)
                {
                    throw new ValidationException("序列号管控的【初盘总数】不能大于1".L10N());
                }

                if (sparePartDetail.ControlMethod == ControlMethod.Sn
                    && sparePartDetail.SecondTotal > 1)
                {
                    throw new ValidationException("序列号管控的【复盘总数】不能大于1".L10N());
                }

                //初盘结果修改时，除了保存修改的字段，还要更新初盘人和初盘时间；
                var dbSparePartDetail = dbInventoryTaskSparePartDetails.FirstOrDefault(p => p.Id == sparePartDetail.Id);

                if (dbSparePartDetail != null && dbSparePartDetail.FirstResult != sparePartDetail.FirstResult)
                {
                    sparePartDetail.FirstCounterId = RT.IdentityId;
                    sparePartDetail.FirstDateTime = now;
                }

                //复盘结果修改时，除了保存修改的字段，还要更新复盘人和复盘时间
                if (dbSparePartDetail != null && dbSparePartDetail.SecondResult != sparePartDetail.SecondResult)
                {
                    sparePartDetail.SecondCounterId = RT.IdentityId;
                    sparePartDetail.SecondDateTime = now;
                }
            }

            //主表盘点状态为【初盘完成】时，更新为【复盘中】
            if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone)
            {
                task.InventoryTaskStatus = InventoryTaskStatus.ScondDoing;
            }
        }

        /// <summary>
        /// 更新备件盘点汇总
        /// </summary>
        /// <param name="taskIds"></param>
        private void UpdateSparePartGroup(List<double> taskIds)
        {
            var spareDetailCountDatas = GetSpareDetailCountDataByTaskIds(taskIds);
            EntityList<InventoryTaskSparePart> inventoryTaskSpareParts = new EntityList<InventoryTaskSparePart>();
            foreach (var spareDetailCountData in spareDetailCountDatas)
            {
                var updateDataCount = DB.Update<InventoryTaskSparePart>()
                    .Set(x => x.FirstGood, spareDetailCountData.FirstGood)
                    .Set(x => x.FirstNg, spareDetailCountData.FirstNg)
                    .Set(x => x.FirstTotal, spareDetailCountData.FirstTotal)
                    .Set(x => x.FirstDiff, spareDetailCountData.FirstDiff)
                    .Set(x => x.SecondGoodQty, spareDetailCountData.SecondGoodQty)
                    .Set(x => x.SecondNgQty, spareDetailCountData.SecondNgQty)
                    .Set(x => x.SecondTotal, spareDetailCountData.SecondTotal)
                    .Set(x => x.SecondDiff, spareDetailCountData.SecondDiff)
                    .Where(x => x.InventoryTaskId == spareDetailCountData.InventoryTaskId
                        && x.SparePartId == spareDetailCountData.SparePartId)
                    .Execute();

                if (updateDataCount <= 0)
                {
                    inventoryTaskSpareParts.Add(new InventoryTaskSparePart()
                    {
                        InventoryTaskId = spareDetailCountData.InventoryTaskId,
                        SparePartId = spareDetailCountData.SparePartId,
                        FirstGood = spareDetailCountData.FirstGood,
                        FirstNg = spareDetailCountData.FirstNg,
                        FirstTotal = spareDetailCountData.FirstTotal,
                        FirstDiff = spareDetailCountData.FirstDiff,
                        SecondGoodQty = spareDetailCountData.SecondGoodQty,
                        SecondNgQty = spareDetailCountData.SecondNgQty,
                        SecondTotal = spareDetailCountData.SecondTotal,
                        SecondDiff = spareDetailCountData.SecondDiff,
                    });
                }
            }

            if (inventoryTaskSpareParts.Any())
            {
                RF.Save(inventoryTaskSpareParts);
            }
        }

        /// <summary>
        /// 更新设备盘点任务和盘点计划的盘点进度
        /// </summary>
        /// <param name="planIds">盘点计划Id列表</param>
        /// <param name="taskIds">盘点任务Id列表</param>
        private void UpdatePercentage(List<double> planIds, List<double> taskIds)
        {
            var plans = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlansByIds(planIds);

            var allSpareDetails = GetInventoryTaskSpareDetailByPlanIds(planIds);

            foreach (var taskId in taskIds)
            {
                //这个盘点单下所有来源为【账内资产】的数据中，初盘结果有值的数量除以所有来源为【账内资产】的数量，保留2位小数
                decimal allQty = allSpareDetails.Where(p => p.InventoryTaskId == taskId).Sum(x => x.Count);

                decimal qty = allSpareDetails.Where(p => p.InventoryTaskId == taskId && p.FirstResult != null).Sum(x => x.Count);

                if (allQty > 0)
                {
                    var percentage = Math.Floor(qty / allQty * 100);

                    DB.Update<InventoryTask>()
                        .Set(p => p.Percentage, percentage)
                        .Where(p => p.Id == taskId)
                        .Execute();
                }
            }

            foreach (var planId in plans.Select(x => x.Id))
            {
                //盘点计划关联的盘点单下所有来源为【账内资产】的数据中，初盘结果有值的数量除以所有来源为【账内资产】的数量，保留2位小数
                decimal allQty = allSpareDetails.Where(p => p.InventoryPlanId == planId).Sum(x => x.Count);
                decimal qty = allSpareDetails.Where(p => p.InventoryPlanId == planId && p.FirstResult != null).Sum(x => x.Count);

                if (allQty > 0)
                {
                    var percentage = Math.Floor(qty / allQty * 100);

                    DB.Update<InventoryPlan>()
                        .Set(p => p.Percentage, percentage)
                        .Where(p => p.Id == planId).Execute();
                }
            }
        }

        /// <summary>
        /// 获取盘点任务设备清单信息
        /// </summary>
        /// <param name="planIds">计划id列表</param>
        /// <returns>盘点任务设备清单信息</returns>
        private IList<SpareDetailFirstResult> GetInventoryTaskSpareDetailByPlanIds(List<double> planIds)
        {
            var list = Query<InventoryTaskSparePartDetail>()
                .Join<InventoryTask>((a, b) => a.InventoryTaskId == b.Id && planIds.Contains(b.InventoryPlanId))
                .Where(x => x.InventoryAssetSource == InventoryAssetSource.Account)
                .GroupBy<InventoryTask>((x, y) => new
                {
                    y.InventoryPlanId,
                    x.InventoryTaskId,
                    x.FirstResult
                })
                .Select<InventoryTask>((x, y) => new
                {
                    InventoryPlanId = y.InventoryPlanId,
                    InventoryTaskId = x.InventoryTaskId,
                    FirstResult = x.FirstResult,
                    Count = x.Id.COUNT()
                })
                .ToList<SpareDetailFirstResult>();

            return list;
        }

        /// <summary>
        /// 获取任务盘点备件清单
        /// </summary>
        /// <param name="taskIds">任务id列表</param>
        /// <returns>任务盘点设备清单</returns>
        public virtual EntityList<InventoryTaskSparePartDetail> GetInventoryTaskSparePartDetailsByTaskIds(List<double> taskIds)
        {
            return taskIds.SplitContains(ids => Query<InventoryTaskSparePartDetail>()
            .Where(p => ids.Contains(p.InventoryTaskId))
            .ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取盘点任务设备清单信息
        /// </summary>
        /// <param name="taskIds">盘点任务id列表</param>
        /// <returns>盘点任务设备清单信息</returns>
        private IList<SpareDetailCountData> GetSpareDetailCountDataByTaskIds(List<double> taskIds)
        {
            var list = Query<InventoryTaskSparePartDetail>()
                .Where(x => taskIds.Contains(x.InventoryTaskId))
                .GroupBy(x => new
                {
                    x.InventoryTaskId,
                    x.SparePartId,
                })
                .Select(x => new
                {
                    InventoryTaskId = x.InventoryTaskId,
                    SparePartId = x.SparePartId,
                    FirstGood = x.FirstGood.SUM(),
                    FirstNg = x.FirstNg.SUM(),
                    FirstTotal = x.FirstTotal.SUM(),
                    FirstDiff = x.FirstDiff.SUM(),
                    SecondGoodQty = x.SecondGoodQty.SUM(),
                    SecondNgQty = x.SecondNgQty.SUM(),
                    SecondTotal = x.SecondTotal.SUM(),
                    SecondDiff = x.SecondDiff.SUM(),
                })
                .ToList<SpareDetailCountData>();

            return list;
        }

        /// <summary>
        /// 导入备件盘点明细的保存
        /// </summary>
        /// <param name="batch">导入备件盘点明细</param>
        /// <returns></returns>
        public virtual IEnumerable<ImportMessageResult> ImportTaskSparePartDetails(IList<RowData> batch)
        {
            List<ImportMessageResult> messageList = new List<ImportMessageResult>();
            var sparePartDetailsOfImport = batch.Select(x => x.Entity as InventoryTaskSparePartDetail).ToList();

            var storageLocationCodes = sparePartDetailsOfImport.Select(x => x.StorageLocationCode).Distinct().ToList();

            var inventoryTaskIds = sparePartDetailsOfImport.Select(x => x.InventoryTaskId).Distinct().ToList();

            var storageLocations = RT.Service.Resolve<WarehouseController>().GetStorageLocationByCodes(storageLocationCodes);

            var lotNos = sparePartDetailsOfImport.Where(x => x.SparePart.ControlMethod == ControlMethod.Batch).Select(x => x.LotNo).Distinct().ToList();
            var storeSummaryLots = RT.Service.Resolve<SparePartController>().GetStoreSummaryLotByLotNos(lotNos);

            var snList = sparePartDetailsOfImport.Where(x => x.SparePart.ControlMethod == ControlMethod.Sn).Select(x => x.Sn).Distinct().ToList();
            var storeSummaryDetails = RT.Service.Resolve<SparePartController>().GetStoreSummaryDetailBySns(snList);

            var sparePartDetailsOfDb = inventoryTaskIds.SplitContains(tempIds =>
            {
                return Query<InventoryTaskSparePartDetail>().Where(x => tempIds.Contains(x.InventoryTaskId)).ToList();
            });

            var inventoryTaskSparePartScopes = inventoryTaskIds.SplitContains(tempIds =>
            {
                return Query<InventoryTaskSparePartScope>().Where(x => tempIds.Contains(x.InventoryTaskId)).ToList();
            });

            var now = RF.Find<InventoryTaskFixtureEncode>().GetDbTime();

            Dictionary<double, bool> taskIdsOfNeedUpdateDictionary = new Dictionary<double, bool>();

            List<double> planIds = new List<double>();
            List<double> taskIds = new List<double>();

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var rowData in batch)
                {
                    var sparePartDetailOfImport = rowData.Entity as InventoryTaskSparePartDetail;

                    if (VlidationIfDuplicate(batch, messageList, rowData, sparePartDetailOfImport))
                    {
                        continue;
                    }

                    bool error = ValidationImportDataRow(messageList, rowData, sparePartDetailOfImport);

                    if (error)
                    {
                        continue;
                    }

                    StorageLocation storageLocation = GetStorageLocation(messageList, storageLocations, rowData, sparePartDetailOfImport);

                    if (storageLocation == null)
                    {
                        continue;
                    }

                    InventoryTaskSparePartDetail sparePartDetail = GetExistsSparePartDetail(sparePartDetailsOfDb, sparePartDetailOfImport, storageLocation);

                    try
                    {
                        if (sparePartDetail == null)
                        {
                            //获取不到时，创建盘盈
                            sparePartDetail = AddSparePartProfit(storeSummaryLots, storeSummaryDetails, sparePartDetailsOfDb,
                                inventoryTaskSparePartScopes, now, sparePartDetailOfImport);
                        }
                        else
                        {
                            //更新盘点结果
                            UpdateInventoryResultWhenImport(sparePartDetailOfImport.InventoryTask, sparePartDetail, sparePartDetailOfImport.GoodQty,
                                sparePartDetailOfImport.NgQty, now);
                        }

                        RF.Save(sparePartDetail);

                        //初盘完成的，要更新为复盘中
                        if (sparePartDetailOfImport.InventoryTask.InventoryTaskStatus == InventoryTaskStatus.FirstDone
                            && !taskIdsOfNeedUpdateDictionary.ContainsKey(sparePartDetailOfImport.InventoryTaskId))
                        {
                            taskIdsOfNeedUpdateDictionary.Add(sparePartDetailOfImport.InventoryTaskId, true);
                        }

                        if (!planIds.Any(x => x == sparePartDetailOfImport.InventoryTask.InventoryPlanId))
                        {
                            planIds.Add(sparePartDetailOfImport.InventoryTask.InventoryPlanId);
                        }

                        if (!taskIds.Any(x => x == sparePartDetailOfImport.InventoryTaskId))
                        {
                            taskIds.Add(sparePartDetailOfImport.InventoryTaskId);
                        }

                        messageList.Add(new ImportMessageResult
                        {
                            RowNum = rowData.RowIndex + 1,
                            MsgType = ImportMessageType.SaveSucess,
                            Message = "保存成功！".L10N()
                        });
                    }
                    catch (Exception ex)
                    {
                        messageList.Add(new ImportMessageResult()
                        {
                            Message = ex.Message,
                            MsgType = ImportMessageType.LoadFail,
                            RowNum = rowData.RowIndex + 1
                        });
                    }
                }

                if (taskIds.Any())
                {
                    //更新盘点任务和盘点计划的盘点进度
                    UpdatePercentage(planIds, taskIds);

                    //更新备件汇总数据
                    UpdateSparePartGroup(taskIds);
                }

                if (taskIdsOfNeedUpdateDictionary.Any())
                {
                    //初盘完成的，要更新为复盘中
                    foreach (var inventoryTaskId in taskIdsOfNeedUpdateDictionary.Keys)
                    {
                        DB.Update<InventoryTask>()
                            .Set(x => x.InventoryTaskStatus, InventoryTaskStatus.ScondDoing)
                            .Where(x => x.Id == inventoryTaskId)
                            .Execute();
                    }
                }

                trans.Complete();
            }

            return messageList;
        }

        private bool ValidationImportDataRow(List<ImportMessageResult> messageList, RowData rowData, InventoryTaskSparePartDetail sparePartDetailOfImport)
        {
            var error = false;

            if (sparePartDetailOfImport.GoodQty < 0)
            {
                messageList.Add(new ImportMessageResult()
                {
                    Message = "【良品数】不能为负数"
                          .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode),
                    MsgType = ImportMessageType.LoadFail,
                    RowNum = rowData.RowIndex + 1
                });

                error = true;
            }

            if (sparePartDetailOfImport.NgQty < 0)
            {
                messageList.Add(new ImportMessageResult()
                {
                    Message = "【不良品数】不能为负数"
                          .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode),
                    MsgType = ImportMessageType.LoadFail,
                    RowNum = rowData.RowIndex + 1
                });

                error = true;
            }

            if (sparePartDetailOfImport.GoodQty == 0 && sparePartDetailOfImport.NgQty == 0)
            {
                messageList.Add(new ImportMessageResult()
                {
                    Message = "【良品数】和【不良品数】不能同时为空或同时为0"
                          .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode),
                    MsgType = ImportMessageType.LoadFail,
                    RowNum = rowData.RowIndex + 1
                });

                error = true;
            }

            if (sparePartDetailOfImport.SparePart.ControlMethod == ControlMethod.Batch)
            {
                if (sparePartDetailOfImport.LotNo.IsNullOrEmpty())
                {
                    messageList.Add(new ImportMessageResult()
                    {
                        Message = "备件编码【{0}】管控方式是【批次管控】，必须输入【批次号】"
                           .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode),
                        MsgType = ImportMessageType.LoadFail,
                        RowNum = rowData.RowIndex + 1
                    });

                    error = true;
                }

                if (!sparePartDetailOfImport.Sn.IsNullOrEmpty())
                {
                    messageList.Add(new ImportMessageResult()
                    {
                        Message = "备件编码【{0}】管控方式是【批次管控】，不能输入【序列号】"
                           .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode),
                        MsgType = ImportMessageType.LoadFail,
                        RowNum = rowData.RowIndex + 1
                    });

                    error = true;
                }
            }

            if (sparePartDetailOfImport.SparePart.ControlMethod == ControlMethod.Sn)
            {
                if (sparePartDetailOfImport.Sn.IsNullOrEmpty())
                {
                    messageList.Add(new ImportMessageResult()
                    {
                        Message = "备件编码【{0}】管控方式是【序列管控】，必须输入【序列号】"
                           .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode),
                        MsgType = ImportMessageType.LoadFail,
                        RowNum = rowData.RowIndex + 1
                    });

                    error = true;
                }

                if (!sparePartDetailOfImport.LotNo.IsNullOrEmpty())
                {
                    messageList.Add(new ImportMessageResult()
                    {
                        Message = "备件编码【{0}】管控方式是【序列管控】，不能输入【批次号】"
                           .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode),
                        MsgType = ImportMessageType.LoadFail,
                        RowNum = rowData.RowIndex + 1
                    });

                    error = true;
                }

                if ((sparePartDetailOfImport.GoodQty + sparePartDetailOfImport.NgQty) > 1)
                {
                    messageList.Add(new ImportMessageResult()
                    {
                        Message = "备件编码【{0}】管控方式是【序列管控】总数量不能大于1"
                            .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode),
                        MsgType = ImportMessageType.LoadFail,
                        RowNum = rowData.RowIndex + 1
                    });

                    error = true;
                }
            }

            return error;
        }

        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="messageList"></param>
        /// <param name="storageLocations"></param>
        /// <param name="rowData"></param>
        /// <param name="sparePartDetailOfImport"></param>
        /// <returns></returns>
        private StorageLocation GetStorageLocation(List<ImportMessageResult> messageList, EntityList<StorageLocation> storageLocations, RowData rowData, InventoryTaskSparePartDetail sparePartDetailOfImport)
        {
            var storageLocation = storageLocations.FirstOrDefault(x => x.WarehouseId == sparePartDetailOfImport.InventoryTask.WarehouseId
                    && x.Code == sparePartDetailOfImport.StorageLocationCode);

            if (storageLocation == null)
            {
                messageList.Add(new ImportMessageResult()
                {
                    Message = "库位编码【{0}】不存在".L10nFormat(sparePartDetailOfImport.StorageLocationCode),
                    MsgType = ImportMessageType.LoadFail,
                    RowNum = rowData.RowIndex + 1
                });
            }

            return storageLocation;
        }

        /// <summary>
        /// 验证是否重复
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="messageList"></param>
        /// <param name="rowData"></param>
        /// <param name="sparePartDetailOfImport"></param>
        /// <returns></returns>
        private bool VlidationIfDuplicate(IList<RowData> batch, List<ImportMessageResult> messageList, RowData rowData, InventoryTaskSparePartDetail sparePartDetailOfImport)
        {
            bool isDuplicate = false;
            switch (sparePartDetailOfImport.SparePart.ControlMethod)
            {
                case ControlMethod.ItemCode:
                    if (batch.Any(x => x.RowIndex != rowData.RowIndex
                         && (x.Entity as InventoryTaskSparePartDetail).SparePartId == sparePartDetailOfImport.SparePartId
                         && (x.Entity as InventoryTaskSparePartDetail).StorageLocationCode == sparePartDetailOfImport.StorageLocationCode))
                    {
                        messageList.Add(new ImportMessageResult()
                        {
                            Message = "导入的资料中，备件编码【{0}】、库位【{1}】重复"
                                .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode, sparePartDetailOfImport.StorageLocationCode),
                            MsgType = ImportMessageType.LoadFail,
                            RowNum = rowData.RowIndex + 1
                        });

                        isDuplicate = true;
                    }
                    break;
                case ControlMethod.Batch:
                    if (batch.Any(x => x.RowIndex != rowData.RowIndex
                        && (x.Entity as InventoryTaskSparePartDetail).SparePartId == sparePartDetailOfImport.SparePartId
                        && (x.Entity as InventoryTaskSparePartDetail).StorageLocationCode == sparePartDetailOfImport.StorageLocationCode
                        && (x.Entity as InventoryTaskSparePartDetail).LotNo == sparePartDetailOfImport.LotNo))
                    {
                        messageList.Add(new ImportMessageResult()
                        {
                            Message = "导入的资料中,备件编码【{0}】、库位【{1}】、批次【{2}】重复"
                                .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode, sparePartDetailOfImport.StorageLocationCode,
                                    sparePartDetailOfImport.LotNo),
                            MsgType = ImportMessageType.LoadFail,
                            RowNum = rowData.RowIndex + 1
                        });

                        isDuplicate = true;
                    }
                    break;
                case ControlMethod.Sn:
                    if (batch.Any(x => x.RowIndex != rowData.RowIndex
                        && (x.Entity as InventoryTaskSparePartDetail).SparePartId == sparePartDetailOfImport.SparePartId
                        && (x.Entity as InventoryTaskSparePartDetail).StorageLocationCode == sparePartDetailOfImport.StorageLocationCode
                        && (x.Entity as InventoryTaskSparePartDetail).Sn == sparePartDetailOfImport.Sn))
                    {
                        messageList.Add(new ImportMessageResult()
                        {
                            Message = "导入的资料中,备件编码【{0}】、库位【{1}】、序列号【{2}】重复"
                                .L10nFormat(sparePartDetailOfImport.SparePart.SparePartCode, sparePartDetailOfImport.StorageLocationCode,
                                    sparePartDetailOfImport.Sn),
                            MsgType = ImportMessageType.LoadFail,
                            RowNum = rowData.RowIndex + 1
                        });

                        isDuplicate = true;
                    }
                    break;
                default:
                    break;
            }

            return isDuplicate;
        }

        /// <summary>
        /// 导入时增加盘盈
        /// </summary>
        /// <param name="storeSummaryLots">备件批次明细</param>
        /// <param name="storeSummaryDetails">备件序列号明细</param>
        /// <param name="sparePartDetailsOfDb">已存在盘件盘点明细</param>
        /// <param name="inventoryTaskSparePartScopes">盘点盘点范围</param>
        /// <param name="now">时间</param>
        /// <param name="sparePartDetailOfImport">导入的备件盘点明细</param>
        /// <returns></returns>
        private InventoryTaskSparePartDetail AddSparePartProfit(EntityList<StoreSummaryLot> storeSummaryLots, EntityList<StoreSummaryDetail> storeSummaryDetails, EntityList<InventoryTaskSparePartDetail> sparePartDetailsOfDb, EntityList<InventoryTaskSparePartScope> inventoryTaskSparePartScopes, DateTime now, InventoryTaskSparePartDetail sparePartDetailOfImport)
        {
            InventoryTaskSparePartDetail sparePartDetail;
            var inventoryTaskSparePartScope = inventoryTaskSparePartScopes
                .FirstOrDefault(x => x.InventoryTaskId == sparePartDetailOfImport.InventoryTaskId);

            ValidationInventoryScope(inventoryTaskSparePartScope, sparePartDetailOfImport.SparePart);

            sparePartDetail = new InventoryTaskSparePartDetail()
            {
                InventoryTaskId = sparePartDetailOfImport.InventoryTaskId,
                InventoryStatus = InventoryStatus.Done,
                FirstResult = InventoryResult.Profit,
                SparePartId = sparePartDetailOfImport.SparePartId,
                GoodQty = 0,
                NgQty = 0,
                Total = 0,
                InventoryAssetSource = InventoryAssetSource.Profit,
                FirstCounterId = RT.IdentityId,
                FirstDateTime = now,
                SecondCounterId = RT.IdentityId,
                SecondDateTime = now,
            };

            switch (sparePartDetailOfImport.SparePart.ControlMethod)
            {
                case ControlMethod.ItemCode:
                    break;
                case ControlMethod.Batch:
                    {
                        ProcessIfBatch(false, sparePartDetailOfImport.LotNo, sparePartDetailOfImport.SparePart,
                            sparePartDetail, storeSummaryLots);
                    }
                    break;
                case ControlMethod.Sn:
                    {
                        ProcessIfSn(false, sparePartDetailOfImport.Sn, sparePartDetailOfImport.SparePart, sparePartDetail,
                            sparePartDetailsOfDb, storeSummaryDetails);
                    }
                    break;
                default:
                    break;
            }

            UpdateInventoryResultWhenAddProfit(sparePartDetailOfImport.InventoryTask, sparePartDetail, sparePartDetailOfImport.GoodQty,
                sparePartDetailOfImport.NgQty, sparePartDetailOfImport.GoodQty + sparePartDetailOfImport.NgQty);

            return sparePartDetail;
        }

        /// <summary>
        /// 获取已经存在的备件明细
        /// </summary>
        /// <param name="sparePartDetailsOfDb">已经存在备件盘点明细</param>
        /// <param name="sparePartDetailImport">盘件盘点导入</param>
        /// <param name="storageLocation">库位</param>        
        /// <returns></returns>
        private InventoryTaskSparePartDetail GetExistsSparePartDetail(EntityList<InventoryTaskSparePartDetail> sparePartDetailsOfDb,
            InventoryTaskSparePartDetail sparePartDetailImport, StorageLocation storageLocation)
        {
            InventoryTaskSparePartDetail sparePartDetail = null;

            //编码管控和批次号管控的备件根据【备件编码+批次号+库位】到备件清单获取数据，能获取到时更新，获取不到时新增数据
            //序列号管控的根据【序列号】备件清单获取数据，能获取到时更新，获取不到时新增数据
            switch (sparePartDetailImport.SparePart.ControlMethod)
            {
                case ControlMethod.ItemCode:
                    {
                        sparePartDetail = sparePartDetailsOfDb
                            .FirstOrDefault(x => x.SparePartId == sparePartDetailImport.SparePartId
                                && (x.StorageLocationId == storageLocation.Id || x.StorageLocationId == null));
                    }
                    break;
                case ControlMethod.Batch:
                    {
                        sparePartDetail = sparePartDetailsOfDb
                            .FirstOrDefault(x => x.SparePartId == sparePartDetailImport.SparePartId
                                && (x.StorageLocationId == storageLocation.Id || x.StorageLocationId == null) && x.LotNo == sparePartDetailImport.LotNo);
                    }
                    break;
                case ControlMethod.Sn:
                    {
                        sparePartDetail = sparePartDetailsOfDb
                            .FirstOrDefault(x => x.SparePartId == sparePartDetailImport.SparePartId
                                && (x.StorageLocationId == storageLocation.Id || x.StorageLocationId == null) && x.Sn == sparePartDetailImport.Sn);
                    }
                    break;
                default:
                    break;
            }

            return sparePartDetail;
        }

        /// <summary>
        /// 导入时更新盘点结果
        /// </summary>
        /// <param name="task">盘点任务</param>
        /// <param name="sparePartDetail">盘点备件明细</param>
        /// <param name="goodQty">良品</param>
        /// <param name="ngQty">不良品</param>
        /// <param name="now">时间</param>        
        public virtual void UpdateInventoryResultWhenImport(InventoryTask task, InventoryTaskSparePartDetail sparePartDetail, int goodQty, int ngQty,
            DateTime now)
        {
            sparePartDetail.InventoryStatus = InventoryStatus.Done;

            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
            {
                sparePartDetail.FirstGood = goodQty;
                sparePartDetail.FirstNg = ngQty;
                sparePartDetail.FirstTotal = goodQty + ngQty;
                sparePartDetail.FirstDiff = (goodQty + ngQty) - sparePartDetail.Total;
                sparePartDetail.FirstCounterId = RT.IdentityId;
                sparePartDetail.FirstDateTime = now;

                //盘差异数大于0时，更新为【盘盈】
                //初盘差异数小于0时，更新为【盘亏】
                //初盘差异数等于0，且初盘良品数和与良品数相等、初盘不良品数和不良品数相等），更新为【正常】
                //其他场景，更新为【信息变动】
                if (sparePartDetail.FirstDiff > 0)
                {
                    sparePartDetail.FirstResult = InventoryResult.Profit;
                }
                else if (sparePartDetail.FirstDiff < 0)
                {
                    sparePartDetail.FirstResult = InventoryResult.Loss;
                }
                else if (sparePartDetail.FirstDiff == 0
                    && sparePartDetail.FirstGood == sparePartDetail.GoodQty
                    && sparePartDetail.FirstNg == sparePartDetail.NgQty)
                {
                    sparePartDetail.FirstResult = InventoryResult.Normal;
                }
                else
                {
                    sparePartDetail.FirstResult = InventoryResult.InfoChange;
                }
            }
            else if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone ||
                  task.InventoryTaskStatus == InventoryTaskStatus.ScondDoing)
            {
                sparePartDetail.SecondResult = InventoryResult.Profit;
                sparePartDetail.SecondGoodQty = goodQty;
                sparePartDetail.SecondNgQty = ngQty;
                sparePartDetail.SecondTotal = goodQty + ngQty;
                sparePartDetail.SecondDiff = (goodQty + ngQty) - sparePartDetail.Total;
                sparePartDetail.SecondCounterId = RT.IdentityId;
                sparePartDetail.SecondDateTime = now;

                //复盘差异数大于0时，更新为【盘盈】
                //复盘差异数小于0时，更新为【盘亏】
                //复盘差异数等于0，且复盘良品数和与良品数相等、复盘不良品数和不良品数相等，更新为【正常】
                //其他场景，更新为【信息变动】
                if (sparePartDetail.SecondDiff > 0)
                {
                    sparePartDetail.SecondResult = InventoryResult.Profit;
                }
                else if (sparePartDetail.SecondDiff < 0)
                {
                    sparePartDetail.SecondResult = InventoryResult.Loss;
                }
                else if (sparePartDetail.SecondDiff == 0
                    && sparePartDetail.SecondGoodQty == sparePartDetail.GoodQty
                    && sparePartDetail.SecondNgQty == sparePartDetail.NgQty)
                {
                    sparePartDetail.SecondResult = InventoryResult.Normal;
                }
                else
                {
                    sparePartDetail.SecondResult = InventoryResult.InfoChange;
                }
            }
            else
            {
                throw new ValidationException("盘点任务【{0}】状态为【{1}】,不能操作备件盘点明细导入"
                    .L10nFormat(task.TaskNo, task.InventoryTaskStatus.ToLabel()));
            }
        }

        /// <summary>
        /// 备件盘点差异
        /// </summary>
        /// <param name="saprePartTaskIds">盘点任务ID列表</param>
        public virtual EntityList<InventoryTaskSparePartDiff> GenerateSparePartDiff(List<double> saprePartTaskIds)
        {
            EntityList<InventoryTaskSparePartDiff> inventoryTaskSparePartDiffList = new EntityList<InventoryTaskSparePartDiff>();

            //	编码管控的，盘点结果为【盘盈、盘亏】的每条数据生成一条盘点差异
            GenerateDiffControlByItemCode(saprePartTaskIds, inventoryTaskSparePartDiffList);

            //	批次管控的，按编码汇总【总数】和【盘点总数】，总数和盘点总数不相等的数据生成一条盘点差异
            GenerateDiffControlByBatch(saprePartTaskIds, inventoryTaskSparePartDiffList);

            //	序列号
            GenerateDiffControlBySn(saprePartTaskIds, inventoryTaskSparePartDiffList);

            return inventoryTaskSparePartDiffList;
        }

        private void GenerateDiffControlBySn(List<double> saprePartTaskIds, EntityList<InventoryTaskSparePartDiff> inventoryTaskSparePartDiffList)
        {
            var inventoryTaskSparePartDetailsOfSn = saprePartTaskIds.SplitContains(tempIds =>
            {
                return Query<InventoryTaskSparePartDetail>()
                    .Where(x => x.SparePart.ControlMethod == ControlMethod.Sn && tempIds.Contains(x.InventoryTaskId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            foreach (var sparePartDetail in inventoryTaskSparePartDetailsOfSn)
            {
                if ((!sparePartDetail.FirstTotal.HasValue && !sparePartDetail.SecondTotal.HasValue) ||
                       (!sparePartDetail.SecondResult.HasValue && !sparePartDetail.FirstResult.HasValue))
                {
                    throw new ValidationException("盘点任务【{0}】备件【{1}】无盘点结果，不能盘点完成"
                        .L10nFormat(sparePartDetail.InventoryTaskNo, sparePartDetail.SparePartCode));
                }

                int actualTotal = 0;

                if (sparePartDetail.SecondTotal.HasValue)
                {
                    actualTotal = sparePartDetail.SecondTotal.Value;
                }
                else
                {
                    actualTotal = sparePartDetail.FirstTotal.Value;
                }

                var inventoryResult = (sparePartDetail.SecondResult ?? sparePartDetail.FirstResult);

                InventoryTaskSparePartDiff sparePartDiff = new InventoryTaskSparePartDiff();
                sparePartDiff.SparePartId = sparePartDetail.SparePartId;
                sparePartDiff.Sn = sparePartDetail.Sn;
                sparePartDiff.Total = sparePartDetail.Total;
                sparePartDiff.ActualTotal = actualTotal;
                sparePartDiff.Diff = sparePartDiff.ActualTotal - sparePartDiff.Total;
                sparePartDiff.InventoryTaskId = sparePartDetail.InventoryTaskId;
                sparePartDiff.InventoryResult = inventoryResult.Value;
                inventoryTaskSparePartDiffList.Add(sparePartDiff);
            }
        }

        private void GenerateDiffControlByBatch(List<double> saprePartTaskIds, EntityList<InventoryTaskSparePartDiff> inventoryTaskSparePartDiffList)
        {
            var detailsOfBatch = saprePartTaskIds.SplitContains(tempIds =>
            {
                return Query<InventoryTaskSparePartDetail>()
                    .Where(x => (x.SparePart.ControlMethod == ControlMethod.Batch)
                      && tempIds.Contains(x.InventoryTaskId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            if (detailsOfBatch.Any(x => x.FirstTotal == null && x.SecondTotal == null))
            {
                throw new ValidationException("选择的盘点任务有备件无盘点结果，不能盘点完成".L10N());
            }

            foreach (var taskId in saprePartTaskIds)
            {
                var sparePartIds = detailsOfBatch.Where(x => x.InventoryTaskId == taskId)
                        .Select(x => x.SparePartId).Distinct();

                foreach (var sparePartId in sparePartIds)
                {
                    int actualTotalOfSparePart = 0;
                    int totalOfSparePart = 0;

                    foreach (var sparePartDetail in detailsOfBatch.Where(x => x.SparePartId == sparePartId))
                    {
                        int actualTotal = 0;

                        if (sparePartDetail.SecondTotal.HasValue)
                        {
                            actualTotal = sparePartDetail.SecondTotal.Value;
                        }
                        else
                        {
                            actualTotal = sparePartDetail.FirstTotal.Value;
                        }

                        actualTotalOfSparePart += actualTotal;
                        totalOfSparePart += sparePartDetail.Total;
                    }

                    if (totalOfSparePart == actualTotalOfSparePart)
                    {
                        continue;
                    }

                    InventoryTaskSparePartDiff sparePartDiff = new InventoryTaskSparePartDiff();
                    sparePartDiff.SparePartId = sparePartId;
                    sparePartDiff.Total = totalOfSparePart;
                    sparePartDiff.ActualTotal = actualTotalOfSparePart;
                    sparePartDiff.Diff = actualTotalOfSparePart - totalOfSparePart;
                    sparePartDiff.InventoryTaskId = taskId;

                    if (sparePartDiff.Diff > 0)
                    {
                        sparePartDiff.InventoryResult = InventoryResult.Profit;
                    }
                    else
                    {
                        sparePartDiff.InventoryResult = InventoryResult.Loss;
                    }

                    inventoryTaskSparePartDiffList.Add(sparePartDiff);
                }
            }
        }

        private void GenerateDiffControlByItemCode(List<double> saprePartTaskIds, EntityList<InventoryTaskSparePartDiff> inventoryTaskSparePartDiffList)
        {
            var inventoryTaskSparePartDetails = saprePartTaskIds.SplitContains(tempIds =>
            {
                return Query<InventoryTaskSparePartDetail>()
                    .Where(x => (x.SparePart.ControlMethod == ControlMethod.ItemCode)
                      && tempIds.Contains(x.InventoryTaskId)
                      && (x.SecondResult == InventoryResult.Profit || x.SecondResult == InventoryResult.Loss
                        || x.FirstResult == InventoryResult.Profit || x.FirstResult == InventoryResult.Loss))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            foreach (var sparePartDetail in inventoryTaskSparePartDetails)
            {
                if ((!sparePartDetail.FirstTotal.HasValue && !sparePartDetail.SecondTotal.HasValue) ||
                        (!sparePartDetail.SecondResult.HasValue && !sparePartDetail.FirstResult.HasValue))
                {
                    throw new ValidationException("盘点任务【{0}】备件【{1}】无盘点结果，不能盘点完成"
                        .L10nFormat(sparePartDetail.InventoryTaskNo, sparePartDetail.SparePartCode));
                }

                int actualTotal = 0;

                if (sparePartDetail.SecondTotal.HasValue)
                {
                    actualTotal = sparePartDetail.SecondTotal.Value;
                }
                else
                {
                    actualTotal = sparePartDetail.FirstTotal.Value;
                }

                var inventoryResult = (sparePartDetail.SecondResult ?? sparePartDetail.FirstResult);
                InventoryTaskSparePartDiff sparePartDiff = new InventoryTaskSparePartDiff();
                sparePartDiff.SparePartId = sparePartDetail.SparePartId;
                sparePartDiff.Total = sparePartDetail.Total;
                sparePartDiff.ActualTotal = actualTotal;
                sparePartDiff.Diff = sparePartDiff.ActualTotal - sparePartDiff.Total;
                sparePartDiff.InventoryTaskId = sparePartDetail.InventoryTaskId;
                sparePartDiff.InventoryResult = inventoryResult.Value;
                inventoryTaskSparePartDiffList.Add(sparePartDiff);
            }
        }

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="entityType">打印条码类型</param>
        /// <param name="info">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <returns>打印模板列表</returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplatesByType(string entityType, PagingInfo info = null, string keyword = "")
        {
            var query = Query<PrintTemplate>().Where(p => p.EntityType.Contains(entityType) && p.State == State.Enable);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.FileName.Contains(keyword));
            }
            return query.ToList(info);
        }

        /// <summary>
        /// 查询盘点任务下的备件清单
        /// </summary>
        /// <param name="id">盘点任务下的备件清单ID</param>
        /// <returns></returns>
        public virtual InventoryTaskSparePartDetail GetInventoryTaskSparePartDetailById(double id)
        {
            return RF.GetById<InventoryTaskSparePartDetail>(id, new EagerLoadOptions().LoadWithViewProperty());
        }
    }

    /// <summary>
    /// 备件盘点数据统计
    /// </summary>
    internal class SpareDetailCountData
    {
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 初盘良品数
        /// </summary>
        public int FirstGood { get; set; }

        /// <summary>
        /// 初盘不良品数
        /// </summary>
        public int FirstNg { get; set; }

        /// <summary>
        /// 初盘总数
        /// </summary>
        public int FirstTotal { get; set; }

        /// <summary>
        /// 初盘差异数
        /// </summary>
        public int FirstDiff { get; set; }

        /// <summary>
        /// 复盘良品数
        /// </summary>
        public int SecondGoodQty { get; set; }

        /// <summary>
        /// 复盘不良品数
        /// </summary>
        public int SecondNgQty { get; set; }

        /// <summary>
        /// 复盘总数
        /// </summary>
        public int SecondTotal { get; set; }

        /// <summary>
        /// 复盘差异数
        /// </summary>
        public int SecondDiff { get; set; }
    }

    /// <summary>
    /// 备件明细初盘结果
    /// </summary>
    internal class SpareDetailFirstResult
    {
        /// <summary>
        /// 盘点计划ID
        /// </summary>
        public double InventoryPlanId { get; set; }

        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 初盘结果
        /// </summary>
        public InventoryResult? FirstResult { get; set; }

        /// <summary>
        /// 记录数
        /// </summary>
        public int Count { get; set; }
    }
}