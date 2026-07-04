using SIE.Domain;
using SIE.EMS.AssetReturns;
using SIE.Equipments.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetReturns.DataQueryer
{
    /// <summary>
    /// 资产归还查询器
    /// </summary>
    public class AssetReturnDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取归还单号
        /// </summary>
        /// <returns>归还单号</returns>
        public string GetAssetReturnNo()
        {
            var code = RT.Service.Resolve<AssetReturnController>().GetNo();
            return code;
        }

        /// <summary>
        /// 获取审批流配置信息
        /// </summary>
        /// <returns>审批流配置信息</returns>
        public ApprovalConfigValue GetApprovalConfigValue()
        {
            var configValue = RT.Service.Resolve<AssetReturnController>().GetApprovalFlowConfigValue();
            return configValue;
        }

        /// <summary>
        /// 获取可归还的设备清单
        /// </summary>
        /// <param name="returnId">归还单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>设备清单集合</returns>
        public EntityList<AssetReturnEquipment> GetAssetReturnEquipmentsById(double returnId, double requisitionId)
        {
            return RT.Service.Resolve<AssetReturnController>().GetAssetReturnEquipmentsById(returnId, requisitionId);
        }

        /// <summary>
        /// 获取可归还的工治具清单
        /// </summary>
        /// <param name="returnId">归还单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>工治具清单集合</returns>
        public EntityList<AssetReturnFixture> GetAssetReturnFixturesById(double returnId, double requisitionId)
        {
            return RT.Service.Resolve<AssetReturnController>().GetAssetReturnFixturesById(returnId, requisitionId);
        }

        /// <summary>
        /// 获取已归还的设备清单
        /// </summary>
        /// <param name="returnId">归还单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>已归还的设备清单集合</returns>
        public EntityList<AssetReturnEquipment> GetExistAssetReturnEquipmentsById(double returnId, double requisitionId)
        {
            return RT.Service.Resolve<AssetReturnController>().GetExistAssetReturnEquipmentsById(new List<OrderInfo>(), null, returnId, requisitionId);
        }

        /// <summary>
        /// 获取已归还的工治具清单
        /// </summary>
        /// <param name="returnId">归还单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>已归还的工治具清单集合</returns>
        public EntityList<AssetReturnFixture> GetExistAssetReturnFixturesById(double returnId, double requisitionId)
        {
            return RT.Service.Resolve<AssetReturnController>().GetExistAssetReturnFixturesById(new List<OrderInfo>(), null, returnId, requisitionId);
        }
    }
}
