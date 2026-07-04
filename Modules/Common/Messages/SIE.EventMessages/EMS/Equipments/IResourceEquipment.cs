using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.Equipments
{
    /// <summary>
    /// 设备Id列表接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultResourceEquipment))]
    public interface IResourceEquipment
    {
        /// <summary>
        /// 获取设备Id列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <returns>设备Id列表</returns>
        List<double> GetWipResourceEquipmentIds(double resourceId);
    }

    /// <summary>
    /// 设备接口默认实现
    /// </summary>
    public class DefaultResourceEquipment : IResourceEquipment
    {
        /// <summary>
        /// 获取设备Id列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <returns>设备Id列表</returns>
        public List<double> GetWipResourceEquipmentIds(double resourceId)
        {
            return new List<double>();
        }
    }
}
