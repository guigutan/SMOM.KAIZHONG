using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects
{
    /// <summary>
    /// 缺陷分类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("缺陷代码分类")]
    [DisplayMember(nameof(Code))]
    public partial class DefectCategory : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<DefectCategory>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("描述")]
        [MaxLength(1000)]
        public static readonly Property<string> DescriptionProperty = P<DefectCategory>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 缺陷分类 实体配置
    /// </summary>
    internal class DefectCategoryConfig : EntityConfig<DefectCategory>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DEF_CATE").MapAllProperties();
            Meta.Property(DefectCategory.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(DefectCategory.CodeProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
            Meta.EnableSort();
            Meta.SupportTree();
        }
    }
}