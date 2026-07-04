using SIE.Api;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Release;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.ProductIntfc.OutputProducts.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using static IronPython.Modules._ast;

namespace SIE.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 联/副产品入库 控制器
    /// </summary>
    public partial class OutputProductController
    {
        #region 副产品

        /// <summary>
        /// 查询副产品任务单数量
        /// </summary>
        /// <returns></returns>
        public virtual int GetOutputProductInfosCount()
        {
            var count = Query<WorkOrder>()
                    .Where(p => p.State != Core.WorkOrders.WorkOrderState.Close)
                    .Exists<WorkOrderOutputProduct>((x, y) => y.Where(p => p.WorkOrderId == x.Id))
                    //.Exists<WorkOrderOutputProduct>((x, y) => y.Where(p => p.WorkOrderId == x.Id && p.Qty > p.SubmitQty))
                    .Count();
            return count;
        }

        /// <summary>
        /// 查询副产品任务单列表信息
        /// </summary>
        /// <param name="keyword">工序标签/工单/产品编码/产品名称</param>
        /// <returns></returns>
        [ApiService("查询副产品任务单列表信息")]
        public virtual List<OutputProductInfo> GetOutputProductInfos([ApiParameter("关键词(工序标签/工单/产品编码/产品名称)")] string keyword)
        {
            EntityList<WorkOrder> woList;
            woList = Query<WorkOrder>()
                .Where(p => p.State != Core.WorkOrders.WorkOrderState.Close && (p.No.Contains(keyword) || p.Product.Code.Contains(keyword) || p.Product.Name.Contains(keyword)))
                .Exists<WorkOrderOutputProduct>((pto, es) => es.Where(p => pto.Id == p.WorkOrderId))
                //.Exists<WorkOrderOutputProduct>((x, y) => y.Where(p => p.WorkOrderId == x.Id && p.Qty > p.SubmitQty))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (woList.Count == 0)
            {
                //通过工序标签查询工单数据
                var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(keyword);
                if (wipBatch == null)
                    throw new ValidationException("未查询到相关工单数据".L10nFormat(keyword));
                var wo = RF.GetById<WorkOrder>(wipBatch.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (wo.WorkOrderOutputProductList.Count > 0)
                    woList.Add(wo);
            }

            if (woList.Count == 0)
                throw new ValidationException("未查询到相关副产品工单数据".L10nFormat(keyword));
            var woIds = woList.Select(p => p.Id).ToList();
            //var records = Query<OutputProductRecord>().Where(p => woIds.Contains(p.StorageWorkOrderId)).ToList();
            var list = woList.Select(wo => new OutputProductInfo()
            {
                WorkOrderId = wo.Id,
                WorkOrderNo = wo.No,
                ProductId = wo.ProductId,
                ProductCode = wo.ProductCode,
                ProductName = wo.ProductName,
                PlanQty = wo.PlanQty,
                DetailInfos = wo.WorkOrderOutputProductList/*.Where(p => p.Qty > p.SubmitQty)*/.Select(p => new OutputProductDetailInfo()
                {
                    WorkOrderId = wo.Id,
                    WorkOrderNo = wo.No,
                    ItemId = p.ItemId,
                    ItemCode = p.ItemCode,
                    ItemName = p.ItemName,
                    ItemUnitName = p.ItemUnitName,
                    Qty = p.Qty,
                    SubmitQty = p.SubmitQty
                }).ToList(),
            }).ToList();

            return list;
        }


        /// <summary>
        /// 提交副产品数据
        /// </summary>
        /// <param name="productInfo">副产品数据</param>
        [ApiService("提交副产品数据")]
        public virtual void SubmitOutputProductInfo([ApiParameter("副产品数据")] OutputProductInfo productInfo)
        {
            if (productInfo == null || productInfo.DetailInfos.Count == 0)
                throw new ValidationException("提交数据为空");
            //if (productInfo.DetailInfos.Any(p => p.InputQty > p.Qty - p.SubmitQty))
            //    throw new ValidationException("副产品提交数量不能超过剩余需求量");

            var wo = RF.GetById<OutputProduct>(productInfo.WorkOrderId);
            if (wo == null)
                throw new ValidationException("提交的工单数据不存在");
            if (wo.State == Core.WorkOrders.WorkOrderState.Close)
                throw new ValidationException("工单[{0}]已关闭,请确认".L10nFormat(productInfo.WorkOrderNo));
            foreach (var p in productInfo.DetailInfos)
            {
                if (p.InputQty == 0)
                    continue;
                wo.OutputProductRecords.Add(new OutputProductRecord()
                {
                    OutPutType = OutputListType.ByProducts,
                    ItemId = p.ItemId,
                    Qty = p.InputQty
                });
            }
            //更新累计提交数量
            foreach (var p in wo.WorkOrderOutputProductList)
            {
                p.SubmitQty = wo.OutputProductRecords.Where(x => x.ItemId == p.ItemId).Sum(x => x.Qty);
                //if (p.SubmitQty > p.Qty)
                //    throw new ValidationException("物料[{0}]累计提交数量已超需求数量".L10nFormat(p.Item?.Code));
            }
            using (var trans = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                RF.Save(wo.OutputProductRecords);
                RF.Save(wo.WorkOrderOutputProductList);

                trans.Complete();
            }

        }
        #endregion

        /// <summary>
        /// 获取副产品收货数据
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual EntityList<OutputProductRecordViewModel> GetOutputProductRecordViewModels(List<double> woIds)
        {
            var list = Query<WorkOrderOutputProduct>().Where(p => woIds.Contains(p.WorkOrderId) && p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Close).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var vms = list.Select(p =>
            {
                var vm = new OutputProductRecordViewModel()
                {
                    WorkOrderId = p.WorkOrderId,
                    ItemId = p.ItemId,
                    Qty = p.Qty,
                    SubmitQty = p.SubmitQty,
                    TotalQty = p.SubmitQty
                };
                vm.LoadProperty(OutputProductRecordViewModel.WorkOrderNoProperty, p.WorkOrderNo);
                vm.LoadProperty(OutputProductRecordViewModel.ProductCodeProperty, p.ProductCode);
                vm.LoadProperty(OutputProductRecordViewModel.ProductNameProperty, p.ProductName);
                vm.LoadProperty(OutputProductRecordViewModel.ItemCodeProperty, p.ItemCode);
                vm.LoadProperty(OutputProductRecordViewModel.ItemNameProperty, p.ItemName);
                return vm;
            });

            return vms.OrderBy(p => p.WorkOrderId).ThenBy(p => p.ItemCode).AsEntityList();
        }

        /// <summary>
        /// 提交副产品收货数据
        /// </summary>
        /// <param name="viewModels"></param>
        public virtual void SubmitOutputProductDatas(List<OutputProductRecordViewModel> viewModels)
        {
            if (viewModels == null || viewModels.All(p => p.InputQty == 0))
                throw new ValidationException("提交数据为空");
            using (var trans = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                viewModels
                .Where(p => p.InputQty > 0)
                .GroupBy(p => new { p.WorkOrderId, p.WorkOrderNo })
                .ForEach(p =>
                {
                    var data = new OutputProductInfo()
                    {
                        WorkOrderId = p.Key.WorkOrderId,
                        WorkOrderNo = p.Key.WorkOrderNo,
                        DetailInfos = p.Select(x =>
                            new OutputProductDetailInfo
                            {
                                ItemId = x.ItemId,
                                InputQty = x.InputQty
                            }).ToList()

                    };
                    SubmitOutputProductInfo(data);
                });

                trans.Complete();
            }
        }


    }
}
