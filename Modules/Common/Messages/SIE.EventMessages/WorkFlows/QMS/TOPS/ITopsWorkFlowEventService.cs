namespace SIE.EventMessages.WorkFlows.QMS.TOPS
{
    /// <summary>
    /// 8D-工作流接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultTopsEventService))]
    public interface ITopsWorkFlowEventService
    {
        /// <summary>
        ///  撤回工作流（撤回至首节点）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void WithDrawWorkflow(WithDrawWorkflowModel model);

        /// <summary>
        ///  执行工作流（全新或恢复）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void ExecuteWorkflow(ExecuteWorkflowModel model);

    }

    /// <summary>
    /// 接口的默认实现
    /// </summary>
    public class DefaultTopsEventService : ITopsWorkFlowEventService
    {
        /// <summary>
        ///  撤回工作流（撤回至首节点）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void WithDrawWorkflow(WithDrawWorkflowModel model)
        {
            // Method intentionally left empty.
        }

        /// <summary>
        ///  执行工作流（全新或恢复）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void ExecuteWorkflow(ExecuteWorkflowModel model)
        {
            // Method intentionally left empty.
        }
    }

}
