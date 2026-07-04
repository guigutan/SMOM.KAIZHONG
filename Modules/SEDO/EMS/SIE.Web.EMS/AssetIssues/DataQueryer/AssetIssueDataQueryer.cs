using SIE.Domain;
using SIE.EMS.AssetIssues;
using SIE.Equipments.Configs;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.AssetIssues.DataQueryer
{
    /// <summary>
    /// 资产发放查询器
    /// </summary>
    public class AssetIssueDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取发放单号
        /// </summary>
        /// <returns>发放单号</returns>
        public string GetAssetIssueNo()
        {
            var code = RT.Service.Resolve<AssetIssueController>().GetNo();
            return code;
        }

        /// <summary>
        /// 获取审批流配置信息
        /// </summary>
        /// <returns>审批流配置信息</returns>
        public ApprovalConfigValue GetApprovalConfigValue()
        {
            var configValue = RT.Service.Resolve<AssetIssueController>().GetApprovalFlowConfigValue();
            return configValue;
        }

        /// <summary>
        /// 获取可发放的设备清单
        /// </summary>
        /// <param name="issueId">发放单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>设备清单集合</returns>
        public EntityList<AssetIssueEquipment> GetAssetIssueEquipmentsById(double issueId, double requisitionId)
        {
            return RT.Service.Resolve<AssetIssueController>().GetAssetIssueEquipmentsById(issueId, requisitionId);
        }

        /// <summary>
        /// 通过设备清单明细Id获取发放设备清单明细
        /// </summary>
        /// <param name="reqEquipId">领用设备清单明细Id</param>
        /// <returns>发放设备清单明细</returns>
        public virtual EntityList<AssetIssueEquipment> GetAssetIssueEquipmentsByReqEquipId(double reqEquipId)
        {
            return RT.Service.Resolve<AssetIssueController>().GetAssetIssueEquipmentsByReqEquipId(reqEquipId);
        }

        /// <summary>
        /// 通过设备Id获取发放设备清单明细
        /// </summary>
        /// <param name="equipId">设备Id</param>
        /// <returns>发放设备清单明细</returns>
        public virtual EntityList<AssetIssueEquipment> GetAssetIssueEquipmentsByEquipId(double equipId)
        {
            return RT.Service.Resolve<AssetIssueController>().GetAssetIssueEquipmentsByEquipId(equipId);
        }

        /// <summary>
        /// 获取可发放的工治具清单
        /// </summary>
        /// <param name="issueId">发放单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>工治具清单集合</returns>
        public virtual EntityList<AssetIssueFixture> GetAssetIssueFixturesById(double issueId, double requisitionId)
        {
            return RT.Service.Resolve<AssetIssueController>().GetAssetIssueFixturesById(issueId, requisitionId);
        }

        /// <summary>
        /// 根据仓库和工治具编码获取在库数
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="id">工治具编码Id</param>
        /// <param name="locationId">工治具库位Id</param>
        /// <param name="QualityStatus">质量状态</param>
        /// <returns>工治具可用库存数</returns>
        public int GetCanUseNumByWarehouseId(double warehouseId, double id, double? locationId, FixtureQualityState? QualityStatus)
        {
            int canUseNum = 0;
            var locationIds = new List<double>();

            if (locationId != null && locationId != 0) 
            {
                locationIds.Add((double)locationId);
            }
            var stocklist = RT.Service.Resolve<CoreFixtureController>().GetCanUseNumByWarehouseId(warehouseId, new List<double>() { id }, locationIds);

            if (stocklist.Any()) 
            {
                if (QualityStatus == FixtureQualityState.Pass)
                {
                    canUseNum = stocklist.Sum(p => p.PassQty);
                }
                else if (QualityStatus == FixtureQualityState.Ng) 
                {
                    canUseNum = stocklist.Sum(p => p.NgQty);
                }
                else 
                {
                    canUseNum = stocklist.Sum(p => p.TotalQty);
                }
            }
            
            return canUseNum;
        }
    }
}
