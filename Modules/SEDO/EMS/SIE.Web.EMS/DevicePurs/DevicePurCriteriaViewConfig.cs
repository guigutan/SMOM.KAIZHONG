using SIE.EMS.DevicePurs;
using SIE.Rbac.Users;
using System.Collections.Generic;

namespace SIE.Web.EMS.DevicePurs
{
    /// <summary>
    /// 设备与人员权限查询实体视图配置
    /// </summary>
    internal class DevicePurCriteriaViewConfig : WebViewConfig<DevicePurCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.UserGroup).UseDataSource((e, p, k) =>
            {
                return (RT.Service.Resolve<DevicePurController>().GetUserGroups(p, k));
            }).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.UserGroupName), nameof(r.UserGroup.Name));
                m.DicLinkField = keyValues;
                m.ValueField = UserGroup.CodeProperty.Name;
            }).HasLabel("用户组编号");

            View.Property(p => p.UserGroupName);
            View.Property(p => p.EmployeeId).UseDataSource((e, p, k) =>
            {
                return (RT.Service.Resolve<DevicePurController>().GetUsers(p, k));
            }).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.EmployeeName), nameof(r.Employee.EmployeeName));
                m.DicLinkField = keyValues;
            }).HasLabel("用户编号");
            View.Property(p => p.EmployeeName);
        }
    }
}
