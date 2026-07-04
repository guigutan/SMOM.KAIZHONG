using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 物料库存扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StorageSaftyExtStorageArea
    {
        #region StorageSafty StorageSaftyExtList (物料库存)
        /// <summary>
        /// 物料库存 扩展属性。
        /// </summary>
        public static ListProperty<EntityList<StorageSafty>> StorageSaftyExtListProperty { get; } =
            P<StorageArea>.RegisterExtensionList<EntityList<StorageSafty>>("StorageSaftyExtList", typeof(StorageSaftyExtStorageArea));

        /// <summary>
        /// 获取 物料库存 属性的值
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>物料库存</returns>
        public static EntityList<StorageSafty> GetStorageSaftyExtList(StorageArea me)
        {
            return me.GetProperty(StorageSaftyExtListProperty);
        }

        /// <summary>
        /// 设置物料库存 属性的值
        /// </summary>
        /// <param name="me">工位货区</param>
        /// <param name="value">物料库存</param>
        public static void SetStorageSaftyExtList(StorageArea me, EntityList<StorageSafty> value)
        {
            me.SetProperty(StorageSaftyExtListProperty, value);
        }
        #endregion

        /// <summary>
        /// 工位货区 实体配置
        /// </summary>
        internal class StorageSaftyExtStorageAreaConfig : EntityConfig<StorageArea>
        {
            /// <summary>
            /// 实体配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(StationStorageExtStorageArea.StationStorageAreaExtListProperty).DontMapColumn();
            }
        }
    }
}