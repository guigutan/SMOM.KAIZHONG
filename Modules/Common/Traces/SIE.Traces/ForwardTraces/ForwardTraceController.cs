using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Traces;
using SIE.EventMessages.QMS.Traces;
using SIE.EventMessages.WMS.Traces;
using SIE.Traces.Common;

namespace SIE.Traces.ForwardTraces
{
    /// <summary>
    /// 正向追溯控制器
    /// </summary>
    public class ForwardTraceController : DomainController
    {
        /// <summary>
        /// 获取追溯的物料列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<ForwardTraceViewModel> GetTraceItems(ForwardTraceViewModelCriteria criteria, PagingInfo pagingInfo)
        {
            EntityList<ForwardTraceViewModel> result = new EntityList<ForwardTraceViewModel>();
            if (!criteria.ItemId.HasValue)
                throw new ValidationException("物料不能为空".L10N());

            var wmsItemCriteria = new WmsItemCriteria()
            {
                ItemId = criteria.ItemId.Value,
                ItemExtPropName = criteria.ItemExtPropName,
                ItemLot = criteria.Lot,
                PagingInfo = pagingInfo,
                SN = criteria.Sn,
                SupplierId = criteria.SupplierId,
                ProductDateStart = criteria.ProductionDate.BeginValue,
                ProductDateEnd = criteria.ProductionDate.EndValue,
                ReceiptDateStart = criteria.ReceiptDate.BeginValue,
                ReceiptDateEnd = criteria.ReceiptDate.EndValue,
            };
            var wmsItemInfo = RT.Service.Resolve<EventMessages.WMS.Traces.ITrace>().GetWmsItemInfo(wmsItemCriteria);
            if (wmsItemInfo.Data.IsNotEmpty())
                result = wmsItemInfo.Data.Select(c => new ForwardTraceViewModel()
                {
                    TraceType = c.TraceType,
                    ItemId = c.ItemId,
                    ItemCode = c.ItemCode,
                    ItemName = c.ItemName,
                    ItemLot = c.ItemLot,
                    Sn = c.SN,
                    ItemExtPropName = c.ItemExtPropName,
                }).AsEntityList();

            result.SetTotalCount(wmsItemInfo.TotalCount);

            return result;
        }

        /// <summary>
        /// 库存追溯
        /// </summary>
        /// <param name="forwardTraceViewModel"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WmsFwdTraceViewModel> GetWmsTraceDatas(ForwardTraceViewModel forwardTraceViewModel, PagingInfo pagingInfo)
        {
            EntityList<WmsFwdTraceViewModel> result = new EntityList<WmsFwdTraceViewModel>();


            var wmsItemOnhandCriteria = new WmsItemOnhandCriteria()
            {
                PagingInfo = pagingInfo,
                TraceType = forwardTraceViewModel.TraceType,
                ItemExtPropName = forwardTraceViewModel.ItemExtPropName,
                ItemId = forwardTraceViewModel.ItemId,
                ItemLot = forwardTraceViewModel.ItemLot,
                Sn = forwardTraceViewModel.Sn
            };
            var wmsTraceItemInfo = RT.Service.Resolve<EventMessages.WMS.Traces.ITrace>().GetWmsItemOnhandInfo(wmsItemOnhandCriteria);
            if (wmsTraceItemInfo.Data.IsNotEmpty())
                result = wmsTraceItemInfo.Data.Select(c => new WmsFwdTraceViewModel()
                {
                    TraceType = forwardTraceViewModel.TraceType,
                    Sn=forwardTraceViewModel.Sn,
                    ItemId=forwardTraceViewModel.ItemId,
                    ItemLot = forwardTraceViewModel.ItemLot,
                    ItemExtPropName=forwardTraceViewModel.ItemExtPropName,
                    LotLpnOnhandId=c.OnhandId,
                    WarehouseCode = c.WarehouseCode,
                    StorageLocationCode = c.StorageLocationCode,
                    OnhandState=c.OnhandState,
                    Qty = c.Qty,
                    AsnNo= c.AsnNo,
                    SupplierName= c.SupplierName,
                    ProductionDate = c.ProductionDate,
                    CollectDate= c.CollectDate
                }
                ).AsEntityList();

            result.SetTotalCount(wmsTraceItemInfo.TotalCount);
            return result;
        }

        /// <summary>
        /// 库存发运信息追溯
        /// </summary>
        /// <param name="wmsFwdTraceViewModel"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WmsShippingViewModel> GetWmsShippingTraceDatas(WmsFwdTraceViewModel wmsFwdTraceViewModel, PagingInfo pagingInfo)
        {
            EntityList<WmsShippingViewModel> result = new EntityList<WmsShippingViewModel>();

            var shipmentTraceInfoCriteria = new ShipmentTraceInfoCriteria()
            {
                PagingInfo = pagingInfo,
                TraceType = wmsFwdTraceViewModel.TraceType,
                LotLpnOnhandId = wmsFwdTraceViewModel.LotLpnOnhandId,
                ItemExtPropName = wmsFwdTraceViewModel.ItemExtPropName,
                ItemId = wmsFwdTraceViewModel.ItemId,
                ItemLot = wmsFwdTraceViewModel.ItemLot,
                Sn = wmsFwdTraceViewModel.Sn
            };
            var wmsTraceItemInfo = RT.Service.Resolve<EventMessages.WMS.Traces.ITrace>().GetWmsShippingTraceInfo(shipmentTraceInfoCriteria);
            if (wmsTraceItemInfo.Data.IsNotEmpty())
                result = wmsTraceItemInfo.Data.Select(c => new WmsShippingViewModel()
                {
                    ShipQty = c.ShippingQty,
                    ShippingOrderNo = c.ShippingOrderNo,
                    ReceiveByName = c.ReceiveByName,
                    ReceiveTime = c.ReceiveTime,
                    WorkOrderNo = c.WorkOrderNo,
                    WorkShopName = c.WorkShopName,
                    ResourceName = c.ResourceName,
                    FactoryName = c.FactoryName
                }
                ).AsEntityList();

            result.SetTotalCount(wmsTraceItemInfo.TotalCount);
            return result;
        }


        /// <summary>
        /// 来料品质追溯
        /// </summary>
        /// <param name="forwardTraceViewModel"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<QmsTraceViewModel> GetQmsIqcTraceDatas(ForwardTraceViewModel forwardTraceViewModel, PagingInfo pagingInfo)
        {
            EntityList<QmsTraceViewModel> result = new EntityList<QmsTraceViewModel>();
            AsnDetailIdsCriteria asnDetailIdsCriteria = new AsnDetailIdsCriteria()
            {
                TraceType = forwardTraceViewModel.TraceType,
                Sn = forwardTraceViewModel.Sn,
                ItemId = forwardTraceViewModel.ItemId,
                ItemLot = forwardTraceViewModel.ItemLot,
                ItemExtPropName = forwardTraceViewModel.ItemExtPropName
            };
            //QMS的IQC报检是基于asnDetailId
            var asnDetailIds = RT.Service.Resolve<EventMessages.WMS.Traces.ITrace>().GetAsnDetailIds(asnDetailIdsCriteria);
            if (asnDetailIds.IsNullOrEmpty())
                return result;

            var criteria = new TraceInfoForIqcCriteria()
            {
                PagingInfo = pagingInfo,
                AsnDetailIds = asnDetailIds,
            };
            var traceInfoForIqc = RT.Service.Resolve<EventMessages.QMS.Traces.ITrace>().GetTraceInfoForIqc(criteria);
            if (traceInfoForIqc.Data.IsNotEmpty())
                result = traceInfoForIqc.Data.Select(c => new QmsTraceViewModel()
                {
                    InspectionType = c.InspectionType,
                    InspectionNo = c.InspectionNo,
                    InspectionResult = c.InspectionResult,
                    FailedAuditResult = c.FailedAuditResult,
                    DefectRecord = c.DefectRecord,
                    FailedAuditWorkflowCode = c.FailedAuditWorkflowCode,
                    QualityWorkflowCode = c.QualityWorkflowCode,
                    InspectionBy = c.InspectionBy,
                    InspectionTime = c.InspectionTime
                }
                ).AsEntityList();

            result.SetTotalCount(traceInfoForIqc.TotalCount);
            return result;
        }

        /// <summary>
        /// 过程追溯-关联产品追溯
        /// </summary>
        /// <param name="forwardTraceViewModel"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<MesFwdTraceViewModel> GetTraceInfoForProduct(ForwardTraceViewModel forwardTraceViewModel, PagingInfo pagingInfo)
        {
            EntityList<MesFwdTraceViewModel> result = new EntityList<MesFwdTraceViewModel>();


            var criteria = new TraceInfoForProductCriteria()
            {
                PagingInfo = pagingInfo,
                TraceType = forwardTraceViewModel.TraceType,
                ItemId = forwardTraceViewModel.ItemId,
                ItemLot = forwardTraceViewModel.ItemLot,
                ItemExtPropName = forwardTraceViewModel.ItemExtPropName,
                Sn = forwardTraceViewModel.Sn
            };
            var traceInfoForIqc = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetTraceInfoForProduct(criteria);
            if (traceInfoForIqc.Data.IsNotEmpty())
                result = traceInfoForIqc.Data.Select(c => new MesFwdTraceViewModel()
                {
                    Id = c.Id.ToString(),
                    TreePId = c.TreePId,
                    WipProductVersionId = c.WipProductVersionId,
                    RelatedProductId = c.RelatedProductId,
                    RelatedProductSn = c.RelatedProductSn,
                    RelatedProductLot = c.RelatedProductLot,
                    RelatedProductCode = c.RelatedProductCode,
                    RelatedProductName = c.RelatedProductName,
                    RelatedWorkOrderNo = c.RelatedWorkOrderNo,
                    RelatedWorkOrderId = c.RelatedWorkOrderId,
                    ProductExtProp = c.ProductExtProp,
                    ItemExtPropName = c.ItemExtPropName,
                    ItemId = c.ItemId,
                    ItemSourceCode = c.ItemSourceCode
                }
                ).AsEntityList();

            result.SetTotalCount(traceInfoForIqc.TotalCount);
            return result;
        }

        /// <summary>
        /// 过程追溯-采集记录追溯
        /// </summary>
        /// <param name="mesFwdTraceViewModel"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<MesProcessKeyItemFwdViewModel> GetMesProcssKeyItemData(MesFwdTraceViewModel mesFwdTraceViewModel, PagingInfo pagingInfo)
        {
            EntityList<MesProcessKeyItemFwdViewModel> result = new EntityList<MesProcessKeyItemFwdViewModel>();

            var criteria = new TraceInfoForProcssKeyItemCriteria()
            {
                PagingInfo = pagingInfo,
                WipProductVersionId = mesFwdTraceViewModel.WipProductVersionId,
                ProductId = mesFwdTraceViewModel.RelatedProductId,
                ProductSn = mesFwdTraceViewModel.RelatedProductSn,
                ItemId = mesFwdTraceViewModel.ItemId,
                ProductExtProp = mesFwdTraceViewModel.ProductExtProp,
                ItemExtPropName = mesFwdTraceViewModel.ItemExtPropName,
                ItemSourceCode = mesFwdTraceViewModel.ItemSourceCode
            };
            var traceInfo = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetTraceInfoForProcssKeyItem(criteria);
            if (traceInfo.Data.IsNotEmpty())
                result = traceInfo.Data.Select(c => new MesProcessKeyItemFwdViewModel()
                {
                    CollectSn=c.CollectSn,
                    StationName = c.StationName,
                    ProcessName = c.ProcessName,
                    Qty = c.Qty,
                    CollectBy = c.CollectBy,
                    CollectTime = c.CollectTime
                }
                ).AsEntityList();

            result.SetTotalCount(traceInfo.TotalCount);
            return result;
        }

        
        

        
    }
}
