using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipModels;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Equipments.EquipTypes
{
    #region 设备类型删除验证规则
    /// <summary>
    /// 关联设备模型不能删除--设备类型
    /// </summary>
    [DisplayName("设备类型删除规则")]
    [Description("关联设备型号不能删除")]
    public class ProductModelNoReferenceRule : NoReferencedRule<EquipType>
    {
        /// <summary>
        /// 构造函数添加验证属性
        /// </summary>
        public ProductModelNoReferenceRule()
        {
            Properties.Add(EquipModel.EquipTypeIdProperty);
            MessageBuilder = (o, e) =>
            {
                var equipType = o as EquipType;
                return "设备类型".L10N()+"[{0}]".L10nFormat(equipType.TypeName) + "已关联设备型号，不允许删除".L10N();
            };
        }
    }
    #endregion    

    #region 设备类型编码非重复验证规则
    ///// <summary>
    ///// 设备类型编码非重复验证规则
    ///// </summary>
    //[DisplayName("设备类型编码非重复验证规则")]
    //[Description("编码、类别不能重复")]
    //class EquipTypeCodeNotDuplicateRule : NotDuplicateRule<EquipType>
    //{
    //    /// <summary>
    //    /// 不重复规则
    //    /// </summary>
    //    public EquipTypeCodeNotDuplicateRule()
    //    {
    //        Properties.Add(EquipType.TypeCodeProperty);
    //        Properties.Add(EquipType.TypeCategoryProperty);
    //        Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
    //        MessageBuilder = e =>
    //        {
    //            var entity = e as EquipType;
    //            var typeCategory = RT.Service.Resolve<CatalogController>().GetCatalog(EquipType.EquipTypeCatalogType, entity.TypeCategory);
    //            return string.Format("类型编码[{0}]在类别[{1}]已维护，不允许重复维护！".L10N(), entity.TypeCode, typeCategory.Name);
    //        };
    //    }
    //}
    #endregion

    #region 设备编码非重复验证规则
    /// <summary>
    /// 设备编码非重复验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("类型编码非重复验证规则")]
    [System.ComponentModel.Description("类型编码不能重复")]
    public class EquipTypeCodeNotDuplicateByIsCheckRule : EntityRule<EquipType>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">设备型号维护实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var equiptype = entity as EquipType;
            var controller = RT.Service.Resolve<CoreEquipController>();
            var result = controller.GetEquipTypeList(equiptype.TypeCode, equiptype.Id);
            if (result.Count >= 1)
                e.BrokenDescription = "类型编码".L10N()+"[{0}]".L10nFormat(equiptype.TypeCode) + "不能重复".L10N()+"\n";
        }
    }
    #endregion

    #region 设备类型名称非重复验证规则
    ///// <summary>
    ///// 设备类型名称非重复验证规则
    ///// </summary>
    //[DisplayName("设备类型名称非重复验证规则")]
    //[Description("名称、类别不能重复")]
    //class EquipTypeNameNotDuplicateRule : NotDuplicateRule<EquipType>
    //{
    //    /// <summary>
    //    /// 不重复规则
    //    /// </summary>
    //    public EquipTypeNameNotDuplicateRule()
    //    {
    //        Properties.Add(EquipType.TypeNameProperty);
    //        Properties.Add(EquipType.TypeCategoryProperty);
    //        Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
    //        MessageBuilder = e =>
    //        {
    //            var entity = e as EquipType;
    //            var typeCategory= RT.Service.Resolve<CatalogController>().GetCatalog(EquipType.EquipTypeCatalogType, entity.TypeCategory);
    //            return string.Format("类型名称[{0}]在类别[{1}]已维护，不允许重复维护！".L10N(), entity.TypeName, typeCategory.Name);
    //        };
    //    }
    //}
    #endregion
}
