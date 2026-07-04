using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.MES.QTimes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Daos
{
    /// <summary>
    /// QT推送对象Dao
    /// </summary>
    public class QTPushDao : BaseDao<QTPushObject>
    {
        /// <summary>
        /// 获取QT推送对象子表
        /// </summary>
        /// <param name="pushIds">子表Ids</param>
        /// <param name="standardIds">主表Ids</param>
        /// <param name="types">类型</param>
        /// <returns></returns>
        public EntityList<QTPushObject> GetPushList(List<double> pushIds, List<double> standardIds, List<QTPushType> types)
        {
            return Query().Where(p => !pushIds.Contains(p.Id) && standardIds.Contains(p.QTStandardId) && types.Contains(p.ObjectType)).ToList();
        }
    }
}
