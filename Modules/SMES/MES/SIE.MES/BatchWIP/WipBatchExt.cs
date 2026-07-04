using SIE.Barcodes.WipBatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 生产批次
    /// </summary>
    [Serializable]
    [RootEntity]
    [Label("生产批次")]
    [ConditionQueryType(typeof(BatchCriteria))]
    [DisplayMember(nameof(BatchNo))]
    public class WipBatchExt : SubWipBatch
    {
    }

    /// <summary>
    /// 生产批次 实体配置
    /// </summary>
    internal class WipBatchExtEntityConfig : EntityConfig<WipBatchExt>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
        }
    }
}
