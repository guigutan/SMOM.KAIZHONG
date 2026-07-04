using SIE.Common;
using SIE.Domain.Validation;
using SIE.EventMessages.Inspection;
using SIE.MES.WIP;
using SIE.MES.WIP.Runtime;
using System;

namespace SIE.MES.Events
{
    /// <summary>
    /// MES报检控制器
    /// </summary>
    /// <seealso cref="SIE.DomainController" />
    public partial class InspController : DomainController
    {
        /// <summary>
        /// 创建MES条码采集成功后报检记录
        /// </summary>
        /// <param name="wipCollectedEvent">在制品采集后信息</param>
        public virtual void CreateInspRecords(WipCollectedEvent wipCollectedEvent)
        {
            //检验结果为NG。则不报检
            if (wipCollectedEvent.Data.CollectData.Result == ResultType.Fail)
                return;
            var product = wipCollectedEvent.Data.Product;
            if (product != null && product.Routing.Current != null && product.Routing.Current.InInning)
                return;
            var inspEvent = CreateProductInspEvent(wipCollectedEvent);
            RT.Service.Resolve<IProductInsp>().ProductInsp(inspEvent);
        }

        /// <summary>
        /// 创建成品报检参数
        /// </summary>
        /// <param name="wipCollectedEvent">在制品事件</param>
        /// <returns>成品报检参数</returns>
        ProductInspEvent CreateProductInspEvent(WipCollectedEvent wipCollectedEvent)
        {
            var data = wipCollectedEvent.Data;
            var workcell = data.Workcell;
            var product = wipCollectedEvent.Data.Product;

            if (!product.WorkOrder.WorkShopId.HasValue)
            {
                throw new ValidationException("创建成品报检参数失败，工单【{0}】的车间为空".L10nFormat(product.WorkOrder.No));
            }

            return new ProductInspEvent()
            {
                WorkOrderId = product.WorkOrderId,
                ItemId = product.ItemId,
                ProcessId = workcell.ProcessId,
                ResourceId = workcell.ResourceId,
                StationId = workcell.StationId,
                ShopId = product.WorkOrder.WorkShopId.Value,
                EmployeeId = workcell.EmployeeId,
                Barcode = wipCollectedEvent.Data.Barcodes[0].Code,
                CustomerId = product.WorkOrder.CustomerId,
                CollectionDate = data.CollectDate,
                IsEndProcess = product.Routing.Current.IsEnd,
                IsStartProcess = product.Routing.Current.IsStart,
                Context = wipCollectedEvent.Data.CollectData.Context,
                OkQty = product.Qty,
            };
        }

        /// <summary>
        /// 创建采集后的首检记录
        /// </summary>
        /// <param name="wipCollectedEvent">在制品采集后事件</param>
        public virtual void CreateFirstInsps(WipCollectedEvent wipCollectedEvent)
        {
            var product = wipCollectedEvent.Data.Product;
            if (product != null && product.Routing.Current != null && product.Routing.Current.InInning)
                return;
            //检验结果为NG。则不报检         
            var inspEvent = CreateFirstInspEvent(wipCollectedEvent);
            RT.Service.Resolve<IFirstInsp>().GenerateFirstInsp(inspEvent);
        }

        /// <summary>
        /// 创建首检参数
        /// </summary>
        /// <param name="wipCollectedEvent">在制品事件</param>
        /// <returns>首检参数</returns>
        FirstInspEvent CreateFirstInspEvent(WipCollectedEvent wipCollectedEvent)
        {
            var data = wipCollectedEvent.Data;
            var workcell = data.Workcell;
            var product = wipCollectedEvent.Data.Product;
            return new FirstInspEvent()
            {
                WorkOrderId = product.WorkOrderId,
                ItemId = product.ItemId,
                ProcessId = workcell.ProcessId,
                ResourceId = workcell.ResourceId,
                StationId = workcell.StationId,
                ShopId = product.WorkOrder.WorkShopId.HasValue? product.WorkOrder.WorkShopId.Value:0,
                EmployeeId = workcell.EmployeeId,
                Barcode = wipCollectedEvent.Data.Barcodes[0].Code,
                CustomerId = product.WorkOrder.CustomerId,
                CollectionDate = data.CollectDate,
                IsEndProcess = product.Routing.Current.IsEnd,
                IsStartProcess = product.Routing.Current.IsStart
            };
        }

        /// <summary>
        /// 创建采集后条码下线的成品入库条码记录
        /// </summary>
        /// <param name="wipFinishedEvent"></param>
        public virtual void CreateStorageBarcode(WipFinishedEvent wipFinishedEvent)
        {
            if (wipFinishedEvent == null)
            {
                return;
            }

            var tpye = wipFinishedEvent.Product.Routing.Current.Type;

            ////最后一个工序是批次包装不触发，批次包装在包装条码生成后推送（批次包装主单位也会打包装）       
            if (tpye == Tech.Processs.ProcessType.BatchPacking)
            {
                return;
            }

            if (wipFinishedEvent.Product.Routing.Current != null && wipFinishedEvent.Product.Routing.Current.InInning)
            {
                return;
            }

            var inspEvent = CreateToStorageBarcode(wipFinishedEvent);
            RT.Service.Resolve<IToStorageBarcode>().ToStorageBarcode(inspEvent);
        }

        /// <summary>
        /// 创建成品入库
        /// </summary>
        /// <param name="wipFinishedEvent">条码下线事件</param>
        /// <returns>成品入库参数</returns>
        ToStorageBarcodeEvent CreateToStorageBarcode(WipFinishedEvent wipFinishedEvent)
        {
            if (wipFinishedEvent.OutputBatch != null &&  //此处应永远为true
                !(wipFinishedEvent.OutputBatch.BatchNo.IsNullOrWhiteSpace() 
                    && wipFinishedEvent.OutputBatch.SubBatchNo.IsNullOrWhiteSpace()))// 当两个批次号都为空时不是批次采集
            {
                return new ToStorageBarcodeEvent()
                {
                    WorkOrderId = wipFinishedEvent.Product.WorkOrderId,
                    Barcode = string.IsNullOrEmpty(wipFinishedEvent.OutputBatch.SubBatchNo) ? wipFinishedEvent.OutputBatch.BatchNo : wipFinishedEvent.OutputBatch.SubBatchNo,
                    Qty = wipFinishedEvent.OutputBatch.Qty,
                    CollectionDate = wipFinishedEvent.CollectionDate,
                    BatchBarcode = wipFinishedEvent.OutputBatch.BatchNo
                };
            }
            else
            {
                return new ToStorageBarcodeEvent()
                {
                    WorkOrderId = wipFinishedEvent.Product.WorkOrderId,
                    Barcode = wipFinishedEvent.Barcode,
                    Qty = wipFinishedEvent.Product.Qty, //1,
                    CollectionDate = wipFinishedEvent.CollectionDate
                };
            }
        }
    }
}