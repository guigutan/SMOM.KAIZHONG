using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工艺路线信息
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺路线信息")]
    public class LayoutInfo : DataEntity
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<LayoutInfo>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Parent);

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
            P<LayoutInfo>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工序流水码 Vornr
        /// <summary>
        /// 工序流水码
        /// </summary>
        [Label("工序流水码")]
        public static readonly Property<string> VornrProperty = P<LayoutInfo>.Register(e => e.Vornr);

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr
        {
            get { return this.GetProperty(VornrProperty); }
            set { this.SetProperty(VornrProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<LayoutInfo>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工作中心编码 WorkCenterCode
        /// <summary>
        /// 工作中心编码
        /// </summary>
        [Label("工作中心编码")]
        public static readonly Property<string> WorkCenterCodeProperty = P<LayoutInfo>.Register(e => e.WorkCenterCode);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode
        {
            get { return this.GetProperty(WorkCenterCodeProperty); }
            set { this.SetProperty(WorkCenterCodeProperty, value); }
        }
        #endregion

        #region 控制码(工序控制码) Steus
        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        [Label("控制码(工序控制码)")]
        public static readonly Property<string> SteusProperty = P<LayoutInfo>.Register(e => e.Steus);

        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        public string Steus
        {
            get { return this.GetProperty(SteusProperty); }
            set { this.SetProperty(SteusProperty, value); }
        }
        #endregion

        #region 工序数量 ProcessQty
        /// <summary>
        /// 工序数量
        /// </summary>
        [Label("工序数量")]
        public static readonly Property<decimal> ProcessQtyProperty = P<LayoutInfo>.Register(e => e.ProcessQty);

        /// <summary>
        /// 工序数量
        /// </summary>
        public decimal ProcessQty
        {
            get { return this.GetProperty(ProcessQtyProperty); }
            set { this.SetProperty(ProcessQtyProperty, value); }
        }
        #endregion

        #region 分单数量 Zcode
        /// <summary>
        /// 分单数量
        /// </summary>
        [Label("分单数量")]
        public static readonly Property<decimal> ZcodeProperty = P<LayoutInfo>.Register(e => e.Zcode);

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode
        {
            get { return this.GetProperty(ZcodeProperty); }
            set { this.SetProperty(ZcodeProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<LayoutInfo>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 订单工艺路线号 Aufpl
        /// <summary>
        /// 订单工艺路线号
        /// </summary>
        [Label("订单工艺路线号")]
        public static readonly Property<string> AufplProperty = P<LayoutInfo>.Register(e => e.Aufpl);

        /// <summary>
        /// 订单工艺路线号
        /// </summary>
        public string Aufpl
        {
            get { return this.GetProperty(AufplProperty); }
            set { this.SetProperty(AufplProperty, value); }
        }
        #endregion

        #region 订单工艺路线序号 Aplzl
        /// <summary>
        /// 订单工艺路线序号
        /// </summary>
        [Label("订单工艺路线序号")]
        public static readonly Property<string> AplzlProperty = P<LayoutInfo>.Register(e => e.Aplzl);

        /// <summary>
        /// 订单工艺路线序号
        /// </summary>
        public string Aplzl
        {
            get { return this.GetProperty(AplzlProperty); }
            set { this.SetProperty(AplzlProperty, value); }
        }
        #endregion

        #region 直接人工-人工时间 Vgw01
        /// <summary>
        /// 直接人工-人工时间
        /// </summary>
        [Label("直接人工-人工时间")]
        public static readonly Property<decimal?> Vgw01Property = P<LayoutInfo>.Register(e => e.Vgw01);

        /// <summary>
        /// 直接人工-人工时间
        /// </summary>
        public decimal? Vgw01
        {
            get { return this.GetProperty(Vgw01Property); }
            set { this.SetProperty(Vgw01Property, value); }
        }
        #endregion

        #region 间接人工-循环时间 Vgw02
        /// <summary>
        /// 间接人工-循环时间
        /// </summary>
        [Label("间接人工-循环时间")]
        public static readonly Property<decimal?> Vgw02Property = P<LayoutInfo>.Register(e => e.Vgw02);

        /// <summary>
        /// 间接人工-循环时间
        /// </summary>
        public decimal? Vgw02
        {
            get { return this.GetProperty(Vgw02Property); }
            set { this.SetProperty(Vgw02Property, value); }
        }
        #endregion

        #region 动力-机器时间 Vgw03
        /// <summary>
        /// 动力-机器时间
        /// </summary>
        [Label("动力-机器时间")]
        public static readonly Property<decimal?> Vgw03Property = P<LayoutInfo>.Register(e => e.Vgw03);

        /// <summary>
        /// 动力-机器时间
        /// </summary>
        public decimal? Vgw03
        {
            get { return this.GetProperty(Vgw03Property); }
            set { this.SetProperty(Vgw03Property, value); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<LayoutInfo>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #endregion
    }

    internal class LayoutInfoConfig : EntityConfig<LayoutInfo>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("LAYOUT_INFO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
