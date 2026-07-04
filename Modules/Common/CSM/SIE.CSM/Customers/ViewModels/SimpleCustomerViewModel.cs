using System;

namespace SIE.CSM.Customers
{
    /// <summary>
    /// 客户 简易ViewModel
    /// </summary>
    [Serializable]
    public class SimpleCustomerViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}