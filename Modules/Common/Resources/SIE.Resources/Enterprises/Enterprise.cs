using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 企业模型
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery(typeof(CriteriaProvider))]
    [DisplayMember(nameof(Name))]
    [Label("企业模型")]
    public partial class Enterprise : DataEntity
    {
        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public Enterprise()
        {
            IsByHand = YesNo.Yes;
        }
        #endregion

        #region 库存组织ID InvOrgId
        /// <summary>
        /// 库存组织ID
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<Enterprise>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织ID
        /// </summary>
        public int? InvOrgId
        {
            get { return GetProperty(InvOrgIdProperty); }
            set { SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Enterprise>.Register(e => e.Code);

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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Enterprise>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region ERP存储的组织ID ErpOrgId
        /// <summary>
        /// ERP存储的组织ID
        /// </summary>
        [Label("ERP存储的组织ID")]
        public static readonly Property<int> ErpOrgIdProperty = P<Enterprise>.Register(e => e.ErpOrgId);

        /// <summary>
        /// ERP存储的组织ID
        /// </summary>
        public int ErpOrgId
        {
            get { return GetProperty(ErpOrgIdProperty); }
            set { SetProperty(ErpOrgIdProperty, value); }
        }
        #endregion

        #region 是否资源 IsResource
        /// <summary>
        /// 是否资源
        /// </summary>
        [Label("是否资源")]
        public static readonly Property<bool> IsResourceProperty = P<Enterprise>.Register(e => e.IsResource);

        /// <summary>
        /// 是否资源
        /// </summary>
        public bool IsResource
        {
            get { return GetProperty(IsResourceProperty); }
            set { SetProperty(IsResourceProperty, value); }
        }
        #endregion

        #region 是否手工录入 IsByHand
        /// <summary>
        /// 是否手工录入
        /// </summary>
        [Label("是否手工录入")]
        public static readonly Property<YesNo> IsByHandProperty = P<Enterprise>.Register(e => e.IsByHand);

        /// <summary>
        /// 是否手工录入
        /// </summary>
        public YesNo IsByHand
        {
            get { return GetProperty(IsByHandProperty); }
            set { SetProperty(IsByHandProperty, value); }
        }
        #endregion

        #region 企业层级 Level
        /// <summary>
        /// 企业层级Id
        /// </summary>
        [Label("层级")]
        public static readonly IRefIdProperty LevelIdProperty = P<Enterprise>.RegisterRefId(e => e.LevelId, ReferenceType.Normal);

        /// <summary>
        /// 企业层级Id
        /// </summary>
        public double LevelId
        {
            get { return (double)GetRefId(LevelIdProperty); }
            set { SetRefId(LevelIdProperty, value); }
        }

        /// <summary>
        /// 企业层级
        /// </summary>
        [Label("层级")]
        public static readonly RefEntityProperty<EnterpriseLevel> LevelProperty = P<Enterprise>.RegisterRef(e => e.Level, LevelIdProperty);

        /// <summary>
        /// 企业层级
        /// </summary>
        public EnterpriseLevel Level
        {
            get { return GetRefEntity(LevelProperty); }
            set { SetRefEntity(LevelProperty, value); }
        }
        #endregion

        #region 企业类型 LevelType
        /// <summary>
        /// 企业类型
        /// </summary>
        [Label("企业类型")]
        public static readonly Property<EnterpriseType> LevelTypeProperty = P<Enterprise>.RegisterView(e => e.LevelType, p => p.Level.Type);

        /// <summary>
        /// 企业类型
        /// </summary>
        public EnterpriseType LevelType
        {
            get { return this.GetProperty(LevelTypeProperty); }
            set { this.SetProperty(LevelTypeProperty, value); }
        }
        #endregion

        #region 是否资源 LevelIsResource
        /// <summary>
        /// 是否资源
        /// </summary>
        [Label("是否资源")]
        public static readonly Property<bool> LevelIsResourceProperty = P<Enterprise>.RegisterView(e => e.LevelIsResource, p => p.Level.IsResource);

        /// <summary>
        /// 是否资源
        /// </summary>
        public bool LevelIsResource
        {
            get { return this.GetProperty(LevelIsResourceProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 企业模型 实体配置
    /// </summary>
    internal class EnterpriseConfig : EntityConfig<Enterprise>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_ENTERPRISE").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.SupportTree();
            Meta.EnablePhantoms();
        }
    }
}