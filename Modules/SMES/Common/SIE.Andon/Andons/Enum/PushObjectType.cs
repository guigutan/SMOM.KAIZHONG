using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Enum
{
    /// <summary>
    /// 推送对象类型枚举
    /// </summary>
    public enum PushObjectType
    {
        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        Staff = 10,

        /// <summary>
        /// 角色
        /// </summary>
        [Label("角色")]
        Role = 20,

        /// <summary>
        /// 用户组
        /// </summary>
        [Label("用户组")]
        UserGroup = 30,

        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        Department = 40,

        /// <summary>
        /// 触发人
        /// </summary>
        [Label("触发人")]
        Trigger = 50,

        /// <summary>
        /// 处理人
        /// </summary>
        [Label("处理人")]
        Handler = 60,

        /// <summary>
        /// 班组长
        /// </summary>
        [Label("班组长")]
        WorkGroupCharge = 70,

        /// <summary>
        /// 负责人
        /// </summary>
        [Label("负责人")]
        AndonCharger = 80,
    }
}
