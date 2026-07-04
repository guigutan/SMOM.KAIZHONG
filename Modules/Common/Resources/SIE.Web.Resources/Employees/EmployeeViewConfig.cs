using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Resources.Skills;
using SIE.Web.Resources.Employees.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.Resources.Employees
{
    /// <summary>
    /// 员工视图配置
    /// </summary>
    public class EmployeeViewConfig : WebViewConfig<Employee>
    {
        /// <summary>
        /// 班组员工ViewGroup
        /// </summary>
        public const string WorkGroupEmployeeView = "WorkGroupEmployeeView";

        /// <summary>
        /// 下拉查询视图
        /// </summary>
        public const string LookUpQueryView = "LookUpQueryView";

        public const string OnlyReadonlyViewStr = "OnlyReadonlyView";
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkGroupEmployeeView);
            //// 班组的员工视图View
            if (this.ViewGroup == WorkGroupEmployeeView)
            {
                WorkGroupEpyConfigView();
            }
            if (ViewGroup == LookUpQueryView)
            {
                LookUpQueryConfigView();
            }
            if (ViewGroup == OnlyReadonlyViewStr)
            {
                OnlyReadonlyView();
            }
        }

        /// <summary>
        /// 表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(WebCommandNames.Add, "SIE.Web.Resources.Employees.Commands.EmployeeEditCommand", WebCommandNames.Delete);
            View.UseCommands(typeof(LinkUserCommand).FullName, typeof(UnLinkUserCommand).FullName);
            View.UseCommands("SIE.Web.Resources.Employees.Commands.EmployeeGroupCommand");
            View.UseCommands("SIE.Web.Resources.Employees.Commands.EmployeeUploadAttachmentCommand");
            View.UseCommands(typeof(EmployeeLabelPrintCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Sex);
                View.Property(p => p.EmployeeGroup);
                View.Property(p => p.WorkGroup);
                View.Property(p => p.HireDate).UseDateEditor();
                View.Property(p => p.EmployeeStatus);
                View.Property(p => p.Phone);
                View.Property(p => p.Email);
                View.Property(p => p.Remark);
                View.Property(p => p.User);
                View.Property(p => p.OrgLevel1).Show();
                View.Property(p => p.OrgLevel2).Show();
                View.Property(p => p.OrgLevel3).Show();
                View.Property(p => p.OrgLevel4).Show();
                View.ChildrenProperty(p => p.ResourceList).IsVisible = true;
                View.ChildrenProperty(p => p.EnterpriseList).HasLabel("工厂权限").IsVisible = true;
                View.AttachChildrenProperty(typeof(EmployeeSkill), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var employee = args.Parent as Employee;
                    var employeeSkillList = new EntityList<EmployeeSkill>();
                    if (employee != null)
                    {
                        var ctl = RT.Service.Resolve<SkillController>();
                        employeeSkillList = ctl.GetEmployeeSkillList(employee.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                    }
                    return employeeSkillList;
                }, ListView).Show(ChildShowInWhere.All).HasLabel("技能").OrderNo = 99;
                View.AttachChildrenProperty(typeof(EmployeeSign), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var employee = args.Parent as Employee;
                    var employeeSignList = new EntityList<EmployeeSign>();
                    if (employee != null)
                    {
                        var ctl = RT.Service.Resolve<EmployeeController>();
                        employeeSignList = ctl.GetEmployeeSign(employee.Id);
                    }
                    return employeeSignList;
                }, ListView).Show(ChildShowInWhere.All).HasLabel("签名").OrderNo = 100;
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Resources.Employees.Behaviors.EditEmployeeBehavior");
            View.ReplaceCommands(WebCommandNames.FormSave, typeof(EmployeeSaveCommand).FullName).HasDetailColumnsCount(2);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Photo).UseImageComponentEditor(p => { p.Width = 300; p.Height = 400; p.Border = 1; }).ShowInDetail(rowSpan: 11);
            View.Property(p => p.Name);
            View.Property(p => p.Sex);
            View.Property(p => p.EmployeeGroup);
            View.Property(p => p.WorkGroup);
            View.Property(p => p.HireDate).UseDateTimeEditor(p=> { p.MinValue = new DateTime(1900,01,01); });
            View.Property(p => p.EmployeeStatus);
            View.Property(p => p.EmployeeType).UseEnumEditor();
            View.Property(p => p.Phone);
            View.Property(p => p.Email);
            View.Property(p => p.Remark);
            View.Property(p => p.FeiId);
            View.Property(p => p.User).UsePagingLookUpEditor()
                .UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetNotLinkedUser(r, c);
                });
            View.ChildrenProperty(p => p.ResourceList).IsVisible = false;
            View.ChildrenProperty(p => p.EnterpriseList).IsVisible = false;
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected  void OnlyReadonlyView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.HireDate).UseDateEditor();
            View.Property(p => p.User);
            View.Property(p => p.WorkGroup);
            View.Property(p => p.EmployeeStatus);
            View.Property(p => p.Sex);
            View.ChildrenProperty(p => p.ResourceList).IsVisible = false;
            View.ChildrenProperty(p => p.EnterpriseList).IsVisible = false;
        }
        

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.HireDate).UseDateEditor();
            View.Property(p => p.User);
            View.Property(p => p.WorkGroup);
            View.Property(p => p.EmployeeStatus);
            View.Property(p => p.Sex);
        }

        /// <summary>
        ///  配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Sex);
        }

        /// <summary>
        /// 班组的员工视图
        /// </summary>
        protected void WorkGroupEpyConfigView()
        {
            View.InlineEdit();
            View.UseClientOrder();
            View.UseCommands(typeof(ChangeGroupCommand).FullName, typeof(ChargehandCommand).FullName, typeof(MonitorCommand).FullName, typeof(ForemanCommand).FullName, typeof(ClearTypeCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly().Show();
                View.Property(p => p.EmployeeGroup).Readonly().Show();
                View.Property(p => p.EmployeeType).Readonly().Show();
                View.Property(p => p.Name).Readonly().Show();
                View.Property(p => p.Sex).Readonly().Show();
                View.Property(p => p.WorkGroup).Readonly().Show();
                View.Property(p => p.HireDate).Readonly().UseDateEditor().Show();
                View.Property(p => p.Phone).Readonly().Show();
                View.Property(p => p.Email).Readonly().Show();

                View.ChildrenProperty(p => p.ResourceList).IsVisible = false;
                View.ChildrenProperty(p => p.EnterpriseList).IsVisible = false;
            }
        }

        /// <summary>
        /// 弹出框查询实体
        /// </summary>
        protected void LookUpQueryConfigView()
        {
            View.ClearCommands();
            View.Property(p => p.Code).HasLabel("编码").Show();
            View.Property(p => p.Name).HasLabel("名称").Show();
            View.Property(p => p.Sex).HasLabel("性别").Show();
            View.Property(p => p.EmployeeGroup).HasLabel("员工组").Show();
            View.Property(p => p.WorkGroup).HasLabel("班组").Show();
            View.Property(p => p.HireDate).UseDateEditor().HasLabel("入职时间").Show();
            View.Property(p => p.EmployeeStatus).HasLabel("员工状态").Show();
            View.Property(p => p.Phone).HasLabel("电话号码").Show();
            View.Property(p => p.Email).HasLabel("电子邮件").Show();
            View.Property(p => p.Remark).HasLabel("备注").Show();
            View.Property(p => p.User).HasLabel("关联用户").Show();
            View.ChildrenProperty(p => p.ResourceList).IsVisible = false;
            View.ChildrenProperty(p => p.EnterpriseList).IsVisible = false;
        }
    }
}
