using SIE.Domain.Validation;
using SIE.Items;
using System;
using System.ComponentModel;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 关联关键物料不能删除
    /// </summary>
    [DisplayName("物料删除验证规则")]
    [Description("关联关键物料不能删除")]
    public class ItemNoReferenceRoutingRule : NoReferencedRule<Item>
    {
        /// <summary>
        /// 关联关键物料不能删除
        /// </summary>
        public ItemNoReferenceRoutingRule()
        {
            Properties.Add(KeyItem.ItemIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已经被[{1}]引用，不能删除".L10nFormat(item.Code, "关键物料".L10N());
            };
        }
    }
}
