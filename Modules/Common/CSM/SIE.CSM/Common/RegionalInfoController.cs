using SIE.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SIE.CSM.Common
{
    /// <summary>
    /// 区域信息控制器
    /// </summary>
    public class RegionalInfoController : DomainController
    {
        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <param name="upperLevelRegion">上一级区域信息</param>
        /// <param name="upperLevel2Region">上两级区域信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>区域信息</returns>
        public virtual EntityList<RegionalInfo> GetRegionalInfos(string upperLevelRegion, string upperLevel2Region, PagingInfo pagingInfo, string keyword = "")
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            //区域名称有重复，需使用2级确定查询
            var result = new EntityList<RegionalInfo>();
            var query = DB.Query<RegionalInfo>("p");
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Region.Contains(keyword));
            }
            if (string.IsNullOrEmpty(upperLevelRegion))
            {
                if (string.IsNullOrEmpty(upperLevel2Region))
                {
                    result = query.Where(p => p.TreePId.NVL(-1) == -1).ToList(null, elo);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(upperLevelRegion))
                {
                    query.Join<RegionalInfo>("h", (p, h) => p.TreePId == h.Id && h.Region == upperLevelRegion);
                }
                if (!string.IsNullOrEmpty(upperLevel2Region))
                {
                    query.Join<RegionalInfo, RegionalInfo>("h2", (h, h2) => h.TreePId == h2.Id && h2.Region == upperLevel2Region);
                }
                result = query.ToList(pagingInfo, elo);
            }

            return result;
        }

        /// <summary>
        /// 根据上一级区域信息获取下一级区域信息
        /// </summary>     
        /// <param name="upperLevelRegion">上一级区域信息</param>
        /// <param name="upperLevel2Region">上两级区域信息</param>
        /// <returns>区域信息</returns>
        public virtual List<string> GetRegionList(string upperLevelRegion, string upperLevel2Region)
        {
            var list = GetRegionalInfos(upperLevelRegion, upperLevel2Region, null);
            return list.Select(p => p.Region).ToList();
        }

        /// <summary>
        /// 根据上一级区域信息获取下一级区域信息
        /// </summary>     
        /// <param name="upperLevelRegion">上一级区域信息</param>
        /// <param name="upperLevel2Region">上两级区域信息</param>
        /// <returns>区域信息</returns>
        public virtual Dictionary<string, string> GetRegionDic(string upperLevelRegion, string upperLevel2Region)
        {
            var list = GetRegionalInfos(upperLevelRegion, upperLevel2Region, null);
            var result = list.Select(p => new { p.Id, p.Region }).ToDictionary(p => p.Region, p => p.Region);
            return result;
        }

    }
}
