using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.CSM.Customers
{
    /// <summary>
    /// 客户保存事件
    /// </summary>
    [System.ComponentModel.DisplayName("客户保存事件")]
    [System.ComponentModel.Description("客户保存事件")]
    public class CustomerSubmitting : OnSubmitting<Customer>
    {
        /// <summary>
        /// 客户保存事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(Customer entity, EntitySubmittingEventArgs e)
        {
            if (e == null || entity == null)
            {
                return;
            }
            if ((e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update) 
                && entity.SupplierId.HasValue 
                && RT.Service.Resolve<CustomerController>().CheckExistTypeAndSupplier(entity.CustomerType, entity.SupplierId.Value))
            {
                throw new ValidationException("已存在相同的类型+供应商的记录".L10N());
            }
        }
    }
}
