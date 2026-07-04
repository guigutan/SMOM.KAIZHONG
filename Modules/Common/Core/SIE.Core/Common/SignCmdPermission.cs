using SIE.Common.Platform;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Common
{
    /// <summary>
    /// 签名模块配置
    /// </summary>
    [RootEntity, Serializable]     
    [Label("签名模块配置")]    
    public partial class SignCmdPermission : DataEntity
    {
        //通过前端JS可以拿到entity_type匹配ModuleViewConfig的entity_type拿到Id （多个）需要注意join 表 ModuleConfig 关联 Platform 拿到唯一       
        //再通过SYS_CMD_CONFIG 的MODULE_VIEW_CONFIG_ID 和当前表的OperationKey关联 SYS_CMD_CONFIG Key

        #region 父实体 ParentEntityType
        /// <summary>
        /// 父实体
        /// </summary>
        [Label("父实体")]
        public static readonly Property<string> ParentEntityTypeProperty = P<SignCmdPermission>.Register(e => e.ParentEntityType);

        /// <summary>
        /// 父实体
        /// </summary>
        public string ParentEntityType
        {
            get { return this.GetProperty(ParentEntityTypeProperty); }
            set { this.SetProperty(ParentEntityTypeProperty, value); }
        }
        #endregion

        #region 实体 EntityType
        /// <summary>
        /// 实体
        /// </summary>
        [Label("实体")]
        public static readonly Property<string> EntityTypeProperty = P<SignCmdPermission>.Register(e => e.EntityType);

        /// <summary>
        /// 实体
        /// </summary>
        public string EntityType
        {
            get { return this.GetProperty(EntityTypeProperty); }
            set { this.SetProperty(EntityTypeProperty, value); }
        }
        #endregion

        #region 操作 OperationKey
        /// <summary>
        /// 操作
        /// </summary>
        [Label("命令")]
        public static readonly Property<string> OperationKeyProperty = P<SignCmdPermission>.Register(e => e.OperationKey);

        /// <summary>
        /// 操作
        /// </summary>
        public string OperationKey
        {
            get { return GetProperty(OperationKeyProperty); }
            set { SetProperty(OperationKeyProperty, value); }
        }
        #endregion

        #region 父实体名称 ParentEntityName
        /// <summary>
        /// 父实体名称
        /// </summary>
        [Label("功能")]
        public static readonly Property<string> ParentEntityNameProperty = P<SignCmdPermission>.Register(e => e.ParentEntityName);

        /// <summary>
        /// 父实体名称
        /// </summary>
        public string ParentEntityName
        {
            get { return this.GetProperty(ParentEntityNameProperty); }
            set { this.SetProperty(ParentEntityNameProperty, value); }
        }
        #endregion

        #region 实体名称 EntityName
        /// <summary>
        /// 实体名称
        /// </summary>
        [Label("页签")]
        public static readonly Property<string> EntityNameProperty = P<SignCmdPermission>.Register(e => e.EntityName);

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName
        {
            get { return this.GetProperty(EntityNameProperty); }
            set { this.SetProperty(EntityNameProperty, value); }
        }
        #endregion

        #region 平台 Platform
        /// <summary>
        /// 平台
        /// </summary>
        [Label("属性名")]
        public static readonly Property<Platform> PlatformProperty = P<SignCmdPermission>.Register(e => e.Platform);

        /// <summary>
        /// 平台
        /// </summary>
        public Platform Platform
        {
            get { return this.GetProperty(PlatformProperty); }
            set { this.SetProperty(PlatformProperty, value); }
        }
        #endregion

        #region 允许发起人签名 IsAllowCreater
        /// <summary>
        /// 允许发起人签名
        /// </summary>
        [Label("允许发起人签名")]
        public static readonly Property<bool> IsAllowCreaterProperty = P<SignCmdPermission>.Register(e => e.IsAllowCreater);

        /// <summary>
        /// 允许发起人签名
        /// </summary>
        public bool IsAllowCreater
        {
            get { return this.GetProperty(IsAllowCreaterProperty); }
            set { this.SetProperty(IsAllowCreaterProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 签名模块配置
    /// </summary>
    internal class SignCmdPermissionConfig : EntityConfig<SignCmdPermission>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("SIGN_CMD_PERMISSION").MapAllProperties();          
            Meta.Property(SignCmdPermission.OperationKeyProperty).ColumnMeta.HasLength(240);
            Meta.Property(SignCmdPermission.ParentEntityTypeProperty).ColumnMeta.HasLength(240);
            Meta.Property(SignCmdPermission.EntityTypeProperty).ColumnMeta.HasLength(240);            
        }
    }
}
