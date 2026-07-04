using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// NC主数据接口返回结果
    /// </summary>
    [Serializable]
    public class OuterSystemRetVO
    {
        /// <summary>
        /// 交互成功与否
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string errorMsg { get; set; }

        /// <summary>
        /// 业务系统回写的业务数据ID列表
        /// </summary>
        public List<MdMapingVO> mdMapings { get; set; } = new List<MdMapingVO>();
        /// <summary>
        /// 错误对象集合
        /// </summary>
        public List<object> errorList { get; set; } = new List<object>();

        /// <summary>
        /// 成功对象集合
        /// </summary>
        public List<object> sucessList { get; set; } = new List<object>();

        /// <summary>
        /// 合并结果(只适用于只有库存组织差别)
        /// </summary>
        /// <param name="vO"></param>
        public void Add(OuterSystemRetVO vO)
        {
            this.errorMsg = this.errorMsg.IsNullOrEmpty() ? vO.errorMsg : this.errorMsg + ";" + vO.errorMsg;
            this.errorList.AddRange(vO.errorList);
            this.sucessList.AddRange(vO.sucessList);
        }
    }
}
