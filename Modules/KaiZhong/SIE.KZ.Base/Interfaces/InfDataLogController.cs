using Newtonsoft.Json;
using SIE.Domain;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    public class InfDataLogController : DomainController
    {

        /// <summary>
        /// 0是Success 1是fail  原来有存list，后面去掉了，不想改动太多引用的地方 后续可以直接用apiResult.SuccessCount
        /// </summary>
        /// <param name="type"></param>
        public virtual void UpadateLogData<T>(InfDataLog erpDataInfLog, List<T> list, ApiCommonRes apiResult, string message = null, int type = 0)
        {
            if (type == 0)
            {
                erpDataInfLog.TipMsg = $"{erpDataInfLog.InfType?.ToLabel()}接口保存成功{list.Count}条!";
                erpDataInfLog.CallResult = CallResult.Success;
                erpDataInfLog.ErrorMsg = JsonConvert.SerializeObject(apiResult.ErrorList);
            }
            else
            {
                erpDataInfLog.ErrorMsg = $"{erpDataInfLog.InfType?.ToLabel()}接口调用失败!失败原因是{message}";
                //保存MES与另一方接口接口日志
                erpDataInfLog.CallResult = CallResult.Fail;
            }
            erpDataInfLog.ResponseContent = JsonConvert.SerializeObject(apiResult);
            erpDataInfLog.EndDate = DateTime.Now;
            RF.Save(erpDataInfLog);
        }

        /// <summary>
        /// 查询MOM与其它系统接口日志
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<InfDataLog> GetInfDataLog(InfDataLogCriteria criteria)
        {
            var query = Query<InfDataLog>();
            if (criteria.BeginDate.BeginValue.HasValue)
                query.Where(p => p.BeginDate >= criteria.BeginDate.BeginValue);
            if (criteria.BeginDate.EndValue.HasValue)
                query.Where(p => p.BeginDate <= criteria.BeginDate.EndValue);
            if (criteria.InfType.HasValue)
                query.Where(p => p.InfType == criteria.InfType);
            if (criteria.CallDirection.HasValue)
                query.Where(p => p.CallDirection == criteria.CallDirection);
            if (criteria.CallResult.HasValue)
                query.Where(p => p.CallResult == criteria.CallResult);
            if (!criteria.Remark.IsNullOrEmpty())
                query.Where(p => p.Remark.Contains('%' + criteria.Remark + '%'));
            if (!criteria.ErrorMsg.IsNullOrEmpty())
                query.Where(p => p.ErrorMsg.Contains('%' + criteria.ErrorMsg + '%'));
            if (!criteria.TipMsg.IsNullOrEmpty())
                query.Where(p => p.TipMsg.Contains('%' + criteria.TipMsg + '%'));
            if (!criteria.RequestContent.IsNullOrEmpty())
                query.Where(p => p.RequestContent.Contains('%' + criteria.RequestContent + '%'));
            if (!criteria.ResponseContent.IsNullOrEmpty())
                query.Where(p => p.ResponseContent.Contains('%' + criteria.ResponseContent + '%'));
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo);
        }


        public virtual InfDataLog SaveErpDataInfLog(InfType type
                                               , string requestContent
                                              , DateTime beginDate
                                              , CallDirection callDirection
                                              , CallResult callResult
                                              , int qty
                                              , string tipMsg = null
                                              , string errorMsg = null
                                              , string responseContent = null)
        {
            InfDataLog entity = null;
            using (var trans = DB.AutonomousTransactionScope(BaseEntityDataProvider.ConnectionStringName))
            {
                entity = new InfDataLog();
                entity.InfType = type;
                entity.RequestContent = requestContent;
                entity.BeginDate = beginDate;
                entity.CallDirection = callDirection;
                entity.CallResult = callResult;
                entity.Qty = qty;
                entity.TipMsg = tipMsg;
                entity.ErrorMsg = errorMsg;
                entity.ResponseContent = responseContent;
                //entity.ReLoadCount = 0;
                RF.Save(entity);

                trans.Complete();
            }

            return entity;
        }

    }
}
