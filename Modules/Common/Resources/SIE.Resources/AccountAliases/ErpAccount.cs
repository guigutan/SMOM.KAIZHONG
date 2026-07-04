using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources
{
    /// <summary>
    /// 账户别名
    /// </summary>
    [RootEntity, Serializable]
    [Label("账户别名")]
    [CriteriaQuery]
    [DisplayMember(nameof(Code))]
    public partial class ErpAccount : DataEntity, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpAccount()
        {
            State = State.Enable;
            SourceType = AccountSourceType.ERP;
        }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(80)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ErpAccount>.Register(e => e.Code);

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
        [MaxLength(80)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ErpAccount>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 启用/禁用
        /// <summary>
        /// 启用/禁用
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<ErpAccount>.Register(e => e.State);

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }

            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 生效日期 EffectiveDate
        /// <summary>
        /// 生效日期
        /// </summary>
        [Label("生效日期")]
        public static readonly Property<DateTime?> EffectiveDateProperty = P<ErpAccount>.Register(e => e.EffectiveDate);

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime? EffectiveDate
        {
            get { return this.GetProperty(EffectiveDateProperty); }
            set { this.SetProperty(EffectiveDateProperty, value); }
        }
        #endregion

        #region 失效日期 DisableDate
        /// <summary>
        /// 失效日期
        /// </summary>
        [Label("失效日期")]
        public static readonly Property<DateTime?> DisableDateProperty = P<ErpAccount>.Register(e => e.DisableDate);

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? DisableDate
        {
            get { return this.GetProperty(DisableDateProperty); }
            set { this.SetProperty(DisableDateProperty, value); }
        }
        #endregion

        #region 来源 SourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<AccountSourceType> SourceTypeProperty = P<ErpAccount>.Register(e => e.SourceType);

        /// <summary>
        /// 
        /// </summary>
        public AccountSourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 关键字 SourceKey
        /// <summary>
        /// 关键字
        /// </summary>
        [Label("关键字")]
        public static readonly Property<string> SourceKeyProperty = P<ErpAccount>.Register(e => e.SourceKey);

        /// <summary>
        /// 关键字
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 仓库 实体配置
    /// </summary>
    internal class ErpWarehouseConfig : EntityConfig<ErpAccount>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ERP_ACCOUNT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

}