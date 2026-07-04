using SIE.Core.ApiModels;
using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.Equipments
{
    /// <summary>
    /// 设备查询接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultEquipmentQuery))]
    public interface IEquipmentQuery
    {
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <returns>设备列表</returns>
        List<BaseDataInfo> GetEquipAccounts(double resourceId);

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <param name="equipIdList">排除的设备Id集合</param>
        /// <param name="key">查询字段</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备列表</returns>
        PagingBaseDataInfo GetLoadItemEquipAccounts(double resourceId, List<double> equipIdList, string key, PagingInfo pagingInfo);

        /// <summary>
        /// 根据设备获取设备产线ID
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>产线ID</returns>
        double? GetEquipResource(double equipId);

        /// <summary>
        /// 根据设备ID虚拟设备ID
        /// </summary>
        /// <param name="equipIds">设备ID</param>
        /// <returns>设备ID</returns>
        List<double> GetEquipIdByIds(List<double?> equipIds);
    }

    /// <summary>
    /// 设备查询接口默认实现
    /// </summary>
    public class DefaultEquipmentQuery : IEquipmentQuery
    {
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <returns>设备列表</returns>
        public List<BaseDataInfo> GetEquipAccounts(double resourceId)
        {
            return new List<BaseDataInfo>();
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <param name="equipIdList">排除的设备Id集合</param>
        /// <param name="key">查询字段</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>设备列表</returns>
        public PagingBaseDataInfo GetLoadItemEquipAccounts(double resourceId, List<double> equipIdList, string key, PagingInfo pagingInfo)
        {
            return new PagingBaseDataInfo();
        }

        /// <summary>
        /// 根据设备获取设备产线ID
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>产线ID</returns>
        public double? GetEquipResource(double equipId)
        {
            return null;
        }

        /// <summary>
        /// 根据设备ID虚拟设备ID
        /// </summary>
        /// <param name="equipIds">设备ID</param>
        /// <returns>设备ID</returns>
        public virtual List<double> GetEquipIdByIds(List<double?> equipIds)
        {
            return new List<double>();
        }
    }
}