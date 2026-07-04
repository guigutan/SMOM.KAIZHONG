using SIE.ObjectModel;

namespace SIE.Equipments.Abnormal
{
    /// <summary>
	/// 安灯呼叫类型
	/// </summary>
	public enum AlertCallType
    {
        /// <summary>
        /// 停线呼叫
        /// </summary>
        [Label("停线呼叫")]
        Stop = 0,

        /// <summary>
        /// 异常呼叫
        /// </summary>
        [Label("异常呼叫")]
        Exception = 1,

        /// <summary>
        /// 求助呼叫
        /// </summary>
        [Label("求助呼叫")]
        Help = 2,

        /*/// <summary>
        /// 测试呼叫30
        /// </summary>
        [Label("测试呼叫30")]
        CallTypeTest30 = 30,

        /// <summary>
        /// 测试呼叫40
        /// </summary>
        [Label("测试呼叫40")]
        CallTypeTest40 = 40,*/
    }
}
