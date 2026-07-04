using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 合并时间段
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("合并时间段")]
    public partial class StockOrderMergeTimes : DataEntity
    {       
        #region 起始需求时间 StartTime
        /// <summary>
        /// 起始需求时间
        /// </summary>
        [Label("起始需求时间")]
        [Required]
        public static readonly Property<DateTime> StartTimeProperty = P<StockOrderMergeTimes>.Register(e => e.StartTime);

        /// <summary>
        /// 起始需求时间
        /// </summary>
        public DateTime StartTime
        {
            get => GetProperty(StartTimeProperty);
            set => SetProperty(StartTimeProperty, value);
        }
        #endregion

        #region 结束需求时间 EndTime
        /// <summary>
        /// 结束需求时间
        /// </summary>
        [Label("结束需求时间")]
        [Required]
        public static readonly Property<DateTime> EndTimeProperty = P<StockOrderMergeTimes>.Register(e => e.EndTime);

        /// <summary>
        /// 结束需求时间
        /// </summary>
        public DateTime EndTime
        {
            get => GetProperty(EndTimeProperty);
            set => SetProperty(EndTimeProperty, value);
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<StockOrderMergeTimes>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 合并下发规则和合并时间段的关系 StockOrderMergeIssued
        /// <summary>
        /// 合并下发规则和合并时间段的关系Id
        /// </summary>
        public static readonly IRefIdProperty StockOrderMergeIssuedIdProperty = P<StockOrderMergeTimes>.RegisterRefId(e => e.StockOrderMergeIssuedId, ReferenceType.Parent);

        /// <summary>
        /// 合并下发规则和合并时间段的关系Id
        /// </summary>
        public double StockOrderMergeIssuedId
        {
            get => (double)GetRefId(StockOrderMergeIssuedIdProperty);
            set => SetRefId(StockOrderMergeIssuedIdProperty, value);
        }

        /// <summary>
        /// 合并下发规则和合并时间段的关系
        /// </summary>
        public static readonly RefEntityProperty<StockOrderMergeIssued> StockOrderMergeIssuedProperty = P<StockOrderMergeTimes>.RegisterRef(e => e.StockOrderMergeIssued, StockOrderMergeIssuedIdProperty);

        /// <summary>
        /// 合并下发规则和合并时间段的关系
        /// </summary>
        public StockOrderMergeIssued StockOrderMergeIssued
        {
            get => GetRefEntity(StockOrderMergeIssuedProperty);
            set => SetRefEntity(StockOrderMergeIssuedProperty, value);
        }
        #endregion

        #region 起始需求时间 Start
        /// <summary>
        /// 起始需求时间
        /// </summary>
        [Label("起始需求时间")]
        public static readonly Property<int> StartProperty = P<StockOrderMergeTimes>.RegisterReadOnly(
            e => e.Start, e => e.GetStart());
        /// <summary>
        /// 起始需求时间
        /// </summary>

        public int Start => GetProperty(StartProperty);
        private int GetStart()
        {
            return int.Parse(StartTime.ToString("HHmmss"));
        }
        #endregion

        #region 结束需求时间 End
        /// <summary>
        /// 结束需求时间
        /// </summary>
        [Label("结束需求时间")]
        public static readonly Property<int> EndProperty = P<StockOrderMergeTimes>.RegisterReadOnly(
            e => e.End, e => e.GetEnd());
        /// <summary>
        /// 结束需求时间
        /// </summary>

        public int End => GetProperty(EndProperty);
        private int GetEnd()
        {
            return int.Parse(EndTime.ToString("HHmmss"));
        }
        #endregion

        #region 是否跨日 IsCrossDay
        /// <summary>
        /// 是否跨日
        /// </summary>
        [Label("是否跨日")]
        public static readonly Property<bool> IsCrossDayProperty = P<StockOrderMergeTimes>.RegisterReadOnly(
            e => e.IsCrossDay, e => e.GetIsCrossDay());
        /// <summary>
        /// 是否跨日
        /// </summary>

        public bool IsCrossDay => GetProperty(IsCrossDayProperty);
        private bool GetIsCrossDay()
        {
            return Start > End;
        }
        #endregion        

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<StockOrderMergeTimes>.RegisterView(e => e.State, p => p.StockOrderMergeIssued.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State => GetProperty(StateProperty);
        #endregion

        #region 起始时间 StartTimeText
        /// <summary>
        /// 起始时间
        /// </summary>
        [Label("起始需求时间")]
        public static readonly Property<string> StartTimeTextProperty = P<StockOrderMergeTimes>.Register(e => e.StartTimeText);

        /// <summary>
        /// 起始时间
        /// </summary>
        public string StartTimeText
        {
            get { return this.GetProperty(StartTimeTextProperty); }
            set { this.SetProperty(StartTimeTextProperty, value); }
        }
        #endregion

        #region 结束时间 EndTimeText
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束需求时间")]
        public static readonly Property<string> EndTimeTextProperty = P<StockOrderMergeTimes>.Register(e => e.EndTimeText);

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTimeText
        {
            get { return this.GetProperty(EndTimeTextProperty); }
            set { this.SetProperty(EndTimeTextProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 合并时间段 实体配置
    /// </summary>
    internal class StockOrderMergeTimesConfig : EntityConfig<StockOrderMergeTimes>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("STOCK_ORDER_MERGE_TIMES").MapAllProperties();
            Meta.Property(StockOrderMergeTimes.StartTimeTextProperty).DontMapColumn();
            Meta.Property(StockOrderMergeTimes.EndTimeTextProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }

}
