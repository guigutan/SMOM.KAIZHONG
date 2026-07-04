using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 维修责任保存记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("维修责任保存记录")]
    public class RepairResponseRecord : DataEntity
    {
        #region 缺陷维修记录 Record
        /// <summary>
        /// 缺陷维修记录Id
        /// </summary>
        [Label("缺陷维修记录")]
        public static readonly IRefIdProperty RecordIdProperty =
            P<RepairResponseRecord>.RegisterRefId(e => e.RecordId, ReferenceType.Parent);

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
            P<RepairResponseRecord>.RegisterRef(e => e.Record, RecordIdProperty);

        /// <summary>
        /// 缺陷维修记录
        /// </summary>
        public RepairDefectRecord Record
        {
            get { return this.GetRefEntity(RecordProperty); }
            set { this.SetRefEntity(RecordProperty, value); }
        }
        #endregion 

        #region 责任 Responsibility
        /// <summary>
        /// 责任Id
        /// </summary>
        [Label("责任")]
        public static readonly IRefIdProperty ResponsibilityIdProperty =
            P<RepairResponseRecord>.RegisterRefId(e => e.ResponsibilityId, ReferenceType.Normal);

        /// <summary>
        /// 责任Id
        /// </summary>
        public double ResponsibilityId
        {
            get { return (double)this.GetRefId(ResponsibilityIdProperty); }
            set { this.SetRefId(ResponsibilityIdProperty, value); }
        }

        /// <summary>
        /// 责任
        /// </summary>
        public static readonly RefEntityProperty<DefectResponsibility> ResponsibilityProperty =
            P<RepairResponseRecord>.RegisterRef(e => e.Responsibility, ResponsibilityIdProperty);

        /// <summary>
        /// 责任
        /// </summary>
        public DefectResponsibility Responsibility
        {
            get { return this.GetRefEntity(ResponsibilityProperty); }
            set { this.SetRefEntity(ResponsibilityProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 维修责任保存记录 实体配置
    /// </summary>
    internal class RepairResponseRecordConfig : EntityConfig<RepairResponseRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_REPAIRE_RSP_RECORD").MapAllProperties();
            Meta.DisablePhantoms();
        }
    }
}
