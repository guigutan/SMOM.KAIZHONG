using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects
{
    /// <summary>
    /// 缺陷责任
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("缺陷责任")]
    [DisplayMember(nameof(Description))]
    public partial class DefectResponsibility : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        public static readonly Property<string> CodeProperty = P<DefectResponsibility>.Register(e => e.Code);

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
        public static readonly Property<string> DescriptionProperty = P<DefectResponsibility>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>        
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 缺陷责任分类 Category
        /// <summary>
        /// 缺陷责任分类Id
        /// </summary>
        [Label("缺陷责任分类")]
        public static readonly IRefIdProperty CategoryIdProperty = P<DefectResponsibility>.RegisterRefId(e => e.CategoryId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷责任分类Id
        /// </summary>
        public double CategoryId
        {
            get { return (double)GetRefId(CategoryIdProperty); }
            set { SetRefId(CategoryIdProperty, value); }
        }

        /// <summary>
        /// 缺陷责任分类
        /// </summary>
        public static readonly RefEntityProperty<DefectResponsibilityCategory> CategoryProperty = P<DefectResponsibility>.RegisterRef(e => e.Category, CategoryIdProperty);

        /// <summary>
        /// 缺陷责任分类
        /// </summary>
        public DefectResponsibilityCategory Category
        {
            get { return GetRefEntity(CategoryProperty); }
            set { SetRefEntity(CategoryProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 分类编码 CategoryCode
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> CategoryCodeProperty = P<DefectResponsibility>.RegisterView(e => e.CategoryCode, p => p.Category.Code);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode
        {
            get { return this.GetProperty(CategoryCodeProperty); }
        }
        #endregion

        #region 描述 CategoryDescription
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> CategoryDescriptionProperty = P<DefectResponsibility>.RegisterView(e => e.CategoryDescription, p => p.Category.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string CategoryDescription
        {
            get { return this.GetProperty(CategoryDescriptionProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 缺陷责任 实体配置
    /// </summary>
    internal class DefectResponsibilityConfig : EntityConfig<DefectResponsibility>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("DEF_RESP").MapAllProperties();
            Meta.Property(DefectResponsibility.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}