using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Common;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Ebs.Download.PurchaseOrders;
using SIE.Items;
using SIE.Warehouses;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 采购订单下载控制
    /// </summary>
    public class DownloadPoController : DomainController
    {
        /// <summary>
        /// 从API下载采购订单到业务表
        /// </summary>
        /// <param name="poDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadPoToBusiness(List<PoData> poDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<PoData>(
                poDatas,
                p => this.SavePurchaseOrders(p.OrderByLastUpdateDate()),
                JobType.PurchaseOrder,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载采购订单到业务表
        /// </summary>
        public virtual ProcessResult DownloadPoInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<PurchaseOrderInf, PurchaseOrderDetailInf>(
                () => ctl.GetUnprocessedDatas<PurchaseOrderInf>(),             //采购订单中间表数据
                p =>
                {
                    //采购订单明细中间表数据
                    var nos = p.Select(y => y.No).Distinct().ToList();
                    var whereDtl = nos.CreateContainsExpression<PurchaseOrderDetailInf>("x", PurchaseOrderDetailInf.PoNoProperty.Name);
                    var dtlDatas = ctl.GetUnprocessedDatas(whereDtl);

                    return dtlDatas;
                },
                (x, y) =>
                {
                    //构建明细数据嵌套字典
                    var dtlDataDicts = ctl.GenerateDictionarys<string, PurchaseOrderDetailInf>(y, PurchaseOrderDetailInf.PoNoProperty);

                    //调用业务接口
                    var paras = this.GeneratePoPara(x, dtlDataDicts);
                    return this.SavePurchaseOrders(paras.OrderByLastUpdateDate());
                },
                JobType.PurchaseOrder, JobType.PurchaseOrderDetail, isManual);
        }

        /// <summary>
        /// 生成采购订单实体
        /// </summary>
        /// <param name="poInfs">中间表实体数据</param>
        /// <param name="poDtlInfs">中间表明细实体数据</param>
        /// <returns></returns>
        private List<PoData> GeneratePoPara(IEnumerable<PurchaseOrderInf> poInfs, Dictionary<string, List<PurchaseOrderDetailInf>> poDtlInfs)
        {
            var paras = new List<PoData>();
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();
            var dtlCtl = RT.Service.Resolve<DownloadPoDtlController>();

            poInfs.ForEach(p =>
            {
                //构建子列表
                List<PurchaseOrderDetailInf> details;
                if (poDtlInfs.TryGetValue(p.No, out details))
                    poDtlInfs.Remove(p.No);      //由于来源数据集允许重复数据，已取值明细清除，避免重复构建浪费资源
                else
                    details = new List<PurchaseOrderDetailInf>();
                ctl.GenerateChildren(p, details);
                var dtlDatas = dtlCtl.GeneratePoDtlPara(details);

                //构建主数据
                var data = new PoData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.No = p.No;
                ////poData.Contacts = poInf.Contacts;
                ////poData.ContactNumber = poInf.ContactNumber;
                data.WarehouseCode = p.ReceivingWhCode;
                data.SupplierCode = p.SupplierCode;
                //data.OrderType = (int)PoOrderType.Purchase;
                data.ErpKey = p.ErpKey;
                data.DetailList = dtlDatas;       //附加字列表

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
                    RT.Service.Resolve<SoapPoController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadPoInfToBusiness(true);           //执行业务表下载
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
        /// 用于接口中心下载数据保存到SMOM PO表中
        /// </summary>
        /// <param name="datas">采购数据</param>
        /// <returns>错误数据列表</returns>
        public virtual List<ErpErrorData> SavePurchaseOrders(List<PoData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //var ctl = RT.Service.Resolve<PurchaseOrderController>();

            //获取PO
            List<string> noList = datas.Select(p => p.No).Distinct().ToList();
            //var poList = ctl.GetPurchaseOrderDatas(noList);
            //var poDict = poList.ToDictionary(p => p.No, p => p);

            //获取PO明细数据
            //var poDetails = ctl.GetPurchaseOrderDetailData(poList.Select(p => p.Id).ToList());
            //var dicPoDetails = poDetails.GroupBy(p => p.PurchaseOrder.No).ToDictionary(p => p.Key, p => p.ToList());    //<PoNo,Po明细列表>

            //获取供应商数据
            var supplierCodes = datas.Select(p => p.SupplierCode).Distinct().ToList();
            var suppliers = RT.Service.Resolve<SupplierController>().GetSuppliers(supplierCodes);
            var supplierDict = suppliers.ToDictionary(p => p.Code, p => p);

            //获取仓库数据
            var warehouseCodes = datas.Select(p => p.WarehouseCode).Distinct().ToList();
            var warehouses = RT.Service.Resolve<WarehouseController>().GetUserWarehouses(warehouseCodes, string.Empty, null);
            var warehouseDict = warehouses.ToDictionary(p => p.Code, p => p);

            //物料字典数据
            var itemCodes = datas.SelectMany(p => p.DetailList).Select(p => p.ItemCode).ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes.Distinct().ToList(), new EagerLoadOptions().LoadWithViewProperty());
            var dicItem = items.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var data in datas)
            {
                try
                {
                    //SavePurchaseOrder(data, poDict, supplierDict, warehouseDict, dicItem, dicPoDetails);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = data.Infkey });
                }
            }

            return errors;
        }

        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        /// <param name="dicSupplier">数据字典</param>
        /// <param name="dicWarehouse">数据字典</param>
        /// <param name="dicItem">数据字典</param>
        /// <param name="dicDetails">数据字典</param>
        //private void SavePurchaseOrder(PoData data, Dictionary<string, PurchaseOrder> dic, Dictionary<string, Supplier> dicSupplier, Dictionary<string, Warehouse> dicWarehouse, Dictionary<string, Item> dicItem, Dictionary<string, List<PurchaseOrderDetail>> dicDetails)
        //{
        //    var ctl = RT.Service.Resolve<DownloadBusBaseController>();
        //    var dtlCtl = RT.Service.Resolve<DownloadPoDtlController>();

        //    //启用事务，保存主从数据完整性
        //    using (var trans = DB.TransactionScope(POEntityDataProvider.ConnectionStringName))
        //    {
        //        PurchaseOrder po;
        //        var key = data.No;
        //        if (key.IsNullOrEmpty())
        //            throw new ValidationException("采购单号为空".L10nFormat(key));
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
        //            dic.Add(key, new PurchaseOrder());
        //        po = dic[key];

        //        if (data.SupplierCode.IsNullOrEmpty() || !dicSupplier.ContainsKey(data.SupplierCode))
        //            throw new ValidationException("供应商[{0}]不存在".L10nFormat(data.SupplierCode));
        //        if (data.WarehouseCode.IsNotEmpty() && !dicWarehouse.ContainsKey(data.WarehouseCode))
        //        {
        //            throw new ValidationException("仓库[{0}]不存在".L10nFormat(data.WarehouseCode));
        //            po.ReceivingWarehouse = dicWarehouse[data.WarehouseCode];
        //        }
        //        var detailDatas = data.DetailList;

        //        po.No = data.No;
        //        po.OrderType = (PoOrderType)data.OrderType;
        //        po.Supplier = dicSupplier[data.SupplierCode];

        //        po.SourceKey = data.ErpKey;

        //        RF.Save(po);

        //        //处理明细
        //        if (!dicDetails.ContainsKey(key))
        //            dicDetails.Add(key, new List<PurchaseOrderDetail>());
        //        var dicDetail = dicDetails[key].ToDictionary(p => p.LineNo);
        //        int i = 0;
        //        var unitCodes = detailDatas.Select(a => a.UnitCode).Distinct().ToList();
        //        var units = RT.Service.Resolve<ItemController>().GetUnitList(unitCodes).ToDictionary(p => p.Code, p => p.Id);
        //        var unitIds = units.Select(a => a.Value).ToList();
        //        var itemUnits = RT.Service.Resolve<ItemUnitController>().GetItemUnits(unitIds);
        //        foreach (var detail in detailDatas)
        //        {
        //            i++;
        //            dtlCtl.SavePurchaseOrderDetail(detail, dicDetail, dicItem, po, i.ToString(), units, itemUnits);
        //        }
        //        trans.Complete();
        //    }
        //}
    }
}
