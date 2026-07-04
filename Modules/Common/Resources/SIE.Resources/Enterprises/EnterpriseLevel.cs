using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 企业层级
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery(typeof(CriteriaProvider))]
    [Label("企业层级")]
    [DisplayMember(nameof(Name))]
    public partial class EnterpriseLevel : DataEntity
    {
        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseLevel()
        {
            IsByHand = YesNo.Yes;
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EnterpriseLevel>.Register(e => e.Code);

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
        [MaxLength(80)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EnterpriseLevel>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 库存组织ID InvOrgId
        /// <summary>
        /// 库存组织ID
        /// </summary>
        [Label("库存组织ID")]
        public static readonly Property<int?> InvOrgIdProperty = P<EnterpriseLevel>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织ID
        /// </summary>
        public int? InvOrgId
        {
            get { return GetProperty(InvOrgIdProperty); }
            set { SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

        #region 是否资源 IsResource
        /// <summary>
        /// 是否资源
        /// </summary>
        [Label("是否资源")]
        public static readonly Property<bool> IsResourceProperty = P<EnterpriseLevel>.Register(e => e.IsResource);

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
        public static readonly Property<YesNo> IsByHandProperty = P<EnterpriseLevel>.Register(e => e.IsByHand);

        /// <summary>
        /// 是否手工录入
        /// </summary>
        public YesNo IsByHand
        {
            get { return GetProperty(IsByHandProperty); }
            set { SetProperty(IsByHandProperty, value); }
        }
        #endregion

        #region 企业类型 Type
        /// <summary>
        /// 企业类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<EnterpriseType?> TypeProperty = P<EnterpriseLevel>.Register(e => e.Type);

        /// <summary>
        /// 企业类型
        /// </summary>
        public EnterpriseType? Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 企业层级 实体配置
    /// </summary>
    internal class EnterpriseLevelConfig : EntityConfig<EnterpriseLevel>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_ENT_LEVEL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.SupportTree();    //支持树型功能
            Meta.DisableInvOrg();   //禁用库存组织
        }
    }
}