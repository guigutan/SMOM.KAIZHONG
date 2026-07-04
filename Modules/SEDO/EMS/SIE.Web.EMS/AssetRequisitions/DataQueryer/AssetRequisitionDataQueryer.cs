using SIE.Domain;
using SIE.EMS.AssetRequisitions;
using SIE.Equipments.Configs;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.AssetRequisitions.DataQueryer
{
    /// <summary>
    /// 资产领用查询器
    /// </summary>
    public class AssetRequisitionDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取领用单号
        /// </summary>
        /// <returns>领用单号</returns>
        public string GetAssetRequisitionNo()
        {
            var code = RT.Service.Resolve<AssetRequisitionController>().GetNo();
            return code;
        }

        /// <summary>
        /// 根据仓库和工治具编码获取在库数
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="id">工治具编码Id</param>
        /// <returns>工治具可用库存数</returns>
        public int GetCanUseNumByWarehouseId(double warehouseId, double id)
        {
            var encodelist = RT.Service.Resolve<CoreFixtureController>().GetCanUseNumByWarehouseId(warehouseId, new List<double>() { id });
            return encodelist.Any() ? encodelist.First().CanUseNum : 0;
        }

        /// <summary>
        /// 获取审批流配置信息
        /// </summary>
        /// <returns>审批流配置信息</returns>
        public ApprovalConfigValue GetApprovalConfigValue()
        {
            var configValue = RT.Service.Resolve<AssetRequisitionController>().GetApprovalFlowConfigValue();
            return configValue;
        }
    }
}
