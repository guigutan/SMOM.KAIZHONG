using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Enums
{
    /// <summary>
    /// 执行结果
    /// </summary>
    public enum CallResultSO
    {
/*        /// <summary>
        /// 接口已触发
        /// </summary>
        [Label("接口已触发")]
        UnSave = 0,*/

        /// <summary>
        /// 成功
        /// </summary>
        [Label("成功")]
        Success = 1,

        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        Fail = 2,
    }
}
