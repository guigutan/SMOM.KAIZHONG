using SIE.Common;
using SIE.Common.Catalogs;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 供应商地址下载控制器
    /// </summary>
    public class DownloadSupplierAddrController : DomainController
    {
        /// <summary>
        /// 从API下载供应商地址到业务表
        /// </summary>
        /// <param name="supplierAddrDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadSupplierAddrToBusiness(List<SupplierAddressData> supplierAddrDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<SupplierAddressData>(
                supplierAddrDatas,
                p => this.SaveSupplierAddress(p.OrderByLastUpdateDate()),
                JobType.SupplierAddress,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载供应商地址到业务表
        /// </summary>
        public virtual ProcessResult DownloadSupplierAddrInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<SupplierAddressInf>(
                () => ctl.GetUnprocessedDatas<SupplierAddressInf>(),       //供应商地址中间表数据
                p =>
                {
                    var paras = this.GenerateSupplierAddrPara(p);
                    return this.SaveSupplierAddress(paras.OrderByLastUpdateDate());
                },
                JobType.SupplierAddress, isManual);
        }

        /// <summary>
        /// 生成供应商地址实体
        /// </summary>
        /// <param name="supplierAddrInfs">中间表实体地址数据</param>
        /// <returns></returns>
        private List<SupplierAddressData> GenerateSupplierAddrPara(IEnumerable<SupplierAddressInf> supplierAddrInfs)
        {
            var paras = new List<SupplierAddressData>();

            supplierAddrInfs.ForEach(p =>
            {
                var data = new SupplierAddressData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Name = p.Name;
                data.Contacts = p.Contacts;
                data.Phone = p.Phone;
                data.Fax = p.Fax;
                data.ZipCode = p.ZipCode;
                data.Country = p.Country;
                data.Province = p.Province;
                data.City = p.City;
                data.Area = p.Area;
                data.Address = p.DetailAddress;
                data.Email = p.Email;
                data.Remark = p.Remark;
                data.SupplierCode = p.SupplierCode;
                data.State = 1;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 保存数据到供应商地址
        /// </summary>
        /// <param name="datas">供应商地址信息</param>
        public virtual List<ErpErrorData> SaveSupplierAddress(List<SupplierAddressData> datas)
        {
            var errors = new List<ErpErrorData>();

            if (datas.Count == 0)
                return errors;

            var catalog = RT.Service.Resolve<CatalogController>().GetCatalogList(SupplierAddress.CatalogAddressType);
            if (catalog.Count == 0)
                throw new ValidationException("供应商地址类型快码[{0}]未维护相关数据!".L10nFormat(SupplierAddress.CatalogAddressType));
            var addressType = catalog.FirstOrDefault().Code;

            //获取供应商地址数据
            var supplierCodes = datas.Select(p => p.SupplierCode).Distinct().ToList();

            var addresses = RT.Service.Resolve<SupplierController>().GetSupplierAddress(supplierCodes);//供应商地址集合
            //供应商字典
            var dicSupplier = addresses.Select(p => p.Supplier).DistinctBy(p => p.Code).ToDictionary(p => p.Code);
            //供应商地址字典 （供应商编码+地址为主键）
            var dicAddress = addresses.ToDictionary(p => "{0}_{1}".FormatArgs(p.Supplier.Code, p.Address));

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveSupplierAddress(p, dicAddress, dicSupplier, addressType);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.Infkey });
                }
            }

            return errors;
        }

        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">供应商地址数据字典</param>
        /// <param name="dicSupplier">供应商数据字典</param>
        /// <param name="addressType">供应商地址类型</param>
        private void SaveSupplierAddress(SupplierAddressData data, Dictionary<string, SupplierAddress> dic, Dictionary<string, Supplier> dicSupplier, string addressType)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            SupplierAddress entity;
            var key = "{0}_{1}".FormatArgs(data.SupplierCode, data.Address);   //供应商编码+地址为主键
            if (!dic.ContainsKey(key))
                dic.Add(key, new SupplierAddress());
            entity = dic[key];
            //处理待删除数据
            if (data.IsDelete)
            {
                ctl.DeleteEntity(dic, key, entity);
                return;
            }
            if (data.SupplierCode.IsNullOrEmpty() || !dicSupplier.ContainsKey(data.SupplierCode))
                throw new ValidationException("供应商[{0}]不存在".L10nFormat(data.SupplierCode));

            entity.Name = data.Name;
            entity.Country = data.Country;
            entity.Province = data.Province;
            entity.City = data.City;
            entity.Area = data.Area;
            entity.Address = data.Address;
            entity.Contacts = data.Contacts;
            entity.Phone = data.Phone;
            entity.Fax = data.Fax;
            entity.EMail = data.Email;
            entity.ZipCode = data.ZipCode;
            entity.State = data.State == 0 ? State.Disable : State.Enable;
            entity.Remark = data.Remark;
            entity.Supplier = dicSupplier[data.SupplierCode];
            entity.AddressType = addressType;
            entity.SourceKey = data.ErpKey;

            RF.Save(entity);
        }
    }
}
