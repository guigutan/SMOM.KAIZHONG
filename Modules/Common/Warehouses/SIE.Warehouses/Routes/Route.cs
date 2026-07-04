using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 路径基础信息
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(RouteCriteria))]
    [Label("路径基础信息")]
    public partial class Route : DataEntity, IStateEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("路径编码")]
        [Required]
        public static readonly Property<string> CodeProperty = P<Route>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 起始仓库 SrcWhCode
        /// <summary>
        /// 起始仓库
        /// </summary>
        [Label("起始仓库")]
        public static readonly Property<string> SrcWhCodeProperty = P<Route>.Register(e => e.SrcWhCode);

        /// <summary>
        /// 起始仓库
        /// </summary>
        public string SrcWhCode
        {
            get { return GetProperty(SrcWhCodeProperty); }
            set { SetProperty(SrcWhCodeProperty, value); }
        }
        #endregion

        #region 起始地址 SrcAdd
        /// <summary>
        /// 全程起点的地址，站台代码或巷道编号
        /// </summary>
        [MaxLength(64)]
        [Label("起始地址")]
        public static readonly Property<string> SrcAddProperty = P<Route>.Register(e => e.SrcAdd);

        /// <summary>
        /// 全程起点的地址，站台代码或巷道编号
        /// </summary>
        public string SrcAdd
        {
            get { return GetProperty(SrcAddProperty); }
            set { SetProperty(SrcAddProperty, value); }
        }
        #endregion

        #region 终点仓库 DesWhCode
        /// <summary>
        /// 终点仓库
        /// </summary>
        [Label("终点仓库")]
        public static readonly Property<string> DesWhCodeProperty = P<Route>.Register(e => e.DesWhCode);

        /// <summary>
        /// 终点仓库
        /// </summary>
        public string DesWhCode
        {
            get { return GetProperty(DesWhCodeProperty); }
            set { SetProperty(DesWhCodeProperty, value); }
        }
        #endregion

        #region 终点地址 DesAdd
        /// <summary>
        /// 全程终点的地址，站台代码或巷道编号
        /// </summary>
        [MaxLength(64)]
        [Label("终点地址")]
        public static readonly Property<string> DesAddProperty = P<Route>.Register(e => e.DesAdd);

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
        public static readonly Property<string> DocksProperty = P<Route>.Register(e => e.Docks);

        /// <summary>
        /// 堆垛机的接驳站台，如堆垛机有多层出入库楼层站台，则通过该字段确定
        /// </summary>
        public string Docks
        {
            get { return GetProperty(DocksProperty); }
            set { SetProperty(DocksProperty, value); }
        }
        #endregion

        #region 记录备注 Remark
        /// <summary>
        /// 记录备注
        /// </summary>
        [MaxLength(128)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<Route>.Register(e => e.Remark);

        /// <summary>
        /// 记录备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 启用/禁用
        /// <summary>
        /// 启用/禁用
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Route>.Register(e => e.State);

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }

            set { SetProperty(StateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 路径基础信息 实体配置
    /// </summary>
    internal class RouteConfig : EntityConfig<Route>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("WCS_ROUTE").MapAllProperties();
            Meta.Property(Route.SrcAddProperty).ColumnMeta.HasLength(128);
            Meta.Property(Route.DesAddProperty).ColumnMeta.HasLength(128);
            Meta.Property(Route.DocksProperty).ColumnMeta.HasLength(128);
            Meta.Property(Route.RemarkProperty).ColumnMeta.HasLength(256);
            Meta.EnablePhantoms();
        }
    }
}