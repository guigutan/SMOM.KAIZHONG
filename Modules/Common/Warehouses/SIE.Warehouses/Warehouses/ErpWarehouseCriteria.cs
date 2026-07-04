using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// ERP子库查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("ERP子库查询")]
    public partial class ErpWarehouseCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码，ERP子库不同的组织编码可能一样
        /// </summary>
        [Label("ERP子库代码")]
        public static readonly Property<string> CodeProperty = P<ErpWarehouseCriteria>.Register(e => e.Code);

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
        [Label("ERP子库描述")]
        public static readonly Property<string> NameProperty = P<ErpWarehouseCriteria>.Register(e => e.Name);

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
        [Label("WMS库存组织")]
        public static readonly Property<string> WmsInvOrgProperty = P<ErpWarehouseCriteria>.Register(e => e.WmsInvOrg);

        /// <summary>
        /// WMS库存组织
        /// </summary>
        public string WmsInvOrg
        {
            get { return this.GetProperty(WmsInvOrgProperty); }
            set { this.SetProperty(WmsInvOrgProperty, value); }
        }
        #endregion

        #region 仓库 WarehouseCode
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseCodeProperty = P<ErpWarehouseCriteria>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 库区 StorageAreaCode
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> StorageAreaCodeProperty = P<ErpWarehouseCriteria>.Register(e => e.StorageAreaCode);

        /// <summary>
        /// 库区
        /// </summary>
        public string StorageAreaCode
        {
            get { return this.GetProperty(StorageAreaCodeProperty); }
            set { this.SetProperty(StorageAreaCodeProperty, value); }
        }
        #endregion

        #region 库位 StorageLocationCode
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationCodeProperty = P<ErpWarehouseCriteria>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
            set { this.SetProperty(StorageLocationCodeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            var result = RT.Service.Resolve<WarehouseController>().GetErpWarehouseData(this);
            return result;
        }
    }
}
