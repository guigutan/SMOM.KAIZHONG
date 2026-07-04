namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
	/// 子元素
	/// </summary>
	public interface IChildElement : IElement
    {
        /// <summary>
        /// Z-索引
        /// </summary>
        int ZIndex { get; set; }

        /// <summary>
        /// 控件
        /// </summary> 
        object Control { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 调整变焦深度
        /// </summary>
        /// <param name="deep">深度</param>
        void Zoom(double deep);

        /// <summary>
        /// 删除
        /// </summary>
        void Delete();
    }
}
