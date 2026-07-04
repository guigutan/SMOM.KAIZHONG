using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models
{
    /// <summary>
    /// 拼板码绑定信息
    /// </summary>
    [Serializable]
    public class CombinedCode
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CombinedCode()
        {
            BindingSns = new List<BindingSn>();
        }

        /// <summary>
        /// 绑定SN数量
        /// </summary>
        public int ToBindingQty { get; set; }

        /// <summary>
        /// 自动生成SN并绑定
        /// </summary>
        public bool AutoCreateAndBinding { get; set; }

        /// <summary>
        /// 待手动绑定SN
        /// 当配置项配置拼板码手动绑定SN，且未绑定Sn情况下为true
        /// </summary>
        public bool ToBeManualBinding { get { return !AutoCreateAndBinding && BindingSns.Count == 0; } }

        /// <summary>
        /// 绑定条码列表
        /// </summary>
        public List<BindingSn> BindingSns { get; set; }
    }
}
