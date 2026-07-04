using System;

namespace SIE.MES.Workbench.Fpy
{
    /// <summary>
    /// 产线班次直通率
    /// </summary> 
    [Serializable]
    public class ResourceShiftFpy
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }
        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }
        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 直通率
        /// </summary>
        public decimal Fpy { get; set; }
        /// <summary>
        /// 班次日期
        /// </summary>
        public DateTime ShiftDate { get; set; }

        public ResourceShiftFpy()
        {
            Fpy = 0;
            ShiftDate = DateTime.Today;
        }


    }
}