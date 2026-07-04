using SIE.Domain;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.Web.Data;
using System.Linq;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations.DataQuery
{
    /// <summary>
    /// 计量设备定检帮助类
    /// </summary>
    public class CalibrationDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取润滑记录编码
        /// </summary>
        /// <returns>润滑记录编码</returns>
        public string GetCalibrationNo()
        {
            return RT.Service.Resolve<CalibrationController>().GetCalibrationNo().FirstOrDefault();
        }
    }
}
