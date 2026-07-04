using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    /// <summary>
    /// 星宇接口返回数据
    /// </summary>
    [Serializable]
    public class KzApiCommonRes : BaseApiRes
    {
        /// <summary>
        /// 接口是否执行成功(接口业务代码无异常)
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Message { get; set; }
    }
}
