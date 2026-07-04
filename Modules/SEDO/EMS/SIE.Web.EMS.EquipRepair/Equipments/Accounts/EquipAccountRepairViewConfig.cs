using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Web.EMS.EquipRepair.EquipRepairs;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipRepair.Equipments.Accounts
{
    /// <summary>
    /// 设备台账维修视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EquipAccountRepairViewConfig : WebViewConfig<EquipAccount>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            base.ConfigView();
            View.AttachChildrenProperty(typeof(EquipRepairBill), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<EquipAccount>();
                if (parent == null)
                    return new EntityList<EquipRepairBill>();

                //界面加载数据源、页签工具栏查询按钮触发的数据源
                var state = (EquipRepairState?)parent.GetRepairStateDontMap();
                var repairs = RT.Service.Resolve<RepairController>()
                    .GetNotCompletedEquipRepairBills(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo, state);
                return repairs;
            }, EquipRepairViewConfig.EquipAccountRepairView).HasLabel("维修记录").HasOrderNo(120);
        }
    }
}
