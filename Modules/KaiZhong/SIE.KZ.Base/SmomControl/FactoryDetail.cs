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
    /// 工厂明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工厂明细")]
    public class FactoryDetail : DataEntity
    {
        #region 总控 SmomControlSetting
        /// <summary>
        /// 总控Id
        /// </summary>
        [Label("总控")]
        public static readonly IRefIdProperty SmomControlSettingIdProperty =
            P<FactoryDetail>.RegisterRefId(e => e.SmomControlSettingId, ReferenceType.Parent);

        /// <summary>
        /// 总控Id
        /// </summary>
        public double SmomControlSettingId
        {
            get { return (double)this.GetRefId(SmomControlSettingIdProperty); }
            set { this.SetRefId(SmomControlSettingIdProperty, value); }
        }

        /// <summary>
        /// 总控
        /// </summary>
        public static readonly RefEntityProperty<SmomControlSetting> SmomControlSettingProperty =
            P<FactoryDetail>.RegisterRef(e => e.SmomControlSetting, SmomControlSettingIdProperty);

        /// <summary>
        /// 总控
        /// </summary>
        public SmomControlSetting SmomControlSetting
        {
            get { return this.GetRefEntity(SmomControlSettingProperty); }
            set { this.SetRefEntity(SmomControlSettingProperty, value); }
        }
        #endregion

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<FactoryDetail>.Register(e => e.FactoryCode);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
            set { this.SetProperty(FactoryCodeProperty, value); }
        }
        #endregion

    }

    internal class FactoryDetailConfig : EntityConfig<FactoryDetail>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(FactoryDetail.FactoryCodeProperty, new RequiredRule());
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                    FactoryDetail.FactoryCodeProperty,
                    FactoryDetail.SmomControlSettingIdProperty
                },
                MessageBuilder = (e) => {
                    return "已存在相同的工厂编码".L10N();
                }
            });
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("SMOM_CONTROL_FACTORY_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
