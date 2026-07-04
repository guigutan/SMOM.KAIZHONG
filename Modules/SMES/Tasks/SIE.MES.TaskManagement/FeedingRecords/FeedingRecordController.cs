using Microsoft.Scripting.Utils;
using SIE.Domain;
using SIE.EventMessages.ErpCommon;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using SIE.MES.TaskManagement.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 上料控制器
    /// </summary>
    public class FeedingRecordController : DomainController
    {

        #region 余料称重记录

        /// <summary>
        /// 根据余料称重记录Id获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<ScrapWeighingRecord> GetScrapWeighingRecordsByIds(List<double> ids)
        {
            var list = ids.SplitContains(i =>
            {
                return Query<ScrapWeighingRecord>().Where(p => i.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 修改余料称重记录
        /// </summary>
        /// <param name="records"></param>
        public virtual void EditScrapWeighingRecord(List<ScrapWeighingRecord> records)
        {
            var ids = records.Select(p => p.Id).Distinct().ToList();

            var list = ids.SplitContains(i =>
            {
                return Query<ScrapWeighingRecord>().Where(p => i.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            EntityList<ScrapWeighingRecordEditLog> logs = new EntityList<ScrapWeighingRecordEditLog>();

            foreach (var l in list)
            {
                var record = records.FirstOrDefault(p => p.Id == l.Id);
                //记录日志
                logs.Add(new ScrapWeighingRecordEditLog()
                {
                    PersistenceStatus = PersistenceStatus.New,
                    ScrapWeighingRecordId = l.Id,
                    OldEditQty = l.EditQty,
                    NewEditQty = record.EditQty
                });

                l.EditQty = l.ActualQty;
                l.ActualQty = record.ActualQty;
                l.DeductedQty = l.RemainingQty - l.ActualQty;
                l.DiffQty = l.ActualQty - l.RemainingQty;
                l.PersistenceStatus = PersistenceStatus.Modified;
            }

            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                if (list.Count > 0)
                {
                    RF.Save(list);
                    RF.Save(logs);

                    //修改事务上传的数量，改成现在的修改数量
                    RT.Service.Resolve<IUploadLogControllercs>().EditScrapWeighingRecordQty(ids);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 余料称重记录查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ScrapWeighingRecord> CriteriaScrapWeighingRecords(ScrapWeighingRecordCriteria criteria)
        {
            var q = Query<ScrapWeighingRecord>();
            if (!criteria.Sn.IsNullOrEmpty())
                q.Where(p => p.Sn.Contains(criteria.Sn));
            if (!criteria.Lot.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Lot.Contains(criteria.Lot));
            if (!criteria.ItemCode.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Item.Code.Contains(criteria.ItemCode));
            if (!criteria.ItemName.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Item.Name.Contains(criteria.ItemName));

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        #endregion

        #region 扣料记录修改日志

        /// <summary>
        /// 根据报工记录获取扣料记录修改日志
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<DeductionRecordEditLog> GetDeductionRecordEditLogsByReportId(double id, PagingInfo pagingInfo = null)
        {
            var list = Query<DeductionRecordEditLog>().Where(p => p.DeductionRecord.ReportRecordId == id).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        #endregion

        #region 扣料记录

        /// <summary>
        /// 根据ID获取扣料记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<DeductionRecord> GetDeductionRecordsByIds(List<double> ids)
        {
            var list = ids.SplitContains(i =>
            {
                return Query<DeductionRecord>().Where(p => i.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="records"></param>
        public virtual void EditDeductionRecord(List<DeductionRecord> records)
        {
            var ids = records.Select(p => p.Id).Distinct().ToList();

            var list = ids.SplitContains(i =>
            {
                return Query<DeductionRecord>().Where(p => i.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            EntityList<DeductionRecordEditLog> logs = new EntityList<DeductionRecordEditLog>();

            foreach (var l in list)
            {
                var record = records.FirstOrDefault(p => p.Id == l.Id);
                //记录日志
                logs.Add(new DeductionRecordEditLog()
                {
                    PersistenceStatus = PersistenceStatus.New,
                    DeductionRecordId = l.Id,
                    OldEditQty = l.EditQty,
                    NewEditQty = record.EditQty
                });

                l.EditQty = record.EditQty;
                l.PersistenceStatus = PersistenceStatus.Modified;
            }

            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                if (list.Count > 0)
                {
                    RF.Save(list);
                    RF.Save(logs);
                    //修改事务上传的数量，改成现在的修改数量
                    RT.Service.Resolve<IUploadLogControllercs>().EditDeductionRecordQty(ids);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 扣料记录查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<DeductionRecord> CriteriaDeductionRecords(DeductionRecordCriteria criteria)
        {
            var q = Query<DeductionRecord>();
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
                q.Where(p => p.ReportRecord.WorkOrder.No.Contains(criteria.WorkOrderNo));
            if (!criteria.TaskNo.IsNullOrEmpty())
                q.Where(p => p.ReportRecord.DispatchTask.No.Contains(criteria.TaskNo));
            if (!criteria.ProductCode.IsNullOrEmpty())
                q.Where(p => p.ReportRecord.WorkOrder.Product.Code.Contains(criteria.ProductCode));
            if (!criteria.ProductName.IsNullOrEmpty())
                q.Where(p => p.ReportRecord.WorkOrder.Product.Name.Contains(criteria.ProductName));
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Where(p => p.ReportRecord.WorkOrder.Product.ShortDescription.Contains(criteria.ShortDescription));
            if (!criteria.BatchNo.IsNullOrEmpty())
                q.Where(p => p.ReportRecord.BatchNo.Contains(criteria.BatchNo));
            if (!criteria.Resource.IsNullOrEmpty())
                q.Where(p => p.Resource.Name.Contains(criteria.Resource));
            if (!criteria.Process.IsNullOrEmpty())
                q.Where(p => p.ReportRecord.Process.Code.Contains(criteria.Process) || p.ReportRecord.Process.Name.Contains(criteria.Process));
            if (!criteria.ItemCode.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Item.Code.Contains(criteria.ItemCode));
            if (!criteria.ItemName.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Item.Name.Contains(criteria.ItemName));
            if (!criteria.ItemShortDescription.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Item.ShortDescription.Contains(criteria.ItemShortDescription));
            if (criteria.CreateDate.BeginValue != null)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue != null)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            if (!criteria.ItemLabel.IsNullOrEmpty())
                q.Where(p => p.FeedingItemLabel.Contains(criteria.ItemLabel));
            if (!criteria.ItemLabelLot.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Lot.Contains(criteria.ItemLabelLot));
            if (!criteria.Mblnr.IsNullOrEmpty())
                q.Where(p => p.Mblnr.Contains(criteria.Mblnr));
            if (!criteria.Mjahr.IsNullOrEmpty())
                q.Where(p => p.Mjahr.Contains(criteria.Mjahr));
            if (!criteria.UploadResult.IsNullOrEmpty())
                q.Where(p => p.UploadResult.Contains(criteria.UploadResult));
            if (!criteria.Licha.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Licha.Contains(criteria.Licha));
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;

        }
        #endregion

        /// <summary>
        /// 上料记录查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<FeedingRecord> CriteriaFeedingRecords(FeedingRecordCriteria criteria)
        {
            var q = Query<FeedingRecord>();
            //if (!criteria.WorkOrder.IsNullOrEmpty())
            //    q.Where(p => p.DispatchTask.WorkOrder.No.Contains(criteria.WorkOrder));
            //if (!criteria.TaskNo.IsNullOrEmpty())
            //    q.Where(p => p.DispatchTask.No.Contains(criteria.TaskNo));
            //if (!criteria.Process.IsNullOrEmpty())
            //    q.Where(p => p.DispatchTask.Process.Code.Contains(criteria.Process));
            if (!criteria.FeedingAreaCode.IsNullOrEmpty())
                q.Where(p => p.FeedingArea.Code.Contains(criteria.FeedingAreaCode));
            if (!criteria.FeedingAreaName.IsNullOrEmpty())
                q.Where(p => p.FeedingArea.Name.Contains(criteria.FeedingAreaName));
            if (!criteria.WipResource.IsNullOrEmpty())
                q.Where(p => p.Resource.Code.Contains(criteria.WipResource));
            if (!criteria.WipResourceName.IsNullOrEmpty())
                q.Where(p => p.Resource.Name.Contains(criteria.WipResourceName));
            if (!criteria.ItemCode.IsNullOrEmpty())
                q.Where(p => p.Item.Code.Contains(criteria.ItemCode));
            if (!criteria.ItemName.IsNullOrEmpty())
                q.Where(p => p.Item.Name.Contains(criteria.ItemName));
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Where(p => p.Item.ShortDescription.Contains(criteria.ShortDescription));
            if (!criteria.Label.IsNullOrEmpty())
                q.Where(p => p.FeedingItemLabel.Contains(criteria.Label));
            if (criteria.CreateDate.BeginValue != null)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue != null)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            if (!criteria.ItemLabelLot.IsNullOrEmpty())
                q.Where(p => p.ItemLabel.Lot.Contains(criteria.ItemLabelLot));

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;

        }


        /// <summary>
        /// 根据资源获取上料记录
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="eagerLoad"></param>
        /// <param name="isGetFeedArea">是否包含供料区</param>
        /// <returns></returns>
        public virtual EntityList<FeedingRecord> GetFeedingRecordsByResourceId(double resourceId, EagerLoadOptions eagerLoad = null, bool isGetFeedArea = false)
        {
            var areIds = new List<double?>();
            if (isGetFeedArea)
            {
                var areRes = Query<FeedingAreaReource>().Where(p => p.ResourceId == resourceId && p.FeedingArea.State == State.Enable).ToList();
                areIds.AddRange(areRes.Select(p => (double?)p.FeedingAreaId).ToList());
            }
            var q = Query<FeedingRecord>().Where(p => (p.ResourceId == resourceId || areIds.Contains(p.FeedingAreaId)) && p.RemainingQty > 0);
            var list = q.ToList(null, eagerLoad);
            return list;
        }


        /// <summary>
        /// 根据资源获取上料记录
        /// </summary>
        /// <param name="reportRecordId"></param>
        /// <returns></returns>
        public virtual EntityList<DeductionRecord> GetDeductionRecordsByReportId(double reportRecordId)
        {
            var q = Query<DeductionRecord>().Where(p => p.ReportRecordId == reportRecordId);
            var list = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据资源获取上料记录
        /// </summary>
        /// <param name="reportRecordIdList"></param>
        /// <returns></returns>
        public virtual EntityList<DeductionRecord> GetDeductionRecordsByReportIdList(List<double> reportRecordIdList)
        {
            var q = Query<DeductionRecord>().Where(p => reportRecordIdList.Contains((double)p.ReportRecordId));
            var list = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 保存上料记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public virtual bool SaveFeedingRecord(FeedingRecord record)
        {
            try
            {
                RF.Save(record);
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 上料标签是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual FeedingRecord GetFeedingRecord(string name )
        {
           return Query<FeedingRecord>().Where(p => p.FeedingItemLabel == name).FirstOrDefault();
        }

        /// <summary>
        /// 获取供料区
        /// </summary>
        /// <param name="areCode"></param>
        /// <returns></returns>
        public virtual FeedingArea GetFeedingArea(string areCode)
        {
            var area = Query<FeedingArea>().Where(p => p.Code == areCode).FirstOrDefault();
            return area;
        }

        /// <summary>
        /// 删除供料区资源
        /// </summary>
        /// <param name="selectIds"></param>
        public virtual void DeleteFeedingAreaResources(List<double> selectIds)
        {
            var ret = DB.Delete<FeedingAreaReource>().Where(p => selectIds.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 删除料应区物料
        /// </summary>
        /// <param name="selectIds"></param>
        public virtual void DeleteFeedingAreaItems(List<double> selectIds)
        {
            var ret = DB.Delete<FeedingAreaItem>().Where(p => selectIds.Contains(p.Id)).Execute();
        }
    }
}
