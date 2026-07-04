using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Skills;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 机型技能
    /// </summary>
    [ChildEntity, Serializable]
    [Label("机型技能")]
    public partial class ProductModelSkill : DataEntity
    {
        #region 产品机型 Model
        /// <summary>
        /// 产品机型Id
        /// </summary>
        [Label("产品机型")]
        public static readonly IRefIdProperty ModelIdProperty =
            P<ProductModelSkill>.RegisterRefId(e => e.ModelId, ReferenceType.Parent);

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double ModelId
        {
            get { return (double)this.GetRefId(ModelIdProperty); }
            set { this.SetRefId(ModelIdProperty, value); }
        }

        /// <summary>
        /// 产品机型
        /// </summary>
        public static readonly RefEntityProperty<ProductModel> ModelProperty =
            P<ProductModelSkill>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel Model
        {
            get { return this.GetRefEntity(ModelProperty); }
            set { this.SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 技能清单 Skill
        /// <summary>
        /// 技能清单Id
        /// </summary>
        [Label("技能清单")]
        public static readonly IRefIdProperty SkillIdProperty =
            P<ProductModelSkill>.RegisterRefId(e => e.SkillId, ReferenceType.Normal);

        /// <summary>
        /// 技能清单Id
        /// </summary>
        public double SkillId
        {
            get { return (double)this.GetRefId(SkillIdProperty); }
            set { this.SetRefId(SkillIdProperty, value); }
        }

        /// <summary>
        /// 技能清单
        /// </summary>
        public static readonly RefEntityProperty<Skill> SkillProperty =
            P<ProductModelSkill>.RegisterRef(e => e.Skill, SkillIdProperty);

        /// <summary>
        /// 技能清单
        /// </summary>
        public Skill Skill
        {
            get { return this.GetRefEntity(SkillProperty); }
            set { this.SetRefEntity(SkillProperty, value); }
        }
        #endregion

        #region 技能名称 SkillName
        /// <summary>
        /// 技能名称
        /// </summary>
        [Label("技能名称")]
        public static readonly Property<string> SkillNameProperty = P<ProductModelSkill>.RegisterView(e => e.SkillName, p => p.Skill.Name);

        /// <summary>
        /// 技能名称
        /// </summary>
        public string SkillName
        {
            get { return this.GetProperty(SkillNameProperty); }
        }
        #endregion 

        #region 技能编码 SkillCode
        /// <summary>
        /// 技能名称
        /// </summary>
        [Label("技能编码")]
        public static readonly Property<string> SkillCodeProperty = P<ProductModelSkill>.RegisterView(e => e.SkillCode, p => p.Skill.Code);

        /// <summary>
        /// 技能编码
        /// </summary>
        public string SkillCode
        {
            get { return this.GetProperty(SkillCodeProperty); }
        }
        #endregion 

        #region 需求人数 DemandQty
        /// <summary>
        /// 需求人数
        /// </summary>
        [Label("需求人数")]
        [Required]
        public static readonly Property<int?> DemandQtyProperty = P<ProductModelSkill>.Register(e => e.DemandQty);

        /// <summary>
        /// 需求人数
        /// </summary>
        public int? DemandQty
        {
            get { return this.GetProperty(DemandQtyProperty); }
            set { this.SetProperty(DemandQtyProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 机型技能 实体配置
    /// </summary>
    internal class ProductModelSkillConfig : EntityConfig<ProductModelSkill>
    {
        /// <summary>
        /// 子类重写此方法，并完成对实体验证规则的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        /// <param name="rules">验证规则声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(new NotDuplicateRule()
            {
                Properties =
                {
                    ProductModelSkill.ModelIdProperty,
                    ProductModelSkill.SkillIdProperty,
                },
                MessageBuilder = (e) =>
                 {
                     return "产品机型与机型技能不能重复添加".L10N();
                 }
            });
            base.AddValidations(rules);
        }

        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRODUCT_MODEL_Skill").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }


}