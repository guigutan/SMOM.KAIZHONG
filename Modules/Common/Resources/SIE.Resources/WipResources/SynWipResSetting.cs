using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 生产资源同步配置
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("资源同步配置"), DisplayMember(nameof(Name))]
    public class SynWipResSetting : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [MaxLength(100)]
        public static readonly Property<string> NameProperty = P<SynWipResSetting>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 资源类型 Type
        /// <summary>
        /// 资源类型
        /// </summary>
        [Label("资源类型")]
        [MaxLength(200)]
        public static readonly Property<string> TypeProperty = P<SynWipResSetting>.Register(e => e.Type);

        /// <summary>
        /// 资源类型
        /// </summary>
        public string Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region DLL名称 AssenblyName
        /// <summary>
        /// DLL名称
        /// </summary>
        [Label("DLL名称")]
        [MaxLength(200)]
        public static readonly Property<string> AssenblyNameProperty = P<SynWipResSetting>.Register(e => e.AssenblyName);

        /// <summary>
        /// DLL名称
        /// </summary>
        public string AssenblyName
        {
            get { return this.GetProperty(AssenblyNameProperty); }
            set { this.SetProperty(AssenblyNameProperty, value); }
        }
        #endregion

        #region 是否同步 IsSyn
        /// <summary>
        /// 是否同步
        /// </summary>
        [Label("是否同步")]
        public static readonly Property<bool> IsSynProperty = P<SynWipResSetting>.Register(e => e.IsSyn);

        /// <summary>
        /// 是否同步
        /// </summary>
        public bool IsSyn
        {
            get { return this.GetProperty(IsSynProperty); }
            set { this.SetProperty(IsSynProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class SynWipResSettingEntityConfig : EntityConfig<SynWipResSetting>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapAllProperties().MapTable("WIP_RES_SYN_SET");
            Meta.Property(SynWipResSetting.TypeProperty).ColumnMeta.HasLength(800);
            Meta.Property(SynWipResSetting.AssenblyNameProperty).ColumnMeta.HasLength(800);
            Meta.IsPhantomEnabled = true;
            Meta.DisableInvOrg();
            base.ConfigMeta();
        }
    }
}
