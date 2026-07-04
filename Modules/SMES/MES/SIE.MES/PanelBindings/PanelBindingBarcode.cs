using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.PanelBindings
{
    /// <summary>
    /// MES工单条码绑定记录-条码数量
    /// </summary>
    [RootEntity, Serializable]
    public class PanelBindingBarcode : Entity<double>
    {
        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WorkOrderIdProperty = P<PanelBindingBarcode>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<PanelBindingBarcode>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class PanelBindingBarcodeConfig : EntityConfig<PanelBindingBarcode>
    {
        /// <summary> 
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<PanelAndBarcode>("w")
            .Select(w => new
            {
                id = w.Id.MAX(),
                Work_Order_Id = w.WorkOrderId,
                Qty = w.Qty.SUM(),
            })
            .GroupBy(w => new
            {
                w.WorkOrderId
            })
            .ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.DisablePhantoms();
        }
    }
}
