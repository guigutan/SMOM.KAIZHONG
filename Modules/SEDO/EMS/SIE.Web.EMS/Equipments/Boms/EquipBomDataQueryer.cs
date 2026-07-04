using SIE.Domain;
using SIE.EMS.Equipments.Boms;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.Equipments.Boms
{
    /// <summary>
    /// 设备bom请求数据
    /// </summary>
    public class EquipBomDataQueryer : DataQueryer
    {
        /// <summary>
        /// 查询选择以外的包含明细的设备bom
        /// </summary>
        /// <param name="ecpIds"></param>
        /// <returns></returns>
        public EntityList<EquipBomSelect> GetEquipBomsExceptIds(List<double> ecpIds)
        {
            return RT.Service.Resolve<EquipBomController>().GetEquipBomsExceptIds(ecpIds);
        }
    }
}
