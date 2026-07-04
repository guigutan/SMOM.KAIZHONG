using SIE.Core.Barcodes;
using SIE.EventMessages.MES.PanelBindings.Models;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;

namespace SIE.MES.WIP.Models
{
    /// <summary>
    /// 拼板与条码关系数据
    /// </summary>
    [Serializable]
    public class PanelAndSnData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PanelAndSnData()
        {
            SnList = new List<SnData>();
        }

        /// <summary>
        /// 产品条码做拼板码
        /// </summary>
        public bool SnAsPanel { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 可绑定产品数量
        /// </summary>
        public int CandBindQty { get; set; }

        /// <summary>
        /// 叉板数
        /// </summary>
        public int ForkPlateQty { get; set; }

        #region 空拼板过站配置绑定SN
        /// <summary>
        /// 空拼板
        /// </summary>
        public bool IsEmptyPanel { get; set; }

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
                    return CandBindQty - ForkPlateQty;
                }
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// 拼板码绑定条码集合
        /// </summary>
        public List<SnData> SnList { get; }

        /// <summary>
        /// 组合板工单ID
        /// </summary>
        public double PanelWorkOrderId { get; set; }

        /// <summary>
        /// 清楚数据
        /// </summary>
        public virtual void Clear()
        {
            SnList.Clear();
            PanelCode = "";
            CandBindQty = ForkPlateQty = 0;
            IsEmptyPanel = false;
            BindingMode = null;
        }
    }

    /// <summary>
    /// 拼板绑定条码数据
    /// </summary>
    [Serializable]
    public class SnData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SnData()
        {
            //默认条码类型为SN(产品条码)
            BarcodeType = BarcodeType.SN;
        }

        /// <summary>
        /// 待绑定
        /// </summary>
        public bool ToBinding { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 子产品工单
        /// </summary>
        public double? ChildWorkOrderId { get; set; }

        /// <summary>
        /// 子产品物料ID
        /// </summary>
        public double ChildProductId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProducName { get; set; }

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType { get; set; }

        /// <summary>
        /// 板号
        /// </summary>
        public int BoardNo { get; set; }
    }

    /// <summary>
    /// SN绑定方式
    /// </summary>
    public enum BindingMode
    {
        /// <summary>
        /// 自动
        /// </summary>
        [Label("自动")]
        Auto,

        /// <summary>
        /// 手动
        /// </summary>
        [Label("手动")]
        Manual
    }
}
