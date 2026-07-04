using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Equipments;
using SIE.EMS.Elec.Fixtrue.Equipments.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 设备API控制器
    /// </summary>
    public partial class ElecEquipController : CoreEquipController
    {
        /// <summary>
        /// 根据设备台账获取分区列表
        /// </summary>
        /// <param name="queryInfo">分区查询信息</param>
        /// <returns>分页分区信息</returns>
        [ApiService("根据设备台账获取分区列表")]
        [return: ApiReturn("分页分区信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetSubareaInfos([ApiParameter("分区查询信息")] SubareaQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var res = RT.Service.Resolve<ElecEquipController>().GetEquipLocationsByAccountId(queryInfo.EquipId, null);
            var subareas = res.Where(p => !p.Subarea.IsNullOrEmpty()).Select(p => p.Subarea).Distinct();
            var infos = new List<BaseDataInfo>();
            subareas.ForEach(subarea =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Code = subarea
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = res.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 根据设备、分区获取站位列表
        /// </summary>
        /// <param name="queryInfo">站位查询信息</param>
        /// <returns>分页分区信息</returns>
        [ApiService("根据设备、分区获取站位列表")]
        [return: ApiReturn("分页站位信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetStanceInfos([ApiParameter("分区查询信息")] StanceQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var res = RT.Service.Resolve<ElecEquipController>().GetEquipLocationsByAccountIdSubarea(queryInfo.EquipId, queryInfo.Subarea, pagingInfo);
            var stances = res.Where(p => !p.Stance.IsNullOrEmpty()).Select(p => p.Stance).Distinct();
            var infos = new List<BaseDataInfo>();
            stances.ForEach(stance =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Code = stance
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = res.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }
    }
}