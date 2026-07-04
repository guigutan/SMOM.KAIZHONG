using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.ProcessSegments
{
    /// <summary>
    /// 工段
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工段")]
    [DisplayMember(nameof(Name))]
    public partial class ProcessSegment : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ProcessSegment>.Register(e => e.Code);

        /// <summary>
        /// 编码
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
        [MaxLength(40)]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ProcessSegment>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 是否拆分 IsSplit
        /// <summary>
        /// 是否拆分
        /// </summary>
        [Label("是否拆分")]
        public static readonly Property<bool?> IsSplitProperty = P<ProcessSegment>.Register(e => e.IsSplit);

        /// <summary>
        /// 是否拆分
        /// </summary>
        public bool? IsSplit
        {
            get { return this.GetProperty(IsSplitProperty); }
            set { this.SetProperty(IsSplitProperty, value); }
        }
        #endregion


        #region 顺序(领料) ReceiveMaterialSortIndex
        /// <summary>
        /// 顺序(领料)
        /// </summary>
        [Label("顺序(领料)")]
        public static readonly Property<int?> ReceiveMaterialSortIndexProperty = P<ProcessSegment>.Register(e => e.ReceiveMaterialSortIndex);

        /// <summary>
        /// 顺序(领料)
        /// </summary>
        public int? ReceiveMaterialSortIndex
        {
            get { return GetProperty(ReceiveMaterialSortIndexProperty); }
            set { SetProperty(ReceiveMaterialSortIndexProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工段 实体配置
    /// </summary>
    internal class ProcessSegmentConfig : EntityConfig<ProcessSegment>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROC_SEGMENT").MapAllProperties();
            Meta.Property(ProcessSegment.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(ProcessSegment.NameProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}