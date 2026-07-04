using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.Items
{
    #region 物料验证
    ///// <summary>
    ///// 关联物料属性不能删除
    ///// </summary>
    //[DisplayName("NoReferencedRule验证规则")]
    //[Description("关联物料属性不能删除")]
    //public class UndeleteInvolveItemProperty : NoReferencedRule<Item>
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public UndeleteInvolveItemProperty()
    //    {
    //        Properties.Add(ItemPropertyValue.ItemIdProperty);
    //        MessageBuilder = (o, e) =>
    //        {
    //            var item = o as Item;
    //            return "物料[{0}]已经被[{1}]引用，不能删除".L10nFormat(item.Code, "物料属性");
    //        };
    //    }
    //}

    /// <summary>
    /// 关联产品BOM不能删除
    /// </summary>
    [DisplayName("物料删除验证规则")]
    [Description("关联产品BOM不能删除")]
    public class UndeleteInvolveBom : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteInvolveBom()
        {
            Properties.Add(ProductBom.ProductIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已经被[{1}]引用，不能删除".L10nFormat(item.Code, "产品BOM".L10N());
            };
        }
    }

    /// <summary>
    /// 关联产品BOM明细不能删除
    /// </summary>
    [DisplayName("物料删除验证规则")]
    [Description("关联产品BOM明细不能删除")]
    public class UndeleteInvolveBomDetail : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数，添加验证属性
        /// </summary>
        public UndeleteInvolveBomDetail()
        {
            Properties.Add(ProductBomDetail.ItemIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已经被[{1}]引用，不能删除".L10nFormat(item.Code, "产品BOM明细".L10N());
            };
        }
    }

    /// <summary>
    /// 长必须为正数
    /// </summary>
    [DisplayName("长必须为正数")]
    [Description("请输入大于 0 的数字")]
    public class ItemLengthRule : EntityRule<Item>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public ItemLengthRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (item.Length.HasValue && item.Length <= 0)
                e.BrokenDescription = "物料[{0}] - [设计资料]页签 - [长]不能少于0".L10nFormat(item.Code);
        }
    }

    /// <summary>
    /// 宽必须为正数
    /// </summary>
    [DisplayName("宽必须为正数")]
    [Description("请输入大于 0 的数字")]
    public class ItemWidthRule : EntityRule<Item>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public ItemWidthRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (item.Width.HasValue && item.Width <= 0)
                e.BrokenDescription = "物料[{0}] - [设计资料]页签 - [宽]不能少于0".L10nFormat(item.Code);
        }
    }

    /// <summary>
    /// 高必须为正数
    /// </summary>
    [DisplayName("高必须为正数")]
    [Description("请输入大于 0 的数字")]
    public class ItemHeightRule : EntityRule<Item>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public ItemHeightRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (item.Height.HasValue && item.Height <= 0)
                e.BrokenDescription = "物料[{0}] - [设计资料]页签 - [高]不能少于0".L10nFormat(item.Code);
        }
    }

    /// <summary>
    /// 单位体积必须为正数
    /// </summary>
    [System.ComponentModel.DisplayName("单位体积必须为正数")]
    [System.ComponentModel.Description("请输入大于 0 的数字")]
    public class ItemVolumeRule : EntityRule<Item>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public ItemVolumeRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (item.Volume.HasValue && item.Volume <= 0)
                e.BrokenDescription = "物料[{0}] - [设计资料]页签 - [体积]不能少于0".L10nFormat(item.Code);
        }
    }

    /// <summary>
    /// 单位净重必须为正数
    /// </summary>
    [System.ComponentModel.DisplayName("单位净重必须为正数")]
    [System.ComponentModel.Description("请输入大于 0 的数字")]
    public class ItemWeightRule : EntityRule<Item>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public ItemWeightRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (item.Weight.HasValue && item.Weight <= 0)
                e.BrokenDescription = "物料[{0}] - [设计资料]页签 - [单位净重]不能少于0".L10nFormat(item.Code);
        }
    }

    /// <summary>
    /// 启用扩展属性验证
    /// </summary>
    [DisplayName("启用扩展属性验证")]
    [Description("启用扩展属性验证")]
    public class EnableExtendPropertyRule : EntityRule<Item>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public EnableExtendPropertyRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (item.EnableExtendProperty && item.PropertyValueList.Count == 0)
                e.BrokenDescription = "物料【{0}】启用扩展属性列勾选时，必须维护扩展属性子列表".L10nFormat(item.Code);
        }
    }
    #endregion

    #region
    /// <summary>
    /// 物料启用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("物料删除验证规则")]
    [System.ComponentModel.Description("物料已启用不能删除")]
    public class UndeleteItemEnable : EntityRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteItemEnable()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (item.State == State.Enable)
            {
                e.BrokenDescription = "物料已启用不能删除".L10N();
            }
        }
    }
    #endregion

    #region 最小包装数必须大于零验证
    /// <summary>
    /// 最小包装数必须大于零
    /// </summary>
    [System.ComponentModel.DisplayName("最小包装数验证规则")]
    [System.ComponentModel.Description("最小包装数必须大于零")]
    public class MinPackingQty : EntityRule<Item>
    {
        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as Item;
            if (item.MinPackingQty <= 0)
            {
                e.BrokenDescription = "最小包装数必须大于0".L10N();
            }
        }
    }
    #endregion

    #region 物料属性定义验证

    /// <summary>
    /// 物料属性类型不是快码时，快码组为空
    /// </summary>
    [System.ComponentModel.DisplayName("物料属性定义验证规则")]
    [System.ComponentModel.Description("物料属性类型不是快码时，快码组为空")]
    public class SetNullNotCatalogType : EntityRule<ItemPropertyDefinition>
    {
        /// <summary>
        /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var itemPropertyDefine = entity as ItemPropertyDefinition;
            if (itemPropertyDefine.PropertyType != ItemPropertyType.Catalog)
                itemPropertyDefine.CatalogType = null;
        }
    }

    /// <summary>
    /// 关联物料属性不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("物料属性定义验证规则")]
    [System.ComponentModel.Description("关联物料属性不能删除")]
    public class UndeleteItemPropertyDefinitionInvolveItemProperty : NoReferencedRule<ItemPropertyDefinition>
    {
        /// <summary>
        /// 构造函数，添加验证属性
        /// </summary>
        public UndeleteItemPropertyDefinitionInvolveItemProperty()
        {
            Properties.Add(ItemPropertyValue.DefinitionIdProperty);
        }
    }

    /// <summary>
    /// 关联物料属性不能修改
    /// </summary>
    [System.ComponentModel.DisplayName("物料属性定义验证规则")]
    [System.ComponentModel.Description("关联物料属性不能修改")]
    public class UnupdateItemPropertyDefinitionInvolveItem : EntityRule<ItemPropertyDefinition>
    {
        /// <summary>
        /// 构造函数，添加验证属性
        /// </summary>
        public UnupdateItemPropertyDefinitionInvolveItem()
        {
            ConnectToDataSource = true;
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e.Rule.Meta.Scope == EntityStatusScopes.Update)
            {
                ItemPropertyDefinition itemPropertyDefine = entity as ItemPropertyDefinition;
                var ctl = RT.Service.Resolve<ItemController>();
                var result = ctl.GetItemPropertyValue(itemPropertyDefine.Id);
                if (result != null &&
                    result.Definition.PropertyType != itemPropertyDefine.PropertyType) //如果当前修改的物料类型与数据库物料类型相同，则不操作
                {
                    e.BrokenDescription = "【{0}】物料属性已被物料【{1}】关联，不能修改物料属性类型  ".L10nFormat(itemPropertyDefine.Name, result.Item.Name);
                }
            }
        }
    }

    /// <summary>
    /// 物料属性类型为快码时，快码组不能为空
    /// </summary>
    [System.ComponentModel.DisplayName("物料属性定义验证规则")]
    [System.ComponentModel.Description("物料属性类型为快码时，快码组不能为空")]
    public class CatalogTypeNotDuplicateRule : EntityRule<ItemPropertyDefinition>
    {
        /// <summary>
        /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            ItemPropertyDefinition itempropertyDefine = entity as ItemPropertyDefinition;
            if (itempropertyDefine.PropertyType == ItemPropertyType.Catalog && itempropertyDefine.CatalogType == null)
            {
                e.BrokenDescription = "类型为快码，快码组不能为空".L10N();
            }
        }
    }
    #endregion

    #region 物料属性值验证
    /// <summary>
    /// 物料属性的属性值不能相同
    /// </summary>
    [System.ComponentModel.DisplayName("物料属性值验证规则")]
    [System.ComponentModel.Description("属性值不能相同")]
    public class ItemPropertyIdAndValueNotDuplicateRule : NotDuplicateRule<ItemPropertyValue>
    {
        /// <summary>
        /// 构造函数，添加验证属性
        /// </summary>
        public ItemPropertyIdAndValueNotDuplicateRule()
        {
            Properties.Add(ItemPropertyValue.DefinitionIdProperty);
            Properties.Add(ItemPropertyValue.ValueProperty);
            Properties.Add(ItemPropertyValue.PropertyGroupProperty);
            Properties.Add(ItemPropertyValue.ItemIdProperty);
            MessageBuilder = (e) =>
            {
                var value = e as ItemPropertyValue;
                return "物料[{0}]已存在属性组[{1}]属性[{2}]值[{3}]".L10nFormat(value.Item?.Name, value.PropertyGroup,value.Definition?.Name, value.Value);
            };
        }
    }

    /// <summary>
    /// 验证一个属性组下不能存在多个物料属性
    /// </summary>
    [System.ComponentModel.DisplayName("物料属性值验证规则")]
    [System.ComponentModel.Description("验证一个属性组下不能存在多个物料属性")]
    public class ItemPropertyGroupNotDuplicateRule : EntityRule<ItemPropertyValue>
    {
        /// <summary>
        /// 构造函数，验证一个属性组下不能存在多个物料属性
        /// </summary>
        public ItemPropertyGroupNotDuplicateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var itemPropertyValue = entity as ItemPropertyValue;
            ItemPropertyDefinition definition = RT.Service.Resolve<ItemController>().GetDefinitionIdByPropertyGroup(itemPropertyValue.ItemId, itemPropertyValue.PropertyGroup);
            if (definition != null && definition.Id != itemPropertyValue.DefinitionId)
            {
                if(itemPropertyValue.PropertyGroup.IsNotEmpty())
                    e.BrokenDescription = "属性组[{0}]已经录入物料属性[{1}],不允许录入其他的物料属性".L10nFormat(itemPropertyValue.PropertyGroup, definition.Name);
            }
        }
    }

    /// <summary>
    /// 验证相同属性和属性值时，不能同时存在空与非空属性组
    /// </summary>
    [System.ComponentModel.DisplayName("空与非空属性组不共存验证规则")]
    [System.ComponentModel.Description("验证相同属性和属性值时不能同时存在空与非空属性组")]
    public class ItemPropertyGroupNullOrNotNull : EntityRule<ItemPropertyValue>
    {
        /// <summary>
        /// 构造函数，验证相同属性和属性值时，不能同时存在空与非空属性组
        /// </summary>
        public ItemPropertyGroupNullOrNotNull()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var itemPropertyValue = entity as ItemPropertyValue;
            List<ItemPropertyValue> values = RT.Service.Resolve<ItemController>().GetItemPropertyList(new List<double>() { itemPropertyValue.ItemId }).Where(p=>p.DefinitionId==itemPropertyValue.DefinitionId&&p.Value==itemPropertyValue.Value).ToList() ;
            if (values.Count > 0)
            {
                if (itemPropertyValue.PropertyGroup.IsNullOrEmpty())
                {
                    e.BrokenDescription = "属性[{0}]值[{1}],不能同时存在空与非空的属性组".L10nFormat(itemPropertyValue.Definition.Name, itemPropertyValue.Value);
                }
                else
                {
                    foreach(var value in values)
                    {
                        if(value.PropertyGroup.IsNullOrEmpty())
                            e.BrokenDescription = "属性[{0}]值[{1}],不能同时存在空与非空的属性组".L10nFormat(itemPropertyValue.Definition.Name, itemPropertyValue.Value);
                    }
                }
            }
        }
    }

    #endregion

    #region 转换单位验证
    /// <summary>
    /// 转换单位不重复验证
    /// </summary>
    [System.ComponentModel.DisplayName("辅助单位不重复验证")]
    [System.ComponentModel.Description("辅助单位不允许重复")]
    public class ItemUnitRule : NotDuplicateRule<ItemUnit>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemUnitRule()
        {
            Properties.Add(ItemUnit.ItemIdProperty);
            Properties.Add(ItemUnit.MainUnitIdProperty);
            Properties.Add(ItemUnit.UnitIdProperty);
            MessageBuilder = (e) =>
            {
                return "该物料已存在相同的辅助单位".L10N();
            };
        }
    }

    /// <summary>
    /// 关联物料不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("单位删除规则")]
    [System.ComponentModel.Description("关联单位不能删除")]
    public class UnitNoReferenceRule : NoReferencedRule<Unit>
    {
        /// <summary>
        /// 构造函数添加验证属性
        /// </summary>
        public UnitNoReferenceRule()
        {
            Properties.Add(Item.UnitIdProperty);
        }
    }

    /// <summary>
    /// 关联转换单位不能删除
    /// </summary>
    [DisplayName("单位删除规则")]
    [Description("关联转换单位不能删除")]

    public class UnitNoReferenceRoutingRule : NoReferencedRule<Unit>
    {
        /// <summary>
        /// 构造函数，添加验证属性
        /// </summary>
        public UnitNoReferenceRoutingRule()
        {
            Properties.Add(ItemUnit.UnitIdProperty);
        }
    }

    #endregion

    #region 产品机型验证
    /// <summary>
    /// 关联产品机型不能删除--物料
    /// </summary>
    [System.ComponentModel.DisplayName("产品机型删除规则")]
    [System.ComponentModel.Description("关联产品机型不能删除")]
    public class ProductModelNoReferenceRule : NoReferencedRule<ProductModel>
    {
        /// <summary>
        /// 构造函数添加验证属性
        /// </summary>
        public ProductModelNoReferenceRule()
        {
            Properties.Add(Item.ModelIdProperty);
            MessageBuilder = (o, e) =>
            {
                var productModel = o as ProductModel;
                return "产品机型[{0}]已关联物料，不允许删除".L10nFormat(productModel.Name);
            };
        }
    }

    /// <summary>
    /// 关联产品机型不能删除--产品族
    /// </summary>
    [System.ComponentModel.DisplayName("产品机型删除实体验证规则")]
    [System.ComponentModel.Description("关联产品机型不能删除")]
    public class ProductModelDeleteRule : EntityRule<ProductModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductModelDeleteRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var productModel = entity as ProductModel;
            if (productModel.ProductFamily != null)
            {
                e.BrokenDescription = "产品机型[{0}]已关联产品族，不允许删除".L10nFormat(productModel.Name);
            }
        }
    }
    #endregion

    #region 计量单位不能为空
    /// <summary>
    /// 计量单位不能为空
    /// </summary>
    [System.ComponentModel.DisplayName("计量单位不能为空")]
    [System.ComponentModel.Description("计量单位不能为空")]
    public class RequiredUnit : RequireRule<Item>
    {
        /// <summary>
        /// 选择实体适用规则
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return Item.UnitIdProperty;
            }
        }
    }
    #endregion
}
