using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.StockDeducRecords
{
    /// <summary>
    /// 扣料明细
    /// </summary>
    [ChildEntity,Serializable]
    [Label("扣料明细")]
    public class StockDeducRecordDetail : DataEntity
    {
        #region 扣料记录 StockDeducRecord
        /// <summary>
        /// 扣料记录Id
        /// </summary>
        [Label("扣料记录")]
        public static readonly IRefIdProperty StockDeducRecordIdProperty =
            P<StockDeducRecordDetail>.RegisterRefId(e => e.StockDeducRecordId, ReferenceType.Parent);

        /// <summary>
        /// 扣料记录Id
        /// </summary>
        public double StockDeducRecordId
        {
            get { return (double)this.GetRefId(StockDeducRecordIdProperty); }
            set { this.SetRefId(StockDeducRecordIdProperty, value); }
        }

        /// <summary>
        /// 扣料记录
        /// </summary>
        public static readonly RefEntityProperty<StockDeducRecord> StockDeducRecordProperty =
            P<StockDeducRecordDetail>.RegisterRef(e => e.StockDeducRecord, StockDeducRecordIdProperty);

        /// <summary>
        /// 扣料记录
        /// </summary>
        public StockDeducRecord StockDeducRecord
        {
            get { return this.GetRefEntity(StockDeducRecordProperty); }
            set { this.SetRefEntity(StockDeducRecordProperty, value); }
        }
        #endregion

        #region 物料标签 ItemLabel
        /// <summary>
        /// 物料标签
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemLabelProperty = P<StockDeducRecordDetail>.Register(e => e.ItemLabel);

        /// <summary>
        /// 物料标签
        /// </summary>
        public string ItemLabel
        {
            get { return this.GetProperty(ItemLabelProperty); }
            set { this.SetProperty(ItemLabelProperty, value); }
        }
        #endregion

        #region 扣料数量 DeductedQty
        /// <summary>
        /// 扣料数量
        /// </summary>
        [Label("扣料数量")]
        public static readonly Property<decimal> DeductedQtyProperty = P<StockDeducRecordDetail>.Register(e => e.DeductedQty);

        /// <summary>
        /// 扣料数量
        /// </summary>
        public decimal DeductedQty
        {
            get { return this.GetProperty(DeductedQtyProperty); }
            set { this.SetProperty(DeductedQtyProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StockDeducRecordDetail>.RegisterView(e => e.ItemCode, p => p.StockDeducRecord.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<StockDeducRecordDetail>.RegisterView(e => e.ItemName, p => p.StockDeducRecord.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #endregion
    }

    internal class StockDeducRecordDetailConfig : EntityConfig<StockDeducRecordDetail>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("STOCK_DEDUC_RECORD_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
