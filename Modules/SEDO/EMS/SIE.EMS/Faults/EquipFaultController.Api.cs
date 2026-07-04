using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Faults
{
    /// <summary>
    /// 设备故障 API
    /// </summary>
    public partial class EquipFaultController : DomainController
    {
        /// <summary>
        /// 获取故障分类
        /// </summary>
        /// <param name="type">0-大类，1-中类，2-小类</param>
        /// <param name="parentId">上级ID</param>
        /// <param name="pageNumber">页数，为空查第一页</param>
        /// <param name="pageSize">页数据数量，为空查所有</param>
        /// <returns>故障分类列表</returns>
        [ApiService("获取故障分类")]
        [return: ApiReturn("故障分类列表 List<BaseDataInfo>")]
        public virtual List<BaseDataInfo> GetEquipFault([ApiParameter("0-大类，1-中类，2-小类")] int type, [ApiParameter("上级ID")] double? parentId, [ApiParameter("页数，为空查第一页")] int? pageNumber, [ApiParameter("页数据数量，为空查所有")] int? pageSize)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1
            };
            var result = new List<BaseDataInfo>();
            switch (type)
            {
                case 0:
                    result.AddRange(GetEquipLargeFaultInfos(pagingInfo));
                    break;
                case 1:
                    result.AddRange(GetEquipMiddleFaultInfos(parentId, pagingInfo));
                    break;
                case 2:
                    result.AddRange(GetEquipSmallFaultInfos(parentId, pagingInfo));
                    break;
                default:
                    throw new ValidationException("故障分类类型错误".L10N());
            }
            return result;
        }

        /// <summary>
        /// 获取设备故障大类信息列表
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备故障列表</returns>
        IEnumerable<BaseDataInfo> GetEquipLargeFaultInfos(PagingInfo pagingInfo)
        {
            return GetEquipLargeFaults(null, pagingInfo).Select(p => new BaseDataInfo()
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name
            });
        }

        /// <summary>
        /// 获取设备故障大类信息列表
        /// </summary>
        /// <param name="parentId">上级ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备故障列表</returns>
        IEnumerable<BaseDataInfo> GetEquipMiddleFaultInfos(double? parentId, PagingInfo pagingInfo)
        {
            if (!parentId.HasValue)
                throw new ValidationException("故障大类不能为空".L10N());
            return GetEquipMiddleFaults(p => p.LargeFaultId == parentId, pagingInfo).Select(p => new BaseDataInfo()
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name
            });
        }

        /// <summary>
        /// 获取设备故障大类信息列表
        /// </summary>
        /// <param name="parentId">上级ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备故障列表</returns>
        IEnumerable<BaseDataInfo> GetEquipSmallFaultInfos(double? parentId, PagingInfo pagingInfo)
        {
            if (!parentId.HasValue)
                throw new ValidationException("故障中类不能为空".L10N());
            return GetEquipSmallFaults(p => p.MiddleFaultId == parentId, pagingInfo).Select(p => new BaseDataInfo()
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name
            });
        }
    }
}