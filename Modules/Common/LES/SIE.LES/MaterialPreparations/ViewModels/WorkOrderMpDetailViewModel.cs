using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations.ViewModels
{
    /// <summary>
    /// 工单备料汇总明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单备料汇总明细")]
    public class WorkOrderMpDetailViewModel : Entity<double>
    {
        #region 工单Id WoId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WoIdProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.WoId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId
        {
            get { return this.GetProperty(WoIdProperty); }
            set { this.SetProperty(WoIdProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 物料消耗方式 ConsumeMode
        /// <summary>
        /// 物料消耗方式
        /// </summary>
        [Label("物料消耗方式")]
        public static readonly Property<ConsumeMode> ConsumeModeProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 物料消耗方式
        /// </summary>
        public ConsumeMode ConsumeMode
        {
            get { return this.GetProperty(ConsumeModeProperty); }
            set { this.SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性值 ItemExtPropName
        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        [Label("物料扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 需求量 BomNeedQty
        /// <summary>
        /// 需求量
        /// </summary>
        [Label("需求量")]
        public static readonly Property<decimal> BomNeedQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.BomNeedQty);

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal BomNeedQty
        {
            get { return this.GetProperty(BomNeedQtyProperty); }
            set { this.SetProperty(BomNeedQtyProperty, value); }
        }
        #endregion

        #region 已建备料数 HasQty
        /// <summary>
        /// 已建备料数
        /// </summary>
        [Label("已建备料数")]
        public static readonly Property<decimal> HasQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.HasQty);

        /// <summary>
        /// 已建备料数
        /// </summary>
        public decimal HasQty
        {
            get { return this.GetProperty(HasQtyProperty); }
            set { this.SetProperty(HasQtyProperty, value); }
        }
        #endregion

        #region 可备料数 CanPrepareQty
        /// <summary>
        /// 可备料数
        /// </summary>
        [Label("可备料数")]
        public static readonly Property<decimal> CanPrepareQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.CanPrepareQty);

        /// <summary>
        /// 可备料数
        /// </summary>
        public decimal CanPrepareQty
        {
            get { return this.GetProperty(CanPrepareQtyProperty); }
            set { this.SetProperty(CanPrepareQtyProperty, value); }
        }
        #endregion

        #region 已接收数 HasReceiveQty
        /// <summary>
        /// 已接收数
        /// </summary>
        [Label("已接收数")]
        public static readonly Property<decimal> HasReceiveQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.HasReceiveQty);

        /// <summary>
        /// 已接收数
        /// </summary>
        public decimal HasReceiveQty
        {
            get { return this.GetProperty(HasReceiveQtyProperty); }
            set { this.SetProperty(HasReceiveQtyProperty, value); }
        }
        #endregion

        #region 待接收数 ToReceiveQty
        /// <summary>
        /// 待接收数
        /// </summary>
        [Label("待接收数")]
        public static readonly Property<decimal> ToReceiveQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.ToReceiveQty);

        /// <summary>
        /// 待接收数
        /// </summary>
        public decimal ToReceiveQty
        {
            get { return this.GetProperty(ToReceiveQtyProperty); }
            set { this.SetProperty(ToReceiveQtyProperty, value); }
        }
        #endregion

        #region 已发料数 HasShippingQty
        /// <summary>
        /// 已发料数
        /// </summary>
        [Label("已发料数")]
        public static readonly Property<decimal> HasShippingQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.HasShippingQty);

        /// <summary>
        /// 已发料数
        /// </summary>
        public decimal HasShippingQty
        {
            get { return this.GetProperty(HasShippingQtyProperty); }
            set { this.SetProperty(HasShippingQtyProperty, value); }
        }
        #endregion

        #region 取消数 CancelQty
        /// <summary>
        /// 取消数
        /// </summary>
        [Label("取消数")]
        public static readonly Property<decimal> CancelQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.CancelQty);

        /// <summary>
        /// 取消数
        /// </summary>
        public decimal CancelQty
        {
            get { return this.GetProperty(CancelQtyProperty); }
            set { this.SetProperty(CancelQtyProperty, value); }
        }
        #endregion

        #region 退料数 ReturnQty
        /// <summary>
        /// 退料数
        /// </summary>
        [Label("退料数")]
        public static readonly Property<decimal> ReturnQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.ReturnQty);

        /// <summary>
        /// 退料数
        /// </summary>
        public decimal ReturnQty
        {
            get { return this.GetProperty(ReturnQtyProperty); }
            set { this.SetProperty(ReturnQtyProperty, value); }
        }
        #endregion

        #region 挪入数 MoveInQty
        /// <summary>
        /// 挪入数
        /// </summary>
        [Label("挪入数")]
        public static readonly Property<decimal> MoveInQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.MoveInQty);

        /// <summary>
        /// 挪入数
        /// </summary>
        public decimal MoveInQty
        {
            get { return this.GetProperty(MoveInQtyProperty); }
            set { this.SetProperty(MoveInQtyProperty, value); }
        }
        #endregion

        #region 挪出数 MoveOutQty
        /// <summary>
        /// 挪出数
        /// </summary>
        [Label("挪出数")]
        public static readonly Property<decimal> MoveOutQtyProperty = P<WorkOrderMpDetailViewModel>.Register(e => e.MoveOutQty);

        /// <summary>
        /// 挪出数
        /// </summary>
        public decimal MoveOutQty
        {
            get { return this.GetProperty(MoveOutQtyProperty); }
            set { this.SetProperty(MoveOutQtyProperty, value); }
        }
        #endregion

    }
}
