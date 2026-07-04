using System;

namespace SIE.CrossPlatform.Collect.Models.Attributes
{

    /// <summary>
    /// 标签属性
    /// </summary>
#pragma warning disable S3993 // Custom attributes should be marked with "System.AttributeUsageAttribute"
#pragma warning disable S4060 // Non-abstract attributes should be sealed
    public class LabelAttribute : Attribute
#pragma warning restore S4060 // Non-abstract attributes should be sealed
#pragma warning restore S3993 // Custom attributes should be marked with "System.AttributeUsageAttribute"
    {
        public string Label { get; private set; }

        public LabelAttribute(string label)
        {
            Label = label;
        }
    }
}
