using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 维修措施保存记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("维修措施保存记录")]
    public class RepairMeasureRecord : DataEntity
    {
        #region 缺陷维修记录 Record
        /// <summary>
        /// 缺陷维修记录Id
        /// </summary>
        [Label("缺陷维修记录")]
        public static readonly IRefIdProperty RecordIdProperty =
            P<RepairMeasureRecord>.RegisterRefId(e => e.RecordId, ReferenceType.Parent);

        /// <summary>
        /// 缺陷维修记录Id
        /// </summary>
        public double RecordId
        {
            get { return (double)this.GetRefId(RecordIdProperty); }
            set { this.SetRefId(RecordIdProperty, value); }
        }

        /// <summary>
        /// 缺陷维修记录
        /// </summary>
        public static readonly RefEntityProperty<RepairDefectRecord> RecordProperty =
            P<RepairMeasureRecord>.RegisterRef(e => e.Record, RecordIdProperty);

        /// <summary>
        /// 缺陷维修记录
        /// </summary>
        public RepairDefectRecord Record
        {
            get { return this.GetRefEntity(RecordProperty); }
            set { this.SetRefEntity(RecordProperty, value); }
        }
        #endregion 

        #region 维修措施 Measure
        /// <summary>
        /// 维修措施Id
        /// </summary>
        [Label("维修措施")]
        public static readonly IRefIdProperty MeasureIdProperty =
            P<RepairMeasureRecord>.RegisterRefId(e => e.MeasureId, ReferenceType.Normal);

        /// <summary>
        /// 维修措施Id
        /// </summary>
        public double MeasureId
        {
            get { return (double)this.GetRefId(MeasureIdProperty); }
            set { this.SetRefId(MeasureIdProperty, value); }
        }

        /// <summary>
        /// 维修措施
        /// </summary>
        public static readonly RefEntityProperty<RepairMeasure> MeasureProperty =
            P<RepairMeasureRecord>.RegisterRef(e => e.Measure, MeasureIdProperty);

        /// <summary>
        /// 维修措施
        /// </summary>
        public RepairMeasure Measure
        {
            get { return this.GetRefEntity(MeasureProperty); }
            set { this.SetRefEntity(MeasureProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 维修措施保存记录 实体配置
    /// </summary>
    internal class RepairMeasureRecordConfig : EntityConfig<RepairMeasureRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_REPAIRE_MEA_RECORD").MapAllProperties();
            Meta.DisablePhantoms();
        }
    }
}