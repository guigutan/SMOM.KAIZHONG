using SIE.Domain;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 新增时，默认第一个供应商地址为默认地址
    /// </summary>
    [System.ComponentModel.DisplayName("供应商地址保存前事件")]
    [System.ComponentModel.Description("新增时，默认第一个供应商地址为默认地址")]
    public class SupplierAddressSubmitting : OnSubmitting<SupplierAddress>
    {
        /// <summary>
        /// 供应商地址保存前事件
        /// </summary>
        /// <param name="entity">供应商地址</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(SupplierAddress entity, EntitySubmittingEventArgs e)
        {
            if (e != null && entity != null && e.Action == SubmitAction.Insert &&
                RT.Service.Resolve<SupplierController>().CanSetDefaultAddress(entity.SupplierId.Value))
            {
                entity.IsDefault = true;
            }
        }
    }
}
