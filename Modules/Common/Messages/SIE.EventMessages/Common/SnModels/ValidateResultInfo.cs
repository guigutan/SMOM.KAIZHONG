using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.Common.SnModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ValidateResultInfo
    {
        /// <summary>
        /// 0：失败，1：成功
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
