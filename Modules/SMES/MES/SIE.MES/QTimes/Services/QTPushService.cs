using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.MES.QTimes.Daos;
using SIE.MES.QTimes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Services
{
    /// <summary>
    /// QT推送对象Service
    /// </summary>
    public class QTPushService : DomainService
    {
        /// <summary>
        /// Dao
        /// </summary>
        private readonly  QTPushDao _qTPushDao;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="qTPushDao"></param>
        public QTPushService(QTPushDao qTPushDao)
        {
            _qTPushDao = qTPushDao;
        }

        /// <summary>
        /// 获取QT推送对象子表
        /// </summary>
        /// <param name="pushIds">子表Ids</param>
        /// <param name="standardIds">主表Ids</param>
        /// <param name="types">类型</param>
        /// <returns></returns>
        public virtual EntityList<QTPushObject> GetPushList(List<double> pushIds, List<double> standardIds, List<QTPushType> types)
        {
            return _qTPushDao.GetPushList(pushIds, standardIds, types);
        }
    }
}
