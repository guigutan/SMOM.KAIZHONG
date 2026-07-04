using SIE.Common.Signature;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.WorkFlow.Activities.Audit;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.DataTraceAudits
{
    /// <summary>
    /// 数据追溯审核控制器
    /// </summary>
    public class DataTraceAuditController : DomainController
    {
        /// <summary>
        /// 保存追溯审核签名信息
        /// </summary>
        /// <param name="traceMainDataId"></param>
        /// <param name="flowInstanceId"></param>
        /// <param name="activityId"></param>
        /// <param name="entityType"></param>
        /// <param name="flowTaskId"></param>
        public virtual void SaveDataTraceSignature(double traceMainDataId, double flowInstanceId, string activityId, string entityType, double flowTaskId)
        {
            //查询审核数据
            var audit = Query<AuditActivity>().Where(p => p.FlowTaskId == flowTaskId).FirstOrDefault();
            if (audit == null)
                throw new ValidationException("找不到对应审核数据。".L10N());

            //新增追溯签名信息
            SaveNewTraceSignature(traceMainDataId, flowInstanceId, activityId, entityType, audit);
        }

        /// <summary>
        /// 新增追溯签名实体
        /// </summary>
        /// <param name="traceMainDataId"></param>
        /// <param name="flowInstanceId"></param>
        /// <param name="activityId"></param>
        /// <param name="entityType"></param>
        /// <param name="audit"></param>
        private void SaveNewTraceSignature(double traceMainDataId, double flowInstanceId, string activityId, string entityType, AuditActivity audit)
        {
            var traceSign = new DataTraceSignature();
            traceSign.FlowInstanceId = flowInstanceId;
            traceSign.ActivityId = activityId;
            traceSign.TraceEntityType = entityType;
            traceSign.TraceMainDataId = traceMainDataId;
   
            RF.Save(traceSign);

            SIE.Threading.AsyncHelper.InvokeSafe(() =>
            {
                //异步等待一段时间后，才能获取签名记录
                System.Threading.Thread.Sleep(1000 * 30);
                //获取签名信息
                var signature = Query<SignatureRecord>().Where(p => p.ModelType == typeof(AuditActivity).GetQualifiedName() && p.ModelId == audit.Id).FirstOrDefault();
                if (signature == null)
                    return;

                traceSign.SignatureRecordId = signature.Id;
                traceSign.SignatureById = signature.SignatureById;

                RF.Save(traceSign);
            });
        }

        /// <summary>
        /// 获取数据追溯签名人ID
        /// </summary>
        /// <param name="traceMainDataId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public virtual double? GetDataTraceAuditSignId(double traceMainDataId,string entityType)
        {
            var q = Query<DataTraceSignature>().Where(p=>p.TraceMainDataId == traceMainDataId  && p.TraceEntityType == entityType);

            return q.FirstOrDefault()?.SignatureById;
        }
    }
}
