using SIE.ObjectModel;
using System;
using System.Xml.Serialization;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 元素模型
    /// </summary>
    [Serializable]
    public class ElementModel : ObservableObject, IElement
    {
        /// <summary>
        /// 元素模型
        /// </summary>
        protected ElementModel()
        {
            Id = Guid.NewGuid().ToString("N");
            Self = this;
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "IsSelected")
            {
                base.OnPropertyChanged("Self");
                if (IsSelected && SelectedEvent != null)
                {
                    SelectedEvent(this);
                }
                else if (!IsSelected && UnselectedEvent != null)
                {
                    UnselectedEvent(this);
                }
                else
                {
                    //
                }
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 复制前原Id
        /// </summary>
        public string CopyId
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public ElementState State
        {
            get { return GetProperty<ElementState>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 元素接口
        /// </summary>
        [XmlIgnore]
        public IElement Self
        {
            get { return GetProperty<IElement>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 选中事件
        /// </summary>
        public event Action<IElement> SelectedEvent;

        /// <summary>
        /// 取消选中事件
        /// </summary>
        public event Action<IElement> UnselectedEvent;

        /// <summary>
        /// 保存验证
        /// </summary>
        public virtual void ValidateSave()
        {
        }

        /// <summary>
        /// 反序列化工艺路线
        /// </summary>
        /// <param name="xml">序列化xml</param>
        /// <param name="isCopy"></param>
        public virtual void Deserialize(string xml, bool isCopy = false)
        {
        }

        /// <summary>
        /// 序列化工艺路线
        /// </summary>
        /// <returns>序列化xml</returns>
        public virtual string Serialize()
        {
            return string.Empty;
        }
    }
}
