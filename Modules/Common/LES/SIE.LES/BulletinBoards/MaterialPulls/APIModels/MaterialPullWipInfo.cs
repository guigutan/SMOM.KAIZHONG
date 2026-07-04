using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.BulletinBoards.MaterialPulls.APIModels
{
    /// <summary>
    /// 物料拉动(生产)API数据实体
    /// </summary>
    public class MaterialPullWipInfo
    {
        /// <summary>
        /// 备料单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 备料单号+行号
        /// </summary>
        public string NoLine { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 需求物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 本次需求量
        /// </summary>
        public decimal NeedQty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 需求时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 发货仓信息
        /// </summary>
        public List<GoodWare> GoodWareInfo { get; set; }
    }

    /// <summary>
    /// 发货仓信息
    /// </summary>
    [Serializable]
    public class GoodWare
    {
        /// <summary>
        /// 发货仓
        /// </summary>
        public string Ware { get; set; }

        /// <summary>
        /// 可接收
        /// </summary>
        public decimal ToReceive { get; set; }

        /// <summary>
        /// 已接收
        /// </summary>
        public decimal Received { get; set; }

        /// <summary>
        /// 需配送数
        /// </summary>
        public decimal Send { get; set; }

        /// <summary>
        /// 需自提数
        /// </summary>
        public decimal Pick { get; set; }
    }

    /// <summary>
    /// 临时记录发货仓库Id + 发货数 + 接收数 + 需配送数 + 需自提数
    /// </summary>
    [Serializable]
    public class TempDeliverInfo
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public string  Ware { get; set; }

        /// <summary>
        /// 发货数
        /// </summary>
        public decimal Send { get; set; }

        /// <summary>
        /// 接收数
        /// </summary>
        public decimal Receive { get; set; }

        /// <summary>
        /// 需配送数
        /// </summary>
        public decimal NeedSend { get; set; }

        /// <summary>
        /// 需自提数
        /// </summary>
        public decimal NeedPick { get; set; }
    }
}
