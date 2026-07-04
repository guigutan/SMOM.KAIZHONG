using System;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 子元素模型
    /// </summary>
    [Serializable]
    public class ChildElementModel : ElementModel, IChildElement
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected ChildElementModel()
        {
            ZIndex = 1;
        }

        /// <summary>
        /// 控件
        /// </summary>
        public object Control
        {
            get { return GetProperty<object>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// Z-索引
        /// </summary>
        public int ZIndex
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        public virtual void Delete()
        {
        }

        /// <summary>
        /// 调整变焦深度
        /// </summary>
        /// <param name="deep">深度</param>
        public void Zoom(double deep)
        {
            throw new NotImplementedException();
        }
    }
}
