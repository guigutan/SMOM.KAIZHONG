using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Utils;
using SIE.Web.Items._Extentions_;
using SIE.Web.Items.ProductBoms.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM明细视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ProductBomDetailViewConfig : WebViewConfig<ProductBomDetail>
    {
        #region 替代料值 AlternativeProperty
        /// <summary>
        /// 替代料值
        /// </summary>
        [Label("替代料值")]
        public static readonly Property<string> AlternativeProperty = P<ProductBomDetail>.RegisterExtensionReadOnly("Alternative", typeof(ProductBomDetailViewConfig),
            GetAlternative, ProductBomDetail.RemarkProperty);

        /// <summary>
        /// 替代料值
        /// </summary>
        /// <param name="detail">产品BOM明细</param>
        /// <returns>string</returns>
        public static string GetAlternative(ProductBomDetail detail)
        {
            string[] result = new string[detail.AlternativeList.Count];
            for (int i = 0; i < detail.AlternativeList.Count; i++)
            {
                result[i] = detail.AlternativeList[i].Item.Code;
            }

            return string.Join(";", result);
        }
        #endregion

        #region 物料属性值 ItemPropertyValue
        /// <summary>
        /// 物料属性值
        /// </summary>
        [Label("物料属性值")]
        public static readonly Property<string> ItemPropertyValueProperty = P<ProductBomDetail>.RegisterExtensionReadOnly("ItemPropertyValueProperty", typeof(ProductBomDetailViewConfig),
            GetItemPropertyValueProperty, ProductBomDetail.PropertyValueListProperty);

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="detail">产品BOM明细</param>
        /// <returns>string</returns>
        public static string GetItemPropertyValueProperty(ProductBomDetail detail)
        {
            var groups = detail.PropertyValueList.GroupBy(p => p.Definition.Name).ToList();
            string[] result = new string[groups.Count];
            for (int i = 0; i < groups.Count; i++)
            {
                var values = groups[i].Select(p => p.Value);
                result[i] = groups[i].Key + "：" + string.Join("、", values);
            }

            return string.Join("；", result);
        }
        #endregion

        #region 来源类型 PropertyName
        /// <summary>
        ///  来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<string> SourceTypeNameProperty = P<ProductBomDetail>.RegisterExtensionReadOnly("SourceTypeName", typeof(ProductBomDetailViewConfig),
            GetSourceTypeName, ProductBomDetail.ItemProperty);

        /// <summary>
        /// 获取来源类型
        /// </summary>
        /// <param name="me">产品BOM明细</param>
        /// <returns>来源类型</returns>
        public static string GetSourceTypeName(ProductBomDetail me)
        {
            return EnumViewModel.EnumToLabel(me.Item?.SourceType).L10N();
        }
        #endregion

        /// <summary>
        /// 工艺单BOM明细视图ViewGroup
        /// </summary>
        public const string TechOrderItemView = "TechOrderItemView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(TechOrderItemView);
            if (ViewGroup == TechOrderItemView)
                ConfigTechOrderItemView();
        }

        private void ConfigTechOrderItemView()
        {

            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).ShowInList().Readonly().UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    keyValues.Add(nameof(e.ItemSpecificationModel), nameof(e.Item.SpecificationModel));
                    keyValues.Add(nameof(e.ItemSourceType), nameof(e.Item.ItemSourceType));
                    keyValues.Add(nameof(e.ItemUnitName), "UnitId_Display");
                    m.DicLinkField = keyValues;
                }).ShowInList(150);
                View.Property(p => p.ItemName).ShowInList().HasLabel("物料名称").ShowInList(150).Readonly();
                View.Property(p => p.ItemSpecificationModel).ShowInList().HasLabel("规格型号").Readonly();
                //View.Property(ProductBomDetailViewConfig.ItemPropertyValueProperty).ShowInList().HasLabel("物料属性").Readonly();
                View.Property(p => p.ItemExtPropName).ShowInList().HasLabel("物料属性").Readonly();
                View.Property(p => p.UnitQty).ShowInList().UseSpinEditor(e => { e.MinValue = 0; e.AllowDecimals = true; }).Readonly();
                View.Property(p => p.ItemUnitName).ShowInList().HasLabel("单位").Readonly();
                View.Property(p => p.LossRate).UseSpinEditor(e => { e.MinValue = 0; e.AllowDecimals = true; }).ShowInList().Readonly();
                View.Property(p => p.IsRecoilItem).HasLabel("反冲物料");
                View.Property(p => p.ItemSourceType).UseEnumEditor().ShowInList().HasLabel("来源类型");
                View.Property(ProductBomDetailViewConfig.AlternativeProperty)/*.UseAlternativeEditor()*/.HasLabel("物料替代").ShowInList().Readonly();
                View.Property(p => p.Remark).ShowInList().Readonly();
                View.Property(p => p.ProcessSegment).ShowInList().Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.AlternativeList).IsVisible = false;
                View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
            }
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal(true);
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(ProductBomDetailImportCommand).FullName);
            View.AddBehavior("SIE.Web.Items.ProductBoms.ProductBomDetailBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UseDataSource((e, c, r) =>
                {
                    List<int> type = new List<int>() { (int)ItemType.Material, (int)ItemType.SemiFinished };
                    var list = RT.Service.Resolve<ItemController>().GetItems(type, c, r);
                    return list;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    keyValues.Add(nameof(e.ItemSpecificationModel), nameof(e.Item.SpecificationModel));
                    keyValues.Add(nameof(e.ItemUnitName), "UnitId_Display");
                    keyValues.Add(nameof(e.ItemSourceType), nameof(e.Item.ItemSourceType));
                    keyValues.Add(nameof(e.Precision), nameof(e.Item.Precision));
                    keyValues.Add(nameof(e.EnableExtendProperty), nameof(e.Item.EnableExtendProperty));

                    keyValues.Add(nameof(e.ItemExtProp), string.Empty);
                    keyValues.Add(nameof(e.ItemExtPropName), string.Empty);
                    m.DicLinkField = keyValues;
                }).UseListSetting(e => { e.HelpInfo = "显示原物料/半成品的物料"; }).ShowInList(150).HasOrderNo(0);
                View.Property(p => p.ItemName).ShowInList(150).Readonly().HasOrderNo(1);
                View.Property(p => p.ItemSpecificationModel).Readonly().HasOrderNo(3);

                View.Property(p => p.ItemExtPropName).ShowInList(250)
                    .UseItemExtPropRecordsFieldEditor(p =>
                    {
                        p.SourceEntityType = "ProductBomDetail";
                        p.ItemIdField = "ItemId";
                        p.DbField = "ItemExtProp";
                    }).Readonly(p => p.EnableExtendProperty==false).Show(ShowInWhere.List);

                View.Property(p => p.UnitQty).UseItemUnitEditor(e => { e.MinValue = 0;});
                View.Property(p => p.ItemUnitName).Readonly();
                View.Property(p => p.Precision).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.LossRate).UseSpinEditor(e => { e.MinValue = 0; e.AllowDecimals = true; });
                View.Property(p => p.IsRecoilItem).HasLabel("反冲物料");
                View.Property(p => p.ItemSourceType).Readonly();
                
                View.Property(p => p.Remark);
                View.Property(p => p.ProcessSegment);
                View.ChildrenProperty(p => p.AlternativeList).IsVisible = false;
                View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
            }
        }
    }
}
