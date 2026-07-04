using System;
using System.Collections.Generic;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
	/// 工艺路线设计接口
	/// </summary>
	public interface IContainer : IElement
    {
        /// <summary>
        /// 选中元素变更事件
        /// </summary>
        event Action<IElement> SelectedElementChanged;

        /// <summary>
        /// 添加子元素
        /// </summary>
        /// <param name="child">元素</param>
        void AddChild(IChildElement child);

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="child">元素</param>
        void RemoveChild(IChildElement child);

        /// <summary>
        /// 删除选中元素
        /// </summary>
        void DeleteSelectedElement();

        /// <summary>
        /// 水平居中
        /// </summary>
        void HorizontalCenter();

        /// <summary>
        /// 垂直居中
        /// </summary>
        void VerticalCenter();

        /// <summary>
        /// 横向分布
        /// </summary>
        void HorizontalDistribution();

        /// <summary>
        /// 纵向分布
        /// </summary>
        void VerticalDistribution();

        /// <summary>
        /// 选中元素
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="isMultiSelected">是否多选</param>
        /// <param name="isClear">是否清除选中元素</param>
        void SelectedElement(IElement element, bool isMultiSelected = false, bool isClear = true);

        /// <summary>
        /// 获取索引
        /// </summary>
        /// <returns>索引</returns>
        int NextIndex();

        /// <summary>
        /// 元素集合
        /// </summary>
        IEnumerable<IChildElement> Children { get; }

        /// <summary>
        /// 活动集合
        /// </summary>
        IList<IActivity> Activitys { get; set; }

        /// <summary>
        /// 规则集合
        /// </summary>
        IList<IRule> Rules { get; set; }

        /// <summary>
        /// 备注集合
        /// </summary>
        IList<INote> Notes { get; set; }

        /// <summary>
        /// 选中元素集合
        /// </summary>
        IList<IElement> SelectElements { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        double RoutingId { get; set; }

        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        double RoutingVersionId { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// 选中元素
        /// </summary>
        IElement SelectElement { get; set; }

        /// <summary>
        /// 显示网格
        /// </summary>
        bool ShowGridLine { get; set; }

        /// <summary>
        /// 变焦深
        /// </summary>
        double ZoomDeep { get; set; }

        /// <summary>
        /// 移动选中项
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        void MoveSelectItems(double x, double y);
    }
}
