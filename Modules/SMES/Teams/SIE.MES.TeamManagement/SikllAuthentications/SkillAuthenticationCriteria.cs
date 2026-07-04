using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Skills;
using System;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 员工技能查询实体类
    /// </summary>
    [QueryEntity, Serializable]
    [Label("员工技能查询实体类")]
    public class SkillAuthenticationCriteria : Criteria
    {
        #region 技能 Skill
        /// <summary>
        /// 技能Id
        /// </summary>
        [Label("技能")]
        public static readonly IRefIdProperty SkillIdProperty =
            P<SkillAuthenticationCriteria>.RegisterRefId(e => e.SkillId, ReferenceType.Normal);

        /// <summary>
        /// 技能Id
        /// </summary>
        public double? SkillId
        {
            get { return (double?)this.GetRefNullableId(SkillIdProperty); }
            set { this.SetRefNullableId(SkillIdProperty, value); }
        }

        /// <summary>
        /// 技能
        /// </summary>
        public static readonly RefEntityProperty<Skill> SkillProperty =
            P<SkillAuthenticationCriteria>.RegisterRef(e => e.Skill, SkillIdProperty);

        /// <summary>
        /// 技能
        /// </summary>
        public Skill Skill
        {
            get { return this.GetRefEntity(SkillProperty); }
            set { this.SetRefEntity(SkillProperty, value); }
        }
        #endregion

        #region 分类 SkillCategory
        /// <summary>
        /// 分类Id
        /// </summary>
        [Label("分类")]
        public static readonly IRefIdProperty SkillCategoryIdProperty =
            P<SkillAuthenticationCriteria>.RegisterRefId(e => e.SkillCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 分类Id
        /// </summary>
        public double? SkillCategoryId
        {
            get { return (double?)this.GetRefNullableId(SkillCategoryIdProperty); }
            set { this.SetRefNullableId(SkillCategoryIdProperty, value); }
        }

        /// <summary>
        /// 分类
        /// </summary>
        public static readonly RefEntityProperty<SkillCategory> SkillCategoryProperty =
            P<SkillAuthenticationCriteria>.RegisterRef(e => e.SkillCategory, SkillCategoryIdProperty);

        /// <summary>
        /// 分类
        /// </summary>
        public SkillCategory SkillCategory
        {
            get { return this.GetRefEntity(SkillCategoryProperty); }
            set { this.SetRefEntity(SkillCategoryProperty, value); }
        }
        #endregion 

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns>认证技能集合</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SkillAuthController>().GetSkillAuths(this);
        }
    }
}