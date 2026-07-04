using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Resources.WipResources.Models;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 产线控制器 API 
    /// </summary> 
    [ApiName("WipResourceController")]
    public partial class WipResourceApiController : DomainController
    {
        /// <summary>
        /// 获取员工关联的生产资源
        /// </summary>      
        /// <returns>生产资源信息列表</returns>
        [ApiService("获取员工关联的生产资源")]
        [return: ApiReturn("生产资源列表信息。返回值类型：List<WipResourceInfo>")]
        public virtual List<WipResourceInfo> GetWipResourceInfos()
        {
            List<WipResourceInfo> infos = new List<WipResourceInfo>();
            var resources = RT.Service.Resolve<WipResourceController>().GetWipResources(RT.IdentityId);
            resources.OrderBy(p => p.Name).ForEach(p => infos.Add(new WipResourceInfo { Id = p.Id, Code = p.Code, Name = p.Name }));
            return infos;
        }

        /// <summary>
        /// 获取产线列表
        /// </summary>
        /// <param name="keyword">产线编码/名称</param>
        /// <param name="pageNumber">页数，为空查第一页</param>
        /// <param name="pageSize">页数据数量，为空查所有</param>
        /// <returns>产线列表</returns>
        [ApiService("获取产线列表")]
        [return: ApiReturn("产线信息列表 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingWipResourceInfos([ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber, [ApiParameter("页数据数量，为空查所有")] int? pageSize)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var stateList = new List<ResourceState>() { ResourceState.Actived };
            var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
            var WipResource = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, srcTypeList, pagingInfo, keyword);
            var infos = new List<BaseDataInfo>();
            WipResource.ForEach(e =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = WipResource.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 根据车间获取产线列表
        /// </summary>
        /// <param name="queryInfo">产线查询信息</param>
        /// <returns>分页产线信息</returns>
        [ApiService("根据车间获取产线列表")]
        [return: ApiReturn("分页产线信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingWipResourceInfoList([ApiParameter("产线查询信息")] ResourceQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
            var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
            var wipResources = RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, queryInfo.WorkShopId, srcTypeList, pagingInfo, queryInfo.Keyword);
            var infos = new List<BaseDataInfo>();
            wipResources.ForEach(e =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = wipResources.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }


        /// <summary>
        /// 根据车间获取产线列表
        /// </summary>
        /// <param name="queryInfo">产线查询信息</param>
        /// <returns>分页产线信息</returns>
        [ApiService("根据多车间获取产线列表")]
        [return: ApiReturn("分页产线信息 PagingBaseDataInfo")]
        public virtual PagingBaseDataInfo GetPagingWipResourceInfoListByWorkShopId([ApiParameter("产线查询信息")] WipResourceQueryInfo queryInfo)
        {
            var pageNumber = queryInfo.PageNumber;
            var pageSize = queryInfo.PageSize;
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
            var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
            var wipResources = RT.Service.Resolve<WipResourceController>().GetWipResourcesByWorkShopId(stateList, queryInfo.WorkShopIdList, srcTypeList, pagingInfo, queryInfo.Keyword);
            var infos = new List<BaseDataInfo>();
            wipResources.ForEach(e =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                });
            });
            PagingBaseDataInfo result = new PagingBaseDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = wipResources.TotalCount
            };
            result.DataInfos.AddRange(infos);
            return result;
        }
    }
}