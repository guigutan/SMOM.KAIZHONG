using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// 批量传入SAP且回传数据与现有数据进行一对一关联
    /// </summary>
    /// <typeparam name="T">入参类型</typeparam>
    /// <typeparam name="R"></typeparam>
    public class SapParameters<T, R>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 入参
        /// </summary>
        public T InputParams { get; set; }
        /// <summary>
        /// 出参类型
        /// </summary>
        public R OutputParams { get; set; }
    }
}
