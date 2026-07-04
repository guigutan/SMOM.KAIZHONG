
using SIE.ERPInterface.Common.InventoryControl;
using SIE.ERPInterface.Smom.InventoryControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ERPInterface.InventoryControl.DataQueryer
{
    /// <summary>
    /// 库存对照表查询器
    /// </summary>
    public class InventoryControlDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取当前用户的库存对照表设置
        /// </summary>
        /// <returns></returns>
        public InventoryControlSetting GetInventoryControlSetting()
        {
            return RT.Service.Resolve<InventoryControlController>().GetInventoryControlSetting();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="itemCode">物料编码</param>
        /// <param name="itemName">物料名称</param>
        /// <param name="erpLotCode">erp批次号</param>
        /// <param name="wareHouseCodes">仓库名称</param>
        /// <param name="erpCodes">Erp子库</param>
        /// <param name="isShowDifferent">只显示差异</param>
        /// <param name="isShowZero">不含0库存</param>
        /// <returns></returns>
        public InventroyControlAllData SearchInventoryControlData(string itemCode,string itemName,string erpLotCode,string wareHouseCodes,string erpCodes,bool? isShowDifferent,bool? isShowZero)
        {
            InventoryControlViewModelCriteria criteria = new InventoryControlViewModelCriteria();
            criteria.ItemCode = itemCode;
            criteria.ItemName = itemName;
            criteria.ErpLotCode = erpLotCode;
            criteria.WarehouseCode = wareHouseCodes;
            criteria.ErpWarehouseCode = erpCodes;
            criteria.IsShowDifferent = isShowDifferent;
            criteria.IsShowZero = isShowZero;
            return RT.Service.Resolve<InventoryControlController>().GetInventoryControlViewModelData(criteria);
        }
    }
}
