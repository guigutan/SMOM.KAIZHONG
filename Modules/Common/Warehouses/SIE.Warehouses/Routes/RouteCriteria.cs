using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 路径查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("路径查询")]
    public class RouteCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("路径编码")]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<RouteCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion
         
        #region 起始仓库 SrcWh
        /// <summary>
        /// 起始仓库Id
        /// </summary>
        [Label("起始仓库")]
        public static readonly IRefIdProperty SrcWhIdProperty =
            P<RouteCriteria>.RegisterRefId(e => e.SrcWhId, ReferenceType.Normal);

        /// <summary>
        /// 起始仓库Id
        /// </summary>
        public double? SrcWhId
        {
            get { return (double?)this.GetRefNullableId(SrcWhIdProperty); }
            set { this.SetRefNullableId(SrcWhIdProperty, value); }
        }

        /// <summary>
        /// 起始仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> SrcWhProperty =
            P<RouteCriteria>.RegisterRef(e => e.SrcWh, SrcWhIdProperty);

        /// <summary>
        /// 起始仓库
        /// </summary>
        public Warehouse SrcWh
        {
            get { return this.GetRefEntity(SrcWhProperty); }
            set { this.SetRefEntity(SrcWhProperty, value); }
        }
        #endregion

        #region 仓库编码 SrcWhCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> SrcWhCodeProperty = P<RouteCriteria>.RegisterView(e => e.SrcWhCode, p => p.SrcWh.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string SrcWhCode
        {
            get { return this.GetProperty(SrcWhCodeProperty); }
        }
        #endregion

        #region 起始地址 SrcAdd
        /// <summary>
        /// 全程起点的地址，站台代码或巷道编号
        /// </summary>
        [MaxLength(64)]
        [Label("起始地址")]
        public static readonly Property<string> SrcAddProperty = P<RouteCriteria>.Register(e => e.SrcAdd);

        /// <summary>
        /// 全程起点的地址，站台代码或巷道编号
        /// </summary>
        public string SrcAdd
        {
            get { return GetProperty(SrcAddProperty); }
            set { SetProperty(SrcAddProperty, value); }
        }
        #endregion

        #region 终点仓库 DesWh
        /// <summary>
        /// 终点仓库Id
        /// </summary>
        [Label("终点仓库")]
        public static readonly IRefIdProperty DesWhIdProperty =
            P<RouteCriteria>.RegisterRefId(e => e.DesWhId, ReferenceType.Normal);

        /// <summary>
        /// 终点仓库Id
        /// </summary>
        public double? DesWhId
        {
            get { return (double?)this.GetRefNullableId(DesWhIdProperty); }
            set { this.SetRefNullableId(DesWhIdProperty, value); }
        }

        /// <summary>
        /// 终点仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> DesWhProperty =
            P<RouteCriteria>.RegisterRef(e => e.DesWh, DesWhIdProperty);

        /// <summary>
        /// 终点仓库
        /// </summary>
        public Warehouse DesWh
        {
            get { return this.GetRefEntity(DesWhProperty); }
            set { this.SetRefEntity(DesWhProperty, value); }
        }
        #endregion

        #region 终点仓库编码 DesWhCode
        /// <summary>
        /// 终点仓库编码
        /// </summary>
        [Label("终点仓库编码")]
        public static readonly Property<string> DesWhCodeProperty = P<RouteCriteria>.RegisterView(e => e.DesWhCode, p => p.DesWh.Code);

        /// <summary>
        /// 终点仓库编码
        /// </summary>
        public string DesWhCode
        {
            get { return this.GetProperty(DesWhCodeProperty); }
        }
        #endregion

        #region 终点地址 DesAdd
        /// <summary>
        /// 全程终点的地址，站台代码或巷道编号
        /// </summary>
        [MaxLength(64)]
        [Label("终点地址")]
        public static readonly Property<string> DesAddProperty = P<RouteCriteria>.Register(e => e.DesAdd);

        /// <summary>
        /// 全程终点的地址，站台代码或巷道编号
        /// </summary>
        public string DesAdd
        {
            get { return GetProperty(DesAddProperty); }
            set { SetProperty(DesAddProperty, value); }
        }
        #endregion

        #region 接驳站台 Docks
        /// <summary>
        /// 堆垛机的接驳站台，如堆垛机有多层出入库楼层站台，则通过该字段确定
        /// </summary>
        [MaxLength(64)]
        [Label("接驳站台")]
        public static readonly Property<string> DocksProperty = P<RouteCriteria>.Register(e => e.Docks);

        /// <summary>
        /// 堆垛机的接驳站台，如堆垛机有多层出入库楼层站台，则通过该字段确定
        /// </summary>
        public string Docks
        {
            get { return GetProperty(DocksProperty); }
            set { SetProperty(DocksProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<RouteCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            var result = RT.Service.Resolve<RouteController>().GetRouteData(this);
            return result;
        }
    }
}
