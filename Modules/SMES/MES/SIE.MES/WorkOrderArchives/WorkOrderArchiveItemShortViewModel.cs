using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单缺料情况
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单制造档案缺料情况")]
    public class WorkOrderArchiveItemShortViewModel : ViewModel
    {
        #region 缺料数量 ShortQty
        /// <summary>
        /// 缺料数量
        /// </summary>
        [Label("缺料数量")]
        public static readonly Property<decimal> ShortQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.ShortQty);

        /// <summary>
        /// 缺料数量
        /// </summary>
        public decimal ShortQty
        {
            get { return this.GetProperty(ShortQtyProperty); }
            set { this.SetProperty(ShortQtyProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料消耗方式 ConsumeModel
        /// <summary>
        /// 物料消耗方式
        /// </summary>
        [Label("物料消耗方式")]
        public static readonly Property<ConsumeMode> ConsumeModelProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.ConsumeModel);

        /// <summary>
        /// 物料消耗方式
        /// </summary>
        public ConsumeMode ConsumeModel
        {
            get { return this.GetProperty(ConsumeModelProperty); }
            set { this.SetProperty(ConsumeModelProperty, value); }
        }
        #endregion

        #region 单位耗用量 SingleQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量")]
        public static readonly Property<decimal> SingleQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.SingleQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty
        {
            get { return this.GetProperty(SingleQtyProperty); }
            set { this.SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 计量单位 UnitName
        /// <summary>
        /// 计量单位
        /// </summary>
        [Label("计量单位")]
        public static readonly Property<string> UnitNameProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 计量单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 可用量(线边仓) AvailableQty
        /// <summary>
        /// 可用量(线边仓)
        /// </summary>
        [Label("可用量(线边仓)")]
        public static readonly Property<decimal> AvailableQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.AvailableQty);

        /// <summary>
        /// 可用量(线边仓)
        /// </summary>
        public decimal AvailableQty
        {
            get { return this.GetProperty(AvailableQtyProperty); }
            set { this.SetProperty(AvailableQtyProperty, value); }
        }
        #endregion

        #region 已上料量 FeedQty
        /// <summary>
        /// 已上料量
        /// </summary>
        [Label("已上料量")]
        public static readonly Property<decimal> FeedQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.FeedQty);

        /// <summary>
        /// 已上料量
        /// </summary>
        public decimal FeedQty
        {
            get { return this.GetProperty(FeedQtyProperty); }
            set { this.SetProperty(FeedQtyProperty, value); }
        }
        #endregion

        #region 已建备料量 StockOrderQty
        /// <summary>
        /// 已建备料量
        /// </summary>
        [Label("已建备料量")]
        public static readonly Property<decimal> StockOrderQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.StockOrderQty);

        /// <summary>
        /// 已建备料量
        /// </summary>
        public decimal StockOrderQty
        {
            get { return this.GetProperty(StockOrderQtyProperty); }
            set { this.SetProperty(StockOrderQtyProperty, value); }
        }
        #endregion

        #region 备料待接收量 ToTakeQty
        /// <summary>
        /// 备料待接收量
        /// </summary>
        [Label("备料待接收量")]
        public static readonly Property<decimal> ToTakeQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.ToTakeQty);

        /// <summary>
        /// 备料待接收量
        /// </summary>
        public decimal ToTakeQty
        {
            get { return this.GetProperty(ToTakeQtyProperty); }
            set { this.SetProperty(ToTakeQtyProperty, value); }
        }
        #endregion

        #region 相同线边仓工单剩余需求数量 SameNeedQty
        /// <summary>
        /// 相同线边仓工单剩余需求数量
        /// </summary>
        [Label("相同线边仓工单剩余需求数量")]
        public static readonly Property<decimal> SameNeedQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.SameNeedQty);

        /// <summary>
        /// 相同线边仓工单剩余需求数量
        /// </summary>
        public decimal SameNeedQty
        {
            get { return this.GetProperty(SameNeedQtyProperty); }
            set { this.SetProperty(SameNeedQtyProperty, value); }
        }
        #endregion

        #region 工单总需求数量 TotalNeedQty
        /// <summary>
        /// 工单总需求数量
        /// </summary>
        [Label("工单总需求数量")]
        public static readonly Property<decimal> TotalNeedQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.TotalNeedQty);

        /// <summary>
        /// 工单总需求数量
        /// </summary>
        public decimal TotalNeedQty
        {
            get { return this.GetProperty(TotalNeedQtyProperty); }
            set { this.SetProperty(TotalNeedQtyProperty, value); }
        }
        #endregion

        #region 工单已耗用数量 HasCostQty
        /// <summary>
        /// 工单已耗用数量
        /// </summary>
        [Label("工单已耗用数量")]
        public static readonly Property<decimal> HasCostQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.HasCostQty);

        /// <summary>
        /// 工单已耗用数量
        /// </summary>
        public decimal HasCostQty
        {
            get { return this.GetProperty(HasCostQtyProperty); }
            set { this.SetProperty(HasCostQtyProperty, value); }
        }
        #endregion

        #region 工单剩余需求数量 ResidueNeedQty
        /// <summary>
        /// 工单剩余需求数量
        /// </summary>
        [Label("工单剩余需求数量")]
        public static readonly Property<decimal> ResidueNeedQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.ResidueNeedQty);

        /// <summary>
        /// 工单剩余需求数量
        /// </summary>
        public decimal ResidueNeedQty
        {
            get { return this.GetProperty(ResidueNeedQtyProperty); }
            set { this.SetProperty(ResidueNeedQtyProperty, value); }
        }
        #endregion

        #region 满足套数 SetQty
        /// <summary>
        /// 满足套数
        /// </summary>
        [Label("满足套数")]
        public static readonly Property<decimal> SetQtyProperty = P<WorkOrderArchiveItemShortViewModel>.Register(e => e.SetQty);

        /// <summary>
        /// 满足套数
        /// </summary>
        public decimal SetQty
        {
            get { return this.GetProperty(SetQtyProperty); }
            set { this.SetProperty(SetQtyProperty, value); }
        }
        #endregion

    }
}
