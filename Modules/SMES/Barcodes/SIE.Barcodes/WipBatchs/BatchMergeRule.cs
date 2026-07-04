using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 批次合并规则
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次合并规则")]
    public partial class BatchMergeRule : DataEntity
    {
        #region 是否选择 IsSelected
        /// <summary>
        /// 是否选择
        /// </summary>
        [Label("是否选择")]
        public static readonly Property<bool> IsSelectedProperty = P<BatchMergeRule>.Register(e => e.IsSelected);

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsSelected
        {
            get { return GetProperty(IsSelectedProperty); }
            set { SetProperty(IsSelectedProperty, value); }
        }
        #endregion

        #region 合并参数 MergeParam
        /// <summary>
        /// 合并参数
        /// </summary>
        [Label("合并参数")]
        public static readonly Property<MergeParam> MergeParamProperty = P<BatchMergeRule>.Register(e => e.MergeParam);

        /// <summary>
        /// 合并参数
        /// </summary>
        public MergeParam MergeParam
        {
            get { return GetProperty(MergeParamProperty); }
            set { SetProperty(MergeParamProperty, value); }
        }
        #endregion

        #region 批次合并方案 BatchMergeSolutions
        /// <summary>
        /// 批次合并方案Id
        /// </summary>
        [Label("批次合并方案")]
        public static readonly IRefIdProperty BatchMergeSolutionsIdProperty = P<BatchMergeRule>.RegisterRefId(e => e.BatchMergeSolutionsId, ReferenceType.Parent);

        /// <summary>
        /// 批次合并方案Id
        /// </summary>
        public double BatchMergeSolutionsId
        {
            get { return (double)GetRefId(BatchMergeSolutionsIdProperty); }
            set { SetRefId(BatchMergeSolutionsIdProperty, value); }
        }

        /// <summary>
        /// 批次合并方案
        /// </summary>
        public static readonly RefEntityProperty<BatchMergeSolutions> BatchMergeSolutionsProperty = P<BatchMergeRule>.RegisterRef(e => e.BatchMergeSolutions, BatchMergeSolutionsIdProperty);

        /// <summary>
        /// 批次合并方案
        /// </summary>
        public BatchMergeSolutions BatchMergeSolutions
        {
            get { return GetRefEntity(BatchMergeSolutionsProperty); }
            set { SetRefEntity(BatchMergeSolutionsProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 批次合并规则 实体配置
    /// </summary>
    internal class BatchMergeRuleConfig : EntityConfig<BatchMergeRule>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BATCH_MERGE_RULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}