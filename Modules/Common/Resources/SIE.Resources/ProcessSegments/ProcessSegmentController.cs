using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.ProcessSegments
{
    /// <summary>
    /// 工段控制器
    /// </summary>
    public partial class ProcessSegmentController : DomainController
    {
        /// <summary>
        /// 根据名称获取工段列表
        /// </summary>
        /// <param name="names">工段名称</param>
        /// <returns>工段列表</returns>
        public virtual EntityList<ProcessSegment> GetProcessSegmentsFromName(List<string> names)
        {
            return Query<ProcessSegment>().Where(m => names.Contains(m.Name)).ToList();
        }

        /// <summary>
        /// 根据编码获取工段
        /// </summary>
        /// <param name="segmentCode">工段编码</param>
        /// <returns>工段</returns>
        public virtual ProcessSegment GetProcessSegmentByCode(string segmentCode)
        {
            return Query<ProcessSegment>().Where(m => m.Code == segmentCode).FirstOrDefault();
        }

        /// <summary>
        /// 根据编码获取工段集合
        /// </summary>
        /// <param name="segmentCodeList"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessSegment> GetProcessSegmentByCodes(List<string> segmentCodeList)
        {
            var itemList = segmentCodeList.SplitContains(tempCodes =>
            {
                return Query<ProcessSegment>()
                .Where(p => tempCodes.Contains(p.Code))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return itemList;
        }

        /// <summary>
        /// 获取工段列表
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessSegment> GetProcessSegments(PagingInfo paging,string keyword)
        {
            var q= Query<ProcessSegment>();
            if (keyword.IsNotEmpty()) {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return q.ToList(paging,new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取所有工段
        /// </summary>
        /// <returns>工段</returns>
        public virtual EntityList<ProcessSegment> GetProcessSegmentList()
        {
            return Query<ProcessSegment>().ToList();
        }

        /// <summary>
        /// 根据名称获取工段
        /// </summary>
        /// <param name="segmentName">工段名称</param>
        /// <returns>工段</returns>
        public virtual ProcessSegment GetProcessSegmentByName(string segmentName)
        {
            return Query<ProcessSegment>().Where(m => m.Name == segmentName).FirstOrDefault();
        }
    }
}
