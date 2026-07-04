using SIE.Api;
using SIE.Core.ApiModels;
using SIE.CSM.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 供应商控制器
    /// </summary>
    public partial class SupplierController
    {
        /// <summary>
        /// 获取所有供应商的信息
        /// </summary>       
        /// <returns>供应商集合</returns>
        [ApiService("获取所有供应商的信息")]
        [return: ApiReturn("返回供应商集合：List<BaseData>")]
        public virtual List<BaseData> GetAllSupplier()
        {
            List<BaseData> result = new List<BaseData>();
            EntityList<Supplier> supplierList = GetSupplierList();
            supplierList.Where(p => p.State == State.Enable).ForEach(p =>
            {
                BaseData data = new BaseData();
                data.Id = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                result.Add(data);
            });

            return result;
        }

        /// <summary>
        /// 获取供应商信息
        /// </summary>
        /// <param name="code">供应商编码</param>
        /// <returns>供应商信息</returns>
        [ApiService("获取供应商信息")]
        [return: ApiReturn("供应商信息 BaseDataInfo")]
        public virtual BaseDataInfo GetSupplierInfo([ApiParameter("供应商编码")] string code)
        {
            if (code.IsNullOrEmpty())
                return null;
            string supplierCode = code.Trim();
            if (supplierCode.IsNullOrEmpty())
                throw new ValidationException("供应商不能为空".L10N());
            var supplier = GetSupplier(supplierCode);
            if (supplier == null)
                throw new ValidationException("供应商编码[{0}]不存在，请确认供应商的有效性！".L10nFormat(supplierCode));
            return new BaseDataInfo()
            {
                Id = supplier.Id,
                Code = supplier.Code,
                Name = supplier.Name
            };
        }

        /// <summary>
        /// 获取供应商信息列表
        /// </summary>
        /// <param name="queryInfo">查询</param>
        /// <returns>供应商信息</returns>
        [ApiService("获取供应商信息列表")]
        [return: ApiReturn("供应商信息列表 BaseDataInfo")]
        public virtual List<BaseDataInfo> GetSupplierInfos([ApiParameter("供应商分页参数")] PagingKeywordQueryInfo queryInfo)
        {
            if (queryInfo == null || queryInfo.PageSize <= 0)
                throw new ValidationException("[每页数据量]必须大于0".L10N());
            if (queryInfo.PageNumber <= 0)
                throw new ValidationException("[页码]必须大于0".L10N());

            //构建分页实体
            var pageInfo = new PagingInfo()
            {
                PageSize = queryInfo.PageSize.Value,
                PageNumber = queryInfo.PageNumber.Value,
                IsNeedCount = true
            };

            //查询供应商数据
            var suppliers = this.GetSuppliers(pageInfo, queryInfo.Keyword);

            //构建返回信息
            var infos = new List<BaseDataInfo>();
            suppliers.ForEach(p =>
            {
                infos.Add(new BaseDataInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name
                });
            });

            return infos;
        }
    }
}