using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料小类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("物料小类")]
    [DisplayMember(nameof(Name))]
    public partial class ItemSmallCategory : ItemCategory
    {
    }

    /// <summary>
    /// 物料小类 实体配置
    /// </summary>
    internal class ItemSmallCategoryEntityConfig : EntityConfig<ItemSmallCategory>
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