using System;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
	/// 元素接口
	/// </summary>
	public interface IElement
    {
        /// <summary>
        /// 选中后事件
        /// </summary>
        event Action<IElement> SelectedEvent;

        /// <summary>
        /// 取消选中事件
        /// </summary>
        event Action<IElement> UnselectedEvent;

        /// <summary>
        /// 保存验证
        /// </summary>
        void ValidateSave();

        /// <summary>
        /// 序列化元素
        /// </summary>
        /// <returns>序列化xml</returns>
        string Serialize();

        /// <summary>
        /// 反序列元素
        /// </summary>
        /// <param name="xml">序列化xml</param>
        /// <param name="isCopy">是否复制</param>
        void Deserialize(string xml, bool isCopy = false);

        /// <summary>
        /// 元素状态
        /// </summary>
        ElementState State { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// 元素接口
        /// </summary>
        IElement Self { get; set; }
    }
}
