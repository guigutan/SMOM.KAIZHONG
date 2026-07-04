using SIE.EventMessages.Common.SnModels;
using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 更新IQC报检接口信息
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIqcRecheckInterface))]
    public interface IIqcRecheck
    {
        /// <summary>
        /// 更新IQC复检单数据
        /// </summary>
        /// <returns>更新成功true/失败false</returns>
        bool UpdateIqcRecheck(List<IqcRecheckData> iqcRecheckDatas);

        /// <summary>
        /// 验证Sn是否属于单据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        ValidateResultInfo ValidateSnInBill(ValidateRequestInfo info);

    }

    class DefalitIqcRecheckInterface : IIqcRecheck
    {
        public bool UpdateIqcRecheck(List<IqcRecheckData> iqcRecheckDatas)
        {
            return true;
        }

        /// <summary>
        /// 验证Sn是否属于单据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ValidateResultInfo ValidateSnInBill(ValidateRequestInfo info)
        {
            return null;
        }
    }
}
