using SIE.Barcodes.Printables;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.ClientMetaModel;
using SIE.Web.Items.Common;
using SIE.Web.MES.BatchGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES
{
    /// <summary>
    /// 视图扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        ///获取打印模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseBatchPrintTemplateLookUpEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var model = source as BatchGeneratingViewModel;
                var templates = new EntityList<PrintTemplate>();
                if (model == null || model.NumberRule == null)
                {
                    return templates;
                }
                return RT.Service.Resolve<SIE.Barcodes.BarcodeController>().GetPrintTemplatesByRuleId(model.NumberRuleId.Value, typeof(WipBatchPrintable).GetQualifiedName(), pagingInfo, keyword);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 车间编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseResourceWorkShopEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                if (workshop == null || workshop.Count <= 0)
                    return new EntityList<Enterprise>();
                workshop.ForEach(p => p.TreePId = null);
                return workshop;
            }).UsePagingLookUpEditor(action);
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
                List<ItemType> itemTypeList = new List<ItemType>() { ItemType.Product, ItemType.SemiFinished };
                List<int> itemTypeValueList = itemTypeList.Select(p => (int)p).ToList();
                return RT.Service.Resolve<ItemController>().GetItemsFormType(itemTypeValueList, State.Enable, string.Format("{0}%", keyword), pagingInfo);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 使用条码规则下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseNumberRuleLookupEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<ComboListConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<NumberRuleController>().GetNumberRules(new List<int>() { (int)RuleType.Barcode }, keyword, pagingInfo);
            }).UsePagingLookUpOldEditor(action);
            return meta;
        }

        /// <summary>
        /// 标签模板下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseBarcodeModelLookupEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<ComboListConfig> action = null)
        {
            meta.UseDataSource((e, p, k) =>
            {
                var template = e as SIE.Core.Items.LabelPrintTemplate;
                var templates = new EntityList<PrintTemplate>();
                if (template == null || template.NumberRule == null)
                    return templates;
                template.NumberRule.TemplateList.Where(f => f.Template.State == State.Enable).Where(f => f.Template.EntityType == typeof(SIE.Barcodes.Printables.BarcodePrintable).GetQualifiedName() ||
                f.Template.EntityType == typeof(SIE.Barcodes.Printables.WipBatchPrintable).GetQualifiedName()).ForEach(f => templates.Add(f.Template)
                );
                return templates;
            }).UsePagingLookUpOldEditor(action);
            return meta;
        }

        /// <summary>
        /// 外包装模板下拉编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WebEntityPropertyViewMeta<T> UseBarcodeMoveModelLookupEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<ComboListConfig> action = null)
        {
            meta.UseDataSource((e, p, k) =>
            {
                var template = e as SIE.Core.Items.LabelPrintTemplate;
                var templates = new EntityList<PrintTemplate>();
                if (template == null || template.NumberRule == null)
                    return templates;
                template.NumberRule.TemplateList.Where(f => f.Template.State == State.Enable && f.Template.EntityType == typeof(SIE.MES.WIP.Moves.BarcodePrintable).GetQualifiedName()).ForEach(f => templates.Add(f.Template));
                return templates;
            }).UsePagingLookUpOldEditor(action);
            return meta;
        }

        /// <summary>
        /// 企业模型资源编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>web实体属性视图元数据（返回）</returns>
        public static WebEntityPropertyViewMeta<T> UseEnterpriseResourceEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig> action = null)
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, new List<SyncSourceType>() { SyncSourceType.Enterprise }, pagingInfo, keyword);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 单元格值导入前必填写验证执行函数
        /// </summary>
        /// <param name="meta"></param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static WebEntityPropertyViewMeta<T> BeforeImportRequireFunc<T>(this WebEntityPropertyViewMeta<T> meta, string fieldName)
        {
            meta.ViewMeta.BeforeImportFunc = (v) =>
            {
                var str = v.ToString();
                if (str.IsNullOrEmpty())
                {
                    throw new ValidationException("【{0}】必须填写".L10nFormat(fieldName));
                }

                return v;
            };

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
    }
}