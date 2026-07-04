using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.InventoryTasks.ApiModels
{
    /// <summary>
    /// 工治具清单
    /// </summary>
    public class InventoryFixtureListInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public InventoryFixtureListInfo()
        {
            this.QualityList = new List<CheckBoxInfo>()
            {
                new CheckBoxInfo()
                {
                    Type = "在库".L10N(),
                    isBoole = false,
                    Value = 5
                },
                new CheckBoxInfo()
                {
                    Type = "在线".L10N(),
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
        /// 工治具ID
        /// </summary>
        public double FixtureEncodeId { get; set; }

        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 管控方式
        /// </summary>
        public int ManageMode { get; set; }
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
        /// 序列号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 盘点结果
        /// </summary>
        public string InvResult { get; set; }

        /// <summary>
        ///良品数
        /// </summary>
        public int PassQty { get; set; }

        /// <summary>
        /// 不良品数
        /// </summary>
        public int NgQty { get; set; }
        /// <summary>
        /// 工治具状态 序列号用
        /// </summary>
        public List<CheckBoxInfo> QualityList { get; set; }


    }
}
