using SIE.Common.Algorithm;
using SIE.Core.Items;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.Domain;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ScheProdVariance
{
    /// <summary>
    /// 获取排程生产数量数据
    /// </summary>
    [QueryEntity, Serializable]
    [Label("获取排程生产数量数据")]
    public class ScheduleProductionVarianceCriteria : Criteria
    {
        #region 排程时间 BeginDate
        /// <summary>
        /// 排程时间
        /// </summary>
        [Label("排程时间")]
        public static readonly Property<DateRange> BeginDateProperty = P<ScheduleProductionVarianceCriteria>.Register(e => e.BeginDate);

        /// <summary>
        /// 排程时间
        /// </summary>
        public DateRange BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ScheduleProductionVarianceController>().GetScheProdVarianceDataAll(this);
        }
    }
}