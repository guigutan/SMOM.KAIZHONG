using SIE.EMS.EquipLends;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipLends.DataQueryers
{
    /// <summary>
    /// 设备借还数据请求
    /// </summary>
    public class EquipLendDataQueryer : DataQueryer
    {
        /// <summary>
        /// 是否需要审核
        /// </summary>
        /// <returns></returns>
        public bool IsNeedExamine()
        {
            return RT.Service.Resolve<EquipLendController>().IsNeedExamine();
        }

        /// <summary>
        /// 是否需要归还审核
        /// </summary>
        /// <returns></returns>
        public bool IsNeedReutrnExamine()
        {
            return RT.Service.Resolve<EquipLendController>().IsNeedReutrnExamine();

        }
    }
}
