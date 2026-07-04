using Microsoft.Scripting.Utils;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Daos;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProcessPrepareRecords
{
    public class ProcessPrepareRecordsController : DomainController
    {

        private EntityList<ProcessPrepareRecordDetail> GetHadRecord(ProcessPrepareRecord prepareRecord)
        {
            var detail = GetHadRecord(prepareRecord);
            EntityList<ProcessPrepareRecordDetail> recordDetail = new EntityList<ProcessPrepareRecordDetail>();
            var groupDetail = detail.GroupBy(p => new { p.ProcessId, p.PrepareProjectId }).ToList();
            foreach (var item in groupDetail)
            {
                var maxCounter = item.OrderByDescending(p => p.Counter).FirstOrDefault();
                recordDetail.Add(maxCounter);
            }
            return recordDetail;
        }

        /// <summary>
        /// 根据主表id获取明细子表
        /// </summary>
        /// <param name="preRecordId"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessPrepareRecordDetail> GetPrepareRecordDetailList(double preRecordId)
        {
            return DB.Query<ProcessPrepareRecordDetail>().Where(p => p.PrepareRecordId == preRecordId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

    }
}
