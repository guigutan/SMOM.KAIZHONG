using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 扩展盘点计划范围
    /// </summary>
    [CompiledPropertyDeclarer]
    public static class InventoryPlanExtEquip
    {
        #region InventoryPlanEquipment InventoryEquipment (盘点范围)
        /// <summary>
        /// 盘点范围 扩展属性。
        /// </summary>
        public static readonly Property<InventoryPlanEquipment> InventoryEquipmentProperty =
            P<InventoryPlan>.RegisterExtension<InventoryPlanEquipment>("InventoryEquipment", typeof(InventoryPlanExtEquip));

        /// <summary>
        /// 获取 盘点范围 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static InventoryPlanEquipment GetInventoryEquipment(InventoryPlan me)
        {
            return me.GetProperty(InventoryEquipmentProperty);
        }

        /// <summary>
        /// 设置 盘点范围 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetInventoryEquipment(InventoryPlan me, InventoryPlanEquipment value)
        {
            me.SetProperty(InventoryEquipmentProperty, value);
        }
        #endregion


        #region InventoryPlanFixture InventoryPlanFixture (盘点范围(工治具))
        /// <summary>
        /// 盘点范围(工治具) 扩展属性。
        /// </summary>
        public static readonly Property<InventoryPlanFixture> InventoryPlanFixtureProperty =
            P<InventoryPlan>.RegisterExtension<InventoryPlanFixture>("InventoryPlanFixture", typeof(InventoryPlanExtEquip));

        /// <summary>
        /// 获取 盘点范围(工治具) 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static InventoryPlanFixture GetInventoryPlanFixture(InventoryPlan me)
        {
            return me.GetProperty(InventoryPlanFixtureProperty);
        }

        /// <summary>
        /// 设置 盘点范围(工治具) 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetInventoryPlanFixture(InventoryPlan me, InventoryPlanFixture value)
        {
            me.SetProperty(InventoryPlanFixtureProperty, value);
        }
        #endregion

        #region InventoryPlanSparePart InventoryPlanSparePart (盘点范围（备件）)
        /// <summary>
        /// 盘点范围（备件） 扩展属性。
        /// </summary>
        public static readonly Property<InventoryPlanSparePart> InventoryPlanSparePartProperty =
            P<InventoryPlan>.RegisterExtension<InventoryPlanSparePart>("InventoryPlanSparePart", typeof(InventoryPlanExtEquip));

        /// <summary>
        /// 获取 盘点范围（备件） 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static InventoryPlanSparePart GetInventoryPlanSparePart(this InventoryPlan me)
        {
            return me.GetProperty(InventoryPlanSparePartProperty);
        }

        /// <summary>
        /// 设置 盘点范围（备件） 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetInventoryPlanSparePart(this InventoryPlan me, InventoryPlanSparePart value)
        {
            me.SetProperty(InventoryPlanSparePartProperty, value);
        }
        #endregion        
    }

    /// <summary>
    /// 作用是映射的时候能找到对应的实体
    /// </summary>
    internal class InventoryPlanExtEquipConfig : EntityConfig<InventoryPlan>
    {
        protected override void ConfigMeta()
        {
            Meta.Property(InventoryPlanExtEquip.InventoryEquipmentProperty).DontMapColumn();
            Meta.Property(InventoryPlanExtEquip.InventoryPlanFixtureProperty).DontMapColumn();
            Meta.Property(InventoryPlanExtEquip.InventoryPlanSparePartProperty).DontMapColumn();
        }
    }
}
