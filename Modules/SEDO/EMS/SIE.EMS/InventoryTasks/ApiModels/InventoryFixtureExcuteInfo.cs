using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.InventoryTasks.ApiModels
{
    /// <summary>
    /// 工治具盘点执行信息
    /// </summary>
    [Serializable]
    public class InventoryFixtureExcuteInfo
    {
        private int qualityState;
        private bool isget;
        /// <summary>
        /// 构造函数
        /// </summary>
        public InventoryFixtureExcuteInfo(bool isGet)
        {
            if (isGet)
            {
                isget = isGet;
                this.ProduceState = new List<CheckBoxInfo>(){
                new CheckBoxInfo()
                {
                    Type = "正常".L10N(),
                    isBoole = false,
                    Value = 0
                },
                new CheckBoxInfo()
                {
                    Type = "信息变动".L10N(),
                    isBoole = false,
                    Value = 1
                },
                new CheckBoxInfo()
                {
                    Type = "盘亏".L10N(),
                    isBoole = false,
                    Value = 2
                }
            };

                this.QualityList = new List<CheckBoxInfo>()
            {
                new CheckBoxInfo()
                {
                    Type= "在库".L10N(),
                    isBoole= false,
                    Value= 5
                },
                new CheckBoxInfo()
                {
                     Type="在线".L10N(),
                     isBoole= false,
                     Value=10
                }
            };
            }
        }
        /// <summary>
        /// 数据库Id(明细明细ID)
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 序列号ID
        /// </summary>
        public double AccountId { get; set; }
        /// <summary>
        /// 是否盲盘
        /// </summary>
        public bool IsBlind { get; set; }

        /// <summary>
        /// PDA操作类型
        /// </summary>
        public int PdaType { get; set; }

        /// <summary>
        /// 盘点任务Id
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 在库数
        /// </summary>
        public int StockQty { get; set; }

        /// <summary>
        /// 在库合格数
        /// </summary>
        public int StockPassQty { get; set; }

        /// <summary>
        /// 在库不合格数
        /// </summary>
        public int StockNgQty { get; set; }

        /// <summary>
        /// 在线数
        /// </summary>
        public int OnlineQty { get; set; }

        /// <summary>
        /// 管理方式
        /// </summary>
        public int ManageMode { get; set; }

        /// <summary>
        /// 工治具状态
        /// </summary>
        public int QualityState
        {
            get { return qualityState; }
            set
            {
                qualityState = value;
     /*           if (isget)
                {
                    this.QualityList.ForEach(item =>
                    {
                        item.isBoole = item.Value == qualityState;
                    });
                }*/

            }
        }

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn { get; set; }


        /// <summary>
        /// 工治具编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///型号名称
        /// </summary>
        public string ModelName { get; set; }


        /// <summary>
        /// 1-正常 2-清空
        /// </summary>
        public int CountStatus { get; set; }

        /// <summary>
        /// 盘点结果 '盘亏', '正常', '信息变动','盘盈' 前端变色使用
        /// </summary>
        public string CountStr { get; set; }

        /// <summary>
        /// 实盘在库合格数
        /// </summary>
        public string InputPassQty { get; set; }

        /// <summary>
        /// 实盘在库不合格数
        /// </summary>
        public string InputNgQty { get; set; }

        /// <summary>
        /// 实盘在线数
        /// </summary>
        public string InputOnlineQty { get; set; }


        /// <summary>
        /// 盘点结果 用于单选框
        /// </summary>
        public List<CheckBoxInfo> ProduceState { get; set; }

        /// <summary>
        /// 质量状态列表
        /// </summary>
        public List<CheckBoxInfo> QualityList { get; set; }

        /// <summary>
        /// 盘点执行类型
        /// </summary>
        public string InventoryType { get; set; }
    }
}
