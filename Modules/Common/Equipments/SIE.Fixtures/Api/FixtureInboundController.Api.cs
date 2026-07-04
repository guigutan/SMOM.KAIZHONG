using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.EventMessages.EMS.Purchases;
using SIE.Fixtures.ApiModels;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.Models;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Fixtures
{
    /// <summary>
    /// 工治具入库API控制
    /// </summary>
    public partial class FixtureInboundController : CoreFixtureController
    {
        private const string CODE_AND_NAME_FORMAT = "{0}({1})";

        /// <summary>
        /// 获取入库任务信息列表
        /// </summary>
        /// <param name="queryInfo">入库任务查询信息</param>
        /// <returns>入库任务信息列表</returns>
        [ApiService("获取入库任务信息列表")]
        [return: ApiReturn("获取入库任务信息列表 LaunchTaskDataInfo")]
        public virtual LaunchTaskDataInfo GetPagingLaunchTaskInfos([ApiParameter("入库任务查询信息")] LaunchTaskQueryInfo queryInfo)
        {
            if (queryInfo == null)
            {
                throw new ValidationException("输入参数有误！".L10N());
            }
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var launchTasks = GetLaunchTasks(pagingInfo, queryInfo.Code);
            var taskInfos = CreateLaunchTaskInfos(launchTasks);

            LaunchTaskDataInfo result = new LaunchTaskDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = launchTasks.TotalCount
            };
            result.LaunchTaskInfos.AddRange(taskInfos);
            return result;
        }

        /// <summary>
        /// 根据上架任务单号或者工装Id获取上架任务列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="code">工装Id/上架任务单号</param>
        /// <returns>上架任务列表</returns>
        private EntityList<InboundOrder> GetLaunchTasks(PagingInfo pagingInfo, string code)
        {
            if (code.IsNullOrEmpty())
            {
                return Query<InboundOrder>().Where(p => p.InboundStatus != InboundStatus.Done &&
                (p.MaintainTask.State == MaintainTasks.MaintainState.Finish || p.MaintainTaskId == null)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                var tasks = GetLaunchTasksByIDCode(pagingInfo, code);
                if (tasks.Any())
                {
                    return tasks;
                }
                else
                {
                    var res = GetLaunchTasksByNo(pagingInfo, code);
                    if (!res.Any())
                    {
                        return GetLaunchTasksByEncodeCode(pagingInfo, code);
                    }
                    return res;
                }
            }
        }

        /// <summary>
        /// 根据上架任务单号获取上架任务列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="no">上架任务单号</param>
        /// <returns>上架任务列表</returns>
        public virtual EntityList<InboundOrder> GetLaunchTasksByNo(PagingInfo pagingInfo, string no)
        {
            return Query<InboundOrder>().Where(p => p.InboundStatus != InboundStatus.Done &&
            (p.MaintainTask.State == MaintainTasks.MaintainState.Finish || p.MaintainTaskId == null) && p.No == no).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据ID编码获取上架任务列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="code">ID编码</param>
        /// <returns>上架任务列表</returns>
        public virtual EntityList<InboundOrder> GetLaunchTasksByIDCode(PagingInfo pagingInfo, string code)
        {
            return Query<InboundOrder>().Where(p => p.InboundStatus != InboundStatus.Done
            && (p.MaintainTask.State == MaintainTasks.MaintainState.Finish || p.MaintainTaskId == null))
                .Exists<FixtureAccount>((a, b) => b.Where(f => f.FixtureEncodeId == a.FixtureEncodeId && (f.Code == code || f.Rfid == code))).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务号根据工治具编码
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual EntityList<InboundOrder> GetLaunchTasksByEncodeCode(PagingInfo pagingInfo, string code)
        {
            return Query<InboundOrder>().Where(p => p.InboundStatus != InboundStatus.Done
            && (p.MaintainTask.State == MaintainTasks.MaintainState.Finish || p.MaintainTaskId == null))
                .Exists<FixtureEncode>((a, b) => b.Where(f => f.Id == a.FixtureEncodeId && (f.Code == code))).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 创建入库任务信息列表
        /// </summary>
        /// <param name="launchTasks">入库任务列表</param>
        /// <returns>入库任务信息列表</returns>
        private List<LaunchTaskInfo> CreateLaunchTaskInfos(EntityList<InboundOrder> launchTasks)
        {
            List<LaunchTaskInfo> launchTaskInfos = new List<LaunchTaskInfo>();
            if (!launchTasks.Any())
            {
                return launchTaskInfos;
            }
            var taskInfos = new List<LaunchTaskInfo>();
            var ids = launchTasks.Select(m => m.Id).ToList();
            var fixtureEncodeId = launchTasks.Select(m => m.FixtureEncodeId).ToList();
            var fixtureEncodes = Query<FixtureEncode>().Where(m => fixtureEncodeId.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var numberDetail = Query<InboundOrderFixtureIdAccount>().
                Join<InboundOrder>((x, y) => x.InboundOrderId == y.Id && ids.Contains(y.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var codeDetail = Query<InboundOrderFixtureCodeAccount>().
                Join<InboundOrder>((x, y) => x.InboundOrderId == y.Id && ids.Contains(y.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());


            //数量（显示治具明细未出库的数量和，即入库数减去未维护库位的治具数）
            foreach (var task in launchTasks)
            {
                var inboundQty = (int)task.Qty;
                var taskFixtureEncode = fixtureEncodes.FirstOrDefault(m => m.Id == task.FixtureEncodeId);
                if (taskFixtureEncode == null)
                { continue; }
                if (taskFixtureEncode.ManageMode == ManageMode.Code)
                {
                    inboundQty -= (int)codeDetail.Where(m => m.InboundOrderId == task.Id && m.StorageLocationId.HasValue).Sum(m => m.Qty);
                }
                if (taskFixtureEncode.ManageMode == ManageMode.Number)
                {
                    inboundQty -= (int)numberDetail.Where(m => m.InboundOrderId == task.Id && m.StorageLocationId.HasValue).Sum(m => m.Qty);
                }

                var taskInfo = new LaunchTaskInfo()
                {
                    No = task.No,
                    Code = taskFixtureEncode.Code,
                    ManageMode = taskFixtureEncode.ManageMode.ToLabel().L10N(),
                    ManageModeValue = (int)taskFixtureEncode.ManageMode,
                    FixedStorage = taskFixtureEncode.FixedStorage == YesNo.Yes ? "是".L10N() : "否".L10N(),
                    EncodeCode = taskFixtureEncode.Code,
                    ModelName = taskFixtureEncode.ModelName,
                    BusinessType = task.InboundType.ToLabel(),
                    Qty = inboundQty,
                    LaunchTaskQty = (int)task.Qty,
                    InboundQty = (int)task.Qty - inboundQty,
                    CreateDate = task.CreateDate,
                    FixtureType = taskFixtureEncode.FixtureType,
                    Warehouse = task.WarehouseId.HasValue ? task.Warehouse.Code : "",
                    WarehouseId = task.WarehouseId,
                    MaintainStatus = task.MaintainStatus == MaintainTasks.MaintainState.Wait ? task.MaintainStatus.ToLabel() : ""
                };
                var resDetails = CreateLaunchTaskInfoDetail(taskFixtureEncode.ManageMode, taskInfo, task.Id, numberDetail);
                if (resDetails.Any())
                {
                    taskInfo.Details = resDetails;
                }
                taskInfos.Add(taskInfo);
            }
            return taskInfos;
        }

        /// <summary>
        /// 创建入库明细
        /// </summary>
        /// <param name="manageMode"></param>
        /// <param name="launchTaskInfo"></param>
        /// <param name="taskId"></param>
        /// <param name="idDetails"></param>
        /// <returns></returns>
        private List<LaunchTaskInfoDetail> CreateLaunchTaskInfoDetail(ManageMode manageMode, LaunchTaskInfo launchTaskInfo, double taskId,
            EntityList<InboundOrderFixtureIdAccount> idDetails)
        {
            List<LaunchTaskInfoDetail> launchTaskInfoDetails = new List<LaunchTaskInfoDetail>();

            if (manageMode == ManageMode.Number)
            {
                var matchedNumbers = idDetails.Where(m => m.InboundOrderId == taskId).ToList();
                foreach (var item in matchedNumbers)
                {
                    //if (!item.StorageLocationId.HasValue)
                    //{
                    LaunchTaskInfoDetail launchTaskInfoDetail = new LaunchTaskInfoDetail()
                    {
                        Id = item.Id,
                        IDCode = item.IDCode,
                        FixtureType = launchTaskInfo.FixtureType,
                        Location = item.StorageLocationId.HasValue ? string.Format(CODE_AND_NAME_FORMAT, item.StorageLocation.Code, item.StorageLocation.Name) : "",
                        LocaltionId = item.StorageLocationId.HasValue ? item.StorageLocationId.Value : 0,
                        Qty = (int)item.Qty,
                        IsSelect = item.StorageLocationId.HasValue,
                        RFID = item.FixtureIDAccount != null ? item.FixtureIDAccount.Rfid : ""
                    };
                    launchTaskInfoDetails.Add(launchTaskInfoDetail);
                    //}
                }
            }

            return launchTaskInfoDetails;


        }

        /// <summary>
        /// 根据工治具ID和库位编码获取仓库库位信息 
        /// </summary>
        /// <param name="code">工治具ID</param>
        /// <param name="locationCode">库位编码</param>
        /// <param name="warehouseId"></param>
        /// <returns>仓库库位信息</returns>
        [ApiService("获取仓库库位信息列表")]
        [return: ApiReturn("获取仓库库位信息列表 GetPagingWareLocationInfos")]
        public virtual WareLocationInfo GetPagingWareLocationInfos([ApiParameter("工治具ID")] string code, [ApiParameter("库位编码")] string locationCode, [ApiParameter("仓库Id")] double warehouseId)
        {
            if (!code.IsNotEmpty())
            {
                throw new ValidationException("输入/扫描的工治具ID不能为空！".L10N());
            }

            if (!locationCode.IsNotEmpty())
            {
                throw new ValidationException("输入/扫描的库位编码不能为空！".L10N());
            }
            var account = this.GetFixtureAccountByCodeOrRFID(code);

            if (account != null && account.ManageMode == ManageMode.Number && account.FixedStorage == YesNo.Yes && account.LocationCode != locationCode)
            {
                throw new ValidationException("ID类工治具台帐，固定储位的仓库[{0}]和输入/扫描的库位[{1}]不一致，请先确认！".L10nFormat(account.LocationCode, locationCode));
            }
            var encodeStorageLocs = GetFixtureEncodeStorageLocations(code);

            var result = new WareLocationInfo();
            if (encodeStorageLocs.Any())
            {
                var encodeStorageLoc = encodeStorageLocs.FirstOrDefault(p => p.FixtureStorageLocationCode == locationCode&&p.WarehouseId== warehouseId);
                if (encodeStorageLoc == null)
                {
                    throw new ValidationException("工治具台帐的工治具编码下有维护存储位置，输入/扫描的库位不存在,或对应仓库下不存在，请先确认！".L10N());
                }
                result.LocationId = encodeStorageLoc.StorageLocationId;
                result.Location = CODE_AND_NAME_FORMAT.L10nFormat(encodeStorageLoc.FixtureStorageLocationCode, encodeStorageLoc.FixtureStorageLocationName);

            }
            else
            {
                var location = this.GetStorageLocation(locationCode, warehouseId);
                if (location == null)
                { throw new ValidationException("扫描库位有误，请扫描入库单指定仓库下的库位编码！".L10nFormat(locationCode)); }
                result.LocationId = location.Id;
                result.Location = CODE_AND_NAME_FORMAT.L10nFormat(location.Code, location.Name);
            }

            return result;
        }

        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <param name="queryInfo">查询信息</param>
        /// <returns>仓库列表</returns>
        [ApiService("获取仓库列表")]
        [return: ApiReturn("获取仓库列表 GetPagingWarehouseInfos")]
        public virtual WarehouseDataInfo GetPagingWarehouseInfos([ApiParameter("仓库查询信息")] WarehouseQueryInfo queryInfo)
        {
            if (queryInfo == null)
            {
                throw new ValidationException("参数有误！".L10N());
            }
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            if (!queryInfo.Code.IsNotEmpty())
                throw new ValidationException("输入/扫描的工治具ID不能为空！".L10N());
            WarehouseDataInfo result = new WarehouseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize
            };

            EntityList<Warehouse> warehouses = null;
            var isExist = this.IsExistEncodeStorageLocation(queryInfo.Code);
            if (isExist)
            { warehouses = this.GetFixtureWarehouses(queryInfo.Keyword, queryInfo.Code, pagingInfo); }
            else
            { warehouses = this.GetFixtureWarehouses(queryInfo.Keyword, pagingInfo); }
            result.TotalCount = warehouses.TotalCount;

            warehouses.ForEach(ware =>
            {
                var warehouseInfo = new ApiModels.WarehouseInfo()
                {
                    WarehouseId = ware.Id,
                    Warehouse = CODE_AND_NAME_FORMAT.L10nFormat(ware.Code, ware.Name),
                    Code = ware.Code,
                };

                result.WarehouseInfos.Add(warehouseInfo);
            });

            return result;
        }

        /// <summary>
        /// 获取库位列表
        /// </summary>
        /// <param name="queryInfo">库位查询信息</param>
        /// <returns>库位列表</returns>
        [ApiService("获取库位列表")]
        [return: ApiReturn("获取库位列表 GetPagingLocationInfos")]
        public virtual LocationDataInfo GetPagingLocationInfos([ApiParameter("库位查询信息")] LocationQueryInfo queryInfo)
        {
            if (queryInfo == null)
            {
                throw new ValidationException("输入有误！".L10N());
            }
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            if (!queryInfo.Code.IsNotEmpty())
            {
                throw new ValidationException("输入/扫描的工治具ID不能为空！".L10N());
            }
            if (queryInfo.WarehouseId <= 0)
            {
                throw new ValidationException("仓库未选择，请先选择仓库！".L10N());
            }
            EntityList<StorageLocation> locations = null;
            var isExist = IsExistEncodeStorageLocation(queryInfo.Code);
            if (isExist)
            {
                locations = GetEncodeStorageLocations(queryInfo.Keyword, queryInfo.Code, queryInfo.WarehouseId, pagingInfo);
            }
            else
            {
                locations = GetFixtureStorageLocationsByWarehouseId(queryInfo.Keyword, queryInfo.WarehouseId, pagingInfo);
            }
            LocationDataInfo result = new LocationDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = locations.TotalCount
            };

            locations.ForEach(lo =>
            {
                var locationInfo = new LocationInfo()
                {
                    LocationId = lo.Id,
                    Location = CODE_AND_NAME_FORMAT.L10nFormat(lo.Code, lo.Name),
                    Code = lo.Code
                };

                result.LocationInfos.Add(locationInfo);
            });

            return result;
        }

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="launchTaskInfo">入库任务信息</param>
        /// <param name="launchTaskInfoDetails"></param>
        [ApiService("提交入库任务")]
        [return: ApiReturn("提交入库任务 SubmitLaunchTaskInfos")]
        public virtual void SubmitLaunchTaskInfos([ApiParameter("入库任务信息")] LaunchTaskInfo launchTaskInfo, [ApiParameter("入库任务明细")] List<LaunchTaskInfoDetail> launchTaskInfoDetails)
        {
            if (launchTaskInfoDetails == null)
            {
                throw new ValidationException("入库明细为空，请检查".L10N());
            }
            var task = Query<InboundOrder>().Where(p => p.No == launchTaskInfo.No).FirstOrDefault(new EagerLoadOptions().LoadWith(InboundOrder.InboundOrderFixtureIdAccountListProperty));
            if (task == null)
            {
                throw new ValidationException("入库任务[{0}]不存在！".L10nFormat(launchTaskInfo.No));
            }
            if (task.QualityState == null)
            {
                throw new ValidationException("入库任务[{0}]的质量状态必填！".L10nFormat(launchTaskInfo.No));
            }
            if (!launchTaskInfo.WarehouseId.HasValue)
            {
                throw new ValidationException("入库任务[{0}]的仓库必填！".L10nFormat(launchTaskInfo.No));
            }

            task.WarehouseId = launchTaskInfo.WarehouseId;//更新仓库
            EntityList<InboundOrderFixtureIdAccount> inboundOrderFixtureIdAccounts = new EntityList<InboundOrderFixtureIdAccount>();
            EntityList<InboundOrderFixtureCodeAccount> inboundOrderFixtureCodeAccounts = new EntityList<InboundOrderFixtureCodeAccount>();
            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                if (launchTaskInfo.ManageModeValue == (int)ManageMode.Number)//Id类的获取明细
                {
                    var detailsId = launchTaskInfoDetails.Where(m => m.IsSelect).Select(m => m.Id);
                    inboundOrderFixtureIdAccounts = Query<InboundOrderFixtureIdAccount>().Where(m => detailsId.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    if (!inboundOrderFixtureIdAccounts.Any())
                    {
                        throw new ValidationException("未找到入库任务[{0}]的明细，请确认！".L10nFormat(launchTaskInfo.No));
                    }
                    var launchedQty = launchTaskInfoDetails.Where(m => m.LocaltionId != 0).Sum(m => m.Qty);
                    if (task.Qty != launchedQty)
                    {
                        throw new ValidationException("本次提交入库数不等于入库单入库数，请确认".L10N());
                    }
                    var summitIds = inboundOrderFixtureIdAccounts.Select(m => m.Id).ToList();
                    var exsitedQty = task.InboundOrderFixtureIdAccountList.Where(m => m.StorageLocationId.HasValue && !summitIds.Contains(m.Id)).Sum(m => m.Qty);
                    if (task.Qty < launchedQty + exsitedQty)
                    {
                        throw new ValidationException("入库任务[{0}]的入库数量[{1}]大于实际可入库数量[{2}]，请刷新数据后，再入库！".L10nFormat(launchTaskInfo.No,launchTaskInfo.LaunchTaskQty, launchedQty));
                    }

                    var accountIds = inboundOrderFixtureIdAccounts.Select(m => m.FixtureIDAccountId).ToList();
                    var accountList = accountIds.SplitContains(tempIds =>
                    {
                        return Query<FixtureAccount>().Where(m => tempIds.Contains(m.Id)).ToList();
                    });

                    foreach (var item in inboundOrderFixtureIdAccounts)//更新库位
                    {
                        var detailIndx = launchTaskInfoDetails.FindIndex(m => m.Id == item.Id);
                        if (detailIndx < 0)
                        {
                            continue;
                        }
                        if (item.MaintainTaskId.HasValue&& item.MaintainState != MaintainTasks.MaintainState.Finish)
                        {
                            throw new ValidationException("存在待保养的入库明细,请先保养完成再提交！".L10N());
                        }

                        var account = accountList.FirstOrDefault(m => m.Id == item.FixtureIDAccountId);

                        if (account != null && launchTaskInfoDetails[detailIndx].LocaltionId != 0)
                        {
                            item.StorageLocationId = launchTaskInfoDetails[detailIndx].LocaltionId;
                            ValidateWarehouseLocation(launchTaskInfo, account, launchTaskInfoDetails[detailIndx]);
                            UpdateAccountStock(task, account, (int)item.Qty, item.StorageLocationId.Value);
                            UpdateAccountOfLaunchTask(task, account, (int)item.Qty);
                        }

                        CreateLaunchTaskRecordForID(task, account, (int)item.Qty, item.StorageLocationId);
                    }
                    task.InboundStatus = (task.Qty == launchedQty + exsitedQty) ? InboundStatus.Done : InboundStatus.Doing;
                }

                List<PurchasesUpdateInfo> updateInfos = new List<PurchasesUpdateInfo>();
                //编码类
                if (launchTaskInfo.ManageModeValue == (int)ManageMode.Code)//编码类的只会新增
                {
                    var launchedQty = launchTaskInfoDetails.Sum(p => p.Qty);
                    if (task.Qty != launchedQty)
                    {
                        throw new ValidationException("本次提交入库数不等于入库单入库数，请确认".L10N());
                    }
                    var exsitedDetail = Query<InboundOrderFixtureCodeAccount>().Where(m => m.InboundOrderId == task.Id && m.StorageLocationId != null).ToList();
                    var exsitedQty = exsitedDetail.Sum(p => p.Qty);

                    if (task.Qty < launchedQty + exsitedQty)
                    {
                        throw new ValidationException("入库任务[{0}]的入库数量[{1}]大于实际可入库数量[{2}]，请刷新数据后，再入库！".L10nFormat(task.No,launchedQty, task.Qty - exsitedQty));
                    }
                    inboundOrderFixtureCodeAccounts = GetInboundOrderFixtureCodeAccount(task, launchedQty, launchTaskInfoDetails);
                    DB.Delete<InboundOrderFixtureCodeAccount>().Where(m => m.InboundOrderId == task.Id && m.StorageLocationId == null).Execute();

                    task.InboundStatus = (task.Qty == launchedQty + exsitedQty) ? InboundStatus.Done : InboundStatus.Doing;
                }
                if (inboundOrderFixtureCodeAccounts.Any())
                {
                    RF.Save(inboundOrderFixtureCodeAccounts);
                }
                if (inboundOrderFixtureIdAccounts.Any())
                {
                    RF.Save(inboundOrderFixtureIdAccounts);
                }
                RF.Save(task);
                UpdatePoItemQty(task, updateInfos);
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新采购明细的入库数量和采购订单状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="updateInfos"></param>
        private void UpdatePoItemQty(InboundOrder task, List<PurchasesUpdateInfo> updateInfos)
        {
            if (task.InboundType == FixtureInboundType.Po)//回写采购订单的数据
            {

                foreach (var detail in task.InboundOrderFixtureIdAccountList)
                {
                    updateInfos.Add(new PurchasesUpdateInfo()
                    {
                        AccNo = task.AcceptanceOrderNo,
                        PoNo = detail.PoNo,
                        PoNoLineNo = !detail.PoLineNo.IsNullOrEmpty() ? int.Parse(detail.PoLineNo) : 0,
                        InboundQty = (int)detail.Qty,
                        RecNo = task.ReceiptOrderNo
                    });
                }
                foreach (var detail in task.InboundOrderPurchaseList)
                {
                    updateInfos.Add(new PurchasesUpdateInfo()
                    {
                        AccNo = task.AcceptanceOrderNo,
                        PoNo = detail.PoNo,
                        PoNoLineNo = !detail.PoLine.IsNullOrEmpty() ? int.Parse(detail.PoLine) : 0,
                        InboundQty = (int)detail.Qty,
                        RecNo = task.ReceiptOrderNo
                    });
                }
                if (updateInfos.Any())
                {
                    RT.Service.Resolve<IPurchases>().UpdatePurchasesInbound(updateInfos);
                }
            }
        }

        /// <summary>
        /// 获取编码类明细
        /// </summary>
        /// <param name="task">任务主表</param>
        /// <param name="launchedQty"></param>
        /// <param name="launchTaskInfoDetails"></param>
        /// <returns></returns>
        public virtual EntityList<InboundOrderFixtureCodeAccount> GetInboundOrderFixtureCodeAccount(InboundOrder task, int launchedQty, List<LaunchTaskInfoDetail> launchTaskInfoDetails)
        {
            EntityList<InboundOrderFixtureCodeAccount> inboundOrderFixtureCodeAccounts = new EntityList<InboundOrderFixtureCodeAccount>();
            var dbTime = RF.Find<InboundOrder>().GetDbTime();
            var account = Query<FixtureAccount>().Where(m => m.FixtureEncodeId == task.FixtureEncodeId).FirstOrDefault();
            foreach (var item in launchTaskInfoDetails)
            {
                if (item.LocaltionId == 0)
                {
                    throw new ValidationException("存在明细未填写库位,请检查！".L10N());
                }
                CreateLaunchTaskRecordForCode(task, item.Qty, item.LocaltionId);
                var taskCodeDetail = new InboundOrderFixtureCodeAccount()
                {
                    StorageLocationId = item.LocaltionId,
                    Qty = item.Qty,
                    InboundOrderId = task.Id,
                    CreateDate = dbTime
                };

                inboundOrderFixtureCodeAccounts.Add(taskCodeDetail);
                UpdateAccountStock(task, account, item.Qty, item.LocaltionId);
            }
            //创建使用履历
            CreateSaveAccountUseResume(account.Id, null, null, UseResumeType.Shelf, launchedQty);
            UpdateAccountOfLaunchTask(task, account, launchedQty);
            return inboundOrderFixtureCodeAccounts;
        }

        /// <summary>
        /// 验证工治具仓库库位信息是否合法
        /// </summary>
        /// <param name="launchTaskInfo">入库任务信息</param>
        /// <param name="account">工治具台帐</param>
        /// <param name="launchTaskInfoDetail"></param>
        private void ValidateWarehouseLocation(LaunchTaskInfo launchTaskInfo, FixtureAccount account, LaunchTaskInfoDetail launchTaskInfoDetail)
        {
            if (account.ManageMode == ManageMode.Number && account.FixedStorage == YesNo.Yes)
            {
                if (launchTaskInfo.WarehouseId != account.WarehouseId)
                {
                    throw new ValidationException("入库任务[{0}]的仓库[{1}]与固定储位的ID类台帐的仓库[{2}]不一致！".L10nFormat(launchTaskInfo.No, launchTaskInfo.Warehouse, account.WarehouseCode));
                }
                if (launchTaskInfoDetail.LocaltionId != account.LocationId)
                {
                    throw new ValidationException("入库任务[{0}]的库位[{1}]与固定储位的ID类台帐的库位[{2}]不一致！".L10nFormat(launchTaskInfo.No, launchTaskInfo.Location, account.LocationCode));
                }
            }
            else
            {
                var isExist = IsExistEncodeStorageLocation(account.Code);
                if (isExist)
                {
                    if (IsExistWarehouseLocation(account.Id, launchTaskInfo.WarehouseId.Value, launchTaskInfoDetail.LocaltionId))
                    {
                        throw new ValidationException("入库任务[{0}]的仓库[{1}]和库位[{2}]在工治具台帐的工治具编码的存储位置下不存在，请先维护！".L10nFormat(launchTaskInfo.No, launchTaskInfo.Warehouse, launchTaskInfoDetail.Location));
                    }
                }
                else
                {
                    if (IsExistWarehouseLocation(launchTaskInfo.WarehouseId.Value, launchTaskInfoDetail.LocaltionId))
                    {
                        throw new ValidationException("入库任务[{0}]的仓库[{1}]和库位[{2}]在工治具仓库下不存在，请先维护！".L10nFormat(launchTaskInfo.No, launchTaskInfo.Warehouse, launchTaskInfoDetail.Location));
                    }
                }
            }
        }

        /// <summary>
        /// 创建或更新库存详情
        /// </summary>
        /// <param name="task"></param>
        /// <param name="account"></param>
        /// <param name="qty"></param>
        /// <param name="storageLocationId"></param>
        private void UpdateAccountStock(InboundOrder task, FixtureAccount account, int qty, double storageLocationId)
        {
            var accountStock = GetFixtureAccountStock(account.Id, task.WarehouseId.Value, storageLocationId);
            if (accountStock == null)
            {
                accountStock = CreateAccountStock(account, task.WarehouseId.Value, storageLocationId);
            }
            SaveAccountStock(task, account, qty, accountStock);
        }


        /// <summary>
        /// 保存库存详情
        /// </summary>
        /// <param name="task"></param>
        /// <param name="account"></param>
        /// <param name="qty"></param>
        /// <param name="accountStock"></param>
        private void SaveAccountStock(InboundOrder task, FixtureAccount account, int qty, FixtureAccountStock accountStock)
        {
            accountStock.TotalQty += qty;
            if (task.QualityState == FixtureQualityState.Pass)
            {
                accountStock.PassQty += qty;
            }
            else
            {
                accountStock.NgQty += qty;
            }
            RF.Save(accountStock);
            RF.Save(account);
        }


        /// <summary>
        /// 创建库存详情
        /// </summary>
        /// <param name="account"></param>
        /// <param name="fixtureWarehouseId"></param>
        /// <param name="fixtureStorageLocationId"></param>
        /// <returns></returns>
        private FixtureAccountStock CreateAccountStock(FixtureAccount account, double fixtureWarehouseId, double fixtureStorageLocationId)
        {
            return new FixtureAccountStock()
            {
                FixtureAccountId = account.Id,
                FixtureWarehouseId = fixtureWarehouseId,
                FixtureStorageLocationId = fixtureStorageLocationId,
            };
        }

        /// <summary>
        /// 更新工治具台账
        /// </summary>
        /// <param name="task">入库任务</param>
        /// <param name="account">工治具台账</param>
        /// <param name="qty">数量</param>
        private void UpdateAccountOfLaunchTask(InboundOrder task, FixtureAccount account, int qty)
        {
            account.InStockQty += qty;
            account.WaitShelfQty -= qty;
            if (account.FixtureEncode.FixtureModel.ManageMode == ManageMode.Number)
            {
                account.AccountState = FixtureAccountState.InStorage;
            }
            RF.Save(account);
        }

        /// <summary>
        /// 创建工治具出入库记录
        /// </summary>
        /// <param name="task"></param>
        /// <param name="qty"></param>
        /// <param name="storageLocationId"></param>
        private void CreateLaunchTaskRecordForCode(InboundOrder task, int qty, double? storageLocationId)
        {
            var dbTime = RF.Find<FixtureAccount>().GetDbTime();
            var encode = RF.GetById<FixtureEncode>(task.FixtureEncodeId);
            if (encode == null)
            {
                throw new ValidationException("提交的工治具编码,工治具编码中不存在！".L10N());
            }
            var encodeCode = encode.Code;
            var account = Query<FixtureCodeAccount>().Where(m => m.Code == encodeCode).FirstOrDefault();
            if (account == null)
            {
                throw new ValidationException("提交的工治具编码,工治具台账中不存在！".L10N());
            }
            var record = new FixtureRecord();
            record.Code = task.No;
            record.Qty = qty;
            record.ApplyById = task.CreateBy;
            record.ApplyDate = task.CreateDate;
            record.ComplyById = RT.IdentityId;
            record.ComplyDate = dbTime;
            record.FixtureAccountId = account.Id;
            record.BusinessType = (BusinessType)(int)task.InboundType;
            record.FixtureWarehouseId = task.WarehouseId;
            record.FixtureStorageLocationId = storageLocationId;
            record.RecordType = RecordType.In;
            //account.QualityState = task.QualityState; 2022/6/29 俊杰要求不更新质量状态
            RF.Save(record);
            RF.Save(account);

        }

        /// <summary>
        ///创建ID类工治具出入库明细
        /// </summary>
        /// <param name="task"></param>
        /// <param name="account"></param>
        /// <param name="qty"></param>
        /// <param name="storageLocationId"></param>
        private void CreateLaunchTaskRecordForID(InboundOrder task, FixtureAccount account, int qty, double? storageLocationId)
        {
            var dbTime = RF.Find<FixtureAccount>().GetDbTime();
            if (account == null)
            {
                throw new ValidationException("提交的工治具编码,工治具台账中不存在！".L10N());
            }
            var record = new FixtureRecord();
            record.Code = task.No;
            record.Qty = qty;
            record.ApplyById = task.CreateBy;
            record.ApplyDate = task.CreateDate;
            record.ComplyById = RT.IdentityId;
            record.ComplyDate = dbTime;
            record.FixtureAccountId = account.Id;
            record.BusinessType = (BusinessType)(int)task.InboundType;
            record.FixtureWarehouseId = task.WarehouseId;
            record.FixtureStorageLocationId = storageLocationId;
            record.RecordType = RecordType.In;
            //account.QualityState = task.QualityState; 2022/6/29 俊杰要求不更新质量状态
            if (!account.InStorageDate.HasValue)
            {
                account.InStorageDate = dbTime;
            }
            RF.Save(record);
            RF.Save(account);
        }

    }
}
