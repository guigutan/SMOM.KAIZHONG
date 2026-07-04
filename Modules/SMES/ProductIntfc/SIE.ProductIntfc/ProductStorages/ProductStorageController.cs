using Newtonsoft.Json;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Sort;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Inspection;
using SIE.EventMessages.MES.ProductStorage;
using SIE.EventMessages.MES.WIP;
using SIE.EventMessages.Receipt;
using SIE.Items;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packings;
using SIE.ProductIntfc.Configs;
using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.InspSettings;
using SIE.ProductIntfc.OutputProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SIE.EventMessages.DataTrace.PushMsg;
using SIE.DataSync.DataSyncPush;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库查询实体数据
    /// </summary>
    public class ProductStorageController : DomainController, IToStorageBarcode, IProductStorage, IDirectPackage
    {
        /// <summary>
        /// ProductStorageController控制器
        /// </summary>
        private readonly string _productStorageControllerString = "ProductStorageController";

        /// <summary>
        /// IToStorageBarcode字符
        /// </summary>
        private readonly string _iToStorageBarcodeString = "IToStorageBarcode";

        /// <summary>
        /// 成品入库查询实体数据
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns>实体列表</returns>
        public virtual EntityList<StorageWorkOrder> GetProductStorages(ProductStorageCriteria criteria)
        {
            var query = DB.Query<StorageWorkOrder>().Where(p => p.State != Core.WorkOrders.WorkOrderState.CancelRelease && p.State != Core.WorkOrders.WorkOrderState.Release);
            if (!criteria.Barcode.IsNullOrEmpty())
            {
                query.Exists<ToStorageBarcode>((x, y) => y.Where(e => e.StorageWorkOrderId == x.Id).Join<ToStorageBarcodeDetail>((c, d) => c.Id == d.ToStorageBarcodeId
                    && d.Barcode == criteria.Barcode));
            }
            else
                query.Exists<ToStorageBarcode>((x, y) => y.Where(e => x.Id == e.StorageWorkOrderId));
            if (criteria.ShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.ShopId);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (!criteria.WorkOrder.IsNullOrEmpty())
                query.Where(p => p.No.Contains(criteria.WorkOrder));
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取待入库条码
        /// </summary>
        /// <param name="id">入库工单Id</param>
        /// <param name="isSuccess">入库成功true,false</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>待入库条码列表</returns>
        public virtual EntityList<ToStorageBarcode> GetToStoreBarcode(double id, bool isSuccess, PagingInfo pagingInfo = null, List<OrderInfo> orderInfos = null)
        {
            return Query<ToStorageBarcode>().Where(p => p.StorageWorkOrderId == id && p.IsStored == isSuccess).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取已入库信息
        /// </summary>
        /// <param name="id">入库工单ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>已入库信息列表</returns>
        public virtual EntityList<InStorageBill> GetInStoreBarcode(double id, PagingInfo pagingInfo = null, List<OrderInfo> orderInfos = null)
        {
            return Query<InStorageBill>().Where(p => p.StorageWorkOrderId == id).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取入库信息
        /// </summary>
        /// <returns>入库条码集合</returns>
        public virtual EntityList<ToStorageBarcode> GetToStoreBarcodesByCodes(double? workOrderId, List<string> barcodes)
        {
            return Query<ToStorageBarcode>().Where(p => p.StorageWorkOrderId == workOrderId && barcodes.Contains(p.Barcode)).ToList();
        }

        /// <summary>
        /// 获取待入库条码明细ByCode
        /// </summary>       
        /// <returns>条码明细</returns>        
        public virtual ToStorageBarcodeDetail GetToStoreBarcodeDetailByCode(double workOrderId, string barcode, bool isBatch)
        {
            return Query<ToStorageBarcodeDetail>().Where(p => p.ToStorageBarcode.StorageWorkOrderId == workOrderId && !p.ToStorageBarcode.IsStored && (barcode == p.Barcode && !isBatch || barcode == p.Batch && isBatch)).FirstOrDefault();
        }

        /// <summary>
        /// 获取待入库条码明细ByCodes集合
        /// </summary>      
        /// <returns>条码明细集合</returns>
        public virtual EntityList<ToStorageBarcodeDetail> GetToStoreBarcodeDetailByCodes(double workOrderId, List<string> barcodes, bool isBatch)
        {
            return Query<ToStorageBarcodeDetail>().Where(p => p.ToStorageBarcode.StorageWorkOrderId == workOrderId && !p.ToStorageBarcode.IsStored && (barcodes.Contains(p.Barcode) && !isBatch || barcodes.Contains(p.Batch) && isBatch)).ToList();
        }

        /// <summary>
        /// 获取待入库条码明细列表
        /// </summary>
        /// <param name="barcodeId">入库条码Id</param>
        /// <returns>待入库条码明细列表</returns>
        public virtual EntityList<ToStorageBarcodeDetail> GetToStoreBarcodeDetails(double barcodeId)
        {
            return Query<ToStorageBarcodeDetail>().Where(p => p.ToStorageBarcodeId == barcodeId).ToList();
        }

        /// <summary>
        /// 自动入库
        /// </summary>
        /// <returns>自动报检结果信息</returns>
        public virtual string AutoStorageBarcode()
        {
            StringBuilder message = new StringBuilder();
            try
            {
                ////查找达到数量的条码，入库，若5个入一次，一共10个，则入库两次
                var paramList = GetParamList(InspDimension.BatchQty);
                if (paramList.Count == 0) return message.Append("没有设置了入库维度是数量的入库参数！").ToString();
                var list = paramList.Select(e => e.ItemId);
                ////查找包含产品的入库工单
                var woList = GetStorageWorkOrder(list.ToList());
                if (woList.Count == 0) return message.Append("没有获取到工单信息！").ToString();
                var woIds = woList.Select(e => e.Id);
                var barcodeList = Query<ToStorageBarcode>().NotExists<InStorageBarcodeDetail>((x, y) => y.Where(e => e.Barcode == x.Barcode))
                    .Where(p => woIds.Contains(p.StorageWorkOrderId) && !p.IsStored).ToList();
                if (barcodeList.Count == 0) return message.Append("没有获取到待入库条码信息！").ToString();
                var grouplist = barcodeList.OrderBy(p => p.CreateDate).GroupBy(p => p.StorageWorkOrderId);
                foreach (var item in grouplist)
                {
                    bool iferror = false;
                    var woNo = item.Select(p => p.StorageWorkOrder.No).FirstOrDefault();
                    try
                    {
                        //获取条码数量=参数数量
                        var itemid = item.Select(e => e.StorageWorkOrder.ProductId).FirstOrDefault();
                        var sumcount = (int)paramList.Where(p => p.ItemId == itemid).Select(e => e.Qty).FirstOrDefault();
                        var itemList = item.ToList();
                        if (itemList.Count >= sumcount)
                        {
                            Logging.LogManager.GetLogger("job").Info("开始工单[{0}]自动成品入库".L10nFormat(woNo));
                            ////满足数量才执行,获取数量的条数                     
                            int c = itemList.Count / sumcount;
                            for (int i = 1; i <= c; i++)
                            {
                                var entityList = item.Skip(sumcount * (i - 1)).Take(sumcount).AsEntityList();
                                try
                                {
                                    ToStorageIn(entityList);
                                }
                                catch (Exception ex)
                                {
                                    if (!iferror)
                                    {
                                        message.Append(new StringBuilder("工单[{0}]自动成品入库失败".L10nFormat(woNo) + ":" + ex.Message + "。"));
                                    }

                                    iferror = true;
                                }
                            }

                            Logging.LogManager.GetLogger("job").Info("结束工单[{0}]自动成品入库".L10nFormat(woNo));
                        }
                    }
                    catch (Exception ex)
                    {
                        message.AppendLine("工单[{0}]自动成品入库失败，失败信息：{1}".L10nFormat(woNo, ex.Message));
                    }
                    finally
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                message.Append("自动成品入库失败" + ex.Message);
            }

            return message.ToString();
        }

        /// <summary>
        /// 新增入库条码,无包装工序情况
        /// </summary>
        /// <param name="workorderid">工单Id</param>
        /// <param name="storeBarcode">入库-产品条码,或扫包装的条码</param>
        /// <param name="isSingleStroe">是否单体入库</param>
        /// <param name="collectDate">采集时间</param>
        /// <param name="newRule">新包装规则</param>
        /// <param name="barcodeQty">条码数量</param>
        /// <param name="batchBarcode">批次条码</param>
        /// <param name="dispatchTaskId">任务单ID</param>
        public virtual void AddNewStoreBarcode(double workorderid, string storeBarcode, bool isSingleStroe = true, DateTime? collectDate = null, WorkOrderPackageRuleDetail newRule = null, decimal barcodeQty = 0, string batchBarcode = "", double? dispatchTaskId = null)
        {
            //// 注释原因：参数只管控要不要显示数据出来          
            var wo = RF.GetById<WorkOrder>(workorderid);
            if (wo == null) return;
            bool isBatch = false;
            var batchRule = RT.Service.Resolve<ItemController>().GetBatchRule(wo.ProductId);
            if ((batchRule != null && batchRule.RetrospectType == Core.Items.RetrospectType.Batch) || dispatchTaskId != null)
            {
                isBatch = true;
            }

            var _tostoragebarcode = GetStorageBarcode(workorderid, storeBarcode);
            var packUnitList = wo.PackageRuleDetailList;
            bool _isMasterUnit = false;
            string _unit = string.Empty;
            string _ruleName = string.Empty;
            var packUnit = new WorkOrderPackageRuleDetail();
            if (newRule != null)
            {
                packUnit = newRule;
                _isMasterUnit = packUnit.PackageUnit.IsMasterUnit;
                _ruleName = packUnit.Detail.ItemPackageRule.Name;
            }
            else
            {
                if (packUnitList.Count == 0 || dispatchTaskId != null)
                {
                    _isMasterUnit = true;
                    _ruleName = string.Empty;
                }
                else
                {
                    packUnit = packUnitList.Where(p => p.IsInStockLabel || p.PackageUnit.IsMasterUnit).OrderByDescending(p => p.IsInStockLabel).FirstOrDefault();
                    _isMasterUnit = packUnit.PackageUnit.IsMasterUnit;
                    _ruleName = packUnit.Detail?.ItemPackageRule?.Name;
                }
            }

            _unit = _isMasterUnit ? "主单位" : packUnit.PackageUnit.Name;

            var detailBarcodes = new EntityList<ToStorageBarcodeDetail>();
            if (_isMasterUnit && isSingleStroe)
            {
                //主单位入库，批次包装主单位入库IsSingleStore=false
                if (_tostoragebarcode != null) return;
                ToStorageBarcodeDetail label = new ToStorageBarcodeDetail()
                {
                    Barcode = isBatch ? string.Empty : storeBarcode,
                    CollectDate = (DateTime)collectDate,
                    Batch = isBatch ? storeBarcode : string.Empty,
                    FinishQty = barcodeQty
                };

                detailBarcodes.Add(label);
                SaveNewToBarcode(workorderid, storeBarcode, _unit, detailBarcodes, _ruleName, _isMasterUnit, null, batchBarcode);
            }
            else
            {
                if (_isMasterUnit && _tostoragebarcode != null) return;
                var packRelation = RT.Service.Resolve<PackageController>().GetPackingRelationByPackNo(storeBarcode);
                var packItemBarcodes = GetProductBarcodesByPackingNo(packRelation, packUnit.PackageUnitId, isBatch);
                ////判断是否当前包装是否=入库包装单位
                if (packItemBarcodes != null && packItemBarcodes.Count > 0)
                {
                    if (_tostoragebarcode != null)
                    {
                        packItemBarcodes = packItemBarcodes.Where(p => !_tostoragebarcode.ToStorageBarcodeDetailList.Select(e => e.Barcode).Contains(p.Barcode)).AsEntityList();
                    }

                    detailBarcodes.AddRange(packItemBarcodes);
                    SaveNewToBarcode(workorderid, storeBarcode, _unit, detailBarcodes, _ruleName, _isMasterUnit, _tostoragebarcode, packRelation.BatchNo);
                }
                ////补漏逻辑，2019.1.14批次打包的事务包了两层导致，批次打包修复后不需要补漏
            }
        }

        /// <summary>
        /// 根据包装号条码寻找下面的所有产品条码
        /// </summary>
        /// <param name="packingNoRelation">包装信息</param>
        /// <param name="packageUnitId">包装单位Id</param>
        /// <param name="isBatch">是否批次</param>
        /// <returns>包装号</returns>
        public virtual EntityList<ToStorageBarcodeDetail> GetProductBarcodesByPackingNo(BatchPackingRelation packingNoRelation, double packageUnitId, bool isBatch)
        {
            var ctl = RT.Service.Resolve<PackageController>();
            var item = packingNoRelation;
            if (item == null || item.PackageUnitId != packageUnitId)
            {
                return new EntityList<ToStorageBarcodeDetail>();
            }
            else
            {
                EntityList<ToStorageBarcodeDetail> listArr = new EntityList<ToStorageBarcodeDetail>();
                EntityList<BatchPackingRelation> sonEntityList = new EntityList<BatchPackingRelation>();
                sonEntityList.Add(item);
                sonEntityList = ctl.GetStoreRelationByUnitId(sonEntityList);
                //空箱
                if (sonEntityList == null)
                {
                    return new EntityList<ToStorageBarcodeDetail>();
                }

                foreach (var items in sonEntityList)
                {
                    if (isBatch)
                    {
                        //批次包装只需取到最底下的包装号，因为批次的的最底层的单体也会被打包
                        ToStorageBarcodeDetail detail = new ToStorageBarcodeDetail();
                        detail.Batch = items.PackageNo;
                        detail.CollectDate = items.CreateDate;
                        detail.FinishQty = items.ItemQty;
                        listArr.Add(detail);
                    }
                    else
                    {
                        var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelByRelationId(items.Id);
                        if (labels.Count > 0)
                        {
                            foreach (var itemLable in labels)
                            {
                                ToStorageBarcodeDetail detail = new ToStorageBarcodeDetail();
                                detail.Barcode = itemLable.Label;
                                detail.CollectDate = itemLable.CreateDate;
                                detail.FinishQty = itemLable.Qty;
                                listArr.Add(detail);
                            }
                        }
                    }
                }

                return listArr;
            }
        }

        /// <summary>
        /// 工单改变入库单位，需把遗漏的条码入库
        /// </summary>
        /// <param name="workorderId">工单Id</param>
        /// <param name="oldRule">原包装规则</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="newRule">新包装规则</param>
        /// <param name="packRelation">包装关系</param>
        public virtual void AddLessBarCode(double workorderId, WorkOrderPackageRuleDetail oldRule, double itemId, WorkOrderPackageRuleDetail newRule = null, PackingRelation packRelation = null)
        {
            bool isBatch = false;
            var batchRule = RT.Service.Resolve<ItemController>().GetBatchRule(itemId);
            if (batchRule != null && batchRule.RetrospectType == Core.Items.RetrospectType.Batch)
            {
                isBatch = true;
            }

            if (packRelation != null)
            {
                //查询这个relation下的所有符合的没入库的条码                 
                //找新包装下的所有条码
                var list = RT.Service.Resolve<PackageController>().GetPackRelations(workorderId, oldRule.PackageUnitId, packRelation);
                if (list.Count == 0) return;
                var effectList = list.Where(e => !string.IsNullOrEmpty(e.PackageNo)).ToList();

                ////过滤没打包完的数据，没打包完的需要手工打包触发打包事件
                foreach (var item in effectList)
                {
                    var packItemBarcodes = GetProductBarcodesByPackingNo(item, item.PackageUnitId, isBatch);
                    if (!CheckBarcodeDetail(workorderId, packItemBarcodes.Select(e => e.Barcode).ToList()))
                    {
                        //判断已有条码明细不再插入数据                          
                        AddNewStoreBarcode(workorderId, item.PackageNo, false, null, newRule);
                    }
                }
            }
            else
            {   //查询所有漏了的入库条码

                ////入库变成主单位或空
                if (newRule == null || newRule.PackageUnit.IsMasterUnit)
                {
                    if (isBatch)
                    {
                        var batchList = GetBatchIsFinish(workorderId);
                        foreach (var item in batchList)
                        {
                            AddNewStoreBarcode(workorderId, item.Bid, true, item.UpdateDate, newRule, item.Qty);
                        }
                    }
                    else
                    {
                        var productVersion = GetProductVersionsIsFinish(workorderId);
                        foreach (var item in productVersion)
                        {
                            AddNewStoreBarcode(workorderId, item.Sn, true, item.UpdateDate, newRule);
                        }
                    }
                }
                else
                {
                    var newRelation = RT.Service.Resolve<PackageController>().GetPackingRelationByWorkOrderId(workorderId, newRule.PackageUnitId);

                    //判断包装单位是否由大变小包装
                    if (RT.Service.Resolve<PackageController>().GetParentRelationByUnitId(newRelation, oldRule.PackageUnitId) != null)
                    {
                        //找新包装下的所有条码
                        var list = RT.Service.Resolve<PackageController>().GetPackRelations(workorderId, newRelation.PackageUnitId);
                        var effectList = list.Where(e => !string.IsNullOrEmpty(e.PackageNo)).ToList();

                        ////过滤没打包完的数据，没打包完的需要手工打包触发打包事件
                        foreach (var item in effectList)
                        {
                            var packItemBarcodes = GetProductBarcodesByPackingNo(item, item.PackageUnitId, isBatch);
                            if (!CheckBarcodeDetail(workorderId, packItemBarcodes.Select(e => e.Barcode).ToList()))
                            {
                                //判断已有条码明细不再插入数据                          
                                AddNewStoreBarcode(workorderId, item.PackageNo, false, null, newRule);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断已有条码
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="list">条码列表</param>
        /// <returns>bool</returns>
        public virtual bool CheckBarcodeDetail(double workOrderId, List<string> list)
        {
            return Query<ToStorageBarcodeDetail>().Where(p => p.ToStorageBarcode.StorageWorkOrderId == workOrderId && list.Contains(p.Barcode)).FirstOrDefault() != null;
        }

        /// <summary>
        /// 关联获取没入库的单体条码
        /// </summary>
        /// <param name="workorderId">工单Id</param>
        /// <returns>完工单体条码列表</returns>
        public virtual EntityList<WipProductVersion> GetProductVersionsIsFinish(double workorderId)
        {
            return Query<WipProductVersion>().Where(p => p.WorkOrderId == workorderId && p.IsFinish).NotExists<ToStorageBarcodeDetail>((x, y) => y.Where(e => e.ToStorageBarcode.StorageWorkOrderId == x.WorkOrderId && e.Barcode == x.Sn)).ToList();
        }

        /// <summary>
        /// 关联获取没入库的批次条码
        /// </summary>
        /// <param name="workorderId">workorderId</param>
        /// <returns>批次关系列表</returns>
        public virtual EntityList<BatchRelation> GetBatchIsFinish(double workorderId)
        {
            return Query<BatchRelation>().Where(p => p.WorkOrderId == workorderId && p.IsFinish)
                .NotExists<ToStorageBarcodeDetail>((x, y) => y.Where(e => e.ToStorageBarcode.StorageWorkOrderId == x.WorkOrderId && e.Barcode == x.Bid)).ToList();
        }

        /// <summary>
        ///  获取入库工单信息
        /// </summary>
        /// <param name="ids">产品Id</param>
        /// <returns>入库工单列表</returns>
        public virtual EntityList<StorageWorkOrder> GetStorageWorkOrder(List<double> ids)
        {
            return Query<StorageWorkOrder>().Where(p => ids.Contains(p.ProductId)).Exists<ToStorageBarcode>((x, y) => y.Where(e => !e.IsStored)).ToList();
        }

        /// <summary>
        /// 参数中是否有配置物料入库
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <returns>true/false</returns>
        public virtual bool IsHasParam(double woId)
        {
            return Query<ProductStorageParam>().Join<WorkOrder>((x, y) => x.ItemId == y.ProductId && y.Id == woId).FirstOrDefault() != null;
        }

        /// <summary>
        /// 获取成品入库参数列表
        /// </summary>
        /// <param name="inspDimension">维度</param>
        /// <returns>列表数据</returns>
        public virtual EntityList<ProductStorageParam> GetParamList(InspDimension inspDimension)
        {
            return Query<ProductStorageParam>().Where(p => p.InspDimension == inspDimension).ToList();
        }

        /// <summary>
        /// 订阅包装条码生成入库事件
        /// </summary>
        /// <param name="packingEvent">packingEvent</param>       
        public virtual void AddPackingStore(DoPackingEvent packingEvent)
        {
            if (packingEvent.OuterPackingRelations.ToList().Count > 0)
            {
                var wo = RF.GetById<WorkOrder>(packingEvent.OuterPackingRelations[0].WorkOrderId);
                if (wo == null) return;
                var unit = wo.PackageRuleDetailList.Where(p => p.IsInStockLabel || p.PackageUnit.IsMasterUnit).OrderByDescending(p => p.IsInStockLabel).FirstOrDefault();

                foreach (var item in packingEvent.OuterPackingRelations)
                {
                    if (!item.PackageNo.IsNullOrEmpty() && item.PackageUnitId == unit.PackageUnitId)
                    {
                        AddNewStoreBarcode(item.WorkOrderId, item.PackageNo, false);
                    }
                }
            }
        }

        /// <summary>
        /// 保存入库条码
        /// </summary>
        /// <param name="workorderid">工单Id</param>
        /// <param name="storeBarcode">入库条码</param>
        /// <param name="level">包装层级</param>
        /// <param name="detailBarcodes">包装对应底下的条码</param>
        /// <param name="packageRuleName">包装规则名称</param>
        /// <param name="isMasterUnit"></param>     
        /// <param name="toStorageBarcode">待入库条码</param>
        /// <param name="batchBarcode">生产批条码</param>
        /// <param name="dispatchTaskId">任务单ID</param>
        public virtual void SaveNewToBarcode(double workorderid, string storeBarcode, string level, EntityList<ToStorageBarcodeDetail> detailBarcodes,
            string packageRuleName, bool isMasterUnit, ToStorageBarcode toStorageBarcode = null, string batchBarcode = "", double? dispatchTaskId = null)
        {
            var toBarcode = new ToStorageBarcode();
            if (toStorageBarcode != null && !toStorageBarcode.IsStored)
            {
                toBarcode = toStorageBarcode;
            }
            else
            {
                toBarcode.StorageWorkOrderId = workorderid;
                toBarcode.Barcode = storeBarcode;
                toBarcode.Level = level;
                toBarcode.PackageRuleName = packageRuleName;
                toBarcode.Qty = 0;
                toBarcode.BatchBarcode = batchBarcode;
                toBarcode.IsMasterUnit = isMasterUnit;
                toBarcode.DispatchTaskId = dispatchTaskId;
            }

            detailBarcodes.ForEach(e =>
           {
               e.InspectionResult = Common.InspectionResult.Pass;
               toBarcode.Qty += e.FinishQty;
           });

            toBarcode.ToStorageBarcodeDetailList.AddRange(detailBarcodes);

            RF.Save(toBarcode);
        }

        /// <summary>
        /// 查询入库条码
        /// </summary>
        /// <param name="workOrderId">工单</param>
        /// <param name="barcode">条码</param>
        /// <returns>入库条码</returns>
        public virtual ToStorageBarcode GetStorageBarcode(double workOrderId, string barcode)
        {
            return Query<ToStorageBarcode>().Where(p => p.StorageWorkOrderId == workOrderId && p.Barcode == barcode).FirstOrDefault();
        }

        /// <summary>
        /// 查询入库条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isStored">入库</param>
        /// <param name="workorderId">工单ID</param>
        /// <returns>入库条码</returns>
        public virtual ToStorageBarcode GetStorageBarcode(string barcode, double workorderId, bool isStored)
        {
            return Query<ToStorageBarcode>().Where(p => p.Barcode == barcode && p.StorageWorkOrderId == workorderId && p.IsStored == isStored).FirstOrDefault();
        }

        /// <summary>
        /// 根据ID集合获取待入库条码
        /// </summary>
        /// <param name="storeBarIds">Id集合</param>
        /// <returns>待入库列表集合</returns>
        public virtual EntityList<ToStorageBarcode> GetStorageBarcode(IEnumerable<double> storeBarIds)
        {
            return Query<ToStorageBarcode>().Where(p => storeBarIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 接口实现入库条码保存,同步不进行成品报检的下线条码数据
        /// </summary>
        /// <param name="toStorageEvent">接口实体</param>
        public virtual void ToStorageBarcode(ToStorageBarcodeEvent toStorageEvent)
        {
            SaveToStorageBarcodeLog(toStorageEvent);
            AddNewStoreBarcode(toStorageEvent.WorkOrderId, toStorageEvent.Barcode, true, toStorageEvent.CollectionDate, null, toStorageEvent.Qty, toStorageEvent.BatchBarcode, toStorageEvent.DispatchTaskId);
        }

        /// <summary>
        /// 保存成品入库日志
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        private void SaveToStorageBarcodeLog(ToStorageBarcodeEvent toStorageEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(toStorageEvent);
                var inputValue = "成品入库事件:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = _iToStorageBarcodeString,
                    Method = "ToStorageBarcode",
                    ControllerName = _productStorageControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 对接入库信息到WMS
        /// </summary>
        /// <param name="entityList">待入库条码</param>
        /// <param name="billNo">已入库单号</param>
        /// <param name="deliverDate">接收时间</param>
        /// <param name="billId">已入库单号Id</param>
        public virtual void StoreToWMS(EntityList<ToStorageBarcode> entityList, string billNo, DateTime deliverDate, double billId)
        {
            ProductToAsnEvent asnEvent = new ProductToAsnEvent();
            List<RemoteAsnEvent> paramList = new List<RemoteAsnEvent>();
            var storeWorkOrder = entityList[0].StorageWorkOrder;
            var masterInfo = new RemoteAsnEvent();
            masterInfo.RequireId = billId;
            masterInfo.RequireNo = billNo;
            masterInfo.OrderType = storeWorkOrder.Product.Type == ItemType.Product ? 10 : 20;
            masterInfo.PriorityType = 0;
            masterInfo.DeliveryDate = deliverDate;
            masterInfo.WarehouseId = GetWarehouseId();
            masterInfo.EnterpriseId = storeWorkOrder.WorkShopId;
            double? ruleid = null;
            double? ruledetailid = null;
            var storerule = storeWorkOrder.PackageRuleDetailList.FirstOrDefault(p => p.IsInStockLabel && !p.PackageUnit.IsMasterUnit);
            if (storerule != null)
            {
                ruleid = storerule.Detail?.ItemPackageRuleId;
                ruledetailid = storerule.DetailId;
            }

            masterInfo.DetailList = new List<RemoteAsnDTLEvent>();
            foreach (var toStorageBarcode in entityList)
            {
                //对接到WMS明细条码赋值
                RemoteAsnDTLEvent detail = new RemoteAsnDTLEvent();
                detail.WorkNo = storeWorkOrder.No;
                detail.ItemId = storeWorkOrder.ProductId;
                //detail.LPN = item.Barcode;
                detail.LPNPackageRuleId = ruleid;
                detail.LPNPackageRuleDetailId = ruledetailid;
                detail.ExpectQty = toStorageBarcode.Qty;

                detail.BarCode = toStorageBarcode.Barcode;
                detail.LotAtt01 = toStorageBarcode.CreateDate;
                detail.LotAtt04 = toStorageBarcode.BatchBarcode;

                //成品入库信息到WMS，增加传递工单扩展属性
                detail.ItemExtProp = storeWorkOrder.ItemExtProp;
                detail.ItemExtPropName = storeWorkOrder.ItemExtPropName;
                detail.ProjectNo = storeWorkOrder.ProjectMaintain?.Code;

                masterInfo.DetailList.Add(detail);
            }

            paramList.Add(masterInfo);
            asnEvent.RemoteAsnEventList = paramList;
            RT.EventBus.Publish<ProductToAsnEvent>(asnEvent);
        }

        /// <summary>
        /// 更新WMS回传的ASN单号
        /// </summary>
        /// <param name="wmsAsn">wmsAsn</param>
        public virtual void UpdateFromWMSAsn(RemoteAsnNo wmsAsn)
        {
            var time = RF.Find<InStorageBill>().GetDbTime();
            foreach (var item in wmsAsn.AsnNoList)
            {
                var inStore = RF.GetById<InStorageBill>(item.RequireId);
                if (inStore != null)
                {
                    inStore.AsnNo = item.AsnNo;
                    RF.Save(inStore);
                }
                var outputProductInStores = Query<OutputProductDetail>().Where(m => m.RequireId == item.RequireId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                foreach (var outputProductInStore in outputProductInStores)
                {
                    if (outputProductInStore != null)
                    {
                        outputProductInStore.AsnNo = item.AsnNo;
                        outputProductInStore.InStorageState = OutputProducts.InStorageState.InStorage;
                        outputProductInStore.ReceiveState = OutputProducts.ReceiveState.Received;
                        outputProductInStore.ReceiveDate = time;
                        RF.Save(outputProductInStore);
                    }
                }

            }
        }

        /// <summary>
        /// 更新WMS回传的成品入库单条码入库状态
        /// </summary>
        /// <param name="mesSnInfos">WMS传的入库单及条码</param>
        public virtual void UpdateSNStorageFromWMS(List<UpdateMesSnInfo> mesSnInfos)
        {
            if (mesSnInfos.Count > 0)
            {
                var requrieIds = mesSnInfos.Select(m => m.RequireId).Distinct().ToList();
                var inStorageBarcodeDetails = Query<InStorageBarcodeDetail>().Join<InStorageBill>((x, y) => x.InStorageBillId == y.Id
                && requrieIds.Contains(y.Id)).ToList();
                var sns = mesSnInfos.Select(n => n.Sn).ToList();
                foreach (var inStoreDetail in inStorageBarcodeDetails)
                {
                    if (sns.Contains(inStoreDetail.Barcode))
                    {
                        inStoreDetail.State = InStorageState.InStorage;
                        RF.Save(inStoreDetail);
                    }
                }
            }
        }

        /// <summary>
        /// 执行入库,保存入库单
        /// </summary>
        /// <param name="storageWorkOrderId">入库工单Id</param>
        /// <param name="barcodeList">待入库条码列表</param>
        public virtual void ToStorageIn(double storageWorkOrderId, EntityList<ToStorageBarcode> barcodeList)
        {
            var storageWorkOrder = RF.GetById<StorageWorkOrder>(storageWorkOrderId);
            if (storageWorkOrder != null)
            {
                barcodeList.ForEach(p =>
                {
                    p.StorageWorkOrder = storageWorkOrder;
                    p.PersistenceStatus = PersistenceStatus.Unchanged;
                    if (p.IsStored)
                    {
                        throw new ValidationException("条码【{0}】已入库，请勿重复入库，请刷新界面".L10nFormat(p.Barcode));
                    }
                });
            }

            ToStorageIn(barcodeList);
        }

        /// <summary>
        /// 检查是否存在更高层级的包装 存在则不允许入库
        /// </summary>
        /// <param name="entityList"></param>
        /// <exception cref="ValidationException"></exception>
        private void CheckPackLevel(EntityList<ToStorageBarcode> entityList)
        {
            var barcodeList = entityList.Select(p => p.Barcode).ToList();

            if (!barcodeList.Any())
            {
                throw new ValidationException("请选择入库条码".L10N());
            }
            List<string> erroBarcode = new List<string>();
            var packingRelations = Query<BatchPackingRelation>().Where(p => barcodeList.Contains(p.PackageNo)).ToList();
            foreach (var packingRelation in packingRelations)
            {
                if (packingRelation != null && (packingRelation.TreePId.HasValue || !string.IsNullOrEmpty(packingRelation.ParentNo)))//packingRelation.RootId== packingRelation.Id时候说明自己是最高层级
                {
                    erroBarcode.Add(packingRelation.PackageNo);
                }
            }
            if (erroBarcode.Count > 0)
            {
                var strBarcode = string.Empty;
                for (int i = 0; i < erroBarcode.Count; i++)
                {
                    strBarcode += (i + 1) % 4 == 0 ? erroBarcode[i] + ",</br>" : erroBarcode[i] + ",";
                }
                throw new ValidationException("所选入库条码【{0}】存在上级包装层级，无法提交成品入库".L10nFormat(strBarcode.TrimEnd(',')));
            }
        }


        /// <summary>
        /// 执行入库,保存入库单
        /// </summary>
        /// <param name="entityList">列表数据</param>
        public virtual void ToStorageIn(EntityList<ToStorageBarcode> entityList)
        {
            CheckPackLevel(entityList);
            //判断是否存在上层包装 存在不能入库

            //需管控条码必须检验合格，才可产生入库单
            var sns = entityList.Select(p => p.Barcode).ToList();
            var inspLogs = RT.Service.Resolve<InspLogController>().GetBarcodeLogsBySnType(sns, InspType.Product);
            var inspBarcodes = RT.Service.Resolve<InspRecordController>().GetInspBarcodeBySn(sns, InspType.Product);
            foreach (var sn in sns)
            {
                var inspLog = inspLogs.OrderByDescending(p => p.CreateDate).FirstOrDefault(p => p.Barcode == sn);
                var inspBarcode = inspBarcodes.FirstOrDefault(p => p.Barcode == sn);
                if (inspBarcode != null && inspLog == null)
                {
                    throw new ValidationException("产品【{0}】未报检，不允许入库".L10nFormat(sn));
                }
                if (inspLog != null)
                {
                    if (inspLog.InspectionResult == null)
                    {
                        throw new ValidationException("产品【{0}】未检验，不允许入库".L10nFormat(sn));
                    }
                    if (inspLog.InspectionResult == Common.InspectionResult.Fail)
                    {
                        throw new ValidationException("产品【{0}】检验不合格，不允许入库".L10nFormat(sn));
                    }
                }
            }
            //生成入库单
            string storeBillNo = GetStoreNo();
            var storeWorkOrder = entityList[0].StorageWorkOrder;
            InStorageBill bill = new InStorageBill();
            bill.No = storeBillNo;
            bill.WarehouseId = GetWarehouseId();
            bill.ReceiveState = ReceiveState.Received;
            bill.ReceiveDate = RF.Find<InStorageBill>().GetDbTime();
            bill.StorageWorkOrderId = storeWorkOrder.Id;
            decimal storeqty = 0;
            foreach (var item in entityList)
            {
                ////入库单明细赋值
                InStorageBarcodeDetail billDetail = new InStorageBarcodeDetail();
                billDetail.Level = item.Level;
                billDetail.Qty = item.Qty;
                billDetail.Barcode = item.Barcode;
                billDetail.BatchBarcode = item.BatchBarcode;
                bill.InStorageBarcodeDetailList.Add(billDetail);
                ////累加入库条码完工数量到入库单
                storeqty += item.Qty;
            }

            bill.Qty = storeqty;
            entityList.ForEach(p => p.IsStored = true);

            using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                RF.Save(entityList);
                RF.Save(bill);
                StoreToWMS(entityList, storeBillNo, (DateTime)bill.ReceiveDate, bill.Id);
                tran.Complete();
            }

            //推送追溯数据同步消息
            SendDataTraceToStorageMsg(bill, entityList);
        }

        /// <summary>
        /// 推送追溯数据同步消息
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="entityList"></param>
        private void SendDataTraceToStorageMsg(InStorageBill bill, EntityList<ToStorageBarcode> entityList)
        {
            ProductInWarehouseMsg msg = new ProductInWarehouseMsg()
            {
                WorkOrderId = bill.StorageWorkOrderId,
                BillId = bill.Id,
                ToStorageBarcodeIds = entityList.Select(p => p.Id).ToList()
            };
            //推送追溯数据同步消息
            RT.Service.Resolve<DataSyncPushController>().SendDataSyncPushMsg<ProductInWarehouseMsg>(msg);
        }

        /// <summary>
        /// 半成品流转入库
        /// </summary>
        /// <param name="storageWorkOrderId"></param>
        /// <param name="barcodeList"></param>
        public virtual void SemiFinishToStorageIn(double storageWorkOrderId, EntityList<ToStorageBarcode> barcodeList)
        {
            var storageWorkOrder = RF.GetById<StorageWorkOrder>(storageWorkOrderId);
            if (storageWorkOrder != null)
                barcodeList.ForEach(p => { p.StorageWorkOrder = storageWorkOrder; });

            //生成入库单
            string storeBillNo = GetStoreNo();
            var storeWorkOrder = barcodeList[0].StorageWorkOrder;
            InStorageBill bill = new InStorageBill();
            bill.No = storeBillNo;
            bill.WarehouseId = GetWarehouseId();
            bill.ReceiveState = ReceiveState.Received;
            bill.ReceiveDate = RF.Find<InStorageBill>().GetDbTime();
            bill.StorageWorkOrderId = storeWorkOrder.Id;
            decimal storeqty = 0;
            foreach (var item in barcodeList)
            {
                ////入库单明细赋值
                InStorageBarcodeDetail billDetail = new InStorageBarcodeDetail();
                billDetail.Level = item.Level;
                billDetail.Qty = item.Qty;
                billDetail.Barcode = item.Barcode;
                billDetail.BatchBarcode = item.BatchBarcode;
                bill.InStorageBarcodeDetailList.Add(billDetail);
                ////累加入库条码完工数量到入库单
                storeqty += item.Qty;
                //var detailBarcodes = new EntityList<ToStorageBarcodeDetail>();
                //ToStorageBarcodeDetail label = new ToStorageBarcodeDetail()
                //{
                //    Barcode = item.Barcode,
                //    FinishQty = item.Qty,
                //    CollectDate = RF.Find<ToStorageBarcodeDetail>().GetDbTime()
                //};
                //detailBarcodes.Add(label);
                if (!item.ToStorageBarcodeDetailList.Any(m => m.Barcode == item.Barcode))//避免重复
                {
                    ToStorageBarcodeDetail label = new ToStorageBarcodeDetail()
                    {
                        Barcode = item.Barcode,
                        FinishQty = item.Qty,
                        CollectDate = RF.Find<ToStorageBarcodeDetail>().GetDbTime()
                    };
                    item.ToStorageBarcodeDetailList.Add(label);
                }
            }

            bill.Qty = storeqty;
            barcodeList.ForEach(p => p.IsStored = true);
            using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                RF.Save(barcodeList);
                RF.Save(bill);
                StoreToWMS(barcodeList, storeBillNo, (DateTime)bill.ReceiveDate, bill.Id);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取成品和半成品物料
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemIsProduct(string keyword = null, PagingInfo pagingInfo = null)
        {
            var query = Query<Item>().Where(p => (p.Type == Items.ItemType.Product || p.Type == Items.ItemType.SemiFinished)).Exists<ItemPackageRule>((x, y) => y.Where(e => e.ItemId == x.Id));
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取成品入库单号
        /// </summary>
        /// <returns>报检单号</returns>
        public virtual string GetStoreNo()
        {
            var config = ConfigService.GetConfig(new ProductStorageConfig(), typeof(StorageWorkOrder));
            if (config == null || !config.NumberRuleId.HasValue)
                throw new ValidationException("未找到成品入库单号配置规则，请配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取入库仓库Id
        /// </summary>
        /// <returns>仓库Id</returns>
        public virtual double GetWarehouseId()
        {
            var config = ConfigService.GetConfig(new ProductStorageConfig(), typeof(StorageWorkOrder));
            if (config == null || config.WarehouseId == 0)
                throw new ValidationException("未找到成品入库仓库配置，请配置".L10N());
            return config.WarehouseId;
        }

        #region 包装拆合批
        /// <summary>
        /// 判断包装条码或其明细是否已经入库
        /// </summary>
        /// <param name="barcode">包装条码</param>
        /// <param name="isBatch">是否批次</param>
        /// <returns>true存在，false不存在</returns>
        public virtual bool IsExistsPakStorageDetail(string barcode, bool isBatch)
        {
            SaveIsExistsPakStorageDetailLog(barcode, isBatch);
            var ctl = RT.Service.Resolve<PackageController>();
            var packRelation = ctl.GetPackingRelationByPackNo(barcode);
            EntityList<BatchPackingRelation> sonEntityList = new EntityList<BatchPackingRelation>();
            sonEntityList.Add(packRelation);
            sonEntityList = ctl.GetStoreRelationByUnitId(sonEntityList);
            //包装里面没有底层条码
            if (sonEntityList == null) return false;
            List<string> checkBarcodes;
            if (isBatch)
            {
                checkBarcodes = sonEntityList.Select(p => p.PackageNo).ToList();

            }
            else
            {
                var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelByRelationId(sonEntityList.Select(p => p.Id).ToList());
                checkBarcodes = labels.Select(p => p.Label).ToList();
            }

            return CheckIsExistsStorageDetailByBarcodes(sonEntityList[0].WorkOrderId, checkBarcodes, isBatch);
        }

        /// <summary>
        /// 保存生成成品检验单日志
        /// </summary>
        /// <param name="barcode">包装条码</param>
        /// <param name="isBatch">是否批次</param>
        private void SaveIsExistsPakStorageDetailLog(string barcode, bool isBatch)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "包装条码:{0};是否批次:{1}".L10nFormat(barcode, isBatch);
                var log = new InterfaceLog()
                {
                    Name = _iToStorageBarcodeString,
                    Method = "IsExistsPakStorageDetail",
                    ControllerName = _productStorageControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 检查条码集合是否已经入库
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="barcodes">条码集合</param>
        /// <param name="isBatch">是否批次</param>
        /// <returns>true存在，false不存在</returns>
        public virtual bool CheckIsExistsStorageDetailByBarcodes(double workOrderId, List<string> barcodes, bool isBatch)
        {
            return Query<ToStorageBarcodeDetail>().Where(p => p.ToStorageBarcode.StorageWorkOrderId == workOrderId && p.ToStorageBarcode.IsStored && (barcodes.Contains(p.Barcode) && !isBatch || barcodes.Contains(p.Batch) && isBatch)).FirstOrDefault() != null;
        }

        /// <summary>
        /// 检查单体条码是否已经入库
        /// </summary>
        /// <param name="barcode">条码</param>      
        /// <returns>true存在，false不存在</returns>
        public virtual bool IsExistsStorageDetailByBarcode(string barcode)
        {
            SaveIsExistsStorageDetailByBarcodeLog(barcode);
            return Query<ToStorageBarcodeDetail>().Where(p => barcode == p.Barcode && p.ToStorageBarcode.IsStored).FirstOrDefault() != null;
        }

        /// <summary>
        /// 保存检查单个产品条码是否已经入库日志
        /// </summary>
        /// <param name="barcode">单个产品条码</param>
        private void SaveIsExistsStorageDetailByBarcodeLog(string barcode)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "单个产品条码:{0}".L10nFormat(barcode);
                var log = new InterfaceLog()
                {
                    Name = _iToStorageBarcodeString,
                    Method = "IsExistsStorageDetailByBarcode",
                    ControllerName = _productStorageControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        ///  删除入库明细条码
        /// </summary>
        /// <param name="productBarcode">生产产品条码</param>
        public virtual void DeleteToStoreDetailByCode(string productBarcode)
        {
            SaveDeleteToStoreDetailByCodeLog(productBarcode);
            DB.Delete<ToStorageBarcodeDetail>().Where(p => p.Barcode == productBarcode).Execute();
        }

        /// <summary>
        /// 保存删除单体入库条码明细日志
        /// </summary>
        /// <param name="productBarcode">单体产品条码</param>
        private void SaveDeleteToStoreDetailByCodeLog(string productBarcode)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "单体产品条码:{0}".L10nFormat(productBarcode);
                var log = new InterfaceLog()
                {
                    Name = _iToStorageBarcodeString,
                    Method = "DeleteToStoreDetailByCode",
                    ControllerName = _productStorageControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 包装拆合移除包装时，入库对应条码和明细要移除
        /// </summary>
        /// <param name="batchPackRelationId">移除的包装条码包装关系</param>
        /// <param name="isBatch">是否批次</param>
        public virtual void MoveStorageDetailByPackcode(double batchPackRelationId, bool isBatch)
        {
            SaveMoveStorageDetailByPackcodeLog(batchPackRelationId, isBatch);
            var packRelation = RF.GetById<BatchPackingRelation>(batchPackRelationId);
            if (packRelation == null) return;
            var storeWo = RF.GetById<StorageWorkOrder>(packRelation.WorkOrderId);
            if (storeWo == null) return;
            var unit = storeWo.PackageRuleDetailList.Where(p => p.IsInStockLabel || p.PackageUnit.IsMasterUnit).OrderByDescending(p => p.IsInStockLabel).FirstOrDefault();
            if (unit.PackageUnit.IsMasterUnit || packRelation.PackageUnitId == unit.PackageUnitId)
            {
                //刚好是入库包装或主单位入库
                return;
            }
            else
            {
                var packIndex = storeWo.PackageRuleDetailList.Where(p => p.PackageUnitId == packRelation.PackageUnitId)?.Select(p => p.GetIndex()).FirstOrDefault();
                if (packIndex == null) throw new ValidationException("工单包装规则没有当前包装层级".L10N());
                var unitIndex = storeWo.PackageRuleDetailList.Where(p => p.PackageUnitId == unit.PackageUnitId).Select(p => p.GetIndex()).FirstOrDefault();
                if (packIndex > unitIndex)
                {
                    return;
                }
                else
                {
                    //刚好是入库明细的条码（批次）则删除不再跑下面的逻辑   
                    var delDtl = GetToStoreBarcodeDetailByCode(packRelation.WorkOrderId, packRelation.PackageNo, true);
                    if (delDtl != null)
                    {
                        DB.Delete<ToStorageBarcodeDetail>().Where(p => p.Batch == packRelation.PackageNo).Execute();
                        var toBarcode = delDtl.ToStorageBarcode;
                        toBarcode.Qty -= delDtl.FinishQty;
                        RF.Save(toBarcode);
                    }
                    else
                    {
                        //入库包装里面的中包装，对应里面明细入库全部拿掉                                           
                        var list = new EntityList<BatchPackingRelation>();
                        list.Add(packRelation);
                        list = RT.Service.Resolve<PackageController>().GetStoreRelationByUnitId(list);
                        //空箱移除
                        if (list == null) return;
                        List<string> deleteBarcodes;
                        if (isBatch)
                            deleteBarcodes = list.Select(p => p.PackageNo).ToList();
                        else
                        {
                            var relationIds = list.Select(p => p.Id).ToList();
                            var itemLabelList = RT.Service.Resolve<ItemLabelController>().GetItemLabelByRelationId(relationIds);
                            deleteBarcodes = itemLabelList.Select(p => p.Label).ToList();
                        }

                        var deleteDtls = GetToStoreBarcodeDetailByCodes(packRelation.WorkOrderId, deleteBarcodes, isBatch);
                        if (deleteDtls.Count == 0) return;
                        var toBarcode = deleteDtls[0].ToStorageBarcode;
                        toBarcode.Qty -= deleteDtls.Sum(p => p.FinishQty);
                        RF.Save(toBarcode);
                        var delIds = deleteDtls.Select(e => e.Id).ToList();
                        DB.Delete<ToStorageBarcodeDetail>().Where(p => delIds.Contains(p.Id)).Execute();
                    }

                }
            }
        }

        /// <summary>
        /// 保存移除入库数据日志
        /// </summary>
        /// <param name="batchPackRelationId">包装关系Id</param>
        /// <param name="isBatch">是否批次</param>
        private void SaveMoveStorageDetailByPackcodeLog(double batchPackRelationId, bool isBatch)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "包装关系Id:{0};是否批次:{1}".L10nFormat(batchPackRelationId, isBatch);
                var log = new InterfaceLog()
                {
                    Name = _iToStorageBarcodeString,
                    Method = "MoveStorageDetailByPackcode",
                    ControllerName = _productStorageControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 包装拆合加入包装时，入库对应条码和明细要加上
        /// </summary>
        /// <param name="batchPackRelationId">加入的包装条码包装关系（productBarcode不空时，relation是itemLabel对应的relation）</param>
        /// <param name="isBatch">是否批次</param>      
        /// <param name="ProductBarcode">产品条码</param>
        public virtual void JoinStorageDetailByPackcode(double batchPackRelationId, bool isBatch, string ProductBarcode)
        {
            SaveJoinStorageDetailByPackcodeLog(batchPackRelationId, isBatch, ProductBarcode);
            var packRelation = RF.GetById<BatchPackingRelation>(batchPackRelationId);
            if (packRelation == null) return;
            var storeWo = RF.GetById<StorageWorkOrder>(packRelation.WorkOrderId);
            if (storeWo == null) return;
            var unit = storeWo.PackageRuleDetailList.Where(p => p.IsInStockLabel || p.PackageUnit.IsMasterUnit).OrderByDescending(p => p.IsInStockLabel).FirstOrDefault();
            if (unit.PackageUnit.IsMasterUnit || packRelation.PackageUnitId == unit.PackageUnitId && string.IsNullOrEmpty(ProductBarcode))
            {
                //刚好是入库包装单位或主单位入库，如何是单个产品，packRelation是上级的包装，如果刚好是入库的层级，需要加入
                return;
            }
            else
            {
                //当前单位>入库单位，循环找下面的对应的入库单位，然后入库      
                var ctl = RT.Service.Resolve<PackageController>();
                var packIndex = storeWo.PackageRuleDetailList.Where(p => p.PackageUnitId == packRelation.PackageUnitId)?.Select(p => p.GetIndex()).FirstOrDefault();
                if (packIndex == null) throw new ValidationException("工单包装规则没有当前包装层级".L10N());
                var unitIndex = storeWo.PackageRuleDetailList.Where(p => p.PackageUnitId == unit.PackageUnitId).Select(p => p.GetIndex()).FirstOrDefault();
                if (packIndex > unitIndex)
                {
                    return;
                }
                else
                {
                    //当前单位<入库单位，找到对应的入库条码Id,把明细加进去（批次的不需要找itemlabel）
                    //1.先找到入库的上级包装，再把明细加入
                    var storeRelation = ctl.GetParentRelationByUnitId(packRelation, unit.PackageUnitId);
                    var tostorebarcode = GetStorageBarcode(packRelation.WorkOrderId, storeRelation?.PackageNo);
                    if (tostorebarcode == null) return;
                    if (!string.IsNullOrEmpty(ProductBarcode))
                    {
                        //当个产品进来直接加入
                        ToStorageBarcodeDetail detail = new ToStorageBarcodeDetail()
                        {
                            Barcode = ProductBarcode,
                            CollectDate = RF.Find<ToStorageBarcodeDetail>().GetDbTime(),
                            FinishQty = 1
                        };
                        tostorebarcode.ToStorageBarcodeDetailList.Add(detail);
                    }
                    else
                    {
                        var detailToStoreCode = GetProductBarcodesByPackingNo(storeRelation, unit.PackageUnitId, isBatch);
                        if (detailToStoreCode != null)
                        {
                            tostorebarcode.ToStorageBarcodeDetailList.Clear();
                            tostorebarcode.ToStorageBarcodeDetailList.AddRange(detailToStoreCode);
                        }
                    }

                    tostorebarcode.Qty = tostorebarcode.ToStorageBarcodeDetailList.Sum(p => p.FinishQty);
                    RF.Save(tostorebarcode);
                }
            }
        }

        /// <summary>
        /// 保存加入入库数据日志
        /// </summary>
        /// <param name="batchPackRelationId">包装关系Id</param>
        /// <param name="isBatch">是否批次</param>
        /// <param name="ProductBarcode">产品条码</param>
        private void SaveJoinStorageDetailByPackcodeLog(double batchPackRelationId, bool isBatch, string ProductBarcode)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "包装关系Id:{0};是否批次:{1};产品条码:{2}".L10nFormat(batchPackRelationId, isBatch, ProductBarcode);
                var log = new InterfaceLog()
                {
                    Name = _iToStorageBarcodeString,
                    Method = "JoinStorageDetailByPackcode",
                    ControllerName = _productStorageControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        #endregion

        /// <summary>
        /// 获取工单入库仓库
        /// </summary>
        /// <returns></returns>
        public virtual double GetWoWarehouse()
        {
            try { return GetWarehouseId(); }
            catch { return 0; }
        }

        /// <summary>
        /// 判断条码是否已成品入库
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool BarcodeIsDetail(string barcode, double? workOrderId)
        {
            if (workOrderId == null)
            {
                return false;
            }
            var barcodeDetail = Query<InStorageBarcodeDetail>().Join<InStorageBill>((x, y) => x.InStorageBillId == y.Id && y.StorageWorkOrderId == workOrderId.Value)
                .Where(p => p.Barcode == barcode).FirstOrDefault();
            return barcodeDetail != null;
        }
    }
}
