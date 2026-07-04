using SIE.ObjectModel;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
	/// 工治具状态
	/// </summary>
	public enum FixtureAccountState
    {
        /// <summary>
        /// 待保养
        /// </summary>
        [Label("待保养")]
        WaitMaintain = 5,

        //      /// <summary>
        ///// 保养完成
        ///// </summary>
        //[Label("保养完成")]
        //      FinishMaintain = 10,

        /// <summary>
        /// 在库
        /// </summary>
        [Label("在库")]
        InStorage = 15,

        /// <summary>
        /// 待领用
        /// </summary>
        [Label("待领用")]
        WaitReceive = 20,

        /// <summary>
        /// 在线
        /// </summary>
        [Label("在线")]
        Online = 25,

        /// <summary>
        /// 待维修
        /// </summary>
        [Label("待维修")]
        WaitRepair = 30,

        /// <summary>
		/// 维修完成
		/// </summary>
		[Label("维修完成")]
        FinishRepair = 35,

        /// <summary>
        /// 待入库
        /// </summary>
        [Label("待入库")]
        WaitShelf = 40,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap = 45,

        /// <summary>
		/// 使用中
		/// </summary>
		[Label("使用中")]
        Using = 50,

        /// <summary>
		/// 待归还
		/// </summary>
		[Label("待归还")]
        ToBeReturn = 55,

        /// <summary>
        /// 委外维修
        /// </summary>
        [Label("委外维修")]
        OutedSide = 60,

        /// <summary>
        /// 借出
        /// </summary>
        [Label("借出")]
        Lending = 65,

        /// <summary>
        /// 部门领用
        /// </summary>
        [Label("部门领用")]
        Requisition = 70,

        /// <summary>
        /// 处置
        /// </summary>
        [Label("处置")]
        Disposal = 75,
    }
}
