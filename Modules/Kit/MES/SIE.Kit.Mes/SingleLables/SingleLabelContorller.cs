using SIE.Domain;
using System;

namespace SIE.Kit.MES.SingleLabels
{
    /// <summary>
    /// 单体条码控制器
    /// </summary>
    public class SingleLabelContorller : DomainController
    {
        /// <summary>
        /// 查询单体条码
        /// </summary>
        /// <param name="sn">单体条码Sn</param>
        /// <returns>SingleLabel</returns>
        /// <exception cref="ArgumentNullException">条码号为空</exception>
        public virtual SingleLabel GetSingleLabel(string sn)
        {
            if (sn == null)
                throw new ArgumentNullException(nameof(sn));
            return Query<SingleLabel>().Where(p => p.Sn == sn).FirstOrDefault();
        }

        /// <summary>
        /// 更新单体标签状态
        /// </summary>
        /// <param name="sn">标签号</param>
        /// <param name="state">状态</param>
        public virtual void UpdateSingleLabelState(string sn, LabelState state)
        {
            DB.Update<SingleLabel>().Set(p => p.LabelState, state).Where(p => p.Sn == sn).Execute();
        }
    }
}