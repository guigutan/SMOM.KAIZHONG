using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 线边仓盘点单工单调账记录
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("线边仓盘点单工单调账记录")]
    public partial class LesStockCountWorkOrder : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        [MaxLength(80)]
        public static readonly Property<string> LineNoProperty = P<LesStockCountWorkOrder>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<LesStockCountWorkOrder>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 现有量 Qty
        /// <summary>
        /// 现有量
        /// </summary>
        [Label("现有量")]
        public static readonly Property<decimal?> QtyProperty = P<LesStockCountWorkOrder>.Register(e => e.Qty);

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 实际数量 ActualCountQty
        /// <summary>
        /// 实际数量
        /// </summary>
        [MinValue(0)]
        [Label("实际数量")]
        public static readonly Property<decimal?> ActualCountQtyProperty = P<LesStockCountWorkOrder>.Register(e => e.ActualCountQty);

        /// <summary>
        /// 实际数量
        /// </summary>
        public decimal? ActualCountQty
        {
            get { return GetProperty(ActualCountQtyProperty); }
            set { SetProperty(ActualCountQtyProperty, value); }
        }
        #endregion

        #region 差异数量 DiffCountQty
        /// <summary>
        /// 差异数量
        /// </summary>
        [Label("差异数量")]
        public static readonly Property<decimal?> DiffCountQtyProperty = P<LesStockCountWorkOrder>.Register(e => e.DiffCountQty);

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal? DiffCountQty
        {
            get { return GetProperty(DiffCountQtyProperty); }
            set { SetProperty(DiffCountQtyProperty, value); }
        }
        #endregion

        #region 盘点单 LesStockCount
        /// <summary>
        /// 盘点单Id
        /// </summary>
        [Label("盘点单")]
        public static readonly IRefIdProperty LesStockCountIdProperty = P<LesStockCountWorkOrder>.RegisterRefId(e => e.LesStockCountId, ReferenceType.Parent);

        /// <summary>
        /// 盘点单Id
        /// </summary>
        public double LesStockCountId
        {
            get { return (double)GetRefId(LesStockCountIdProperty); }
            set { SetRefId(LesStockCountIdProperty, value); }
        }

        /// <summary>
        /// 盘点单
        /// </summary>
        public static readonly RefEntityProperty<LesStockCount> LesStockCountProperty = P<LesStockCountWorkOrder>.RegisterRef(e => e.LesStockCount, LesStockCountIdProperty);

        /// <summary>
        /// 盘点单
        /// </summary>
        public LesStockCount LesStockCount
        {
            get { return GetRefEntity(LesStockCountProperty); }
            set { SetRefEntity(LesStockCountProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 盘点单明细 实体配置
    /// </summary>
    internal class LesStockCountWorkOrderConfig : EntityConfig<LesStockCountWorkOrder>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_STOCK_COUNT_WO").MapAllProperties();   
            Meta.EnablePhantoms();
        }
    }
}