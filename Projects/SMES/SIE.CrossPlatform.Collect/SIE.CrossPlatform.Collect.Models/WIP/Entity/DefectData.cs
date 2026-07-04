using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP.Entity
{
    /// <summary>
    /// 缺陷数据
    /// </summary>
    [Serializable]
    public class DefectData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefectData()
        {
            Qty = 1;
        }

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 板号
        /// </summary>
        public int? BoardNo { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public double DefectId { get; set; }

        /// <summary>
        /// 缺陷代码编码
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 缺陷代码分类ID
        /// </summary>
        public double CategoryId { get; set; }

        /// <summary>
        /// 缺陷代码分类编码
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        public double Qty { get; set; }

        /// <summary>
        /// 检验项目
        /// </summary>
        public double InspectionItemId { get; set; }


        /// <summary>
        /// 工序
        /// </summary>
        public double? ProcessId { get; set; }
        /// <summary>
        /// 是否误判
        /// </summary>
        public bool IsMistake { get; set; }
        /// <summary>
        /// 是否新增    
        /// </summary>
        public bool IsAdd { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }
    }
}
