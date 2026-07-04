using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;

namespace SIE.xUnit.CSM
{
    /// <summary>
    /// CSM单元测试控制器
    /// </summary>
    public class CsmTestComtroller : DomainController
    {
        #region 供应商
        /// <summary>
        /// 创建供应商（单个）
        /// </summary>
        /// <returns></returns>
        public virtual Supplier CreateSupplier()
        {
            var supplier = new Supplier();
            supplier.GenerateId();
            supplier.Code = "SupplierCode" + supplier.Id.ToString();
            supplier.Name = "SupplierName" + supplier.Id.ToString();
            supplier.Type = "1";
            supplier.Region = "01";
            var address = new SupplierAddress();
            address.AddressType = "01";
            address.Name = supplier.Code;
            address.Contacts = "测试";
            address.Address = "测试";
            supplier.AddressList.Add(address);
            RF.Save(supplier);
            return supplier;
        }

        /// <summary>
        /// 创建供应商，关联物料
        /// </summary>
        /// <returns></returns>
        public virtual SupplierItem CreateSupplierWithItem(Item item)
        {
            var supplier = CreateSupplier();
            SupplierItem supplierItem = new SupplierItem()
            {
                ItemId = item.Id,
                Item = item,
                SupplierId = supplier.Id,
                Supplier = supplier
            };
            RF.Save(supplierItem);
            return supplierItem;
        }

        /// <summary>
        /// 创建供应商物料
        /// </summary>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="purchaseSupplyType">采购供应模式</param>
        public virtual void CreateSupplierItem(double supplierId, double itemId, PurchaseSupplyType purchaseSupplyType)
        {
            var supplerItem = new SupplierItem();
            supplerItem.SupplierId = supplierId;
            supplerItem.ItemId = itemId;
            supplerItem.PurchaseSupplyType = purchaseSupplyType;
            RF.Save(supplerItem);
        }

        /// <summary>
        /// 创建供应商用户关系
        /// </summary>
        /// <param name="supplierId"></param>
        public virtual void CreateSupplierUser(double supplierId)
        {
            var supplierUser = new SupplierUser();
            supplierUser.SupplierId = supplierId;
            supplierUser.UserId = RT.IdentityId;
            RF.Save(supplierUser);
        }
        #endregion

        #region 客户
        /// <summary>
        /// 创建客户（单个）
        /// </summary>
        /// <returns></returns>
        public virtual Customer CreateCustomer(CustomerType type)
        {
            var customer = new Customer();
            customer.GenerateId();
            customer.Code = "CustomerCode" + customer.Id.ToString();
            customer.Name = "CustomerName" + customer.Id.ToString();
            customer.Region = "01";

            customer.CustomerType = type;
            if (type == CustomerType.SHIPPER)
                customer.Supplier = CreateSupplier();

            var address = new CustomerAddress();
            address.AddressType = "01";
            address.Name = customer.Code;
            address.Contacts = "测试";
            address.Address = "测试";
            customer.CustomerAddressList.Add(address);
            RF.Save(customer);
            return customer;
        }
        #endregion
    }
}
