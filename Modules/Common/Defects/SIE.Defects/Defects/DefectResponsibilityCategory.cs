using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects
{
    /// <summary>
    /// 缺陷责任分类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("缺陷责任分类")]
    [DisplayMember(nameof(Code))]
    public partial class DefectResponsibilityCategory : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        public static readonly Property<string> CodeProperty = P<DefectResponsibilityCategory>.Register(e => e.Code);

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
        [Label("描述")]
        [Required]
        [NotDuplicate]
        [MaxLength(1000)]
        public static readonly Property<string> DescriptionProperty = P<DefectResponsibilityCategory>.Register(e => e.Description);

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
    /// 缺陷责任分类 实体配置
    /// </summary>
    internal class DefectResponsibilityCategoryConfig : EntityConfig<DefectResponsibilityCategory>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("DEF_RESP_CATE").MapAllProperties();
            Meta.Property(DefectResponsibilityCategory.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
            Meta.SupportTree();
            Meta.EnableSort();
        }
    }
}