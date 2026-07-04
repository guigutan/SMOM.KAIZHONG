using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 暂存维修措施记录
    /// </summary>
    [ChildEntity, Serializable]
    public partial class MeasureRecord : DataEntity
    {
        #region 维修记录 Record
        /// <summary>
        /// 维修记录Id
        /// </summary>
        [Label("维修记录")]
        public static readonly IRefIdProperty RecordIdProperty =
            P<MeasureRecord>.RegisterRefId(e => e.RecordId, ReferenceType.Parent);

        /// <summary>
        /// 维修记录Id
        /// </summary>
        public double RecordId
        {
            get { return (double)this.GetRefId(RecordIdProperty); }
            set { this.SetRefId(RecordIdProperty, value); }
        }

        /// <summary>
        /// 维修记录
        /// </summary>
        public static readonly RefEntityProperty<RepairRecord> RecordProperty =
            P<MeasureRecord>.RegisterRef(e => e.Record, RecordIdProperty);

        /// <summary>
        /// 维修记录
        /// </summary>
        public RepairRecord Record
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
            P<MeasureRecord>.RegisterRefId(e => e.MeasureId, ReferenceType.Normal);

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
            P<MeasureRecord>.RegisterRef(e => e.Measure, MeasureIdProperty);

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
    /// 暂存维修措施记录实体配置
    /// </summary>
    internal class MeasureRecordEntityConfig : EntityConfig<MeasureRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_MEASURE_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}