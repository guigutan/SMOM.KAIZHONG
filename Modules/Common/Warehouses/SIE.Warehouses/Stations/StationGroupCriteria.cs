using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// 站台组查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("站台组查询")]
    public partial class StationGroupCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<StationGroupCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> NameProperty = P<StationGroupCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 站台组所在方案中位置 Location
        /// <summary>
        /// 站台组所在方案中位置
        /// </summary>
        [MaxLength(64)]
        [Label("所在方案中位置")]
        public static readonly Property<string> LocationProperty = P<StationGroupCriteria>.Register(e => e.Location);

        /// <summary>
        /// 站台组所在方案中位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 仓库 WarehouseCode
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StationGroupCriteria>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 是否盘点 IsCount
        /// <summary>
        /// 是否盘点
        /// </summary>
        [Label("是否盘点")]
        public static readonly Property<bool> IsCountProperty = P<StationGroupCriteria>.Register(e => e.IsCount);

        /// <summary>
        /// 是否盘点
        /// </summary>
        public bool IsCount
        {
            get { return this.GetProperty(IsCountProperty); }
            set { this.SetProperty(IsCountProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>计划资料列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<StationController>().GetStationGroups(this);
        }
    }
}
