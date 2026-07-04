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
    /// 余料称重记录修改记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("余料称重记录修改记录")]
    public class ScrapWeighingRecordEditLog : DataEntity
    {
        #region 余料称重记录 ScrapWeighingRecord
        /// <summary>
        /// 余料称重记录Id
        /// </summary>
        [Label("余料称重记录")]
        public static readonly IRefIdProperty ScrapWeighingRecordIdProperty =
            P<ScrapWeighingRecordEditLog>.RegisterRefId(e => e.ScrapWeighingRecordId, ReferenceType.Normal);

        /// <summary>
        /// 余料称重记录Id
        /// </summary>
        public double ScrapWeighingRecordId
        {
            get { return (double)this.GetRefId(ScrapWeighingRecordIdProperty); }
            set { this.SetRefId(ScrapWeighingRecordIdProperty, value); }
        }

        /// <summary>
        /// 余料称重记录
        /// </summary>
        public static readonly RefEntityProperty<ScrapWeighingRecord> ScrapWeighingRecordProperty =
            P<ScrapWeighingRecordEditLog>.RegisterRef(e => e.ScrapWeighingRecord, ScrapWeighingRecordIdProperty);

        /// <summary>
        /// 余料称重记录
        /// </summary>
        public ScrapWeighingRecord ScrapWeighingRecord
        {
            get { return this.GetRefEntity(ScrapWeighingRecordProperty); }
            set { this.SetRefEntity(ScrapWeighingRecordProperty, value); }
        }
        #endregion

        #region 原修改实际称重数量 OldEditQty
        /// <summary>
        /// 原修改实际称重数量
        /// </summary>
        [Label("原修改实际称重数量")]
        public static readonly Property<decimal?> OldEditQtyProperty = P<ScrapWeighingRecordEditLog>.Register(e => e.OldEditQty);

        /// <summary>
        /// 原修改实际称重数量
        /// </summary>
        public decimal? OldEditQty
        {
            get { return this.GetProperty(OldEditQtyProperty); }
            set { this.SetProperty(OldEditQtyProperty, value); }
        }
        #endregion

        #region 新修改实际称重数量 NewEditQty
        /// <summary>
        /// 新修改实际称重数量
        /// </summary>
        [Label("新修改实际称重数量")]
        public static readonly Property<decimal?> NewEditQtyProperty = P<ScrapWeighingRecordEditLog>.Register(e => e.NewEditQty);

        /// <summary>
        /// 新修改实际称重数量
        /// </summary>
        public decimal? NewEditQty
        {
            get { return this.GetProperty(NewEditQtyProperty); }
            set { this.SetProperty(NewEditQtyProperty, value); }
        }
        #endregion

    }

    internal class ScrapWeighingRecordEditLogConfig : EntityConfig<ScrapWeighingRecordEditLog>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("SCRAP_WEIGHING_REC_EDIT_LOG").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
