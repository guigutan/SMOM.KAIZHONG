using Microsoft.Scripting.Utils;
using SIE.Common;
using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.QTimes.Handles;
using SIE.MES.QTimes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Daos
{
    /// <summary>
    /// 查询
    /// </summary>
    public class QTimeReportDao : BaseDao<QTimeReportViewModel>
    {
        /// <summary>
        /// QT超时报表查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EntityList<QTimeReportViewModel> QueryQTimeReports(QTimeReportViewModelCriteria criteria)
        {
            if (criteria.WipResourceId == null)
            {
                throw new ValidationException("请选择产线！".L10N());
            }
            if (!criteria.CollectTime.BeginValue.HasValue || !criteria.CollectTime.EndValue.HasValue)
            {
                throw new ValidationException("请选择采集时间范围！".L10N());
            }
            EntityList<QTimeReportViewModel> qTimeReports = new EntityList<QTimeReportViewModel>();
            var qtReportHandle = new QTimeReportHandle();
            qtReportHandle.Query(criteria);
            qtReportHandle.OrderByDescStartOperateTime();
            qTimeReports.AddRange(qtReportHandle.OutputData());
            qTimeReports = qTimeReports.Skip((criteria.PagingInfo.PageNumber - 1) * criteria.PagingInfo.PageSize).Take(criteria.PagingInfo.PageSize).AsEntityList();
            qTimeReports.SetTotalCount(qtReportHandle.OutputPageInfo());
            return qTimeReports;
        }
    }
}
