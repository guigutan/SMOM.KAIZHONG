using System;

namespace SIE.Kit.MES.CallMaterials.Interfaces
{
    /// <summary>
    /// 已退料物料标签信息
    /// 继承"物料标签退料信息基类"WithdrawalItemLabelBase
    /// </summary>
    [Serializable]
    public class WithdrawaledItemLabelInfo : WithdrawalItemLabelBase
    {

        /// <summary>
        /// 退料人Id
        /// </summary>
        public double WithdrawalById { get; set; }

        /// <summary>
        /// 退料人姓名
        /// </summary>
        public string WithdrawalByName { get; set; }

        /// <summary>
        /// 退料时间
        /// </summary>
        public string WithdrawalDate { get; set; }
    }
}