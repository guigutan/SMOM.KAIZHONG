using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.Packages.Packages
{
    #region 包装单位验证规则
    /// <summary>
    /// 关联包装单位不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("包装单位验证规则")]
    [System.ComponentModel.Description("关联包装单位不能删除")]
    public class PackingUnitNoReferencedRule : NoReferencedRule<PackingUnit>
    {
        /// <summary>
        /// 构造函数添加验证属性
        /// </summary>
        public PackingUnitNoReferencedRule()
        {
            Properties.Add(PackageRuleDetail.PackageUnitIdProperty);
        }
    }

    /// <summary>
    /// 不允许添加多个主单位
    /// </summary>
    [System.ComponentModel.DisplayName("包装单位验证规则")]
    [System.ComponentModel.Description("不允许添加多个主单位")]
    public class PackingUnitAddMasterUnit : EntityRule<PackingUnit>
    {
        /// <summary>
        /// 包装单位添加验证规则
        /// </summary>
        public PackingUnitAddMasterUnit()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证是否已存在主单位
        /// </summary>
        /// <param name="entity">包装单位</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var packingUnit = entity as PackingUnit;
            if (packingUnit != null && packingUnit.IsMasterUnit &&
                RT.Service.Resolve<PackageController>().IsExistsMasterUnit(packingUnit.Id))
            {
                e.BrokenDescription = "已存在主单位，不允许重复添加".L10N();
                return;
            }
            if (packingUnit != null && !packingUnit.IsMasterUnit && packingUnit.PackageUnitType == PackageUnitType.MasterUnit)
            {
                e.BrokenDescription = "非主单位不能设置包装类型是主单位".L10N();
            }
        }
    }

    /// <summary>
    /// 关联包装规则不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("包装规则验证规则")]
    [System.ComponentModel.Description("关联包装规则不能删除")]
    public class PackageRuleNoReferencedRule : NoReferencedRule<PackageRule>
    {
        /// <summary>
        /// 构造函数添加验证属性
        /// </summary>
        public PackageRuleNoReferencedRule()
        {
            Properties.Add(ItemPackageRule.PackageRuleIdProperty);
        }
    }
    #endregion
}
