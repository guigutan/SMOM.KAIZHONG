using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 企业模型中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("企业模型中间表")]
    public partial class EnterpriseInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EnterpriseInf>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<EnterpriseInf>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region ERP组织ID ErpOrgId
        /// <summary>
        /// ERP组织ID
        /// </summary>
        [Label("ERP组织ID")]
        public static readonly Property<string> ErpOrgIdProperty = P<EnterpriseInf>.Register(e => e.ErpOrgId);

        /// <summary>
        /// ERP组织ID
        /// </summary>
        public string ErpOrgId
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
        public static readonly Property<bool> IsResourceProperty = P<EnterpriseInf>.Register(e => e.IsResource);

        /// <summary>
        /// 是否资源
        /// </summary>
        public bool IsResource
        {
            get { return GetProperty(IsResourceProperty); }
            set { SetProperty(IsResourceProperty, value); }
        }
        #endregion

        #region 企业层级 Level
        /// <summary>
        /// 企业层级
        /// </summary>
        [Label("企业层级")]
        public static readonly Property<string> LevelProperty = P<EnterpriseInf>.Register(e => e.Level);

        /// <summary>
        /// 企业层级
        /// </summary>
        public string Level
        {
            get { return GetProperty(LevelProperty); }
            set { SetProperty(LevelProperty, value); }
        }
        #endregion

        #region 层次 EnterpriseLevelNum
        /// <summary>
        /// 层次
        /// </summary>
        [Label("层次")]
        public static readonly Property<int> EnterpriseLevelNumProperty = P<EnterpriseInf>.Register(e => e.EnterpriseLevelNum);

        /// <summary>
        /// 层次
        /// </summary>
        public int EnterpriseLevelNum
        {
            get { return this.GetProperty(EnterpriseLevelNumProperty); }
            set { this.SetProperty(EnterpriseLevelNumProperty, value); }
        }
        #endregion

        #region 父编码 ParentCode
        /// <summary>
        /// 父编码
        /// </summary>
        [Label("父编码")]
        public static readonly Property<string> ParentCodeProperty = P<EnterpriseInf>.Register(e => e.ParentCode);

        /// <summary>
        /// 父编码
        /// </summary>
        public string ParentCode
        {
            get { return this.GetProperty(ParentCodeProperty); }
            set { this.SetProperty(ParentCodeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 企业模型中间表 实体配置
    /// </summary>
    internal class EnterpriseInfConfig : EntityConfig<EnterpriseInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_ENTERPRISE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}