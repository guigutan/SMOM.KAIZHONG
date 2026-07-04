using SapNwRfc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// SAP供应商信息返回结果
    /// </summary>
    public class SupplierResult
    {
        /// <summary>
        /// 供应商信息集合
        /// </summary>
        [SapName("ITEM")]
        public SupplierInfo[] SupplierList { get; set; }
    }
}
