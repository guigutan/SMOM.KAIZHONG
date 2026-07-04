using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.LES
{
    /// <summary>
    /// 周转规则明细行号不重复
    /// </summary>
    [DisplayName("启用扩展属性必须填写")]
    [Description("启用扩展属性必须填写")]
    public class PrepareItemSubmitting : OnSubmitting<BasePrepareItem>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(BasePrepareItem entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                if (entity.Item != null && entity.Item.EnableExtendProperty&&entity.ItemExtProp.IsNullOrEmpty())
                {
                    throw new ValidationException("物料启用扩展属性,必须填写!".L10N());
                }
                
            }
        }
    }
}
