using SIE.Common.Users;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工删除校验
    /// </summary>
    [System.ComponentModel.DisplayName("员工删除校验")]
    [System.ComponentModel.Description("员工关联资源不允许删除")]
    public class EmployeeInvolveResource : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeInvolveResource()
        {
            Properties.Add(EmployeeResource.EmployeeIdProperty);
            MessageBuilder = (o, e) =>
            {
                var employee = o as Employee;
                return "员工[{0}]已关联资源，不允许删除".L10nFormat(employee.Name);
            };
        }
    }

    /// <summary>
    /// 员工删除校验
    /// </summary>
    [System.ComponentModel.DisplayName("员工删除校验")]
    [System.ComponentModel.Description("员工关联用户不允许删除")]
    public class EmployeeInvolveUser : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeInvolveUser()
        {
            Properties.Add(User.EmployeeIdProperty);
            MessageBuilder = (o, e) =>
            {
                var employee = o as Employee;
                return "员工[{0}]已关联用户，不允许删除".L10nFormat(employee.Name);
            };
        }
    }

    /// <summary>
    /// 员工电话号码验证规则
    /// </summary>
    //[System.ComponentModel.DisplayName("员工电话属性规则")]
    //[System.ComponentModel.Description("员工电话号码正则表达式规则")]
    //class EmployeePhoneRule : EntityRule<Employee>
    //{
    //    public EmployeePhoneRule()
    //    {
    //        Property = Employee.PhoneProperty;
    //        ConnectToDataSource = false;
    //    }
    //    protected override void Validate(IEntity entity, RuleArgs e)
    //    {
    //        var employee = entity as Employee;
    //        if (!employee.Phone.IsNullOrWhiteSpace())
    //        {
    //            const string phoneRule = @"^((\d{3}-\d{8}|\d{4}-\d{7,8})|(0?(13|14|15|17|18|19)[0-9]{9}))$";
    //            Regex phoneRegex = new Regex(phoneRule);
    //            var matches = phoneRegex.IsMatch(employee.Phone);
    //            if (!matches)
    //            {
    //                e.BrokenDescription = "员工[{0}]电话号码格式不正确".L10nFormat(employee.Code);
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 员工属性规则
    /// </summary>
    [System.ComponentModel.DisplayName("员工规则")]
    [System.ComponentModel.Description("员工属性规则")]
    class EmployeeEntityRule : EntityRule<Employee>
    {
        public EmployeeEntityRule()
        {
            ConnectToDataSource = false;
        }
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var employee = entity as Employee;
            if (employee.HireDate != null && employee.HireDate.Value.Year <= 1900)
                e.BrokenDescription = "员工[{0}]入职日期年份必须大于1900年".L10nFormat(employee.Code);
        }
    }

    /// <summary>
    /// 员工邮箱验证规则
    /// </summary>
    //[System.ComponentModel.DisplayName("员工邮箱属性规则")]
    //[System.ComponentModel.Description("员工邮箱正则表达式规则")]
    //class EmployeeEmailRule : EntityRule<Employee>
    //{
    //    public EmployeeEmailRule()
    //    {
    //        Property = Employee.EmailProperty;
    //        ConnectToDataSource = false;
    //    }
    //    protected override void Validate(IEntity entity, RuleArgs e)
    //    {
    //        var employee = entity as Employee;
    //        if (!employee.Email.IsNullOrWhiteSpace())
    //        {
    //            const string emailRule = @"^([a-zA-Z0-9])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$";
    //            Regex emailRegex = new Regex(emailRule);
    //            var matches = emailRegex.IsMatch(employee.Email);
    //            if (!matches)
    //            {
    //                e.BrokenDescription = "员工[{0}]邮箱格式不正确".L10nFormat(employee.Code);
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 生产资源删除验证规则
    /// </summary>
    [DisplayName("生产资源删除验证规则")]
    [Description("员工资源引用的生产资源不能删除")]
    public class WipResourceDeleteRuleEmployeeResource : EntityRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceDeleteRuleEmployeeResource()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 根据生产资源是否被员工资源引用，判断是否能被删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var wipResource = entity as WipResource;
            var flag = RT.Service.Resolve<EmployeeController>().EmployeeResourceHasUsedResource(wipResource.Id);
            if (flag)
            {
                e.BrokenDescription = "生产资源 [{0}] 被员工资源引用, 不能删除!".L10nFormat(wipResource.Code);
            }
        }
    }
}
