using Irony;
using Microsoft.AspNetCore.Http;
using SIE.Domain;
using SIE.EventMessages.Common.Traces;
using SIE.EventMessages.MES.Traces;
using SIE.Items;
using SIE.MES.Report.WipProducts;
using SIE.MES.Traces.Models;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.ProductStorages;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;

namespace SIE.MES.Traces
{
    /// <summary>
    /// MES追溯管理控制器
    /// </summary>
    public partial class TraceController : DomainController, ITrace
    {
        /// <summary>
        /// 过程追溯-关联产品追溯
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public virtual TraceInfoForProduct GetTraceInfoForProduct(TraceInfoForProductCriteria criteria)
        {
            var result = new TraceInfoForProduct();
            //关联产品追溯是一个树形数据，需要递归查询
            //为了匹配不同数据库，不使用语句递归的方式，采用程序递归的方式(牺牲服务器内存)

            string sourceCode = string.Empty;
            if (criteria.TraceType == TraceType.SerialNumber)
                sourceCode = criteria.Sn;
            else if (criteria.TraceType == TraceType.Batch)
                sourceCode = criteria.ItemLot;

            List<RecursiveProductInfo> treeDatas = new List<RecursiveProductInfo>();

            var query = Query<WipProductReportProcessKeyItem>();
            query.Join<WipProductReportProcess>((keyItem, wipProcess) => keyItem.ProcessId == wipProcess.Id && keyItem.ItemId == criteria.ItemId && keyItem.Qty > 0);
            query.Join<WipProductReportProcess, WipProductVersionReport>((wipProcess, versionReport) => wipProcess.VersionId == versionReport.Id);
            query.Join<WipProductVersionReport, WorkOrder>((versionReport, workOrder) => versionReport.WorkOrderId == workOrder.Id);
            query.WhereIf(criteria.ItemExtPropName.IsNotEmpty(), keyItem => keyItem.ItemExtPropName == criteria.ItemExtPropName);
            if (criteria.TraceType == TraceType.None)//非序列号非批次用ItemId追溯
                query.Where(keyItem => keyItem.ItemId == criteria.ItemId);
            else
                query.Where(keyItem => keyItem.SourceCode == sourceCode);

            var firstLevelDatas = query.Select<WipProductReportProcess, WipProductVersionReport, WorkOrder>((keyItem, wipProcess, versionReport, workOrder) => new
            {
                Id = versionReport.Id,
                SnId = versionReport.Id,
                ProductId = workOrder.ProductId,
                ProductSn = versionReport.Sn,
                ProductExtPropName = workOrder.ItemExtPropName,
                WorkOrderId = versionReport.WorkOrderId,
                CreateDate = versionReport.CreateDate,
                ItemId = keyItem.ItemId,
                ItemExtPropName = keyItem.ItemExtPropName,
                ItemSourceCode = keyItem.SourceCode
            }).ToList<RecursiveProductInfo>().ToList();

            if (firstLevelDatas.IsNullOrEmpty())
                return result;

            treeDatas.AddRange(firstLevelDatas);

            Recursion(treeDatas, firstLevelDatas);

            var newTreeData = new TreeProcessor().FlattenAndReverseTrees(treeDatas);
            result.Data = TrunToTraceInfo(newTreeData);

            return result;
        }

        private void Recursion(List<RecursiveProductInfo> treeDatas, List<RecursiveProductInfo> lastLevelDatas)
        {
            if (lastLevelDatas.IsNullOrEmpty())
            {
                lastLevelDatas.Clear();
                lastLevelDatas = null;
                return;
            }


            List<double> productIds = lastLevelDatas.Select(c => c.ProductId).ToList();
            System.DateTime earliestTime = lastLevelDatas.Select(c => c.CreateDate).OrderBy(c => c).First();//根据时间缩小一点范围

            List<RecursiveProductInfo> newDatas = new List<RecursiveProductInfo>();

            while (true)
            {
                if (productIds.Count() <= 1000)
                {
                    newDatas.AddRange(QueryData(productIds, earliestTime));
                    break;
                }
                var firstCodes = productIds.Take(1000);
                newDatas.AddRange(QueryData(firstCodes, earliestTime));
                productIds = productIds.Skip(1000).ToList();
            }

            //匹配数据，建立树形数据
            var newLevelTreeDatas = new List<RecursiveProductInfo>();
            foreach (var lastLevelData in lastLevelDatas)
            {
                var tempDatas = newDatas.FindAll(c => c.ItemId == lastLevelData.ProductId && c.ItemExtPropName == lastLevelData.ProductExtPropName && c.ItemSourceCode == lastLevelData.ProductSn);
                if (tempDatas.IsNotEmpty())
                {
                    tempDatas.ForEach(c => c.TreeId = lastLevelData.Id);
                    newLevelTreeDatas.AddRange(tempDatas);
                }

            }
            treeDatas.AddRange(newLevelTreeDatas);

            lastLevelDatas.Clear();//清掉内存

            Recursion(treeDatas, newLevelTreeDatas);
        }

        private List<RecursiveProductInfo> QueryData(IEnumerable<double> productIds, System.DateTime earliestTime)
        {

            var query = Query<WipProductReportProcessKeyItem>();
            query.Join<WipProductReportProcess>((keyItem, wipProcess) => keyItem.ProcessId == wipProcess.Id);
            query.Join<WipProductReportProcess, WipProductVersionReport>((wipProcess, versionReport) => wipProcess.VersionId == versionReport.Id);
            query.Join<WipProductVersionReport, WorkOrder>((versionReport, workOrder) => versionReport.WorkOrderId == workOrder.Id);
            query.Where(keyItem => productIds.Contains(keyItem.ItemId) && keyItem.Qty > 0 && keyItem.CreateDate >= earliestTime);
            var newDatas = query.Select<WipProductReportProcess, WipProductVersionReport, WorkOrder>((keyItem, wipProcess, versionReport, workOrder) => new
            {
                Id = versionReport.Id,
                SnId = versionReport.Id,
                ProductId = workOrder.ProductId,
                ProductSn = versionReport.Sn,
                ProductExtPropName = workOrder.ItemExtPropName,
                WorkOrderId = versionReport.WorkOrderId,
                CreateDate = versionReport.CreateDate,
                ItemExtPropName = keyItem.ItemExtPropName,
                ItemId = keyItem.ItemId,
                ItemSourceCode = keyItem.SourceCode,
            }).ToList<RecursiveProductInfo>().ToList();

            return newDatas;
        }


        private List<TraceItemInfoForProduct> TrunToTraceInfo(List<RecursiveProductInfo> recursiveProductInfos)
        {
            List<TraceItemInfoForProduct> result = new List<TraceItemInfoForProduct>();
            var productIds = recursiveProductInfos.Select(c => c.ProductId).ToList();
            var workOrderIds = recursiveProductInfos.Select(c => c.WorkOrderId).ToList();
            var productList = productIds.SplitContains(tempIds =>
            {
                return Query<Item>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            }).ToList();
            var workOrderList = workOrderIds.SplitContains(tempIds =>
            {
                return Query<WorkOrder>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            }).ToList();

            foreach (var recursiveProduct in recursiveProductInfos)
            {
                var product = productList.FirstOrDefault(c => c.Id == recursiveProduct.ProductId);
                var workOrder = workOrderList.FirstOrDefault(c => c.Id == recursiveProduct.WorkOrderId);
                var traceInfo = new TraceItemInfoForProduct()
                {
                    Id = recursiveProduct.Id,
                    TreePId = recursiveProduct.TreeId,
                    WipProductVersionId = recursiveProduct.SnId,
                    RelatedProductSn = recursiveProduct.ProductSn,
                    RelatedProductLot = string.Empty,
                    RelatedProductId = recursiveProduct.ProductId,
                    RelatedProductCode = product?.Code,
                    RelatedProductName = product?.Name,
                    ProductExtProp = workOrder?.ItemExtPropName,
                    RelatedWorkOrderNo = workOrder?.No,
                    ItemId = recursiveProduct.ItemId,
                    ItemExtPropName = recursiveProduct.ItemExtPropName,
                    ItemSourceCode = recursiveProduct.ItemSourceCode,
                    RelatedWorkOrderId = recursiveProduct.WorkOrderId
                };
                result.Add(traceInfo);
            }
            return result;
        }

        /// <summary>
        /// 过程追溯-采集记录追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual TraceInfoForProcssKeyItem GetTraceInfoForProcssKeyItem(TraceInfoForProcssKeyItemCriteria criteria)
        {
            var result = new TraceInfoForProcssKeyItem();

            var query = Query<WipProductReportProcessKeyItem>();
            query.Join<WipProductReportProcess>((keyItem, process) => keyItem.ProcessId == process.Id);
            query.Join<WipProductReportProcess, WipProductVersionReport>((process, report) => process.VersionId == report.Id && report.Id == criteria.WipProductVersionId);
            query.Where(keyItem => keyItem.ItemId == criteria.ItemId && keyItem.ItemExtPropName == criteria.ItemExtPropName && keyItem.SourceCode == criteria.ItemSourceCode);

            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var dataList = list.Select(c => new TraceItemInfoForProcssKeyItem
            {
                CollectSn = c.Barcode,
                StationName = c.StationName,
                ProcessName = c.ProcessName,
                CollectBy = c.UpdateByName,
                CollectTime = c.UpdateDate,
                Qty = c.Qty

            }).ToList();
            result.TotalCount = list.Count;
            result.Data = dataList;

            return result;
        }


        /// <summary>
        /// 过程追溯-产品检验记录追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual TraceInfoForProductInspect GetTraceInfoForProductInspect(TraceInfoForProductInspectCriteria criteria)
        {
            var result = new TraceInfoForProductInspect();

            var query = Query<WipProductReportInspectionItem>().Where(c => c.VersionId == criteria.WipProductVersionId);
            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var dataList = list.Select(c => new TraceItemInfoForProductInspect
            {
                InspectionValue = c.InspectionValue,
                LimitLow = c.LimitLow,
                LimitMax = c.LimitMax,
                Result = c.Result.ToLabel(),
                Name = c.Name,
                Remarks = c.Remarks

            }).ToList();
            result.TotalCount = list.Count;
            result.Data = dataList;

            return result;
        }


        /// <summary>
        /// 过程追溯-产品缺陷记录追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual TraceInfoForProductDefect GetTraceInfoForProductDefect(TraceInfoForProductDefectCriteria criteria)
        {
            var result = new TraceInfoForProductDefect();

            var query = Query<WipProductReportDefect>().Where(c => c.VersionId == criteria.WipProductVersionId);
            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var dataList = list.Select(c => new TraceItemInfoForProductDefect
            {
                BoardNo = c.BoardNo,
                DefectCode = c.DefectCode,
                DefectDescription = c.DefectDescription,
                FixedDate = c.FixedDate,
                InspItemName = c.InspItemName,
                IsMisjudgment = c.IsMisjudgment,
                Location = c.Location,
                Process = c.ProcessName,
                Remark = c.Remark,
                Sn = c.Sn,
                FixedBy = c.EmployeeName

            }).ToList();
            result.TotalCount = list.Count;
            result.Data = dataList;

            return result;
        }


        /// <summary>
        /// 过程追溯-产品维修记录追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual TraceInfoForProductRepair GetTraceInfoForProductRepair(TraceInfoForProductRepairCriteria criteria)
        {
            var result = new TraceInfoForProductRepair();
            var query = Query<WipProductReportRepair>().Where(c => c.VersionId == criteria.WipProductVersionId);
            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var dataList = list.Select(c => new TraceItemInfoForProductRepair
            {
                WipProductReportRepairId = c.Id,
                DefectDes = string.Empty,
                DefectRemark = string.Empty,
                RepairBy = c.CreateByName,
                RepairProcess = c.ProcessName,
                RepairStation = c.StationName,
                RepairTime = c.RepaireTime,
                RepairType = c.RepairType.ToLabel(),

            }).ToList();
            if (dataList.IsNotEmpty())
            {
                List<double> repairIds = dataList.Select(c => c.WipProductReportRepairId).Distinct().ToList();
                var defectQuery = Query<WipProductReportRepairDefect>().Where(c => repairIds.Contains(c.WipProductRepairId));
                var defectList = defectQuery.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                foreach (var item in dataList)
                {
                    var group = defectList.Where(c => c.WipProductRepairId == item.WipProductReportRepairId);
                    if (group.Any(c => c.DefectDesc.IsNotEmpty()))
                        item.DefectDes = string.Join(',', group.Select(c => c.DefectDesc).ToList());
                    if (group.Any(c => c.DefectRemark.IsNotEmpty()))
                        item.DefectRemark = string.Join(',', group.Select(c => c.DefectRemark).ToList());
                }
            }


            result.TotalCount = list.Count;
            result.Data = dataList;

            return result;
        }


        /// <summary>
        /// 获取产品报检信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual List<ProductInspectInfo> GetProductInspectInfo(ProductInspectInfoCriteria criteria)
        {
            var inspBarCodeList = Query<InspBarcodeLog>().Join<InspLog>((b, log) => b.InspLogId == log.Id && log.WorkOrderId == criteria.WorkOrderId).Where(c => c.Barcode == criteria.ProductSn).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var dataList = inspBarCodeList.Select(c => new ProductInspectInfo
            {
                InspLogId = c.InspLogId,
                QmsInspNo = c.CheckNo,
                InspType = (int)c.InspType
            }).ToList();

            return dataList;
        }



        /// <summary>
        /// 获取包装信息
        /// </summary>
        /// <param name="criteriaInfo">查询条件</param>
        /// <returns></returns>
        public virtual List<PackageInfo> GetPackageInfos(PackageInfoCriteria criteriaInfo)
        {
            var result = new List<PackageInfo>();

            var productPackage = Query<PackingRelation>().Exists<ItemLabel>((a, b) => b.Where(c => c.RelationId == a.Id && c.Label == criteriaInfo.ProductSn)).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (productPackage == null)
                return result;

            var itemLabel = Query<ItemLabel>().Where(c => c.RelationId == productPackage.Id && c.Label == criteriaInfo.ProductSn).FirstOrDefault();

            if (itemLabel == null)
                return result;

            var productPackageInfo = new PackageInfo()
            {
                Id = productPackage.Id.ToString(),
                TreePId = productPackage.TreePId.ToString(),
                PackageNo = productPackage.PackageNo,
                PackageUnitName = productPackage.PackageUnitName,
                Qty = productPackage.ItemQty,
                PackageTime = productPackage.PackedDate
            };
            result.Add(productPackageInfo);

            var itemLabelPackageInfo = new PackageInfo()
            {
                Id = itemLabel.Id.ToString(),
                TreePId = productPackage.Id.ToString(),
                PackageNo = itemLabel.Label,
                PackageUnitName = string.Empty,
                Qty = itemLabel.Qty,
                PackageTime = itemLabel.CreateDate
            };
            result.Add(itemLabelPackageInfo);

            var list = Query<PackingRelation>().Where(c => c.RootId == productPackage.RootId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (list.IsNullOrEmpty())
                return result;

            var parentTree = GetParentData(list.ToList(), productPackage);
            parentTree.ForEach(c =>
            {
                result.Add(new PackageInfo()
                {
                    Id = c.Id.ToString(),
                    TreePId = c.TreePId.HasValue ? c.TreePId.ToString() : string.Empty,
                    PackageNo = c.PackageNo,
                    PackageUnitName = c.PackageUnitName,
                    Qty = c.ItemQty,
                    PackageTime = c.PackedDate
                });

            });

            var storageBarcode = Query<ToStorageBarcode>().Exists<ToStorageBarcodeDetail>((a, b) => b.Where(t => t.ToStorageBarcodeId == a.Id && t.Barcode == criteriaInfo.ProductSn && a.IsStored == true)).FirstOrDefault();

            if (storageBarcode != null)
            {
                var productSnResult = result.FirstOrDefault(c => c.PackageNo == criteriaInfo.ProductSn);
                if (productSnResult != null)
                    productSnResult.StorageBarcode = storageBarcode.Barcode;
            }

            return result;
        }


        private List<PackingRelation> GetParentData(List<PackingRelation> dataList, PackingRelation packingRelation)
        {
            List<PackingRelation> result = new List<PackingRelation>();

            // 查找所有直接上级数据
            List<PackingRelation> parents = dataList.Where(d => d.Id == packingRelation.TreePId).ToList();

            foreach (PackingRelation parent in parents)
            {
                result.Add(parent);

                // 递归查找上级数据
                List<PackingRelation> grandParents = GetParentData(dataList, parent);
                result.AddRange(grandParents);
            }

            return result;
        }

        /// <summary>
        /// 获取Mes产品列表信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual MesProductInfo GetMesProductInfo(MesProductInfoCriteria criteria)
        {
            var result = new MesProductInfo();
            var query = Query<WipProductVersionReport>().Join<WorkOrder>((report, workOrder) => report.WorkOrderId == workOrder.Id).Where<WorkOrder>((report, workOrder) => workOrder.ProductId == criteria.ProductId);
            query.WhereIf(criteria.ProductSn.IsNotEmpty(), report => report.Sn == criteria.ProductSn);
            query.WhereIf<WorkOrder>(criteria.WorkOrderNo.IsNotEmpty(), (report, workOrder) => workOrder.No == criteria.WorkOrderNo);


            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var dataList = list.Select(c => new MesProductItemInfo
            {
                VersionId = c.Id,
                ProductId = c.WorkOrderProductId,
                ProductCode = c.ProductCode,
                ProductName = c.ProductName,
                ProductSn = c.Sn,
                ProductionLot = string.Empty,
                ProductExtPropName = c.WorkOrderExtPropName,
                Qty = 1,
                VersionName = c.VersionName,
                WorkOrderId = c.WorkOrderId,
                WorkOrderNo = c.WorkOrderNo,
                WorkShopName = c.WorkShopName,
                ProductionDate = c.FinishDateTime

            }).ToList();

            result.TotalCount = list.Count;
            result.Data = dataList;

            return result;
        }

        /// <summary>
        /// 获取Mes工序采集信息-反向追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual MesProcessCollectInfo GetMesProcessCollectInfo(MesProcessCollectInfoCriteria criteria)
        {
            var result = new MesProcessCollectInfo();
            var query = Query<WipProductReportProcess>().Where(c => c.VersionId == criteria.ProductVersionId);
            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var dataList = list.Select(c => new MesProcessCollectItemInfo
            {
                ReportProcessId = c.Id,
                CollectSn = c.Barcode,
                StateName = c.State.ToLabel(),
                StationName = c.StationName,
                ProcessName = c.ProcessName,
                ResourceName = c.ResourceName,
                Result = c.Result.ToLabel(),
                CollectTime = c.OperateTime,
                CollectBy = c.EmployeeName,
            }).ToList();

            result.TotalCount = list.Count;
            result.Data = dataList;

            return result;


        }
        /// <summary>
        /// 获取Mes关键件采集信息-反向追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual MesProcessCollectKeyItemInfo GetMesProcessCollectKeyItemInfo(MesProcessCollectKeyItemInfoCriteria criteria)
        {
            var result = new MesProcessCollectKeyItemInfo();
            var query = Query<WipProductReportProcessKeyItem>().Where(c => c.ProcessId == criteria.ReportProcessId);
            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var dataList = list.Select(c => new MesProcessCollectKeyItemItemInfo
            {
                ItemId = c.ItemId,
                ItemCode = c.ItemCode,
                ItemExtPropName = c.ItemExtPropName,
                ItemName = c.ItemName,
                Qty = c.Qty,
                SourceCode = c.SourceCode,
            }).ToList();

            result.TotalCount = list.Count;
            result.Data = dataList;

            return result;
        }
    }
}
