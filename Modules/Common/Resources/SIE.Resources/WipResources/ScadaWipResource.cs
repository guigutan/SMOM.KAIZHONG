using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// SCADA生产资源
    /// </summary>
    [RootEntity, Serializable]
    [Label("SCADA生产资源")]
    public partial class ScadaWipResource : WipResource
    {
        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<ScadaWipResource>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// SCADA生产资源 实体配置
    /// </summary>
    internal class ScadaWipResourceConfig : EntityConfig<ScadaWipResource>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_WIP_SCHE").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}