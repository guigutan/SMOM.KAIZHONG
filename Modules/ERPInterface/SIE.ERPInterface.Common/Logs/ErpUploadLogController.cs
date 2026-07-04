using Castle.Core.Internal;
using SIE.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 事物上传ERP记录控制器
    /// </summary>
    public partial class ErpUploadLogController : DomainController
    {
        /// <summary>
        /// 查询事物上传ERP记录
        /// </summary>
        /// <param name="criteria">接口下载异常查询实体</param>
        /// <returns>事物上传ERP记录</returns>
        public virtual EntityList<ErpUploadLog> GetErpUploadLogs(ErpUploadLogCriteria criteria)
        {
            var query = Query<ErpUploadLog>().Join<UploadTransaction>((x, y) => x.UploadTransactionId == y.Id);
            if (criteria.TransactionId.HasValue)
                query.Where(p => p.TransactionId == criteria.TransactionId);
            if (!criteria.OrderNo.IsNullOrEmpty())
                query.Where(p => p.OrderNo.Contains(criteria.OrderNo));
            if (!string.IsNullOrEmpty(criteria.OrderType))
            {
                var criteriaState = new List<int>();
                criteria.OrderType.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.OrderType));
            }
            if (criteria.IsSuccess.HasValue)
                query.Where(p => p.IsSuccess == criteria.IsSuccess);
            if (criteria.LogDate.BeginValue.HasValue)
                query.Where(p => p.UpdateDate >= criteria.LogDate.BeginValue);
            if (criteria.LogDate.EndValue.HasValue)
                query.Where(p => p.UpdateDate <= criteria.LogDate.EndValue);

            if (criteria.OrderInfoList.Count == 0)
                criteria.OrderInfoList.Add(new OrderInfo() { Property = "UpdateDate", SortIndex = 0, SortOrder = System.ComponentModel.ListSortDirection.Descending });
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
          
        }

        /// <summary>
        /// 更新事务上传记录
        /// </summary>
        /// <param name="ebsUploadLogId">事务ID</param>
        /// <param name="requestStr">修改报文</param>
        /// <returns></returns>
        public virtual bool UpdateEbsUploadLog(double ebsUploadLogId, string requestStr)
        {
            ErpUploadLog ebsUploadLog = RF.GetById<ErpUploadLog>(ebsUploadLogId);
            if (ebsUploadLog != null)
            {
                ebsUploadLog.RequestStr = requestStr;
                RF.Save(ebsUploadLog);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回请求的报文
        /// </summary>
        /// <param name="erplogId"></param>
        /// <returns></returns>
        public virtual string GetRequestStr(double erplogId)
        {
            return Query<ErpUploadLog>().Where(f => f.Id == erplogId).Select(f => f.RequestStr).FirstOrDefault<string>();
        }
    }
}
