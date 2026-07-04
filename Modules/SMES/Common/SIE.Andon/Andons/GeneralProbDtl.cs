using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 通用问题描述
    /// </summary>
    [ChildEntity, Serializable]
    [Label("通用问题描述")]
    [DisplayMember(nameof(Desc))]
    public class GeneralProbDtl : DataEntity
    {
        #region 安灯维护 Andon
        /// <summary>
        /// 安灯维护Id
        /// </summary>
        [Label("属性名")]
        public static readonly IRefIdProperty AndonIdProperty =
            P<GeneralProbDtl>.RegisterRefId(e => e.AndonId, ReferenceType.Parent);

        /// <summary>
        /// 安灯维护Id
        /// </summary>
        public double AndonId
        {
            get { return (double)this.GetRefId(AndonIdProperty); }
            set { this.SetRefId(AndonIdProperty, value); }
        }

        /// <summary>
        /// 安灯维护
        /// </summary>
        public static readonly RefEntityProperty<Andon> AndonProperty =
            P<GeneralProbDtl>.RegisterRef(e => e.Andon, AndonIdProperty);

        /// <summary>
        /// 安灯维护
        /// </summary>
        public Andon Andon
        {
            get { return this.GetRefEntity(AndonProperty); }
            set { this.SetRefEntity(AndonProperty, value); }
        }
        #endregion

        #region 问题描述 Desc
        /// <summary>
        /// 问题描述
        /// </summary>
        [Label("问题描述")]
        public static readonly Property<string> DescProperty = P<GeneralProbDtl>.Register(e => e.Desc);

        /// <summary>
        /// 问题描述
        /// </summary>
        public string Desc
        {
            get { return this.GetProperty(DescProperty); }
            set { this.SetProperty(DescProperty, value); }
        }
        #endregion
    }

    internal class GeneralProbDtlConfig : EntityConfig<GeneralProbDtl>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                GeneralProbDtl.AndonIdProperty,GeneralProbDtl.DescProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同的描述".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("GENERAL_PROB_DTL").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
            Meta.EnableSort();
        }
    }
}
