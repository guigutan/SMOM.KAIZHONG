using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.Items.ViewModels;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 产品BOM物料属性子视图，属性PagingLookUpEditor
    /// </summary>
    public class PBPDefinitionLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "PBPDefinitionLookUpEditor";

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <param name="titleProperty">托管属性</param>
        /// <returns>EntityList</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            double itemId = 0d;
            var definitionIds = new List<double>();

            if (source is ProductBomPropertyValue)
            {
                var propertyValue = source as ProductBomPropertyValue;
                if (propertyValue.ProductBom != null && propertyValue.ProductBom.Product != null)
                {
                    itemId = propertyValue.ProductBom.ProductId;
                }
                definitionIds.AddRange(propertyValue.ParentList.OfType<ProductBomPropertyValue>().Select(p => p.DefinitionId));
            }
            else if ((source is PropertyValueViewModel))
            {
                var propertyValue = source as PropertyValueViewModel;
                if (propertyValue.ItemId > 0)
                {
                    itemId = propertyValue.ItemId;
                }

                definitionIds.AddRange(propertyValue.ParentList.OfType<PropertyValueViewModel>().Select(p => p.DefinitionId));
            }

            var result = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId).Select(o => o.Definition).Where(p => !definitionIds.Contains(p.Id)).Distinct(new PropertyComparer<ItemPropertyDefinition>(nameof(ItemPropertyDefinition.Id)));
            EntityList<ItemPropertyDefinition> definitions = new EntityList<ItemPropertyDefinition>();
            definitions.AddRange(result);
            return definitions;
        }
    }

    /// <summary>
    /// 产品BOM物料属性子视图，属性值PagingLookUpEditor
    /// </summary>
    public class PBPValueLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "PBPValueLookUpEditor";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="propertyViewMeta">propertyViewMeta</param>
        public override void Initialize(WPFEntityPropertyViewMeta propertyViewMeta)
        {
            propertyViewMeta.SelectionViewMeta = new MetaModel.View.SelectionViewMeta
            {
                SelectionMode = MetaModel.EntitySelectionMode.Single,
                SelectionEntityType = typeof(ItemPropertyValue),
                SelectedValuePath = ItemPropertyValue.ValueProperty,
            };
            base.Initialize(propertyViewMeta);
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <param name="titleProperty">托管属性</param>
        /// <returns>EntityList</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var propertyValue = source as ProductBomPropertyValue;
            if (propertyValue.Definition == null || propertyValue.ProductBom == null)
            {
                return new EntityList<ItemPropertyValue>();
            }

            return RT.Service.Resolve<ItemController>().GetItemPropertyValues(propertyValue.ProductBom.ProductId, propertyValue.DefinitionId);
        }
    }
}
