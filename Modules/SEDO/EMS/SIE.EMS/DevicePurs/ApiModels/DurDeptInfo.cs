using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.DevicePurs.ApiModels
{
    /// <summary>
    /// 确认人权限部门信息
    /// </summary>
    [Serializable]
    public class DurDeptInfo
    {
        /// <summary>
        /// 责任部门Id
        /// </summary>
        public double DeptId { get; set; }

        /// <summary>
        /// 是否确认
        /// </summary>
        public bool IsConfirm { get; set; }
    }
}
