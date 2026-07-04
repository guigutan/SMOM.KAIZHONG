using SIE.Common.Configs;
using SIE.Equipments.Configs;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Web.Data;

namespace SIE.Web.Equipments.EquipmentCards.DataQuery
{
    /// <summary>
    /// 设备立卡查询器
    /// </summary>
    public class EquipmentCardDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取设备台账编码
        /// </summary>
        /// <returns>设备台账编码</returns>
        public string GetEquipAccountNo()
        {
            return RT.Service.Resolve<EquipAccountController>().GetAccountNo();
        }

        /// <summary>
        /// 是否启用固定资产
        /// </summary>
        /// <returns>是否启用固定资产</returns>
        public bool GetIsEnableAsset()
        {
            return ConfigService.GetConfig(new EquipAccountAssetConfig(), typeof(EquipAccount)).Asset;
        }

        /// <summary>
        /// 根据Id设备立卡
        /// </summary>
        /// <returns>设备立卡</returns>
        public EquipmentCard GetEquipmentCardById(double id)
        {
            return RT.Service.Resolve<EquipmentCardController>().GetEquipmentCardById(id);
        }

    }
}
