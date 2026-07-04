using SIE.Core.Enums;
using System;
using System.Collections.Generic;

namespace SIE.EMS.InventoryTasks.ApiModels
{
    /// <summary>
    /// 备件盘点任务信息
    /// </summary>
    [Serializable]
    public class InventorySparePartInfo
    {
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
        /// 仓库
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 需盘点
        /// </summary>
        public int NeedInventory { get; set; }

        /// <summary>
        /// 未盘点
        /// </summary>
        public int NoInventory { get; set; }

        /// <summary>
        /// 盘点单的工厂ID
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 是否强制拍照
        /// </summary>
        public bool NeedPhoto { get; set; }

        /// <summary>
        /// 盘点说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否超期
        /// </summary>
        public bool IsOverdue { get; set; }

        /// <summary>
        /// 盘点状态
        /// </summary>
        public string InventoryTaskStatus { get; set; }

        /// <summary>
        /// 图片内容
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartType { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string ItemCategoryName { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public string Progress { get; set; }

    }

    /// <summary>
    /// 备件盘点执行信息
    /// </summary>
    [Serializable]
    public class InventorySparePartExcuteInfo
    {

        private int qualityState;
        private bool isget;
        /// <summary>
        /// 构造函数
        /// </summary>
        public InventorySparePartExcuteInfo(bool isGet)
        {
            if (isGet)
            {
                isget = isGet;
                this.ProduceState = new List<CheckBoxInfo>()
            {
                new CheckBoxInfo()
                {
                    Type = "正常",
                    isBoole = false,
                    Value = 0
                },
                new CheckBoxInfo()
                {
                    Type = "信息变动",
                    isBoole = false,
                    Value = 1
                },
                new CheckBoxInfo()
                {
                    Type = "盘亏",
                    isBoole = false,
                    Value = 2
                }
            };

                this.QualityList = new List<CheckBoxInfo>()
            {
                new CheckBoxInfo()
                {
                    Type= "良品",
                    isBoole= false,
                    Value= 5
                },
                new CheckBoxInfo()
                {
                     Type="不良品",
                     isBoole= true,
                     Value=10
                }
            };
            }
        }

        /*
         ProduceState: [{
    Type: "正常",
    isBoole: false,
    Value: 0
  },
  {
    Type: "信息变动",
    isBoole: false,
    Value: 1
  },
  {
    Type: "盘亏",
    isBoole: false,
    Value: 2
  }
  ],
  QualityList: [{
    Type: "良品",
    isBoole: false,
    Value: 5
  }, 
         * 
         
         */
        /// <summary>
        /// 数据库Id
        /// </summary>
        public double Id { get; set; }
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
        /// 良品数
        /// </summary>
        public int PassQty { get; set; }

        /// <summary>
        /// 不良品数
        /// </summary>
        public int NgQty { get; set; }

        /// <summary>
        /// 管理方式
        /// </summary>
        public int ControlMode { get; set; }

        /// <summary>
        /// 质量状态
        /// </summary>
        public int QualityState
        {
            get { return qualityState; }
            set
            {
                qualityState = value;
                if (isget)
                {
                    this.QualityList.ForEach(item =>
                    {
                        item.isBoole = item.Value == qualityState;
                    });
                }

            }
        }

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn { get; set; }


        /// <summary>
        /// 备件编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// 1-正常 2-清空
        /// </summary>
        public int CountStatus { get; set; }

        /// <summary>
        /// 盘点结果 '盘亏', '正常', '信息变动','盘盈' 前端变色使用
        /// </summary>
        public string CountStr { get; set; }

        /// <summary>
        /// 实盘良品数
        /// </summary>
        public string InputPassQty { get; set; }

        /// <summary>
        /// 实盘不良品数
        /// </summary>
        public string InputNgQty { get; set; }


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


    /// <summary>
    /// 质量信息
    /// </summary>

    [Serializable]
    public class CheckBoxInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool isBoole { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }


    /// <summary>
    /// PDA端质量状态
    /// </summary>
    public enum PdaQualityState
    {

        /// <summary>
        /// 良品
        /// </summary>
        Pass = 5,

        NG = 10
    }

}
