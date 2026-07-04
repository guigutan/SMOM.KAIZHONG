namespace SIE.Wpf.Tech.Processs
{
    /// <summary>
    /// 树信息（表示工序树、工艺路线树）
    /// </summary>
    public class TreeInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public double? Pid { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 实体ID
        /// </summary>
        public double? EntityId { get; set; }
    }
}
