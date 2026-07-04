using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;

namespace SIE.EMS.Equipments.Boms
{
    /// <summary>
    /// 设备BOM列表明细属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public static class EquipBomExtension
    {
        #region EntityList<EquipBomDetail> EquipBomDetailList (MI部件列表)
        /// <summary>
        /// 设备BOM列表明细 扩展属性。
        /// </summary>
        public static readonly ListProperty<EntityList<EquipBomDetail>> EquipBomDetailListProperty =
            P<EquipBom>.RegisterExtensionList<EntityList<EquipBomDetail>>("EquipBomDetailList", typeof(EquipBomExtension));

        /// <summary>
        /// 获取 设备BOM列表明细 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static EntityList<EquipBomDetail> GetEquipBomDetailList(this EquipBom me)
        {
            return me.GetLazyList(EquipBomDetailListProperty) as EntityList<EquipBomDetail>;
        }

        /// <summary>
        /// 设置设备BOM列表明细
        /// </summary>
        /// <param name="me">Asn对象</param>
        /// <param name="value">需要设置的序列号对象</param>
        public static void SetEquipBomDetailList(EquipBom me, EntityList<EquipBomDetail> value)
        {
            me.SetProperty(EquipBomDetailListProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 设备BOM扩展 实体配置
    /// </summary>
    internal class EquipBomExtensionConfig : EntityConfig<EquipBom>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(EquipBomExtension.EquipBomDetailListProperty).DontMapColumn();
        }
    }
}
