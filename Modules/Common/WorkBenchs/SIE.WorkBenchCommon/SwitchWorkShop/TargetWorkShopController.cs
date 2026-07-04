using SIE.Domain;
using SIE.Resources.Enterprises;

namespace SIE.WorkBenchCommon.SwitchWorkShop
{
    /// <summary>
    /// 目标车间控制器
    /// </summary>
    public class TargetWorkShopController : DomainController
    {
        /// <summary>
        /// 保存目标车间
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="workShopId">车间ID</param>
        public virtual void SaveTargetWorkShop(double employeeId, double workShopId)
        {
            var res = DB.Update<TargetWorkShop>()
                .Set(p => p.WorkShopId, workShopId)
                .Where(p => p.EmployeeId == employeeId)
                .Execute();
            if (res == 0)
            {
                RF.Save(new TargetWorkShop()
                {
                    EmployeeId = employeeId,
                    WorkShopId = workShopId
                });
            }
        }

        /// <summary>
        /// 获取目标车间
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>车间ID，不存在返回null</returns>
        public virtual Enterprise GetTargetWorkShop(double employeeId)
        {
            var query= Query<TargetWorkShop>().Where(p => p.EmployeeId == employeeId).FirstOrDefault();
            if (query == null) return null;
            else return query.WorkShop;
        }
    }
}