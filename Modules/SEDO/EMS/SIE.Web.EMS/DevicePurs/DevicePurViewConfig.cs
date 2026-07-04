using System.Collections.Generic;
using SIE.EMS.DevicePurs;
using SIE.Domain;

namespace SIE.Web.EMS.DevicePurs
{
    /// <summary>
    /// 设备与人员权限维护视图配置
    /// </summary>
    internal class DevicePurViewConfig : WebViewConfig<DevicePur>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseImportCommands();
            View.UseClientOrder();
            View.Property(p => p.UserGroup).UseDataSource((e, p, k) =>
            {
                return (RT.Service.Resolve<DevicePurController>().GetUserGroups(p, k));
            }).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.UserGroupName), nameof(r.UserGroup.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("用户组编号").Readonly(p => p.UserId != null && p.UserId != 0);

            View.Property(p => p.UserGroupName).Readonly();

            View.Property(p => p.UserId).UseDataSource((e, p, k) =>
            {
                return (RT.Service.Resolve<DevicePurController>().GetUsers(p, k));
            }).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.EmployeeName), nameof(r.User.EmployeeName));
                keyValues.Add(nameof(r.EmployeeNames), nameof(r.User.EmployeeName));
                m.DicLinkField = keyValues;
            }).HasLabel("用户编号").Readonly(p => p.UserGroupId != null && p.UserGroupId != 0);

            View.Property(p => p.EmployeeName).Readonly();
            View.Property(p => p.EmployeeNames).Show(ShowInWhere.Hide);
            View.Property(p => p.EquipMaintain);
            View.Property(p => p.CheckConfirm);
            View.Property(p => p.MaintainConfirm);
            View.Property(p => p.RepairConfirm).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);

            View.ChildrenProperty(p => p.DeviceUseDepaList).HasLabel("使用部门").HasOrderNo(1);
            View.ChildrenProperty(p => p.DeviceBillList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(DevicePur.DeviceBillListProperty,
                e =>
                {
                    var arg = e as ChildPagingDataWithParentEntityArgs;
                    var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<DevicePur>();
                    if (parent == null)
                    {
                        return new EntityList<DeviceBill>();
                    }
                    else
                    {
                        var keyword = parent.SearchKeywordDontMap;//UI工具栏附加查询输入栏值
                        return RT.Service.Resolve<DevicePurController>().GetDeviceBills(parent.Id, (List<OrderInfo>)arg.SortInfo, arg.PagingInfo, keyword);
                    }
                }).HasLabel("设备清单").HasOrderNo(3);
            View.ChildrenProperty(p => p.DeviceDepaList).HasLabel("责任部门").HasOrderNo(4);
            View.ChildrenProperty(p => p.DeviceBudgetDepartmentList).HasLabel("预算部门").HasOrderNo(5);
            View.ChildrenProperty(p => p.DevicePurchaseList).HasLabel("采购对象").HasOrderNo(6);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.UserGroup.Code).HasLabel("用户组编码");
            View.PropertyRef(p => p.User.Code).HasLabel("用户编码");
            View.Property(p => p.EquipMaintain);
            View.Property(p => p.CheckConfirm);
            View.Property(p => p.MaintainConfirm);
            View.Property(p => p.RepairConfirm).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 下拉选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.EmployeeName).Readonly();
            View.Property(p => p.EquipMaintain);
        }
    }
}