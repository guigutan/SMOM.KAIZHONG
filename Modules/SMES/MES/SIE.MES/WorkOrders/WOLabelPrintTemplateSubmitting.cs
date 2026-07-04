using SIE.Core.Items;
using SIE.Domain;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单保存前需要保存打印设置
    /// </summary>
    [System.ComponentModel.DisplayName("工单保存前需要保存打印设置")]
    [System.ComponentModel.Description("工单保存前需要保存打印设置")]
    public class WOLabelPrintTemplateSubmitting : OnSubmitting<WorkOrder>
    {
        /// <summary>
        /// 保存工单前执行
        /// </summary>
        /// <param name="entity">工单实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(WorkOrder entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                LabelPrintTemplate labelPrintTemplate = entity.GetProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty);
                if (labelPrintTemplate != null)
                {
                    var template = RF.GetById<LabelPrintTemplate>(labelPrintTemplate.Id);
                    if (template == null)
                    {
                        labelPrintTemplate.PersistenceStatus = PersistenceStatus.New;
                    }

                    RF.Save(labelPrintTemplate);
                    labelPrintTemplate.MarkSaved();
                }
            }
        }
    }
}
