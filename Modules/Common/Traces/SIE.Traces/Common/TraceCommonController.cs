using SIE.Common;
using SIE.Domain;
using SIE.EventMessages.MES.Traces;
using SIE.EventMessages.QMS.Traces;
using SIE.EventMessages.WMS.Traces;
using SIE.Traces.ForwardTraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Traces.Common
{
    /// <summary>
    /// 追溯控制器
    /// </summary>
    public class TraceCommonController : DomainController
    {
        /// <summary>
        /// 包装信息追溯
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productSn"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<PackageTraceViewModel> GetPackageData(double productId, string productSn, PagingInfo pagingInfo)
        {
            EntityList<PackageTraceViewModel> result = new EntityList<PackageTraceViewModel>();

            var criteria = new PackageInfoCriteria()
            {
                ProductId = productId,
                ProductSn = productSn
            };

            var packageInfoList = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetPackageInfos(criteria);
            if (packageInfoList.IsNotEmpty())
            {
                result = packageInfoList.Select(c => new PackageTraceViewModel()
                {
                    Id = c.Id,
                    TreePId = c.TreePId,
                    PackageNo = c.PackageNo,
                    PackageUnitName = c.PackageUnitName,
                    Qty = c.Qty,
                    PackageTime = c.PackageTime,
                    WarehouseName = "*",
                    StationName = "*",
                    ShippingOrderNo = "*",
                    CustomerName = "*",
                    DeliveryDate = string.Empty,
                    StorageBarcode = c.StorageBarcode
                }).AsEntityList();

                var packageInfo = result.First(c => c.PackageNo == productSn);

                var packageWmsInfoCriteria = new PackageWmsInfoCriteria() { StorageBarcode = packageInfo.StorageBarcode, ProductId = productId, ProductSn = productSn };
                var packageWmsInfo = RT.Service.Resolve<EventMessages.WMS.Traces.ITrace>().GetPackageWmsInfo(packageWmsInfoCriteria);
                if (packageWmsInfo != null)
                {
                    packageInfo.WarehouseName = packageWmsInfo.WarehouseName;
                    packageInfo.StationName = packageWmsInfo.StationName;
                    packageInfo.ShippingOrderNo = packageWmsInfo.ShippingOrderNo;
                    packageInfo.CustomerName = packageWmsInfo.CustomerName;
                    packageInfo.DeliveryDate = packageWmsInfo.DeliveryDate;
                }


            }
            return result;
        }


        /// <summary>
        /// 产品品质追溯
        /// </summary>
        /// <param name="productSn"></param>
        /// <param name="productId"></param>
        /// <param name="workOrderId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<QmsTraceViewModel> GetMesProductQmsData(string productSn, double productId, double workOrderId, PagingInfo pagingInfo)
        {
            EntityList<QmsTraceViewModel> result = new EntityList<QmsTraceViewModel>();

            var criteria = new TraceInfoForProductQmsCriteria()
            {
                ProductId = productId,
                WorkOrderId = workOrderId
            };

            //获取Mes报检信息
            var mesInspList = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetProductInspectInfo(new ProductInspectInfoCriteria() { ProductSn = productSn, WorkOrderId = workOrderId });
            if (mesInspList.IsNotEmpty())
            {
                foreach (var mesInsp in mesInspList)
                {
                    if (mesInsp.InspType==0)
                        criteria.ShippingInspLogId = mesInsp.InspLogId;
                    else if (mesInsp.InspType ==1)
                        criteria.FirstInspLogId = mesInsp.InspLogId;
                }
            }

            //获取出货单号
            var shipmentInfo = RT.Service.Resolve<EventMessages.WMS.Traces.ITrace>().GetShipmentInfo(new ShipmentInfoCriteria { ProductSn = productSn });
            if (shipmentInfo.ShippingOrderNo.IsNotEmpty())
                criteria.ShipmentNo = shipmentInfo.ShippingOrderNo;

            var traceInfo = RT.Service.Resolve<EventMessages.QMS.Traces.ITrace>().GetTraceInfoForQms(criteria);
            if (traceInfo.Data.IsNotEmpty())
                result = traceInfo.Data.Select(c => new QmsTraceViewModel()
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

            result.SetTotalCount(traceInfo.TotalCount);
            return result;
        }

        /// <summary>
        /// 产品检验记录追溯
        /// </summary>
        /// <param name="wipProductVersionId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<ProductInspectTraceViewModel> GetMesProductInspectData(double wipProductVersionId, PagingInfo pagingInfo)
        {
            EntityList<ProductInspectTraceViewModel> result = new EntityList<ProductInspectTraceViewModel>();

            var criteria = new TraceInfoForProductInspectCriteria()
            {
                PagingInfo = pagingInfo,
                WipProductVersionId = wipProductVersionId
            };
            var traceInfo = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetTraceInfoForProductInspect(criteria);
            if (traceInfo.Data.IsNotEmpty())
                result = traceInfo.Data.Select(c => new ProductInspectTraceViewModel()
                {
                    InspectionValue = c.InspectionValue,
                    LimitLow = c.LimitLow,
                    LimitMax = c.LimitMax,
                    Result = c.Result,
                    Name = c.Name,
                    Remarks = c.Remarks
                }
                ).AsEntityList();

            result.SetTotalCount(traceInfo.TotalCount);
            return result;
        }

        /// <summary>
        /// 产品缺陷记录追溯
        /// </summary>
        /// <param name="wipProductVersionId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<ProductDefectTraceViewModel> GetMesProductDefectData(double wipProductVersionId, PagingInfo pagingInfo)
        {
            EntityList<ProductDefectTraceViewModel> result = new EntityList<ProductDefectTraceViewModel>();

            var criteria = new TraceInfoForProductDefectCriteria()
            {
                PagingInfo = pagingInfo,
                WipProductVersionId = wipProductVersionId,
            };
            var traceInfo = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetTraceInfoForProductDefect(criteria);
            if (traceInfo.Data.IsNotEmpty())
                result = traceInfo.Data.Select(c => new ProductDefectTraceViewModel()
                {
                    BoardNo = c.BoardNo,
                    DefectCode = c.DefectCode,
                    DefectDescription = c.DefectDescription,
                    FixedDate = c.FixedDate,
                    InspItemName = c.InspItemName,
                    IsMisjudgment = c.IsMisjudgment,
                    Location = c.Location,
                    Process = c.Process,
                    Remark = c.Remark,
                    Sn = c.Sn,
                    FixedBy = c.FixedBy
                }
                ).AsEntityList();

            result.SetTotalCount(traceInfo.TotalCount);
            return result;
        }

        /// <summary>
        /// 产品维修记录追溯
        /// </summary>
        /// <param name="wipProductVersionId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<ProductRepairTraceViewModel> GetMesProductRepairData(double wipProductVersionId, PagingInfo pagingInfo)
        {
            EntityList<ProductRepairTraceViewModel> result = new EntityList<ProductRepairTraceViewModel>();

            var criteria = new TraceInfoForProductRepairCriteria()
            {
                PagingInfo = pagingInfo,
                WipProductVersionId = wipProductVersionId,
            };
            var traceInfo = RT.Service.Resolve<EventMessages.MES.Traces.ITrace>().GetTraceInfoForProductRepair(criteria);
            if (traceInfo.Data.IsNotEmpty())
                result = traceInfo.Data.Select(c => new ProductRepairTraceViewModel()
                {
                    DefectDes = c.DefectDes,
                    DefectRemark = c.DefectRemark,
                    RepairBy = c.RepairBy,
                    RepairProcess = c.RepairProcess,
                    RepairStation = c.RepairStation,
                    RepairTime = c.RepairTime,
                    RepairType = c.RepairType,
                }
                ).AsEntityList();

            result.SetTotalCount(traceInfo.TotalCount);
            return result;
        }

    }
}
