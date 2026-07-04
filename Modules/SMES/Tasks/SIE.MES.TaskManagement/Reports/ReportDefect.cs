using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工缺陷
    /// </summary>
    [ChildEntity, Serializable]
    [Label("报工缺陷")]
    public partial class ReportDefect : DataEntity
    {
        #region 缺陷代码 Defect
        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        [Label("缺陷代码")]
        public static readonly IRefIdProperty DefectIdProperty = P<ReportDefect>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty = P<ReportDefect>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public Defect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 报工记录 ReportRecord
        /// <summary>
        /// 报工记录Id
        /// </summary>
        [Label("报工记录")]
        public static readonly IRefIdProperty ReportRecordIdProperty = P<ReportDefect>.RegisterRefId(e => e.ReportRecordId, ReferenceType.Parent);

        /// <summary>
        /// 报工记录Id
        /// </summary>
        public double ReportRecordId
        {
            get { return (double)GetRefId(ReportRecordIdProperty); }
            set { SetRefId(ReportRecordIdProperty, value); }
        }

        /// <summary>
        /// 报工记录
        /// </summary>
        public static readonly RefEntityProperty<ReportRecord> ReportRecordProperty = P<ReportDefect>.RegisterRef(e => e.ReportRecord, ReportRecordIdProperty);

        /// <summary>
        /// 报工记录
        /// </summary>
        public ReportRecord ReportRecord
        {
            get { return GetRefEntity(ReportRecordProperty); }
            set { SetRefEntity(ReportRecordProperty, value); }
        }
        #endregion

        #region 缺陷代码描述 DefectDescription
        /// <summary>
        /// 缺陷代码描述
        /// </summary>
        [Label("缺陷代码描述")]
        public static readonly Property<string> DefectDescriptionProperty = P<ReportDefect>.RegisterView(e => e.DefectDescription, p => p.Defect.Description);

        /// <summary>
        /// 缺陷代码描述
        /// </summary>
        public string DefectDescription
        {
            get { return this.GetProperty(DefectDescriptionProperty); }
        }
        #endregion 

    }

    /// <summary>
    /// 报工缺陷 实体配置
    /// </summary>
    internal class ReportDefectConfig : EntityConfig<ReportDefect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_REPORT_DEFECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}