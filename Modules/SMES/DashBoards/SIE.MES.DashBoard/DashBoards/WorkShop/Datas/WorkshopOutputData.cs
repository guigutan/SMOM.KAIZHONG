using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.DashBoards.WorkShop.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class WorkshopOutputData
    {
        /// <summary>
        /// 当班计划
        /// </summary>
        public decimal ShiftPlanQty { get; set; }

        /// <summary>
        /// 累计计划
        /// </summary>
        public decimal AccPlannedQty { get; set; }

        /// <summary>
        /// 累计实际
        /// </summary>
        public decimal AccActualQty { get; set; }

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal DiffQty { get; set; }

        /// <summary>
        /// 安全天数
        /// </summary>
        public int SafetyDateNum { get; set; }

        /// <summary>
        /// 看板底部列表数据
        /// </summary>
        public List<WorkshopOutputBottomData> grid1 { get; set; } = new List<WorkshopOutputBottomData>();

        /// <summary>
        /// 看板右边列表数据
        /// </summary>
        public List<WorkshopOutputRightData> grid2 { get; set; } = new List<WorkshopOutputRightData>();
    }

    /// <summary>
    /// 看板底部列表数据
    /// </summary>
    [Serializable]
    public class WorkshopOutputBottomData
    { 
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 计划数
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 实际数量
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal DiffQty { get; set; }
    }

    /// <summary>
    /// 看板右边列表数据
    /// </summary>
    [Serializable]
    public class WorkshopOutputRightData
    { 
        /// <summary>
        /// 产线
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// 计划数
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 实际数量
        /// </summary>
        public decimal ActualQty { get; set; }
    }
}
