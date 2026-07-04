using SIE.Items;
using SIE.Items.Units;
using SIE.Units;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Items.Common.DataQuery
{
    /// <summary>
    /// 获取单位控制器
    /// </summary>
    public class ItemUnitDataQueryer : DataQueryer
    {
        /// <summary>
        /// 通过物料获取物料对应的精度
        /// </summary>
        /// <returns></returns>
        public UnitsModel GetItemUnitPrecisions(double itemId)
        {
            return RT.Service.Resolve<ItemUnitController>().GetItemUnitPrecision(itemId);
        }

        /// <summary>
        /// 通过单位ID获取物料对应的精度
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public UnitsModel GetUnitPrecisions(double unitId)
        {
            return RT.Service.Resolve<ItemUnitController>().GetUnitPrecision(unitId);
        }
    }
}
