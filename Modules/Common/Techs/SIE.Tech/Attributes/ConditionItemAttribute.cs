using System;

namespace SIE.Tech.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConditionItemAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="path"></param>
        public ConditionItemAttribute(string displayName, string path)
        {

        }
    }
}
