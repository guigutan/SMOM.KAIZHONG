using SIE.Core.Common.Dao;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.WorkOrders;
using SIE.LES.Commons;
using SIE.LES.LinesideWarehouses;
using SIE.LES.StockOrders.APIModels;
using SIE.LES.StockOrders.Service;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.LES.StockOrders.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.StockOrders.Dao
{
    /// <summary>
    /// 备料单DAO
    /// </summary>
    public class StockOrderDao : BaseDao<StockOrder>
    {
        /// <summary>
        /// 获取备料单数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>备料单数据</returns>
        public virtual EntityList<StockOrder> GetStockOrders(StockOrderCriteria criteria)
        {
            var query = Query();
            query.LeftJoin<WorkOrder>((x, y) => x.WorkOrderId == y.Id)
                 .LeftJoin<WorkOrder, Item>((w, i) => w.ProductId == i.Id);
            if (criteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }


            if (criteria.WorkOrderNo.IsNotEmpty())
            {
                query.Where<WorkOrder>((x, y) => y.No.Contains(criteria.WorkOrderNo));
            }

            if (criteria.PlanBeginTime.BeginValue.HasValue)
            {
                query.Where<WorkOrder>((x, y) => y.PlanBeginDate >= criteria.PlanBeginTime.BeginValue.Value);
            }

            if (criteria.PlanBeginTime.EndValue.HasValue)
            {
                query.Where<WorkOrder>((x, y) => y.PlanBeginDate <= criteria.PlanBeginTime.EndValue.Value);
            }

            if (criteria.ProductCode.IsNotEmpty())
            {
                query.Where<WorkOrder, Item>((x, w, i) => x.WorkOrderId == w.Id && w.ProductId == i.Id && i.Code.Contains(criteria.ProductCode));
            }

            if (criteria.ProductName.IsNotEmpty())
            {
                query.Where<WorkOrder, Item>((x, w, i) => x.WorkOrderId == w.Id && w.ProductId == i.Id && i.Name.Contains(criteria.ProductName));
            }

            if (criteria.WorkShopId.HasValue)
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId.Value);
            }

            if (criteria.ResourceId.HasValue)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId.Value);
            }

            if (!string.IsNullOrEmpty(criteria.StockState))
            {
                var criteriaState = new List<int>();
                criteria.StockState.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.StockState));
            }

            if (criteria.BillSource.HasValue)
            {
                query.Where(p => p.BillSource == criteria.BillSource.Value);
            }

            if (criteria.StockType.HasValue)
            {
                query.Where(p => p.StockType == criteria.StockType.Value);
            }

            if (criteria.TriggerMode.HasValue)
            {
                query.Where(p => p.TriggerMode == criteria.TriggerMode.Value);
            }

            if (criteria.DemandMode.HasValue)
            {
                query.Where(p => p.DemandMode == criteria.DemandMode.Value);
            }

            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            if (criteria.ItemCode.IsNotEmpty())
            {
                query.Exists<StockOrderDetail>((a, b) => b.Join<Items.Item>("c", (b, c) => b.ItemId == c.Id
 && c.Code.Contains(criteria.ItemCode)).Where<StockOrder, StockOrderDetail>((k, a, b) => a.Id == b.StockOrderId));
            }
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取备料单数据
        /// </summary>
        /// <param name="stockOrderIds">ID集合</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>备料单数据</returns>
        public virtual EntityList<StockOrder> GetStockOrderByIds(List<double> stockOrderIds, EagerLoadOptions elo = null)
        {
            return stockOrderIds.SplitContains(tmpIds =>
            {
                return Query().Where(p => tmpIds.Contains(p.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取备料单明细
        /// </summary>
        /// <param name="stockOrderIds"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderDetail> GetStockOrderDetailByOrderIds(List<double> stockOrderIds, EagerLoadOptions elo = null)
        {
            return stockOrderIds.SplitContains(tmpIds =>
            {
                return DB.Query<StockOrderDetail>().Where(p => tmpIds.Contains(p.StockOrderId)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 是否存在同工单同物料同物料扩展属性的备料单明细
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSameWoItemItemExpExsited(double curId, double? woId, double itemId, string itemExprop)
        {
            /*
             若同工单同物料同拓展属性 存在（待审核、已提交、拣配中、待接收）的需求明细，不能提交新的备料模式=推式的单据
             */
            var res = DB.Query<StockOrderDetail>().Where(m => m.Id != curId && m.StockOrder.WorkOrderId == woId &&
            m.ItemId == itemId && m.ItemExtProp == itemExprop
            && (m.StockState == StockState.Audit || m.StockState == StockState.Submitted || m.StockState == StockState.PickStocking || m.StockState == StockState.TobeReceive)
            ).ToList().Count;
            return res > 0;
        }
        /// <summary>
        /// 是否存在相同仓库同物料扩展属性的备料单明细
        /// </summary>
        /// <param name="curId"></param>
        /// <param name="whId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExprop"></param>
        /// <returns></returns>
        public virtual bool IsSameWhItemItemExpExsited(double curId, double? whId, double itemId, string itemExprop)
        {
            /*
             若同仓库同物料同拓展属性 存在（待审核、已提交、拣配中、待接收）的需求明细，不能提交新的备料模式=拉式的单据
             */
            var res = DB.Query<StockOrderDetail>().Where(m => m.Id != curId && m.WarehouseId == whId &&
            m.ItemId == itemId && m.ItemExtProp == itemExprop
            && (m.StockState == StockState.Audit || m.StockState == StockState.Submitted || m.StockState == StockState.PickStocking || m.StockState == StockState.TobeReceive)
            ).ToList().Count;
            return res > 0;
        }


        /// <summary>
        /// 通过备料单单号获取备料单信息
        /// </summary>
        /// <param name="No">备料单号</param>
        /// <param name="elo">贪婪加载选项</param>
        /// <returns></returns>
        public virtual StockOrder GetStockOrdersByNo(string No, EagerLoadOptions elo)
        {
            return Query().Where(f => f.No == No).FirstOrDefault(elo);
        }

        /// <summary>
        /// 通过单号获取备料单
        /// </summary>
        /// <param name="Nos">条码</param>
        /// <returns></returns>
        public virtual EntityList<StockOrder> GetStockOrderByNos(List<string> Nos, EagerLoadOptions elo)
        {
            var query = Query();
            return query.Where(p => Nos.Contains(p.No)).ToList(null, elo);
        }


        /// <summary>
        /// 通过状态号获取备料单
        /// </summary>
        /// <param name="states">状态</param>
        /// <returns></returns>
        public virtual EntityList<StockOrder> GetStockOrderByStates(List<StockState> states, EagerLoadOptions elo)
        {
            var query = Query();
            return query.Where(p => states.Contains(p.StockState)).ToList(null, elo);
        }

        /// <summary>
        /// 更新备料单状态
        /// </summary>
        /// <param name="stockOrder">备料单</param>
        /// <param name="stockOrderDetails">备料单明细</param>
        public virtual void UpdateStockOrderState(StockOrder stockOrder, EntityList<StockOrderDetail> stockOrderDetails)
        {
            //存在待接收就更新待接收
            if (stockOrderDetails.Any(f => f.StockState == StockState.TobeReceive))
            {
                stockOrder.StockState = StockState.TobeReceive;
            }//没有待接收,存在拣配更新为拣配
            else if (stockOrderDetails.Any(f => f.StockState == StockState.PickStocking))
            {
                stockOrder.StockState = StockState.PickStocking;
            }
            //else if (stockOrderDetails.Any(f => f.StockState == StockState.PartReceived))
            //{
            //    stockOrder.StockState = StockState.PartReceived;
            //}
            else if (stockOrderDetails.Any(f => f.StockState == StockState.Received))//全部接收后为接收状态
            {
                stockOrder.StockState = StockState.Received;
            }
            else if (stockOrderDetails.All(f => f.StockState == StockState.Closed))
            {
                stockOrder.StockState = StockState.Closed;
            }
            else if (stockOrderDetails.All(f => f.StockState == StockState.Received))//全部接收后为接收状态
            {
                stockOrder.StockState = StockState.Received;
            }
            else if (stockOrderDetails.All(f => f.StockState != StockState.TobeReceive) ||
                stockOrderDetails.Any(f => f.StockState == StockState.PickStocking)
                )
            {
                stockOrder.StockState = StockState.PickStocking;
            }

        }

        /// <summary>
        /// 获取员工权限下的工厂
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetEnterprises(double employeeId)
        {
            var query = DB.Query<Enterprise>()
                .Join<EmployeeEnterprise>((x, y) => y.EnterpriseId == x.Id && y.EmployeeId == employeeId && x.InvOrgId == RT.InvOrg)
                .ToList();
            return query;
        }

        /// <summary>
        /// 获取工厂下的车间
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="workShops"></param>
        public virtual void GetWorkShops(double? pId, List<Enterprise> workShops)
        {
            var childrenList = DB.Query<Enterprise>().Where(x => x.TreePId == pId && x.InvOrgId == RT.InvOrg).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            childrenList.ForEach(item =>
            {
                if (item.LevelType == EnterpriseType.Shop)
                {
                    item.TreePId = null;
                    workShops.Add(item);
                }
                else
                {
                    GetWorkShops(item.Id, workShops);
                }
            });
        }

        /// <summary>
        /// 获取工厂下的资源
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="workShopId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResources(double factoryId, double? workShopId = null, PagingInfo pagingInfo = null, string keyword = null)
        {
            var query = DB.Query<WipResource>().Where(p => p.FactoryId == factoryId && p.ResourceState == ResourceState.Actived)
                .WhereIf(workShopId != null && workShopId != 0, p => p.WorkShopId == workShopId)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }

        /// <summary>
        /// 获取工厂下的资源
        /// </summary>
        /// <param name="workShopId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResources(double? workShopId = null, PagingInfo pagingInfo = null, string keyword = null)
        {
            if (workShopId == null)
            {
                return new EntityList<WipResource>();
            }
            var query = DB.Query<WipResource>().Where(p => p.WorkShopId == workShopId && p.ResourceState == ResourceState.Actived)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }

        /// <summary>
        /// 获取资源对应的产线线边仓
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual EntityList<LinesideWarehouse> GetLinesideWarehouse(double wipId)
        {
            var query = DB.Query<LinesideWarehouse>().Where(p => p.WipResouceId == wipId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }

        /// <summary>
        /// 更新或创建用户备料单app基本信息
        /// </summary>
        /// <param name="stockBaseInfo"></param>
        public virtual void UpdataUserInfo(StockBaseInfo stockBaseInfo)
        {
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                var info = DB.Query<StockOrderAppBaseInfo>().Where(p => p.EmployeeId == stockBaseInfo.EmployeeId).FirstOrDefault();
                // 为空则新建
                if (info == null)
                {
                    if (stockBaseInfo.FactoryId == 0)
                    {
                        throw new ValidationException("工厂信息异常！".L10N());
                    }
                    StockOrderAppBaseInfo stockOrderAppBaseInfo = new StockOrderAppBaseInfo
                    {
                        EmployeeId = stockBaseInfo.EmployeeId,
                        FactoryId = stockBaseInfo.FactoryId,
                        FactoryName = stockBaseInfo.FactoryName,
                        WorkShopId = stockBaseInfo.WorkShopId,
                        WorkShopName = stockBaseInfo.WorkShopName,
                        ResourceId = stockBaseInfo.ResourceId,
                        ResourceName = stockBaseInfo.ResourceName,
                        WarehouseId = stockBaseInfo.WarehouseId,
                        WarehouseName = stockBaseInfo.WarehouseName,
                        StorageId = stockBaseInfo.StorageId,
                        StorageName = stockBaseInfo.StorageName,
                        StockType = stockBaseInfo.StockType,
                        ReceiveType = stockBaseInfo.ReceiveType,
                    };
                    RF.Save(stockOrderAppBaseInfo);
                }
                // 否则更新
                else
                {
                    info.FactoryId = stockBaseInfo.FactoryId;
                    info.FactoryName = stockBaseInfo.FactoryName;
                    info.WorkShopId = stockBaseInfo.WorkShopId;
                    info.WorkShopName = stockBaseInfo.WorkShopName;
                    info.ResourceId = stockBaseInfo.ResourceId;
                    info.ResourceName = stockBaseInfo.ResourceName;
                    info.WarehouseId = stockBaseInfo.WarehouseId;
                    info.WarehouseName = stockBaseInfo.WarehouseName;
                    info.StorageId = stockBaseInfo.StorageId;
                    info.StorageName = stockBaseInfo.StorageName;
                    info.StockType = stockBaseInfo.StockType;
                    info.ReceiveType = stockBaseInfo.ReceiveType;
                    RF.Save(info);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="employeeId"></param>
        public virtual StockBaseInfo GetUserInfo(double employeeId)
        {
            var baseInfo = DB.Query<StockOrderAppBaseInfo>().Where(p => p.EmployeeId == employeeId).FirstOrDefault();
            if (baseInfo == null)
            {
                return new StockBaseInfo { isNew = true };
            }
            var lineWarehouse = DB.Query<LinesideWarehouse>().Where(p => p.WipResouceId == baseInfo.ResourceId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            List<BaseInfo> lineWareList = new List<BaseInfo>();
            List<BaseInfo> lineStorageList = new List<BaseInfo>();
            lineWarehouse.ForEach(item =>
            {
                BaseInfo warehouse = new BaseInfo
                {
                    Id = item.WarehouseId,
                    Name = item.WarehouseName,
                    Code = item.WarehouseCode,
                };
                BaseInfo storage = new BaseInfo
                {
                    Id = item.StorageLocationId,
                    Name = item.LocaltionName,
                    Code = item.LocaltionCode,
                };
                lineStorageList.Add(storage);
                lineWareList.Add(warehouse);
            });

            StockBaseInfo stockBaseInfo = new StockBaseInfo
            {
                EmployeeId = baseInfo.EmployeeId,
                FactoryId = baseInfo.FactoryId,
                FactoryName = baseInfo.FactoryName,
                WorkShopId = baseInfo.WorkShopId,
                WorkShopName = baseInfo.WorkShopName,
                ResourceId = baseInfo.ResourceId,
                ResourceName = baseInfo.ResourceName,
                WarehouseId = baseInfo.WarehouseId,
                WarehouseName = baseInfo.WarehouseName,
                StorageId = baseInfo.StorageId,
                StorageName = baseInfo.StorageName,
                StockType = baseInfo.StockType,
                ReceiveType = baseInfo.ReceiveType,
                LineWarehouseList = lineWareList,
                LineStorageList = lineStorageList,
            };
            return stockBaseInfo;
        }
        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>

        public virtual ItemLabel GetItemLabel(string label)
        {
            var itemLabel = DB.Query<ItemLabel>()
                    .Where(p => p.Label == label)
                    .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return itemLabel;
        }


        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="itemCode">物料编码</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <returns></returns>
        public virtual BaseInfo GetItemInfo(string itemCode, double? warehouseId)
        {
            var item = DB.Query<Items.Item>()
                .Where(p => p.Code.Contains(itemCode))
                .FirstOrDefault();

            if (item == null)
            {
                throw new ValidationException("扫描失败。请扫描正确的物料编码或物料标签".L10N());
            }

            BaseInfo baseInfo = new BaseInfo
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                EnableItemExtProp = item.EnableExtendProperty
            };

            if (warehouseId.HasValue && warehouseId != 0
                && (item.ConsumeMode == Items.ConsumeMode.Pull || item.ConsumeMode == Items.ConsumeMode.Reserve))
            {
                var pullItem = DB.Query<PrepareItemPull>()
                    .Where(p => p.WarehouseId == warehouseId && p.ItemId == item.Id)
                    .FirstOrDefault();

                //按物料+仓库找不到备料模式设置时，再按仓库+物料类型找一次
                if (pullItem == null)
                {
                    pullItem = DB.Query<PrepareItemPull>()
                      .Join<Items.ItemCategoryRelation>((x, y) => x.ItemCategoryId == y.ItemCategoryId && y.Type == Items.Items.CategoryType.Item)
                      .Where<Items.ItemCategoryRelation>((x, y) => x.WarehouseId == warehouseId && y.ItemId == item.Id)
                      .FirstOrDefault();
                }

                if (pullItem != null && pullItem.DemandType == DemandMode.FixedQuantity)
                {
                    baseInfo.Qty = pullItem.FixedQuantity.Value;
                }
            }

            switch (item.ConsumeMode)
            {
                case Items.ConsumeMode.Pull:
                    baseInfo.StockType = PrepareItemType.Pull;
                    break;
                case Items.ConsumeMode.Push:
                    baseInfo.StockType = PrepareItemType.Push;
                    break;
                case Items.ConsumeMode.Reserve:
                    baseInfo.StockType = PrepareItemType.Pull;
                    break;
                default:
                    break;
            }

            return baseInfo;
        }

        /// <summary>
        /// 根据物料获取工单
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="workShopId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>

        public virtual List<BaseInfo> GetWorkOders(double? itemId, double? workShopId, double? resourceId)
        {
            var workOrders = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(workShopId, resourceId, null, null);
            var workOrderIds = workOrders.Select(o => o.Id).ToList();
            var workOrderBoms = workOrderIds.SplitContains(tempId =>
            {
                return DB.Query<WorkOrderBom>().Where(p => tempId.Contains(p.WorkOrderId) && p.ItemId == itemId).ToList();
            });
            workOrderIds = workOrderBoms.Select(o => o.WorkOrderId).ToList();
            workOrders = DB.Query<WorkOrder>().Where(p => workOrderIds.Contains(p.Id) && p.State == WorkOrderState.Release).ToList();
            List<BaseInfo> baseInfos = new List<BaseInfo>();
            workOrders.ForEach(item =>
            {

                var bom = workOrderBoms.FirstOrDefault(m => m.WorkOrderId == item.Id && m.ItemId == itemId);
                BaseInfo baseInfo = new BaseInfo
                {
                    Id = item.Id,
                    Code = item.No,
                    Name = item.No,

                };
                if (bom != null)
                {
                    baseInfo.ItemExtPropDataInfos.Add(
                        new ItemExtPropDataInfo()
                        {
                            ItemExtPropValue = bom.ItemExtProp,
                            DefinitionName = bom.ItemExtPropName
                        });
                }
                baseInfos.Add(baseInfo);
            });
            return baseInfos;
        }

        /// <summary>
        /// 创建备料单
        /// </summary>
        /// <param name="baseInfo"></param>
        /// <param name="itemId"></param>
        /// <param name="stockQty"></param>
        /// <param name="workOrderId"></param>
        /// <param name="itemExtProp"></param>
        /// <param name="itemExtPropName"></param>
        /// <returns></returns>
        public virtual string CreateStockOrder(StockBaseInfo baseInfo, double itemId, decimal stockQty, double? workOrderId, string itemExtProp, string itemExtPropName)
        {
            var dbDateTime = RF.Find<StockOrder>().GetDbTime();

            var lineWare = DB.Query<LinesideWarehouse>()
                .Where(p => p.WipResouceId == baseInfo.ResourceId && p.WarehouseId == baseInfo.WarehouseId)
                .FirstOrDefault();

            WorkOrderBom workOrderBom = null;
            if (workOrderId.HasValue)
            {
                workOrderBom = DB.Query<WorkOrderBom>()
                  .Where(p => p.WorkOrderId == workOrderId && p.ItemId == itemId)
                  .FirstOrDefault();
            }

            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                StockOrder stockOrder = new StockOrder
                {
                    No = RT.Service.Resolve<StockOrderService>().GetStockOrderNo(),
                    StockState = StockState.Created,
                    StockType = baseInfo.StockType,
                    FactoryId = baseInfo.FactoryId,
                    WorkShopId = baseInfo.WorkShopId.Value,
                    ResourceId = baseInfo.ResourceId,
                    WorkOrderId = workOrderId,
                    BillSource = BillSource.Manual,
                    TriggerMode = TriggerMode.ManualModel,
                    DemandMode = DemandMode.ManualFillIn,
                };

                RF.Save(stockOrder);

                StockOrderDetail stockOrderDetail = new StockOrderDetail
                {
                    StockOrderId = stockOrder.Id,
                    LineNo = "1",
                    StockState = StockState.Created,
                    Qty = stockQty,
                    ItemId = itemId,
                    DemandTime = dbDateTime,
                    WarehouseId = baseInfo.WarehouseId,
                    IsManualRec = (baseInfo.ReceiveType == StockReceiveType.Hand),
                    ItemExtProp = itemExtProp,
                    ItemExtPropName = itemExtPropName
                };
                if (lineWare != null)
                {
                    stockOrderDetail.StorageLocationId = lineWare.StorageLocationId;

                }
                if (workOrderId.HasValue && workOrderBom != null)
                {
                    stockOrderDetail.WoTotalQty = workOrderBom.RequireQty;
                }
                else
                {
                    stockOrderDetail.WoTotalQty = 0;
                }

                RF.Save(stockOrderDetail);

                tran.Complete();
                return stockOrder.No;
            }
        }

        /// <summary>
        /// 备料单查询物料
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<StockOrderItemViewModel> GetStockOrderItemViewModels(StockOrderItemViewModelCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<StockOrderItemViewModel>();
            }
            // 对应工单bom的物料
            var bomItemIds = new List<double>();
            EntityList<WorkOrderBom> boms = new EntityList<WorkOrderBom>();
            
            var itemQuery = DB.Query<SIE.Items.Item>();
            if (criteria.Code.IsNotEmpty())
            {
                itemQuery.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                itemQuery.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.ConsumeMode.HasValue)
            {
                itemQuery.Where(p => p.ConsumeMode == criteria.ConsumeMode);
            }
            if (bomItemIds.Count > 0)
            {
                itemQuery.Where(p => bomItemIds.Contains(p.Id));
            }
            itemQuery.OrderByDescending(p => p.CreateDate);
            List<SIE.Items.ItemInfo> itemList = new List<Items.ItemInfo>();

            if (criteria.WoId != null && criteria.StockType == PrepareItemType.Push) // 工单bom物料
            {
                boms = DB.Query<WorkOrderBom>().Where(p => p.WorkOrderId == criteria.WoId).ToList();
                var iq = itemQuery.Exists<WorkOrderBom>((i, wb) => wb.Where(w => w.ItemId == i.Id && w.WorkOrderId == criteria.WoId));
                var queryList = iq.Join<SIE.Items.Unit>((x, y) => x.UnitId == y.Id).Select<SIE.Items.Unit>((x, y) => new {
                    ItemId = x.Id,
                    ItemCode = x.Code,
                    ItemName = x.Name,
                    SpecificationModel = x.SpecificationModel,
                    ConsumeMode = x.ConsumeMode,
                    Unit = y.Name,
                    IsEnableItemExtProp = x.EnableExtendProperty,
                }).ToList<SIE.Items.ItemInfo>(criteria.PagingInfo).ToList();
                var totalCount = iq.Count();

                EntityList<StockOrderItemViewModel> viewmodel = new EntityList<StockOrderItemViewModel>();
                foreach (var bom in boms)
                {
                    var item = queryList.FirstOrDefault(p => p.ItemId == bom.ItemId);
                    if (item == null) continue;
                    StockOrderItemViewModel view = new StockOrderItemViewModel
                    {
                        Id = item.ItemId,
                        Code = item.ItemCode,
                        Name = item.ItemName,
                        SpecificationModel = item.SpecificationModel,
                        ConsumeMode = item.ConsumeMode,
                        Unit = item.Unit,
                        IsEnableItemExtProp = item.IsEnableItemExtProp,
                        ItemExtProp = bom.ItemExtProp,
                        ItemExtPropName = bom.ItemExtPropName,
                        WorkOrderQty = bom.RequireQty,
                    };
                    viewmodel.Add(view);
                }
                viewmodel.SetTotalCount(totalCount);
                return viewmodel;
            }
            else // 启用物料
            {
                var iq = itemQuery.Where(p => p.State == State.Enable).Join<SIE.Items.Unit>((x, y) => x.UnitId == y.Id);
                itemList = iq.Select<SIE.Items.Unit>((x, y) => new {
                    ItemId = x.Id,
                    ItemCode = x.Code,
                    ItemName = x.Name,
                    SpecificationModel = x.SpecificationModel,
                    ConsumeMode = x.ConsumeMode,
                    Unit = y.Name,
                    IsEnableItemExtProp = x.EnableExtendProperty,
                }).ToList<SIE.Items.ItemInfo>(criteria.PagingInfo).ToList();
                var totalCount = iq.Count();

                EntityList<StockOrderItemViewModel> viewmodel = new EntityList<StockOrderItemViewModel>();
                foreach (var item in itemList)
                {
                    StockOrderItemViewModel view = new StockOrderItemViewModel
                    {
                        Id = item.ItemId,
                        Code = item.ItemCode,
                        Name = item.ItemName,
                        SpecificationModel = item.SpecificationModel,
                        ConsumeMode = item.ConsumeMode,
                        Unit = item.Unit,
                        IsEnableItemExtProp = item.IsEnableItemExtProp,
                    };
                    viewmodel.Add(view);
                }
                viewmodel.SetTotalCount(totalCount);
                return viewmodel;
            }
        }
    }
}