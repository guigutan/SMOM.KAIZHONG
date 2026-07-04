using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts.Enums;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Items
{
    /// <summary>
    /// 物料扩展属性
    /// </summary>
    [Label("物料扩展属性")]
    [CompiledPropertyDeclarer]
    public static class ItemExtension
    {
        #region SpartType (类型)
        /// <summary>
        /// 类型 扩展属性。
        /// </summary>
        public static readonly Property<SparePartType?> SpartTypeProperty =
            P<Item>.RegisterExtension<SparePartType?>("SpartType", typeof(ItemExtension));

        /// <summary>
        /// 获取 类型 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static SparePartType? GetSpartType(this Item me)
        {
            return me.GetProperty(SpartTypeProperty);
        }

        /// <summary>
        /// 设置 类型 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetSpartType(this Item me, SparePartType? value)
        {
            me.SetProperty(SpartTypeProperty, value);
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class ItemExtensionConfig : EntityConfig<Item>
    {        
        /// <summary>
        /// 校验规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var para = o.CastTo<Item>();
                    SparePartType? spartType = para.GetProperty(ItemExtension.SpartTypeProperty);
                    if (para.Type == ItemType.SparePart && spartType == null)
                    {
                        e.BrokenDescription = "当【物料类型】为备件时，【备件类型】必填，请确认！".L10N();                        
                    }
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}
