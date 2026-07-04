using System;

namespace SIE.XPCJ.Models.Attributes
{

    /// <summary>
    /// 标签属性
    /// </summary>
    public class LabelAttribute : Attribute
    {
        public string Label { get; private set; }

        public LabelAttribute(string label)
        {
            Label = label;
        }
    }
}
