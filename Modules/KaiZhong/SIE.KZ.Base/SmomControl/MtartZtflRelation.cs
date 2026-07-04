using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.SmomControl
{
    /// <summary>
    /// 是否启用制卡数量维护
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("是否启用制卡数量维护")]
    public class MtartZtflRelation : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<MtartZtflRelation>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 物料类型 Mtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> MtartProperty = P<MtartZtflRelation>.Register(e => e.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string Mtart
        {
            get { return this.GetProperty(MtartProperty); }
            set { this.SetProperty(MtartProperty, value); }
        }
        #endregion

        #region 是否启用制卡数量 IsZtfl
        /// <summary>
        /// 是否启用制卡数量
        /// </summary>
        [Label("是否启用制卡数量")]
        public static readonly Property<bool?> IsZtflProperty = P<MtartZtflRelation>.Register(e => e.IsZtfl);

        /// <summary>
        /// 是否启用制卡数量
        /// </summary>
        public bool? IsZtfl
        {
            get { return this.GetProperty(IsZtflProperty); }
            set { this.SetProperty(IsZtflProperty, value); }
        }
        #endregion

        #region 是否启用交货容差 IsUebto
        /// <summary>
        /// 是否启用交货容差
        /// </summary>
        [Label("是否启用交货容差")]
        public static readonly Property<bool?> IsUebtoProperty = P<MtartZtflRelation>.Register(e => e.IsUebto);

        /// <summary>
        /// 是否启用交货容差
        /// </summary>
        public bool? IsUebto
        {
            get { return this.GetProperty(IsUebtoProperty); }
            set { this.SetProperty(IsUebtoProperty, value); }
        }
        #endregion

    }

    internal class MtartZtflRelationConfig : EntityConfig<MtartZtflRelation>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                //MtartZtflRelation.FactoryProperty,
                MtartZtflRelation.MtartProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同的数据".L10N();
                }
            });
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("MTART_ZTFL_REL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
