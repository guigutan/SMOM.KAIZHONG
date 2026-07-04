using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Rules
{
    /// <summary>
    /// 安灯类型推送对象验证
    /// </summary>
    [System.ComponentModel.DisplayName("<对象类型非触发人或处理人或班组长或负责人时对象编码不能为空>")]
    [System.ComponentModel.Description("<对象类型非触发人或处理人或班组长或负责人时对象编码不能为空>")]
    public class AndonTypePushObjectCodeRule : EntityRule<AndonTypePushObject>
    {
        /// <summary>
        /// 验证操作类型
        /// </summary>
        public AndonTypePushObjectCodeRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pushObject = entity as AndonTypePushObject;
            if (pushObject.Type != Enum.PushObjectType.Trigger && pushObject.Type != Enum.PushObjectType.Handler && pushObject.Type != Enum.PushObjectType.WorkGroupCharge && pushObject.Type != Enum.PushObjectType.AndonCharger)
            {
                if (pushObject.Code.IsNullOrEmpty())
                {
                    e.BrokenDescription = "对象类型非触发人或处理人或班组长或负责人时对象编码不能为空".L10N();
                }
            }
        }
    }


    /// <summary>
    /// 安灯类型维护推送对象不能重复
    /// </summary>
    [System.ComponentModel.DisplayName("<安灯类型维护推送对象不能重复>")]
    [System.ComponentModel.Description("<安灯类型维护推送对象不能重复>")]
    public class AndonTypePushObjectNotDupRule: NotDuplicateRule<AndonTypePushObject>
    {
        /// <summary>
        /// 安灯类型维护推送对象不能重复
        /// </summary>
        public AndonTypePushObjectNotDupRule()
        {
            Properties.Add(AndonTypePushObject.MessageSendIdProperty);
            Properties.Add(AndonTypePushObject.TypeProperty);
            Properties.Add(AndonTypePushObject.CodeProperty);
            MessageBuilder = (e) =>
            {
                var andonTypePushObject = e as AndonTypePushObject;
                return "对象类型({0}),对象编码({1})数据已重复".L10nFormat(andonTypePushObject.Type.ToLabel().L10N(), andonTypePushObject.Code);
            };
        }
    }
}
