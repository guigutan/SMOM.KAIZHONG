using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Barcodes.Barcodes
{
    /// <summary>
    /// 唯一条码
    /// </summary>
    [RootEntity, Serializable]
    [Label("唯一条码")]
    public class UniqueBarcode:DataEntity
    {
        #region 条码号 Sn
        /// <summary>
        /// 条码号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("条码号")]
        public static readonly Property<string> SnProperty = P<UniqueBarcode>.Register(e => e.Sn);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 库存组织 InvOrg
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly IRefIdProperty InvOrgIdProperty =
            P<UniqueBarcode>.RegisterRefId(e => e.InvOrgId, ReferenceType.Normal);

        /// <summary>
        /// 注释Id
        /// </summary>
        public double InvOrgId
        {
            get { return (double)this.GetRefId(InvOrgIdProperty); }
            set { this.SetRefId(InvOrgIdProperty, value); }
        }

        /// <summary>
        /// 注释
        /// </summary>
        public static readonly RefEntityProperty<InvOrg> InvOrgProperty =
            P<UniqueBarcode>.RegisterRef(e => e.InvOrg, InvOrgIdProperty);

        /// <summary>
        /// 注释
        /// </summary>
        public InvOrg InvOrg
        {
            get { return this.GetRefEntity(InvOrgProperty); }
            set { this.SetRefEntity(InvOrgProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 库存组织名称 InvOrgName
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("库存组织名称")]
        public static readonly Property<string> InvOrgNameProperty = P<UniqueBarcode>.RegisterView(e => e.InvOrgName, p => p.InvOrg.Name);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string InvOrgName
        {
            get { return this.GetProperty(InvOrgNameProperty); }
        }
        #endregion

        #endregion

    }
    internal class UniqueBarcodeConfig : EntityConfig<UniqueBarcode>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BC_BARCODE").MapAllProperties();
            Meta.Property(UniqueBarcode.SnProperty).ColumnMeta.HasIndex();
            Meta.DisableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
