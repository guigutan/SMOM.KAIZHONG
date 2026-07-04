using Microsoft.Scripting.Utils;
using SIE.Common;
using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.MES.BatchWIP.Products.ViewModels.BatchWipProductReport;
using SIE.MES.QTimes;
using SIE.MES.WIP;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次产品版本控制器
    /// </summary>
    public class BatchWipProductVersionController : DomainController
    {



        public virtual EntityList<BatchSplitMergeRecord> GetBatchSplitMergeRecord(PagingInfo pagingInfo, string code)
        {
            return Query<BatchSplitMergeRecord>().Where(m => m.OutputBatchNo == code||m.InputBatchNo== code).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次通用报表集合
        /// </summary>
        /// <param name="query">标准查询实体</param>
        /// <returns>批次通用报表集合</returns>
        public virtual EntityList<BatchWipProductVersion> GetBatchWipProductVersion(CriteriaQuery query)
        {
            return Query<BatchWipProductVersion>().Where(query.Criteria).Join<Core.Items.ItemBatchRule>((x, y) => x.Product.Item.Id == y.ItemId && y.RetrospectType == Core.Items.RetrospectType.Batch)
                .ToList(query.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品当前生产版本
        /// </summary>
        /// <param name="sn">产品条码</param>
        /// <returns>返回生产产品版本列表</returns>
        public virtual BatchWipProductVersion GetBatchWipProductVersion(string sn)
        {
            return Query<BatchWipProductVersion>().Where(p => p.BatchNo == sn).FirstOrDefault(eagerLoad: new EagerLoadOptions().LoadWith(BatchWipProductVersion.ProcessListProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 批量获取条码产品当前生产版本
        /// </summary>
        /// <param name="sns"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipProductVersion> GetBatchWipProductVersion(List<string> sns)
        {
            return Query<BatchWipProductVersion>().Where(p => sns.Contains(p.BatchNo)).ToList(eagerLoad: new EagerLoadOptions().LoadWith(BatchWipProductVersion.DefectListProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 根据进站明细获取对应出站明细
        /// </summary>
        /// <param name="inputDetail">工序进站明细</param>
        /// <param name="info">分页信息</param>
        /// <returns>批次关系</returns>
        public virtual EntityList<BatchWipProductProcessDetail> GetBatchWipProductProcessOutDetail(BatchWipProductProcessDetail inputDetail, PagingInfo info = null)
        {
            return Query<BatchWipProductProcessDetail>()
                .Where(p => p.PlugType == PlugType.Out && p.ProductProcessId == inputDetail.ProductProcessId && p.InputDetailId == inputDetail.Id)
                .OrderBy(p => p.OutputDate)
                .ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据进站明细获取对应出站明细
        /// </summary>
        /// <param name="processId">工序记录Id</param>
        /// <param name="info">分页信息</param>
        /// <returns>批次关系</returns>
        public virtual EntityList<BatchWipProductProcessDetail> GetBatchWipProductProcessInDetail(double processId, PagingInfo info = null)
        {
            return Query<BatchWipProductProcessDetail>()
                .Where(p => p.PlugType == PlugType.In && p.ProductProcessId == processId)
                .OrderBy(p => p.InputDate)
                .ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过批次采集记录Id获取批次采集工序列表
        /// </summary>
        /// <param name="processId">批次采集记录Id</param>
        /// <returns>批次采集工序列表</returns>
        public virtual EntityList<BatchWipProductProcessDetail> GetBatchWipProcessDetails(double processId)
        {
            return Query<BatchWipProductProcessDetail>().Where(p => p.ProductProcessId == processId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单工序过站数量（去除重复过站）
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="processId">工序Id</param>
        /// <param name="wipBatchNo">生产批次号</param>
        /// <returns>工单生产批次工序过站数量</returns>
        public virtual decimal GetWipBatchProcessMoveOutQty(double workOrderId, double processId, string wipBatchNo)
        {
            return Query<BatchWipProductProcess>().Join<BatchWipProductVersion>((p, v) => p.VersionId == v.Id && v.WorkOrderId == workOrderId && p.ProcessId == processId && v.BatchNo == wipBatchNo).Select(p => p.OutputQty).FirstOrDefault<decimal>();
        }

        /// <summary>
        /// 获取生产采集记录
        /// </summary>
        /// <param name="wipProductVersionId"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipProductProcess> GetBatchWipProductProcess(List<double> wipProductVersionId)
        {
            var wipProductProcessList = new EntityList<BatchWipProductProcess>();
            for (int i = 0; i < Math.Ceiling((double)wipProductVersionId.Count / 1000); i++)
            {
                var queryRsult = Query<BatchWipProductProcess>().Where(p => wipProductVersionId.Skip(i * 1000).Take(1000).Contains(p.VersionId));
                wipProductProcessList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductProcessList;
        }

        /// <summary>
        ///获取批次产品缺陷记录
        /// </summary>
        /// <param name="wipProductVersionId"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipProductDefect> GetBatchWipProductDefects(List<double> wipProductVersionId)
        {
            var wipProductDefectList = new EntityList<BatchWipProductDefect>();
            for (int i = 0; i < Math.Ceiling((double)wipProductVersionId.Count / 1000); i++)
            {
                var queryRsult = Query<BatchWipProductDefect>().Where(p => wipProductVersionId.Skip(i * 1000).Take(1000).Contains(p.VersionId));
                wipProductDefectList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductDefectList;
        }

        /// <summary>
        /// 获取批次缺陷责任
        /// </summary>
        /// <param name="productDefectIds"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipDefectResponsibility> GetBatchWipDefectResponsibilitys(List<double> productDefectIds)
        {
            var wipDefectResponsibilityList = new EntityList<BatchWipDefectResponsibility>();
            for (int i = 0; i < Math.Ceiling((double)productDefectIds.Count / 1000); i++)
            {
                var queryRsult = Query<BatchWipDefectResponsibility>().Where(p => productDefectIds.Skip(i * 1000).Take(1000).Contains(p.DefectId));
                wipDefectResponsibilityList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipDefectResponsibilityList;
        }

        /// <summary>
        /// 获取批次维修缺陷数据
        /// </summary>
        /// <param name="productDefectIds"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipDefectMeasure> GetBatchWipDefectMeasures(List<double> productDefectIds)
        {
            var wipDefectMeasureList = new EntityList<BatchWipDefectMeasure>();
            for (int i = 0; i < Math.Ceiling((double)productDefectIds.Count / 1000); i++)
            {
                var queryRsult = Query<BatchWipDefectMeasure>().Where(p => productDefectIds.Skip(i * 1000).Take(1000).Contains(p.DefectId));
                wipDefectMeasureList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipDefectMeasureList;
        }

        /// <summary>
        /// 获取维修记录
        /// </summary>
        /// <param name="wipProductVersionId"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipProductRepaire> GetBatchWipProductRepairs(List<double> wipProductVersionId)
        {
            var wipProductRepairList = new EntityList<BatchWipProductRepaire>();
            for (int i = 0; i < Math.Ceiling((double)wipProductVersionId.Count / 1000); i++)
            {
                var queryRsult = Query<BatchWipProductRepaire>().Where(p => wipProductVersionId.Skip(i * 1000).Take(1000).Contains(p.VersionId));
                wipProductRepairList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductRepairList;
        }

        /// <summary>
        /// 根据工单ids获取批次关键件
        /// </summary>
        /// <param name="sameWoIds"></param>
        /// <returns></returns>
        public virtual List<WipProductKeyItem> GetBatchWipKeyItemByWoIds(List<double> sameWoIds)
        {
            List<WipProductKeyItem> batchKeyItemList = new List<WipProductKeyItem>();
            sameWoIds.SplitDataExecute(tempIds =>
            {
                var list = DB.Query<BatchWipProductProcessKeyItem>()
                    .Join<BatchWipProductProcessDetail>((x, y) => x.DetailId == y.Id)
                    .Join<BatchWipProductProcessDetail, BatchWipProductProcess>((x, y) => x.ProductProcessId == y.Id)
                    .Join<BatchWipProductProcess, BatchWipProductVersion>((x, y) => x.VersionId == y.Id && tempIds.Contains(y.WorkOrderId))
                    .GroupBy<BatchWipProductProcessDetail, BatchWipProductProcess, BatchWipProductVersion>((k, d, p, v) => new
                    {
                        k.ItemId,
                        k.ItemExtProp,
                        v.WorkOrderId,
                    })
                    .Select<BatchWipProductProcessDetail, BatchWipProductProcess, BatchWipProductVersion>((k, d, p, v) => new
                    {
                        ItemId = k.ItemId,
                        ItemExtProp = k.ItemExtProp,
                        WoOrderId = v.WorkOrderId,
                        Qty = k.Qty.SUM(),
                    }).ToList<WipProductKeyItem>().ToList();
                batchKeyItemList.AddRange(list);
            });
            return batchKeyItemList;
        }

        /// <summary>
        /// 根据工单id获取批次关键件清单
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual List<WipProductKeyItem> GetBatchWipKeyItemByWoId(double woId)
        {
            return DB.Query<BatchWipProductProcessKeyItem>()
                    .Join<BatchWipProductProcessDetail>((x, y) => x.DetailId == y.Id)
                    .Join<BatchWipProductProcessDetail, BatchWipProductProcess>((x, y) => x.ProductProcessId == y.Id)
                    .Join<BatchWipProductProcess, BatchWipProductVersion>((x, y) => x.VersionId == y.Id && woId == y.WorkOrderId)
                    .GroupBy<BatchWipProductProcessDetail, BatchWipProductProcess, BatchWipProductVersion>((k, d, p, v) => new
                    {
                        k.ItemId,
                        k.ItemExtProp,
                    })
                    .Select<BatchWipProductProcessDetail, BatchWipProductProcess, BatchWipProductVersion>((k, d, p, v) => new
                    {
                        ItemId = k.ItemId,
                        ItemExtProp = k.ItemExtProp,
                        Qty = k.Qty.SUM(),
                    }).ToList<WipProductKeyItem>().ToList();
        }

        /// <summary>
        /// 获取特定时间范围内的生产采集记录
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public virtual List<WipProductVersionInfo> GetWipProductVersionInfos(QTimeReportViewModelCriteria criteria)
        {
            var batchRecords = Query<BatchWipProductProcess>()
                .Join<BatchWipProductVersion>((x, y) => x.VersionId == y.Id)
                .Where(p => p.InputDate >= criteria.CollectTime.BeginValue && (p.OutputDate <= criteria.CollectTime.EndValue || p.OutputDate == null))
                .WhereIf<BatchWipProductVersion>(criteria.WipResourceId != null, (x, y) => x.ResourceId == criteria.WipResourceId)
                .WhereIf<BatchWipProductVersion>(criteria.WorkOrderId != null, (x, y) => y.WorkOrderId == criteria.WorkOrderId)
                .WhereIf<BatchWipProductVersion>(criteria.Sn.IsNotEmpty(), (x, y) => y.BatchNo.Contains(criteria.Sn))
                .WhereIf<BatchWipProductVersion>(criteria.ProCode.IsNotEmpty(), (x, y) => y.WorkOrder.Product.Code.Contains(criteria.ProCode))
                .WhereIf<BatchWipProductVersion>(criteria.ProName.IsNotEmpty(), (x, y) => y.WorkOrder.Product.Name.Contains(criteria.ProName))
                .Select<BatchWipProductVersion>((x, y) => new
                {
                    ParentId = y.Id,
                    Id = x.Id,
                    Sn = y.BatchNo,
                    WorkOrderId = y.WorkOrderId,
                    WoNo = y.WorkOrder.No,
                    WipResourceId = x.ResourceId,
                    WipResourceCode = x.Resource.Code,
                    ProductId = y.WorkOrder.ProductId,
                    ProductCode = y.WorkOrder.Product.Code,
                    ProductName = y.WorkOrder.Product.Name,
                    ProcessId = x.ProcessId,
                    ProcessName = x.Process.Name,
                    Qty = y.Qty,
                    IsBatch = true,
                    InTime = x.InputDate,
                    OutTime = x.OutputDate,
                }).ToList<WipProductVersionInfo>().ToList();
            return batchRecords;
        }

        /// <summary>
        /// 获取工位库龄查询报表信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipProductReport> GetBatchWipProductVersionsByReport(BatchWipProductReportCriteria criteria)
        {
            EntityList<BatchWipProductReport> batchWipProductReport = new EntityList<BatchWipProductReport>();
            EntityList<BatchWipProductReport> result = new EntityList<BatchWipProductReport>();
            var query = Query<BatchWipProductVersion>().Where(p => !p.IsFinish && p.RemainQty > 0);
            if (criteria.ProcessId.HasValue && criteria.ProcessId > 0)
            {
                query.Where(p => p.ProcessId == criteria.ProcessId);
            }
            if (criteria.StationId.HasValue && criteria.StationId > 0)
            {
                query.Where(p => p.StationId == criteria.StationId);
            }
            if (criteria.WorkOrderId.HasValue && criteria.WorkOrderId > 0)
            {
                query.Where(p => p.WorkOrderId == criteria.WorkOrderId);
            }
            if (criteria.ItemId.HasValue && criteria.ItemId > 0)
            {
                query.Where(p => p.WorkOrder.ProductId == criteria.ItemId);
            }
            if (!criteria.BatchNo.IsNullOrEmpty())
            {
                query.Where(p => p.BatchNo.Contains(criteria.BatchNo));
            }
            var batchWipProductVersion = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            batchWipProductVersion.ForEach(p =>
            {
                var item = new BatchWipProductReport();
                item.BatchNo = p.BatchNo;
                item.BatchQty = p.RemainQty;
                item.StationName = p.StationName;
                item.ProcessName = p.ProcessName;
                item.ProductCode = p.ProductCode;
                item.ProductName = p.ProductName;
                item.WorkOrderNo = p.WorkOrderNo;
                //1、获取与主表工序一致的最新一笔采集记录（按创建时间），如果这笔采集记录出入类型为入站，报表才做显示
                var BatchWipRecordItem = p.BatchWipRecordList.Where(f => f.ProcessId == p.ProcessId).OrderByDescending(f => f.CreateDate).FirstOrDefault();
                if (BatchWipRecordItem != null && BatchWipRecordItem.InOutType == PlugType.In)
                {
                    item.SluggishStorageAgeDays = 0;
                    var inTime = BatchWipRecordItem.CreateDate;
                    var nowTime = DateTime.Now;
                    var time = (nowTime - inTime).TotalDays;
                    item.SluggishStorageAgeDays = Math.Round(time, 3);
                    batchWipProductReport.Add(item);
                }
            });
            batchWipProductReport = batchWipProductReport.OrderByDescending(p => p.SluggishStorageAgeDays).AsEntityList();
            List<BatchWipProductReport> sortList;
            if (criteria.OrderInfoList.Any())
            {
                sortList = SetSortField<BatchWipProductReport>(batchWipProductReport.ToList(), criteria.OrderInfoList);
            }
            else
            {
                sortList = batchWipProductReport.ToList();
            }

            var list = sortList.Skip(criteria.PagingInfo.PageSize * (criteria.PagingInfo.PageNumber - 1)).Take(criteria.PagingInfo.PageSize);
            result.AddRange(list);
            result.SetTotalCount(batchWipProductReport.Count);
            criteria.OrderInfoList.Clear();
            criteria.MarkSaved();
            return result;
        }

        /// <summary>
        /// 排序操作
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="sortList">排序列表</param>
        /// <param name="orderInfos">排序栏位</param>
        /// <returns>排序列表</returns>
        private List<T> SetSortField<T>(List<T> sortList, IList<OrderInfo> orderInfos) where T : ViewModel
        {
            List<T> sorts;
            var orderInfo = orderInfos.First();
            if (orderInfo.SortOrder == ListSortDirection.Ascending)
            {
                sorts = sortList.OrderBy(x => GetPropertyValue(x, orderInfo.Property)).ToList();
            }
            else
            {
                sorts = sortList.OrderByDescending(x => GetPropertyValue(x, orderInfo.Property)).ToList();
            }

            return sorts;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="obj">obj</param>
        /// <param name="property">属性</param>
        /// <returns>属性值</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// 通过批次获取生产版本
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public virtual BatchWipProductVersion GetBatchWipProductByNo(string batch)
        {
            return Query<BatchWipProductVersion>().Where(p => p.BatchNo == batch).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}