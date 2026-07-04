using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.Items;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Items
{
    /// <summary>
    /// 分类层级删除验证规则
    /// 分类层级存在子不允许删除
    /// </summary>
    [DisplayName("分类层级删除验证规则")]
    [Description("分类层级存在子不允许删除")]
    class CategoryLevelDeleteRule : EntityRule<ItemCategoryLevel>
    {
        /// <summary>
        /// 分类层级删除验证规则
        /// </summary>
        public CategoryLevelDeleteRule()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 分类层级存在子不允许删除
        /// </summary>
        /// <param name="entity">分类层级</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var level = entity as ItemCategoryLevel;
            if (RT.Service.Resolve<ItemController>().HasChildLevel(level.Id))
            {
                e.BrokenDescription = "不允许删除，[{0}]存在子分类层级".L10nFormat(level.Code);
            }
        }
    }

    /// <summary>
    /// 分类层级有关联分类时不允许删除验证
    /// </summary>
    [DisplayName("分类层级删除验证规则")]
    [Description("分类层级有关联分类时不允许删除验证")]
    public class UnDeleteCategoryLevelHasCategory : EntityRule<ItemCategoryLevel>
    {
        /// <summary>
        /// 分类层级删除验证规则
        /// </summary>
        public UnDeleteCategoryLevelHasCategory()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 分类层级有关联分类时不允许删除验证
        /// </summary>
        /// <param name="entity">分类层级</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var level = entity as ItemCategoryLevel;
            if (RT.Service.Resolve<ItemController>().LevelHasCategory(level.Id))
            {
                e.BrokenDescription = "不允许删除,分类层级[{0}]有关联分类".L10nFormat(level.Code);
            }
        }
    }

    /// <summary>
    /// 不允许添加相同类型的分类层级树
    /// </summary>
    [DisplayName("分类层级添加验证规则")]
    [Description("不允许添加相同类型的分类层级树")]
    public class CategoryLevelAddRule : EntityRule<ItemCategoryLevel>
    {
        /// <summary>
        /// 分类层级添加验证规则
        /// </summary>
        public CategoryLevelAddRule()
        {
            Scope = EntityStatusScopes.Add;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证是否存在相同类型的分类层级树
        /// </summary>
        /// <param name="entity">分类层级</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var level = entity as ItemCategoryLevel;
            if (level != null && level.TreePId == null &&
                RT.Service.Resolve<ItemController>().IsExistLevelType(level.Type))  ////添加根节点时验证
            {
                e.BrokenDescription = "已存在类型为[{0}]的分类层级".L10nFormat(Utils.EnumViewModel.EnumToLabel(level.Type).L10N());
            }
        }
    }

    /// <summary>
    /// 分类层级添加修改验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("分类层级类型添加修改验证规则")]
    [System.ComponentModel.Description("分类层级类型必须与上级层级类型相同")]
    public class ItemCategoryLevelAdd : EntityRule<ItemCategoryLevel>
    {
        /// <summary>
        /// 分类层级类型添加修改验证规则
        /// </summary>
        public ItemCategoryLevelAdd()
        {
            Scope = EntityStatusScopes.Add;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 分类层级类型必须与上级层级类型相同
        /// </summary>
        /// <param name="entity">分类层级</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var categoryLevel = entity as ItemCategoryLevel;
            if (categoryLevel != null && categoryLevel.TreePId != null &&
                RF.GetById<ItemCategoryLevel>(categoryLevel.TreePId)?.Type != categoryLevel.Type)
            {
                e.BrokenDescription = "分类层级[{0}]的类型必须与上级分类层级类型相同".L10nFormat(categoryLevel.Code);
            }

        }
    }

    /// <summary>
    /// 分类层级修改验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("分类层级类型修改验证规则")]
    [System.ComponentModel.Description("分类层级存在多层时不允许修改类型")]
    public class ItemCategoryLevelUpdate : EntityRule<ItemCategoryLevel>
    {
        /// <summary>
        /// 分类层级类型修改验证规则
        /// </summary>
        public ItemCategoryLevelUpdate()
        {
            Scope = EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 分类层级存在多层时不允许修改类型
        /// </summary>
        /// <param name="entity">分类层级</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var categoryLevel = entity as ItemCategoryLevel;
            if (categoryLevel != null && categoryLevel.TreePId != null &&
                RF.GetById<ItemCategoryLevel>(categoryLevel.TreePId)?.Type != categoryLevel.Type)
            {
                e.BrokenDescription = "分类层级[{0}]存在上级层级不允许修改类型".L10nFormat(categoryLevel.Code);
            }

            if (categoryLevel != null && RT.Service.Resolve<ItemController>().HasChildLevel(categoryLevel.Id))
            {
                var categoryLevelList = RT.Service.Resolve<ItemController>().GetChildItemCategoryLevel(categoryLevel.Id);
                if (categoryLevelList != null && categoryLevelList.Count > 0 && categoryLevelList[0].Type != categoryLevel.Type)
                    e.BrokenDescription = "分类层级[{0}]存在子层级不允许修改类型".L10nFormat(categoryLevel.Code);
            }
        }
    }

    /// <summary>
    /// 父子分类物料类型必须相同
    /// </summary>
    [System.ComponentModel.DisplayName("分类验证规则")]
    [System.ComponentModel.Description("父子分类物料类型必须相同")]
    public class ItemCategoryItemTypeNotDuplicateRule : EntityRule<ItemCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemCategoryItemTypeNotDuplicateRule()
        {
            ConnectToDataSource = true;
            Scope = EntityStatusScopes.Update | EntityStatusScopes.Add;
        }

        /// <summary>
        /// 父子分类物料类型必须相同
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {

            ItemCategory itemCategory = entity as ItemCategory;
            var ctl = RT.Service.Resolve<ItemController>();
            if (itemCategory.ItemType.HasValue)
            {
                var result = ctl.IsExistNotSameItemType(itemCategory);
                var ParentResult = ctl.ParentIsExistNotSameItemType(itemCategory);
                if (result || ParentResult)
                {
                    e.BrokenDescription = "【{0}】的物料类型【{1}】与父分类不相同，请修改物料类型".L10nFormat(itemCategory.Name, itemCategory.ItemType.ToLabel());
                }
            }
        }
    }

    /// <summary>
    /// 分类删除验证规则
    /// 分类存在子不允许删除
    /// </summary>
    [DisplayName("分类删除验证规则")]
    [Description("分类存在子不允许删除")]
    class CategoryDeleteRule : EntityRule<ItemCategory>
    {
        /// <summary>
        /// 分类层级删除验证规则
        /// </summary>
        public CategoryDeleteRule()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 分类存在子不允许删除
        /// </summary>
        /// <param name="entity">分类</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var category = entity as ItemCategory;
            if (RT.Service.Resolve<ItemController>().HasChildCategory(category.Id))
            {
                e.BrokenDescription = "不允许删除，[{0}]存在子分类".L10nFormat(category.Code);
            }
        }
    }

    /// <summary>
    /// 分类添加修改验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("分类类型添加修改验证规则")]
    [System.ComponentModel.Description("分类类型必须与上级分类类型相同")]
    public class ItemCategoryAdd : EntityRule<ItemCategory>
    {
        /// <summary>
        /// 分类添加修改验证规则
        /// </summary>
        public ItemCategoryAdd()
        {
            Scope = EntityStatusScopes.Add;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 分类类型必须与上级分类类型相同
        /// </summary>
        /// <param name="entity">分类</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var category = entity as ItemCategory;
            if (category != null && category.TreePId != null &&
                RF.GetById<ItemCategory>(category.TreePId)?.Type != category.Type)
            {
                e.BrokenDescription = "分类[{0}]的类型必须与上级分类类型相同".L10nFormat(category.Code);
            }

        }
    }

    /// <summary>
    /// 分类修改验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("分类类型修改验证规则")]
    [System.ComponentModel.Description("分类存在多层时不允许修改类型")]
    public class ItemCategoryUpdate : EntityRule<ItemCategory>
    {
        /// <summary>
        /// 分类类型修改验证规则
        /// </summary>
        public ItemCategoryUpdate()
        {
            Scope = EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 分类存在多层时不允许修改类型
        /// </summary>
        /// <param name="entity">分类</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var category = entity as ItemCategory;
            if (category != null && category.TreePId != null &&
                RF.GetById<ItemCategory>(category.TreePId)?.Type != category.Type)
            {
                e.BrokenDescription = "分类[{0}]存在上级分类不允许修改类型".L10nFormat(category.Code);
            }

            if (category != null && RT.Service.Resolve<ItemController>().HasChildCategory(category.Id))
            {
                var categoryList = RT.Service.Resolve<ItemController>().GetChildItemCategory(category.Id);
                if (categoryList != null && categoryList.Count > 0 && categoryList[0].Type != category.Type)
                    e.BrokenDescription = "分类[{0}]存在子层级不允许修改类型".L10nFormat(category.Code);
            }
        }
    }

    /// <summary>
    /// 物料分类关系存在引用不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("NoReferencedRule验证规则")]
    [System.ComponentModel.Description("物料分类关系存在引用不能删除")]
    public class ItemCategoryReferencedRule : NoReferencedRule<ItemCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemCategoryReferencedRule()
        {
            Properties.Add(ItemCategoryRelation.ItemCategoryIdProperty);
            MessageBuilder = (o, e) =>
            {
                var itemCategory = o as ItemCategory;
                return "分类[{0}]已经被[{1}]引用，不能删除".L10nFormat(itemCategory.Code, "物料".L10N());
            };
        }
    }

    /// <summary>
    /// 物料类型必填
    /// </summary>
    [System.ComponentModel.DisplayName("物料类型必填")]
    [System.ComponentModel.Description("物料类型必填")]
    public class ItemTypeNotNullRule : EntityRule<ItemCategory>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as ItemCategory;
            if (pro.Type != CategoryType.Item && !pro.ItemType.HasValue)
            {
                e.BrokenDescription = "物料类型不能为空！".L10N();
            }
        }
    }
}
