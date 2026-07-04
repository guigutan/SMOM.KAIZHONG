using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Resources.Skills
{
    /// <summary>
    /// 考试要求
    /// </summary>
    public enum ExamRequired
    {
        /// <summary>
        /// Required：优秀
        /// </summary>
        [Category("Common")]
        [Label("优秀")]
        Excellent,

        /// <summary>
        /// Required：及格
        /// </summary>
        [Category("Common")]
        [Label("及格")]
        Pass,

        /// <summary>
        /// Required:无
        /// </summary>
        [Category("Required")]
        [Label("无")]
        NoMatter,

        /// <summary>
        /// Result:不及格
        /// </summary>
        [Category("Result")]
        [Label("不及格")]
        Fail,
    }
}