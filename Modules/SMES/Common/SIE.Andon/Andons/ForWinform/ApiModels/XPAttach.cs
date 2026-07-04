using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class XPAttach
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] Contents { get; set; }
    }
}
