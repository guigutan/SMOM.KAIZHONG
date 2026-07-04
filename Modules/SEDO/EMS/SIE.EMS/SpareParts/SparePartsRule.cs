using SIE.Domain.Validation;
using SIE.EMS.Equipments.Boms;
using SIE.EMS.MainenanceProjects;
using System;

namespace SIE.EMS.SpareParts
{

    #region 备件基础数据存在设备BOM备件时不能删除
    /// <summary>
    /// 备件基础数据存在设备BOM备件时不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("NoReferencedRule验证规则")]
    [System.ComponentModel.Description("备件基础数据存在设备BOM备件时不能删除")]
    public class SparePartsRule : NoReferencedRule<SparePart>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SparePartsRule()
        {
            Properties.Add(EquipBomDetail.SparePartIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，备件基础数据被设备BOM-备件引用".L10nFormat();
            };
        }
    }
    #endregion

    /// <summary>
    /// 校验规则
    /// </summary>
    [System.ComponentModel.DisplayName("备料基础资料校验验证规则")]
    [System.ComponentModel.Description("备料基础资料校验验证规则")]
    public class SparePartCheckRule : EntityRule<SparePart>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public SparePartCheckRule()
        {
            Scope = MetaModel.EntityStatusScopes.Add | MetaModel.EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(Domain.IEntity entity, MetaModel.RuleArgs e)
        {
            var detail = entity as SparePart;
            if (!detail.ItemCategoryId.HasValue)
            {
                e.BrokenDescription = detail.SparePartItem==null? "备件[{0}]未维护[{1}]类型的分类层级".L10nFormat(detail.SparePartCode, detail.SpartType.ToLabel()) : "物料[{0}]未维护[{1}]类型的分类层级".L10nFormat(detail.SparePartItem.Code, detail.SpartType.ToLabel());
            }
        }
    }
}
