using SIE.Domain;
using SIE.EMS.AssetScraps;
using SIE.Equipments.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetScraps.DataQueryer
{
    /// <summary>
    /// 资产报废查询器
    /// </summary>
    public class AssetScrapDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取报废单号
        /// </summary>
        /// <returns>报废单号</returns>
        public string GetAssetScrapNo()
        {
            var code = RT.Service.Resolve<AssetScrapController>().GetNo();
            return code;
        }

        /// <summary>
        /// 获取审批流配置信息
        /// </summary>
        /// <returns>审批流配置信息</returns>
        public ApprovalConfigValue GetApprovalConfigValue()
        {
            var configValue = RT.Service.Resolve<AssetScrapController>().GetApprovalFlowConfigValue();
            return configValue;
        }

        /// <summary>
        /// 根据报废单Id获取设备清单
        /// </summary>
        /// <param name="equipAccountId">设备台账Id</param>
        /// <returns>设备清单集合</returns>
        public EntityList<AssetScrapEquipment> GetAssetScrapEquipmentsById(double equipAccountId)
        {
            return RT.Service.Resolve<AssetScrapController>().GetWorkHourAndCostInfoById(equipAccountId);
        }
    }
}
