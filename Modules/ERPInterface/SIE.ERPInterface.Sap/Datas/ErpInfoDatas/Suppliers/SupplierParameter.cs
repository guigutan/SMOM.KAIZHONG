using SapNwRfc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// 供应商下载参数
    /// </summary>
    public class SupplierParameter
    {
        /// <summary>
        ///  查询参数
        /// </summary>
        public Selection Selection { get; set; }
    }

    /// <summary>
    ///  参数字段
    /// </summary>
    public class Selection
    {
        /// <summary>
        /// 供应商分类
        /// </summary>
        [SapName("ZGYSFL")]
        public string SupplierType { get; set; }

        /// <summary>
        /// 外部系统中的业务伙伴编号
        /// </summary>
        [SapName("BPEXT")]
        public string ExtCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [SapName("NAME1")]
        public string Name { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [SapName("ERDAT")]
        public DateTime? LastUpdateTime { get; set; }
    }
}
