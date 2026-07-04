using SIE.Domain.Validation;
using SIE.Resources.WipResources;
using System;
using System.ComponentModel;

namespace SIE.Items.ProductModels
{
    #region 产线产能验证规则
    /// <summary>
    /// 产品机型被产线产能引用时不允许删除
    /// </summary>
    [DisplayName("产品机型验证规则")]
    [Description("产品机型被产线产能引用时不允许删除")]
    public class ProductModelRefByLineCapacity : NoReferencedRule<ProductModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductModelRefByLineCapacity()
        {
            Properties.Add(ProductModelLineCapacity.ProductModelIdProperty);
            MessageBuilder = (e, c) =>
            {
                return "产品机型[{0}]被产线产能引用[{1}]次不能删除".L10nFormat((e as ProductModel)?.Name, c);
            };
        }
    }
    #endregion

    #region 机型技能验证规则
    /// <summary>
    /// 产品机型被机型技能引用时不允许删除
    /// </summary>
    [DisplayName("产品机型验证规则")]
    [Description("产品机型被机型技能引用时不允许删除")]
    public class ProductModelRefBySkill : NoReferencedRule<ProductModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductModelRefBySkill()
        {
            Properties.Add(ProductModelSkill.ModelIdProperty);
            MessageBuilder = (e, c) =>
            {
                return "产品机型[{0}]被机型技能引用[{1}]次不能删除".L10nFormat((e as ProductModel)?.Name, c);
            };
        }
    }

    /// <summary>
    /// 生产资源被产线产能引用时不允许删除
    /// </summary>
    [DisplayName("生产资源删除验证规则")]
    [Description("生产资源被产线产能引用时不允许删除")]
    public class WipResourceRefByProductModel : NoReferencedRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceRefByProductModel()
        {
            Properties.Add(ProductModelLineCapacity.ResourceIdProperty);
            MessageBuilder = (e, c) =>
            {
                return "生产资源[{0}]被产线产能引用[{1}]次不能删除".L10nFormat((e as WipResource)?.Name, c);
            };
        }
    }
    #endregion
}