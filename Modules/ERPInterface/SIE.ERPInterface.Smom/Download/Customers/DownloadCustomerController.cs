using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Download.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 客户下载控制器
    /// </summary>
    public class DownloadCustomerController : DomainController
    {
        /// <summary>
        /// 从API下载客户到业务表
        /// </summary>
        /// <param name="customerDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadCustomerToBusiness(List<CustomerData> customerDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<CustomerData>(
                customerDatas,
                p => this.SaveCustomers(p.OrderByLastUpdateDate()),
                JobType.Customer,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载客户到业务表
        /// </summary>
        public virtual ProcessResult DownloadCustomerInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<CustomerInf>(
                () => ctl.GetUnprocessedDatas<CustomerInf>(),       //客户中间表数据E
                p =>
                {
                    //执行业务接口
                    var paras = this.GenerateCustomerPara(p);
                    return this.SaveCustomers(paras.OrderByLastUpdateDate());
                },
                JobType.Customer, isManual);
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
                    RT.Service.Resolve<SoapCustomerController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadCustomerInfToBusiness(true);           //执行业务表下载
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
        /// 生成客户实体
        /// </summary>
        /// <param name="customerInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<CustomerData> GenerateCustomerPara(IEnumerable<CustomerInf> customerInfs)
        {
            var paras = new List<CustomerData>();

            customerInfs.ForEach(p =>
            {
                var data = new CustomerData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.ShortName = p.ShortName;
                data.SalesArea = p.Region;
                data.EnglishName = p.EnglishName;
                data.Description = p.Description;
                data.CustomerType = (int)p.CustomerType;
                data.DutyParagraph = p.DutyParagraph;
                data.Contacts = p.Contacts;
                data.ContactNumber = p.ContactsNumber;
                data.ContactAddress = p.ContactsAddress;
                data.Email = p.EMail;
                data.PostalCode = p.ZipCode;
                data.Remark = p.Remark;
                data.OwnCode = p.OwnCode;
                data.OwnName = p.OwnName;
                data.SupplierCode = "";
                data.State = 1;
                data.ErpKey = p.ErpKey;
                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 保存数据到客户
        /// </summary>
        /// <param name="datas"></param>
        private List<ErpErrorData> SaveCustomers(List<CustomerData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //获取客户数据
            var codeList = datas.Select(p => p.Code).Distinct().ToList();
            var customers = RT.Service.Resolve<CustomerController>().GetCustomers(codeList);
            var customerDic = customers.ToDictionary(p => p.Code);

            //获取供应商数据
            var supplierCodeList = datas.Where(p => !string.IsNullOrWhiteSpace(p.SupplierCode)).Select(p => p.SupplierCode).Distinct().ToList();
            var suppliers = RT.Service.Resolve<SupplierController>().GetSuppliers(supplierCodeList);
            var supplierDic = suppliers.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveCustomer(p, customerDic, supplierDic, "");
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
        /// <param name="dic">客户数据字典</param>
        /// <param name="dicSupplier">供应商数据字典</param>
        /// <param name="customerType">类型</param>
        private void SaveCustomer(CustomerData data, Dictionary<string, Customer> dic, Dictionary<string, Supplier> dicSupplier, string customerType)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            Customer entity = new Customer();
            var key = data.Code;
            if (key.IsNullOrEmpty())
                throw new ValidationException("客户编码为空".L10nFormat(key));
            //处理待删除数据
            if (dic.ContainsKey(key))
            {
                if (data.IsDelete)
                {
                    ctl.DeleteEntity(dic, key, dic[key]);
                }
                return;
            }
            if (!dic.ContainsKey(key))
                dic.Add(key, new Customer());

            if (data.CustomerType == 1 && !dicSupplier.ContainsKey(data.SupplierCode))
            {
                throw new ValidationException("供应商{0}不存在".L10nFormat(data.SupplierCode));
            }

            entity.Code = data.Code;
            entity.Name = data.Name;
            entity.Description = data.Description;
            entity.ShortName = data.ShortName;
            entity.EnglishName = data.EnglishName;
            entity.Region = data.SalesArea;
            entity.CustomerType = (CustomerType)data.CustomerType;
            entity.State = data.State == 0 ? State.Disable : State.Enable;
            entity.OwnCode = data.OwnCode;
            entity.OwnName = data.OwnName;
            entity.Supplier = dicSupplier[data.SupplierCode];
            entity.DutyParagraph = data.DutyParagraph;
            entity.Contacts = data.Contacts;
            entity.ContactsNumber = data.ContactNumber;
            entity.ContactsAddress = data.ContactAddress;
            entity.EMail = data.Email;
            entity.ZipCode = data.PostalCode;
            entity.Remark = data.Remark;
            entity.SourceKey = data.ErpKey;

            RF.Save(entity);
        }
    }
}
