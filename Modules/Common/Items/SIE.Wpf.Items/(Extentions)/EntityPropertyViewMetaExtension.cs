using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.Items.Editors;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 使用物料分类层级编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemCateLevelEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<ItemCateLevelEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemCateLevelEditor.EditorName;
            var config = new ItemCateLevelEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料分类小类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemSmallCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemSmallCategoryEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用所有类型质量分类小类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseAllTypesQualitySmallCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemQualitySmallCategoryEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            config.SetExtendedProperty(ItemQualitySmallCategoryEditor.TypeList, new List<ItemType>() { ItemType.Material, ItemType.SemiFinished, ItemType.Product });
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用原材料质量分类小类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseMaterialQualitySmallCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemQualitySmallCategoryEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            config.SetExtendedProperty(ItemQualitySmallCategoryEditor.TypeList, new List<ItemType>() { ItemType.Material });
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用半成品质量分类小类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseSemiFinishedQualitySmallCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemQualitySmallCategoryEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            config.SetExtendedProperty(ItemQualitySmallCategoryEditor.TypeList, new List<ItemType>() { ItemType.SemiFinished });
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用成品质量分类小类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseProductQualitySmallCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemQualitySmallCategoryEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            config.SetExtendedProperty(ItemQualitySmallCategoryEditor.TypeList, new List<ItemType>() { ItemType.Product });
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料，半成品质量分类小类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseMatOrSemiQualitySmallCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemQualitySmallCategoryEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            config.SetExtendedProperty(ItemQualitySmallCategoryEditor.TypeList, new List<ItemType>() { ItemType.Material, ItemType.SemiFinished });
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用半成品，成品质量分类小类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseSemiOrProdQualitySmallCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemQualitySmallCategoryEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            config.SetExtendedProperty(ItemQualitySmallCategoryEditor.TypeList, new List<ItemType>() { ItemType.SemiFinished, ItemType.Product });
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        ///// <summary>
        ///// 使用成品质量分类小类编辑器
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="meta">属性视图元数据</param>
        ///// <param name="action">委托</param>
        ///// <returns>泛型属性视图元数据</returns>
        //public static WPFEntityPropertyViewMeta<T> UseProductQualitySmallCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        //{
        //    meta.ViewMeta.EditorName = ProductQualitySmallCategoryEditor.EditorName;
        //    var config = new PagingLookUpEditorConfig();
        //    meta.ViewMeta.Config = config;
        //    action?.Invoke(config);
        //    return meta;
        //}

        /// <summary>
        /// 使用替代料编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseAlternativeEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<EditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = AlternativeEditor.EditorName;
            var config = new EditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料属性编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemPropertyEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<EditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemPropertyEditor.EditorName;
            var config = new EditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料属性快码编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemPropertyCatalogEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = CatalogLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 获取产品等级快码编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseProductGradeCatalogLookupEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = CatalogProductGradeLookupEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（原材料，状态不过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseNoFilterItemMaterialEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Material });
            ////config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            config.ReloadDataOnPopping = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（半成品，状态不过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseNoFilterItemSemiFinishedEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.SemiFinished });
            ////config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            config.ReloadDataOnPopping = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（成品，状态不过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseNoFilterItemProductEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Product });
            ////config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            config.ReloadDataOnPopping = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（成品、半成品，状态不过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseNoFilterProductCombinationEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Product, ItemType.SemiFinished });
            ////config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            config.ReloadDataOnPopping = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（原材料、半成品，状态不过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseNoFilterMaterialCombinationEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Material, ItemType.SemiFinished });
            ////config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            config.ReloadDataOnPopping = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料全部类型（状态不过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseNoFilterAllItemTypeEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Material, ItemType.SemiFinished, ItemType.Product });
            ////config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            config.ReloadDataOnPopping = true;
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（原材料，状态过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemMaterialEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Material });
            config.ReloadDataOnPopping = true;
            config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（半成品，状态过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemSemiFinishedEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.SemiFinished });
            config.ReloadDataOnPopping = true;
            config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（成品，状态过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemProductEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Product });
            config.ReloadDataOnPopping = true;
            config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（成品、半成品，状态过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseProductCombinationEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Product, ItemType.SemiFinished });
            config.ReloadDataOnPopping = true;
            config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料（原材料、半成品，状态过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseMaterialCombinationEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Material, ItemType.SemiFinished });
            config.ReloadDataOnPopping = true;
            config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料全部类型（状态过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseAllItemTypeEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemTypeEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(ItemTypeEditor.TypeList, new List<ItemType>() { ItemType.Material, ItemType.SemiFinished, ItemType.Product });
            config.ReloadDataOnPopping = true;
            config.SetExtendedProperty(ItemTypeEditor.State, State.Enable);
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用文本+单位编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseUnitInputEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<UnitInputEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = UnitInputEditor.EditorName;
            var config = new UnitInputEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料分类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemCategoryEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemCategoryLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用产品BOM明细物料属性值选择编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UsePBDPDefinitionLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = PBDPDefinitionLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用产品BOM物料属性值选择编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UsePBPDefinitionLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = PBPDefinitionLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用条码规则下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseNumberRuleLookupEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = NumberRuleLookupEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 标签模板下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseBarcodeModelLookupEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = BarcodeModelLookupEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 包装模板下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UsePackageModelLookupEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = PackageModelLookupEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用范围单位编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseRangeUnitInputEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<RangeUnitInputEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = RangeUnitInputEditor.EditorName;
            var config = new RangeUnitInputEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 采购组员工编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseWorkGroupEmployeeLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = WorkGroupEmployeeLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料属性定义编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UsePropertyValueEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<EditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = PropertyValueEditor.EditorName;
            var config = new EditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}