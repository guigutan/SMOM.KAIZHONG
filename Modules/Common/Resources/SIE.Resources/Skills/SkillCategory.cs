using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Skills
{
    /// <summary>
    /// 技能分类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("技能分类")]
    [DisplayMember(nameof(Name))]
    public partial class SkillCategory : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> CodeProperty = P<SkillCategory>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<SkillCategory>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(1000)]
        public static readonly Property<string> RemarkProperty = P<SkillCategory>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 技能分类 实体配置
    /// </summary>
    internal class SkillCategoryConfig : EntityConfig<SkillCategory>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_SKILL_CATE").MapAllProperties();
            Meta.Property(SkillCategory.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}