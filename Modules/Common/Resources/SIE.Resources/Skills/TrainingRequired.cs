using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Resources.Skills
{
    /// <summary>
    /// 培训要求
    /// </summary>
    public enum TrainingRequired
    {
        /// <summary>
        /// Required：完成
        /// </summary>
        [Category("Common")]
        [Label("完成")]
        Finish,

        /// <summary>
        /// Required：无
        /// </summary>
        [Category("Required")]
        [Label("无")]
        NoMatter,

        /// <summary>
        /// Result:未完成
        /// </summary>
        [Category("Result")]
        [Label("未完成")]
        UnFinish
    }
}