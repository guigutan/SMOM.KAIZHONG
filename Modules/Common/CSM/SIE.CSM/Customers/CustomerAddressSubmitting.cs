using SIE.Domain;
using SIE.EventMessages;


namespace SIE.CSM.Customers
{
    /// <summary>
    /// 新增时，默认第一个客户地址为默认地址
    /// </summary>
    [System.ComponentModel.DisplayName("客戶地址保存前事件")]
    [System.ComponentModel.Description("新增时，默认第一个客户地址为默认地址")]
    public class CustomerAddressSubmitting : OnSubmitting<CustomerAddress>
    {
        /// <summary>
        /// 客戶地址保存前事件
        /// </summary>
        /// <param name="entity">客户地址</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(CustomerAddress entity, EntitySubmittingEventArgs e)
        {
            if (e != null && entity != null && e.Action == SubmitAction.Insert &&
                RT.Service.Resolve<CustomerController>().CanSetDefaultAddress(entity.CustomerId.Value))
            {
                entity.IsDefault = true;
            }
        }
    }


    /// <summary>
    /// 新增时，默认第一个客户地址为默认地址
    /// </summary>
    [System.ComponentModel.DisplayName("客戶地址保存前事件")]
    [System.ComponentModel.Description("新增时，默认第一个客户地址为默认地址")]
    public class CustomerSubmitted : OnSubmitted<Customer>
    {
        /// <summary>
        /// 客戶地址保存前事件
        /// </summary>
        /// <param name="entity">客户地址</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(Customer entity, EntitySubmittedEventArgs e)
        {
            if (e != null && entity != null && e.Action == SubmitAction.Insert)
            {
                RT.Service.Resolve<ICustomerCreated>().CreateCustLevelSetting(entity.Id);
            }
        }
    }


}
