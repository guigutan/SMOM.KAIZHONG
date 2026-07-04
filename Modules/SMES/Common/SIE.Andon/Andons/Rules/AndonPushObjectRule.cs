using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Rules
{
    #region 安灯推送对象验证
    ///// <summary>
    ///// 安灯推送对象验证
    ///// </summary>
    //[System.ComponentModel.DisplayName("<对象类型非触发人或处理人或班组长或负责人时对象编码不能为空>")]
    //[System.ComponentModel.Description("<对象类型非触发人或处理人或班组长或负责人时对象编码不能为空>")]
    //public class AndonPushObjectCodeRule : EntityRule<AndonPushObject>
    //{
    //    /// <summary>
    //    /// 验证操作类型
    //    /// </summary>
    //    public AndonPushObjectCodeRule()
    //    {
    //        Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
    //        ConnectToDataSource = true;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="entity"></param>
    //    /// <param name="e"></param>
    //    protected override void Validate(IEntity entity, RuleArgs e)
    //    {
    //        var pushObject = entity as AndonPushObject;
    //        if (pushObject.Type != Enum.PushObjectType.Trigger && pushObject.Type != Enum.PushObjectType.Handler && pushObject.Type != Enum.PushObjectType.WorkGroupCharge && pushObject.Type != Enum.PushObjectType.AndonCharger)
    //        {
    //            if (pushObject.Code.IsNullOrEmpty())
    //            {
    //                e.BrokenDescription = "对象类型非触发人或处理人或班组长或负责人时对象编码不能为空".L10N();
    //            }
    //        }

    //    }
    //}


    ///// <summary>
    ///// 安灯维护推送对象不能重复
    ///// </summary>
    //[System.ComponentModel.DisplayName("<安灯维护推送对象不能重复>")]
    //[System.ComponentModel.Description("<安灯维护推送对象不能重复>")]
    //public class AndonPushObjectNotDupRule : NotDuplicateRule<AndonPushObject>
    //{
    //    /// <summary>
    //    /// 安灯维护推送对象不能重复
    //    /// </summary>
    //    public AndonPushObjectNotDupRule()
    //    {
    //        Properties.Add(AndonPushObject.MessageSendIdProperty);
    //        Properties.Add(AndonPushObject.TypeProperty);
    //        Properties.Add(AndonPushObject.CodeProperty);
    //        MessageBuilder = (e) =>
    //        {
    //            var andonPushObject = e as AndonPushObject;
    //            return "对象类型({0}),对象编码({1})数据已重复".L10nFormat(andonPushObject.Type.ToLabel().L10N(), andonPushObject.Code);
    //        };
    //    }
    //}
    #endregion
}
