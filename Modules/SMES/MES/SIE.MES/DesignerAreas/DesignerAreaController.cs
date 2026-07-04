using SIE.Common.InvOrg;
using SIE.Common.Messages;
using SIE.Domain;
using SIE.MES.WIP;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.DesignerAreas
{

    /// <summary>
    /// 《看板区域》控制器，增、删、查、改
    /// </summary>
    public partial class DesignerAreaController : WipController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="prodAreaCriteria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<DesignerArea> Fetch(DesignerAreaCriteria prodAreaCriteria)
        {
            var q = Query<DesignerArea>();

            //看板区域编码
            if (!prodAreaCriteria.AreaCode.IsNullOrEmpty())
            {
                q.Where(m => m.AreaCode.Contains("%" + prodAreaCriteria.AreaCode + "%"));               
            }

            //看板区域名称
            if (!prodAreaCriteria.AreaName.IsNullOrEmpty())
            {
                q.Where(m => m.AreaName.Contains("%" + prodAreaCriteria.AreaName + "%"));
            }

            return q.OrderBy(prodAreaCriteria.OrderInfoList).ToList(prodAreaCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

        }







    }
}
