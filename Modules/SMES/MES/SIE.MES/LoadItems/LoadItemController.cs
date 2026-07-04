using SIE.Core.Items;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.Common.WorkOrders;
using SIE.LES.LinesideWarehouses;
using SIE.LES.Reports;
using SIE.MES.BatchWIP.Products;
using SIE.MES.LoadItemRecords;
using SIE.MES.LoadItems.Enum;
using SIE.MES.LoadItems.Models;
using SIE.MES.LoadItems.ViewModels;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 上料控制器
    /// </summary>
    public class LoadItemController : DomainController
    {
        #region 上料
        /// <summary>
        /// 根据资源工序工位获取上料列表，只加载RemainderQty不为0的记录
        /// </summary>
        /// <param name="lineId">产线Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="filterSingleton">过滤单件（单体和半成品）</param>
        /// <returns>上料列表</returns>
        public virtual EntityList<LoadItem> GetLoadItemList(double lineId, double stationId, bool filterSingleton = false)
        {
            var query = Query<LoadItem>()
                .Where(p => p.ResourceId == lineId && p.StationId == stationId && p.Qty != 0);

            if (filterSingleton)
            {
                query.Where(p => p.SourceType != LoadItemSourceType.SingleLabel && p.SourceType != LoadItemSourceType.SN);
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据资源工序工位获取上料列表，只加载RemaindQty不为0的记录
        /// </summary>
        /// <param name="lineId">产线Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>上料列表</returns>
        public virtual EntityList<LoadItem> GetLoadItemEntityList(double lineId, double stationId,
            double workOrderId)
        {
            //过站时，按工序BOM的用量扣上料记录的数据，只扣上料记录中工单符合的物料标签。
            var query = Query<LoadItem>()
                .Where(p => p.ResourceId == lineId && p.StationId == stationId && p.WorkOrderId == workOrderId && p.Qty > 0);
            return query.ToList();
        }

        /// <summary>
        /// 根据资源ID获取该资源下所有工位的物料数量
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <returns>上料集合</returns>
        public virtual EntityList<LoadItem> GetLoadItems(double resourceId)
        {
            var query = Query<LoadItem>().Where(x => x.ResourceId == resourceId && x.StationId > 0 && x.Qty >= 0);
            return query.ToList();
        }

        /// <summary>
        /// 获取上料列表
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>上料列表</returns>
        public virtual EntityList<LoadItem> GetLoadItems(double resourceId, double stationId, double itemId)
        {
            return Query<LoadItem>().Where(p => p.Qty > 0 && p.ResourceId == resourceId && p.StationId == stationId && p.ItemId == itemId).ToList();
        }

        /// <summary>
        /// 根据工位ID/物料ID/工单ID获取上料信息
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>上料信息</returns>
        public virtual EntityList<LoadItem> GetLoadItems(double stationId, double itemId, double? workOrderId)
        {
            return Query<LoadItem>().Where(p => p.WorkOrderId == workOrderId && p.StationId == stationId && p.ItemId == itemId).ToList();
        }

        /// <summary>
        /// 根据物料标签参数获取上料信息
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="sourceId">来源标签ID</param>
        /// <returns>上料信息</returns>
        public virtual EntityList<LoadItem> GetLoadItemsBySourceId(double? workOrderId, double itemId, double sourceId)
        {
            return Query<LoadItem>().Where(p => p.WorkOrderId == workOrderId && p.ItemId == itemId && p.SourceId == sourceId && p.Qty > 0).ToList();
        }

        /// <summary>
        /// 根据工单获取上料信息
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual EntityList<LoadItem> GetLoadItemsByWoId(double workOrderId)
        {
            return Query<LoadItem>().Where(p => p.WorkOrderId == workOrderId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据多条件获取上料列表
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="woId">工单id</param>
        /// <param name="stationId">工位id</param>        
        /// <returns>上料列表</returns>
        public virtual EntityList<LoadItem> GetLoadItemByWoAndItem(double resourceId, double itemId, double woId, double? stationId)
        {
            return Query<LoadItem>().Where(p => p.ResourceId == resourceId && p.ItemId == itemId && p.WorkOrderId == woId)
                .WhereIf(stationId.HasValue, p => p.StationId == stationId)
                .ToList();
        }

        /// <summary>
        /// 判断条码是否已上料
        /// </summary>
        /// <param name="SourceCode">来源编码</param>
        /// <param name="sourceType">来源类型</param> 
        /// <returns>已上料返回true，否则返回false</returns>
        public virtual bool IsLoadItem(string SourceCode, LoadItemSourceType sourceType)
        {
            return Query<LoadItem>().Where(p => p.SourceCode == SourceCode && p.SourceType == sourceType).Count() > 0;
        }

        /// <summary>
        /// 根据工位判断是否包含上料列表，且RemainderQty不为0
        /// </summary>
        /// <param name="stationId">工位Id</param>
        /// <returns>包含返回true，否则返回false</returns>
        public virtual bool CheckStationItems(double stationId)
        {
            var result = Query<LoadItem>()
                .Where(p => p.StationId == stationId && p.Qty != 0)
                .Count();
            return result > 0;
        }

        /// <summary>
        /// 验证上料信息
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="dicLoadItemSourceType">可上料类型字典</param>
        /// <param name="toLoadItemWorkOrderId">上料的工单Id</param>
        /// <returns>换料条码信息</returns>
        public virtual EntityList<LoadItemBarcodeInfo> ValidateLoadItem(string barcode, Workcell workcell,
            Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType, double toLoadItemWorkOrderId)
        {
            EntityList<LoadItemBarcodeInfo> result = new EntityList<LoadItemBarcodeInfo>();
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            var barcoedInfo = new LoadItemBarcodeInfo() { Barcode = barcode };

            var toLoadItemWorkOrder = RF.GetById<WorkOrder>(toLoadItemWorkOrderId,new EagerLoadOptions().LoadWithViewProperty());
            if (toLoadItemWorkOrder == null)
            {
                throw new ValidationException("上料的工单为空，无法上料".L10N());
            }
            //获取上料工单的工单BOM校验扩展属性
            var woProcessBoms = toLoadItemWorkOrder.ProcessBomList;
            //查找产品条码
            WipProductVersion version = null;

            if (dicLoadItemSourceType != null
                && dicLoadItemSourceType.ContainsKey(LoadItemSourceType.SN)
                && dicLoadItemSourceType[LoadItemSourceType.SN])
            {
                version = GetProductVersion(barcode);
            }

            if (version != null)
            {
                barcoedInfo.Id = version.Id.ToString();//增加ID 用于简化版本 APP等数据区分
                barcoedInfo.Type = LoadItemSourceType.SN;
                barcoedInfo.ItemId = version.Product.ItemId;
                barcoedInfo.ItemCode = version.Product.Item.Code;
                barcoedInfo.ItemName = version.Product.Item.Name;
                barcoedInfo.Qty = 1;
                barcoedInfo.SourceId = version.Id;
                barcoedInfo.WipWorkOrderId = toLoadItemWorkOrderId;
                barcoedInfo.WipWorkOrderNo = toLoadItemWorkOrder.No;
                barcoedInfo.ItemExtProp = version.ItemExtProp;
                barcoedInfo.ItemExtPropName = version.ItemExtPropName;
                barcoedInfo.ProjectNo = toLoadItemWorkOrder.ProjectMaintainCode;
                result.Add(barcoedInfo);
            }
            else
            {
                //物料标签一定是可上料
                //获取物料标签
                var itemLabels = GetItemLabels(barcode);
                var woProjectNo = toLoadItemWorkOrder.ProjectMaintainCode.IsNullOrEmpty() ? "*" : toLoadItemWorkOrder.ProjectMaintainCode;
                var ids = itemLabels.Select(m => m.Id).ToList();

                var itemIds = itemLabels.Select(m => m.ItemId).ToList();
                var itemList = itemIds.SplitContains(tempIds =>
                {
                    return Query<Items.Item>().Where(p => tempIds.Contains(p.Id)).ToList();
                });

                foreach (var itemLabel in itemLabels)
                {
                    //校验项目号
                    var projectNo = itemLabel.ProjectNo.IsNullOrEmpty() ? "*" : itemLabel.ProjectNo;
                    if (woProjectNo != projectNo)
                        continue;
                    // 校验有效期
                    if (!AssemblyValityManager.IsValidity(itemLabel))
                    {
                        throw new ValidationException("上料失败，标签{0}已失效".L10nFormat(itemLabel.Label));
                    }


                    var barcoedInfoEntity = new LoadItemBarcodeInfo() { Barcode = barcode };
                    if (woProcessBoms.Any())//工序BOM存在信息，过来扩展属性不匹配的数据
                    {
                        var bomItem = woProcessBoms.FirstOrDefault(m => m.ItemId == itemLabel.ItemId && m.ItemExtProp == itemLabel.ItemExtProp);
                        if (bomItem == null)
                        {
                            continue;
                        }
                    }
                    var item = itemList.FirstOrDefault(m => m.Id == itemLabel.ItemId);

                    if (item == null)
                    {
                        throw new ValidationException("物料标签【{0}】的物料信息找不到，无法上料".L10nFormat(barcode));
                    }

                    barcoedInfoEntity.Qty = itemLabel.Qty;

                    SetBarcoedInfo(toLoadItemWorkOrderId, toLoadItemWorkOrder, itemLabel, barcoedInfoEntity, item);

                    result.Add(barcoedInfoEntity);
                }
            }


            if (!result.Any())
            {
                throw new ValidationException("物料标签条码[{0}]的可用数量为0或工单不匹配".L10nFormat(barcode));
            }

            return result;
        }

        /// <summary>
        /// 设置条码信息
        /// </summary>
        /// <param name="toLoadItemWorkOrderId"></param>
        /// <param name="toLoadItemWorkOrder"></param>
        /// <param name="itemLabel"></param>
        /// <param name="barcoedInfoEntity"></param>
        /// <param name="item"></param>
        private void SetBarcoedInfo(double toLoadItemWorkOrderId, WorkOrder toLoadItemWorkOrder, ItemLabel itemLabel, LoadItemBarcodeInfo barcoedInfoEntity, Items.Item item)
        {
            barcoedInfoEntity.Type = LoadItemSourceType.ItemLabel;
            barcoedInfoEntity.Id = itemLabel.Id.ToString();//增加Id 用于APP端区分
            barcoedInfoEntity.ItemId = itemLabel.ItemId;
            barcoedInfoEntity.ItemCode = item.Code;
            barcoedInfoEntity.ItemName = item.Name;
            barcoedInfoEntity.ConsumeMode = item.ConsumeMode;
            barcoedInfoEntity.SourceId = itemLabel.Id;
            barcoedInfoEntity.ItemExtProp = itemLabel.ItemExtProp;
            barcoedInfoEntity.ItemExtPropName = itemLabel.ItemExtPropName;
            barcoedInfoEntity.ProjectNo = itemLabel.ProjectNo;
            //记录下使用了哪个投入工单的Id
            barcoedInfoEntity.WipWorkOrderId = toLoadItemWorkOrderId;
            barcoedInfoEntity.WipWorkOrderNo = toLoadItemWorkOrder.No;
            barcoedInfoEntity.StorageLocationCode = itemLabel.StorageLocationCode;
            barcoedInfoEntity.WarehouseName = itemLabel.WarehouseCode;
            barcoedInfoEntity.Label = itemLabel.Label;
            barcoedInfoEntity.LotNo = itemLabel.Lot;
            barcoedInfoEntity.IsSerialNumber = itemLabel.IsSerialNumber;
        }

        /// <summary>
        /// 获取物料标签 多个的情况
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public virtual List<ItemLabel> GetItemLabels(string label)
        {
            ////物料标签的标签状态必须是已配送或者已接收或者已下料的才能上料 
            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(label);

            if (!itemLabels.Any())
            {
                throw new ValidationException("未找到物料标签条码[{0}]".L10nFormat(label));
            }

            if (itemLabels.All(x => x.Qty <= 0))
            {
                throw new ValidationException("物料标签条码[{0}]的可用数量为0".L10nFormat(label));
            }

            return itemLabels.Where(x => x.Qty > 0).ToList();
        }


        /// <summary>
        /// 验证选择后的物料标签
        /// </summary>
        /// <param name="changedBarcodeInfo"></param>
        /// <param name="workcell"></param>
        /// <param name="validateCurrentProcessBom">验证本工序的工序BOM</param>
        /// <returns></returns>
        private void ValidationItemLabel(LoadItemBarcodeInfo changedBarcodeInfo, Workcell workcell, bool validateCurrentProcessBom)
        {
            if (changedBarcodeInfo == null)
            {
                throw new EntityNotFoundException(nameof(changedBarcodeInfo));
            }

            if (changedBarcodeInfo.Qty == 0)
            {
                throw new ValidationException("不能上料，物料标签[{0}]可用数量为:{1}".L10nFormat(changedBarcodeInfo.Label, changedBarcodeInfo.Qty));
            }


            int count = 0;

            //是否不验证工单工序BOM
            if (validateCurrentProcessBom)
            {
                //校验是否存在工序bom需要物料
                count = Query<WorkOrderProcessBom>()
                  .Where(x => x.WorkOrderId == changedBarcodeInfo.WipWorkOrderId
                      && x.ProcessId == workcell.ProcessId && x.ItemId == changedBarcodeInfo.ItemId
                      && x.ItemExtProp == changedBarcodeInfo.ItemExtProp)
                  .Count();
            }
            else
            {
                // 校验是否存在工序bom需要物料
                count = Query<WorkOrderProcessBom>()
                  .Where(x => x.WorkOrderId == changedBarcodeInfo.WipWorkOrderId
                      && x.ItemId == changedBarcodeInfo.ItemId
                      && x.ItemExtProp == changedBarcodeInfo.ItemExtProp)
                  .Count();
            }

            if (count <= 0)
            {
                throw new ValidationException("物料标签的物料【{0}】与工位在制工单【{1}】的工序BOM中无匹配项，无法上料！"
                    .L10nFormat(RF.GetById<Item>(changedBarcodeInfo.ItemId).Code, RF.GetById<WorkOrder>(changedBarcodeInfo.WipWorkOrderId).No));
            }
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="label">条码</param>
        /// <returns>物料编码</returns>
        public virtual ItemLabel GetItemLabel(string label)
        {
            ////物料标签的标签状态必须是已配送或者已接收或者已下料的才能上料 
            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(label);
            if (itemLabel == null)
            {
                return itemLabel;
            }

            if (itemLabel.Qty == 0)
            {
                throw new ValidationException("不能上料，物料标签[{0}]剩余数量为:{1}".L10nFormat(label, itemLabel.Qty));
            }

            return itemLabel;
        }

        /// <summary>
        /// 新的上料逻辑
        /// </summary>
        /// <param name="barcodeInfo">上料条码信息</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="validateCurrentProcessBom">验证本工序的工序BOM</param>


        public virtual void NewLoadItem(LoadItemBarcodeInfo barcodeInfo, Workcell workcell, bool validateCurrentProcessBom = true)
        {
            if (barcodeInfo == null)
            {
                throw new ValidationException("条码信息丢失，请检查".L10N());
            }

            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            ValidationItemLabel(barcodeInfo, workcell, validateCurrentProcessBom);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                AddLoadItem(barcodeInfo, workcell);

                tran.Complete();
            }
        }

        /// <summary>
        /// 边缘端物料标签上料
        /// </summary>
        /// <param name="barcode">条码</param>
        public virtual void LoadEdgeItemLabel(string barcode)
        {
            //更新物料标签状态和剩余数量
            DB.Update<ItemLabel>()
                .Set(p => p.Qty, 0)
                .Where(p => p.Label == barcode)
                .Execute();
        }

        /// <summary>
        /// 新增上料信息
        /// </summary>
        /// <param name="barcodeInfo">上料标签信息</param>
        /// <param name="workcell">工作单元</param> 
        /// <returns>上料</returns>
        public virtual void AddLoadItem(LoadItemBarcodeInfo barcodeInfo, Workcell workcell)
        {
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            if (barcodeInfo == null)
            {
                throw new ArgumentNullException(nameof(barcodeInfo));
            }

            //新增到上料记录表

            switch (barcodeInfo.Type)
            {
                case LoadItemSourceType.DistributionBill:
                    break;
                case LoadItemSourceType.DistributionLabel:
                    break;
                case LoadItemSourceType.ItemLabel:
                    {
                        var itemLabel = ItemLabelAfterLoadItemProcess(barcodeInfo, workcell);
                    }
                    break;
                case LoadItemSourceType.SingleLabel:
                    break;
                case LoadItemSourceType.SN:
                    break;
                default:
                    break;
            }
            var now = RF.Find<LoadItem>().GetDbTime();

            var item = RF.GetById<SIE.Items.Item>(barcodeInfo.ItemId, new EagerLoadOptions().LoadWithViewProperty());
            //保存上料
            var loadItem = new LoadItem()
            {
                ResourceId = workcell.ResourceId,
                StationId = workcell.StationId,
                ItemId = barcodeInfo.ItemId,
                Shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(workcell.ResourceId, DateTime.Now),
                SourceCode = barcodeInfo.Barcode,
                LoadQty = barcodeInfo.Qty,
                Qty = Math.Round(barcodeInfo.Qty, item.UnitPrecision ?? 3),
                NgQty = 0m,
                SourceType = barcodeInfo.Type,
                SourceId = barcodeInfo.SourceId,
                WorkOrderId = barcodeInfo.WipWorkOrderId,
                ItemExtProp = barcodeInfo.ItemExtProp,
                ItemExtPropName = barcodeInfo.ItemExtPropName,
                ProjectNo = barcodeInfo.ProjectNo
            };
            RF.Save(loadItem);
            //记录上料记录
            var res = new LoadItemsRecord()
            {
                Item = loadItem.Item,
                ItemCode = loadItem.ItemCode,
                ItemName = loadItem.ItemName,

                Qty = loadItem.Qty.ToString(),
                Station = loadItem.Station,
                SourceCode = loadItem.SourceCode,
                Resource = loadItem.Resource,
                LoadDownQty = loadItem.LoadQty,
                OpareteTime = now,
                WorkOrder = loadItem.WorkOrder,
                OparetorName = RT.Identity.Name,
                OpareteType = OpareteType.LoadItem,
                ItemExtPropName = loadItem.ItemExtPropName,
                ProjectNo = barcodeInfo.ProjectNo,
                SourceType = loadItem.SourceType,
                SourceId = loadItem.Id
            };
            if (loadItem.Resource.FactoryId.HasValue)
            {
                res.FactoryName = loadItem.Resource.Factory.Name;
            }
            RF.Save(res);
        }

        /// <summary>
        /// 生成工单耗用记录
        /// </summary>
        /// <param name="barcodeInfo"></param>
        /// <param name="workcell"></param>
        /// <param name="itemLabel"></param>
        void CreateWoCostItem(LoadItemBarcodeInfo barcodeInfo, Workcell workcell, ItemLabel itemLabel)
        {
            var workOrderBom = Query<WorkOrderBom>().Where(p => p.WorkOrderId == barcodeInfo.WipWorkOrderId && p.ItemId == barcodeInfo.ItemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (workOrderBom == null)
                return;
            var woCostItem = RT.Service.Resolve<BackflushMaterialController>().CreateDeductItems(
                barcodeInfo.DisplayBarCode,
                workcell.ResourceId,
                workcell.ProcessId,
                workcell.StationId,
                itemLabel.FactoryId ?? 0,
                barcodeInfo.WipWorkOrderId,
                barcodeInfo.Qty,
                new EntityList<WorkOrderBom>() { workOrderBom },
                RetrospectType.Single
            ).FirstOrDefault();
            woCostItem.Qty = barcodeInfo.Qty;
            woCostItem.CostItemLabelId = itemLabel.Id;
            woCostItem.WarehouseId = itemLabel.WarehouseId;
            woCostItem.StorageId = itemLabel.StorageLocationId;
            woCostItem.State = WoCostItemState.Submitted;
            woCostItem.SubmiterId = RT.IdentityId;
            woCostItem.SubmitTime = DateTime.Now;

            RF.Save(woCostItem);

            //更新工单Bom耗料数
            RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woCostItem.WorkOrderId, new List<double> { woCostItem.ItemId });

        }

        /// <summary>
        /// 物料标签上料后的处理
        /// </summary>
        /// <param name="barcodeInfo"></param>
        /// <exception cref="ValidationException"></exception>
        private ItemLabel ItemLabelAfterLoadItemProcess(LoadItemBarcodeInfo barcodeInfo, Workcell workcell)
        {
            ItemLabel itemLabel = RF.GetById<ItemLabel>(barcodeInfo.SourceId, new EagerLoadOptions().LoadWithViewProperty());

            if (itemLabel == null)
            {
                throw new ValidationException("未找到物料标签[{0}]".L10nFormat(barcodeInfo.Label));
            }

            if (itemLabel.Qty == 0)
            {
                throw new ValidationException("不能上料，物料标签[{0}]可用数量为:{1}".L10nFormat(barcodeInfo.Label, itemLabel.Qty));
            }
            var wo = RF.GetById<WorkOrder>(barcodeInfo.WipWorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
            //校验项目号
            var woProjectNo = wo.ProjectMaintainCode.IsNullOrEmpty() ? "*" : wo?.ProjectMaintainCode;
            var projectNo = itemLabel.ProjectNo.IsNullOrEmpty() ? "*" : itemLabel.ProjectNo;
            if (projectNo != woProjectNo)
            {
                throw new ValidationException("不能上料，物料标签[{0}]项目号[{1}]与工单项目号[{2}]不一致".L10nFormat(barcodeInfo.Label, projectNo,woProjectNo));
            }
            //维护产线线边仓信息
            LinesideWarehouse lineWh = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(wo.WorkShopId, wo.ResourceId);

            //上料成功后，调用WMS上料接口，建发运单，扣减物料标签所在库位的合格库存。（标签的库位为空时，不用调用此接口）
            if (itemLabel.StorageLocationId.HasValue && lineWh != null)
            {
                MesUpdateOnhandData mesUpdateOnhandData = new MesUpdateOnhandData()
                {
                    // 操作类型 0-上料 1-下料 2-倒扣非工序BOM物料 3-接收 ,只有下料和接收是增加库位库存、其他都是减库存
                    OpType = 0,
                    WoId = barcodeInfo.WipWorkOrderId,
                };

                if (wo != null)
                {
                    mesUpdateOnhandData.WoNo = wo.No;
                    mesUpdateOnhandData.EnterpriseId = wo.WorkShopId;
                }

                var employee = RF.GetById<Resources.Employee>(RT.IdentityId);
                if (employee != null)
                {
                    mesUpdateOnhandData.EmpCode = employee.Code;
                }

                var labelData = new MesLabelData
                {
                    LabelNo = itemLabel.Label,
                    IsFail = false,
                    WarehouseId = itemLabel.WarehouseId ?? 0,
                    WarehouseCode = itemLabel.WarehouseCode,
                    StorageLocationId = itemLabel.StorageLocationId,
                    StorageLocationCode = itemLabel.StorageLocationCode,
                    //数量为标签剩余数量
                    Qty = itemLabel.Qty,
                    ItemExtProp = barcodeInfo.ItemExtProp,
                    ItemExtPropName = barcodeInfo.ItemExtPropName,
                    ProjectNo = barcodeInfo.ProjectNo,
                    LotCode = barcodeInfo.LotNo,
                    ItemId = barcodeInfo.ItemId,
                    ItemCode = itemLabel.ItemCode,
                    WorkOrderId = barcodeInfo.WipWorkOrderId,
                    WorkOrderNo = wo?.No,
                };
                //工单占用不足时,进行自动挪料
                var woDemandQty = RT.Service.Resolve<WoDemandReportController>().GetWoDemandReportQty(labelData, true);
                if (woDemandQty <= 0)
                    throw new ValidationException("工单[{0}]物料[{1}]在线边仓[{2}]剩余可用数不足，无法上料".L10nFormat(wo?.No, itemLabel.ItemCode, itemLabel.WarehouseCode));

                barcodeInfo.Qty = woDemandQty > itemLabel.Qty ? itemLabel.Qty : woDemandQty;
                labelData.Qty = barcodeInfo.Qty;
                mesUpdateOnhandData.LabelDatas.Add(labelData);

                if (itemLabel.SourceType == LabelSource.Receive)
                {
                    //更新MES线边库存
                    RT.Service.Resolve<WoDemandReportController>().MesUpdateOnhand(mesUpdateOnhandData.Copy());
                }

                //生成工单耗用单
                CreateWoCostItem(barcodeInfo, workcell, itemLabel);

                try
                {
                    RT.Service.Resolve<ILotLpnOnhand>().MesUpdateOnhand(mesUpdateOnhandData);
                }
                catch (Exception ex)
                {
                    throw new ValidationException("调用 WMS 接口扣减库失败，详细信息：{0}".L10nFormat(ex.GetExceptionMessage()));
                }

            }

            //更新标签可用数量
            itemLabel.Qty = itemLabel.Qty - barcodeInfo.Qty;

            // 更新标签开始时间
            AssemblyValityManager.ValidityStart(itemLabel, false);
            RF.Save(itemLabel);
            return itemLabel;
        }

        /// <summary>
        /// 获取换料条码信息（匹配属性）
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="keyItem">关键物料</param>
        /// <returns>换料条码信息</returns>
        public virtual LoadItemBarcodeInfo GetChangedBarcode(string barcode, WipProductProcessKeyItem keyItem)
        {
            if (barcode == null || keyItem == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            var barcoeInfo = new LoadItemBarcodeInfo() { Barcode = barcode };
            decimal qty = 0m;

            if (keyItem.SourceType == LoadItemSourceType.SN)
            {
                var version = GetProductVersion(barcode);
                if (version == null)
                {
                    throw new ValidationException("未找到产品条码[{0}]".L10nFormat(barcode));
                }
                var itemExtProp = version.Product.ItemExtProp;

                ValidatePropertyValue(version.Product.ItemId, itemExtProp, keyItem);

                barcoeInfo.Type = LoadItemSourceType.SN;
                barcoeInfo.ItemId = version.Product.ItemId;
                qty = 1;
                barcoeInfo.ItemExtProp = itemExtProp;
                barcoeInfo.ItemExtPropName = itemExtProp;
            }
            else
            {
                var itemLabel = GetItemLabel(barcode);
                if (itemLabel == null)
                {
                    throw new ValidationException("未找到标签条码[{0}]".L10nFormat(barcode));
                }
                else
                {
                    ValidatePropertyValue(itemLabel.ItemId, itemLabel, keyItem);
                    barcoeInfo.Type = LoadItemSourceType.ItemLabel;
                    barcoeInfo.ItemId = itemLabel.ItemId;
                    qty = itemLabel.Qty;
                }
            }

            barcoeInfo.Qty = qty;

            return barcoeInfo;

        }

        /// <summary>
        /// 获取换料条码信息（匹配属性）
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="keyItem">关键物料</param>
        /// <returns>换料条码信息</returns>
        public virtual LoadItemBarcodeInfo GetChangedBarcode(string barcode, BatchWipProductProcessKeyItem keyItem)
        {
            if (barcode == null || keyItem == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            var barcoeInfo = new LoadItemBarcodeInfo() { Barcode = barcode };
            var propertyValues = "";
            var propertyValuesName = "";
            var qty = 0m;
            if (keyItem.SourceType == LoadItemSourceType.SN)
            {
                var version = GetProductVersion(barcode);
                if (version == null)
                {
                    throw new ValidationException("未找到产品条码[{0}]".L10nFormat(barcode));
                }
                propertyValues = version.Product.ItemExtProp;
                propertyValuesName = version.Product.ItemExtPropName;
                ValidatePropertyValue(version.Product.ItemId, propertyValues, keyItem);
                barcoeInfo.Type = LoadItemSourceType.SN;
                barcoeInfo.ItemId = version.Product.ItemId;
                qty = 1;
            }
            else
            {
                var itemLabel = GetItemLabel(barcode);
                if (itemLabel == null)
                {
                    throw new ValidationException("未找到标签条码[{0}]".L10nFormat(barcode));
                }
                else
                {
                    propertyValues = itemLabel.ItemExtProp;
                    propertyValuesName = itemLabel.ItemExtPropName;
                    ValidatePropertyValue(itemLabel.ItemId, propertyValues, keyItem);
                    barcoeInfo.Type = LoadItemSourceType.ItemLabel;
                    barcoeInfo.ItemId = itemLabel.ItemId;
                    barcoeInfo.ProjectNo = itemLabel.ProjectNo;
                    qty = itemLabel.Qty;
                }
            }

            barcoeInfo.Qty = qty;
            barcoeInfo.ItemExtProp = propertyValues;
            barcoeInfo.ItemExtPropName = propertyValuesName;
            return barcoeInfo;
        }

        /// <summary>
        /// 获取产品版本
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>生产产品版本</returns>
        public virtual WipProductVersion GetProductVersion(string barcode)
        {
            var version = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersion(barcode);

            if (version == null)
            {
                return version;
            }

            if (!version.IsFinish)
            {
                throw new ValidationException("不允许上料，半成品{0}未完工下线".L10nFormat(barcode));
            }

            if (version.IsScrapped)
            {
                throw new ValidationException("不允许上料，半成品{0}已报废".L10nFormat(barcode));
            }

            if (RT.Service.Resolve<WipProductVersionController>().GetWipKeyItem(barcode) != null)
            {
                throw new ValidationException("半成品条码{0}已被使用".L10nFormat(barcode));
            }
            return version;
        }

        /// <summary>
        /// 换料匹配原关键件的物料编码和扩展属性
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="keyItem">关键件</param>
        private void ValidatePropertyValue(double itemId, ItemLabel itemLabel, WipProductProcessKeyItem keyItem)
        {
            if (keyItem.ItemId != itemId)
            {
                throw new ValidationException("物料不匹配".L10N());
            }

            if (itemLabel.ItemExtProp != keyItem.ItemExtProp)
            {
                throw new ValidationException("物料{0}扩展属性不匹配".L10nFormat(keyItem.Item.Code));
            }
        }

        /// <summary>
        /// 匹配换料属性
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="key"></param>
        /// <param name="keyItem"></param>
        private void ValidatePropertyValue(double itemId, string key, WipProductProcessKeyItem keyItem)
        {
            if (keyItem.ItemId != itemId)
            {
                throw new ValidationException("物料不匹配".L10N());
            }
            if (key != keyItem.ItemExtProp)
            {
                throw new ValidationException("物料{0}扩展属性不匹配".L10nFormat(keyItem.Item.Code));
            }
        }

        /// <summary>
        /// 匹配换料属性
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="propertyValues">物料属性</param>
        /// <param name="keyItem">关键件</param>
        private void ValidatePropertyValue(double itemId, string propertyValues, BatchWipProductProcessKeyItem keyItem)
        {
            if (keyItem.ItemId != itemId)
            {
                throw new ValidationException("物料不匹配".L10N());
            }
            if (propertyValues != keyItem.ItemExtProp)
            {
                throw new ValidationException("物料{0}属性不匹配".L10nFormat(keyItem.Item.Code));
            }
        }

        /// <summary>
        /// 获取上料记录
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<LoadItem> GetLoadItems(List<double> itemIds)
        {
            return itemIds.SplitContains(tempIds =>
            {
                return Query<LoadItem>().Where(p => p.Qty > 0 && tempIds.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取上料基本信息
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual List<LoadItemBaseData> GetLoadItemBaseDatas(List<double> itemIds)
        {
            List<LoadItemBaseData> loadItemBaseDatas = new List<LoadItemBaseData>();
            itemIds.SplitDataExecute(tempIds =>
            {
                var list = Query<LoadItem>()
                .Where(p => p.Qty > 0 && tempIds.Contains(p.ItemId))
                .Select(p => new
                {
                    p.Id,
                    p.Qty,
                    p.ItemId,
                    p.ItemExtProp,
                    p.ItemExtPropName,
                    p.ProjectNo,
                    p.ResourceId,
                    p.WorkOrderId,
                })
                .ToList<LoadItemBaseData>();
                loadItemBaseDatas.AddRange(list);
            });
            return loadItemBaseDatas;
        }
        #endregion

        #region 下料
        /// <summary>
        /// 加载下料数据，加载未确认状态
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <param name="lineId">产线资源Id</param>
        /// <param name="stationId">工位Id</param>
        /// <returns>下料列表</returns>
        public virtual EntityList<UnloadItem> GetUnloadItemList(double processId, double lineId, double stationId)
        {
            var query = Query<UnloadItem>()
                .Where(p => p.ResourceId == lineId && p.StationId == stationId && p.State == UnloadState.UnConfirm);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 正常下料
        /// </summary>
        /// <param name="loadItemId">上料Id</param>
        /// <param name="qty">数量</param>
        public virtual void UnloadItem(double loadItemId, decimal qty)
        {
            if (qty <= 0)
            {
                throw new ValidationException("下料数量不能小于等于0，请重新输入数量".L10N());
            }

            UnloadItem(loadItemId, qty, null);
        }


        /// <summary>
        /// 不良下料
        /// </summary>
        /// <param name="loadItemId">上料Id</param>
        /// <param name="defects">缺陷列表</param>
        public virtual void UnloadItem(double loadItemId, List<DefectData> defects)
        {
            if (defects == null)
            {
                throw new ValidationException("缺陷列表不能为空，选择缺陷！".L10N());
            }

            UnloadItem(loadItemId, defects.Sum(p => p.Qty).ConvertTo<decimal>(), defects);
        }

        /// <summary>
        /// 下料
        /// </summary>
        /// <param name="loadItemId">下料Id</param>
        /// <param name="qty">数量</param>
        /// <param name="defects">缺陷列表</param>        
        protected virtual void UnloadItem(double loadItemId, decimal qty, List<DefectData> defects)
        {
            var loadItem = RF.GetById<LoadItem>(loadItemId, new EagerLoadOptions().LoadWithViewProperty());

            if (loadItem == null)
            {
                throw new ValidationException("该上料记录不存在，请刷新再操作".L10N());
            }
            //转换单位精度
            qty = Math.Round(qty, loadItem.UnitPrecision ?? 3, MidpointRounding.AwayFromZero);

            var itemLabel = RF.GetById<ItemLabel>(loadItem.SourceId, new EagerLoadOptions().LoadWithViewProperty());


            if (loadItem.Qty < qty)
            {
                throw new ValidationException("下料数量[{0}]不能比剩余数量[{1}]大".L10nFormat(qty, loadItem.Qty));
            }

            var unloadItem = CreateUnLoadItem(loadItem, qty, defects);

            //更新标签数量为剩余数量，更新状态为已下料
            if (loadItem.SourceType == LoadItemSourceType.ItemLabel)
            {
                if (itemLabel == null)
                {
                    throw new ValidationException("物料标签为空".L10N());
                }

                if (itemLabel.IsSerialNumber == true && loadItem.Qty != qty)
                {
                    throw new ValidationException("【序列号管理】的物料标签不允许部分下料".L10N());
                }

                //不良下料质量状态改为不合格
                if (unloadItem.IsNg)
                {
                    itemLabel.NgQty = itemLabel.NgQty + qty;
                }
                else
                {
                    //更新物料标签 【可用数量】
                    itemLabel.Qty = itemLabel.Qty + qty;
                }

            }

            //更新上料记录
            loadItem.Qty -= qty;
            loadItem.UnloadQty += qty;

            if (loadItem.Qty == 0)
            {
                loadItem.PersistenceStatus = PersistenceStatus.Deleted;
            }

            // 有效期
            AssemblyValityManager.ValidityEnd(itemLabel);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(unloadItem);

                if (itemLabel != null)
                {
                    RF.Save(itemLabel);

                    //调用WMS下料接口，增加物料标签所在库位的合格库存（标签的库位为空时，不用调用此接口）
                    UpdateWmsOnhandWhenUnlaodItem(itemLabel, unloadItem.IsNg, unloadItem.Qty, loadItem.WorkOrderId);
                }

                RF.Save(loadItem);

                tran.Complete();
            }
        }

        /// <summary>
        /// 下料更新WMS库存
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="isNg">是否不合格</param>
        /// <param name="qty">库存变化数量</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="isReWorkRepair">是否返工返修</param>
        public virtual void UpdateWmsOnhandWhenUnlaodItem(ItemLabel itemLabel, bool isNg, decimal qty, double? workOrderId, bool isReWorkRepair = false)
        {
            if (itemLabel == null)
            {
                return;
            }

            if (itemLabel.StorageLocationId.HasValue)
            {
                MesUpdateOnhandData mesUpdateOnhandData = new MesUpdateOnhandData
                {
                    // 操作类型 0-上料 1-下料 2-倒扣非工序BOM物料 3-接收 ,只有下料和接收是增加库位库存、其他都是减库存
                    OpType = 1,
                    WoId = workOrderId,
                };

                WorkOrder workOrder = null;
                if (workOrderId.HasValue)
                {
                    workOrder = RF.GetById<WorkOrder>(workOrderId.Value, new EagerLoadOptions().LoadWith(WorkOrder.PackageRuleDetailListProperty));

                    if (workOrder != null)
                    {
                        mesUpdateOnhandData.WoNo = workOrder.No;
                        mesUpdateOnhandData.EnterpriseId = workOrder.WorkShopId;
                        var packageRuleDetail = workOrder.PackageRuleDetailList.FirstOrDefault();
                        if (packageRuleDetail != null)
                        {
                            mesUpdateOnhandData.ItemPackRuleDetailId = packageRuleDetail.DetailId;
                        }
                    }
                }

                var employee = RF.GetById<Resources.Employee>(RT.IdentityId);

                if (employee != null)
                {
                    mesUpdateOnhandData.EmpCode = employee.Code;
                }
                if (isReWorkRepair)
                {
                    mesUpdateOnhandData.IsRepair = true;//序列号返工返修下料会生产新的标签 该标记传给WMS创建新标签
                }

                mesUpdateOnhandData.LabelDatas.Add(new MesLabelData
                {
                    LabelNo = itemLabel.Label,
                    IsFail = isNg,//【不合格】设备为【是否不良下料】的值
                    WarehouseId = itemLabel.WarehouseId ?? 0,
                    WarehouseCode = itemLabel.WarehouseCode,
                    StorageLocationId = itemLabel.StorageLocationId,
                    StorageLocationCode = itemLabel.StorageLocationCode,
                    //数量为下料数量
                    Qty = qty,
                    ItemExtProp = itemLabel.ItemExtProp,
                    ItemExtPropName = itemLabel.ItemExtPropName,
                    ProjectNo = itemLabel.ProjectNo,
                    ItemId = itemLabel.ItemId,
                    ItemCode = itemLabel.ItemCode,
                    LotCode = itemLabel.Lot,
                    WorkOrderId = workOrderId,
                    WorkOrderNo = workOrder?.No,

                });

                if (itemLabel.SourceType == LabelSource.Receive)
                {
                    //更新MES线边库存
                    RT.Service.Resolve<WoDemandReportController>().MesUpdateOnhand(mesUpdateOnhandData.Copy());
                }
                RT.Service.Resolve<ILotLpnOnhand>().MesUpdateOnhand(mesUpdateOnhandData);
            }
        }

        /// <summary>
        /// 创建下料
        /// </summary>
        /// <param name="loadItem">上料记录</param>
        /// <param name="qty">数量</param>
        /// <param name="defects">物料缺陷数据</param>
        /// <returns>下料</returns>
        public virtual UnloadItem CreateUnLoadItem(LoadItem loadItem, decimal qty, List<DefectData> defects)
        {
            if (loadItem == null)
            {
                throw new  ValidationException ("系统不存在该上料记录，请刷新后操作".L10N());
            }

            var unloadItem = new UnloadItem
            {
                LoadItemQty = loadItem.LoadQty,
                Qty = qty,
                RemainderQty = qty,
                Shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(loadItem.ResourceId, DateTime.Now),
                State = UnloadState.UnConfirm,
                StationId = loadItem.StationId,
                ResourceId = loadItem.ResourceId,
                SourceCode = loadItem.SourceCode,
                SourceId = loadItem.SourceId,
                SourceType = loadItem.SourceType,
                Item = loadItem.Item,
                WorkOrderId = loadItem.WorkOrderId
            };
            unloadItem.ItemExtProp = loadItem.ItemExtProp;
            unloadItem.ItemExtPropName = loadItem.ItemExtPropName;
            unloadItem.ProjectNo = loadItem.ProjectNo;

            if (defects != null)  //不良退料
            {
                loadItem.NgQty += qty;
                unloadItem.GenerateId();
                unloadItem.IsNg = true;
                foreach (var defect in defects)
                {
                    var unloadItemDefect = new UnloadItemDefect
                    {
                        Qty = defect.Qty.ConvertTo<decimal>(),
                        DefectId = defect.DefectId,
                    };

                    unloadItem.DefectList.Add(unloadItemDefect);
                }
            }


            return unloadItem;
        }

        /// <summary>
        /// 一键下料
        /// </summary>
        /// <param name="unloadList">一键下料列表</param>
        public virtual void UnloadAllItem(List<UnloadAllItemViewModel> unloadList)
        {
            if (unloadList == null)
            {
                throw new ArgumentNullException(nameof(unloadList));
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                unloadList.ForEach(e => UnloadItem(e.LoadItem.Id, e.LoadItem.Qty));
                tran.Complete();
            }
        }


        /// <summary>
        /// 获取下料列表
        /// </summary>
        /// <param name="SourceCode">来源编号</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns>下料列表</returns>
        public virtual EntityList<UnloadItem> GetUnloadItems(string SourceCode, double resourceId, LoadItemSourceType sourceType)
        {
            return Query<UnloadItem>().Where(p => p.Qty > 0 && p.RemainderQty > 0 && p.ResourceId == resourceId && p.SourceCode == SourceCode && p.SourceType == sourceType).ToList();
        }

        /// <summary>
        /// 获取下料列表
        /// </summary>
        /// <param name="SourceCode">来源编号</param>
        /// <param name="sourceTypes">来源类型集合</param>
        /// <returns>下料列表</returns>
        public virtual EntityList<UnloadItem> GetUnloadItems(string SourceCode, List<LoadItemSourceType> sourceTypes)
        {
            var types = sourceTypes.Select(p => (int)p);
            return Query<UnloadItem>().Where(p => p.Qty > 0 && p.RemainderQty > 0 && p.SourceCode == SourceCode && types.Contains((int)p.SourceType)).ToList();
        }

        /// <summary>
        /// 判断条码是否已下料
        /// </summary>
        /// <param name="SourceCode">来源编号</param>
        /// <param name="sourceType">来源类型</param> 
        /// <returns>已下料返回true，否则返回false</returns>
        public virtual bool IsUnloadItem(string SourceCode, LoadItemSourceType sourceType)
        {
            return Query<UnloadItem>().Where(p => p.Qty > 0 && p.RemainderQty > 0 && p.SourceCode == SourceCode && p.SourceType == sourceType).Count() > 0;
        }

        /// <summary>
        /// 根据资源工序工位获取上料列表
        /// </summary>        
        /// <param name="resourceId">产线Id</param>
        /// <param name="stationId">工位Id</param>
        /// <returns>上料列表</returns>
        public virtual EntityList<UnloadAllItemViewModel> GetUnloadAllItems(double resourceId, double stationId)
        {
            //过滤掉单体/半成品上料记录保证上料有工单记录，进行匹对，实际上单体/半成品是不会提前上料的
            var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(resourceId, stationId, true);

            EntityList<UnloadAllItemViewModel> unloadList = new EntityList<UnloadAllItemViewModel>();
            loadItems.ForEach(loadItem =>
            {
                unloadList.Add(new UnloadAllItemViewModel
                {
                    IsLoadItem = true,
                    LoadItem = loadItem,
                });
            });

            return unloadList;
        }
        #endregion

        #region 挪料

        /// <summary>
        /// 验证挪料
        /// </summary>
        /// <param name="loadItemId">上料Id</param>
        /// <param name="qty">数量</param>
        /// <param name="workcell">工作单元信息</param>
        public virtual void ValidateMoveItem(double loadItemId, decimal qty, Workcell workcell)
        {
            var loadItem = ValidateStandardMoveItem(loadItemId, qty, workcell);
            ////验证目标工作站在制工单对应的工单工序BOM是否匹配
            var wipLineWorkOrder = RT.Service.Resolve<AssemblyController>().GetWipResourceWorkOrder(workcell);
            if (wipLineWorkOrder == null)
                return;
            var workOrder = wipLineWorkOrder.WorkOrder;
            ////上料时，工序BOM属性没有不提示，限制只上工序BOM物料。
            if (workOrder.ProcessBomList.Any())
            {
                var bom = workOrder.ProcessBomList
                    .FirstOrDefault(p => p.ProcessId == workcell.ProcessId && p.ItemId == loadItem.ItemId);

                if (bom == null)
                {
                    throw new ValidationException("目标工作单元在制工单[{0}]工序BOM不存在物料[{1}]".L10nFormat(workOrder.No, loadItem.Item.Code));
                }
                if (bom.ItemExtProp != loadItem.ItemExtProp)
                {
                    throw new ValidationException("目标工作单元在制工单[{0}]工序BOM物料[{1}]的属性不相等]".L10nFormat(workOrder.No, loadItem.Item.Code));
                }
            }
        }

        /// <summary>
        /// 验证上料采集的挪料（必须的）
        /// </summary>
        /// <param name="loadItemId">上料Id</param>
        /// <param name="qty">数量</param>
        /// <param name="workcell">工作单元信息</param>
        /// <returns>上料</returns>
        private LoadItem ValidateStandardMoveItem(double loadItemId, decimal qty, Workcell workcell)
        {
            var loadItem = GetById<LoadItem>(loadItemId);
            if (loadItem == null)
            {
                throw new EntityNotFoundException(typeof(LoadItem), loadItemId);
            }

            if (loadItem.Qty <= 0)
            {
                throw new ValidationException("剩余数量为 0 不允许进行挪料".L10N());
            }
            if (loadItem.Qty < qty)
            {
                throw new ValidationException("挪料数量:{0} 不能大于剩余数量:{1}".L10nFormat(qty, loadItem.Qty));
            }
            return loadItem;
        }

        /// <summary>
        /// 加载挪料数据（加载3天内的挪料数据）
        /// </summary>
        /// <param name="resourceId">资源ID</param>        
        /// <param name="stationId">工位ID</param>
        /// <returns>工位挪料列表</returns>
        public virtual EntityList<MoveItem> GetMoveItemList(double resourceId, double stationId)
        {
            return Query<MoveItem>()
                .Where(p => p.ResourceId == resourceId && p.StationId == stationId && p.CreateDate >= DateTime.Now.AddDays(-3)).OrderByDescending(p => p.CreateDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion
    }
}