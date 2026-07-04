using SIE.Common;
using SIE.Core.Barcodes;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Products;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 产品信息
    /// </summary>
    [Serializable]
    public class ProductInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductInfo()
        {
            Context = new CollectionContext();
        }

        /// <summary>
        /// 产品Puid
        /// </summary>
        public string Puid { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 拼板码信息
        /// </summary>
        public PanelInfo PanelInfo { get; set; } = new PanelInfo();

        /// <summary>
        /// 上下文
        /// </summary>
        public CollectionContext Context { get; }

        /// <summary>
        /// 过站记录状态
        /// </summary>
        public WipProductProcessState WipProductProcessState { get; set; }

        /// <summary>
        /// 采集结果 Result (上一次采集的采集结果)
        /// </summary>
        public ResultType? LastResultType { get; set; }
    }

    /// <summary>
    /// 拼板码信息
    /// </summary>
    [Serializable]
    public class PanelInfo
    {
        /// <summary>
        /// 工序是否绑定
        /// </summary> 
        public bool IsBinding { get; set; }

        /// <summary>
        /// 拼板码可绑定产品数量
        /// </summary>
        public int CanBindQty { get; set; }

        /// <summary>
        /// 拼板号
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 叉板数
        /// </summary>
        public int ForkPlateQty { get; set; }

        /// <summary>
        /// 拼板码绑定条码集合
        /// </summary>
        public List<SnData> SnList { get; } = new List<SnData>();

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            IsBinding = false;
            CanBindQty = ForkPlateQty = 0;
            SnList.Clear();
        }
    }

    /// <summary>
    /// 采集过站拼板码信息
    /// </summary>
    [Serializable]
    public class SubmitPanelInfo
    {

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType? BarcodeType { get; set; }

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 拼板数
        /// </summary>
        public int PanelQty { get; set; }

        /// <summary>
        /// 叉板数
        /// </summary>
        public int ForkPlateQty { get; set; }

        /// <summary>
        /// SN绑定方式
        /// 可空标识不需要绑定
        /// </summary>
        public BindingMode? BindingMode { get; set; }

        /// <summary>
        /// 剩余待绑定数量
        /// </summary>
        public int ToBindingQty
        {
            get
            {
                if (BindingMode == Models.BindingMode.Manual)
                {
                    return NeetToBindingQty - SnList.Count;
                }
                return 0;
            }
        }

        /// <summary>
        /// 是否需要绑定SN
        /// </summary>
        public bool NeetToBindingSn
        {
            get
            {
                if (BindingMode == Models.BindingMode.Manual)
                {
                    return ToBindingQty > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// 总共需要绑定的SN数量
        /// </summary>
        public int NeetToBindingQty
        {
            get
            {
                if (BindingMode == Models.BindingMode.Manual)
                {
                    return PanelQty - ForkPlateQty;
                }
                return 0;
            }
        }

        /// <summary>
        /// 拼板码绑定条码集合
        /// </summary>
        public List<SnData> SnList { get; } = new List<SnData>();

        /// <summary>
        /// 清楚数据
        /// </summary>
        public virtual void Clear()
        {
            SnList.Clear();
            PanelCode = "";
            PanelQty = ForkPlateQty = 0;
            BindingMode = null;
        }
    }
}