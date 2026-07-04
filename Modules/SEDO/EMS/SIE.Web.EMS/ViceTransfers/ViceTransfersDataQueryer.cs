using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.ViceTransfers;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Warehouses;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.ViceTransfers
{
    /// <summary>
    /// 副资产调拨
    /// </summary>
    public class ViceTransfersDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取新的资产调拨
        /// </summary>
        /// <returns></returns>
        public ViceTransfer GetNewViceTransfers()
        {
            return RT.Service.Resolve<ViceTransferController>().GetNewViceTransfers();
        }

        /// <summary>
        /// 获取工治具编码的在线数和在库数
        /// </summary>
        /// <param name="fixtureEncodeId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="fixtureQualityState"></param>
        /// <returns></returns>
        public Tuple<decimal, decimal> GetFixtureEncodeQty(double fixtureEncodeId, double warehouseId, FixtureQualityState fixtureQualityState)
        {
            return RT.Service.Resolve<ViceTransferController>().GetFixtureEncodeQty(fixtureEncodeId, warehouseId, fixtureQualityState);
        }

        /// <summary>
        /// 获取备件库存信息
        /// </summary>
        /// <param name="sparePartId"></param>
        /// <param name="warehouseCode"></param>
        /// <param name="qualityStatus"></param>
        /// <returns></returns>
        public decimal GetSparePartQty(double sparePartId, string warehouseCode, QualityStatus qualityStatus)
        {
            return RT.Service.Resolve<ViceTransferController>().GetSparePartQty(sparePartId, warehouseCode, qualityStatus);
        }

        /// <summary>
        /// 获取用户是否存在当前仓库权限
        /// </summary>
        /// <returns></returns>
        public bool GetWarehouseAvailable(double warehouseId)
        {
            var whList = RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployee(null, "");
            return whList.Any() && whList.Any(m => m.Id == warehouseId);
        }

        /// <summary>
        /// 管控方式为【批次】时，根据【批次号+质量状态+来源库位】获取可用库存或不良品数 
        /// </summary>
        /// <returns></returns>
        public int GetWarehouseLotQty(ViceTransferSparePartDetail detail)
        {
            return RT.Service.Resolve<ViceTransferController>().GetWarehouseLotQty(detail);
        }

        /// <summary>
        /// 获取工治具ID库位
        /// </summary>
        /// <param name="detail"></param>
        /// <returns>库位Id，库位名称，车间，产线 </returns>
        public Tuple<double?, string,string,string> GetFixtureIDAccountLocation(ViceTransferFixtureDetail detail)
        {
            return RT.Service.Resolve<ViceTransferController>().GetFixtureIDAccountLocationInfo(detail);
        }

        /// <summary>
        /// 管控方式为【编码】时 选择来源库位的时候获取 根据【工治具编码+质量状态+来源库位】获取合格数量或不合格数量
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public int GetFixtureStorageLocationQty(ViceTransferFixtureDetail detail)
        {
            return RT.Service.Resolve<ViceTransferController>().GetFixtureStorageLocationQty(detail);
        }

        /// <summary>
        /// 获取编码类工治具编码台账的在线数
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public int GetFixtureEncodeOnlineQty(ViceTransferFixtureDetail detail)
        {
            return RT.Service.Resolve<ViceTransferController>().GetFixtureEncodeOnlineQty(detail);
        }

        /// <summary>
        /// 获取审批配置项
        /// </summary>
        /// <returns></returns>
        public ApprovalConfigValue GetViceTransferApproval()
        {
            return RT.Service.Resolve<ViceTransferController>().GetApprovalConfigValue();
        }

        /// <summary>
        /// 执行保存
        /// </summary>
        public void ExecutSave(ViceTransfer model, EntityList<ViceTransferFixtureDetail> fixtureList,
            EntityList<ViceTransferSparePartDetail> sparePartList)
        {
            if (sparePartList.Any())
            {
                if (sparePartList.Any(m => m.TransferQty == 0))
                {
                    throw new ValidationException("调拨数量须大于0".L10N());
                }
                model.ViceTransferSparePartDetailList.Clear();
                model.ViceTransferSparePartDetailList.AddRange( sparePartList);
                RT.Service.Resolve<ViceTransferController>().ExecutSave(model);
            }
            if (fixtureList.Any())
            {
                if (fixtureList.Any(m => m.TransferQty == 0))
                {
                    throw new ValidationException("调拨数量须大于0".L10N());
                }
                model.ViceTransferFixtureDetailList.Clear();
                model.ViceTransferFixtureDetailList.AddRange(fixtureList);
                RT.Service.Resolve<ViceTransferController>().ExecutSave(model);
            }

        }
    }
}
