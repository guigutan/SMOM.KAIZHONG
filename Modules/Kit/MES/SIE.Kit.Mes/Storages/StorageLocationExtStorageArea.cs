using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线货区货位扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StorageLocationExtStorageArea
    {
        #region StorageLocation StorageLocationExtList (产线货区货位)
        /// <summary>
        /// 产线货区货位 扩展属性。
        /// </summary>
        public static ListProperty<EntityList<StorageLocation>> StorageLocationExtListProperty { get; } =
            P<StorageArea>.RegisterExtensionList<EntityList<StorageLocation>>("StorageLocationExtList", typeof(StorageLocationExtStorageArea));

        /// <summary>
        /// 获取 产线货区货位 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>产线货区货位</returns>
        public static EntityList<StorageLocation> GetStorageLocationExtList(StorageArea me)
        {
            return me.GetProperty(StorageLocationExtListProperty);
        }

        /// <summary>
        /// 获取 产线货区货位
        /// </summary>
        /// <param name="me">工位货区</param>
        /// <param name="value">产线货区货位</param>
        public static void SetStorageLocationExtList(StorageArea me, EntityList<StorageLocation> value)
        {
            me.SetProperty(StorageLocationExtListProperty, value);
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