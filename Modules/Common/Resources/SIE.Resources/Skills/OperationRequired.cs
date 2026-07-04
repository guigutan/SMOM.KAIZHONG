using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Resources.Skills
{
    /// <summary>
    /// 实操要求
    /// </summary>
    public enum OperationRequired
    {
        /// <summary>
        /// Required：满意
        /// </summary>
        [Category("Common")]
        [Label("满意")]
        Satisfaction,

        /// <summary>
        /// Required：通过
        /// </summary>
        [Category("Common")]
        [Label("通过")]
        Pass,

        /// <summary>
        /// Required：无
        /// </summary>
        [Category("Required")]
        [Label("无")]
        NoMatter,

        /// <summary>
        /// Result：不通过
        /// </summary>
        [Category("Result")]
        [Label("不通过")]
        Fail,
    }
}