namespace SIE.Wpf.MES.Workbench.CapacityDistributions
{
    /// <summary>
    /// 产线情况分布
    /// </summary>
    public class SeriesViewData : ObjectModel.ObservableObject
    {
        /// <summary>
        /// 时间参数
        /// </summary>
        public int Argu { get; set; }

        /// <summary>
        /// 开始值
        /// </summary>
        public double StartValue { get; set; }

        /// <summary>
        /// 目标值
        /// </summary>
        public double PlanValue { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public double ActualValue { get; set; }

        /// <summary>
        /// 目标值2
        /// </summary>
        public double PlanValueTwo { get; set; }
    }
}
