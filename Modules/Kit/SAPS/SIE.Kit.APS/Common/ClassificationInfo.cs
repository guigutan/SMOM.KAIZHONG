using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.APS.Common
{
    /// <summary>
    /// 分类值信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("分类值信息")]
    [DisplayMember(nameof(Value))]
    public class ClassificationInfo : ViewModel
    {
        #region 键 Key
        /// <summary>
        /// 键
        /// </summary>
        [Label("键")]
        public static readonly Property<string> KeyProperty = P<ClassificationInfo>.Register(e => e.Key);

        /// <summary>
        /// 键
        /// </summary>
        public string Key
        {
            get { return GetProperty(KeyProperty); }
            set { SetProperty(KeyProperty, value); }
        }
        #endregion

        #region 值 Value
        /// <summary>
        /// 值
        /// </summary>
        [Label("值")]
        public static readonly Property<string> ValueProperty = P<ClassificationInfo>.Register(e => e.Value);

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion
    }
}
