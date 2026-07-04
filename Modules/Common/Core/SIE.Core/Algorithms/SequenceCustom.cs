using SIE.Domain;
using SIE.MetaModel;
using System;

namespace SIE.Core.Algorithms
{
    /// <summary>
    /// 序列自定义生成
    /// </summary>
    [RootEntity, Serializable]
    public partial class SequenceCustom : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SequenceCustom()
        {
            LockTime = DateTime.Now;
        }

        #region 当前值 CurrentValue
        /// <summary>
        /// 当前值
        /// </summary>
        [Required]
        public static readonly Property<int> CurrentValueProperty = P<SequenceCustom>.Register(e => e.CurrentValue);

        /// <summary>
        /// 当前值
        /// </summary>
        public int CurrentValue
        {
            get { return this.GetProperty(CurrentValueProperty); }
            set { this.SetProperty(CurrentValueProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public static readonly Property<int> TypeProperty = P<SequenceCustom>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public int Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 前缀 PreContent
        /// <summary>
        /// 前缀
        /// </summary>
        [Required]
        [MaxLength(80)]
        public static readonly Property<string> PreContentProperty = P<SequenceCustom>.Register(e => e.PreContent);

        /// <summary>
        /// 前缀
        /// </summary>
        public string PreContent
        {
            get { return this.GetProperty(PreContentProperty); }
            set { this.SetProperty(PreContentProperty, value); }
        }
        #endregion

        #region 上锁时间 LockTime
        /// <summary>
        /// 上锁时间
        /// </summary>
        public static readonly Property<DateTime> LockTimeProperty = P<SequenceCustom>.Register(e => e.LockTime);

        /// <summary>
        /// 上锁时间
        /// </summary>
        public DateTime LockTime
        {
            get { return this.GetProperty(LockTimeProperty); }
            set { this.SetProperty(LockTimeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 序列自定义生成 实体配置
    /// </summary>
    internal class SequenceCustomConfig : EntityConfig<SequenceCustom>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("SEQ_CUSTOM").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableTimeStamp();
        }
    }
}
