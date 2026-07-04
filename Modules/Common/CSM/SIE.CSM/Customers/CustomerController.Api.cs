using SIE.Api;
using SIE.CSM.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.CSM.Customers
{
    /// <summary>
    /// 客户控制器
    /// </summary>
    public partial class CustomerController : DomainController
    {
        /// <summary>
        /// 获取所有货主的信息
        /// </summary>
        /// <returns>货主集合</returns>
        [ApiService("获取所有货主的信息")]
        [return: ApiReturn("返回货主集合：List<BaseData>")]
        public virtual List<BaseData> GetAllShipper()
        {
            List<BaseData> result = new List<BaseData>();
            EntityList<Customer> customerList = GetCustomer(CustomerType.SHIPPER, string.Empty, null);
            customerList.ForEach(p =>
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
        /// 验证货主的信息
        /// </summary>
        /// <param name="code">货主</param>
        [ApiService("验证货主是否可用")]
        public virtual void CheckCustomerData([ApiParameter("货主编码")] string code)
        {
            if (code.IsNotEmpty() && code != "*")
            {
                var count = GetEnableCustomerCount(code);
                if (count <= 0)
                    throw new ValidationException("货主:[{0}]不可用".L10nFormat(code));
            }
        }

        /// <summary>
        /// 获取可用的客户列表
        /// </summary>     
        /// <param name="code">货主编码</param>
        /// <param name="type">类型</param>
        /// <returns>可用的客户列表</returns>
        [ApiService("获取货主和客户数据")]
        public virtual Customer GetCustomerDatas([ApiParameter("货主编码")] string code, [ApiParameter("客户类型")] int type)
        {
            var query = Query<Customer>();
            //获取供应商有值的数据
            query.Where(p => p.State == State.Enable && p.CustomerType == (CustomerType)type && p.SupplierId != null && (p.Code.Contains(code) || p.Name.Contains(code)));

            Customer customer = query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            if (customer == null)
                throw new ValidationException("货主或客户:[{0}]不存在或不可用".L10nFormat(code));

            return customer;
        }

        /// <summary>
        /// 获取可用的客户列表
        /// </summary>     
        /// <param name="code">货主编码</param>
        /// <returns>可用的客户列表</returns>
        [ApiService("获取货主和客户数据")]
        public virtual List<BaseData> GetStroerDatas([ApiParameter("货主编码")] string code)
        {
            if (!string.IsNullOrEmpty(code) && !code.EndsWith("%"))
                code = code + "%";
            var query = Query<Customer>();
            query.Where(p => p.State == State.Enable && (p.CustomerType == CustomerType.CUSTOMER || p.CustomerType == CustomerType.SHIPPER) && (p.Code.Contains(code) || p.Name.Contains(code)));
            //供应商类型是客户或货主
            var customers = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (customers.Count == 0)
                throw new ValidationException("货主或客户:[{0}]不存在或不可用".L10nFormat(code.Replace("%", "")));
            List<BaseData> rst = new List<BaseData>();
            customers.ForEach(p =>
            {
                BaseData baseData = new BaseData() { Code = p.Code, Name = p.Name, Id = p.Id };
                rst.Add(baseData);
            });

            return rst;
        }
    }
}
