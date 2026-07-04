using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料扩展属性 视图
    /// </summary>
    [RootEntity, Serializable]
    public class ItemPropertyValueViewModel : Entity<double>
    {
        #region 物料ID ItemId
        /// <summary>
        /// 物料ID
        /// </summary>
        public static readonly Property<double> ItemIdProperty = P<ItemPropertyValueViewModel>.Register(e => e.ItemId);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return GetProperty(ItemIdProperty); }
            set { SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 物料属性定义Id DefinitionId
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public static readonly Property<double> DefinitionIdProperty = P<ItemPropertyValueViewModel>.Register(e => e.DefinitionId);

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return GetProperty(DefinitionIdProperty); }
            set { SetProperty(DefinitionIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class ItemPropertyValueViewModelEntityConfig : EntityConfig<ItemPropertyValueViewModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<ItemPropertyValue>("b1")
                    .GroupBy(p => new { p.ItemId, p.DefinitionId })
                    .Select((b1) => new
                    {
                        b1.ItemId,
                        Item_Id = b1.ItemId,
                        Definition_Id = b1.DefinitionId
                    })
            .ToQuery();

            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
        }
    }
}