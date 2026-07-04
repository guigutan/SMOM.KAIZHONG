using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Specifications
{
    /// <summary>
	/// 规格件清单
	/// </summary>
	[RootEntity, Serializable]
    [CriteriaQuery]
    [DisplayMember(nameof(Code))]
    [Label("规格件清单")]
    public partial class Specification : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Specification>.Register(e => e.Code);

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
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Specification>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<Specification>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 规格件分类 Category
        /// <summary>
        /// 规格件分类Id
        /// </summary>
        public static readonly IRefIdProperty CategoryIdProperty = P<Specification>.RegisterRefId(e => e.CategoryId, ReferenceType.Normal);

        /// <summary>
        /// 规格件分类Id
        /// </summary>
        public double CategoryId
        {
            get { return (double)GetRefId(CategoryIdProperty); }
            set { SetRefId(CategoryIdProperty, value); }
        }

        /// <summary>
        /// 规格件分类
        /// </summary>
        public static readonly RefEntityProperty<SpecificationCategory> CategoryProperty = P<Specification>.RegisterRef(e => e.Category, CategoryIdProperty);

        /// <summary>
        /// 规格件分类
        /// </summary>
        public SpecificationCategory Category
        {
            get { return GetRefEntity(CategoryProperty); }
            set { SetRefEntity(CategoryProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 规格件清单 实体配置
    /// </summary>
    internal class SpecificationConfig : EntityConfig<Specification>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_SPEC").MapAllProperties();
            Meta.Property(Specification.CodeProperty).MapColumn().UseDataLang();
            Meta.Property(Specification.NameProperty).MapColumn().UseDataLang();
            Meta.EnablePhantoms();
        }
    }
}
