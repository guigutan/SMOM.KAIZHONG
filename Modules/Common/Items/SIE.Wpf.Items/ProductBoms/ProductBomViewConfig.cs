using SIE.Domain;
using SIE.Items;
using SIE.Items.ViewModels;
using SIE.Wpf.Items.Commands;
using SIE.Wpf.Items.ViewModels;
using System.Linq;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 产品BOM视图配置
    /// </summary>
    internal class ProductBomViewConfig : WPFViewConfig<ProductBom>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // 视图配置
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands().UseCommands(typeof(ProductBomDefaultCommand));
            View.RemoveCommands(WPFCommandNames.ListCopy);
            View.ReplaceCommands(WPFCommandNames.ListAdd, typeof(ProductBomAddCommand));
            View.UseDefaultBehaviors();
            View.UseDetail(dialogHeight: 530, dialogWidth: 880);
            using (View.DeclareGroup("基本信息", 2, false))
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Product).UseProductCombinationEditor().Readonly(DataEntityStatus.IsEditStatusProperty).HasLabel("产品编码");
                View.Property(p => p.Product.Name).Readonly().HasLabel("产品名称");
                View.Property(p => p.Product.SpecificationModel).HasLabel("规格型号");
                View.Property(p => p.Product.Unit.Name).HasLabel("单位");
                View.Property(p => p.Version);
                View.Property(p => p.IsDefault).Readonly();
                View.Property(p => p.SourceType).Readonly(true);
                View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.All).HasLabel("物料清单");
                View.ChildrenProperty(p => p.PropertyValueList).Visible(false);
                View.ChildrenProperty(p => p.CombinationReplateList).Visible(false);
                View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
                {
                    var bom = o.Parent as ProductBom;
                    if (bom == null) return null;
                    var list = bom.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("PropertyValueViewModel");
                    if (list == null)
                    {
                        var result = bom.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel
                        {
                            DefinitionId = f.Key,
                            Values = f.Select(p => p.Value).ToList(),
                            Type = f.Select(p => p.ProductBom).FirstOrDefault().GetType(),
                            ParentId = bom.Id,
                        });

                        list = new EntityList<PropertyValueViewModel>();
                        list.AddRange(result);
                        foreach (var value in list)
                            value.ItemId = bom.ProductId;
                        bom.LocalContext.SetExtendedProperty("PropertyValueViewModel", list);
                    }

                    list.MarkSaved();
                    return list;
                }, BomPropertyValueViewModelViewConfig.BomPropertyValueViewModelListView).Show(ChildShowInWhere.All).HasLabel("产品属性");
            }
        }

        /// <summary>
        /// 默认下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Product).HasLabel("产品编码");
            View.Property(p => p.Version);
        }

        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Product).HasLabel("产品编码").Readonly(false);
            View.Property(p => p.Product.Name).HasLabel("产品名称").Readonly(false);
            View.Property(p => p.Product.SpecificationModel).HasLabel("规格型号").Readonly(false);
        }
    }
}
