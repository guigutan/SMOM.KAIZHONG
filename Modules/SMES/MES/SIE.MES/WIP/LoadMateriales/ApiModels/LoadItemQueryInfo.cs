using SIE.Defects;
using SIE.Domain;
using SIE.MES.WIP.ApiModels;
using SIE.MES.WIP.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WIP.LoadMateriales.ApiModels
{
    /// <summary>
    /// 上料查询信息
    /// </summary>
    [Serializable]
    public class LoadItemQueryInfo : WipQueryInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadItemQueryInfo()
        {
            AssemblyItemsDictionary = new Dictionary<string, LoadItemBarcodeInfo>();
            AssemblyDetailList = new List<AssemblyDetailViewModel>();
        }
        /// <summary>
        ///是否在上料 true是 false 在采集装配
        /// </summary>
        public bool IsLoadItem { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// SN已采集
        /// </summary>
        public bool IsSnCollected { get; set; }

        /// <summary>
        /// 采集工序状态
        /// </summary>
        public WipProductProcessState WipProductProcessState { get; set; }

        /// <summary>
        /// 上料字典集合
        /// </summary>
        public Dictionary<string, LoadItemBarcodeInfo> AssemblyItemsDictionary { get; set; }

        /// <summary>
        /// 装配清单
        /// </summary>
        public List<AssemblyDetailViewModel> AssemblyDetailList { get; set; }

        /// <summary>
        /// 多上料标签时 所选的物料标签条码Id
        /// </summary>
        public double SelectedBarcodeInfoId { get; set; }
    }

    /// <summary>
    ///上料采集返回信息
    /// </summary>
    [Serializable]
    public class RstLoadMaterialesInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RstLoadMaterialesInfo()
        {
            this.AssemblyDetailViewModels = new List<AssemblyDetailViewModel>();
            this.BarcodeInfos = new List<LoadItemBarcodeInfo>();
            this.LoadItemInfos=new List<LoadItemInfo> ();
            this.AssemblyItemsDictionary = new Dictionary<string, LoadItemBarcodeInfo>();
        }
        /// <summary>
        /// 是否需要选择条码
        /// </summary>
         public bool IsNeetToSelectedBarcodeInfo { get; set; }

        /// <summary>
        /// 采集返回信息
        /// </summary>
        public RstWipInfo RstWipInfo { get; set; }

        /// <summary>
        /// 装配清单
        /// </summary>
        public List<AssemblyDetailViewModel> AssemblyDetailViewModels { get; set; }

        /// <summary>
        /// 上料字典集合
        /// </summary>
        public Dictionary<string, LoadItemBarcodeInfo> AssemblyItemsDictionary { get; set; }

        /// <summary>
        /// 待选择物料标签
        /// </summary>
        public List<LoadItemBarcodeInfo> BarcodeInfos { get; set; }


        /// <summary>
        /// 上料列表
        /// </summary>
        public List<LoadItemInfo> LoadItemInfos { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProduceName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProduceCode { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProduceModel { get; set; }

        /// <summary>
        /// 当班采集数
        /// </summary>
        public decimal CollectQty { get; set; }

        /// <summary>
        /// 是否成功状态
        /// </summary>
        public bool IsOK { get; set; }
    }
}
