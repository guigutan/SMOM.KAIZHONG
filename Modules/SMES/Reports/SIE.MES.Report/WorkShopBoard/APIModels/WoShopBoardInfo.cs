using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Report.WorkShopBoard.APIModels
{
    /// <summary>
    /// 车间工单信息
    /// </summary>
    public class WoShopOrderInfo
    {
        /// <summary>
        /// 工单状况
        /// </summary>
        public string Situation { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 今日计划产量
        /// </summary>
        public decimal PlanProduce { get; set; }

        /// <summary>
        /// 实际产量
        /// </summary>
        public decimal ActualProduce { get; set; }

        /// <summary>
        /// 在制工单
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 计划达成率%
        /// </summary>
        public decimal PlanRate { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty { get; set; }

        /// <summary>
        /// 直通率
        /// </summary>
        public double FPY { get; set; }

        /// <summary>
        /// 一次良品数
        /// </summary>
        public decimal? PassQty { get; set; }

        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal? InputQty { get; set; }
    }

    /// <summary>
    /// 车间工单产出信息
    /// </summary>
    [Serializable]
    public class ProductOutput
    {
        /// <summary>
        /// 产量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public int HourDate { get; set; }
    }

    /// <summary>
    /// 直通率信息
    /// </summary>
    [Serializable]
    public class PassRate
    {
        /// <summary>
        /// 过板数
        /// </summary>
        public decimal InputQty { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal PassQty { get; set; }
    }

    /// <summary>
    /// 缺陷统计
    /// </summary>
    [Serializable]
    public class DefectCount
    {
        /// <summary>
        ///  缺陷代码
        /// </summary>
        public string DefectCode { get; set; }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public decimal Count { get; set; }
    }
}
