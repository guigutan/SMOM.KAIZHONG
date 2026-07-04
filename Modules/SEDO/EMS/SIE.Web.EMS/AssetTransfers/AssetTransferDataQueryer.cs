using SIE.EMS.AssetTransfers;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetTransfers
{
    /// <summary>
    /// 
    /// </summary>
    public class AssetTransferDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取新的资产调拨
        /// </summary>
        /// <returns></returns>
        public AssetTransfer GetNewAssetTransfer()
        {
            return RT.Service.Resolve<AssetTransfersController>().GetNewAssetTransfer();
        }

        /// <summary>
        /// 根据ID获取设备台账信息
        /// </summary>
        /// <returns></returns>
        public EquipAccountExtInfo GetEquipAccountInfoById(double equipAccountId)
        {
            return RT.Service.Resolve<AssetTransfersController>().GetEquipAccountInfoById(equipAccountId);
        }
    }

    
}
