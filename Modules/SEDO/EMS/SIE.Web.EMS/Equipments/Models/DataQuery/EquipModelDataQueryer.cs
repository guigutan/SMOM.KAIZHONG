using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Models;
using SIE.EMS.Equipments.Models.ViewModels;
using SIE.Equipments.EquipModels;
using SIE.Web.Data;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Models.DataQuery
{
    /// <summary>
    /// 设备型号查询器
    /// </summary>
    public class EquipModelDataQueryer : DataQueryer
    {
        /// <summary>
        /// 通过设备型号获取设备台账子表信息
        /// </summary>
        /// <param name="equipModelId">设备型号ID</param>
        /// <returns>设备台账子表信息</returns>
        public bool isExistThisEquipModel(double equipModelId)
        {
            var list = RT.Service.Resolve<EquipModelController>()
                .GetEquipModels(new List<double>() { equipModelId });
            return list.Count >= 1;
        }

        /// <summary>
        /// 通过点检保养项目维护获取备件清单信息
        /// </summary>
        /// <param name="lubricationProject">点检保养项目维护id</param>
        /// <returns>备件清单信息</returns>
        public EquipModelLubricationProjectInfo GetSparePartItemInfos(EquipModelLubricationProject lubricationProject)
        {
            return RT.Service.Resolve<EquipController>().GetModelSparePartItemInfos(lubricationProject);
        }
    }
}
