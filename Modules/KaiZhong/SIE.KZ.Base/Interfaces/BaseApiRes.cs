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
    public class BaseApiRes
    {
        /// <summary>
        /// 数据量
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// ERP成功对象
        /// </summary>
        public List<object> SuccessList { get; set; } = new List<object>();

        /// <summary>
        /// 错误对象
        /// </summary>
        public List<object> ErrorObjList { get; set; } = new List<object>();

        /// <summary>
        /// 错误信息(中间表)
        /// </summary>
        public List<string> ErrorList { get; set; } = new List<string>();
    }
}
