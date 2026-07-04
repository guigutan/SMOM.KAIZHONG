using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.MES.QTimes.Daos;
using SIE.MES.QTimes.Handles;
using SIE.MES.QTimes.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.QTimes.Services
{
    /// <summary>
    /// QTime报表
    /// </summary>
    public class QTimeReportService : DomainService
    {
        private readonly QTimeReportDao _qTimeReportDao;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="qTimeReportDao"></param>
        public QTimeReportService(QTimeReportDao qTimeReportDao)
        {
            _qTimeReportDao = qTimeReportDao;
        }

        /// <summary>
        /// 报表查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<QTimeReportViewModel> QueryQTimeReports(QTimeReportViewModelCriteria criteria)
        {
            return _qTimeReportDao.QueryQTimeReports(criteria);
        }

        /// <summary>
        /// QT超时报表消息推送
        /// </summary>
        public virtual void QTimeMessageSend()
        {
            QTimeReportHandle qTimeReportHandle = new QTimeReportHandle();
            qTimeReportHandle.JobSend();
            // 获取已超时的数据
            List<QTimeReportViewModel> qTimeReportViewModels = qTimeReportHandle.OutputData().Where(p => p.IsOverTime).ToList();
            // 获取对应标准规则Id
            var standards = qTimeReportHandle.GetQTStandards();
            QTimeReportMsgSendHandle qTimeReportMsgSendHandle = new QTimeReportMsgSendHandle();
            // 获取对应的推送对象下的所有员工(员工、用户组、班组、角色、部门)
            qTimeReportMsgSendHandle.GetStandardPushObjectToEmployee(standards.Select(p => p.Id).ToList());
            // 获取推送方式
            qTimeReportMsgSendHandle.GetPushPlugs(standards.Select(p => p.PushPlugId).ToList());
            // 推送
            foreach (var standard in standards)
            {
                // 报表数据
                var reportRecords = qTimeReportViewModels.Where(p => p.QTId == standard.Id).ToList();
                if (reportRecords.Count > 0)
                {
                    // 邮件推送
                    qTimeReportMsgSendHandle.SendEmail(standard, reportRecords);
                }
            }
        }

    }
}
