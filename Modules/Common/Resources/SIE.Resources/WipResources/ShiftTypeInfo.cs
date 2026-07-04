using SIE.Resources.ShiftTypes;
using System;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 工作日信息，用于显示日历方案
    /// </summary>
    [Serializable]
    public class ShiftTypeInfo
    {
        /// <summary>
        /// 属性初始化
        /// </summary>
        public ShiftTypeInfo()
        {
            Content = string.Empty;
            IsHoliday = false;
            IsActived = false;
        }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 班制
        /// </summary>
        public ShiftType ShiftType { get; set; }

        /// <summary>
        /// 是否法定节假日或休息日，设置休息图标
        /// </summary> 
        public bool IsHoliday { get; set; }

        /// <summary>
        /// 是否启用，且工作日大于当天
        /// </summary> 
        public bool IsActived { get; set; }
    }
}
