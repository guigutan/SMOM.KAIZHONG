using System;
using SIE.Domain;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.InfDataEntitys.Download;

namespace SIE.ERPInterface.Api.WebApi
{
    /// <summary>
    /// 数据下载API接口
    /// ERP下载数据到中间表，需另外通过SMOM的调度，把中间表数据执行到业务表
    /// </summary>
    public partial class InfWebApiController
    {
        #region 基础数据

        /// <summary>
        /// 保存物料数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveItem(ItemData data)
        {
            var itemInf = new ItemInf();
            itemInf.Code = data.Code;
            itemInf.Name = data.Name;
            itemInf.Description = data.Description;
            itemInf.Unit = data.UnitCode;
            itemInf.ItemCategoryCode = data.CategoryCode;

            itemInf.LastUpdateDate = data.LastUpdateDate;
            itemInf.ErpKey = data.ErpKey;
            itemInf.IsDelete = data.IsDelete;
            itemInf.IsManual = false;
            itemInf.ShelfLife = data.ShelfLife;
            itemInf.IsBatch = data.IsBatch;
            itemInf.IsSerialNumber = data.IsSerialNumber;
            RF.Save(itemInf);
        }

        /// <summary>
        /// 保存物料分类数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveItemCategory(ItemCategoryData data)
        {
            var itemCateInf = new ItemCategoryInf();
            itemCateInf.Code = data.Code;
            itemCateInf.Name = data.Name;
            itemCateInf.Level = data.LevelCode;
            //itemCateInf.ItemType = data.ItemType;//需根据实际客制化
            itemCateInf.ParentCode = data.ParentCode;

            itemCateInf.LastUpdateDate = data.LastUpdateDate;
            itemCateInf.ErpKey = data.ErpKey;
            itemCateInf.IsDelete = data.IsDelete;
            itemCateInf.IsManual = false;

            RF.Save(itemCateInf);
        }

        /// <summary>
        /// 保存企业模型数据
        /// </summary>
        /// <param name="data"></param>
        private void SaveEnterprise(EnterpriseData data)
        {
            var enterpriseInf = new EnterpriseInf();
            enterpriseInf.Code = data.Code;
            enterpriseInf.Name = data.Name;
            enterpriseInf.Level = data.LevelCode;
            //itemCateInf.ItemType = data.ItemType;//需根据实际客制化
            enterpriseInf.ParentCode = data.ParentCode;

            enterpriseInf.LastUpdateDate = data.LastUpdateDate;
            enterpriseInf.ErpKey = data.ErpKey;
            enterpriseInf.IsDelete = data.IsDelete;
            enterpriseInf.IsManual = false;

            RF.Save(enterpriseInf);
        }

        /// <summary>
        /// 保存数据到客户中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveCustomer(CustomerData data)
        {
            var customerInf = new CustomerInf();
            customerInf.Code = data.Code;
            customerInf.Name = data.Name;
            customerInf.ContactsAddress = data.ContactAddress;
            customerInf.ContactsNumber = data.ContactNumber;
            customerInf.Contacts = data.Contacts;
            customerInf.DutyParagraph = data.DutyParagraph;

            customerInf.LastUpdateDate = data.LastUpdateDate;
            customerInf.ErpKey = data.ErpKey;
            customerInf.IsDelete = data.IsDelete;
            customerInf.IsManual = false;

            RF.Save(customerInf);
        }

        /// <summary>
        /// 保存客户地址数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveCustomerAddress(CustomerAddressData data)
        {
            var customerAddrInf = new CustomerAddressInf();
            customerAddrInf.Address = data.Address;
            customerAddrInf.Contacts = data.Contacts;
            customerAddrInf.Phone = data.Phone;
            customerAddrInf.ZipCode = data.ZipCode;
            customerAddrInf.Email = data.Email;
            customerAddrInf.AddressType = data.AddressType;
            customerAddrInf.Name = data.Name;
            customerAddrInf.Country = data.Country;
            customerAddrInf.Province = data.Province;
            customerAddrInf.City = data.City;
            customerAddrInf.Area = data.Area;
            customerAddrInf.Fax = data.Fax;
            customerAddrInf.Remark = data.Remark;
            customerAddrInf.CustomerCode = data.CustomerCode;

            customerAddrInf.LastUpdateDate = data.LastUpdateDate;
            customerAddrInf.ErpKey = data.ErpKey;
            customerAddrInf.IsDelete = data.IsDelete;
            customerAddrInf.IsManual = false;

            RF.Save(customerAddrInf);
        }

        /// <summary>
        /// 保存供应商数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveSupplier(SupplierData data)
        {
            var supplierInf = new SupplierInf();
            supplierInf.Code = data.Code;
            supplierInf.Name = data.Name;
            supplierInf.ContactAddress = data.ContactAddress;
            supplierInf.ContactNumber = data.ContactNumber;
            supplierInf.Contacts = data.Contacts;
            supplierInf.DutyParagraph = data.DutyParagraph;
            supplierInf.Email = data.Email;
            supplierInf.ZipCode = data.PostalCode;

            supplierInf.LastUpdateDate = data.LastUpdateDate;
            supplierInf.ErpKey = data.ErpKey;
            supplierInf.IsDelete = data.IsDelete;
            supplierInf.IsManual = false;

            RF.Save(supplierInf);
        }

        /// <summary>
        /// 保存供应商地址数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveSupplierAddress(SupplierAddressData data)
        {
            var customerAddrInf = new SupplierAddressInf();
            customerAddrInf.DetailAddress = data.Address;
            customerAddrInf.Contacts = data.Contacts;
            customerAddrInf.Phone = data.Phone;
            customerAddrInf.ZipCode = data.ZipCode;
            customerAddrInf.Email = data.Email;
            customerAddrInf.AddressType = data.AddressType;
            customerAddrInf.Name = data.Name;
            customerAddrInf.Country = data.Country;
            customerAddrInf.Province = data.Province;
            customerAddrInf.City = data.City;
            customerAddrInf.Area = data.Area;
            customerAddrInf.Fax = data.Fax;
            customerAddrInf.Remark = data.Remark;
            customerAddrInf.SupplierCode = data.SupplierCode;

            customerAddrInf.LastUpdateDate = data.LastUpdateDate;
            customerAddrInf.ErpKey = data.ErpKey;
            customerAddrInf.IsDelete = data.IsDelete;
            customerAddrInf.IsManual = false;

            RF.Save(customerAddrInf);
        }

        /// <summary>
        /// 保存员工数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveEmployee(EmployeeData data)
        {
            var employeeInf = new EmployeeInf();
            employeeInf.Code = data.Code;
            employeeInf.Name = data.Name;
            employeeInf.Sex = data.Gender;
            employeeInf.Email = data.Email;
            employeeInf.AccountCode = data.AccountCode;
            employeeInf.Phone = data.Phone;
            employeeInf.HireDate = data.HireDate;

            employeeInf.LastUpdateDate = data.LastUpdateDate;
            employeeInf.ErpKey = data.ErpKey;
            employeeInf.IsDelete = data.IsDelete;
            employeeInf.IsManual = false;

            RF.Save(employeeInf);
        }

        /// <summary>
        /// 保存产品BOM数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveProductBom(ProductBomData data)
        {
            var productBomInf = new ProductBomInf();
            productBomInf.Code = data.Code;
            productBomInf.Name = data.Name;
            productBomInf.Version = data.Version;
            productBomInf.ProductCode = data.ItemCode;

            productBomInf.LastUpdateDate = data.LastUpdateDate;
            productBomInf.ErpKey = data.ErpKey;
            productBomInf.IsDelete = data.IsDelete;
            productBomInf.IsManual = false;

            RF.Save(productBomInf);
        }

        /// <summary>
        /// 保存产品BOM明细数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveProductBomDetail(ProductBomDetailData data)
        {
            var productBomDtlInf = new ProductBomDetailInf();
            productBomDtlInf.ProductBomCode = data.ProductBomCode;
            productBomDtlInf.LossRate = data.LossRate;
            productBomDtlInf.Remark = data.Remark;
            productBomDtlInf.UnitQty = data.UnitQty;
            productBomDtlInf.ItemCode = data.ItemCode;

            productBomDtlInf.LastUpdateDate = data.LastUpdateDate;
            productBomDtlInf.ErpKey = data.ErpKey;
            productBomDtlInf.IsDelete = data.IsDelete;
            productBomDtlInf.IsManual = false;

            RF.Save(productBomDtlInf);
        }

        /// <summary>
        /// 保存仓库数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveWarehouse(WarehouseData data)
        {
            var warehouseInf = new WarehouseInf();
            warehouseInf.Code = data.Code;
            warehouseInf.Name = data.Name;
            //warehouseInf.Category = data.Category;
            warehouseInf.SimpleCode = data.SimpleCode;

            warehouseInf.LastUpdateDate = data.LastUpdateDate;
            warehouseInf.ErpKey = data.ErpKey;
            warehouseInf.IsDelete = data.IsDelete;
            warehouseInf.IsManual = false;

            RF.Save(warehouseInf);
        }

        /// <summary>
        /// 保存库区数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveStorageArea(StorageAreaData data)
        {
            var storageAreaInf = new StorageAreaInf();
            storageAreaInf.Code = data.Code;
            storageAreaInf.Name = data.Name;
            storageAreaInf.WarehouseCode = data.WarehouseCode;

            storageAreaInf.LastUpdateDate = data.LastUpdateDate;
            storageAreaInf.ErpKey = data.ErpKey;
            storageAreaInf.IsDelete = data.IsDelete;
            storageAreaInf.IsManual = false;

            RF.Save(storageAreaInf);
        }

        /// <summary>
        /// 保存库位数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveStorageLocation(StorageLocationData data)
        {
            var storageLocationInf = new StorageLocationInf();
            storageLocationInf.Code = data.Code;
            storageLocationInf.Name = data.Name;
            storageLocationInf.AreaCode = data.AreaCode;
            storageLocationInf.WhCode = data.WarehouseCode;

            storageLocationInf.LastUpdateDate = data.LastUpdateDate;
            storageLocationInf.ErpKey = data.ErpKey;
            storageLocationInf.IsDelete = data.IsDelete;
            storageLocationInf.IsManual = false;

            RF.Save(storageLocationInf);
        }

        #endregion

        #region 来源订单

        /// <summary>
        /// 保存工单数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveWorkOrder(WorkOrderData data)
        {
            var workOrderInf = new WorkOrderInf();
            workOrderInf.No = data.WorkOrderNo;
            //workOrderData.ErpWorkOrderNo = workOrderInf.No;
            workOrderInf.CustomerCode = data.CustomerCode;
            workOrderInf.CustomerOrderNo = data.CustomerOrderNo;
            workOrderInf.SaleOrderNo = data.SaleOrderNo;
            //workOrderInf.WorkOrderType = data.WorkOrderType;
            workOrderInf.MakerCode = data.MakerCode;
            workOrderInf.WorkshopCode = data.WorkshopCode;
            workOrderInf.ResourceCode = data.ResourceCode;
            workOrderInf.ProductCode = data.ProductCode;
            workOrderInf.PlanQty = data.PlanQty;
            workOrderInf.OrderQty = data.OrderQty;
            workOrderInf.PlanBeginDate = data.PlanBeginDate;
            workOrderInf.PlanEndDate = data.PlanEndDate;
            //workOrderInf.WorkOrderState = data;

            workOrderInf.LastUpdateDate = data.LastUpdateDate;
            workOrderInf.ErpKey = data.ErpKey;
            workOrderInf.IsDelete = data.IsDelete;
            workOrderInf.IsManual = false;

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                //主表
                RF.Save(workOrderInf);
                //明细
                data.BomList.ForEach(p =>
                {
                    SaveWorkOrderBom(p);
                });

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存工单BOM数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveWorkOrderBom(WorkOrderBomData data)
        {
            var workOrderBomInf = new WorkOrderBomInf();
            workOrderBomInf.ItemCode = data.ItemCode;
            workOrderBomInf.WoNo = data.WorkOrderNo;
            workOrderBomInf.RequireQty = data.RequireQty;
            workOrderBomInf.SingleQty = data.SingleQty;
            workOrderBomInf.Remark = data.Remark;
            workOrderBomInf.IsRecoilItem = data.IsRecoilItem;
            workOrderBomInf.IsVritualItem = data.IsVritualItem;

            workOrderBomInf.LastUpdateDate = data.LastUpdateDate;
            workOrderBomInf.ErpKey = data.ErpKey;
            workOrderBomInf.IsDelete = data.IsDelete;
            workOrderBomInf.IsManual = false;

            RF.Save(workOrderBomInf);
        }

        /// <summary>
        /// 保存采购订单数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SavePurchaseOrder(PoData data)
        {
            var purchaseOrderInf = new PurchaseOrderInf();
            purchaseOrderInf.No = data.No;
            purchaseOrderInf.Contacts = data.Contacts;
            purchaseOrderInf.ContactNumber = data.ContactNumber;
            purchaseOrderInf.ReceivingWhCode = data.WarehouseCode;
            purchaseOrderInf.SupplierCode = data.SupplierCode;
            purchaseOrderInf.OrderType = data.OrderType;

            purchaseOrderInf.LastUpdateDate = data.LastUpdateDate;
            purchaseOrderInf.ErpKey = data.ErpKey;
            purchaseOrderInf.IsDelete = data.IsDelete;
            purchaseOrderInf.IsManual = false;

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                //主表
                RF.Save(purchaseOrderInf);
                //明细
                data.DetailList.ForEach(p =>
                {
                    SavePurchaseOrderDtl(p);
                });

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存采购订单明细数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SavePurchaseOrderDtl(PoDetailData data)
        {
            var purchaseOrderDtlInf = new PurchaseOrderDetailInf();
            purchaseOrderDtlInf.PoNo = data.PoNo;
            purchaseOrderDtlInf.LineNo = data.LineNo;
            purchaseOrderDtlInf.ItemCode = data.ItemCode;
            purchaseOrderDtlInf.Quantity = data.Quantity;
            purchaseOrderDtlInf.UnitPrice = data.UnitPrice;
            purchaseOrderDtlInf.DeliveryDate = DateTime.Parse(data.DeliveryDate);
            purchaseOrderDtlInf.PurchaseUnit = data.UnitCode;

            purchaseOrderDtlInf.LastUpdateDate = data.LastUpdateDate;
            purchaseOrderDtlInf.ErpKey = data.ErpKey;
            purchaseOrderDtlInf.IsDelete = data.IsDelete;
            purchaseOrderDtlInf.IsManual = false;

            RF.Save(purchaseOrderDtlInf);
        }

        #endregion

        #region 业务单据

        /// <summary>
        /// 保存ASN单数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveAsn(AsnData data)
        {
            var asnInf = new AsnInf();
            asnInf.No = data.No;
            //asnInf.AsnState = data.AsnState;
            asnInf.OrderType = data.OrderType;
            asnInf.SupplierCode = data.SupplierCode;
            asnInf.WarehouseCode = data.WarehouseCode;
            asnInf.DeliveryDate = data.DeliveryDate;
            asnInf.Contacts = data.Contacts;
            asnInf.ContactNumber = data.ContactNumber;
            asnInf.Connecter = data.Connecter;
            asnInf.CustomerCode = data.CustomerCode;
            asnInf.EnterpriseCode = data.EnterpriseCode;
            asnInf.Remark = data.Remark;
            //data.ErpId = double.Parse(data.ErpKey);                    //注意，ERP表主键不一定是number类型

            asnInf.LastUpdateDate = data.LastUpdateDate;
            asnInf.ErpKey = data.ErpKey;
            asnInf.IsDelete = data.IsDelete;
            asnInf.IsManual = false;

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                //主表
                RF.Save(asnInf);
                //明细
                data.DetailList.ForEach(p =>
                {
                    SaveAsnDtl(p);
                });

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存ASN明细数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveAsnDtl(AsnDetailData data)
        {
            var asnDtlInf = new AsnDetailInf();
            asnDtlInf.AsnNo = data.AsnNo;
            asnDtlInf.ItemCode = data.ItemCode;
            asnDtlInf.ExpectQty = data.ExpectQty;
            asnDtlInf.ReceiveStorageLocation = data.ReceiveStorageLocationCode;
            asnDtlInf.OrderNo = data.OrderNo;
            asnDtlInf.PoNo = data.PoNo;
            asnDtlInf.PoLineNo = asnDtlInf.PoLineNo;

            //asnDtlInf.Lot = AsnDetail.LotProperty.Name;
            asnDtlInf.LotAtt01 = data.LotAtt01;
            asnDtlInf.LotAtt02 = data.LotAtt02;
            asnDtlInf.LotAtt04 = data.LotAtt04;
            asnDtlInf.LotAtt05 = data.LotAtt05;
            asnDtlInf.LotAtt06 = data.LotAtt06;
            asnDtlInf.LotAtt07 = data.LotAtt07;
            asnDtlInf.LotAtt08 = data.LotAtt08;
            asnDtlInf.LotAtt09 = data.LotAtt09;
            asnDtlInf.LotAtt10 = data.LotAtt10;
            asnDtlInf.LotAtt11 = data.LotAtt11;
            asnDtlInf.LotAtt12 = data.LotAtt12;
            asnDtlInf.Remark = data.Remark;
            asnDtlInf.ErpKey = data.ErpKey;

            asnDtlInf.LastUpdateDate = data.LastUpdateDate;
            asnDtlInf.ErpKey = data.ErpKey;
            asnDtlInf.IsDelete = data.IsDelete;
            asnDtlInf.IsManual = false;

            RF.Save(asnDtlInf);
        }

        /// <summary>
        /// 保存发运单数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveShippingOrder(ShippingOrderData data)
        {
            var shippingOrderInf = new ShippingOrderInf();
            shippingOrderInf.No = data.No;
            shippingOrderInf.OrderState = data.OrderState;
            shippingOrderInf.OrderType = data.OrderType;
            shippingOrderInf.DeliveryDate = data.DeliveryDate;
            shippingOrderInf.ShippingDate = data.ShippingDate;
            shippingOrderInf.Connecter = data.Connecter;

            shippingOrderInf.WarehouseCode = data.ShippingWareHouseCode;

            shippingOrderInf.SupplierCode = data.SupplierCode;
            shippingOrderInf.EnterpriseCode = data.EnterpriseCode;
            shippingOrderInf.CustomerCode = data.CustomerCode;

            shippingOrderInf.Contacts = data.Contacts;
            shippingOrderInf.ContactNumber = data.ContactNumber;
            shippingOrderInf.Address = data.Address;
            shippingOrderInf.TransportCompany = data.TransportCompany;
            shippingOrderInf.TransportNo = data.TransportNo;
            shippingOrderInf.PriorityType = data.PriorityType;
            shippingOrderInf.CancelReason = data.CancelReason;
            shippingOrderInf.Remark = data.Remark;

            shippingOrderInf.LastUpdateDate = data.LastUpdateDate;
            shippingOrderInf.ErpKey = data.ErpKey;
            shippingOrderInf.IsDelete = data.IsDelete;
            shippingOrderInf.IsManual = false;

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                //主表
                RF.Save(shippingOrderInf);
                //明细
                data.DetailList.ForEach(p =>
                {
                    SaveShippingOrderDtl(p);
                });

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存发运单明细数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveShippingOrderDtl(ShippingOrderDetailData data)
        {
            var shippingOrderDtlInf = new ShippingOrderDetailInf();
            shippingOrderDtlInf.ShippingOrderNo = data.ShippingOrderNo;
            shippingOrderDtlInf.LineNo = data.LineNo;
            shippingOrderDtlInf.ItemCode = data.ItemCode;
            shippingOrderDtlInf.ExpectQty = data.ExpectQty;
            shippingOrderDtlInf.AppointStorageLocation = data.AppointStorageLocationCode;
            shippingOrderDtlInf.OrderNo = data.OrderNo;
            shippingOrderDtlInf.PoNo = data.PoNo;
            shippingOrderDtlInf.PoDetailLineNo = data.PoDetailLineNo;
            shippingOrderDtlInf.InvoiceNo = data.InvoiceNo;
            shippingOrderDtlInf.Volume = data.Volume;
            shippingOrderDtlInf.Weight = data.Weight;
            shippingOrderDtlInf.Amount = data.Amount;
            shippingOrderDtlInf.AppointLpn = data.AppointLpn;
            shippingOrderDtlInf.AppointLotCode = data.AppointLotCode;
            shippingOrderDtlInf.OrderState = data.OrderState;

            shippingOrderDtlInf.LastUpdateDate = data.LastUpdateDate;
            shippingOrderDtlInf.ErpKey = data.ErpKey;
            shippingOrderDtlInf.IsDelete = data.IsDelete;
            shippingOrderDtlInf.IsManual = false;

            RF.Save(shippingOrderDtlInf);
        }

        /// <summary>
        /// 保存生产订单数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveProductOrder(ProductOrderData data)
        {
            var productOrderInf = new ProductOrderInf();
            productOrderInf.Code = data.Code;
            //productOrderInf.OrderType = (ProductOrderType)data.OrderType;
            productOrderInf.ItemCode = data.ItemCode;
            productOrderInf.Qty = data.Qty;
            productOrderInf.Priority = data.Priority;
            productOrderInf.RouteCode = data.RouteCode;
            //productOrderInf.OrderType = (ProductOrderType)data.OrderType;
            productOrderInf.FactoryCode = data.FactoryCode;
            productOrderInf.SaleNo = data.SaleNo;
            productOrderInf.CustomerCode = data.CustomerCode;
            productOrderInf.RequireDelivery = data.RequireDelivery;
            productOrderInf.PromiseDelivery = data.PromiseDelivery;
            productOrderInf.RawMaterialDate = data.RawMaterialDate;
            productOrderInf.SuggestStart = data.SuggestStart;
            productOrderInf.SuggestEnd = data.SuggestEnd;

            productOrderInf.LastUpdateDate = data.LastUpdateDate;
            productOrderInf.ErpKey = data.ErpKey;
            productOrderInf.IsDelete = data.IsDelete;
            productOrderInf.IsManual = false;

            RF.Save(productOrderInf);
        }

        /// <summary>
        /// 保存生产订单BOM数据中间表
        /// </summary>
        /// <param name="data"></param>
        private void SaveProductionOrderBom(ProductionOrderBomData data)
        {
            var productOrderBomInf = new ProductOrderBomInf();
            productOrderBomInf.Code = data.Code;
            productOrderBomInf.ItemCode = data.ItemCode;
            productOrderBomInf.SpecificationDesc = data.SpecificationDesc;
            //productOrderBomInf.ReplateItemType = (ReplateItemType)data.ReplateItemType;
            productOrderBomInf.MainMaterialCode = data.MainMaterialCode;
            productOrderBomInf.ElementNo = data.ElementNo;
            productOrderBomInf.RequireQty = data.RequireQty;
            productOrderBomInf.ProcessTech = data.ProcessTech;
            productOrderBomInf.Remark = data.Remark;

            productOrderBomInf.LastUpdateDate = data.LastUpdateDate;
            productOrderBomInf.ErpKey = data.ErpKey;
            productOrderBomInf.IsDelete = data.IsDelete;
            productOrderBomInf.IsManual = false;

            RF.Save(productOrderBomInf);
        }

        #endregion
    }
}
