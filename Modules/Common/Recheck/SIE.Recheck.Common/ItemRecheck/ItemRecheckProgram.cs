using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Recheck.Common.ItemRecheck
{
    /// <summary>
    /// 物料复检方案
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [EntityWithConfig(typeof(NoConfig))]
    [Label("物料复检方案")]
    [DisplayMember(nameof(Code))]
    public class ItemRecheckProgram : DataEntity, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemRecheckProgram()
        {
            State = State.Enable;
        }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<ItemRecheckProgram>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        public static readonly Property<string> NameProperty = P<ItemRecheckProgram>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(2000)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<ItemRecheckProgram>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 最大复检次数 MaxRecheckCount
        /// <summary>
        /// 最大复检次数
        /// </summary>
        [Label("最大复检次数")]
        [MinValue(0)]
        public static readonly Property<decimal> MaxRecheckCountProperty = P<ItemRecheckProgram>.Register(e => e.MaxRecheckCount);

        /// <summary>
        /// 最大复检次数
        /// </summary>
        public decimal MaxRecheckCount
        {
            get { return GetProperty(MaxRecheckCountProperty); }
            set { SetProperty(MaxRecheckCountProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<ItemRecheckProgram>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 物料复检方案明细列表 ItemRecheckProgramDetailList
        /// <summary>
        /// 物料复检方案明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<ItemRecheckProgramDetail>> IItemRecheckProgramDetailListProperty = P<ItemRecheckProgram>.RegisterList(e => e.ItemRecheckProgramDetailList);

        /// <summary>
        /// 物料复检方案明细列表
        /// </summary>
        public EntityList<ItemRecheckProgramDetail> ItemRecheckProgramDetailList
        {
            get { return GetLazyList(IItemRecheckProgramDetailListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 物料批次方案 实体配置
    /// </summary>
    internal class ItemRecheckProgramConfig : EntityConfig<ItemRecheckProgram>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_RECHECK_PROGRAM").MapAllProperties();
            Meta.Property(ItemRecheckProgram.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(ItemRecheckProgram.NameProperty).ColumnMeta.HasLength(240);
            Meta.EnablePhantoms();
        }
    }
}
