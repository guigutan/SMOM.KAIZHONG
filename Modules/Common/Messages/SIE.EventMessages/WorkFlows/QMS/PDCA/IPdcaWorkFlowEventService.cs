namespace SIE.EventMessages.WorkFlows.QMS.PDCA
{
    /// <summary>
    /// PDCA工作流服务接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultPdcaWorkFlowEventService))]
    public interface IPdcaWorkFlowEventService
    {
        /// <summary>
        /// 触发原因分析节点
        /// </summary>
        /// <param name="model"></param>

        void TriggerReasonAnalysis(ReasonAnalysisModel model);

        /// <summary>
        /// 触发填写对策节点
        /// </summary>
        /// <param name="model"></param>

        void TriggerFillInMeasure(FillInMeasureModel model);
        /// <summary>
        /// 触发过程确认节点
        /// </summary>
        /// <param name="model"></param>
        void TriggerProcessConfirm(ProcessConfirmModel model);
        /// <summary>
        /// 触发效果验证节点
        /// </summary>
        /// <param name="model"></param>
        void TriggerEfeectVerfication(EfeectVerficationModel model);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultPdcaWorkFlowEventService : IPdcaWorkFlowEventService
    {
        /// <summary>
        /// 触发原因分析节点
        /// </summary>
        /// <param name="model"></param>

        public void TriggerReasonAnalysis(ReasonAnalysisModel model)
        {
            // 触发原因分析节点
        }

        /// <summary>
        /// 触发填写对策节点
        /// </summary>
        /// <param name="model"></param>
        public void TriggerFillInMeasure(FillInMeasureModel model)
        {
            // 触发填写对策节点
        }
        /// <summary>
        /// 触发过程确认节点
        /// </summary>
        /// <param name="model"></param>
        public void TriggerProcessConfirm(ProcessConfirmModel model)
        {
            // 触发过程确认节点
        }

        /// <summary>
        /// 触发效果验证节点
        /// </summary>
        /// <param name="model"></param>
        public void TriggerEfeectVerfication(EfeectVerficationModel model)
        {
            // 触发效果验证节点
        }
    }
}
