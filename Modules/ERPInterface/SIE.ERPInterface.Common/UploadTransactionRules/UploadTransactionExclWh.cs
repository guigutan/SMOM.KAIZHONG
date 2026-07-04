using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.UploadTransactionRules
{
    /// <summary>
    /// 事务上传仓库排除
    /// </summary>
    [RootEntity, Serializable]
    public class UploadTransactionExclWh : DataEntity
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<UploadTransactionExclWh>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<UploadTransactionExclWh>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<UploadTransactionExclWh>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<UploadTransactionExclWh>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 交易上传规则 UploadTransactionRule
        /// <summary>
        /// 交易上传规则Id
        /// </summary>
        [Label("交易上传规则")]
        public static readonly IRefIdProperty UploadTransactionRuleIdProperty =
            P<UploadTransactionExclWh>.RegisterRefId(e => e.UploadTransactionRuleId, ReferenceType.Parent);

        /// <summary>
        /// 交易上传规则Id
        /// </summary>
        public double UploadTransactionRuleId
        {
            get { return (double)this.GetRefId(UploadTransactionRuleIdProperty); }
            set { this.SetRefId(UploadTransactionRuleIdProperty, value); }
        }

        /// <summary>
        /// 交易上传规则
        /// </summary>
        public static readonly RefEntityProperty<UploadTransactionRule> UploadTransactionRuleProperty =
            P<UploadTransactionExclWh>.RegisterRef(e => e.UploadTransactionRule, UploadTransactionRuleIdProperty);

        /// <summary>
        /// 交易上传规则
        /// </summary>
        public UploadTransactionRule UploadTransactionRule
        {
            get { return this.GetRefEntity(UploadTransactionRuleProperty); }
            set { this.SetRefEntity(UploadTransactionRuleProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 事务上传仓库排除列表 实体配置
    /// </summary>
    internal class UploadTransctionExclWhsConfig : EntityConfig<UploadTransactionExclWh>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("UL_TRANS_EXCL_WH").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
