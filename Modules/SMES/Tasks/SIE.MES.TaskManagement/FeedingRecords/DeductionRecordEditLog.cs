using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 扣料记录修改日志s
    /// </summary>
    [RootEntity, Serializable]
    [Label("扣料记录修改日志")]
    public class DeductionRecordEditLog : DataEntity
    {
        #region 扣料记录 DeductionRecord
        /// <summary>
        /// 扣料记录Id
        /// </summary>
        [Label("扣料记录")]
        public static readonly IRefIdProperty DeductionRecordIdProperty =
            P<DeductionRecordEditLog>.RegisterRefId(e => e.DeductionRecordId, ReferenceType.Normal);

        /// <summary>
        /// 扣料记录Id
        /// </summary>
        public double DeductionRecordId
        {
            get { return (double)this.GetRefId(DeductionRecordIdProperty); }
            set { this.SetRefId(DeductionRecordIdProperty, value); }
        }

        /// <summary>
        /// 扣料记录
        /// </summary>
        public static readonly RefEntityProperty<DeductionRecord> DeductionRecordProperty =
            P<DeductionRecordEditLog>.RegisterRef(e => e.DeductionRecord, DeductionRecordIdProperty);

        /// <summary>
        /// 扣料记录
        /// </summary>
        public DeductionRecord DeductionRecord
        {
            get { return this.GetRefEntity(DeductionRecordProperty); }
            set { this.SetRefEntity(DeductionRecordProperty, value); }
        }
        #endregion

        #region 原修改数量 OldEditQty
        /// <summary>
        /// 原修改数量
        /// </summary>
        [Label("原修改数量")]
        public static readonly Property<decimal?> OldEditQtyProperty = P<DeductionRecordEditLog>.Register(e => e.OldEditQty);

        /// <summary>
        /// 原修改数量
        /// </summary>
        public decimal? OldEditQty
        {
            get { return this.GetProperty(OldEditQtyProperty); }
            set { this.SetProperty(OldEditQtyProperty, value); }
        }
        #endregion

        #region 新修改数量 NewEditQty
        /// <summary>
        /// 新修改数量
        /// </summary>
        [Label("新修改数量")]
        public static readonly Property<decimal?> NewEditQtyProperty = P<DeductionRecordEditLog>.Register(e => e.NewEditQty);

        /// <summary>
        /// 新修改数量
        /// </summary>
        public decimal? NewEditQty
        {
            get { return this.GetProperty(NewEditQtyProperty); }
            set { this.SetProperty(NewEditQtyProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 上料标签 FeedingItemLabel
        /// <summary>
        /// 上料标签
        /// </summary>
        [Label("上料标签")]
        public static readonly Property<string> FeedingItemLabelProperty = P<DeductionRecordEditLog>.RegisterView(e => e.FeedingItemLabel, p => p.DeductionRecord.FeedingItemLabel);

        /// <summary>
        /// 上料标签
        /// </summary>
        public string FeedingItemLabel
        {
            get { return this.GetProperty(FeedingItemLabelProperty); }
        }
        #endregion

        #region 标签批次 ItemLabelLot
        /// <summary>
        /// 标签批次
        /// </summary>
        [Label("标签批次")]
        public static readonly Property<string> ItemLabelLotProperty = P<DeductionRecordEditLog>.RegisterView(e => e.ItemLabelLot, p => p.DeductionRecord.ItemLabel.Lot);

        /// <summary>
        /// 标签批次
        /// </summary>
        public string ItemLabelLot
        {
            get { return this.GetProperty(ItemLabelLotProperty); }
        }
        #endregion

        #endregion
    }

    internal class DeductionRecordEditLogConfig : EntityConfig<DeductionRecordEditLog>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("DEDU_RECORD_EDIT_LOG").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
