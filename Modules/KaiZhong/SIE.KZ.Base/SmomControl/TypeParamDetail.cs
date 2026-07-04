using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces.Enums;
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
    /// 类型属性
    /// </summary>
    [ChildEntity, Serializable]
    [Label("类型属性")]
    public class TypeParamDetail : DataEntity
    {
        #region 总控 SmomControlSetting
        /// <summary>
        /// 总控Id
        /// </summary>
        [Label("总控")]
        public static readonly IRefIdProperty SmomControlSettingIdProperty =
            P<TypeParamDetail>.RegisterRefId(e => e.SmomControlSettingId, ReferenceType.Parent);

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
            P<TypeParamDetail>.RegisterRef(e => e.SmomControlSetting, SmomControlSettingIdProperty);

        /// <summary>
        /// 总控
        /// </summary>
        public SmomControlSetting SmomControlSetting
        {
            get { return this.GetRefEntity(SmomControlSettingProperty); }
            set { this.SetRefEntity(SmomControlSettingProperty, value); }
        }
        #endregion

        #region 类型 ParamType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<InfType> ParamTypeProperty = P<TypeParamDetail>.Register(e => e.ParamType);

        /// <summary>
        /// 类型
        /// </summary>
        public InfType ParamType
        {
            get { return this.GetProperty(ParamTypeProperty); }
            set { this.SetProperty(ParamTypeProperty, value); }
        }
        #endregion

    }

    internal class TypeParamDetailConfig : EntityConfig<TypeParamDetail>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(TypeParamDetail.ParamTypeProperty, new RequiredRule());
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                    TypeParamDetail.ParamTypeProperty,
                    TypeParamDetail.SmomControlSettingIdProperty
                },
                MessageBuilder = (e) => {
                    return "已存在相同的类型".L10N();
                }
            });
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("SMOM_CONTROL_TYPE_PARAM_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
