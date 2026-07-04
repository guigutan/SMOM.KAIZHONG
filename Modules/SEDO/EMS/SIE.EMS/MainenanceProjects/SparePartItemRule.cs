using SIE.Domain.Validation;
using System;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
    /// 点检保养项目备件清单验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("点检保养项目备件清单验证规则")]
    [System.ComponentModel.Description("点检保养项目备件清单不能重复")]
    public class NotDuplicateSparePartItem : NotDuplicateRule<SparePartItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateSparePartItem()
        {
            Properties.Add(SparePartItem.ProjectDetailIdProperty);
            Properties.Add(SparePartItem.SparePartIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as SparePartItem;
                return "已存在项目名称[{0}]和备件[{1}]相同的备件清单".L10nFormat(entity.ProjectDetail.Name, entity.SparePart.SparePartName);
            };
        }
    }
   
}
