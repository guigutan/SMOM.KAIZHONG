using SIE.Andon.AndonAverageTime;
using SIE.Domain;
using SIE.Web.Data;

namespace SIE.Web.Andon.AndonAverageTime
{
    /// <summary>
    /// 
    /// </summary>
    public class AndonAverageTimeDataQueryer : DataQueryer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EntityList<AndonAverageTimeViewModel> GetReportData(AndonAverageTimeViewModelCriteria criteria)
        {
            return RT.Service.Resolve<AndonAverageTimeController>().GetAndonAverageTimes(criteria);
        }
    }
}