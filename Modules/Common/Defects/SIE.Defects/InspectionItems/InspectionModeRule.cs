using SIE.Domain.Validation;
using System;

namespace SIE.Defects.InspectionItems
{
    /// <summary>
    /// 检验方式非重验证
    /// </summary>
    [System.ComponentModel.DisplayName("检验方式非重验证")]
    [System.ComponentModel.Description("相同检验类型名称不能重复")]
    public class InspectionModeNameNotDuplicateRule : NotDuplicateRule<InspectionMode>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InspectionModeNameNotDuplicateRule()
        {
            Properties.Add(InspectionMode.NameProperty);
            MessageBuilder = (e) => { return "名称不能重复".L10N(); };
        }
    }
}
