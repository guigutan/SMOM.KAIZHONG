using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.InspectionStandards;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ReworkLayoutVersions
{

    /// <summary>
    /// 标签信息
    /// </summary>
    [ChildEntity, Serializable]
    [Label("标签信息")]
    public class ReworkInfoRecordDtl : DataEntity
    {
        #region 返工信息 ReworkInfoRecord
        /// <summary>
        /// 返工信息Id
        /// </summary>
        [Label("返工信息")]
        public static readonly IRefIdProperty ReworkInfoRecordIdProperty =
            P<ReworkInfoRecordDtl>.RegisterRefId(e => e.ReworkInfoRecordId, ReferenceType.Parent);

        /// <summary>
        /// 返工信息Id
        /// </summary>
        public double ReworkInfoRecordId
        {
            get { return (double)this.GetRefId(ReworkInfoRecordIdProperty); }
            set { this.SetRefId(ReworkInfoRecordIdProperty, value); }
        }

        /// <summary>
        /// 返工信息
        /// </summary>
        public static readonly RefEntityProperty<ReworkInfoRecord> ReworkInfoRecordProperty =
            P<ReworkInfoRecordDtl>.RegisterRef(e => e.ReworkInfoRecord, ReworkInfoRecordIdProperty);

        /// <summary>
        /// 返工信息
        /// </summary>
        public ReworkInfoRecord ReworkInfoRecord
        {
            get { return this.GetRefEntity(ReworkInfoRecordProperty); }
            set { this.SetRefEntity(ReworkInfoRecordProperty, value); }
        }
        #endregion

        #region 批次标签 WipBatch
        /// <summary>
        /// 批次标签Id
        /// </summary>
        [Label("批次标签")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<ReworkInfoRecordDtl>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

        /// <summary>
        /// 批次标签Id
        /// </summary>
        public double WipBatchId
        {
            get { return (double)this.GetRefId(WipBatchIdProperty); }
            set { this.SetRefId(WipBatchIdProperty, value); }
        }

        /// <summary>
        /// 批次标签
        /// </summary>
        public static readonly RefEntityProperty<WipBatch> WipBatchProperty =
            P<ReworkInfoRecordDtl>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 批次标签
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 标签号 Sn
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> SnProperty = P<ReworkInfoRecordDtl>.RegisterView(e => e.Sn, p => p.WipBatch.BatchNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
        }
        #endregion

        #region 批次数量 WipBatchQty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> WipBatchQtyProperty = P<ReworkInfoRecordDtl>.RegisterView(e => e.WipBatchQty, p => p.WipBatch.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal WipBatchQty
        {
            get { return this.GetProperty(WipBatchQtyProperty); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<ReworkInfoRecordDtl>.RegisterView(e => e.WorkOrderNo, p => p.WipBatch.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #endregion
    }

    internal class ReworkInfoRecordDtlConfig : EntityConfig<ReworkInfoRecordDtl>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                ReworkInfoRecordDtl.ReworkInfoRecordIdProperty,
                ReworkInfoRecordDtl.WipBatchIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同的批次标签".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("REWORK_INFO_RECORD_DTL").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
