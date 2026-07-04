using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.RunningStates;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SIE.Web.EMS.Equipments.RunngingStates.DataQuery
{
    /// <summary>
    /// 设备运行状态记录维护查询器
    /// </summary>
    public class EquipRunningStateRecordDataQueryer : DataQueryer
    {
        /// <summary>
        /// 同步设备运行状态记录
        /// </summary>
        /// <returns></returns>
        public int SyncEquipRunningStateRecord()
        {
            var count = RT.Service.Resolve<EquipController>().SyncEquipRunningStateRecord();
            return count;
        }
    }
}
