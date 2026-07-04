using SIE.ObjectModel;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 委外需求明细的状态
    /// </summary>
    public enum OutsourcingDetailState
    {
        /// <summary>
        /// 待提交
        /// </summary>
        [Label("待提交")]
        Created = 10,


        /// <summary>
        /// 已提交
        /// </summary>
        [Label("已提交")]
        Submitted = 20,
    }
}
