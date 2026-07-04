using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.ProcessTechTypes
{
    /// <summary>
    /// 制程工艺类型
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("制程工艺类型")]
    [DisplayMember(nameof(Name))]
    public partial class ProcessTechType : DataEntity
    {
        #region 编号 Code
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("编号")]
        public static readonly Property<string> CodeProperty = P<ProcessTechType>.Register(e => e.Code);

        /// <summary>
        /// 编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(200)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ProcessTechType>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 算法类型 AlgorithmMarking
        /// <summary>
        /// 算法类型
        /// </summary>
        [Label("算法类型")]
        public static readonly Property<AlgorithmMarking> AlgorithmMarkingProperty = P<ProcessTechType>.Register(e => e.AlgorithmMarking);

        /// <summary>
        /// 算法类型
        /// </summary>
        public AlgorithmMarking AlgorithmMarking
        {
            get { return GetProperty(AlgorithmMarkingProperty); }
            set { SetProperty(AlgorithmMarkingProperty, value); }
        }
        #endregion

        #region 显示顺序 Sequence
        /// <summary>
        /// 显示顺序
        /// </summary>
        [Label("显示顺序")]
        public static readonly Property<int?> SequenceProperty = P<ProcessTechType>.Register(e => e.Sequence);

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int? Sequence
        {
            get { return GetProperty(SequenceProperty); }
            set { SetProperty(SequenceProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 制程工艺类型 实体配置
    /// </summary>
    internal class ProcessTechTypeConfig : EntityConfig<ProcessTechType>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("APS_PROTECH_TYPE").MapAllProperties();
            Meta.Property(ProcessTechType.CodeProperty).ColumnMeta.HasLength(160);
            Meta.Property(ProcessTechType.NameProperty).ColumnMeta.HasLength(400);
            Meta.EnablePhantoms();
        }
    }
}
