using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯责任组维护基础表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonGroupCriteria))]
    [Label("安灯责任组维护基础表")]
    [DisplayMember(nameof(Code))]
    public class AndonGroup : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<AndonGroup>.Register(e => e.Code);

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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<AndonGroup>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 明细 AndonGroupDetailList
        /// <summary>
        /// 明细
        /// </summary>
        [Label("明细")]
        public static readonly ListProperty<EntityList<AndonGroupDetail>> AndonGroupDetailListProperty = P<AndonGroup>.RegisterList(e => e.AndonGroupDetailList);

        /// <summary>
        /// 明细
        /// </summary>
        public EntityList<AndonGroupDetail> AndonGroupDetailList
        {
            get { return this.GetLazyList(AndonGroupDetailListProperty); }
        }
        #endregion

    }

    internal class AndonGroupConfig : EntityConfig<AndonGroup>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(AndonGroup.CodeProperty, new NotDuplicateRule());
            rules.AddRule(AndonGroup.NameProperty, new NotDuplicateRule());
            rules.AddRule(AndonGroup.CodeProperty, new RequiredRule());
            rules.AddRule(AndonGroup.NameProperty, new RequiredRule());

            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("ANDON_GROUP").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
