using SIE.Andon.Andons;
using SIE.Common.Organizations;
using SIE.Core.Items;
using SIE.MetaModel.View;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护触发权限视图配置
    /// </summary>
    public class AndonTypeTriggerPowerViewConfig : WebViewConfig<AndonTypeTriggerPower>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseCommands("SIE.Web.Andon.Andons.Commands.AndonTypeTriggerPowerAddStaffCommand"
            //                 , "SIE.Web.Andon.Andons.Commands.AndonTypeTriggerPowerAddRoleCommand"
            //                 , "SIE.Web.Andon.Andons.Commands.AndonTypeTriggerPowerAddUserGroupCommand"
            //                 , "SIE.Web.Andon.Andons.Commands.AndonTypeTriggerPowerAddOrganizationCommand"
            //                 , "SIE.Web.Andon.Andons.Commands.AndonTypeTriggerPowerAddWorkGroupCommand");
            View.UseCommands("SIE.Web.Andon.Andons.Commands.AndonTypeTriggerPowerAddStaffCommand"
                           , "SIE.Web.Andon.Andons.Commands.AndonTypeTriggerPowerAddUserGroupCommand");
            View.UseCommands(WebCommandNames.Delete);
            View.Property(p => p.ObjectType);
            View.Property(p => p.ObjectCode);
            View.Property(p => p.ObjectName);
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.ObjectCode);
            View.Property(p => p.ObjectName);
        }
    }

    /// <summary>
    /// 添加员工视图
    /// </summary>
    public class EmployeeAlterViewConfig : WebViewConfig<Employee>
    {
        /// <summary>
        /// 添加员工视图字符串
        /// </summary>
        public const string SelectEmployeeView = "SelectEmployeeView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SelectEmployeeView);
            if (ViewGroup == SelectEmployeeView)
            {
                using (View.OrderProperties())
                {
                    View.DisableEditing();
                    View.WithoutPaging();
                    View.Property(p => p.Code).HasLabel("员工编码").Show();
                    View.Property(p => p.Name).HasLabel("员工名称").Show();
                    View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                }
            }
        }
    }

    /// <summary>
    /// 添加角色视图
    /// </summary>
    public class RoleAlterViewConfig : WebViewConfig<Role>
    {
        /// <summary>
        /// 添加角色视图字符串
        /// </summary>
        public const string SelectRoleView = "SelectRoleView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SelectRoleView);
            if (ViewGroup == SelectRoleView)
            {
                using (View.OrderProperties())
                {
                    View.WithoutPaging();
                    View.DisableEditing();
                    View.Property(p => p.Code).HasLabel("角色编码").Show();
                    View.Property(p => p.Name).HasLabel("角色名称").Show();
                    View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                }
            }
        }
    }

    /// <summary>
    /// 添加用户组视图
    /// </summary>
    public class UserGroupViewConfig : WebViewConfig<UserGroup>
    {
        /// <summary>
        /// 添加用户组视图字符串
        /// </summary>
        public const string SelectUserGroupView = "SelectUserGroupView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SelectUserGroupView);
            if (ViewGroup == SelectUserGroupView)
            {
                using (View.OrderProperties())
                {
                    View.WithoutPaging();
                    View.DisableEditing();
                    View.Property(p => p.Code).HasLabel("用户组编码").Show();
                    View.Property(p => p.Name).HasLabel("用户组名称").Show();
                    View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                }
            }
        }
    }

    /// <summary>
    /// 添加部门视图
    /// </summary>
    public class OrganizationViewConfig : WebViewConfig<Organization>
    {
        /// <summary>
        /// 添加部门视图字符串
        /// </summary>
        public const string SelectOrganizationView = "SelectOrganizationView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SelectOrganizationView);
            if (ViewGroup == SelectOrganizationView)
            {
                using (View.OrderProperties())
                {
                    View.WithoutPaging();
                    View.DraggableForTree();
                    View.DisableEditing();
                    View.Property(p => p.Code).HasLabel("部门编码").Show();
                    View.Property(p => p.Name).HasLabel("部门名称").Show();
                    View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                }
            }
        }
    }

    /// <summary>
    /// 添加班组视图
    /// </summary>
    public class WorkGroupViewConfig : WebViewConfig<WorkGroup>
    {
        /// <summary>
        /// 添加班组视图字符串
        /// </summary>
        public const string SelectWorkGroupView = "SelectWorkGroupView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SelectWorkGroupView);
            if (ViewGroup == SelectWorkGroupView)
            {
                using (View.OrderProperties())
                {
                    View.WithoutPaging();
                    View.DisableEditing();
                    View.Property(p => p.Code).HasLabel("班组编码").Show();
                    View.Property(p => p.Name).HasLabel("班组名称").Show();
                    View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                }
            }
        }
    }
}
