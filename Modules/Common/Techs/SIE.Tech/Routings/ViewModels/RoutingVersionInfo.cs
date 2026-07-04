using System;

namespace SIE.Tech.Routings.ViewModels
{
    /// <summary>
    /// 工艺路线版本信息
    /// </summary>
    [Serializable]
    public class RoutingVersionInfo
    {
        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工艺路线版本内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否叶子
        /// </summary>
        public bool Leaf { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public string Nodetype { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double RoutingId { get; set; }

        /// <summary>
        /// 工艺流程状态
        /// </summary>
        public RoutingState State { get; set; }

        /// <summary>
        /// 工艺路线是否默认
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 是否复制
        /// </summary>
        public bool IsCopy { get; set; }

        /// <summary>
        /// 工艺路线版本名称
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// 目标工艺路线Id
        /// </summary>
        public double TargetRoutingId { get; set; }
    }
}
