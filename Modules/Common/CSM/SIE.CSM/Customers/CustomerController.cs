using SIE.Core.Common;
using SIE.CSM.Suppliers;
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
        /// 获取客户
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<Customer> GetCustomerDatas(CustomerCriteria criteria)
        {
            var query = Query<Customer>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.CustomerType.HasValue)
                query.Where(p => p.CustomerType == criteria.CustomerType.Value);
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State.Value);
            if (criteria.IsCarrier)
                query.Where(p => p.CustomerType != CustomerType.CARRIER);

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过客户名获取客户
        /// </summary>
        /// <param name="name">客户名</param>
        /// <returns>返回对应客户</returns>
        public virtual Customer GetCustomerFromName(string name)
        {
            Customer customer = Query<Customer>().Where(p => p.Name == name).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            return customer;
        }

        /// <summary>
        /// 通过客户编号获取客户
        /// </summary>
        /// <param name="code">客户名</param>
        /// <returns>返回对应客户</returns>
        public virtual Customer GetCustomerFromCode(string code)
        {
            Customer customer = Query<Customer>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            return customer;
        }

        /// <summary>
        /// 检查客户类型+供应商是否唯一
        /// </summary>
        /// <param name="cusType">客户类型</param>
        /// <param name="supplierId">供应商</param>
        /// <returns>bool</returns>
        public virtual bool CheckExistTypeAndSupplier(CustomerType cusType, double supplierId)
        {
            return Query<Customer>().Where(p => p.CustomerType == cusType && p.SupplierId == supplierId && p.State == State.Enable).Count() > 1;
        }

        /// <summary>
        /// 获取客户
        /// </summary>
        /// <param name="str">客户名</param>
        /// <returns>返回对应客户</returns>
        public virtual Customer GetCustomer(string str)
        {
            Customer customer = Query<Customer>().Where(p => p.Code == str || p.Name == str).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            return customer;
        }

        /// <summary>
        /// 根据客户编码集合获取客户集合
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<Customer> GetCustomerByCodes(List<string> codes)
        {
            return Query<Customer>().Where(p =>codes.Contains(p.Code)).ToList();
        }

        /// <summary>
        /// 根据客户类型获取客户数据
        /// </summary>
        /// <param name="customerType">客户类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<Customer> GetCustomer(CustomerType customerType, string keyword, PagingInfo pagingInfo)
        {
            return Query<Customer>().Where(p => p.CustomerType == customerType && p.State == State.Enable).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="code">编码</param>
        /// <returns>客户信息</returns>
        public virtual Customer GetCustomer(CustomerType type, string code)
        {
            var query = Query<Customer>();
            query.Where(p => p.CustomerType == type && p.Code == code);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过客户Id列表 获取客户列表
        /// </summary>
        /// <param name="ids">客户id列表</param>
        /// <returns>客户列表</returns>
        public virtual EntityList<Customer> GetCustomerList(List<double> ids)
        {
            return ids.SplitContains((tmpIds) =>
            {
                return Query<Customer>().Where(p => tmpIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取可用的客户列表
        /// </summary>     
        /// <returns>可用的客户列表</returns>
        public virtual Customer GetEnableCustomer(string code)
        {
            var query = Query<Customer>();
            query.Where(p => p.State == State.Enable && (p.CustomerType == CustomerType.CUSTOMER || p.CustomerType == CustomerType.SHIPPER) && (p.Code.Contains(code) || p.Name.Contains(code)));

            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可用的客户列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>        
        /// <returns>可用的客户列表</returns>
        public virtual EntityList<Customer> GetEnableCustomers(
            PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Customer>();
            query.Where(p => p.State == State.Enable && p.CustomerType == CustomerType.CUSTOMER);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可用的客户列表
        /// </summary>     
        /// <returns>可用的客户列表</returns>
        public virtual EntityList<Customer> GetEnableCustomers(string code)
        {
            var query = Query<Customer>();
            query.Where(p => p.State == State.Enable && (p.CustomerType == CustomerType.CUSTOMER || p.CustomerType == CustomerType.SHIPPER) && p.SupplierId != null && (p.Code.Contains(code) || p.Name.Contains(code)));

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可用的客户列表
        /// </summary>
        /// <param name="isCarrier">是否过滤</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>        
        /// <returns>可用的客户列表</returns>
        public virtual EntityList<Customer> GetEnableCustomerDatas(PagingInfo pagingInfo, string keyword, bool isCarrier = false)
        {
            var query = Query<Customer>();
            query.Where(p => p.State == State.Enable);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (isCarrier)
                query.Where(p => p.CustomerType == CustomerType.CARRIER);
            else
                query.Where(p => p.CustomerType == CustomerType.CUSTOMER || p.CustomerType == CustomerType.SHIPPER);

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可用的客户行数
        /// </summary>     
        /// <returns>可用的客户列表</returns>
        public virtual int GetEnableCustomerCount(string code)
        {
            var query = Query<Customer>();
            query.Where(p => p.State == State.Enable && (p.CustomerType == CustomerType.CUSTOMER || p.CustomerType == CustomerType.SHIPPER) && p.SupplierId != null && (p.Code.Contains(code) || p.Name.Contains(code)));

            return query.Count();
        }

        /// <summary>
        /// 根据客户类型获取客户数据
        /// </summary>
        /// <param name="customerType">客户类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        /// <remarks>筛选如果货主有关联供应商，供应商必须是可用</remarks>
        public virtual EntityList<Customer> GetCustomerWithSupplier(CustomerType customerType, string keyword, PagingInfo pagingInfo)
        {
            return Query<Customer>().LeftJoin<Supplier>((x, y) => x.SupplierId == y.Id)
                .Where<Supplier>((x, y) => x.SupplierId != null || y.State == State.Enable)
                .Where(p => p.CustomerType == customerType && p.State == State.Enable)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据客户类型获取客户数据
        /// </summary>
        /// <param name="customerType">客户类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        /// <remarks>筛选如果货主有关联供应商，供应商必须是可用</remarks>
        public virtual EntityList<Customer> GetCustomerHasSupplier(CustomerType customerType, string keyword, PagingInfo pagingInfo)
        {
            return Query<Customer>().Join<Supplier>((x, y) => x.SupplierId == y.Id && y.State == State.Enable)
                .Where(p => p.CustomerType == customerType && p.State == State.Enable)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 设置指定客户指定地址为默认地址
        /// </summary>
        /// <param name="customerId">指定客户Id</param>
        /// <param name="customerAddressId">指定地址Id</param>
        public virtual void SetDefaultAddress(double customerId, double customerAddressId)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(Customer.CustomerAddressListProperty);
            elo.LoadWithViewProperty();
            Customer customer = Query<Customer>().Where(p => p.Id == customerId).FirstOrDefault(elo);
            CustomerAddress customerAddress = customer.CustomerAddressList.FirstOrDefault(p => p.Id == customerAddressId);

            if (customerAddress == null)
                throw new ValidationException("当前地址已经被删除!".L10N());
            if (customerAddress.IsDefault)
                throw new ValidationException("当前地址已经是默认地址".L10N());

            customer.CustomerAddressList.Where(p => p.IsDefault).ForEach(p => p.IsDefault = false);
            customerAddress.IsDefault = true;
            RF.Save(customer);
        }

        /// <summary>
        /// 是否可以设置默认地址
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>返回是否可以设置默认地址</returns>
        public virtual bool CanSetDefaultAddress(double customerId)
        {
            return Query<CustomerAddress>().Where(p => p.CustomerId == customerId).Count() == 0;
        }

        /// <summary>
        /// 获取客户默认地址
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns>返回客户默认地址</returns>
        public virtual CustomerAddress GetDefaultAddress(double customerId)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return Query<CustomerAddress>().Where(p => p.CustomerId == customerId && p.IsDefault && p.State == State.Enable).FirstOrDefault(elo);
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="supplieId">供应商Id</param>
        /// <returns>客户信息</returns>
        public virtual Customer GetCustomerData(CustomerType type, double? supplieId)
        {
            var query = Query<Customer>();
            query.Where(p => p.CustomerType == type);

            if (supplieId.HasValue)
                query.Where(p => p.SupplierId == supplieId);

            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据供应商获取客户条数
        /// </summary>
        /// <param name="customerId">排除当前客户</param>
        /// <param name="supplieId">供应商</param>
        /// <returns>客户条数</returns>
        public virtual int GetCustoerCount(double customerId, double? supplieId)
        {
            var query = Query<Customer>().Where(p => p.Id != customerId);
            if (supplieId.HasValue)
                query.Where(p => p.SupplierId == supplieId);
            return query.Count();
        }

        /// <summary>
        /// 获取客户By 编码集合
        /// </summary>
        /// <param name="codes">编码</param>
        /// <param name="type">类型</param>
        /// <returns>客户集合</returns>
        public virtual EntityList<Customer> GetCustomers(List<string> codes, CustomerType? type = null)
        {
            EntityList<Customer> customers = new EntityList<Customer>();
            DataProcessEx.SplitDataExecute(codes, sons =>
            {
                var query = Query<Customer>();
                if (type.HasValue)
                {
                    query.Where(p => p.CustomerType == type.Value);
                }
                var customersList = query.Where(p => sons.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                customers.AddRange(customersList);
            });
            return customers;
        }

        /// <summary>
        /// 获取客户By 编码集合
        /// </summary>
        /// <param name="suppliers">供应商ID集合</param>
        /// <param name="type">客户类型</param>
        /// <param name="elo">是否贪婪加载</param>
        /// <returns>客户集合</returns>
        public virtual EntityList<Customer> GetCustomers(List<double> suppliers, CustomerType? type = null, EagerLoadOptions elo = null)
        {
            EntityList<Customer> customers = new EntityList<Customer>();
            DataProcessEx.SplitDataExecute(suppliers, sons =>
            {
                var query = Query<Customer>();
                if (type.HasValue)
                {
                    query.Where(p => p.CustomerType == type.Value);
                }
                var customersList = query.Where(p => sons.Contains((double)p.SupplierId)).ToList(null, elo);
                customers.AddRange(customersList);
            });
            return customers;
        }

        /// <summary>
        /// 获取客户By 编码集合
        /// </summary>
        /// <param name="suppliers">供应商ID集合</param>
        /// <param name="type">客户类型</param>
        /// <param name="elo">是否贪婪加载</param>
        /// <returns>客户集合</returns>
        public virtual EntityList<Customer> GetCustomersBySupplierIds(List<double> suppliers, CustomerType? type = null, EagerLoadOptions elo = null)
        {
            EntityList<Customer> customers = new EntityList<Customer>();
            List<double?> data = new List<double?>();
            suppliers.ForEach(p =>
            {
                data.Add(p);
            });
            DataProcessEx.SplitDataExecute(data, sons =>
            {
                var query = Query<Customer>();
                if (type.HasValue)
                {
                    query.Where(p => p.CustomerType == type.Value);
                }
                var customersList = query.Where(p => sons.Contains(p.SupplierId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                customers.AddRange(customersList);
            });
            return customers;
        }

        /// <summary>
        /// 根据名称或编码获取客户列表
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Customer> GetCustomers(PagingInfo paging,string keyword) {
            var q = Query<Customer>();
            if (keyword.IsNotEmpty()) {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return q.ToList(paging, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取客户By 编码集合
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>客户集合</returns>
        public virtual EntityList<Customer> GetCustomerTypeCustomers(List<string> codes)
        {             
            EntityList<Customer> customers = new EntityList<Customer>();
            DataProcessEx.SplitDataExecute(codes, sons =>
            {
                var query = Query<Customer>().Where(p => p.CustomerType == CustomerType.CUSTOMER || p.CustomerType == CustomerType.SHIPPER);
                var customersList = query.Where(p => sons.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                customers.AddRange(customersList);
            });
            return customers;
        }

        /// <summary>
        /// 获取客户地址By 详细地址集合
        /// </summary>
        /// <param name="codes">详细地址</param>
        /// <returns>客户地址集合</returns>
        public virtual EntityList<CustomerAddress> GetCustomerAddress(List<string> codes)
        {             
            EntityList<CustomerAddress> customerAddress = new EntityList<CustomerAddress>();
            DataProcessEx.SplitDataExecute(codes, sons =>
            {

                var customerAddressList = Query<CustomerAddress>().Where(p => sons.Contains(p.Address)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                customerAddress.AddRange(customerAddressList);
            });
            return customerAddress;
        }


        /// <summary>
        /// 通过客户编码列表 获取客户列表(忽略库存组织）
        /// </summary>
        /// <param name="codes">客户编码列表</param>
        /// <returns>客户列表</returns>
        public virtual EntityList<Customer> GetCustomerInvOrgList(List<string> codes)
        {
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                return codes.SplitContains((tmpCodes) =>
                {
                    return Query<Customer>().Where(p => tmpCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });

            }
        }

        /// <summary>
        /// 通过客户编号或者客户名称获取客户信息
        /// </summary>
        /// <param name="codeOrNames">客户编号或者客户名称</param>
        /// <returns>返回客户信息</returns>
        public virtual EntityList<Customer> GetCustomersbyCodeOrName(List<string> codeOrNames)
        {
            return codeOrNames.SplitContains(tmps =>
           {
               return Query<Customer>().Where(p => codeOrNames.Contains(p.Code) || codeOrNames.Contains(p.Name)).ToList();
           });
        }

        /// <summary>
        /// 获取货主信息
        /// </summary>
        /// <param name="supplieId">供应商Id</param>
        /// <returns>客户信息</returns>
        public virtual Customer GetCustomerDataBySupplieId(double? supplieId)
        {
            var query = Query<Customer>().Where(p => p.CustomerType != CustomerType.CARRIER && p.State == State.Enable);
            if (supplieId.HasValue)
                query.Where(p => p.SupplierId == supplieId);

            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="supplieId">供应商Id</param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns>客户信息</returns>
        public virtual EntityList<Customer> GetCustomersBySupplieId(CustomerType type, double supplieId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Customer>();
            query.Where(p => p.CustomerType == type && p.SupplierId == supplieId);

            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取客户id和名称字典
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        public virtual Dictionary<double,string> GetCustomerIdAndNameDic(List<double> customerIds)
        {
            List<SimpleCustomerViewModel> models = new List<SimpleCustomerViewModel>();
            customerIds.SplitDataExecute(ids =>
            {
                var tmpModel = Query<Customer>().Where(p => ids.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList<SimpleCustomerViewModel>().ToList();
                models.AddRange(tmpModel);
            });
            return models.GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.First().Name);
        }
    }
}
