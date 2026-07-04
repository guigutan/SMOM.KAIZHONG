using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Common.Utils;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ScheProdVariance
{
    /// <summary>
    /// 
    /// </summary>
    public class ScheduleProductionVarianceController : DomainController
    {
        /// <summary>
        /// 获取排程生产数量数据
        /// </summary>
        public virtual EntityList<ScheduleProductionVariance> GetScheProdVarianceDataAll(ScheduleProductionVarianceCriteria criteria)
        {
            // 创建全新的查询，不继承任何默认条件
            var q = DB.Query<ScheduleProductionVariance>("V_SCHE_PROD_VARIANCE");

            // 1. 排程开始日期条件
            if (criteria.BeginDate.BeginValue.HasValue)
                q.Where(p => p.BeginDate >= criteria.BeginDate.BeginValue.Value);
            if (criteria.BeginDate.EndValue.HasValue)
                q.Where(p => p.BeginDate <= criteria.BeginDate.EndValue.Value);

            // 2. 排程结束日期条件
            if (criteria.BeginDate.BeginValue.HasValue)
                q.Where(p => p.EndDate >= criteria.BeginDate.BeginValue.Value);
            if (criteria.BeginDate.EndValue.HasValue)
                q.Where(p => p.EndDate <= criteria.BeginDate.EndValue.Value);
            // 3. 计划开始时间条件
            if (criteria.BeginDate.BeginValue.HasValue)
            {
                var targetDate = criteria.BeginDate.BeginValue.Value.Date;

                var whiteShiftStart = targetDate.AddHours(8);
                var whiteShiftEnd = targetDate.AddHours(19).AddMinutes(59).AddSeconds(59);
                var nightShiftStart = targetDate.AddHours(20);
                var nightShiftEnd = targetDate.AddHours(23).AddMinutes(59).AddSeconds(59);

                q.Where(p => (p.ShiftType == "白班" && p.PlanBeginTime >= whiteShiftStart && p.PlanBeginTime <= whiteShiftEnd)
                || (p.ShiftType == "晚班" && p.PlanBeginTime >= nightShiftStart && p.PlanBeginTime <= nightShiftEnd)
                );
            }

            // 执行查询
            var list = q.OrderBy(criteria.OrderInfoList)
                        .ToList(criteria.PagingInfo, new EagerLoadOptions());

            return list;
        }
    }
}
