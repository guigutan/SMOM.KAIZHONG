using SIE.Common.Prints;
using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using SIE.Web.Items.Common;
using SIE.Web.Items.Editors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items._Extentions_
{
    /// <summary>
    /// 实体属性视图元数据扩展
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 使用质量分类叶子结点类编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseQualityCategoryLeafNodesEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                List<ItemType> itemTypeList = new List<ItemType>() { ItemType.Material, ItemType.SemiFinished, ItemType.Product };
                List<int> itemTypeValueList = itemTypeList.Select(p => (int)p).ToList();
                return RT.Service.Resolve<SIE.Items.ItemController>().GetLeafNodes(itemTypeValueList, pagingInfo, keyword);
            });
            return meta;
        }

        /// <summary>
        /// 使用物料（成品、半成品，状态过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseProductCombinationEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                List<ItemType> itemTypeList = new List<ItemType>() { ItemType.SemiFinished, ItemType.Product };
                List<int> itemTypeValueList = itemTypeList.Select(p => (int)p).ToList();
                return RT.Service.Resolve<SIE.Items.ItemController>().GetItemsFormType(itemTypeValueList, State.Enable, keyword, pagingInfo);
            });
            return meta;
        }

        /// <summary>
        /// 使用物料（成品、半成品，状态不过滤）编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseNoFilterProductCombinationEditor<T>(
            this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                List<ItemType> itemTypeList = new List<ItemType>() { ItemType.Product, ItemType.SemiFinished };
                List<int> itemTypeValueList = itemTypeList.Select(p => (int)p).ToList();
                return RT.Service.Resolve<SIE.Items.ItemController>().GetItemsFormType(itemTypeValueList, null, keyword, pagingInfo);
            });
            return meta;
        }


        /// <summary>
        /// 打印模板配置-标签模板级联编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseLabelPrintTemplateEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            return GetMeta(meta, action);
        }

        /// <summary>
        /// 打印模板配置-包装模板级联编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseLabelPackingTemplateEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            return GetMeta(meta, action);
        }

        /// <summary>
        /// 获取编码规则下的打印模板
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>打印模板列表</returns>
        private static WebEntityPropertyViewMeta<T> GetMeta<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var template = source as SIE.Core.Items.LabelPrintTemplate;
                if (template == null || template.NumberRule == null)
                    return new EntityList<PrintTemplate>();
                List<double> templateIds = template.NumberRule.TemplateList.Where(x => x.TemplateId != null).Select(x => x.TemplateId.Value).ToList();
                return RT.Service.Resolve<ItemController>().GetRulePrintTemplates(template.NumberRule.Id, templateIds, pagingInfo);
            }).UsePagingLookUpEditor(action);

            return meta;
        }

        /// <summary>
        /// 使用物料扩展属性编辑器
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="meta">实体属性视图元数据 <see cref="WebEntityPropertyViewMeta"/></param>
        /// <param name="action">委托</param>
        /// <returns>实体属性视图元数据</returns>
        ///  <example> View.Property(p => p.Text).UseTextButtonFieldEditor();</example>
        public static WebEntityPropertyViewMeta<T> UseItemExtPropRecordsFieldEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<ItemExtPropRecordsFieldConfig> action = null)
        {
            var config = new ItemExtPropRecordsFieldConfig();
            config.ExtendJsObj = "SIE.Web.Items.Common.Editors.ItemExtPropSelectEditor";
            action?.Invoke(config);
            meta.ViewMeta.Config = config;
            return meta;
        }

        /// <summary>
        /// 使用物料单位属性编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseItemUnitEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<ItemUintFieldConfig> action = null)
        {
            var config = new ItemUintFieldConfig();
            config.ExtendJsObj = "SIE.Web.Items.Common.Editors.ItemUnitEditor";
            action?.Invoke(config);
            meta.ViewMeta.Config = config;
            return meta;
        }


        /// <summary>
        /// 使用产品BOM-物料清单主料编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseBomMainItemEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var combinationReplate = source as CombinationReplate;

                return RT.Service.Resolve<ProductBomController>().GetBomMainItemList(combinationReplate.ProductBomId, pagingInfo, keyword);
            });
            return meta;
        }

        /// <summary>
        /// 使用产品BOM-物料清单替代料编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseBomReplateItemEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var combinationReplate = source as CombinationReplate;
                if (combinationReplate.MainMaterial == null)
                    return new EntityList<Item>();
                return RT.Service.Resolve<ProductBomController>().GetBomReplateItemList(combinationReplate.MainMaterialId, combinationReplate.MainMaterial.Type, pagingInfo, keyword);
            });
            return meta;
        }

        /// <summary>
        /// 获取单据大类枚举编辑器
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="meta">meta</param>
        /// <param name="action">action</param>
        /// <returns>单据大类</returns>
        public static WebEntityPropertyViewMeta<T> UseSelectEnumBoxEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<SelectEnumBoxConfig> action = null)
        {
            meta.ViewMeta.EditorName = WebEditorNames.Enum;
            var config = new SelectEnumBoxConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }
    }
}
