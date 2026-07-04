using SIE.ObjectModel;

namespace SIE.Core.Enums
{
    /// <summary>
	/// 管理状态（原使用状态）
	/// </summary>
	public enum AccountUseState
    {
        /// <summary>
        /// 闲置
        /// </summary>
        [Label("闲置")]
        InIdle = 0,

        /// <summary>
        /// 使用中
        /// </summary>
        [Label("使用中")]
        Using = 5,

        /// <summary>
        /// 封存
        /// </summary>
        [Label("封存")]
        Archive = 10,

        /// <summary>
        /// 租借
        /// </summary>
        [Label("租借")]
        Lease = 15,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap = 20,

        /// <summary>
        /// 待验收
        /// </summary>
        [Label("待验收")]
        ToAccepted = 25,



        ///// <summary>
        ///// 待提取
        ///// </summary>
        //[Label("待提取")]
        //ToExtract = 25,

        ///// <summary>
        ///// 待归还
        ///// </summary>
        //[Label("待归还")]
        //StayBack = 30,

        /// <summary>
        /// 维修
        /// </summary>
        [Label("维修")]
        Repair = 40,

        /// <summary>
        /// 委外维修
        /// </summary>
        [Label("委外维修")]
        OutsourcedRepair = 45,

        /// <summary>
		/// 不合格停用
		/// </summary>
		[Label("不合格停用")]
        Failed = 50,

        /// <summary>
        /// 超期停用
        /// </summary>
        [Label("超期停用")]
        Overdue = 60,

        /// <summary>
        /// 已处置
        /// </summary>
        [Label("已处置")]
        DisposedOf = 70,
    }
}
