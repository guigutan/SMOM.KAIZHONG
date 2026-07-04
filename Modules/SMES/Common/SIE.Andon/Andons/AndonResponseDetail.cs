using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Andon;
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
    /// 安灯责任组
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安灯责任组")]
    public class AndonResponseDetail : DataEntity
    {
        #region 安灯维护 Andon
        /// <summary>
        /// 安灯维护Id
        /// </summary>
        [Label("安灯维护")]
        public static readonly IRefIdProperty AndonIdProperty =
            P<AndonResponseDetail>.RegisterRefId(e => e.AndonId, ReferenceType.Parent);

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
            P<AndonResponseDetail>.RegisterRef(e => e.Andon, AndonIdProperty);

        /// <summary>
        /// 安灯维护
        /// </summary>
        public Andon Andon
        {
            get { return this.GetRefEntity(AndonProperty); }
            set { this.SetRefEntity(AndonProperty, value); }
        }
        #endregion

        #region 安灯区域 AndonUphold
        /// <summary>
        /// 安灯区域Id
        /// </summary>
        [Label("安灯区域")]
        public static readonly IRefIdProperty AndonUpholdIdProperty =
            P<AndonResponseDetail>.RegisterRefId(e => e.AndonUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 安灯区域Id
        /// </summary>
        public double AndonUpholdId
        {
            get { return (double)this.GetRefId(AndonUpholdIdProperty); }
            set { this.SetRefId(AndonUpholdIdProperty, value); }
        }

        /// <summary>
        /// 安灯区域
        /// </summary>
        public static readonly RefEntityProperty<AndonUphold> AndonUpholdProperty =
            P<AndonResponseDetail>.RegisterRef(e => e.AndonUphold, AndonUpholdIdProperty);

        /// <summary>
        /// 安灯区域
        /// </summary>
        public AndonUphold AndonUphold
        {
            get { return this.GetRefEntity(AndonUpholdProperty); }
            set { this.SetRefEntity(AndonUpholdProperty, value); }
        }
        #endregion

        #region 安灯责任组 AndonGroup
        /// <summary>
        /// 安灯责任组Id
        /// </summary>
        [Label("安灯责任组")]
        public static readonly IRefIdProperty AndonGroupIdProperty =
            P<AndonResponseDetail>.RegisterRefId(e => e.AndonGroupId, ReferenceType.Normal);

        /// <summary>
        /// 安灯责任组Id
        /// </summary>
        public double AndonGroupId
        {
            get { return (double)this.GetRefId(AndonGroupIdProperty); }
            set { this.SetRefId(AndonGroupIdProperty, value); }
        }

        /// <summary>
        /// 安灯责任组
        /// </summary>
        public static readonly RefEntityProperty<AndonGroup> AndonGroupProperty =
            P<AndonResponseDetail>.RegisterRef(e => e.AndonGroup, AndonGroupIdProperty);

        /// <summary>
        /// 安灯责任组
        /// </summary>
        public AndonGroup AndonGroup
        {
            get { return this.GetRefEntity(AndonGroupProperty); }
            set { this.SetRefEntity(AndonGroupProperty, value); }
        }
        #endregion

        #region 安灯级别 AndonseepLevel
        /// <summary>
        /// 安灯级别
        /// </summary>
        [Label("安灯级别")]
        public static readonly Property<string> AndonseepLevelProperty = P<AndonResponseDetail>.Register(e => e.AndonseepLevel);

        /// <summary>
        /// 安灯级别
        /// </summary>
        public string AndonseepLevel
        {
            get { return this.GetProperty(AndonseepLevelProperty); }
            set { this.SetProperty(AndonseepLevelProperty, value); }
        }
        #endregion

    }

    internal class AndonResponseDetailConfig : EntityConfig<AndonResponseDetail>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                    AndonResponseDetail.AndonIdProperty, AndonResponseDetail.AndonUpholdIdProperty,AndonResponseDetail.AndonGroupIdProperty,AndonResponseDetail.AndonseepLevelProperty
                },
                MessageBuilder = (e) =>
                {
                    return "存在相同的安灯责任组数据".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("ANDON_RESPONSE_DTL").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
