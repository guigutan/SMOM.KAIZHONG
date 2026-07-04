using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 暂存缺陷责任记录
    /// </summary>
    [ChildEntity, Serializable]
    public partial class ResponsibilityRecord : DataEntity
    {
        #region 维修记录 Record
        /// <summary>
        /// 维修记录Id
        /// </summary>
        [Label("维修记录")]
        public static readonly IRefIdProperty RecordIdProperty =
            P<ResponsibilityRecord>.RegisterRefId(e => e.RecordId, ReferenceType.Parent);

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
            P<ResponsibilityRecord>.RegisterRef(e => e.Record, RecordIdProperty);

        /// <summary>
        /// 维修记录
        /// </summary>
        public RepairRecord Record
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
            P<ResponsibilityRecord>.RegisterRefId(e => e.ResponsibilityId, ReferenceType.Normal);

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
            P<ResponsibilityRecord>.RegisterRef(e => e.Responsibility, ResponsibilityIdProperty);

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
    /// 暂存缺陷责任记录实体配置
    /// </summary>
    internal class ResponsibilityRecordEntityConfig : EntityConfig<ResponsibilityRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_RESPON_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}