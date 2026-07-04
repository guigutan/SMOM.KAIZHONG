using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Ebs.Download.Receipts;
using SIE.Items;
using SIE.Packages;
using SIE.Resources.Enterprises;
using SIE.Warehouses;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// ASN订单下载控制器
    /// </summary>
    public class DownloadAsnController : DomainController
    {
        /// <summary>
        /// 从API下载ASN单到业务表
        /// </summary>
        /// <param name="asnDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadAsnToBusiness(List<AsnData> asnDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<AsnData>(
                asnDatas,
                p => this.SaveASNs(p.OrderByLastUpdateDate()),
                JobType.Asn,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载ASN到业务表
        /// </summary>
        public virtual ProcessResult DownloadAsnInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<AsnInf, AsnDetailInf>(
                () => ctl.GetUnprocessedDatas<AsnInf>(),
                p =>
                {
                    //ASN明细中间表数据
                    var nos = p.Select(y => y.No).Distinct().ToList();
                    var whereDtl = nos.CreateContainsExpression<AsnDetailInf>("x", AsnDetailInf.AsnNoProperty.Name);
                    var dtlDatas = ctl.GetUnprocessedDatas(whereDtl);
                    return dtlDatas;
                },
                (x, y) =>
                {
                    //构建明细数据嵌套字典
                    var dtlDataDicts = ctl.GenerateDictionarys<string, AsnDetailInf>(y, AsnDetailInf.AsnNoProperty);

                    //调用业务接口
                    var paras = this.GenerateAsnPara(x, dtlDataDicts);
                    return this.SaveASNs(paras.OrderByLastUpdateDate());
                },
                JobType.Asn, JobType.AsnDtl, isManual);
        }

        /// <summary>
        /// 生成ASN行实体
        /// </summary>
        /// <param name="asnInfs">中间表实体数据</param>
        /// <param name="asnDtlInfs">中间表明细实体数据</param>
        /// <returns></returns>
        private List<AsnData> GenerateAsnPara(IEnumerable<AsnInf> asnInfs, Dictionary<string, List<AsnDetailInf>> asnDtlInfs)
        {
            var paras = new List<AsnData>();
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();
            var dtlCtl = RT.Service.Resolve<DownloadAsnDtlController>();

            asnInfs.ForEach(p =>
            {
                //构建子列表
                List<AsnDetailInf> details;
                if (asnDtlInfs.TryGetValue(p.No, out details))
                    asnDtlInfs.Remove(p.No);      //由于来源数据集允许重复数据，已取值明细清除，避免重复构建浪费资源
                else
                    details = new List<AsnDetailInf>();
                ctl.GenerateChildren(p, details);
                var dtlDatas = dtlCtl.GenerateAsnDtlPara(details);

                //构建主数据
                var data = new AsnData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.No = p.No;
                //data.AsnState = p.AsnState;
                data.OrderType = p.OrderType;
                data.ShipperCode = "*";
                data.SupplierCode = p.SupplierCode;
                data.WarehouseCode = p.WarehouseCode;
                data.DeliveryDate = p.DeliveryDate;
                ////asnData.Contacts = asnInf.Contacts;
                ////asnData.ContactNumber = asnInf.ContactNumber;
                ////asnData.Connecter = asnInf.Connecter;
                data.CustomerCode = p.CustomerCode;
                data.EnterpriseCode = p.EnterpriseCode;
                data.Remark = p.Remark;
                data.ErpKey = p.ErpKey;
                data.ErpId = double.Parse(p.ErpKey);                    //注意，ERP表主键不一定是number类型
                data.DetailList = dtlDatas;                                    //附加字列表                          

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 手动下载
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        public virtual string DownloadManual(string keyWord)
        {
            ProcessResult result = new ProcessResult();
            string resultMsg = string.Empty;

            try
            {
                if (keyWord.IsNullOrEmpty())
                    throw new ValidationException("唯一主键不能为空".L10N());
                using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<SoapAsnController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadAsnInfToBusiness(true);           //执行业务表下载
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                result.AddFailMsg(ex.GetBaseException());
            }

            if (!result.Result) resultMsg = result.FailMsg.FirstOrDefault();
            return resultMsg;
        }


        /// <summary>
        /// 用于接口中心下载数据保存到SMOM ASN表中
        /// </summary>
        /// <returns>错误数据列表</returns>
        public virtual List<ErpErrorData> SaveASNs(List<AsnData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //var ctl = RT.Service.Resolve<AsnService>();

            #region 获取数据

            List<string> noList = datas.Select(p => p.No).Distinct().ToList();
            //var asnList = ctl.GetAsnDatas(noList);
            //var asnDict = asnList.ToDictionary(p => p.No, p => p);

            //获取明细数据
            //var asnDetails = ctl.GetAsnDetails(asnList.Select(p => p.Id).ToList(), new EagerLoadOptions().LoadWith(AsnDetail.AsnProperty));
            //var dicAsnDetails = asnDetails.GroupBy(p => p.Asn.No).ToDictionary(p => p.Key, p => p.ToList());    //<AsnNo,Asn明细列表>

            //获取供应商数据
            var supplierCodes = datas.Select(p => p.SupplierCode).Distinct().ToList();
            var suppliers = RT.Service.Resolve<SupplierController>().GetSuppliers(supplierCodes);
            var supplierDict = suppliers.ToDictionary(p => p.Code, p => p);

            //获取部门数据
            var enterpriseCodes = datas.Select(p => p.EnterpriseCode).Distinct().ToList();
            var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(enterpriseCodes);
            var enterpriseDict = enterprises.ToDictionary(p => p.Code, p => p);

            //获取客户数据
            var customerCodes = datas.Select(p => p.CustomerCode).Distinct().ToList();
            var customers = RT.Service.Resolve<CustomerController>().GetCustomers(customerCodes);
            var customerDict = customers.ToDictionary(p => p.Code, p => p);

            //获取货主数据
            var shipperCodes = datas.Select(p => p.ShipperCode).Distinct().ToList();
            var shippers = RT.Service.Resolve<CustomerController>().GetCustomers(shipperCodes, CustomerType.SHIPPER);
            var shippersDict = shippers.ToDictionary(p => p.Code, p => p);

            //获取仓库数据
            var warehouseCodes = datas.Select(p => p.WarehouseCode).Distinct().ToList();
            var warehouses = RT.Service.Resolve<WarehouseController>().GetUserWarehouses(warehouseCodes, string.Empty, null);
            var warehouseDict = warehouses.ToDictionary(p => p.Code, p => p);

            //获取物料数据
            var itemCodes = datas.SelectMany(p => p.DetailList).Select(p => p.ItemCode).Distinct().ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes);
            var itemDict = items.ToDictionary(p => p.Code, p => p);           

            //获取PO
            var poNoList = datas.SelectMany(p => p.DetailList).Select(p => p.PoNo).Distinct().ToList();
            //var poList = RT.Service.Resolve<PO.PurchaseOrders.PurchaseOrderController>().GetPurchaseOrderDatas(poNoList);
            //var poDict = poList.ToDictionary(p => p.No, p => p);

            //获取物料包装规则
            var itemIdList = itemDict.Select(p => p.Value.Id).Distinct().ToList();
            var packDetailDic = RT.Service.Resolve<PackageController>().GetItemsMasterUnit(itemIdList);

            #endregion

            foreach (var data in datas)
            {
                try
                {
                    //获取仓库实体
                    Warehouse warehouse;
                    if (!warehouseDict.TryGetValue(data.WarehouseCode, out warehouse))
                        throw new ValidationException("仓库编码[{0}]不存在".L10nFormat(data.WarehouseCode));
                    //获取库位
                    var locationCodeList = data.DetailList.Select(p => p.ReceiveStorageLocationCode).Distinct().ToList();
                    var locationList = RT.Service.Resolve<WarehouseController>().GetStorageLocations(warehouse.Id, locationCodeList);
                    var locDict = locationList.ToDictionary(p => p.Code, p => p);

                    //SaveAsn(data, asnDict, supplierDict, enterpriseDict, customerDict, shippersDict, warehouseDict, dicAsnDetails, itemDict, locDict, poDict, packDetailDic);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = data.Infkey });
                }
            }
            return errors;
        }

        /// <summary>
        /// 保存ASN
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dicAsn"></param>
        /// <param name="dicSupplier"></param>
        /// <param name="dicEnterprise"></param>
        /// <param name="dicCustomer"></param>
        /// <param name="dicShipper"></param>
        /// <param name="dicWarehouse"></param>
        /// <param name="dicDetails"></param>
        /// <param name="dicItem"></param>
        /// <param name="dicLocation"></param>
        /// <param name="dicPo"></param>
        /// <param name="dicPackDetail"></param>
        //private void SaveAsn(AsnData data,
        //    Dictionary<string, Asn> dic,
        //    Dictionary<string, Supplier> dicSupplier,
        //    Dictionary<string, Enterprise> dicEnterprise,
        //    Dictionary<string, Customer> dicCustomer,
        //    Dictionary<string, Customer> dicShipper,
        //    Dictionary<string, Warehouse> dicWarehouse,
        //    Dictionary<string, List<AsnDetail>> dicDetails,
        //    Dictionary<string, Item> dicItem,
        //    Dictionary<string, StorageLocation> dicLocation,
        //    Dictionary<string, PO.PurchaseOrders.PurchaseOrder> dicPo,
        //    Dictionary<double, ItemPackageRuleDetail> dicPackDetail)
        //{
        //    using (var tran = DB.TransactionScope(ReceiptEntityDataProvider.ConnectionStringName))
        //    {
        //        var ctl = RT.Service.Resolve<DownloadBusBaseController>();

        //        Asn asn;
        //        var key = data.No;
        //        if (key.IsNullOrEmpty())
        //            throw new ValidationException("ASN单号为空".L10nFormat(key));
        //        //处理待删除数据
        //        if (dic.ContainsKey(key))
        //        {
        //            if (data.IsDelete)
        //            {
        //                ctl.DeleteEntity(dic, key, dic[key]);
        //            }
        //            return;
        //        }
        //        if (!dic.ContainsKey(key))
        //            dic.Add(key, new Asn());
        //        asn = dic[key];

        //        //获取供应商实体
        //        Supplier supplier = null;
        //        if (data.OrderType == (int)OrderType.PurchaseIn && !dicSupplier.TryGetValue(data.SupplierCode, out supplier))
        //        {
        //            throw new ValidationException("供应商编码[{0}]不存在".L10nFormat(data.SupplierCode));
        //        }

        //        Enterprise enterprise = null;
        //        if ((data.OrderType == (int)OrderType.Finished || data.OrderType == (int)OrderType.PartedIn || data.OrderType == (int)OrderType.MaterialReturn) && !dicEnterprise.TryGetValue(data.EnterpriseCode, out enterprise))
        //        {
        //            throw new ValidationException("部门编码[{0}]不存在".L10nFormat(data.EnterpriseCode));
        //        }

        //        Customer customer = null;
        //        if (data.OrderType == (int)OrderType.SaleReturn && !dicCustomer.TryGetValue(data.CustomerCode, out customer))
        //        {
        //            throw new ValidationException("客户编码[{0}]不存在".L10nFormat(data.CustomerCode));
        //        }

        //        Customer shipper = null;
        //        if (data.OrderType == (int)OrderType.VMIIN && !dicShipper.TryGetValue(data.ShipperCode, out shipper))
        //        {
        //            throw new ValidationException("货主编码[{0}]不存在".L10nFormat(data.ShipperCode));
        //        }

        //        //获取仓库实体
        //        Warehouse warehouse;
        //        if (!dicWarehouse.TryGetValue(data.WarehouseCode, out warehouse))
        //            throw new ValidationException("仓库编码[{0}]不存在".L10nFormat(data.WarehouseCode));

        //        asn.No = data.No;
        //        asn.AsnState = data.AsnState;
        //        asn.AsnSource = AsnSource.ErpWay;
        //        asn.OrderType = (OrderType)data.OrderType;
        //        asn.DeliveryDate = data.DeliveryDate;
        //        asn.ShipperCode = "*";
        //        asn.ShipperName = "*";
        //        asn.Warehouse = warehouse;

        //        asn.Supplier = supplier;
        //        asn.Enterprise = enterprise;
        //        asn.Customer = customer;

        //        asn.ShipperId = shipper?.Id;
        //        asn.ShipperCode = shipper?.Code;
        //        asn.ShipperName = shipper?.Name;
        //        asn.SupplierId = shipper?.SupplierId;
                 
        //        asn.SourceKey = data.ErpKey;

        //        RF.Save(asn);

        //        var dtlCtl = RT.Service.Resolve<DownloadAsnDtlController>();

        //        if (data.DetailList.Count > 0)
        //        {
        //            if (!dicDetails.ContainsKey(data.No))
        //                dicDetails.Add(data.No, new List<AsnDetail>());
        //            var dicDetail = dicDetails[data.No].ToDictionary(p => "{0}_{1}".FormatArgs(p.Item.Code, p.LineNo));   //物料+行号 为主键
        //            int i = 1;
        //            foreach (var detail in data.DetailList)
        //            {
        //                dtlCtl.SaveAsnDetail(detail, dicDetail, dicItem, asn, dicLocation, dicPo, dicPackDetail,i.ToString());
        //                i++;
        //            }
        //        }
        //        tran.Complete();
        //    }
        //}



    }
}
