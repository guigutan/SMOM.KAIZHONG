namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
	/// 备注接口
	/// </summary>
	public interface INote : IChildElement
    {
        /// <summary>
        /// 宽
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// 活动节点
        /// </summary>
        IActivity Activity { get; set; }
    }
}
