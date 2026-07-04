using SIE.Core.Items;
using SIE.Domain;


namespace SIE.Items.Items
{
    /// <summary>
    /// 物料保存前需要保存打印设置
    /// </summary>
    [System.ComponentModel.DisplayName("物料保存前需要保存打印设置")]
    [System.ComponentModel.Description("物料保存前需要保存打印设置")]
    public class LabelPrintTemplateSubmitting : OnSubmitting<Item>
    {
        /// <summary>
        /// 保存物料前执行
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(Item entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                Item item = entity;
                LabelPrintTemplate labelPrintTemplate = entity.GetProperty(LabelPrintDetailProperty.LabelPrintTemProperty);
                if ((entity.TemplateId == null) || (entity.TemplateId == 0 && labelPrintTemplate == null))
                {
                    labelPrintTemplate = new LabelPrintTemplate();
                    RF.Save(labelPrintTemplate);
                    labelPrintTemplate.MarkSaved();
                    item.TemplateId = labelPrintTemplate.Id;
                    item.Template = labelPrintTemplate;
                }

                if (labelPrintTemplate != null && labelPrintTemplate.PersistenceStatus != PersistenceStatus.Unchanged)
                {
                    RF.Save(labelPrintTemplate);
                    labelPrintTemplate.MarkSaved();
                    entity.TemplateId = labelPrintTemplate.Id;
                }
            }
        }
    }
}
