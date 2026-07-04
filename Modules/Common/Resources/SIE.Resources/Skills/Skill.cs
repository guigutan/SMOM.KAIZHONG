using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Skills
{
    /// <summary>
    /// 技能清单
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("技能清单")]
    [DisplayMember(nameof(Name))]
    public partial class Skill : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Skill>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Skill>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 有效期(天) Validity
        /// <summary>
        /// 有效期(天)
        /// </summary>
        [Label("有效期(天)")]
        [MaxValue(3650)]
        public static readonly Property<int?> ValidityProperty = P<Skill>.Register(e => e.Validity);

        /// <summary>
        /// 有效期(天)
        /// </summary>
        public int? Validity
        {
            get { return this.GetProperty(ValidityProperty); }
            set { this.SetProperty(ValidityProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(1000)]
        public static readonly Property<string> RemarkProperty = P<Skill>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 技能分类 Category
        /// <summary>
        /// 技能分类Id
        /// </summary>
        [Label("技能分类")]
        public static readonly IRefIdProperty CategoryIdProperty =
            P<Skill>.RegisterRefId(e => e.CategoryId, ReferenceType.Normal);

        /// <summary>
        /// 技能分类Id
        /// </summary>
        public double CategoryId
        {
            get { return (double)this.GetRefId(CategoryIdProperty); }
            set { this.SetRefId(CategoryIdProperty, value); }
        }

        /// <summary>
        /// 技能分类
        /// </summary>
        public static readonly RefEntityProperty<SkillCategory> CategoryProperty =
            P<Skill>.RegisterRef(e => e.Category, CategoryIdProperty);

        /// <summary>
        /// 技能分类
        /// </summary>
        public SkillCategory Category
        {
            get { return this.GetRefEntity(CategoryProperty); }
            set { this.SetRefEntity(CategoryProperty, value); }
        }
        #endregion 

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 分类编码 CategoryCode
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> CategoryCodeProperty = P<Skill>.RegisterView(e => e.CategoryCode, p => p.Category.Code);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode
        {
            get { return this.GetProperty(CategoryCodeProperty); }
        }
        #endregion

        #region 分类名称 CategoryName
        /// <summary>
        /// 分类名称
        /// </summary>
        [Label("分类名称")]
        public static readonly Property<string> CategoryNameProperty = P<Skill>.RegisterView(e => e.CategoryName, p => p.Category.Name);

        /// <summary>
        /// 描述
        /// </summary>
        public string CategoryName
        {
            get { return this.GetProperty(CategoryNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 技能清单 实体配置
    /// </summary>
    internal class SkillConfig : EntityConfig<Skill>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_SKILL").MapAllProperties();
            Meta.Property(Skill.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
