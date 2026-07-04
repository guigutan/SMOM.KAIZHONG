using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 附加关联属性生产批次规则
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public static class WoWipBatchExt
    {
        #region WoWipBatch AttacWoWipBatch (附加关联属性生产批次)
        /// <summary>
        /// 附加关联属性生产批次 扩展属性。
        /// </summary>
        public static readonly Property<WoWipBatch> AttacWoWipBatchProperty =
            P<WorkOrder>.RegisterExtension<WoWipBatch>("WoWipBatch", typeof(WoWipBatchExt));

        /// <summary>
        /// 获取 附加关联属性生产批次 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>生产批次</returns>
        public static WoWipBatch GetAttacWoWipBatch(WorkOrder me)
        {
            return me.GetProperty(AttacWoWipBatchProperty);
        }

        /// <summary>
        /// 设置 附加关联属性生产批次 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetAttacWoWipBatch(WorkOrder me, WoWipBatch value)
        {
            me.SetProperty(AttacWoWipBatchProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 作用是映射的时候能找到对应的实体
    /// </summary>
    internal class ItemExtBatchRuleConfig : EntityConfig<WorkOrder>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(WoWipBatchExt.AttacWoWipBatchProperty).DontMapColumn();
        }
    }
}