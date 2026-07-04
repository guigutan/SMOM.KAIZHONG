using SIE.CSM.Customers;
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
    /// 客户地址下载控制器
    /// </summary>
    public class DownloadCustomerAddrController : DomainController
    {
        /// <summary>
        /// 从API下载客户地址到业务表
        /// </summary>
        /// <param name="customerAddrDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadCustomerAddrToBusiness(List<CustomerAddressData> customerAddrDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<CustomerAddressData>(
                customerAddrDatas,
                p => this.SaveCustomerAddress(p.OrderByLastUpdateDate()),
                JobType.CustomerAddress,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载客户地址到业务表
        /// </summary>
        public virtual ProcessResult DownloadCustomerAddrInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<CustomerAddressInf>(
                () => ctl.GetUnprocessedDatas<CustomerAddressInf>(),       //客户地址中间表数据
                p =>
                {
                    var paras = this.GenerateCustomerAddrPara(p);
                    return this.SaveCustomerAddress(paras.OrderByLastUpdateDate());
                },
                JobType.CustomerAddress, isManual);
        }

        /// <summary>
        /// 生成客户地址实体
        /// </summary>
        /// <param name="customerAddrInfs">中间表实体地址数据</param>
        /// <returns></returns>
        private List<CustomerAddressData> GenerateCustomerAddrPara(IEnumerable<CustomerAddressInf> customerAddrInfs)
        {
            var paras = new List<CustomerAddressData>();

            customerAddrInfs.ForEach(p =>
            {
                var data = new CustomerAddressData();
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
                data.Address = p.Address;
                data.Email = p.Email;
                data.Remark = p.Remark;
                data.CustomerCode = p.CustomerCode;
                data.AddressType = p.AddressType;
                data.State = 1;
                data.ErpKey = p.ErpKey;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 保存数据到客户地址
        /// </summary>
        /// <param name="datas"></param>
        public virtual List<ErpErrorData> SaveCustomerAddress(List<CustomerAddressData> datas)
        {
            var errors = new List<ErpErrorData>();

            var codeList = datas.Where(p => !string.IsNullOrWhiteSpace(p.CustomerCode)).Select(p => p.CustomerCode).Distinct().ToList();
            if (codeList.Count == 0) return errors;
            var customers = RT.Service.Resolve<CustomerController>().GetCustomers(codeList);
            if (customers.Count == 0) return errors;
            var customerDic = customers.ToDictionary(p => p.Code);

            var addressList = datas.Select(p => p.Address).Distinct().ToList();
            var addresses = RT.Service.Resolve<CustomerController>().GetCustomerAddress(addressList);
            //客户地址字典 （客户编码+地址为主键）
            var dicAddress = addresses.ToDictionary(p => "{0}_{1}".FormatArgs(p.Customer.Code, p.Address));

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveCustomerAddress(p, dicAddress, customerDic, "");
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
        /// <param name="dic">客户地址数据字典</param>
        /// <param name="dicCustomer">客户数据字典</param>
        /// <param name="addressType">客户地址类型</param>
        private void SaveCustomerAddress(CustomerAddressData data, Dictionary<string, CustomerAddress> dic, Dictionary<string, Customer> dicCustomer, string addressType)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            CustomerAddress entity;
            var key = "{0}_{1}".FormatArgs(data.CustomerCode, data.Address);   //客户编码+地址为主键
            if (!dic.ContainsKey(key))
                dic.Add(key, new CustomerAddress());
            entity = dic[key];
            //处理待删除数据
            if (data.IsDelete)
            {
                ctl.DeleteEntity(dic, key, entity);
                return;
            }
            if (data.CustomerCode.IsNullOrEmpty() || !dicCustomer.ContainsKey(data.CustomerCode))
                throw new ValidationException("客户[{0}]不存在".L10nFormat(data.CustomerCode));

            entity.Name = data.Name;
            entity.Country = data.Country;
            entity.Province = data.Province;
            entity.City = data.City;
            entity.Area = data.Area;
            entity.Address = data.Address;
            entity.Contacts = data.Contacts;
            entity.Phone = data.Phone;
            entity.Fax = data.Fax;
            entity.Email = data.Email;
            entity.ZipCode = data.ZipCode;
            entity.State = (State)data.State;
            entity.Remark = data.Remark;
            entity.Customer = dicCustomer[data.CustomerCode];
            entity.AddressType = data.AddressType;
            entity.SourceKey = data.ErpKey;

            RF.Save(entity);
        }

    }
}
