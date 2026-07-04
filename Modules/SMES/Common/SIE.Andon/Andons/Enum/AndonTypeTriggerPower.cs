using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Enum
{
    /// <summary>
    /// 触发权限对象类型枚举
    /// </summary>
    public enum AndonTypeTriggerPower
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
        /// 班组
        /// </summary>
        [Label("班组")]
        Team = 50,
    }
}
