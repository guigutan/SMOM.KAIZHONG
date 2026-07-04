using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.InventoryTasks.ApiModels
{

    /// <summary>
    /// 备件清单列表
    /// </summary>
    [Serializable]
    public class InventorySparePartListInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public InventorySparePartListInfo()
        {
            this.QualityList = new List<CheckBoxInfo>()
            {
                new CheckBoxInfo()
                {
                    Type = "良品",
                    isBoole = false,
                    Value = 5
                },
                new CheckBoxInfo()
                {
                    Type = "不良品",
                    isBoole = false,
                    Value = 10
                }
            };
        }
        /// <summary>
        /// 数据库Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 初盘1 复盘2
        /// </summary>
        public int PdaType { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 管控方式
        /// </summary>
        public int ControlMethod { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 未盘点数
        /// </summary>
        public int NotInvQty { get; set; }

        /// <summary>
        /// 是否已盘点
        /// </summary>
        public bool IsFinishInventory { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 盘点结果
        /// </summary>
        public string InvResult { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        ///良品数
        /// </summary>
        public int PassQty { get; set; }

        /// <summary>
        /// 不良品数
        /// </summary>
        public int NgQty { get; set; }
        /// <summary>
        /// 质量列表 序列号用
        /// </summary>
        public List<CheckBoxInfo> QualityList { get; set; }

    }
}
