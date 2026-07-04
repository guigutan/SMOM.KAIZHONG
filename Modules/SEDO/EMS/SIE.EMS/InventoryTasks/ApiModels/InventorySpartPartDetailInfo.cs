using SIE.EMS.SpareParts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.InventoryTasks.ApiModels
{
    /// <summary>
    ///备件盘点明细
    /// </summary>
    [Serializable]
    public class InventorySpartPartDetailInfo
    {
       /// <summary>
       /// 数据库Id
       /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 初盘1 复盘2
        /// </summary>
        public int PdaType { get; set; }


        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 计划完成日期
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 盘点类型
        /// </summary>
        public string InventoryType { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SpartType { get; set; }

        /// <summary>
        /// 分类层级
        /// </summary>
        public string ItemCategoryName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 管控方式
        /// </summary>
        public string ControlMethod { get; set; }

        /// <summary>
        /// 管控方式值
        /// </summary>
        public ControlMethod ControlMethodValue { get; set; }


        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo { get; set; }


        /// <summary>
        /// Sn
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 图片内容
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 图片文件名称
        /// </summary>
        public string PictureFileName { get; set; }

        /// <summary>
        /// 良品数
        /// </summary>
        public int GoodQty { get; set; }

        /// <summary>
        /// 不良品数
        /// </summary>
        public int NgQty { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 实盘不良品
        /// </summary>
        public int? RealNg { get; set; }

        /// <summary>
        /// 实盘良品数
        /// </summary>
        public int? RealGood { get; set; }

        /// <summary>
        /// 实盘总数
        /// </summary>
        public int? RealTotal { get; set; }
        /// <summary>
        /// 进度
        /// </summary>
        public string Progress { get; set; }

        /// <summary>
        /// 备件图片列表
        /// </summary>
        public List<string> PhotoBase64List { get; set; } = new List<string>();
    }
}
