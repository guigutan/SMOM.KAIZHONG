using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Core.Common.Models
{
    /// <summary>
    /// 导出数据表
    /// </summary>
    [Serializable]
    public class ExportDataTable
    {
        /// <summary>
        /// 导出列
        /// </summary>
        public List<string[]> Columns { get; } = new List<string[]>();

        /// <summary>
        /// 页签名称
        /// </summary>
        public List<string> SheetNames { get; } = new List<string>();

        /// <summary>
        /// 导出数据表
        /// </summary>
        public List<DataTable> Tables { get; } = new List<DataTable>();
    }
}