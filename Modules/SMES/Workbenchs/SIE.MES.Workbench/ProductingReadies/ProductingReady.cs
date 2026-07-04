using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.ProductingReadies
{
    /// <summary>
    /// 产前准备 
    /// TODO huchuqiang demo数据
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("产前准备")]
    public class ProductingReady : DataEntity
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ProductingReady>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<ProductingReady>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料状态 ItemState
        /// <summary>
        /// 物料状态
        /// </summary>
        [Label("物料状态")]
        public static readonly Property<ReadyState> ItemStateProperty = P<ProductingReady>.Register(e => e.ItemState);

        /// <summary>
        /// 物料状态
        /// </summary>
        public ReadyState ItemState
        {
            get { return this.GetProperty(ItemStateProperty); }
            set { this.SetProperty(ItemStateProperty, value); }
        }
        #endregion

        #region 工具状态 ToolState
        /// <summary>
        /// 工具状态
        /// </summary>
        [Label("工具状态")]
        public static readonly Property<ReadyState> ToolStateProperty = P<ProductingReady>.Register(e => e.ToolState);

        /// <summary>
        /// 工具状态
        /// </summary>
        public ReadyState ToolState
        {
            get { return this.GetProperty(ToolStateProperty); }
            set { this.SetProperty(ToolStateProperty, value); }
        }
        #endregion

        #region 人员状态 EmployeeState
        /// <summary>
        /// 人员状态
        /// </summary>
        [Label("人员状态")]
        public static readonly Property<ReadyState> EmployeeStateProperty = P<ProductingReady>.Register(e => e.EmployeeState);

        /// <summary>
        /// 人员状态
        /// </summary>
        public ReadyState EmployeeState
        {
            get { return this.GetProperty(EmployeeStateProperty); }
            set { this.SetProperty(EmployeeStateProperty, value); }
        }
        #endregion

        #region ESOP状态 EsopState
        /// <summary>
        /// ESOP状态
        /// </summary>
        [Label("ESOP状态")]
        public static readonly Property<ReadyState> EsopStateProperty = P<ProductingReady>.Register(e => e.EsopState);

        /// <summary>
        /// ESOP状态
        /// </summary>
        public ReadyState EsopState
        {
            get { return this.GetProperty(EsopStateProperty); }
            set { this.SetProperty(EsopStateProperty, value); }
        }
        #endregion

        #region 品质预防 QualityState
        /// <summary>
        /// 品质预防
        /// </summary>
        [Label("品质预防")]
        public static readonly Property<ReadyState> QualityStateProperty = P<ProductingReady>.Register(e => e.QualityState);

        /// <summary>
        /// 品质预防
        /// </summary>
        public ReadyState QualityState
        {
            get { return this.GetProperty(QualityStateProperty); }
            set { this.SetProperty(QualityStateProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 产前准备实体配置
    /// </summary>
    internal class ProductingReadyEntityConfig : EntityConfig<ProductingReady>
    {
        /// <summary>
        /// 配置实体元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRODUCTING_READY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
