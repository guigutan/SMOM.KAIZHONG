using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses.Configs;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 事务原因
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(ReasonNoConfig))]
    [CriteriaQuery]
    [Label("事务原因")]
    [DisplayMember(nameof(Reason.Name))]
    public partial class Reason : DataEntity, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Reason()
        {
            State = State.Enable;
        }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Reason>.Register(e => e.Code);

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
        [Required]
        [MaxLength(240)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Reason>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Describe
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(2000)]
        [Label("描述")]
        public static readonly Property<string> DescribeProperty = P<Reason>.Register(e => e.Describe);

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe
        {
            get { return GetProperty(DescribeProperty); }
            set { SetProperty(DescribeProperty, value); }
        }
        #endregion

        #region 类型 ReasonType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ReasonType> ReasonTypeProperty = P<Reason>.Register(e => e.ReasonType);

        /// <summary>
        /// 类型
        /// </summary>
        public ReasonType ReasonType
        {
            get { return GetProperty(ReasonTypeProperty); }
            set { SetProperty(ReasonTypeProperty, value); }
        }

        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Reason>.Register(e => e.State);
        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键， 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<Reason>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class ReasonConfig : EntityConfig<Reason>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TRANS_REASON").MapAllProperties();
            Meta.Property(Reason.DescribeProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 原因API交互数据
    /// </summary>
    public class ReasonData
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
    }
}