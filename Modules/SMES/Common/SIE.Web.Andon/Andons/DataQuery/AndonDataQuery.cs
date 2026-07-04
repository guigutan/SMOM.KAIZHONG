using SIE.Andon.Andons;
using SIE.Andon.Andons.APIModel;
using SIE.Domain;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.DataQuery
{
    /// <summary>
    /// 安灯数据操作
    /// </summary>
    public class AndonDataQuery : DataQueryer
    {
        /// <summary>
        /// 安灯类型拉去数据
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <returns></returns>
        public AndonTypeRequestInfo GetAndonTypeInfo(double andonTypeId)
        {
            return RT.Service.Resolve<AndonController>().GetAndonTypeInfo(andonTypeId);
        }
    }
}
