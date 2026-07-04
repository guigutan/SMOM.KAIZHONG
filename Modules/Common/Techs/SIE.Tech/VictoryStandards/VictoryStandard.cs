using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.VictoryStandards
{
    /// <summary>
    /// 胜制方案
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("胜制方案")]
    [DisplayMember(nameof(Code))]
    public partial class VictoryStandard : DataEntity, IStateEntity
    {
        #region 胜制编码 Code
        /// <summary>
        /// 胜制编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("胜制编码")]
        public static readonly Property<string> CodeProperty = P<VictoryStandard>.Register(e => e.Code);

        /// <summary>
        /// 胜制编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 胜制名称 Name
        /// <summary>
        /// 胜制名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("胜制名称")]
        public static readonly Property<string> NameProperty = P<VictoryStandard>.Register(e => e.Name);

        /// <summary>
        /// 胜制名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 最大测试次数 MaxTestQty
        /// <summary>
        /// 最大测试次数
        /// </summary>
        [MinValue(1)]
        [Label("最大测试次数")]
        public static readonly Property<int> MaxTestQtyProperty = P<VictoryStandard>.Register(e => e.MaxTestQty);

        /// <summary>
        /// 最大测试次数
        /// </summary>
        public int MaxTestQty
        {
            get { return this.GetProperty(MaxTestQtyProperty); }
            set { this.SetProperty(MaxTestQtyProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<VictoryStandard>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<VictoryStandard>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 胜局标准 StandardDetailList
        /// <summary>
        /// 胜局标准
        /// </summary>
        [Label("胜局标准")]
        public static readonly ListProperty<EntityList<VictoryStandardDetail>> StandardDetailListProperty = P<VictoryStandard>.RegisterList(e => e.StandardDetailList);

        /// <summary>
        /// 胜局标准
        /// </summary>
        public EntityList<VictoryStandardDetail> StandardDetailList
        {
            get { return this.GetLazyList(StandardDetailListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 胜制方案维护-实体配置
    /// </summary>
    internal class VictoryStandardConfig : EntityConfig<VictoryStandard>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_VIC_STA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
