using SIE.Domain;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;

namespace SIE.Equipments.WorkFlows
{
    /// <summary>
    /// 工作流审批记录控制器
    /// </summary>
    public class WorkFlowRecordController : DomainController
    {
        /// <summary>
        /// 创建工作流审批记录
        /// </summary>
        /// <param name="workFlowBillIds">表单ID列表</param>
        /// <param name="billType">单据类型</param>
        /// <param name="approvalResult">审核结果</param>
        /// <param name="dateTime">时间</param>
        /// <param name="remark">审核意见</param>
        /// <returns></returns>
        public virtual void CreateWorkFlowRecords(List<double> workFlowBillIds, string billType, ApprovalResult approvalResult, DateTime dateTime, string remark)
        {
            EntityList<WorkFlowRecord> workFlowRecords = new EntityList<WorkFlowRecord>();
            foreach (var id in workFlowBillIds)
            {
                workFlowRecords.Add(new WorkFlowRecord()
                {
                    ApprovalDatetime = dateTime,
                    ApprovalResult = approvalResult,
                    ApproverId = RT.IdentityId,
                    SourceId = id,
                    SourceType = billType,
                    Remark = remark
                });
            }

            RF.Save(workFlowRecords);
        }

        /// <summary>
        /// 根据来源id获取审核记录
        /// </summary>
        /// <param name="sourceId">来源id</param>
        /// <param name="billType">单据类型</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>审核记录</returns>
        public virtual EntityList<WorkFlowRecord> GetWorkFlowRecordBySourceId(double sourceId, string billType, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<WorkFlowRecord>().Where(p => p.SourceId == sourceId && p.SourceType == billType).OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
