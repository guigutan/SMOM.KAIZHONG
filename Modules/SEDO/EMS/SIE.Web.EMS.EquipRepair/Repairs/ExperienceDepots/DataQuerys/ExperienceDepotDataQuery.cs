using SIE.EMS.EquipRepair.ExperienceDepots.Controllers;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.Web.Data;

namespace SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.DataQuerys
{
    /// <summary>
    /// dataquery
    /// </summary>
    public class ExperienceDepotDataQuery : DataQueryer
    {
        /// <summary>
        /// 自动获取编码
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {
            var code = RT.Service.Resolve<ExperienceDepotController>().GetCode();
            return code;
        }

        /// <summary>
        /// 得到设备型号和领用部门
        /// </summary>
        /// <param name="accountId">设备台账Id</param>
        /// <returns></returns>
        public object GetEquipModelType(double accountId)
        {
            var account = RT.Service.Resolve<SparePartAppController>().GetEquipModelByAccountId(accountId);
            var result = new { EquipModel = account?.EquipModel, EquipType = account?.EquipModel?.EquipType };

            return result;
        }
    }
}
