using SIE.Defects.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects
{
    /// <summary>
    /// 缺陷等级
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("缺陷等级")]
    [DisplayMember(nameof(Name))]
    public partial class DefectGrade : DataEntity
    {

        #region 缺陷等级 Name
        /// <summary>
        /// 缺陷等级
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("缺陷等级")]
        public static readonly Property<string> NameProperty = P<DefectGrade>.Register(e => e.Name);

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 严重度 DefectSeverity
        /// <summary>
        /// 严重度
        /// </summary>
        [Required]
        [Label("严重度")]
        public static readonly Property<DefectSeverity> AssociationProperty = P<DefectGrade>.Register(e => e.DefectSeverity);

        /// <summary>
        /// 严重度
        /// </summary>
        public DefectSeverity DefectSeverity
        {
            get { return GetProperty(AssociationProperty); }
            set { SetProperty(AssociationProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 缺陷等级 实体配置
    /// </summary>
    internal class DefectGradeConfig : EntityConfig<DefectGrade>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DEF_DEFECTGRADE").MapAllProperties();
            Meta.Property(DefectGrade.NameProperty).ColumnMeta.HasLength(120);
            Meta.EnablePhantoms();
        }
    }
}