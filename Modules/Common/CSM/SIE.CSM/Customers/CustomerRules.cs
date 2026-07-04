using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Text.RegularExpressions;

namespace SIE.CSM.Customers
{
    /// <summary>
    /// 客户验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("客户删除验证规则")]
    [System.ComponentModel.Description("客户已启用不能删除")]
    public class UndeleteCustomerEnable : EntityRule<Customer>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteCustomerEnable()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var customer = entity as Customer;
            if (customer.State == State.Enable)
            {
                e.BrokenDescription = "客户已启用不能删除".L10N();
            }
        }
    }

    /// <summary>
    /// 客户类型的数据必须录入供应商
    /// </summary>
    [System.ComponentModel.DisplayName("客户中的供应商数据维护")]
    [System.ComponentModel.Description("客户类型的数据必须录入供应商")]
    public class CustomerMustSupplierEnable : EntityRule<Customer>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerMustSupplierEnable()
        {
            Property = Customer.SupplierIdProperty;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var customer = entity as Customer;
            if (customer.CustomerType == CustomerType.SHIPPER)
            {
                if ((!customer.SupplierId.HasValue || customer.SupplierId.Value == 0))
                    e.BrokenDescription = "必须录入供应商".L10N();
                else
                {
                    var count = RT.Service.Resolve<CustomerController>().GetCustoerCount(customer.Id, customer.SupplierId);
                    if (count > 0)
                        e.BrokenDescription = "供应商【{0}】已关联其他客户".L10nFormat(customer.Supplier.Name);
                }
            }
        }
    }

    /// <summary>
    /// 客户验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("客户实体验证规则")]
    [System.ComponentModel.Description("客户实体验证规则")]
    public class CustomerRule : EntityRule<Customer>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            //ERP那边没有对数据进行严格限制,为了下载先去掉,移到了界面保存命令校验
            //var customer = entity as Customer;
            //if (customer.ContactsNumber.IsNotEmpty())
            //{
            //    var regex = new Regex(@"^(\d{7,8})$|^(0\d{2,3}-\d{7,8})$|^(1[3456789]\d{9})$");
            //    if (!regex.IsMatch(customer.ContactsNumber))
            //        e.BrokenDescription = "联系电话格式不正确".L10N();
            //}
            //else if (customer.ZipCode.IsNotEmpty())
            //{
            //    var regex = new Regex(@"^\d{6}$");
            //    if (!regex.IsMatch(customer.ZipCode))
            //        e.BrokenDescription = "邮编格式不正确".L10N();
            //}
            //else if (customer.EMail.IsNotEmpty())
            //{
            //    var regex = new Regex(@"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$");
            //    if (!regex.IsMatch(customer.EMail))
            //        e.BrokenDescription = "电子邮箱格式不正确".L10N();
            //}
        }
    }
}
