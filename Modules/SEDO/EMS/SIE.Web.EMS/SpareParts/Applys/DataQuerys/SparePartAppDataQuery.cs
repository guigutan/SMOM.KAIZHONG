using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.Warehouses;

namespace SIE.Web.EMS.SpareParts.Applys.DataQuerys
{
    /// <summary>
    /// 查询器
    /// </summary>
    public class SparePartAppDataQuery : Data.DataQueryer
    {

        /// <summary>
        /// 自动获取编码
        /// </summary>
        /// <returns></returns>
        public string GetNo()
        {
            var code = RT.Service.Resolve<SparePartAppController>().GetNo();
            return code;
        }

        /// <summary>
        /// 得到设备型号和领用部门
        /// </summary>
        /// <param name="accountId">设备台账Id</param>
        /// <returns></returns>
        public object GetEquipModelEnterp(double accountId)
        {
            var account = RT.Service.Resolve<SparePartAppController>().GetEquipModelByAccountId(accountId);

            var result = new { EquipModel = account?.EquipModel, Enterprise = account?.UseDepartment };

            return result;
        }

        /// <summary>
        /// 设备型号 因为子表要控制数据源所以每改变一次就要保存一下
        /// </summary>
        /// <param name="appId">申请单Id</param>
        /// <param name="equipModelId">设备模型Id</param>
        /// <returns></returns>
        public object SaveEquipModel(double appId, double equipModelId)
        {
            RT.Service.Resolve<SparePartAppController>().SaveEquipModel(appId, equipModelId);
            return null;
        }


        /// <summary>
        /// 获取库存数量
        /// </summary>
        /// <returns></returns>
        public object GetPartDepotCount(double partId, double depotId)
        {
            StoreSummaryLot storeSummaryDepot = RT.Service.Resolve<SparePartAppController>().GetStoreDepotByDepotId(partId, depotId);
            StorageLocation storageLocation = storeSummaryDepot?.StorageLocation;
            int sum = RT.Service.Resolve<SparePartAppController>().GetStoreDepotSumByDepot(partId, depotId);

            var result = new
            {
                DepotAmount = sum,
                SparePartSite = storageLocation,
            };
            return result;
        }

        /// <summary>
        /// 判断是否启用了审批流
        /// </summary>
        /// <returns></returns>
        public object VerifyIsEnableAuditFlow()
        {
            return RT.Service.Resolve<SparePartAppController>().IsEnableAuditFlow();
        }
    }
}