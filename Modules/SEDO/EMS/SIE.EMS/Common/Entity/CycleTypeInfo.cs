using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Common.Entity
{
    /// <summary>
    /// 周期信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("周期信息")]
    [DisplayMember(nameof(Value))]
    public class CycleTypeInfo : ViewModel
    {        
        #region 值 Value
        /// <summary>
        /// 值
        /// </summary>
        [Label("值")]
        public static readonly Property<string> ValueProperty = P<CycleTypeInfo>.Register(e => e.Value);

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
