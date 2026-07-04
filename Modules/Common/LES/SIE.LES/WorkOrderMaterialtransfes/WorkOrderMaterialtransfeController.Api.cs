using SIE.Api;
using SIE.Common.Catalogs;
using SIE.Core.ApiModels;
using SIE.Core.WorkOrders;
using SIE.Core.WorkOrders.Models;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.Items;
using SIE.LES.Interfaces;
using SIE.LES.Interfaces.Datas;
using SIE.LES.LinesideWarehouses;
using SIE.LES.RetreatItemManage;
using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Dao;
using SIE.LES.WorkOrderMaterialtransfes.ApiModel;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Routings;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.WorkOrderMaterialtransfes
{
    /// <summary>
    /// 工单挪料控制器
    /// </summary>
    public class WorkOrderMaterialtransfeController : DomainController
    {
        /// <summary>
        /// 获取目标资源列表
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("获取目标资源列表")]
        [return: ApiReturn("目标资源信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetWipResouresInfos([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo)
        {

            var wipResouresInfoResult = new WipResouresInfos();
            var pagingInfo = new PagingInfo()
            {
                PageNumber = queryInfo.PageNumber.HasValue ? queryInfo.PageNumber.Value : 1,
                PageSize = queryInfo.PageSize.HasValue ? queryInfo.PageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            wipResouresInfoResult.PageNumber = pagingInfo.PageNumber;
            wipResouresInfoResult.PageSize = pagingInfo.PageSize;


            var resultWipResource = Query<WipResource>().Exists<EmployeeEnterprise>((x, y) =>
            y.Where(p => p.EmployeeId == RT.IdentityId && p.EnterpriseId == x.FactoryId))
                .Where(m => m.ResourceState == ResourceState.Actived).WhereIf(queryInfo.Keyword != "%%", m => m.Code.Contains(queryInfo.Keyword)).ToList(pagingInfo);
            if (resultWipResource.Any())
            {
                var ids = resultWipResource.Select(m => m.Id).ToList();
                var linesideWarehouseInfos = ids.SplitContains(templist =>
                {
                    return Query<LinesideWarehouse>().Where(m => m.WipResouceId != null && templist.Contains((double)m.WipResouceId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
                wipResouresInfoResult.TotalCount = resultWipResource.Count;
                foreach (var item in resultWipResource)
                {
                    var linesideWarehouseInfo = linesideWarehouseInfos.FirstOrDefault(m => m.WipResouceId == item.Id);
                    var resultItem = new WipResouresInfo()
                    {
                        ResouceId = item.Id,
                        ResouceCode = item.Code,
                        ResouceName = item.Name,
                    };


                    if (linesideWarehouseInfo != null)
                    {
                        resultItem.WarehouseId = linesideWarehouseInfo.WarehouseId;
                        resultItem.WarehouseName = linesideWarehouseInfo.WarehouseName;
                        resultItem.WarehouseCode = linesideWarehouseInfo.WarehouseCode;
                        resultItem.WarehouseId = linesideWarehouseInfo.WarehouseId;
                        if (linesideWarehouseInfo.StorageLocationId > 0)
                        {
                            resultItem.LocationId = linesideWarehouseInfo.StorageLocationId;
                            resultItem.LocationCode = linesideWarehouseInfo.LocaltionCode;
                            resultItem.LocationName = linesideWarehouseInfo.LocaltionName;
                        }

                    }
                    wipResouresInfoResult.ResultInfos.Add(resultItem);


                }
            }
            return wipResouresInfoResult;


        }


        /// <summary>
        /// 选择资源符合，且状态为【发放、生产中、发放暂停、生产中暂停】的工单
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="wipResouresId"></param>
        /// <returns></returns>
        [ApiService("根据目标资源获取工单列表")]
        [return: ApiReturn("目标资源信息 PagingBaseDataInfo")]

        public virtual PagingBaseDataInfo GetWOInfosByWipResouresId([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo, [ApiParameter("资源ID")] double? wipResouresId)
        {
            if (!wipResouresId.HasValue)
            {
                throw new ValidationException("请先选择目标资源".L10N());
            }
            var resource = RF.GetById<WipResource>(wipResouresId);
            if (resource == null)
            {
                throw new ValidationException("请先选择目标资源".L10N());
            }
            var resultInfo = new PagingBaseDataInfo();
            var pagingInfo = new PagingInfo()
            {
                PageNumber = queryInfo.PageNumber.HasValue ? queryInfo.PageNumber.Value : 1,
                PageSize = queryInfo.PageSize.HasValue ? queryInfo.PageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var wos = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(resource.WorkShopId, resource.Id, pagingInfo, queryInfo.Keyword);
            foreach (var wo in wos)
            {
                resultInfo.DataInfos.Add(new BaseDataInfo()
                {
                    Id = wo.Id,
                    Code = wo.No,
                    Name = wo.No
                });
            }
            resultInfo.TotalCount = wos.Count;
            resultInfo.PageNumber = pagingInfo.PageNumber;
            resultInfo.PageSize = pagingInfo.PageSize;
            return resultInfo;
        }

        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("获取仓库列表")]
        [return: ApiReturn("获取仓库列表 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetWarehouseList([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo)
        {
            var resultInfo = new PagingBaseDataInfo();
            var pagingInfo = new PagingInfo()
            {
                PageNumber = queryInfo.PageNumber.HasValue ? queryInfo.PageNumber.Value : 1,
                PageSize = queryInfo.PageSize.HasValue ? queryInfo.PageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var query = Query<Warehouse>().Where(p => p.State == State.Enable && p.LibraryType == LibraryType.Entity);
            if (queryInfo.Keyword != "%%")
            {
                query.Where(m => m.Code.Contains(queryInfo.Keyword) || m.Name.Contains(queryInfo.Keyword));

            }
            var res = query.ToList(pagingInfo);
            foreach (var wo in res)
            {
                resultInfo.DataInfos.Add(new BaseDataInfo()
                {
                    Id = wo.Id,
                    Code = wo.Code,
                    Name = wo.Name
                });
            }
            return resultInfo;
        }

        /// <summary>
        /// 根据仓库资源获取库位列表
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [ApiService("根据仓库资源获取库位列表")]
        [return: ApiReturn("根据仓库资源获取库位列表 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetLocationList([ApiParameter("查询信息")] PagingKeywordQueryInfo queryInfo, [ApiParameter("仓库Id")] double? warehouseId)
        {
            if (!warehouseId.HasValue)
            {
                throw new ValidationException("请先选择仓库".L10N());
            }
            var warehouse = RF.GetById<Warehouse>(warehouseId);
            if (warehouse == null)
            {
                throw new ValidationException("请先选择仓库".L10N());
            }
            var resultInfo = new PagingBaseDataInfo();
            var pagingInfo = new PagingInfo()
            {
                PageNumber = queryInfo.PageNumber.HasValue ? queryInfo.PageNumber.Value : 1,
                PageSize = queryInfo.PageSize.HasValue ? queryInfo.PageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var query = Query<StorageLocation>().Where(p => p.State == State.Enable && p.LibraryType == LibraryType.Entity && p.WarehouseId == warehouseId);
            if (!queryInfo.Keyword.IsNullOrEmpty())
            {
                query.Where(m => m.Code.Contains(queryInfo.Keyword) || m.Name.Contains(queryInfo.Keyword));

            }
            var res = query.ToList(pagingInfo);
            foreach (var location in res)
            {
                resultInfo.DataInfos.Add(new BaseDataInfo()
                {
                    Id = location.Id,
                    Code = location.Code,
                    Name = location.Name
                });
            }
            return resultInfo;
        }

        /// <summary>
        /// 扫描物料标签
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="woId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [ApiService("扫描物料标签")]
        [return: ApiReturn("返回一或多条物料标签信息")]
        public virtual List<ScandResultInfos> GetScandResultInfos([ApiParameter("条码")] string barcode, [ApiParameter("工单ID")] double? woId, [ApiParameter("仓库ID")] double? warehouseId,
           [ApiParameter("库位ID")] double? locationId)
        {
            List<ScandResultInfos> scandResultInfos = new List<ScandResultInfos>();
            if (barcode.IsNullOrEmpty())
            {
                throw new ValidationException("请扫描标签".L10N());
            }

            var itemLabels = Query<ItemLabel>().Where(n => n.Label == barcode && n.NgQty <= 0).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (!itemLabels.Any())
            {
                throw new ValidationException("系统不存在该物料标签,请重新扫描标签".L10N());
            }

            if (itemLabels.All(m => m.Qty <= 0))
            {
                throw new ValidationException("物料标签数量为0，不能挪料".L10N());
            }

            var returnReason = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialReasonDefault();
            foreach (var itemLabel in itemLabels)
            {
                if (!itemLabel.WorkOrderList.Any())//无关联工单
                {
                    if (itemLabel.Qty <= 0)
                    {
                        continue;
                    }
                    var res = new ScandResultInfos()
                    {
                        Id = itemLabel.Id,
                        ItemId = itemLabel.ItemId.ToString(),
                        ItemCode = itemLabel.ItemCode,
                        ItemName = itemLabel.ItemName,
                        Qty = itemLabel.Qty,
                        RemainingQty = itemLabel.Qty,
                        Sn = itemLabel.Label,
                        WoNo = "",
                        Reason = returnReason,
                        BatchNo = itemLabel.Lot,
                        Desc = "工单挪料".L10N(),
                        ReturnTypeDisplay = ReturnTypes.Normal.ToLabel(),
                        ReturnType = ReturnTypes.Normal,
                        Warehouse = itemLabel.WarehouseCode,
                        WarehouseLocation = itemLabel.StorageLocationCode,
                        FactoryId = itemLabel.FactoryId,
                        Factory = itemLabel.FactoryName,

                    };
                    var result = RT.Service.Resolve<ItemController>().GetItemUnitPrecisions(itemLabel.ItemId);
                    if (result != null)
                    {
                        res.unitPrecsion = result.unitPrecsion;
                        res.carry = result.carry;
                    }

                    scandResultInfos.Add(res);
                }

                foreach (var itemWo in itemLabel.WorkOrderList)
                {
                    if (itemWo.Qty <= 0)
                    {
                        continue;
                    }
                    var itemWoRes = GetItemWoRes(returnReason, itemLabel, itemWo);
                    var wipResource = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(itemWo.WorkOrderId);
                    if (wipResource.ResourceId > 0)
                    {
                        var wip = RF.GetById<WipResource>(wipResource.ResourceId);
                        if (wip != null)
                        {
                            itemWoRes.ResoureId = wip.Id;
                            itemWoRes.Resoure = wip.Name;
                        }
                    }
                    itemWoRes.Factory = itemLabel.FactoryName;
                    itemWoRes.FactoryId = itemLabel.FactoryId;

                    var result = RT.Service.Resolve<ItemController>().GetItemUnitPrecisions(itemLabel.ItemId);
                    if (result != null)
                    {
                        itemWoRes.unitPrecsion = result.unitPrecsion;
                        itemWoRes.carry = result.carry;
                    }

                    scandResultInfos.Add(itemWoRes);
                }
            }
            return scandResultInfos;
        }

        /// <summary>
        /// 获取退料原因集合
        /// </summary>
        [ApiService("获取退料原因集合")]
        [return: ApiReturn("返回一退料原因集合")]
        public virtual List<BaseDataInfo> GetReasonsCatalogs()
        {
            var catalist = RT.Service.Resolve<CatalogController>().GetCatalogList(MaterialReturn.ReasonMaterialReturn);
            if (catalist == null)
            {
                throw new ValidationException("退料原因快码异常！".L10N());
            }
            List<BaseDataInfo> returnReasonList = new List<BaseDataInfo>();
            catalist.ForEach(cata =>
            {
                returnReasonList.Add(new BaseDataInfo()
                {
                    Id = cata.Id,
                    Code = cata.Code,
                    Name = cata.Name
                });
            });
            return returnReasonList;
        }

        /// <summary>
        /// 获取物料标签结果
        /// </summary>
        /// <param name="returnReason"></param>
        /// <param name="itemLabel"></param>
        /// <param name="itemWo"></param>
        /// <returns></returns>

        private ScandResultInfos GetItemWoRes(string returnReason, ItemLabel itemLabel, ItemLabelWorkOrder itemWo)
        {
            return new ScandResultInfos()
            {
                Id = itemLabel.Id,
                ItemId = itemLabel.ItemId.ToString(),
                ItemCode = itemLabel.ItemCode,
                ItemName = itemLabel.ItemName,
                Qty = itemWo.Qty,
                RemainingQty = itemWo.Qty,
                Sn = itemLabel.Label,
                WoNo = itemWo.WorkOrder.No,
                WoId = itemWo.WorkOrderId,
                WoLabelRelationId = itemWo.Id,
                Reason = returnReason,
                BatchNo = itemLabel.Lot,
                Desc = "工单挪料".L10N(),
                ReturnTypeDisplay = ReturnTypes.Normal.ToLabel(),
                ReturnType = ReturnTypes.Normal,
                Warehouse = itemLabel.WarehouseCode,
                WarehouseLocation = itemLabel.StorageLocationCode,
                FactoryId = itemLabel.FactoryId,

            };
        }


        /// <summary>
        /// 提交工单挪料
        /// </summary>
        /// <param name="scandResultInfos"></param>
        /// <param name="targetWoId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>

        [ApiService("提交工单挪料")]
        [return: ApiReturn("")]
        public virtual void SubmitTransfer([ApiParameter("已扫数据集合")] List<ScandResultInfos> scandResultInfos, [ApiParameter("目标资源ID")] double resourceId,
            [ApiParameter("目标工单ID")] double? targetWoId, [ApiParameter("仓库ID")] double? warehouseId,
           [ApiParameter("库位ID")] double? locationId)
        {

            if (!scandResultInfos.Any())
            {
                throw new ValidationException("扫描记录不能为空".L10N());
            }
            var barcodeIds = scandResultInfos.Select(m => m.Id).Distinct().ToList();
            //获取回所有的物料标签
            EntityList<ItemLabel> itemLabels = CheckInputParamas(barcodeIds, warehouseId, locationId);
            //校验有无工厂有无仓库权限 所有来去仓库

            CheckAuth(warehouseId, itemLabels);
            IList<WorkOrderBom> workerBoms = new List<WorkOrderBom>();
            var targetLocaltion = RF.GetById<StorageLocation>(locationId);

            //调拨对象
            MesMoveUpdateOnhandData mesMoveUpdateOnhandData = new MesMoveUpdateOnhandData();
            mesMoveUpdateOnhandData.TargetStorageLocationId = targetLocaltion.Id;
            MoveSnData moveSnData = new MoveSnData();
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                foreach (var scandResultInfo in scandResultInfos)
                {
                    var scandResultInfoItemLabel = itemLabels.FirstOrDefault(m => m.Label == scandResultInfo.Sn);
                    if (scandResultInfoItemLabel == null)
                    {
                        throw new ValidationException("物料标签【{0}】系统无法找到原始标签，提交失败".L10nFormat(scandResultInfo.Sn));
                    }
                    if (scandResultInfoItemLabel.IsSerialNumber == true && scandResultInfo.Qty != scandResultInfo.RemainingQty)//序列号标签管理
                    {
                        throw new ValidationException(scandResultInfoItemLabel.Label + "挪料失败，挪料数量不等于标签数量，序列号标签不能部分挪料！".L10N());
                    }

                    if (targetWoId.HasValue && targetWoId > 0)//存在工单
                    {
                        //工单有值只能走推式退料 生备料单

                        var stockOrderDetails = RT.Service.Resolve<StockOrderDetailDao>().GetPushStockOrderDetails(new List<double>() { targetWoId.Value });
                        workerBoms = GetWorkOrderBomInfos(targetWoId.Value);
                        if (scandResultInfoItemLabel.Item.ConsumeMode == Items.ConsumeMode.Pull && targetWoId != scandResultInfo.WoId)//拉式不能变更工单
                        {
                            throw new ValidationException(scandResultInfoItemLabel.Label + "挪料失败，拉式物料不能变更工单！".L10N());
                        }
                        if (targetWoId == scandResultInfo.WoId && targetWoId > 0)
                        {
                            throw new ValidationException(scandResultInfoItemLabel.Label + "提交挪料失败，标签的工单与挪料目标工单一致。若不需变更工单，请将工单栏位号置为空".L10N());
                        }
                        //检验推式物料的所有校验
                        PushCheck(targetWoId, warehouseId, locationId, scandResultInfoItemLabel, scandResultInfo, stockOrderDetails, workerBoms);
                        //创建退料单
                        MaterialReturn materialReturn = GetNewMaterialReturn(scandResultInfo, scandResultInfoItemLabel);
                        if (materialReturn.ReturnReason.IsNullOrEmpty())
                        {
                            materialReturn.ReturnReason = scandResultInfo.Reason;
                        }
                        //保存退料单据
                        RF.Save(materialReturn);
                        //执行退料
                        RT.Service.Resolve<MaterialReturnController>().Submit(new List<double>() { materialReturn.Id }, true);
                        //准备调用接口
                        GetMoveSnData(moveSnData, scandResultInfo, scandResultInfoItemLabel);

                    }
                    else //工单无值 
                    {
                        if (scandResultInfoItemLabel.Item.ConsumeMode == Items.ConsumeMode.Push)//推式
                        {
                            PushCheck(targetWoId, warehouseId, locationId, scandResultInfoItemLabel, scandResultInfo, null, workerBoms);
                            if (scandResultInfoItemLabel.IsSerialNumber == true)//序列号标签管理
                            {
                                if (scandResultInfo.Qty != scandResultInfo.RemainingQty)
                                {
                                    throw new ValidationException(scandResultInfoItemLabel.Label + "挪料失败，挪料数量不等于标签数量，序列号标签不能部分挪料！".L10N());
                                }
                                else//是序列号且数量相等 
                                {
                                    //直接更新原标签ID对应的仓库库位 序列号的移动时候必须全数移动 只更新仓库和库位即可 数量不做变化
                                    GetMesMoveUpdateonHandData(mesMoveUpdateOnhandData, scandResultInfo, scandResultInfoItemLabel);
                                    scandResultInfoItemLabel.WarehouseId = warehouseId;
                                    scandResultInfoItemLabel.StorageLocationId = locationId;
                                    RF.Save(scandResultInfoItemLabel);
                                }
                            }
                            else//非序列号管理
                            {    //需要考了子工单的变化
                                //1.先找是否存在目标标签 没有则增加 并将自身减少
                                GetMesMoveUpdateonHandData(mesMoveUpdateOnhandData, scandResultInfo, scandResultInfoItemLabel);
                                UpdateItemLabeInfo(warehouseId, locationId, scandResultInfo, scandResultInfoItemLabel);

                            }
                        }
                        else//拉式
                        {
                            PullCheck(warehouseId, locationId, scandResultInfoItemLabel, scandResultInfo);
                            if (scandResultInfoItemLabel.IsSerialNumber == true)//序列号标签管理
                            {
                                GetMesMoveUpdateonHandData(mesMoveUpdateOnhandData, scandResultInfo, scandResultInfoItemLabel);
                                scandResultInfoItemLabel.WarehouseId = warehouseId;
                                scandResultInfoItemLabel.StorageLocationId = locationId;
                                RF.Save(scandResultInfoItemLabel);
                            }
                            else
                            {
                                GetMesMoveUpdateonHandData(mesMoveUpdateOnhandData, scandResultInfo, scandResultInfoItemLabel);
                                UpdateItemLabeInfo(warehouseId, locationId, scandResultInfo, scandResultInfoItemLabel);
                            }
                        }
                    }

                }
                HandInterfaceInfo(scandResultInfos, resourceId, targetWoId, warehouseId, locationId, itemLabels, mesMoveUpdateOnhandData, moveSnData);
                tran.Complete();
            }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="itemLabels"></param>
        private void CheckAuth(double? warehouseId, EntityList<ItemLabel> itemLabels)
        {
            //当前用户所有的工厂权限
            var factoryIds = RT.Service.Resolve<EmployeeEnterpriseSelectController>().GetAuthorityFactoryId();

            //当前用户所有的仓库权限
            var warehouseIds = RT.Service.Resolve<WarehouseController>().GetAuthorityWarehouseId();
            //所有标签的工厂Id
            var labelFactoryIds = itemLabels.Where(m => m.FactoryId.HasValue).Select(m => m.FactoryId.Value).ToList();
            var labelwarehouseIds = itemLabels.Where(m => m.WarehouseId.HasValue).Select(m => m.WarehouseId.Value).ToList();

            var resultFactoryExceptList = labelFactoryIds.Except(factoryIds);//取差集
            var resultWarehouseExceptList = labelwarehouseIds.Except(warehouseIds);

            if (resultFactoryExceptList.Any() && resultWarehouseExceptList.Any() && !warehouseIds.Contains(warehouseId.Value))
            {
                throw new ValidationException("提交失败，当前用户不存在工厂权限或仓库权限".L10N());
            }
        }

        /// <summary>
        /// 处理接口调用后信息
        /// </summary>
        /// <param name="scandResultInfos"></param>
        /// <param name="resourceId"></param>
        /// <param name="targetWoId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <param name="itemLabels"></param>
        /// <param name="mesMoveUpdateOnhandData"></param>
        /// <param name="moveSnData"></param>
        private void HandInterfaceInfo(List<ScandResultInfos> scandResultInfos, double resourceId, double? targetWoId, double? warehouseId, double? locationId, EntityList<ItemLabel> itemLabels, MesMoveUpdateOnhandData mesMoveUpdateOnhandData, MoveSnData moveSnData)
        {
            var wipResource = RF.GetById<WipResource>(resourceId);
            var factoryId = wipResource != null ? wipResource.FactoryId : 0;
            moveSnData.FactoryId = factoryId.HasValue ? factoryId.Value : 0;
            if (targetWoId.HasValue && targetWoId > 0)//工单有值
            {
                if (moveSnData.ReturnSnDatas.Any())
                {
                    moveSnData.TargetWarehouseId = warehouseId;
                    moveSnData.TargetStorageLocationId = locationId;
                    moveSnData.TargetWarehouseCode = RF.GetById<Warehouse>(warehouseId).Code;
                    moveSnData.TargetWoId = targetWoId.Value;

                    RT.Service.Resolve<StockOrderController>().WoMoveSn(moveSnData);
                }
                //开始回写物料标签 只回写moveSnData.ReturnSnDatas中存在的SN对应的数据
                foreach (var newScandResultInfo in scandResultInfos)
                {
                    if (moveSnData.ReturnSnDatas.Select(m => m.Sn).Contains(newScandResultInfo.Sn))
                    {
                        var scandResultInfoItemLabel = itemLabels.FirstOrDefault(m => m.Label == newScandResultInfo.Sn);
                        ReWriteItemLable(targetWoId, warehouseId, locationId, scandResultInfoItemLabel, newScandResultInfo);
                    }
                }

            }
            else
            {
                if (mesMoveUpdateOnhandData.LabelDatas.Any())
                {
                    RT.Service.Resolve<ILotLpnOnhand>().MesMoveUpdateOnhand(mesMoveUpdateOnhandData);
                }
            }
        }

        /// <summary>
        /// 更新物料标签的信息
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <param name="scandResultInfo"></param>
        /// <param name="scandResultInfoItemLabel"></param>
        private void UpdateItemLabeInfo(double? warehouseId, double? locationId, ScandResultInfos scandResultInfo, ItemLabel scandResultInfoItemLabel)
        {
            var exsitedItem = Query<ItemLabel>().Where(m => m.Label == scandResultInfoItemLabel.Label && m.ItemExtProp == scandResultInfoItemLabel.ItemExtProp
               && m.ItemId == scandResultInfoItemLabel.ItemId && m.Lot == scandResultInfoItemLabel.Lot && m.WarehouseId == warehouseId
               && m.StorageLocationId == locationId && scandResultInfoItemLabel.Id != m.Id).FirstOrDefault();//是否存在编码 物料属性 等相同的非自己的数据
            if (exsitedItem != null)
            {
                exsitedItem.Qty += scandResultInfo.Qty;
                scandResultInfoItemLabel.Qty -= scandResultInfo.Qty;
                if (scandResultInfo.WoId > 0 && scandResultInfoItemLabel.WorkOrderList.Count > 0)//子表需要移动
                {
                    var workOrderListItem = scandResultInfoItemLabel.WorkOrderList.FirstOrDefault(m => m.WorkOrderId == scandResultInfo.WoId);
                    if (workOrderListItem != null)
                    {
                        workOrderListItem.Qty -= scandResultInfo.Qty;
                        if (workOrderListItem.Qty == 0)
                        {
                            workOrderListItem.ItemLabelId = exsitedItem.Id;
                            scandResultInfoItemLabel.WorkOrderList.Remove(workOrderListItem);
                        }
                        else
                        {
                            var exsitedItemWorkOrder = exsitedItem.WorkOrderList.FirstOrDefault(m => m.WorkOrderId == scandResultInfo.WoId);
                            if (exsitedItemWorkOrder != null)
                            {
                                exsitedItemWorkOrder.Qty += scandResultInfo.Qty;
                                RF.Save(exsitedItemWorkOrder);
                            }
                            else
                            {
                                var newItem = new ItemLabelWorkOrder() { WorkOrderId = scandResultInfo.WoId, Qty = scandResultInfo.Qty, ItemLabelId = exsitedItem.Id };
                                RF.Save(newItem);
                            }
                        }

                        RF.Save(workOrderListItem);
                    }
                }
                RF.Save(exsitedItem);
                RF.Save(scandResultInfoItemLabel);

            }
            else
            {
                var newItem = new ItemLabel()
                {
                    Label = scandResultInfoItemLabel.Label,
                    ItemId = scandResultInfoItemLabel.ItemId,
                    Lot = scandResultInfoItemLabel.Lot,
                    WarehouseId = warehouseId,
                    StorageLocationId = locationId,
                    ItemExtProp = scandResultInfoItemLabel.ItemExtProp,
                    ItemExtPropName = scandResultInfoItemLabel.ItemExtPropName,
                    UnitId = scandResultInfoItemLabel.UnitId,
                    Qty = scandResultInfo.Qty,
                    FactoryId = scandResultInfoItemLabel.FactoryId
                };
                newItem.GenerateId();
                scandResultInfoItemLabel.Qty -= scandResultInfo.Qty;
                if (scandResultInfo.WoId > 0 && scandResultInfoItemLabel.WorkOrderList.Count > 0)//子表需要移动
                {
                    var workOrderListItem = scandResultInfoItemLabel.WorkOrderList.FirstOrDefault(m => m.WorkOrderId == scandResultInfo.WoId);
                    if (workOrderListItem != null)
                    {
                        workOrderListItem.Qty -= scandResultInfo.Qty;
                        if (workOrderListItem.Qty == 0)
                        {
                            workOrderListItem.ItemLabelId = newItem.Id;
                            scandResultInfoItemLabel.WorkOrderList.Remove(workOrderListItem);
                        }
                        else
                        {
                            var itemLabelWorkOrder = new ItemLabelWorkOrder() { WorkOrderId = scandResultInfo.WoId, Qty = scandResultInfo.Qty, ItemLabelId = newItem.Id };
                            RF.Save(newItem);
                            RF.Save(itemLabelWorkOrder);
                        }
                    }
                }
                RF.Save(newItem);
                RF.Save(scandResultInfoItemLabel);
            }
        }

        /// <summary>
        /// 获取挪料接口数据
        /// </summary>
        /// <param name="moveSnData"></param>
        /// <param name="scandResultInfo"></param>
        /// <param name="scandResultInfoItemLabel"></param>
        private static void GetMoveSnData(MoveSnData moveSnData, ScandResultInfos scandResultInfo, ItemLabel scandResultInfoItemLabel)
        {
            var tempItem = new ReturnSnData()
            {
                Qty = scandResultInfo.Qty,
                Sn = scandResultInfoItemLabel.Label,
                SourceStorageLocationId = scandResultInfoItemLabel.StorageLocationId,
                SourceWarehouseCode = scandResultInfoItemLabel.WarehouseCode,
                SourceWarehouseId = scandResultInfoItemLabel.WarehouseId,
                ItemId = scandResultInfoItemLabel.ItemId,
                ItemExtProp = scandResultInfoItemLabel.ItemExtProp,
                WoNo = scandResultInfo.WoNo,
                LotCode = scandResultInfoItemLabel.Lot,
            };
            moveSnData.ReturnSnDatas.Add(tempItem);

            if (scandResultInfo.WoId > 0)//工单有值获取产线
            {
                var resource = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(scandResultInfo.WoId);
                if (resource != null)
                {
                    tempItem.WorkShopId = resource.WorkShopId;
                }
            }
        }

        /// <summary>
        /// 获取挪料接口数据
        /// </summary>
        /// <param name="mesMoveUpdateOnhandData"></param>
        /// <param name="scandResultInfo"></param>
        /// <param name="scandResultInfoItemLabel"></param>
        private static void GetMesMoveUpdateonHandData(MesMoveUpdateOnhandData mesMoveUpdateOnhandData, ScandResultInfos scandResultInfo, ItemLabel scandResultInfoItemLabel)
        {
            var mesLabelData = new MesLabelData()
            {
                LabelNo = scandResultInfoItemLabel.Label,
                ItemExtProp = scandResultInfoItemLabel.ItemExtProp,
                LotCode = scandResultInfoItemLabel.Lot,
                ItemExtPropName = scandResultInfoItemLabel.ItemExtPropName,
                ItemId = scandResultInfoItemLabel.ItemId,
                StorageLocationId = scandResultInfoItemLabel.StorageLocationId,
                Qty = scandResultInfo.Qty
            };

            mesMoveUpdateOnhandData.LabelDatas.Add(mesLabelData);
        }

        /// <summary>
        /// 回写物料标签
        /// </summary>
        /// <param name="woId">目标工单</param>
        /// <param name="warehouseId">目标仓库</param>
        /// <param name="locationId">目标库位</param>
        /// <param name="itemLabel">挪动的物料标签</param>
        /// <param name="scandResultInfo">前端提交的信息</param>
        private void ReWriteItemLable(double? woId, double? warehouseId, double? locationId, ItemLabel itemLabel, ScandResultInfos scandResultInfo)
        {
            //目标物料标签唯一性确认 如果存在则取之增加数量
            var targetItemlabels = Query<ItemLabel>().Where(m => m.WarehouseId == warehouseId && m.StorageLocationId == locationId && m.ItemExtProp == itemLabel.ItemExtProp
              && m.Label == itemLabel.Label && m.ItemId == itemLabel.ItemId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (targetItemlabels.Count > 1)
            {
                throw new ValidationException("目标物料标签【{0}】在系统中存在多条，数据异常，请联系管理员".L10nFormat(scandResultInfo.Sn));
            }
            if (targetItemlabels.Any())//存在目标物料标签
            {
                var targetItemlabel = targetItemlabels.First();
                targetItemlabel.Qty += scandResultInfo.Qty;
                ItemLabelWorkOrder matchedWorkOrder = null;
                if (woId.HasValue)//有工单变动,取关联目标工单的
                {
                    matchedWorkOrder = targetItemlabel.WorkOrderList.FirstOrDefault(m => m.WorkOrderId == woId);
                }
                else//无新工单变化 则取原来工单
                {
                    matchedWorkOrder = targetItemlabel.WorkOrderList.FirstOrDefault(m => m.WorkOrderId == scandResultInfo.WoId);
                }
                if (matchedWorkOrder != null)
                {//目标关联工单增加相关数量
                    matchedWorkOrder.Qty += scandResultInfo.Qty;
                }
                else//不存在相关工单
                {
                    if (woId.HasValue || scandResultInfo.WoId > 0)
                    {
                        matchedWorkOrder = new ItemLabelWorkOrder();
                        matchedWorkOrder.WorkOrderId = woId.HasValue ? woId.Value : scandResultInfo.WoId;
                        matchedWorkOrder.Qty = scandResultInfo.Qty;
                        matchedWorkOrder.ItemLabelId = targetItemlabel.Id;
                        RF.Save(matchedWorkOrder);
                    }
                }
                RF.Save(targetItemlabel);

            }
            else//不存在则新建物料标签
            {
                ItemLabel newItemLabel = new ItemLabel();
                newItemLabel.Clone(itemLabel);
                newItemLabel.GenerateId();
                newItemLabel.WarehouseId = warehouseId;
                newItemLabel.StorageLocationId = locationId;
                newItemLabel.Qty = scandResultInfo.Qty;
                newItemLabel.WorkOrderList.Clear();
                RF.Save(newItemLabel);
                if (woId.HasValue || scandResultInfo.WoId > 0)
                {
                    var matchedWorkOrder = new ItemLabelWorkOrder();
                    matchedWorkOrder.WorkOrderId = woId.HasValue ? woId.Value : scandResultInfo.WoId;
                    matchedWorkOrder.Qty = scandResultInfo.Qty;
                    matchedWorkOrder.ItemLabelId = newItemLabel.Id;
                    RF.Save(matchedWorkOrder);
                }
            }

        }

        /// <summary>
        /// 创建退料单
        /// </summary>
        /// <param name="scandResultInfo"></param>
        /// <param name="scandResultInfoItemLabel"></param>
        /// <returns></returns>
        private MaterialReturn GetNewMaterialReturn(ScandResultInfos scandResultInfo, ItemLabel scandResultInfoItemLabel)
        {
            MaterialReturn materialReturn = new MaterialReturn()
            {
                ItemId = scandResultInfoItemLabel.ItemId,
                BatchNO = scandResultInfoItemLabel.Lot,
                AlreadyQty = (int)scandResultInfo.RemainingQty,
                NO = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialCode(),
                Qty = scandResultInfo.Qty,
                Employee = RF.GetById<SIE.Resources.Employee>(RT.Identity.Id),
                EmployeeId = RT.Identity.Id,
                Label = scandResultInfoItemLabel.Label,
                BadQty = 0,
                LabelId = scandResultInfoItemLabel.Id,
                ReturnWarehouseId = scandResultInfoItemLabel.WarehouseId,
                ReturnState = ReturnStates.Submitted,
                ReturnType = ReturnTypes.Normal,
                ReturnWarehouseLocationId = scandResultInfoItemLabel.StorageLocationId,
                ReturnReason = scandResultInfo.Reason.IsNullOrEmpty() ? RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialReasonDefault() : scandResultInfo.Reason,
                //获取资源
                WipResourceId = scandResultInfo.ResoureId,
                PersistenceStatus = PersistenceStatus.New,
                FactoryId = scandResultInfo.FactoryId.Value,
                WorkOrderId = scandResultInfo.WoId,
                ReturnReasonDesc = scandResultInfo.Desc,
                IsSerial = scandResultInfoItemLabel.IsSerialNumber,
                ItemExtProp = scandResultInfoItemLabel.ItemExtProp,
                ItemExtPropName = scandResultInfoItemLabel.ItemExtPropName
            };
            return materialReturn;
        }


        /// <summary>
        /// 获取工单已备料量
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="itemId"></param>
        /// <param name="ItemExtProp"></param>
        /// <param name="stockOrderDetails"></param>
        /// <returns></returns>
        private static decimal ComputeWorkOrderSurplus(double woId, double itemId, string ItemExtProp, EntityList<StockOrderDetail> stockOrderDetails)
        {
            //已备料数量
            var prepareQty = stockOrderDetails.Where(x => x.WorkOrderId == woId
                    && x.ItemId == itemId
                    && x.ItemExtProp == ItemExtProp
                    && x.StockState != StockState.Created
                    && x.StockState != StockState.ReCall
                    && x.StockState != StockState.Closed)
                .Sum(p => p.Qty - p.CancelQty);

            //已备料
            return prepareQty;
        }


        /// <summary>
        /// 推式
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <param name="itemLabel"></param>
        /// <param name="scandResultInfo"></param>
        /// <param name="stockOrderDetails"></param>
        /// <param name="workOrderBoms"></param>
        private void PushCheck(double? woId, double? warehouseId, double? locationId, ItemLabel itemLabel, ScandResultInfos scandResultInfo,
            EntityList<StockOrderDetail> stockOrderDetails, IList<WorkOrderBom> workOrderBoms
            )
        {
            if (scandResultInfo.Qty > scandResultInfo.RemainingQty && scandResultInfo.WoId > 0)
            {
                throw new ValidationException("物料标签【{0}】挪料失败，本次挪料关联工单的数量小于本次挪料数量".L10nFormat(itemLabel.Label));
            }
            //工单相同或者目标工单为空 认为工单相同
            if ((scandResultInfo.WoId == woId || !woId.HasValue) && (itemLabel.WarehouseId == warehouseId && itemLabel.StorageLocationId == locationId))
            {
                throw new ValidationException("物料标签【{0}】工单、仓库、库位与目标一致".L10nFormat(itemLabel.Label));
            }
            if (woId.HasValue&& woId!=0)//目标工单有值且不相同
            {
                WorkOrderBom exsitedItemBom = null;
                if (scandResultInfo.WoId != woId)
                {
                    if (!workOrderBoms.Any())
                    {
                        throw new ValidationException("物料编码【{0}】不是目标工单的需求物料，不能挪料!".L10nFormat(itemLabel.ItemCode));
                    }
                    else
                    {
                        exsitedItemBom = workOrderBoms.FirstOrDefault(m => m.ItemId == itemLabel.ItemId && m.ItemExtProp == itemLabel.ItemExtProp);
                        if (exsitedItemBom == null)
                        {
                            throw new ValidationException("物料编码【{0}】不是目标工单的需求物料，不能挪料!".L10nFormat(itemLabel.ItemCode));
                        }
                    }
                    var workOrderSurplus = ComputeWorkOrderSurplus(woId.Value, itemLabel.ItemId, itemLabel.ItemExtProp, stockOrderDetails);
                    var surplusQty = exsitedItemBom.RequireQty - workOrderSurplus;
                    if (scandResultInfo.Qty > surplusQty || surplusQty <= 0)
                    {
                        throw new ValidationException("物料标签【{0}】挪料数量超过工单剩余需求数，不能挪料!".L10nFormat(itemLabel.Label));
                    }
                }
            }
        }

        /// <summary>
        /// 检查传入参数
        /// </summary>
        /// <param name="barcodeIds"></param>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        private EntityList<ItemLabel> CheckInputParamas(List<double> barcodeIds, double? warehouseId, double? locationId)
        {
            if (!barcodeIds.Any())
            {
                throw new ValidationException("扫描记录不能为空".L10N());
            }
            if (!warehouseId.HasValue)
            {
                throw new ValidationException("请选择目标仓库".L10N());
            }
            if (!locationId.HasValue)
            {
                throw new ValidationException("请选择目标库位".L10N());
            }

            var itemLabels = barcodeIds.SplitContains(its => { return Query<ItemLabel>().Where(m => its.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()); });
            if (!itemLabels.Any())
            {
                throw new ValidationException("扫描的物料标签系统不存在".L10N());
            }

            return itemLabels;
        }

        /// <summary>
        /// 校验成功后更新物料标签的仓库和库位值
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="locationId"></param>
        /// <param name="itemLabel"></param>
        /// <param name="scandResultInfo"></param>
        private static void PullCheck(double? warehouseId, double? locationId,
            ItemLabel itemLabel, ScandResultInfos scandResultInfo)
        {
            if (itemLabel.WarehouseId == warehouseId && itemLabel.StorageLocationId == locationId)
            {
                throw new ValidationException("物料标签【{0}】提交失败，其仓库、库位与目标仓库、目标库位一致,不允许挪料".L10nFormat(itemLabel.Label));
            }
            if (scandResultInfo.RemainingQty <= 0)
            {
                throw new ValidationException("物料标签【{0}】可用数量为{1}，不能挪料".L10nFormat(itemLabel.Label, scandResultInfo.RemainingQty));
            }
            if (scandResultInfo.RemainingQty < scandResultInfo.Qty)
            {
                throw new ValidationException("{0}(标签号)挪料提交失败，本次退料数为{1}，标签剩余数量为{2}，挪料数量不能大于标签数量".L10nFormat(itemLabel.Label,
                    scandResultInfo.Qty, scandResultInfo.RemainingQty));
            }

        }

        /// <summary>
        /// 根据工单获取工单Bom物料
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工单BOM物料</returns>
        public virtual EntityList<Item> GetBomItems(double woId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Item>().Join<WorkOrderBom>((x, y) => x.Id == y.ItemId && y.WorkOrderId == woId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        ///获取工单的工单BOM
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual IList<WorkOrderBom> GetWorkOrderBomInfos(double woId)
        {
            return Query<WorkOrderBom>()
                .Join<WorkOrder>((b, w) => b.WorkOrderId == w.Id && w.Id == woId).ToList();
        }

    }
}
