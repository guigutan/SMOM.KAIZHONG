using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序与缺陷
    /// </summary>
    [ChildEntity, Serializable]
    [Label("缺陷信息")]
    [DisplayMember(nameof(Id))]
    public partial class ProcessDefect : DataEntity
    {
        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty = P<ProcessDefect>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty = P<ProcessDefect>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessDefect>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessDefect>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 缺陷描述 DefectDescription
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescriptionProperty = P<ProcessDefect>.RegisterView(e => e.DefectDescription, p => p.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDescription
        {
            get { return this.GetProperty(DefectDescriptionProperty); }
        }
        #endregion 

        #region 质量类型 QualityType
        /// <summary>
        /// 质量类型
        /// </summary>
        [Label("质量类型")]
        public static readonly Property<QualityType> QualityTypeProperty = P<ProcessDefect>.RegisterView(e => e.QualityType, p => p.Defect.QualityType);

        /// <summary>
        /// 质量类型
        /// </summary>
        public QualityType QualityType
        {
            get { return this.GetProperty(QualityTypeProperty); }
        }
        #endregion

        #region 缺陷等级 DefectLevel
        /// <summary>
        /// 缺陷等级
        /// </summary>
        [Label("缺陷等级")]
        public static readonly Property<string> DefectLevelProperty = P<ProcessDefect>.RegisterView(e => e.DefectLevel, p => p.Defect.DefectGrade.Name);

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string DefectLevel
        {
            get { return this.GetProperty(DefectLevelProperty); }
        }
        #endregion

        #region 分类编码 CategoryCode
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> CategoryCodeProperty = P<ProcessDefect>.RegisterView(e => e.CategoryCode, p => p.Defect.DefectCategory.Code);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode
        {
            get { return this.GetProperty(CategoryCodeProperty); }
        }
        #endregion

        #region 分类描述 CategoryDescription
        /// <summary>
        /// 分类描述
        /// </summary>
        [Label("分类描述")]
        public static readonly Property<string> CategoryDescriptionProperty = P<ProcessDefect>.RegisterView(e => e.CategoryDescription, p => p.Defect.DefectCategory.Description);

        /// <summary>
        /// 分类描述
        /// </summary>
        public string CategoryDescription
        {
            get { return this.GetProperty(CategoryDescriptionProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工序与缺陷 实体配置
    /// </summary>
    internal class ProcessDefectConfig : EntityConfig<ProcessDefect>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROC_DEF").MapAllProperties();
            Meta.Property(ProcessDefect.ProcessIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}