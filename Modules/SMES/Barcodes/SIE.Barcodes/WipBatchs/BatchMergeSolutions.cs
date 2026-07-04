using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 批次合并方案
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次合并方案")]
    public partial class BatchMergeSolutions : DataEntity
    {
        #region 方案名称 name
        /// <summary>
        /// 方案名称
        /// </summary>
        [Label("方案名称")]
        [Required]
        public static readonly Property<string> NameProperty = P<BatchMergeSolutions>.Register(e => e.Name);

        /// <summary>
        /// 方案名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 方案描述 Description
        /// <summary>
        /// 方案描述
        /// </summary>
        [Label("方案描述")]
        public static readonly Property<string> DescriptionProperty = P<BatchMergeSolutions>.Register(e => e.Description);

        /// <summary>
        /// 方案描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 是否缺省 IsDefault
        /// <summary>
        /// 是否缺省
        /// </summary>
        [Label("是否缺省")]
        public static readonly Property<bool> IsDefaultProperty = P<BatchMergeSolutions>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否缺省
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 批次合并规则列表 RuleList
        /// <summary>
        /// 批次合并规则列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchMergeRule>> RuleListProperty = P<BatchMergeSolutions>.RegisterList(e => e.RuleList);

        /// <summary>
        /// 批次合并规则列表
        /// </summary>
        public EntityList<BatchMergeRule> RuleList
        {
            get { return this.GetLazyList(RuleListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 批次合并方案 实体配置
    /// </summary>
    internal class BatchMergeSolutionsConfig : EntityConfig<BatchMergeSolutions>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BATCH_MERGE_SOL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}