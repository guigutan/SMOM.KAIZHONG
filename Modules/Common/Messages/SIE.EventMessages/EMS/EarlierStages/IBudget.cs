using SIE.Services;

namespace SIE.EventMessages.EMS.EarlierStages
{
    /// <summary>
    /// 预算接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultBudget))]
    public interface IBudget
    {
        /// <summary>
        /// 更新预算的【已使用金额】【预占金额】
        /// </summary>
        /// <param name="proKeyItemId">项目关键事项id</param>
        /// <param name="amount">金额</param>
        void UpdateBudgetUsedAmount(double proKeyItemId, decimal amount);
    }

    /// <summary>
    /// 预算接口默认实现
    /// </summary>
    public class DefaultBudget : IBudget
    {
        /// <summary>
        /// 更新预算的【已使用金额】【预占金额】
        /// </summary>
        /// <param name="proKeyItemId">项目关键事项id</param>
        /// <param name="amount">金额</param>
        public void UpdateBudgetUsedAmount(double proKeyItemId, decimal amount)
        {
            //默认接口无逻辑
        }
    }
}
