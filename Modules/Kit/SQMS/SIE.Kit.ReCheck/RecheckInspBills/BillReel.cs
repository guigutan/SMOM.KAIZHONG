using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Recheck.RecheckInspBills;
using System;

namespace SIE.Kit.ReCheck.RecheckInspBills
{
    /// <summary>
    /// 单据Reel
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("单据Reel")]
    public partial class BillReel : DataEntity
    {
        #region ReelID ReelId
        /// <summary>
        /// ReelID
        /// </summary>
        [Required]
        [Label("ReelID")]
        public static readonly Property<string> ReelIdProperty = P<BillReel>.Register(e => e.ReelId);

        /// <summary>
        /// ReelID
        /// </summary>
        public string ReelId
        {
            get { return GetProperty(ReelIdProperty); }
            set { SetProperty(ReelIdProperty, value); }
        }
        #endregion

        #region 数量 Quannity
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        [Label("数量")]
        public static readonly Property<decimal?> QuannityProperty = P<BillReel>.Register(e => e.Quannity);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Quannity
        {
            get { return GetProperty(QuannityProperty); }
            set { SetProperty(QuannityProperty, value); }
        }
        #endregion

        #region 超期复检单 RecheckInspBillId
        /// <summary>
        /// 超期复检单Id
        /// </summary>
        [Label("超期复检单")]
        public static readonly IRefIdProperty IqcBillIdProperty = P<BillReel>.RegisterRefId(e => e.RecheckInspBillId, ReferenceType.Parent);

        /// <summary>
        /// 超期复检单Id
        /// </summary>
        public double RecheckInspBillId
        {
            get { return (double)GetRefNullableId(IqcBillIdProperty); }
            set { SetRefNullableId(IqcBillIdProperty, value); }
        }

        /// <summary>
        /// 超期复检单
        /// </summary>
        public static readonly RefEntityProperty<RecheckInspBill> IqcBillProperty = P<BillReel>.RegisterRef(e => e.RecheckInspBill, IqcBillIdProperty);

        /// <summary>
        /// 超期复检单
        /// </summary>
        public RecheckInspBill RecheckInspBill
        {
            get { return GetRefEntity(IqcBillProperty); }
            set { SetRefEntity(IqcBillProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 单据Reel 实体配置
    /// </summary>
    internal class BillReelConfig : EntityConfig<BillReel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RECHECKINSPBILL_REEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}