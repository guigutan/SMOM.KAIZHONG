using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.DevicePurs.Rules
{
    #region  用户组或员工必填一项验证
    /// <summary>
    /// 用户组或员工必填一项验证
    /// </summary>
    [DisplayName("用户组或员工必填一项验证")]
    [Description("用户组或员工必填一项验证")]
    public class DevicePurRule : EntityRule<DevicePur>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DevicePurRule()
        {
            Scope = EntityStatusScopes.Update | EntityStatusScopes.Add;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var devicePur = entity as DevicePur;
            if (devicePur.User == null && devicePur.UserGroup == null)
            {
                e.BrokenDescription = "用户组或员工必填一项验证".L10N();
            }
        }
    }
    #endregion

    /// <summary>
    /// 设备与人员权限维护-用户组不能重复的验证规则
    /// </summary>
    [DisplayName("设备与人员权限维护-用户组不能重复")]
    [Description("设备与人员权限维护-用户组不能重复")]
    public class DevicePurUserGroupIdNotDuplicateRule : NotDuplicateRule<DevicePur>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DevicePurUserGroupIdNotDuplicateRule()
        {
            Properties.Add(DevicePur.UserGroupIdProperty);

            MessageBuilder = (e) =>
            {
                DevicePur devicePur = e as DevicePur;

                if (devicePur != null)
                {
                    return "已经存在[用户组编号]是【{0}】的[设备与人员权限维护]"
                        .L10nFormat(devicePur.UserGroup.Code);
                }
                else
                {
                    return "[设备与人员权限维护]的[用户组编号]重复"
                        .L10nFormat();
                }
            };
        }
    }

    /// <summary>
    /// 设备与人员权限维护-用户不能重复的验证规则
    /// </summary>
    [DisplayName("设备与人员权限维护-用户不能重复")]
    [Description("设备与人员权限维护-用户不能重复")]
    public class DevicePurUserIdNotDuplicateRule : NotDuplicateRule<DevicePur>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DevicePurUserIdNotDuplicateRule()
        {
            Properties.Add(DevicePur.UserIdProperty);
            MessageBuilder = (e) =>
            {
                DevicePur devicePur = e as DevicePur;

                if (devicePur != null)
                {
                    return "已经存在[用户]是【{0}】的[设备与人员权限维护]"
                        .L10nFormat(devicePur.User.Employee.Name);
                }
                else
                {
                    return "[设备与人员权限维护]的[用户]重复"
                        .L10nFormat();
                }
            };
        }
    }

    #region 采购对象非重复验证规则
    /// <summary>
    /// 采购对象非重复验证规则
    /// </summary>
    [DisplayName("采购对象非重复验证规则")]
    [Description("采购对象非重复验证规则")]
    public class DevicePurchaseObjectTypeNotDuplicateRule : NotDuplicateRule<DevicePurchaseObjectType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DevicePurchaseObjectTypeNotDuplicateRule()
        {
            Properties.Add(DevicePurchaseObjectType.DevicePurIdProperty);
            Properties.Add(DevicePurchaseObjectType.PurchaseObjectTypeProperty);
            MessageBuilder = (e) => { return "采购对象不能重复".L10N(); };
        }
    }
    #endregion
}
