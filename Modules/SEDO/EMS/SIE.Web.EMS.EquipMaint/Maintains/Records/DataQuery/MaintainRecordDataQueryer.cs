using SIE.Domain;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Projects;
using SIE.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipMaint.Maintains.Records.DataQuery
{
    /// <summary>
    /// 设备保养记录查询器
    /// </summary>
    public class MaintainRecordDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取保养项目图片
        /// </summary>
        /// <param name="projectId">保养项目id</param>
        /// <returns>返回参数</returns>
        public string GetMaintainProjectPhoto(double projectId)
        {
            var photo = RF.GetById<MaintainProject>(projectId)?.ProjectPhoto?.Photo;
            if (photo == null)
                return string.Empty;
            return System.Text.Encoding.Default.GetString(photo);
        }
    }
}
