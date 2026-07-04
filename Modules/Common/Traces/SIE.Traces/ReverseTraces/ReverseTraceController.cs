using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Traces;
using SIE.EventMessages.QMS.Traces;
using SIE.Traces.Common;

namespace SIE.Traces.ReverseTraces
{
    /// <summary>
    /// 反向追溯控制器
    /// </summary>
    public class ReverseTraceController : DomainController
    {
        /// <summary>
        /// 获取追溯的产品列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<ReverseTraceViewModel> GetTraceProducts(ReverseTraceViewModelCriteria criteria, PagingInfo pagingInfo)
        {
            EntityList<ReverseTraceViewModel> result = new EntityList<ReverseTraceViewModel>();
            if (!criteria.ProductId.HasValue)
                throw new ValidationException("产品不能为空".L10N());

            var mesProductInfoCriteria = new MesProductInfoCriteria()
            {
                ProductId = criteria.ProductId.Value,
                ProductSn = criteria.ProductSn,
                WorkOrderNo = criteria.WorkOrderNo,
                PagingInfo = pagingInfo,
                ProductionDateStart = criteria.ProductionDate.BeginValue,
                ProductionDateEnd = criteria.ProductionDate.EndValue,
            };
            var mesProductInfo = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetMesProductInfo(mesProductInfoCriteria);
            if (mesProductInfo.Data.IsNotEmpty())
                result = mesProductInfo.Data.Select(c => new ReverseTraceViewModel()
                {
                    VersionId = c.VersionId,
                    ProductId = c.ProductId,
                    ProductCode = c.ProductCode,
                    ProductName = c.ProductName,
                    ProductionLot = c.ProductionLot,
                    ProductExtPropName = c.ProductExtPropName,
                    ProductSn = c.ProductSn,
                    Qty = c.Qty,
                    VersionName = c.VersionName,
                    ProductionDate = c.ProductionDate,
                    WorkOrderId = c.WorkOrderId,
                    WorkOrderNo = c.WorkOrderNo,
                    WorkShopName = c.WorkShopName,
                }).AsEntityList();

            result.SetTotalCount(mesProductInfo.TotalCount);

            return result;
        }

        /// <summary>
        /// 获取工序采集记录
        /// </summary>
        /// <param name="reverseTraceViewModel"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<MesProcessCollectViewModel> GetMesProcessCollectDatas(ReverseTraceViewModel reverseTraceViewModel, PagingInfo pagingInfo)
        {
            EntityList<MesProcessCollectViewModel> result = new EntityList<MesProcessCollectViewModel>();

            var criteria = new MesProcessCollectInfoCriteria()
            {
                ProductVersionId = reverseTraceViewModel.VersionId,
                PagingInfo = pagingInfo
            };
            var mesProductInfo = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetMesProcessCollectInfo(criteria);
            if (mesProductInfo.Data.IsNotEmpty())
                result = mesProductInfo.Data.Select(c => new MesProcessCollectViewModel()
                {
                    ReportProcessId = c.ReportProcessId,
                    CollectSn=c.CollectSn,
                    StateName = c.StateName,
                    StationName = c.StationName,
                    ProcessName = c.ProcessName,
                    ResourceName = c.ResourceName,
                    Result = c.Result,
                    CollectTime = c.CollectTime,
                    CollectBy = c.CollectBy
                }).AsEntityList();

            result.SetTotalCount(mesProductInfo.TotalCount);

            return result;
        }

        /// <summary>
        /// 获取关键件记录
        /// </summary>
        /// <param name="mesProcessCollectViewModel"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<MesProcessCollectKeyItemViewModel> GetMesProcessCollectKeyItemDatas(MesProcessCollectViewModel mesProcessCollectViewModel, PagingInfo pagingInfo)
        {
            EntityList<MesProcessCollectKeyItemViewModel> result = new EntityList<MesProcessCollectKeyItemViewModel>();

            var criteria = new MesProcessCollectKeyItemInfoCriteria()
            {
                ReportProcessId = mesProcessCollectViewModel.ReportProcessId,
                PagingInfo = null//不分页
            };
            var mesProductInfo = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetMesProcessCollectKeyItemInfo(criteria);
            if (mesProductInfo.Data.IsNotEmpty())
                result = mesProductInfo.Data.Select(c => new MesProcessCollectKeyItemViewModel()
                {
                    ItemId = c.ItemId,
                    ItemCode = c.ItemCode,
                    ItemName = c.ItemName,
                    SourceCode = c.SourceCode,
                    Qty = c.Qty,
                    ItemExtPropName = c.ItemExtPropName
                }).AsEntityList();

            result.SetTotalCount(mesProductInfo.TotalCount);

            return result;
        }


        /// <summary>
        /// 获取关键件的入库明细
        /// </summary>
        /// <param name="mesProcessCollectKeyItemViewModel"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<KeyItemWmsViewModel> GetKeyItemDatas(MesProcessCollectKeyItemViewModel mesProcessCollectKeyItemViewModel, PagingInfo pagingInfo)
        {
            EntityList<KeyItemWmsViewModel> result = new EntityList<KeyItemWmsViewModel>();

            var criteria = new EventMessages.WMS.Traces.AsnInfoCriteria()
            {
                ItemExtPropName = mesProcessCollectKeyItemViewModel.ItemExtPropName,
                ItemId = mesProcessCollectKeyItemViewModel.ItemId,
                SourceCode = mesProcessCollectKeyItemViewModel.SourceCode,
                PagingInfo = null//不分页
            };
            List<double> asnDetailIds = new List<double>();
            var asnInfo = RT.Service.Resolve<EventMessages.WMS.Traces.ITrace>().GetAsnInfo(criteria);
            if (asnInfo.Data.IsNotEmpty())
            {
                asnDetailIds = asnInfo.Data.Select(c => c.AsnDetailId).ToList();
                result = asnInfo.Data.Select(c => new KeyItemWmsViewModel()
                {
                    AsnNo = c.AsnNo,
                    AsnDetailId = c.AsnDetailId,
                    SupplierName = c.SupplierName,
                    ProductionLot = c.ProductionLot,
                    ItemLot = c.ItemLot,
                    ProductionDate = c.ProductionDate,
                    CollectDate = c.CollectDate
                }).AsEntityList();
            }
            if (asnDetailIds.IsNotEmpty())
            {
                var iqcCriteria = new TraceInfoForIqcCriteria()
                {
                    PagingInfo = null,
                    AsnDetailIds = asnDetailIds,
                };
                var iqcInfo = RT.Service.Resolve<EventMessages.QMS.Traces.ITrace>().GetTraceInfoForIqc(iqcCriteria);
                if (iqcInfo.Data.IsNotEmpty())
                    foreach (var iqcData in iqcInfo.Data)
                    {
                        var single = result.Where(c => c.AsnDetailId == iqcData.AsnDetailId).FirstOrDefault();
                        if (single != null)
                        {
                            single.InspectionNo = iqcData.InspectionNo;
                            single.InspectionResult = iqcData.InspectionResult;
                            single.FailedAuditResult = iqcData.FailedAuditResult;
                            single.DefectRecord = iqcData.DefectRecord;
                            single.FailedAuditWorkflowCode = iqcData.FailedAuditWorkflowCode;
                            single.QualityWorkflowCode = iqcData.QualityWorkflowCode;
                            single.InspectionBy = iqcData.InspectionBy;
                            single.InspectionTime = iqcData.InspectionTime;
                        }

                    }

            }

            return result;
        }
    }
}
