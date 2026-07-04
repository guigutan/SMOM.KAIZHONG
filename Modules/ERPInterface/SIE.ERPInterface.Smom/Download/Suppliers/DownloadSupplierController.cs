using SIE.Common.Catalogs;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Ebs.Download.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 供应商下载控制器
    /// </summary>
    public class DownloadSupplierController : DomainController
    {
        /// <summary>
        /// 从API下载供应商到业务表
        /// </summary>
        /// <param name="supplierDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadSupplierToBusiness(List<SupplierData> supplierDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<SupplierData>(
                supplierDatas,
                p => this.SaveSuppliers(p.OrderByLastUpdateDate()),
                JobType.Supplier,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载供应商到业务表
        /// </summary>
        public virtual ProcessResult DownloadSupplierInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<SupplierInf>(
                () => ctl.GetUnprocessedDatas<SupplierInf>(),
                p =>
                {
                    var paras = this.GenerateSupplierPara(p);
                    return this.SaveSuppliers(paras.OrderByLastUpdateDate());
                },
                JobType.Supplier, isManual);
        }

        /// <summary>
        /// 生成供应商实体
        /// </summary>
        /// <param name="supplierInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<SupplierData> GenerateSupplierPara(IEnumerable<SupplierInf> supplierInfs)
        {
            var paras = new List<SupplierData>();

            supplierInfs.ForEach(p =>
            {
                var data = new SupplierData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.Description = p.Description;
                data.ShortName = p.ShortName;
                data.SalesArea = p.Region;
                data.EnglishName = p.EnglishName;
                data.DutyParagraph = p.DutyParagraph;
                data.Contacts = p.Contacts;
                data.ContactNumber = p.ContactNumber;
                data.ContactAddress = p.ContactAddress;
                data.Email = p.Email;
                data.PostalCode = p.ZipCode;
                data.Remark = p.Remark;
                data.State = 1;
                data.ErpKey = p.ErpKey;

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
                    RT.Service.Resolve<SoapSupplierController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadSupplierInfToBusiness();           //执行业务表下载
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
        /// 保存数据到供应商
        /// </summary>
        /// <param name="datas">ERP接口数据</param>
        /// <returns>错误信息</returns>
        public virtual List<ErpErrorData> SaveSuppliers(List<SupplierData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            var ctl = RT.Service.Resolve<SupplierController>();

            //获取供应商数据
            var suppliers = ctl.GetSuppliers(datas.Select(p => p.Code).ToList());//已存在的数据
            var suppliersDic = suppliers.ToDictionary(p => p.Code);

            //供应商类型快码
            var supplierType = string.Empty;
            var catalog = RT.Service.Resolve<CatalogController>().GetCatalogList(Supplier.SupperType);
            if (catalog.Count == 0)
                throw new ValidationException("供应商类型快码[SUPPLIER_TYPE]未维护相关数据!".L10N());
            supplierType = catalog.FirstOrDefault().Code;

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveSupplier(p, suppliersDic, supplierType);
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
        /// <param name="dic">数据字典</param>
        /// <param name="supplierType">供应商类型</param>
        private void SaveSupplier(SupplierData data, Dictionary<string, Supplier> dic, string supplierType)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();
            Supplier entity;
            var key = data.Code;
            if (key.IsNullOrEmpty())
                throw new ValidationException("供应商编码为空".L10nFormat(key));
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
                dic.Add(key, new Supplier());
            entity = dic[key];

            entity.Code = data.Code;
            entity.Name = data.Name;
            entity.Description = data.Description;
            entity.ShortName = data.ShortName;
            entity.EnglishName = data.EnglishName;
            entity.Region = data.SalesArea;
            entity.DutyParagraph = data.DutyParagraph;
            entity.Contacts = data.Contacts;
            entity.ContactNumber = data.ContactNumber;
            entity.ContactAddress = data.ContactAddress;
            entity.EMail = data.Email;
            entity.State = data.State == 1 ? State.Enable : State.Disable;
            entity.ZipCode = data.PostalCode;
            entity.Remark = data.Remark;
            entity.Type = supplierType;
            entity.SourceKey = data.ErpKey;

            RF.Save(entity);
        }

    }
}
