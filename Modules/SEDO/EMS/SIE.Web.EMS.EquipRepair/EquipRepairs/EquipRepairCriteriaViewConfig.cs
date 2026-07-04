using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修单查询实体视图配置
    /// </summary>
    internal class EquipRepairCriteriaViewConfig : WebViewConfig<EquipRepairCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccount).UseDataSource((e, p, s) =>
                {
                    //输入%才能进行模糊查询,支持编码名称查询
                    return RT.Service.Resolve<EquipAccountController>().GetEquipAccountBykeyword(p, s);
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true); 
                View.Property(p => p.SparePart).UseDataSource((e, p, s) =>
                {
                    //输入%才能进行模糊查询,支持编码名称查询
                    return RT.Service.Resolve<SparePartController>().GetSpareParts(p, s);
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true);
                View.Property(p => p.EquipOrSparePartName);
                View.Property(p => p.EquipType)
                    .UseDataSource((e,p,s) => 
                {
                    //输入%才能进行模糊查询,支持编码名称查询
                    return RT.Service.Resolve<EquipModelController>().GetEquipTypesByKeyword(p,s);
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true);

                View.Property(p => p.EquipModel)
                           .UseDataSource((e, p, s) =>
                           {
                               //输入%才能进行模糊查询,支持编码名称查询
                               return RT.Service.Resolve<EquipAccountController>().GetEquipModelLoadAll(p, s);
                           }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true);

                View.Property(p => p.RepairNo);
                View.Property(p => p.ApplyRepairEmployee);
                View.Property(p => p.RepairMaster).UseDataSource((e, p, s) =>
                {
                    //输入%才能进行模糊查询,支持编码名称查询
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(p, s);
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true); 

                View.Property(p => p.RepairEmployee).UseDataSource((e, p, s) =>
                {
                    //输入%才能进行模糊查询,支持编码名称查询
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(p, s);
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true);
                View.Property(p => p.RepairType);
                View.Property(p => p.RepairState);
                View.Property(p => p.RepairWay);

                View.Property(p => p.Workshop).UseDataSource((e, c, r) =>
                {
                    var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                    workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                    return workShopList;
                }).Show(ShowInWhere.All);
                View.Property(p => p.Process).Show(ShowInWhere.All);
                View.Property(p => p.ApplyRepairDate).UseDateRangeEditor(p => { p.DateRangeType = DateRangeType.Month; p.Format = "Y-m-d"; }).Show(ShowInWhere.All);

                View.Property(p => p.IsToFinish);
            }
                
        }
    }
}
