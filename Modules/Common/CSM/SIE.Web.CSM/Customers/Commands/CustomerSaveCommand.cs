using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.Web.CSM.Customers.Commands
{
    /// <summary>
    /// 客户保存命令
    /// </summary>
    [JsCommand("SIE.Web.CSM.Customers.Commands.CustomerSaveCommand")]
    public class CustomerSaveCommand : SaveCommand
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ValidationException"></exception>
        protected override void DoSave(EntityList data)
        {
            var cusList = data as EntityList<Customer>;
            var delIds = cusList.DeletedList.Select(a => a.GetId());
            cusList.Where(a => !delIds.Contains(a.Id) || a.PersistenceStatus == PersistenceStatus.New).ForEach(customer =>
            {
                if (customer.ContactsNumber.IsNotEmpty())
                {
                    var regex = new Regex(@"^(\d{7,8})$|^(0\d{2,3}-\d{7,8})$|^(1[3456789]\d{9})$");
                    if (!regex.IsMatch(customer.ContactsNumber))
                        throw new ValidationException("客户[{0}]联系电话格式不正确".L10nFormat(customer.Name));
                }
                else if (customer.ZipCode.IsNotEmpty())
                {
                    var regex = new Regex(@"^\d{6}$");
                    if (!regex.IsMatch(customer.ZipCode))
                        throw new ValidationException("客户[{0}]邮编格式不正确".L10nFormat(customer.Name));
                }
                else if (customer.EMail.IsNotEmpty())
                {
                    var regex = new Regex(@"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$");
                    if (!regex.IsMatch(customer.EMail))
                        throw new ValidationException("客户[{0}]电子邮箱格式不正确".L10nFormat(customer.Name));
                }
            });

            base.DoSave(data);
        }
    }
}
