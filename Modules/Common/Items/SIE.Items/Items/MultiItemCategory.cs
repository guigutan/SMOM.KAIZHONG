using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items
{
    /// <summary>
    /// 物料小类
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MultiItemCategoryCriteria))]
    [Label("物料分类类")]
    [DisplayMember(nameof(Name))]
    public partial class MultiItemCategory : ItemCategory
    {
    }

    /// <summary>
    /// 物料小类 实体配置
    /// </summary>
    internal class MultiItemCategoryEntityConfig : EntityConfig<MultiItemCategory>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_CATE").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.IsTreeEntity = false;
        }
    }
}
