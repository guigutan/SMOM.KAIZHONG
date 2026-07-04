using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Enums
{
    /// <summary>
    /// QT消息推送对象
    /// </summary>
    public enum QTPushType
    {
        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        Employee = 1,

        /// <summary>
        /// 用户组
        /// </summary>
        [Label("用户组")]
        UserGroup = 2,

        /// <summary>
        /// 班组
        /// </summary>
        [Label("班组")]
        WorkGroup = 3,

        /// <summary>
        /// 角色
        /// </summary>
        [Label("角色")]
        Role = 4,

        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        Department = 5,
    }
}
