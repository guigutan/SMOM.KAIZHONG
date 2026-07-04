using SIE.Domain;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Packages.Packages
{
    /// <summary>
    /// 物料保存前需要保存物料批次规则
    /// </summary>
    [System.ComponentModel.DisplayName("物料保存后需要保存物料包装规则")]
    [System.ComponentModel.Description("物料保存后需要保存物料包装规则")]
    public class ItemSubmmited : OnSubmitted<Item>
    {
        /// <summary>
        /// 物料保存后需要保存物料包装规则
        /// </summary>
        /// <param name="entity">物料</param>
        /// <param name="e">e</param>
        protected override void Invoke(Item entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                var rule = RT.Service.Resolve<PackageController>().GetPackageRuleByCode("初始化默认包装规则");
                if (rule != null)
                    RT.Service.Resolve<PackageController>().CreateItemPackageRule(new List<PackageRule>() { rule }, entity.Id);
            }
        }
    }
}
