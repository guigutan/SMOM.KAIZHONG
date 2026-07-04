using SIE.Core.ApiModels;
using SIE.LES.StockOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.MaterialReceptions.APIModels
{

    /// <summary>
    /// 扫描参数
    /// </summary>
    [Serializable]
    public class ScanParamters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword">当前扫描词</param>
        /// <param name="erroMes">错误信息</param>
        /// <param name="scanRecords">扫描记录</param>
        /// <param name="resourcesId">生产资源</param>
        /// <param name="oparetion">操作类型 1：按明细 2：按单</param>
        public ScanParamters(string keyword, string erroMes, List<MaterialReceptionInfo> scanRecords, double? resourcesId, int oparetion)
        {
            this.Oparetion = oparetion;
            this.Keyword = keyword;
            this.ErroMes = erroMes;
            this.ScanRecords = scanRecords;
            this.ResourcesId = resourcesId;
            this.NewRecords = new List<MaterialReceptionInfo>();
            Isvalidated = false;
            NeedGotoDetail = false;
            ObjectType = 0;
            StockOrderSnList = new List<StockOrderSn>();
            StockOrderListForSelect = new List<BaseDataInfo>();
            NewOrderRecords = new List<MaterialReceptionInfo>();
        }
        public ScanParamters()
        {

        }

        /// <summary>
        /// 扫描关键词
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErroMes { get; set; }

        /// <summary>
        /// 已扫记录
        /// </summary>
        public List<MaterialReceptionInfo> ScanRecords { get; set; }

        /// <summary>
        /// 本次新记录
        /// </summary>
        public List<MaterialReceptionInfo> NewRecords { get; set; }

        /// <summary>
        /// 本次的备料单信息列表
        /// </summary>
        public List<MaterialReceptionInfo> NewOrderRecords { get; set; }
        /// <summary>
        /// 所选资源
        /// </summary>
        public double? ResourcesId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourcesName { get; set; }

        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool Isvalidated { get; set; }

        /// <summary>
        /// 扫描对象类型
        /// </summary>
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// 需跳转明细
        /// </summary>
        public bool NeedGotoDetail { get; set; }

        /// <summary>
        /// 本次扫描出的明细行
        /// </summary>
        public List<StockOrderSn> StockOrderSnList { get; set; }

        /// <summary>
        /// 备料单
        /// </summary>
        public string StockOrderNo { get; set; }

        /// <summary>
        /// 操作类型 1：按明细 2：按单
        /// </summary>
        public int Oparetion { get; set; }

        /// <summary>
        /// 备选备料单
        /// </summary>
        public List<BaseDataInfo> StockOrderListForSelect { get; set; }
    }
}
