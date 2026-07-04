using SIE.ObjectModel;

namespace SIE.Resources.ProcessTechTypes
{
    /// <summary>
    /// 算法类型
    /// </summary>
    public enum AlgorithmMarking
    {
        /// <summary>
        /// 普通
        /// </summary>
        [Label("普通")]
        NORMAL = 0,

        /// <summary>
        /// 注塑
        /// </summary>
        [Label("注塑")]
        MOLDING = 1,

        ///// <summary>
        ///// 包边
        ///// </summary>
        //[Label("包边")]
        //Hemming = 2,
    }
}
