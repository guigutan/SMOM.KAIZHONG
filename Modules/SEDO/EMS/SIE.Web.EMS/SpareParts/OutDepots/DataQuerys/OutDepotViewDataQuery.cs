using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.ViewModels;
using SIE.Equipments.EquipAccounts;
using System;

namespace SIE.Web.EMS.SpareParts.OutDepots.DataQuerys
{
    /// <summary>
    /// 备件出库单
    /// </summary>
    public class OutDepotViewDataQuery : Data.DataQueryer
    {
        /// <summary>
        /// 获取自动生成备件出库编号No
        /// </summary>
        /// <returns>编号</returns>
        public string GetNo()
        {
            var code = RT.Service.Resolve<OutDepotController>().GetNo();
            return code;
        }

        /// <summary>
        /// 获取设备台账中的设备型号和领用部门
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public object GetEquipModelEnterp(double accountId)
        {
            var account = RF.GetById<EquipAccount>(accountId, new EagerLoadOptions()
                 .LoadWith(EquipAccount.EquipModelProperty)
                 .LoadWith(EquipAccount.UseDepartmentProperty));
            var result = new { EquipModel = account?.EquipModel, Enterprise = account?.UseDepartment };
            return result;
        }


        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="batchNoRefId"></param>
        /// <returns>库位</returns>
        public object GetOutLocal(double batchNoRefId)
        {
            var result = RT.Service.Resolve<OutDepotController>().GetOutLocal(batchNoRefId);
            return result;
        }

        /// <summary>
        /// 备件条码出库查询
        /// </summary>
        /// <param name="barcode">备件条码</param>
        /// <param name="form">出库单头信息</param>
        /// <returns>出库条码扫描返回信息</returns>
        public OutDepotQueryInfo OutDepotBarcodeQuery(string barcode, OutDepot form)
        {
            return RT.Service.Resolve<OutDepotController>().OutDepotBarcodeQuery(barcode, form);
        }

        /// <summary>
        /// 备件数量出库查询
        /// </summary>
        /// <param name="qty">备件数量</param>
        /// <param name="form">出库单头信息</param>
        /// <param name="pickedQty">已拣数量</param>
        /// <returns>出库数量输入返回信息</returns>
        public OutDepotQueryInfo OutDepotQtyQuery(int qty, OutDepot form, int pickedQty)
        {
            return RT.Service.Resolve<OutDepotController>().OutDepotQtyQuery(qty, form,pickedQty);
        }
    }
}
