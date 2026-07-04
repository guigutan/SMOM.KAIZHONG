using SIE.Core.ProjectMaintains;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using SIE.Web.Items._Extentions_;
using SIE.Web.Items.ProductBoms.Commands;
using SIE.Web.Items.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM视图配置
    /// </summary>
    internal class ProductBomViewConfig : WebViewConfig<ProductBom>
    {
        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Save, "SIE.Web.Items.ProductBoms.Commands.ProductBomSaveCommand");
            View.RemoveCommands(WebCommandNames.Copy);
            View.ReplaceCommands(WebCommandNames.Add, typeof(ProductBomAddCommand).FullName);
            View.UseCommand("SIE.Web.Items.ProductBoms.Commands.ProductBomDefaultCommand");
            View.UseCommand(typeof(ProductBomImportCommand).FullName);
            
            using (View.DeclareGroup("基本信息", 2, false))
            {
                View.Property(p => p.Code).ShowInList(120);
                View.Property(p => p.Name).ShowInList(120);
                View.Property(p => p.Product).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    keyValues.Add(nameof(e.ProductSpecificationModel), nameof(e.Product.SpecificationModel));
                    keyValues.Add(nameof(e.EnableExtProp), nameof(e.Product.EnableExtendProperty));
                    keyValues.Add(nameof(e.ProductUnitName), "UnitId_Display");
                    keyValues.Add(nameof(e.ItemExtProp), string.Empty);
                    keyValues.Add(nameof(e.ItemExtPropName), string.Empty);
                    m.DicLinkField = keyValues;
                }).UseDataSource((e, c, r) =>
                {
                    List<int> types = new List<int>() {
                    (int)ItemType.Product,
                    (int)ItemType.SemiFinished
                    };

                    return RT.Service.Resolve<ItemController>().GetItems(types, c, r);
                }).UseListSetting(e => { e.HelpInfo = "显示成品/半成品物料"; }).ShowInList(120);

                View.Property(p => p.ProductName).ShowInList(120).Readonly();
                View.Property(p => p.ItemExtPropName)
                    .UseItemExtPropRecordsFieldEditor(p =>
                    {
                        p.SourceEntityType = "ProductBom";
                        p.ItemIdField = "ProductId";
                        p.DbField = "ItemExtProp";
                    }).Readonly(p => p.EnableExtProp == "0");
                View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
                }).Show();
                View.Property(p => p.ProductSpecificationModel).Readonly();
                View.Property(p => p.ProductUnitName).Readonly();
                View.Property(p => p.Version).UseListSetting(e => { e.HelpInfo = "根据产品BOM版本生成规则(配置项--产品BOM版本生成规则)"; });
                View.Property(p => p.IsDefault).Readonly();
                View.Property(p => p.SourceType).Readonly(true);
                View.Property(p => p.CreateByName);
                View.Property(p => p.CreateDate).ShowInList(150);
                View.Property(p => p.UpdateByName);
                View.Property(p => p.UpdateDate).ShowInList(150);
                View.ChildrenProperty(p => p.DetailList).HasLabel("物料清单").Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.PropertyValueList).IsVisible(false);
                View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
                {
                    var bomId = o.Parent.GetId();
                    var bom = RF.GetById<ProductBom>(bomId);
                    if (bom == null) return new EntityList<PropertyValueViewModel>();
                    var list = bom.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("PropertyValueViewModel");
                    if (list == null)
                    {
                        var result = bom.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Definition = f.Select(p => p.Definition).FirstOrDefault(), Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.ProductBom).FirstOrDefault().GetType(), ParentId = f.Select(p => p.ProductBomId).FirstOrDefault() });
                        list = new EntityList<PropertyValueViewModel>();
                        list.AddRange(result);
                        foreach (var value in list)
                            value.ItemId = bom.ProductId;
                        bom.LocalContext.SetExtendedProperty("PropertyValueViewModel", list);
                    }

                    list.MarkSaved();
                    return list;
                }, BomPropertyValueViewModelViewConfig.BomPropertyValueViewModelListView).Show(ChildShowInWhere.Hide).HasLabel("产品属性");
                View.ChildrenProperty(p => p.CombinationReplateList).Show(ChildShowInWhere.Hide).HasOrderNo(10);//组合代替
            }
        }

        /// <summary>
        /// 默认下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Product);
            View.Property(p => p.Version);
            View.Property(p => p.IsDefault);
        }

        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Product).Readonly(false).UsePagingLookUpPopupEditor(p => { p.Editable = true; p.MultiOrSelect = MultiSelect.Multi; }).Show(ShowInWhere.All);
            View.Property(p => p.ProductName).Readonly(false);
            View.Property(p => p.ProductSpecificationModel).Readonly(false);
        }
    }
}
