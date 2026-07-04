using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线货区货位扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class ItemStorageExtStorageLocation
    {
        #region ItemStorage ItemStorageExtList (产线货区货位)
        /// <summary>
        /// 产线货区货位 扩展属性。
        /// </summary>
        public static ListProperty<EntityList<ItemStorage>> ItemStorageExtListProperty { get; } =
            P<StorageLocation>.RegisterExtensionList<EntityList<ItemStorage>>("ItemStorageExtList", typeof(ItemStorageExtStorageLocation), new ListPropertyMeta
            {
                HasManyType = HasManyType.Aggregation,
                DataProvider = e => GetItemStorageExtList(e as StorageLocation)
            });

        /// <summary>
        /// 获取 产线物料货位 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象</param>
        /// <returns>产线物料货位集合</returns>
        public static EntityList<ItemStorage> GetItemStorageExtList(StorageLocation me)
        {
            return me.GetProperty(ItemStorageExtListProperty);
        }

        /// <summary>
        /// 设置 产线物料货位 
        /// </summary>
        /// <param name="me">产线货区货位</param>
        /// <param name="value">产线物料货位</param>
        public static void SetItemStorageExtList(StorageLocation me, EntityList<ItemStorage> value)
        {
            me.SetProperty(ItemStorageExtListProperty, value);
        }
        #endregion

        /// <summary>
        /// 产线货区货位 实体配置
        /// </summary>
        internal class ItemStorageExtStorageLocationConfig : EntityConfig<StorageLocation>
        {
            /// <summary>
            /// 实体配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(ItemStorageExtStorageLocation.ItemStorageExtListProperty).DontMapColumn();
            }
        }
    }
}