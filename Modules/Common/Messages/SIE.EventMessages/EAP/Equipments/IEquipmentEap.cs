using SIE.Core.ApiModels;
using SIE.Services;

namespace SIE.EventMessages.EAP.Equipments
{
    /// <summary>
    /// 设备台账获取EAP实时值接口
    /// </summary>
    [Service(FallbackType = typeof(IDefaultEquipmentEap))]
    public interface IEquipmentEap
    {
        /// <summary>
        /// 获取EAP实时值
        /// </summary>
        /// <returns></returns>
        EquipEapRTValueInfo GetEquipEapRTValueInfo(EquipEapRTValuePara para);
    }

    /// <summary>
    /// 设备台账获取EAP实时值接口默认实现
    /// </summary>
    public class IDefaultEquipmentEap : IEquipmentEap
    {
        /// <summary>
        /// 获取EAP实时值
        /// </summary>
        /// <returns></returns>
        public EquipEapRTValueInfo GetEquipEapRTValueInfo(EquipEapRTValuePara para)
        {
            return new EquipEapRTValueInfo();
        }
    }
}
