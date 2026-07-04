using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.ProductStorage;
using SIE.EventMessages.Receipt;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.ProductIntfc.Configs;
using SIE.ProductIntfc.InspRecords;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 成品入库查询实体数据
    /// </summary>
    public partial class OutputProductController : DomainController, IProductStorage//IToStorageBarcode
    {

        /// <summary>
        /// 成品入库查询实体数据
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns>实体列表</returns>
        public virtual EntityList<OutputProduct> GetProductStorages(OutputProductCriteria criteria)
        {
            var query = Query<OutputProduct>();
            if (criteria.PlanBeginTime.BeginValue.HasValue)
            {
                query.Where(k => k.PlanBeginDate >= criteria.PlanBeginTime.BeginValue);
            }
            if (criteria.PlanBeginTime.EndValue.HasValue)
            {
                query.Where(k => k.PlanBeginDate >= criteria.PlanBeginTime.EndValue);
            }
            if (criteria.Barcode.IsNotEmpty() || criteria.Lot.IsNotEmpty() || criteria.InstorageBarcode.IsNotEmpty())
            {
                query.Exists<OutputProductDetail>((x, y) => y.Where(e => e.StorageWorkOrderId == x.Id)
                 .WhereIf(criteria.Barcode.IsNotEmpty(), e => e.Barcode.Contains(criteria.Barcode))
                .WhereIf(criteria.Lot.IsNotEmpty(), e => e.Lot.Contains(criteria.Lot))
                .WhereIf(criteria.InstorageBarcode.IsNotEmpty(), e => e.NO.Contains(criteria.InstorageBarcode)));

            }

            if (criteria.FactoryId.HasValue)
                query.Where(p => p.FactoryId == criteria.FactoryId);
            if (criteria.ShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.ShopId);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (!criteria.WorkOrder.IsNullOrEmpty())
                query.Where(p => p.No.Contains(criteria.WorkOrder));
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);
            query.Exists<WorkOrderOutputProduct>((pto, es) => es.Where(p => pto.Id == p.WorkOrderId));
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取已入库信息
        /// </summary>
        /// <param name="id">入库工单ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>已入库信息列表</returns>
        public virtual EntityList<OutputProductDetail> GetInStoreBarcode(double id, PagingInfo pagingInfo = null, List<OrderInfo> orderInfos = null)
        {
            return Query<OutputProductDetail>().Where(p => p.StorageWorkOrderId == id).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 对接入库信息到WMS
        /// </summary>
        /// <param name="entityList"></param>
        public virtual void StoreToWMS(EntityList<OutputProductDetail> entityList)
        {
            //根据仓库ID进行分类
            var groupWarehouseId = entityList.GroupBy(m => m.WarehouseId).ToDictionary(p => p.Key, p => p.ToList());
            var storeWorkOrder = entityList.First().StorageWorkOrder;
            foreach (var item in groupWarehouseId)
            {
                var tempIdEntity = new OutputProductDetail();
                tempIdEntity.GenerateId();
                var requireId = tempIdEntity.Id;
                var billNo = GetStoreNo();
                ProductToAsnEvent asnEvent = new ProductToAsnEvent();
                List<RemoteAsnEvent> paramList = new List<RemoteAsnEvent>();
                var masterInfo = new RemoteAsnEvent();
                masterInfo.RequireId = requireId;
                masterInfo.RequireNo = billNo;
                masterInfo.OrderType = storeWorkOrder.ProductType == ItemType.Product ? 10 : 20;
                masterInfo.PriorityType = 0;
                masterInfo.DeliveryDate = item.Value.First().OperatorTime.Value;
                masterInfo.WarehouseId = item.Value.First().WarehouseId;
                masterInfo.EnterpriseId = storeWorkOrder.WorkShopId;
                masterInfo.DetailList = new List<RemoteAsnDTLEvent>();
                foreach (var toStorageBarcode in item.Value)
                {
                    //对接到WMS明细条码赋值
                    RemoteAsnDTLEvent detail = new RemoteAsnDTLEvent();
                    detail.WorkNo = storeWorkOrder.No;
                    detail.ItemId = toStorageBarcode.ItemId;
                    detail.ExpectQty = toStorageBarcode.InStorageQty;

                    //生产条码或批次条码
                    detail.BarCode = toStorageBarcode.Barcode.IsNullOrEmpty() ? toStorageBarcode.Lot : toStorageBarcode.Barcode;
                    //上级条码 生产条件或批次条件不等于入库条码时，传入库条码
                    //detail.PackageNo = toStorageBarcode.Barcode != detail.SN ? toStorageBarcode.Barcode : String.Empty;
                    detail.LotAtt01 = toStorageBarcode.OperatorTime;
                    detail.LotAtt04 = toStorageBarcode.Lot;

                    //成品入库信息到WMS，增加传递工单扩展属性
                    detail.ItemExtProp = toStorageBarcode.ItemExtProp;
                    detail.ItemExtPropName = toStorageBarcode.ItemExtPropName;

                    masterInfo.DetailList.Add(detail);

                    toStorageBarcode.RequireId = requireId;
                    toStorageBarcode.NO = billNo;
                }
                paramList.Add(masterInfo);
                asnEvent.RemoteAsnEventList = paramList;
                RF.Save(entityList);
                RT.EventBus.Publish<ProductToAsnEvent>(asnEvent);

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
            var config = ConfigService.GetConfig(new OutputProductsConfig(), typeof(OutputProduct));
            if (config == null || !config.NumberRuleId.HasValue)
                throw new ValidationException("未找到联/副产品入库单号配置规则，请配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取联产品入库仓库Id
        /// </summary>
        /// <returns>仓库Id</returns>
        public virtual double GetJointWarehouseId()
        {
            var config = ConfigService.GetConfig(new OutputProductsConfig(), typeof(OutputProduct));
            if (config == null || config.JointWarehouseId == 0)
                throw new ValidationException("未找到联产品入库仓库配置，请配置".L10N());
            return config.JointWarehouseId;
        }

        /// <summary>
        /// 获取副产品仓库
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual double GetByWarehouseId()
        {
            var config = ConfigService.GetConfig(new OutputProductsConfig(), typeof(OutputProduct));
            if (config == null || config.JointWarehouseId == 0)
                throw new ValidationException("未找到副产品入库仓库配置，请配置".L10N());
            return config.ByWarehouseId;
        }


        /// <summary>
        /// 获取工单产出物
        /// </summary>
        /// <param name="storageWorkOrderId"></param>
        /// <returns></returns>

        public virtual EntityList<WorkOrderOutputProduct> GetOutputProduceList(double storageWorkOrderId)
        {
            var result = Query<WorkOrderOutputProduct>().Where(m => m.WorkOrderId == storageWorkOrderId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (result.Any())
            {
                var itemIds = result.Select(m => m.ItemId).ToList();
                var itemBaseInfo = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds);
                result.ForEach(m =>
                {
                    var baseInfo = itemBaseInfo.FirstOrDefault(k => m.ItemId == k.ItemId);
                    if (baseInfo != null)
                    {
                        m.IsBatchCtrl = baseInfo.IsBatch;
                    }
                });
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postDatas"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void ToStorageIn(EntityList<OutputProductDetail> postDatas)
        {
            var itemIds = postDatas.Select(p => p.ItemId).Distinct().ToList();
            var ids = postDatas.Select(p => p.Id).Distinct().ToList();
            var outputProductDetails = RT.Service.Resolve<OutputProductController>().GetOutputProductDetails(ids);

            if (outputProductDetails.All(m => m.InStorageState == InStorageState.InStorage))
            {
                throw new ValidationException("入库失败，不存在待入库状态记录!请刷新数据".L10N());
            }

            var stocks = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds);
            var time = RF.Find<OutputProductDetail>().GetDbTime();
            foreach (var item in postDatas)
            {
                if (!item.WarehouseId.HasValue)
                {
                    throw new ValidationException("请选择收货仓库!".L10N());
                }
                var itemStock = stocks.FirstOrDefault(a => a.ItemId == item.ItemId);
                var isBitchManage = itemStock?.IsBatch == true;
                if (isBitchManage && item.Lot.IsNullOrEmpty())
                {
                    throw new ValidationException("批次管理的物料必须输入批次号!".L10N());
                }
                if (item.InStorageQty <= 0)
                {

                    throw new ValidationException("入库数量必填，必须大于0!".L10N());
                }
                item.OperatorId = RT.IdentityId;
                item.OperatorTime = time;
                item.ReceiveState = ReceiveState.Received;
                var isExsited = outputProductDetails.Any(a => a.Id == item.Id);
                item.PersistenceStatus = isExsited ? PersistenceStatus.Modified : PersistenceStatus.New;
            }
            using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                //RF.Save(postDatas);
                StoreToWMS(postDatas);
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据集合Id获取明细
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<OutputProductDetail> GetOutputProductDetails(List<double> ids)
        {
            return ids.SplitContains(item =>
            {
                return Query<OutputProductDetail>().Where(p => item.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 此处不实现
        /// </summary>
        /// <param name="wmsAsn"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void UpdateFromWMSAsn(RemoteAsnNo wmsAsn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据默认仓库
        /// </summary>
        /// <param name="outPutType"></param>
        /// <returns></returns>
        public virtual Warehouse GetBillWh(OutputListType outPutType)
        {
            double whId = 0;
            switch (outPutType)
            {

                case OutputListType.JointProducts:
                    whId = GetJointWarehouseId();
                    break;
                case OutputListType.ByProducts:
                    whId = GetByWarehouseId();
                    break;
                default:
                    break;
            }
            return whId <= 0 ? null : RF.GetById<Warehouse>(whId);
        }

        /// <summary>
        /// 查询副产品记录
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="sortInfo"></param>
        /// <returns></returns>
        public virtual EntityList<OutputProductRecord> GetOutputProductRecords(double workOrderId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var list = Query<OutputProductRecord>().Where(p => p.StorageWorkOrderId == workOrderId).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            if (list.Count == 0)
                return list;
            var wo = RF.GetById<WorkOrder>(workOrderId);
            list.ForEach(p =>
            {
                p.SubmitQty = wo.WorkOrderOutputProductList.FirstOrDefault(x => x.ItemId == p.ItemId)?.SubmitQty ?? 0;
            });
            return list;
        }
    }
}
