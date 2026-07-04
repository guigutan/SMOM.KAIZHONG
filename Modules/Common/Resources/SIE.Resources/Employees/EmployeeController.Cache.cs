using System.Collections.Generic;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工控制器（支持缓存）
    /// </summary>
    public partial class EmployeeController
    {
        /// <summary>
        /// 员工名称字典缓存
        /// </summary>
        private static Dictionary<double, string> EmployeeNameDic = new Dictionary<double, string>();

        /// <summary>
        /// 获取员工名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetEmployeeNameById(double id)
        {
            if (EmployeeNameDic.ContainsKey(id))
                return EmployeeNameDic[id];
            else
            {
                var emp = GetEmployeeById(id);
                EmployeeNameDic.Add(id, emp?.Name);
                return emp?.Name;
            }
        }

        /// <summary>
        /// 清空缓存字典
        /// </summary>
        public virtual void ClearEmployeeNameDic()
        {
            EmployeeNameDic.Clear();
        }

        /// <summary>
        /// 检查缓存中是否包含员工ID，并清空缓存
        /// </summary>
        /// <param name="id"></param>
        public virtual void CheckAndClearEmployeeNameDic(double id)
        {
            if (EmployeeNameDic.ContainsKey(id))
                EmployeeNameDic.Clear();
        }
    }
}
