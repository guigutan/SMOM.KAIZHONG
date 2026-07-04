using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.Equipments
{
    /// <summary>
    /// 设备位置查询接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultEquipmentLocationQuery))]
    public interface IEquipmentLocationQuery
    {
        /// <summary>
        /// 获取设备位置分区列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>分区列表</returns>
        IList<string> GetEquipSubarea(double equipId);

        /// <summary>
        /// 获取设备位置站位列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>站位列表</returns>
        IList<string> GetEquipStance(double equipId);

        /// <summary>
        /// 获取设备位置列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>位置列表</returns>
        IList<EquipLocationInfo> GetLocationInfos(double equipId);
    }

    /// <summary>
    /// 设备位置查询接口默认实现
    /// </summary>
    public class DefaultEquipmentLocationQuery : IEquipmentLocationQuery
    {
        /// <summary>
        /// 获取设备位置分区列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>分区列表</returns>
        public IList<string> GetEquipStance(double equipId)
        {
            return new List<string>();
        }

        /// <summary>
        /// 获取设备位置站位列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>站位列表</returns>
        public IList<string> GetEquipSubarea(double equipId)
        {
            return new List<string>();
        }

        /// <summary>
        /// 获取设备位置列表
        /// </summary>
        /// <param name="equipId">设备ID</param>
        /// <returns>位置列表</returns>
        public IList<EquipLocationInfo> GetLocationInfos(double equipId)
        {
            return new List<EquipLocationInfo>();
        }
    }

    /// <summary>
    /// 设备位置信息
    /// </summary>
    [Serializable]
    public class EquipLocationInfo
    {
        /// <summary>
        /// 分区
        /// </summary>
        public string Subarea { get; set; }

        /// <summary>
        /// 站位
        /// </summary>
        public string Stance { get; set; }
    }
}