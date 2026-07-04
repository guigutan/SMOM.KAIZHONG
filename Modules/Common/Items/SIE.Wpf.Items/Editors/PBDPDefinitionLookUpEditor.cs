using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using System.Linq;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 产品BOM明细物料属性PagingLookUpEditor
    /// </summary>
    public class PBDPDefinitionLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "PBDPDefinitionLookUpEditor";

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
            var propertyValue = source as ProductBomDetailPropertyValue;
            if (propertyValue.Detail == null || propertyValue.Detail.Item == null) 
                return new EntityList<ItemPropertyDefinition>();
            var result = RT.Service.Resolve<ItemController>().GetItemPropertys(propertyValue.Detail.ItemId).Select(o => o.Definition).Distinct<ItemPropertyDefinition>();
            EntityList<ItemPropertyDefinition> definitions = new EntityList<ItemPropertyDefinition>();
            definitions.AddRange(result);
            return definitions;
        }
    }

    /// <summary>
    /// 产品BOM明细物料属性值PagingLookUpEditor
    /// </summary>
    public class PBDPValueLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "PBDPValueLookUpEditor";

        /// <summary>
        ///  设置编辑器选择元数据
        /// </summary>
        //public PBDPValueLookUpEditor()
        //{
        //    //Meta.SelectionViewMeta = new MetaModel.View.SelectionViewMeta
        //    //{
        //    //    SelectionMode = MetaModel.EntitySelectionMode.Single,
        //    //    SelectionEntityType = typeof(ItemPropertyValue),
        //    //    SelectedValuePath = ItemPropertyValue.ValueProperty,
        //    //};
        //}

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
            var propertyValue = source as ProductBomDetailPropertyValue;
            if (propertyValue.Definition == null || propertyValue.Detail == null)
            {
                return new EntityList<ItemPropertyValue>();
            }

            return RT.Service.Resolve<ItemController>().GetItemPropertyValues(propertyValue.Detail.ItemId, propertyValue.DefinitionId);
        }
    }
}
