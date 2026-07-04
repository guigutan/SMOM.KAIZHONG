using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    public class FeedingAreaController : DomainController
    {
        /// <summary>
        /// 查询供料区域信息
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual List<FeedingArea> GetFeedingAreaPrintDatas(List<double> Ids)
        {
            List<FeedingArea> feedingAreaDatas = new List<FeedingArea>();
            Ids.SplitDataExecute(Ids =>
            {
                var list = Query<FeedingArea>()
                    .Where(x => Ids.Contains(x.Id))
                     .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                feedingAreaDatas.AddRange(list);
            });
            return feedingAreaDatas;
        }
    }
}
