using SIE.EMS.Lubrications;
using SIE.EMS.SpareParts;
using SIE.Web.Data;
using System.Linq;

namespace SIE.Web.EMS.Lubrications.DataQuery
{
    /// <summary>
    /// 润滑项目查询器
    /// </summary>
    public class LubricationDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取润滑记录编码
        /// </summary>
        /// <returns>润滑记录编码</returns>
        public string GetLubricationNo()
        {
            return RT.Service.Resolve<LubricationController>().GetLubricationNo().FirstOrDefault();
        }

        /// <summary>
        /// 获取备件库存
        /// </summary>
        /// <param name="sparePareId">备件ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        public int GetSparePartStoreQty(double sparePareId, double warehouseId)
        {
            return RT.Service.Resolve<SparePartController>().GetSparePartStoreQty(sparePareId, warehouseId);
        }

        /// <summary>
        /// 初始化润滑计划执行界面
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public string InitExeLubricationPlan(double accountId, double? departmentId)
        {
            //获取上次润滑小结
            var lastCheckSummary = RT.Service.Resolve<LubricationController>().GetLastLubricationSummaryInfo(accountId, departmentId);
            return lastCheckSummary;
        }
    }
}
