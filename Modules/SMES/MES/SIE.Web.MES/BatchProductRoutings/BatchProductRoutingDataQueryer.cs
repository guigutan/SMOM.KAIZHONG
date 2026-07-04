using SIE.Barcodes.WipBatchs;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Runtime;
using SIE.Tech.Processs;
using SIE.Web.Data;
using System;
using System.Linq;

namespace SIE.Web.MES.BatchProductRoutings
{
    /// <summary>
    /// 批次产品工艺路线数据查询类
    /// </summary>
    public class BatchProductRoutingDataQueryer : DataQueryer
    {
        /// <summary>
        /// 自定义查询的获取批次条码列表的方法
        /// </summary>
        /// <param name="criteria">自定义查询体</param>
        /// <returns>批次条码列表</returns>
        public EntityList<SubWipBatch> GetBatchInfoList(BatchCriteria criteria)
        {
            return RT.Service.Resolve<BatchManageController>().GetBatches(criteria);
        }

        /// <summary>
        /// 获取批次产品版本
        /// </summary>
        /// <param name="batch">批次实体</param>
        /// <returns>批次产品版本</returns>
        public BatchWipProductVersion GetBatchWipProductVersion(WipBatchExt batch)
        {
            var version = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductVersion(GetWipBatchNo(batch));
            return version;
        }

        /// <summary>
        /// 获取批次号
        /// </summary>
        /// <param name="batch">批次实体</param>
        /// <returns>批次号</returns>
        internal string GetWipBatchNo(SubWipBatch batch)
        {
            if (batch == null)
                return string.Empty;
            return batch.BatchNo;
        }

        /// <summary>
        /// 加载生产产品信息 包括工艺路线事件列表 运行时产品信息 批次关系 工艺路线布局XML
        /// </summary>
        /// <param name="batch">批次实体</param>
        /// <returns>生产产品信息</returns>
        public WipBatchProductInfo LoadWipProductData(WipBatchExt batch)
        {
            var info = new WipBatchProductInfo();
            var version = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductVersion(GetWipBatchNo(batch));
            info.WipProductVersion = version;
            BatchRelation batchRelation = new BatchRelation();
            if (version != null)
            {
              
                batchRelation = RT.Service.Resolve<BatchManageController>().GetBatchRelation(batch?.BatchNo, BarcodeType.BatchBarocde);
                info.Product = RT.Service.Resolve<RuntimeController>().FindProduct(batchRelation.Bid, BarcodeType.BatchBarocde);
                info.BatchRelation = batchRelation;

                var eventList = RT.Service.Resolve<BatchWipProductRoutingController>().GetWipProductRoutingEvents(batchRelation.Id);
                info.RoutingEventList.AddRange(eventList);
            }

            GetLayout(info, batchRelation, batch);
            return info;
        }

        /// <summary>
        /// 工序选择变更时获取 生产工序信息 产品缺陷 维修记录 生产关健件
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="productStatus">产品状态，0未上线、1已完工、2在制</param>
        /// <returns>生产工序信息</returns>
        public WipBatchProcessInfo LoadWipProcessData(double batchId, double versionId, double processId, int productStatus, product product)
        {
            var info = new WipBatchProcessInfo();
            var version = RF.GetById<BatchWipProductVersion>(versionId, new EagerLoadOptions().LoadWithViewProperty());
            if (version == null)
            {
                throw new ValidationException("未找到生产产品版本".L10N());
            }
            info.DefectList.AddRange(version.DefectList.Where(p => p.ProcessId == processId));
            info.RepaireList.AddRange(version.RepaireList.Where(p => p.ProcessId == processId));
            var process = version.ProcessList.FirstOrDefault(p => p.ProcessId == processId);

            if (process != null)
            {
                var batch = RF.GetById<SubWipBatch>(batchId);
                if (batch == null)
                {
                    throw new EntityNotFoundException(typeof(SubWipBatch), batchId);
                }
                string batchNo = batch.IsChild ? batch.WipBatch?.BatchNo : batch.BatchNo;
                string subBatchNo = batch.BatchNo;
                EntityList<BatchWipProductProcessDetail> details = new EntityList<BatchWipProductProcessDetail>();
                if (batch.IsChild)
                {
                    details.AddRange(process.DetailList.Where(p => p.PlugType == PlugType.Out && p.BatchNo == batchNo && p.SubBatchNo == subBatchNo));
                }
                else
                {
                    details.AddRange(process.DetailList.Where(p => p.PlugType == PlugType.Out && p.BatchNo == batchNo));
                }
                var detailList = details.SelectMany(p => p.KeyItemList).Distinct((x, y) => x.ItemId == y.ItemId);
                if (detailList.Any())
                {
                    info.KeyItemList.AddRange(detailList);
                }
            }

            if (productStatus == 0)  //未上线，取工单工序bom
            {
                var boms = version.WorkOrder.ProcessBomList.Where(p => p.ProcessId == processId);
                boms.ForEach(p => info.BomList.Add(InitBomViewModes(p.Item, p.SingleQty,p.ItemExtProp,p.ItemExtPropName)));
            }
            if (productStatus == 1)   //已完工，取关键件
            {
                info.KeyItemList.ForEach(p => info.BomList.Add(InitBomViewModes(p.Item, p.SingleQty,p.ItemExtProp,p.ItemExtPropName)));

            }
            if (productStatus == 2)   //在制，取运行时产品bom
            {
                var rtProcess = product?.Routing?.Processes?.FirstOrDefault(p => p.ProcessId == processId);
                rtProcess?.Boms?.ForEach(p => info.BomList.Add(InitBomViewModes(RF.GetById<Item>(p.ItemId), p.Qty,p.ItemExtProp,p.ItemExtPropName)));
            }

            return info;
        }

        /// <summary>
        /// 初始bom模型
        /// </summary>
        /// <param name="item">物料</param>
        /// <param name="qty">用量</param>
        /// <param name="itemExtProp">扩展属性</param>
        /// <param name="itemExtPropName">扩展属性</param>
        /// <returns>bom模型</returns>
        ProductBomViewModel InitBomViewModes(Item item, decimal qty,string itemExtProp,string itemExtPropName)
        {
            return new ProductBomViewModel()
            {
                Item = item,
                Code = item.Code,
                Name = item.Name,
                Qty = qty,
                ItemExtProp = itemExtProp,
                ItemExtPropName = itemExtPropName,
                IsBuckleMaterial = true,
                Id = Guid.NewGuid().ToString()
            };
        }

        /// <summary>
        /// 获取工艺路线布局,如果产品工艺路线布局不为空则取产品工艺路线，否则取工单对应的工艺路线
        /// </summary>
        /// <param name="info">产品信息</param>
        /// <param name="batchRelation">批次关系</param>
        /// <param name="batch">批次实体</param>
        private void GetLayout(WipBatchProductInfo info, BatchRelation batchRelation, WipBatch batch)
        {
            bool isWorkOrderLayout = true;
            string layout = string.Empty;
            var batchProductRouting = RT.Service.Resolve<BatchWipProductRoutingController>().GetWipProductRouting(batchRelation.Id);
            if (batchProductRouting != null && batchProductRouting.Layout != null)
            {
                isWorkOrderLayout = false;
                layout = batchProductRouting.Layout.Layout;
            }
            else
            {
                //拆分生成的子批工艺路线信息取获取运行时的产品工艺路线 
                if (batch == null || batch.WorkOrderId <= 0)
                    throw new ValidationException("未找到产品条码对应工单信息".L10N());
                var batchlayout = RT.Service.Resolve<BatchWipProductRoutingController>().GetBatchRuntimeRoutingLayout(batchRelation.Bid, batch.WorkOrderId);
                if (layout != null)
                {
                    layout = batchlayout.Layout;
                }
            }

            info.IsWorkOrderLayout = isWorkOrderLayout;
            info.Layout = layout;
        }


        /// <summary>
        /// 保存产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本Id</param>
        /// <param name="relationId">批次关系</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        public void SaveBatchProductRouting(double versionId, double relationId, string oldLayout, string newLayout)
        {
            RT.Service.Resolve<BatchWipProductRoutingController>().SaveProductRouting(versionId, relationId, oldLayout, newLayout);
        }



        /// <summary>
        /// 启用产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本Id</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        public void EnableBatchProductRouting(double versionId, string oldLayout, string newLayout)
        {
            RT.Service.Resolve<BatchWipProductRoutingController>().EnableProductRouting(versionId, oldLayout, newLayout);
        }

        /// <summary>
        /// 暂停批次产品工艺路线
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="oldLayout">旧的工艺路线</param>
        /// <param name="newLayout">新的工艺路线</param>
        public void PauseBatchProductRouting(string batchNo, string oldLayout, string newLayout)
        {
            var batchRelation = RT.Service.Resolve<BatchManageController>().GetBatchRelation(batchNo, BarcodeType.BatchBarocde);
            RT.Service.Resolve<BatchWipProductRoutingController>().PauseProductRouting(batchRelation.Id, oldLayout, newLayout);
        }

        /// <summary>
        /// 生产产品信息
        /// </summary>
        public class WipBatchProductInfo
        {
            /// <summary>
            /// 生产产品版本
            /// </summary>
            public BatchWipProductVersion WipProductVersion { get; set; }

            /// <summary>
            /// 是否工单工艺路线布局
            /// </summary>
            public bool IsWorkOrderLayout { get; set; }

            /// <summary>
            /// 工艺路线布局
            /// </summary>
            public string Layout { get; set; }

            /// <summary>
            /// 运行时产品信息
            /// </summary>
            public product Product { get; set; }

            /// <summary>
            /// 产品工艺路线事件列表
            /// </summary>
            public EntityList<BatchWipProductRoutingEvent> RoutingEventList { get; } = new EntityList<BatchWipProductRoutingEvent>();

            /// <summary>
            /// 批次关联关系
            /// </summary>
            public BatchRelation BatchRelation { get; set; }
        }

        /// <summary>
        /// 生产工序信息
        /// </summary>
        public class WipBatchProcessInfo
        {
            /// <summary>
            /// 产品生产关键件
            /// </summary>
            public EntityList<BatchWipProductProcessKeyItem> KeyItemList { get; } = new EntityList<BatchWipProductProcessKeyItem>();

            /// <summary>
            /// 产品缺陷记录
            /// </summary>
            public EntityList<BatchWipProductDefect> DefectList { get; } = new EntityList<BatchWipProductDefect>();

            /// <summary>
            /// 产品维修记录
            /// </summary>
            public EntityList<BatchWipProductRepaire> RepaireList { get; } = new EntityList<BatchWipProductRepaire>();

            /// <summary>
            /// 产品BOM
            /// </summary>
            public EntityList<ProductBomViewModel> BomList { get; } = new EntityList<ProductBomViewModel>();
        }
    }
}