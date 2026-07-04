using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 子生产批次
    /// </summary>
    [ChildEntity, Serializable]
    [Label("子生产批次")]
    public partial class SubWipBatch : WipBatch
    {
        #region 生产批次 WipBatch
        /// <summary>
        /// 生产批次Id
        /// </summary>
        [Label("生产批次")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<SubWipBatch>.RegisterRefId(e => e.WipBatchId, ReferenceType.Parent);

        /// <summary>
        /// 生产批次Id
        /// </summary>
        public double? WipBatchId
        {
            get { return (double?)this.GetRefNullableId(WipBatchIdProperty); }
            set { this.SetRefNullableId(WipBatchIdProperty, value); }
        }

        /// <summary>
        /// 生产批次
        /// </summary>
        public static readonly RefEntityProperty<WipBatch> WipBatchProperty =
            P<SubWipBatch>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 生产批次
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 批次编码 BatchNo
        /// <summary>
        /// 批次编码
        /// </summary>
        [Label("批次编码")]
        public static readonly Property<string> WipBatchNoProperty = P<SubWipBatch>.RegisterView(e => e.WipBatchNo, p => p.WipBatch.BatchNo);

        /// <summary>
        /// 批次编码
        /// </summary>
        public string WipBatchNo
        {
            get { return this.GetProperty(WipBatchNoProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 子生产批次 实体配置
    /// </summary>
    internal class SubWipBatchEntityConfig : EntityConfig<SubWipBatch>
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