using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Web.Data;
using System;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipModels.DataQuery
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
            var list = RT.Service.Resolve<EquipModelController>().GetEquipModels(new List<double>() { equipModelId });
            return list.Count >= 1;
        }

        /// <summary>
        /// 获取设备类型快码
        /// </summary>
        /// <returns></returns>
        public EntityList<Catalog> GetCalalogForEquipModel()
        {
            return RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType);
        }

        /// <summary>
        /// 获取设备型号是否特殊设备、是否计量设备
        /// </summary>
        /// <param name="code"></param>
        /// <returns>isMeasureEquipment/isSpecial</returns>

        public Tuple<bool,bool> GetEquipmentTypeInfo(string code)
        {
            return RT.Service.Resolve<EquipModelController>().GetEquipmentTypeInfo(code);
            
        }
    }
}
