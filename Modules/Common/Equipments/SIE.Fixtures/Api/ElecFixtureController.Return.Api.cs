using SIE.Api;
using SIE.Common.Configs;
using SIE.Core.ApiModels;
using SIE.Core.Common.Controllers;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.EventMessages.EMS.Fixtures;
using SIE.Fixtures.ApiModels;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Models;
using SIE.Fixtures.Models.Config;
using SIE.Resources.WipResources;
using SIE.TurnoverTools.TurnoverTools;
using SIE.Utils;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    public partial class ElecFixtureController : CoreFixtureController
    {
        #region 工治具治具归还接口

        /// <summary>
        /// 获取可归还产线列表
        /// </summary>
        /// <returns>可归还产线列表</returns>
        [ApiService("获取可归还产线列表")]
        [return: ApiReturn("可归还产线列表 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetReturnResourceInfo()
        {
            var wipResources = GetReturnWipResources();
            var infos = new List<BaseDataInfo>();
            wipResources.ForEach(wipResource =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = wipResource.Id,
                    Code = wipResource.Code,
                    Name = wipResource.Name,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = 1,
                PageSize = wipResources.Count(),
                TotalCount = wipResources.Count()
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取可归还产线列表
        /// </summary>
        /// <returns>可归还产线列表</returns>
        private EntityList<WipResource> GetReturnWipResources()
        {
            var workOrders = Query<WipResource>().Join<FixtureDemand>((a, b) => a.Id == b.ResourceId).Distinct().ToList();
            return workOrders;
        }

        /// <summary>
        /// 获取可归还工单列表
        /// </summary>
        /// <returns>可归还工单列表</returns>
        [ApiService("获取可归还工单列表")]
        [return: ApiReturn("可归还工单列表 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetReturnWorkOrderInfo([ApiParameter("产线ID")] double resourceId)
        {
            var workOrders = GetReturnWorkOrders(resourceId);
            var infos = new List<BaseDataInfo>();
            workOrders.ForEach(workOrder =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workOrder.Id,
                    Code = workOrder.No,
                    Name = workOrder.No,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = 1,
                PageSize = workOrders.Count(),
                TotalCount = workOrders.Count()
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <returns></returns>
        [ApiService("获取仓库列表")]
        [return: ApiReturn("仓库列表 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetReturnWareHouseInfo()
        {
            var wareHouses = GetReturnWareHouse();
            var infos = new List<BaseDataInfo>();
            wareHouses.ForEach(workOrder =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = workOrder.Id,
                    Code = workOrder.Code,
                    Name = workOrder.Name,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = 1,
                PageSize = wareHouses.Count(),
                TotalCount = wareHouses.Count()
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 获取归还仓库
        /// </summary>
        /// <returns></returns>
        private EntityList<Warehouse> GetReturnWareHouse()
        {
            var configValue = ConfigService.GetConfig<FixtureEncodeConfigValue>(new Models.Config.FixtureEncodeConfig(), typeof(FixtureEncode));
            if (configValue == null)
            {
                throw new ValidationException("未找到工治具编码仓库分类配置项,请检查配置".L10N());
            }
            var typeIds = configValue.WareHouseTypeCode.Split(new string[] { ","},StringSplitOptions.RemoveEmptyEntries).ToList();
            return Query<Warehouse>().WhereIf(typeIds.Any(), m => typeIds.Contains(m.Category)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取默认工单仓库
        /// </summary>
        /// <returns></returns>
        [ApiService("获取工单默认仓库")]
        [return: ApiReturn("工单默认仓库 BaseDataInfo")]
        public virtual BaseDataInfo GetDefultWorkOrderWarehouse(double workOrderId)
        {
            var demand = Query<FixtureDemand>().Where(m => m.WorkOrderId == workOrderId).FirstOrDefault(new EagerLoadOptions().LoadWith(FixtureDemand.UnloadListProperty));
            if (demand != null && demand.UnloadList.Any())
            {
                var warehouse = demand.UnloadList.First();
                return new BaseDataInfo()
                {
                    Id = warehouse.WarehouseId,
                    Code = warehouse.Warehouse.Code,
                    Name = warehouse.Warehouse.Name,
                };
            }
            return null;
        }

        /// <summary>
        /// 获取可归还工单列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <returns>可归还工单列表</returns>
        private EntityList<WorkOrder> GetReturnWorkOrders(double resourceId)
        {
            var workOrders = Query<WorkOrder>().Join<FixtureDemand>((a, b) => a.Id == b.WorkOrderId && b.ResourceId == resourceId)
                .Join<FixtureDemand, FixtureUnload>((b, c) => b.Id == c.FixtureDemandId && c.State == ReceiveState.Finish && c.ReturnQty < c.UnloadQty)
                .Distinct().ToList();
            return workOrders;
        }

        /// <summary>
        /// 验证工治具归还ID编码
        /// </summary>
        /// <param name="code">ID编码</param>
        /// <param name="resourceId">产线ID</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>归还ID编码信息</returns>
        [ApiService("验证工治具归还ID编码")]
        [return: ApiReturn("归还ID编码信息  ReturnIDCodeInfo")]
        public virtual ReturnIDCodeInfo ValidateReturnIDCode([ApiParameter("ID编码")] string code, [ApiParameter("产线ID")] double? resourceId, [ApiParameter("工单ID")] double? workOrderId)
        {
            var returnIDCodeInfo = new ReturnIDCodeInfo();
            returnIDCodeInfo.IsSuccess = false;
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
            {
                returnIDCodeInfo.Message = "无此ID/编码/RFID".L10N();
                return returnIDCodeInfo;
            }
            if (account.ManageMode == ManageMode.Number && account.AccountState != FixtureAccountState.Online && account.AccountState != FixtureAccountState.Using)
            {
                returnIDCodeInfo.Message = "工治具ID[{0}]状态为[{1}],不可操作归还".L10nFormat(account.Code, account.AccountState.ToLabel().L10N());
                return returnIDCodeInfo;
            }
            if (account.ManageMode == ManageMode.Code)//编码管理
            {
                if (resourceId == null || workOrderId == null)
                {
                    returnIDCodeInfo.Message = "编码类工治具必须选择产线和工单".L10N();
                    return returnIDCodeInfo;
                }
                if (account.OnlineQty == 0)
                {
                    returnIDCodeInfo.Message = "工治具编码[{0}]没有在线数量,不可操作归还".L10nFormat(account.Code);
                    return returnIDCodeInfo;
                }
                var unloads = GetReturnUnloads(workOrderId.Value, resourceId.Value, account.Id);
                if (!unloads.Any())
                {
                    returnIDCodeInfo.Message = "工治具编码[{0}]无可归还的出库明细信息,不可操作归还".L10nFormat(account.Code);
                    return returnIDCodeInfo;
                }
                returnIDCodeInfo.Qty = unloads.Sum(p => p.UnloadQty) - unloads.Sum(p => p.ReturnQty);
            }
            else //ID管理
            {
                var demand = GetReturnUnloadsByAccountId(account.Id);
                if (demand == null)
                {
                    throw new ValidationException("找不到此工治具未归还的工治具需求单！".L10N());
                }
                if (resourceId != null && resourceId != demand.ResourceId)
                {
                    throw new ValidationException("所选产线与工治具已领用产线不一致".L10N());
                }
                if (workOrderId != null && workOrderId != demand.WorkOrderId)
                {
                    throw new ValidationException("所选工单与工治具已领用工单不一致！".L10N());
                }
                returnIDCodeInfo.WorkOrderId = demand.WorkOrderId;
                returnIDCodeInfo.WorkOrderNo = demand.WorkOrderNo;
                returnIDCodeInfo.ResourceId = demand.ResourceId;
                returnIDCodeInfo.ResourceName = demand.ResourceName;
                returnIDCodeInfo.Qty = 1;
            }
            returnIDCodeInfo.IsSuccess = true;
            returnIDCodeInfo.ManageMode = account.ManageMode.ToLabel();
            returnIDCodeInfo.Code = account.Code;
            returnIDCodeInfo.EncodeCode = account.EncodeCode;
            returnIDCodeInfo.ModelName = account.ModelName;
            returnIDCodeInfo.FixtureType = account.FixtureType == null ? "" : account.FixtureType.Code;
            return returnIDCodeInfo;
        }

        /// <summary>
        /// 提交工治具归还
        /// </summary>
        /// <param name="resultDataInfo">工治具归还信息</param>
        [ApiService("提交工治具归还")]
        public virtual void SubmitReturnInfo([ApiParameter("工治具归还信息")] ReturnInfo resultDataInfo)
        {
            if (resultDataInfo == null)
            {
                throw new ValidationException("提交的数据异常！".L10N());
            }
            if (resultDataInfo.Qty <= 0)
            { throw new ValidationException("归还数量必须大于0！".L10N()); }
            if (!resultDataInfo.WarehouseId.HasValue)
            {
                throw new ValidationException("请选择仓库！".L10N());
            }
            var account = GetFixtureAccountByCodeOrRFID(resultDataInfo.Code);
            if (account == null)
            { throw new ValidationException("工治具治具台账{0}不存在！".L10nFormat(resultDataInfo.Code)); }
            if (account.ManageMode == ManageMode.Number && account.AccountState != FixtureAccountState.Online && account.AccountState != FixtureAccountState.Using)
            { throw new ValidationException("工治具ID[{0}]状态为[{1}],不可操作归还".L10nFormat(account.Code, account.AccountState.ToLabel())); }
            if (account.ManageMode == ManageMode.Code && account.OnlineQty < resultDataInfo.Qty)
            { throw new ValidationException("工治具编码[{0}]在线数量不足{1},不可操作归还".L10nFormat(account.Code, resultDataInfo.Qty)); }
            var unloads = GetReturnUnloads(resultDataInfo.WorkOrderId, resultDataInfo.ResourceId, account.Id);
            if (!unloads.Any())
            { throw new ValidationException("此工治具无可归还的出库明细信息！".L10N()); }
            var canReturnQtys = unloads.Sum(p => p.UnloadQty) - unloads.Sum(p => p.ReturnQty) - unloads.Sum(p => p.NgQty);
            if (canReturnQtys < resultDataInfo.Qty)
            { throw new ValidationException("归还数量必须小于未归还数量：{0}！".L10nFormat(canReturnQtys)); }
            var useResumes = RT.Service.Resolve<CoreFixtureController>().GetUseResumes(account.Id);
            if (useResumes.Any(p => p.OnlineDate != null && p.OfflineDate == null))
            { throw new ValidationException("未下线（上线状态）的工治具不可以进行归还！".L10N()); }
            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                //更新出库明细的归还数量
                UpdateUnloadReturnQty(resultDataInfo.Qty, unloads);
                //产生入库任务和保养任务
                var task = ReturnCreateLaunchTask(resultDataInfo.Qty, account, resultDataInfo.WarehouseId.Value);
                if (task == null)
                {
                    throw new ValidationException("创建入库单失败！".L10N());
                }
                var maintainPrjs = GetFixtureEncodeMaintainProjects(account.FixtureEncodeId);
                var isMaintain = maintainPrjs.Any();
                if (isMaintain)
                {
                    task.QualityState = null;
                    this.CreateMaintainTask(task, account, maintainPrjs);
                }
                else
                {
                    task.QualityState = FixtureQualityState.Pass;
                }
                RF.Save(task);
                //更新工治具治具台账
                if (account.ManageMode == ManageMode.Number)
                {
                    account.AccountState = isMaintain ? FixtureAccountState.WaitMaintain : FixtureAccountState.WaitShelf;
                }
                else
                {
                    account.OnlineQty -= resultDataInfo.Qty;
                    //判断是否需要保养
                    if (isMaintain)
                    {
                        account.WaitMaintain += resultDataInfo.Qty;
                    }
                    else
                    {
                        account.WaitShelfQty += resultDataInfo.Qty;
                    }
                }
                // 创建使用履历
                CreateSaveAccountUseResume(account.Id, resultDataInfo.ResourceId, resultDataInfo.WorkOrderId, UseResumeType.Return, resultDataInfo.Qty);
                RF.Save(account);
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新出库明细的归还数量
        /// </summary>
        /// <param name="qty">需归还数量</param>
        /// <param name="unloads">出库明细列表</param>
        private void UpdateUnloadReturnQty(int qty, EntityList<FixtureUnload> unloads)
        {
            var returnQty = qty;
            foreach (var unload in unloads)
            {
                var canReturnQty = unload.UnloadQty - unload.ReturnQty - unload.NgQty;
                if (returnQty <= 0)
                { break; }
                if (returnQty > canReturnQty)
                {
                    unload.ReturnQty = unload.UnloadQty - unload.NgQty;
                    returnQty -= canReturnQty;
                }
                else if (returnQty == canReturnQty)
                {
                    unload.ReturnQty = unload.UnloadQty - unload.NgQty;
                    break;
                }
                else
                {
                    unload.ReturnQty += returnQty;
                    break;
                }
            }
            RF.Save(unloads);
        }

        /// <summary>
        /// 工治具归还产生入库任务
        /// </summary>
        /// <param name="qty">归还数量</param>
        /// <param name="fixtureAccount">工治具台账</param>
        /// <param name="whId">仓库Id</param>
        /// <returns>入库任务</returns>
        private InboundOrder ReturnCreateLaunchTask(int qty, FixtureAccount fixtureAccount,double whId)
        {
            if (fixtureAccount == null)
            {
                return null;
            }
            var task = new InboundOrder
            {
                No = _commonController.GetNo<InboundOrder>("入库任务"),
                Qty = qty,
                InboundType = FixtureInboundType.Return,
                InboundStatus = InboundStatus.ToBe,
                CustomerId = fixtureAccount.CustomerId,
                FixtureEncodeId = fixtureAccount.FixtureEncodeId,
                SupplierId = fixtureAccount.SupplierId,
                WarehouseId= whId
            };
            task.GenerateId();

            var mode = fixtureAccount.FixtureEncode.FixtureModel.ManageMode;
            switch (mode)
            {
                case ManageMode.Code:
                    InboundOrderFixtureCodeAccount inboundOrderFixtureCodeAccount = new InboundOrderFixtureCodeAccount()
                    {
                        InboundOrderId = task.Id,
                        Qty = qty,
                    };
                    task.InboundOrderFixtureCodeAccountList.Add(inboundOrderFixtureCodeAccount);
                    break;
                case ManageMode.Number:
                    InboundOrderFixtureIdAccount inboundOrderFixtureIdAccount = new InboundOrderFixtureIdAccount()
                    {
                        Qty = qty,
                        FixtureIDAccountId = fixtureAccount.Id,
                        InboundOrderId = task.Id,
                    };
                    task.InboundOrderFixtureIdAccountList.Add(inboundOrderFixtureIdAccount);
                    break;
                default:
                    break;

            }
            return task;
        }
        #endregion
    }
}
