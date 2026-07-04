using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Web.Items._Extentions_;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.ProductBoms
{
    /// <summary>
    /// 组合替代 视图对象
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class CombinationReplateViewConfig : WebViewConfig<CombinationReplate>
    {
        #region 物料属性值 ItemPropertyValue
        /// <summary>
        /// 物料属性值
        /// </summary>
        [Label("物料属性值")]
        internal static readonly Property<string> ItemPropertyValueProperty = P<CombinationReplate>.RegisterExtensionReadOnly("ItemPropertyValueProperty", typeof(CombinationReplateViewConfig),
            GetItemPropertyValueProperty, CombinationReplate.PropertyValueListProperty);

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="comReplate">产品BOM明细</param>
        /// <returns>string</returns>
        internal static string GetItemPropertyValueProperty(CombinationReplate comReplate)
        {
            var groups = comReplate.PropertyValueList.GroupBy(p => p.Definition.Name).ToList();
            string[] result = new string[groups.Count];
            for (int i = 0; i < groups.Count; i++)
            {
                var values = groups[i].Select(p => p.Value);
                result[i] = groups[i].Key + "：" + string.Join("、", values);
            }

            return string.Join("；", result);
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.MainMaterialId).UseBomMainItemEditor().UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.MainMaterialName), nameof(e.MainMaterial.Name));
                m.DicLinkField = keyValues;
            }).UseListSetting(e => { e.HelpInfo = "显示物料清单中物料的数据"; }).ShowInList(150);
            View.Property(p => p.MainMaterialName).Readonly();
            View.Property(p => p.ItemId).UseBomReplateItemEditor().UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                keyValues.Add(nameof(e.ItemSpecificationModel), nameof(e.Item.SpecificationModel));
                m.DicLinkField = keyValues;
            }).UseListSetting(e => { e.HelpInfo = "显示物料清单中的替代料数据"; }).ShowInList(150);
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.ItemSpecificationModel).Readonly();
            View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
            View.Property(p=>p.PropertyValueStr).Readonly(false).HasLabel("物料属性")
                .UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.Items.Editors.ComReplatePropertyValueEditor"; p.Editable = false; p.IsReadonly = false; })
                .ShowInList(250);
            View.Property(p => p.ReplateGroup);
        }
    }
}