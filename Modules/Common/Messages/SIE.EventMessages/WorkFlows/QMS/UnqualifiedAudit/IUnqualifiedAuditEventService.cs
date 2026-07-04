using SIE.Core.Inspections;

namespace SIE.EventMessages.WorkFlows.QMS.UnqualifiedAudit
{
    /// <summary>
    /// 不合格审核-工作流接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultUnqualifiedAuditEventService))]
    public interface IUnqualifiedAuditEventService
    {
        /// <summary>
        /// 执行工作流
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void ExecuteWorkflow(ExecuteWorkflowModel model);

        /// <summary>
        /// 触发处理意见节点
        /// </summary>
        /// <param name="model"></param>

        void TriggerProcessingOpinion(ProcessingOpinionModel model);

        /// <summary>
        /// 是否能匹配到不合格审核
        /// </summary>
        /// <param name="inspectionType">检验类型</param>
        /// <param name="categoryId">质量分类ID</param>
        /// <returns></returns>
        bool HasFailedAuditWorkflowDefinition(InspectionType? inspectionType, double? categoryId);
    }

    /// <summary>
    /// 接口的默认实现
    /// </summary>
    class DefaultUnqualifiedAuditEventService : IUnqualifiedAuditEventService
    {
        /// <summary>
        /// 执行工作流
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void ExecuteWorkflow(ExecuteWorkflowModel model)
        {
            // 执行工作流
        }

        /// <summary>
        /// 是否能匹配到不合格审核
        /// </summary>
        /// <param name="inspectionType">检验类型</param>
        /// <param name="categoryId">质量分类ID</param>
        /// <returns></returns>
        public bool HasFailedAuditWorkflowDefinition(InspectionType? inspectionType, double? categoryId)
        {
            return false;
        }

        /// <summary>
        /// 触发处理意见节点
        /// </summary>
        /// <param name="model"></param>
        public void TriggerProcessingOpinion(ProcessingOpinionModel model)
        {
            // 触发处理意见节点
        }
    }
}
