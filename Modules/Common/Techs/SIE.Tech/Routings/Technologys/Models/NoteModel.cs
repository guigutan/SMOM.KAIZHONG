using System;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 规则模型
    /// </summary>
    [Serializable]
    public class NoteModel : ChildElementModel, INote
    {
        #region 属性
        /// <summary>
        /// 活动
        /// </summary>
        public IActivity Activity
        {
            get { return GetProperty<IActivity>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 高
        /// </summary>
        public double Height
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 宽
        /// </summary>
        public double Width
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        #endregion
    }
}
