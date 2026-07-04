using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Packages.Packages
{
    #region 物料必须存在默认包装规则验证规则
    /// <summary>
    /// 物料必须存在默认包装规则验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("物料必须存在默认包装规则验证规则")]
    [System.ComponentModel.Description("物料必须存在默认包装规则验证规则")]
    public class ItemPackageRuleDefaultRule : EntityRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemPackageRuleDefaultRule()
        {
            Scope = EntityStatusScopes.Update;
        }

        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            var rules = ItemPackageRuleDetailProperty.GetItemPackageRuleList(item);
            if (rules == null)
            {
                return;
            }
            if (!rules.DeletedList.Any(p => p.GetProperty(ItemPackageRule.IsDefaultProperty)))
            {
                return;
            }
            var hasDefaultflag = false;
            if (rules.Any())
            {
                if (!rules.Any(p => p.IsDefault))
                {
                    hasDefaultflag = true;
                }
            }
            else
            {
                var deleteRuleIds = rules.DeletedList.Select(p => (double)p.GetId()).ToList();
                var count = RT.Service.Resolve<PackageController>().GetItemPackageRuleCount(item.Id, deleteRuleIds);
                if (count > 0)
                {
                    hasDefaultflag = true;
                }
            }

            if (hasDefaultflag)
            {
                e.BrokenDescription = "物料必须存在默认包装规则".L10N();
            }
        }
    }
    #endregion
}
