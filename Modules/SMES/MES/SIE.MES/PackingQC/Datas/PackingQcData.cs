using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC.Datas
{
    /// <summary>
    /// QC确认查询参数
    /// </summary>
    [Serializable]
    public class PackingQcData
    {
        /// <summary>
        /// 主表id
        /// </summary>
        public double? Id { get;set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public double? WipResId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string WipResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string WipResName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 旧物料编码
        /// </summary>
        public string ItemOldCode { get; set; }

        /// <summary>
        /// 已装总量(格式：已装数/总数)
        /// </summary>
        public string InstalledQty { get; set; }

        /// <summary>
        /// 箱号（等同于蓝标标签号）
        /// </summary>
        public string BoxCode { get; set; }
    }

    /// <summary>
    /// QC确认提交参数
    /// </summary>
    [Serializable]
    public class SubmitPackingQcData
    {
        /// <summary>
        /// 文件内容
        /// </summary>
        public string FileContent { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

    }
}
