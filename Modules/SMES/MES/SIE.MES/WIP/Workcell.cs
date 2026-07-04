using System;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 工作单元信息
    /// </summary>
    [Serializable]
    public partial class Workcell
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Workcell() { Context = new CollectionContext(); }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipmentId { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public CollectionContext Context { get; }
    }

    /// <summary>
    /// 采集上下文，用于扩展
    /// </summary>
    [Serializable]
    public class CollectionContext : System.Collections.Specialized.HybridDictionary
    {
    }
}
