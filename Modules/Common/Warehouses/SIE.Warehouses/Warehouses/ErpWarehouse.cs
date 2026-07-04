using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// ERP子库
    /// </summary>
    [RootEntity, Serializable]
    [Label("ERP子库")]
    [ConditionQueryType(typeof(ErpWarehouseCriteria))]
    [DisplayMember(nameof(Code))]
    public partial class ErpWarehouse : DataEntity, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpWarehouse()
        {
            State = State.Enable;
        }

        #region 编码 Code
        /// <summary>
        /// 编码，ERP子库不同的组织编码可能一样
        /// </summary>
        [Required]         
        [MaxLength(80)]
        [Label("ERP子库代码")]
        public static readonly Property<string> CodeProperty = P<ErpWarehouse>.Register(e => e.Code);

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
        [Label("ERP子库描述")]
        public static readonly Property<string> NameProperty = P<ErpWarehouse>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region WMS库存组织 WmsInvOrg
        /// <summary>
        /// WMS库存组织
        /// </summary>
        [Required]
        [Label("WMS库存组织")]
        public static readonly Property<string> WmsInvOrgProperty = P<ErpWarehouse>.Register(e => e.WmsInvOrg);

        /// <summary>
        /// WMS库存组织
        /// </summary>
        public string WmsInvOrg
        {
            get { return this.GetProperty(WmsInvOrgProperty); }
            set { this.SetProperty(WmsInvOrgProperty, value); }
        }
        #endregion

        #region 启用/禁用
        /// <summary>
        /// 启用/禁用
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<ErpWarehouse>.Register(e => e.State);

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }

            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region ERP库存组织Id ErpOrgId
        /// <summary>
        /// ERP库存组织Id
        /// </summary>        
        [Label("ERP库存组织Id")]
        public static readonly Property<string> ErpOrgIdProperty = P<ErpWarehouse>.Register(e => e.ErpOrgId);

        /// <summary>
        /// ERP库存组织Id
        /// </summary>
        public string ErpOrgId
        {
            get { return this.GetProperty(ErpOrgIdProperty); }
            set { this.SetProperty(ErpOrgIdProperty, value); }
        }
        #endregion

        #region ERP库存组织编码 ErpOrgCode
        /// <summary>
        /// ERP库存组织编码
        /// </summary>
        [Label("ERP库存组织编码")]
        public static readonly Property<string> ErpOrgCodeProperty = P<ErpWarehouse>.Register(e => e.ErpOrgCode);

        /// <summary>
        /// ERP库存组织编码
        /// </summary>
        public string ErpOrgCode
        {
            get { return this.GetProperty(ErpOrgCodeProperty); }
            set { this.SetProperty(ErpOrgCodeProperty, value); }
        }
        #endregion

        #region ERP库存组织名称 ErpOrgName
        /// <summary>
        /// ERP库存组织名称
        /// </summary>
        [Label("ERP库存组织名称")]
        public static readonly Property<string> ErpOrgNameProperty = P<ErpWarehouse>.Register(e => e.ErpOrgName);

        /// <summary>
        /// ERP库存组织名称
        /// </summary>
        public string ErpOrgName
        {
            get { return this.GetProperty(ErpOrgNameProperty); }
            set { this.SetProperty(ErpOrgNameProperty, value); }
        }
        #endregion

        #region 来源 SourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<ErpWarehouseSourceType> SourceTypeProperty = P<ErpWarehouse>.Register(e => e.SourceType);

        /// <summary>
        /// 
        /// </summary>
        public ErpWarehouseSourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region WMS库位信息 ErpWarehouseDetailList
        /// <summary>
        /// WMS库位信息
        /// </summary>
        public static readonly ListProperty<EntityList<ErpWarehouseDetail>> ErpWarehouseDetailListProperty = P<ErpWarehouse>.RegisterList(e => e.ErpWarehouseDetailList);

        /// <summary>
        /// WMS库位信息
        /// </summary>
        public EntityList<ErpWarehouseDetail> ErpWarehouseDetailList
        {
            get { return this.GetLazyList(ErpWarehouseDetailListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 仓库 实体配置
    /// </summary>
    internal class ErpWarehouseConfig : EntityConfig<ErpWarehouse>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ERP_WH").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }

}