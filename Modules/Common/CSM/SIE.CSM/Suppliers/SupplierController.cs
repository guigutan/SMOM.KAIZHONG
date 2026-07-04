using SIE.Common.Configs;
using SIE.Common.Sender;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.CSM.Suppliers.Configs;
using SIE.CSM.Suppliers.Datas;
using SIE.CSM.Suppliers.ViewModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 供应商控制器
    /// </summary>
    public partial class SupplierController : DomainController
    {
        /// <summary>
        /// 根据物料查找对应的供应商
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>供应商列表</returns>
        public virtual EntityList<Supplier> GetSuppliers(PagingInfo pagingInfo, string keyword, double? itemId)
        {
            var q = Query<Supplier>();
            if (itemId != null)
                q.Join<SupplierItem>((x, y) => x.Id == y.SupplierId && y.ItemId == itemId);
            if (keyword.IsNotEmpty())
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return q.Where(p => p.State == State.Enable).ToList(pagingInfo);
        }

        /// <summary>
        /// 查询供应商基础信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetSuppliers(string keyword)
        {
            return Query<Supplier>().Where(p => (p.Code.Contains(keyword) || p.Name.Contains(keyword)) && p.State == State.Enable)
                .Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                }).ToList<BaseDataInfo>().ToList();
        }

        /// <summary>
        /// 查找供应商
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>供应商列表</returns>
        public virtual EntityList<Supplier> GetSuppliers(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<Supplier>();
            if (keyword.IsNotEmpty())
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword) || p.Description.Contains(keyword));
            return q.Where(p => p.State == State.Enable).ToList(pagingInfo);
        }

        /// <summary>
        /// 下拉框获取所有供应商列表
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Supplier> GetSupplierList(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<Supplier>();
            if (keyword.IsNotEmpty())
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword) || p.Description.Contains(keyword));
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 查找与用户对应的供应商列表
        /// </summary>
        /// <param name="userId">查询对应的id</param>
        /// <param name="info">分页信息</param>
        /// <param name="state">状态信息</param>
        /// <param name="code">供应商编码</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>供应商列表</returns>
        public virtual EntityList<Supplier> GetSuppliers(double? userId, PagingInfo info, State? state = State.Enable, string code = null, double? itemId = null)
        {
            var query = Query<Supplier>();
            if (userId != null)
                query.Join<SupplierUser>((x, y) => x.Id == y.SupplierId)
                .Join<SupplierUser, SIE.Common.Users.User>((y, z) => y.UserId == z.Id && z.EmployeeId == RT.IdentityId);
            if (state != null)
                query.Where(p => p.State == state);
            if (code.IsNotEmpty())
                query.Where(p => p.Code == code);
            if (itemId != null)
                query.Join<SupplierItem>((x, y) => x.Id == y.SupplierId && y.ItemId == itemId);

            return query.ToList(info);
        }

        /// <summary>
        /// 获取供应商By 编码集合
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>供应商集合</returns>
        public virtual EntityList<Supplier> GetSuppliers(List<string> codes)
        {
            var exp = codes.CreateContainsExpression<Supplier>("x", "Code");
            if (exp == null)
                return new EntityList<Supplier>();
            return Query<Supplier>().Where(exp).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取供应商By 编码集合
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <returns>供应商集合</returns>
        public virtual EntityList<Supplier> GetSupplierByIds(List<double> ids)
        {
            EntityList<Supplier> suppliers = new EntityList<Supplier>();
            DataProcessEx.SplitDataExecute(ids, sons =>
            {
                var suppliersList = Query<Supplier>().Where(p => sons.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                suppliers.AddRange(suppliersList);
            });
            return suppliers;
        }

        /// <summary>
        /// 获取委外扣料供应商By 编码集合
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <returns>供应商集合</returns>
        public virtual EntityList<Supplier> GetOutLocSupplierByIds(List<double> ids)
        {
            EntityList<Supplier> suppliers = new EntityList<Supplier>();
            DataProcessEx.SplitDataExecute(ids, sons =>
            {
                var suppliersList = Query<Supplier>().Where(p => p.OutsourcingOutLocId > 0 && sons.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                suppliers.AddRange(suppliersList);
            });
            return suppliers;
        }

        /// <summary>
        /// 根据供应商查找对应的物料
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="itemType">物料类型</param>
        /// <param name="state">物料状态</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItems(PagingInfo pagingInfo, string keyword, double? supplierId, ItemType? itemType = null, State? state = State.Enable)
        {
            var query = Query<Item>()/*.Where(p => p.Type == ItemType.Material)*/;
            if (supplierId != null)
                query.Join<SupplierItem>((x, y) => x.Id == y.ItemId && y.SupplierId == supplierId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (itemType != null)
                query.Where(p => p.Type == itemType);
            if (state != null)
                query.Where(p => p.State == state);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据供应商查找对应的物料
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="itemTypeList">物料类型</param>
        /// <param name="state">物料状态</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemsSupportItemTypeList(PagingInfo pagingInfo, string keyword, double? supplierId, List<int> itemTypeList = null, State? state = State.Enable)
        {
            var query = Query<Item>()/*.Where(p => p.Type == ItemType.Material)*/;
            if (supplierId != null)
                query.Join<SupplierItem>((x, y) => x.Id == y.ItemId && y.SupplierId == supplierId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (itemTypeList != null)
                query.Where(p => itemTypeList.Contains((int)p.Type));
            if (state != null)
                query.Where(p => p.State == state);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查找与供应商对应的物料
        /// </summary>
        /// <param name="supplierId">查询对应的id</param>
        /// <returns>供应商物料</returns>
        public virtual EntityList<SupplierItem> GetItemList(double supplierId)
        {
            return Query<SupplierItem>().Where(p => p.SupplierId == supplierId).ToList();
        }

        /// <summary>
        /// 查找与供应商对应的物料
        /// </summary>
        /// <param name="supplierId">供应商Id</param>
        /// <param name="itemId">物料Id</param>
        /// <returns>供应商物料</returns>
        public virtual SupplierItem GetSupplierItem(double supplierId, double itemId)
        {
            return Query<SupplierItem>().Where(p => p.SupplierId == supplierId && p.ItemId == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 获取供应商
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>供应商</returns>
        /// <exception cref="ArgumentNullException">空异常</exception>
        public virtual Supplier GetSupplier(string code)
        {
            return Query<Supplier>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 获取供应商
        /// </summary>
        /// <param name="supplierId">供应商Id</param>
        /// <returns>供应商</returns>
        public virtual Supplier GetSupplier(double supplierId, EagerLoadOptions elo = null)
        {
            return Query<Supplier>().Where(p => p.Id == supplierId).FirstOrDefault(elo);
        }

        /// <summary>
        /// 获取供应商与用户关系列表
        /// </summary>
        /// <param name="supplierId">供应商标识</param>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>供应商与用户关系列表</returns>
        public virtual EntityList<SupplierUser> GetUsersBySupplierId(double supplierId, List<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<SupplierUser>()
                .Where(r => r.SupplierId == supplierId)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取供应商与用户关系列表
        /// </summary>
        /// <param name="supplierId">供应商标识</param>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>供应商与用户关系列表</returns>
        public virtual EntityList<SupplierUser> GetUsersBySupplierId(double supplierId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<SupplierUser>()
                .Where(r => r.SupplierId == supplierId)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取供应商与用户关系列表
        /// </summary>
        /// <param name="supplierIds">供应商ID集合</param>
        /// <returns>供应商与用户关系列表</returns>
        public virtual EntityList<SupplierUser> GetUsersBySupplierIds(List<double> supplierIds)
        {
            EntityList<SupplierUser> supplierUser = new EntityList<SupplierUser>();
            DataProcessEx.SplitDataExecute(supplierIds, sons =>
            {
                var customersList = Query<SupplierUser>().Where(p => sons.Contains(p.SupplierId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                supplierUser.AddRange(customersList);
            });
            return supplierUser;
        }

        /// <summary>
        /// 根据供应商编码查找对应的物料
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="supplierCode">供应商编码</param>
        /// <param name="itemTypeList">物料类型</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemsBySupplierCode(PagingInfo pagingInfo, string keyword, string supplierCode, List<int> itemTypeList = null)
        {
            var supplier = GetSupplier(supplierCode);
            if (supplier == null)
                throw new ValidationException("供应商不存在，请检查用户与供应商关系".L10N());
            var query = Query<Item>()/*.Where(p => p.Type == ItemType.Material)*/;
            query.Join<SupplierItem>((x, y) => x.Id == y.ItemId && y.SupplierId == supplier.Id);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (itemTypeList != null)
                query.Where(p => itemTypeList.Contains((int)p.Type));
            return query/*.Where(p => p.State == State.Enable)*/.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取用户关联供应商
        /// 关联一个供应商时返回供应商，其余返回空
        /// </summary>
        /// <returns>供应商</returns>
        public virtual Supplier GetUserLinkSupplier()
        {
            var suppliers = GetSuppliers(RT.IdentityId, null, null);
            if (suppliers.Count == 1)
                return suppliers.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// 获取所有供应商
        /// </summary>
        /// <returns>所有供应商</returns>
        public virtual EntityList<Supplier> GetSupplierList()
        {
            return Query<Supplier>().ToList();
        }

        /// <summary>
        /// 获取指定供应商
        /// </summary>
        /// <param name="supplierIdList">供应商Id集合</param>
        /// <returns>返回指定供应商</returns>
        public virtual EntityList<Supplier> GetSupplierList(List<double> supplierIdList)
        {
            var query = Query<Supplier>();
            query = query.Where(p => supplierIdList.Contains(p.Id) && p.State == State.Disable);

            return query.ToList();
        }

        /// <summary>
        /// 设置指定供应商指定地址为默认地址
        /// </summary>
        /// <param name="supplierId">指定供应商Id</param>
        /// <param name="supplierAddressId">指定地址Id</param>
        public virtual void SetDefaultAddress(double supplierId, double supplierAddressId)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(Supplier.AddressListProperty);
            Supplier supplier = Query<Supplier>().Where(p => p.Id == supplierId).FirstOrDefault(elo);
            SupplierAddress supplierAddress = supplier.AddressList.FirstOrDefault(p => p.Id == supplierAddressId);

            if (supplierAddress == null)
                throw new ValidationException("当前地址已经被删除！".L10N());

            if (supplierAddress.IsDefault)
            {
                supplierAddress.IsDefault = false;
            }
            else
            {
                supplier.AddressList.Where(p => p.IsDefault).ForEach(p => p.IsDefault = false);
                supplierAddress.IsDefault = true;
            }

            RF.Save(supplier);
        }

        /// <summary>
        /// 是否可以设置默认地址
        /// </summary>
        /// <param name="supplierId">供应商Id</param>
        /// <returns>返回是否可以设置默认地址</returns>
        public virtual bool CanSetDefaultAddress(double supplierId)
        {
            return Query<SupplierAddress>().Where(p => p.SupplierId == supplierId).Count() == 0;
        }

        /// <summary>
        /// 获取供应商默认地址
        /// </summary>
        /// <param name="supplierId">供应商Id</param>
        /// <returns>返回客户默认地址</returns>
        public virtual SupplierAddress GetDefaultAddress(double supplierId)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return Query<SupplierAddress>().Where(p => p.SupplierId == supplierId && p.IsDefault && p.State == State.Enable).FirstOrDefault(elo);
        }

        /// <summary>
        /// 获取指定供应商VMI对应的物料
        /// </summary>
        /// <param name="supplierId">供应商Id</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回物料数据</returns>
        /// <remarks>20241011,BUG发现2023年文档更新了VMI所有物料都可以选</remarks>
        public virtual EntityList<Item> GetVMIItem(double supplierId, PagingInfo pagingInfo, string keyword)
        {
            return Query<Item>().Exists<SupplierItem>((a, b) => b.Join<Supplier>((c, d) => c.SupplierId == d.Id && d.State == State.Enable)
                            .Where(p => p.ItemId == a.Id && p.SupplierId == supplierId && p.PurchaseSupplyType == PurchaseSupplyType.VMI))
                            .WhereIf(keyword.IsNotEmpty(), q => q.Code.Contains(keyword) || q.Name.Contains(keyword) ||
                                        q.Description.Contains(keyword) || q.Model.Name.Contains(keyword) ||
                                        q.PurchasingAgent.Name.Contains(keyword))
                            .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取指定启用的供应商
        /// </summary>
        /// <param name="supplierIdList">供应商Id集合</param>
        /// <returns>返回指定启用的供应商</returns>
        public virtual EntityList<Supplier> GetEanbleSupplierList(List<double> supplierIdList)
        {
            var query = Query<Supplier>();
            query = query.Where(p => supplierIdList.Contains(p.Id) && p.State == State.Enable);

            return query.ToList();
        }

        /// <summary>
        /// 获取当前登录用户所关联的所有供应商
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>用户与供应商关系列表</returns>
        public virtual EntityList<SupplierUser> GetUsersBySupplierData(double userId, IList<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(SupplierUser.SupplierProperty);
            var query = Query<SupplierUser>();
            query.Join<Supplier>((x, y) => x.SupplierId == y.Id && y.State == State.Enable && y.IsPortal)
            .Where<Supplier>((x, y) => x.UserId == userId);
            return query.OrderBy(sortInfo).ToList(pagingInfo, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前登录用户所关联的所有供应商
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户与供应商关系列表</returns>
        public virtual EntityList<SupplierUser> GetUsersBySupplier(double userId)
        {
            var query = Query<SupplierUser>();
            query.Join<Supplier>((x, y) => x.SupplierId == y.Id && y.State == State.Enable && y.IsPortal)
            .Where<Supplier>((x, y) => x.UserId == userId);

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查找与供应商门户用户对应的供应商列表
        /// </summary>
        /// <param name="userId">查询对应的id</param>
        /// <param name="info">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="state">状态信息</param>
        /// <param name="isPortal">是否供应商门户</param>
        /// <returns>供应商列表</returns>
        public virtual EntityList<Supplier> GetSupplierDatas(PagingInfo info, string keyword, State? state = State.Enable, bool? isPortal = false)
        {
            var query = Query<Supplier>();

            query.Join<SupplierUser>((x, y) => x.Id == y.SupplierId && y.UserId == RT.Identity.UserId)
            .Join<SupplierUser, SIE.Common.Users.User>((y, z) => y.UserId == z.Id && z.EmployeeId == RT.IdentityId);
            if (state.HasValue)
                query.Where(p => p.State == state);
            if (isPortal.HasValue)
                query.Where(p => p.IsPortal == isPortal);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(info);
        }

        /// <summary>
        /// 获取供应商地址 by供应商编码集合
        /// </summary>
        /// <param name="supplierCodes"></param>
        /// <returns></returns>
        public virtual EntityList<SupplierAddress> GetSupplierAddress(List<string> supplierCodes)
        {
            return Query<SupplierAddress>().Where(p => supplierCodes.Contains(p.Supplier.Code)).ToList(null, new EagerLoadOptions().LoadWith(SupplierAddress.SupplierProperty));
        }

        /// <summary>
        /// 删除供应商物料
        /// </summary>
        /// <param name="selectedItems">物料实体</param>
        public virtual void DeleteSupplierItem(List<SupplierItem> selectedItems)
        {
            if (selectedItems == null)
                return;
            var productModelList = new EntityList<SupplierItem>();
            foreach (var productModel in selectedItems)
            {
                productModel.PersistenceStatus = PersistenceStatus.Deleted;
                productModelList.Add(productModel);
            }

            RF.Save(productModelList);
        }

        /// <summary>
        /// 启用门户
        /// </summary>
        /// <param name="ids">供应商ID集合</param>
        public virtual EntityList<SupplierPswViewModel> EnableSupplierPortal(List<double> ids)
        {
            var invOrg = Query<Rbac.InvOrgs.InvOrg>().Where(p => p.Code == RT.InvOrg.Value).FirstOrDefault();
            if (invOrg == null)
                throw new ValidationException("库存组织[{0}]不存在".L10nFormat(RT.InvOrg.Value));
            var result = new EntityList<SupplierPswViewModel>();
            var UserController = RT.Service.Resolve<UserController>();
            EntityList<Supplier> suppliers = GetSupplierByIds(ids);
            EntityList<SupplierUser> supplierUserList = GetUsersBySupplierIds(ids);
            List<string> empCodes = suppliers.Select(p => p.Code).Distinct().ToList();
            var employeeDicts = RT.Service.Resolve<EmployeeController>().GetEmployeeList(empCodes).ToDictionary(p => p.Code);

            bool isAutoCreate = false;
            var config = ConfigService.GetConfig(new SupplierIsAutoCreateConfig(), typeof(Supplier));
            if (config != null)
                isAutoCreate = config.IsAutoCreate;

            var IsRandomPassWord = UserController.GetIsRandomPswConfig();
            var policy = UserController.GetSecurityPolicy();
            using (var tran = DB.TransactionScope(CsmEntityDataProvider.ConnectionStringName))
            {
                suppliers.ForEach(p =>
                {
                    p.IsPortal = true;

                    if (isAutoCreate)
                    {
                        var supplierUsers = supplierUserList.Where(q => q.SupplierId == p.Id).ToList();
                        if (!supplierUsers.Any(q => q.UserCode == p.Code) && !employeeDicts.TryGetValue(p.Code, out Employee employee))
                        {
                            AutoCreateSupplierUserAndEmployee(p, invOrg, IsRandomPassWord, policy, result);
                        }
                    }
                });

                RF.Save(suppliers);

                tran.Complete();
            }
            return result;
        }

        /// <summary>
        /// 自动创建供应商用户和员工
        /// </summary>
        /// <param name="supplier">供应商</param>
        /// <param name="invOrg">当前库存组织</param>
        /// <param name="randomPsw">是否随机密码</param>
        /// <param name="policy">密码强度</param>
        /// <param name="supplierPswViewModels">供应商和密码视图</param>
        private void AutoCreateSupplierUserAndEmployee(Supplier supplier, InvOrg invOrg, bool randomPsw, SecurityPolicy policy, EntityList<SupplierPswViewModel> supplierPswViewModels)
        {
            var userController = RT.Service.Resolve<UserController>();
            //用户
            var user = new Rbac.Users.User();
            user.GenerateId();
            var emp = new Employee();
            emp.GenerateId();

            user.Code = supplier.Code;
            user.State = State.Enable;
            user.AuthenticateType = "Native";
            user.EmployeeId = emp.Id;

            //员工
            emp.Code = supplier.Code;
            emp.Name = supplier.Name;
            emp.UserId = user.Id;
            emp.EmployeeStatus = EmployeeStatus.Job;

            RF.Save(user);
            RF.Save(emp);

            //用户安全信息
            //Rbac.Users.UserController.GetSecurityPolicy
            //var security = new UserSecurity();
            //security.UserId = user.Id;
            var security = userController.GetUserSecurityByUserId(user.Id);
            if (security == null)
            {
                security = new UserSecurity();
                security.GenerateId();
                security.UserId = user.Id;
            }
            if (randomPsw)
            {
                var passWord = userController.GetAutoRandomPassWord(policy);
                //userController.SaveRawPwd(security, passWord);
                security.Password = SIE.Security.CryptographyHelper.MD5(passWord);
                supplierPswViewModels.Add(new SupplierPswViewModel()
                {
                    UserId = user.Id,
                    UserName = user.Code,
                    UserCode = user.Code,
                    PassWord = passWord,
                });
            }
            else
            {
                security.Password = SIE.Security.CryptographyHelper.MD5("123456");
            }

            RF.Save(security);
            //用户数据
            var userData = new UserData();
            userData.UserId = user.Id;
            userData.CurInvOrg = invOrg.Code;
            RF.Save(userData);

            //用户与库存组织关系
            var orgUser = new UserInInvOrg();
            orgUser.InvOrgId = invOrg.Id;
            orgUser.InvOrg = invOrg;
            orgUser.UserId = user.Id;
            RF.Save(orgUser);

            //供应商与用户关系
            var supplierUser = new SupplierUser();
            supplierUser.SupplierId = supplier.Id;
            supplierUser.UserId = user.Id;
            RF.Save(supplierUser);
        }

        /// <summary>
        /// 禁用门户
        /// </summary>
        /// <param name="ids">供应商ID集合</param>
        public virtual void DisableSupplierPortal(List<double> ids)
        {
            EntityList<Supplier> suppliers = GetSupplierByIds(ids);
            suppliers.ForEach(p => p.IsPortal = false);
            RF.Save(suppliers);
        }

        /// <summary>
        /// 启用供应商
        /// </summary>
        /// <param name="id">供应商ID</param>
        public virtual void EnableSupplier(double id)
        {
            var supplier = GetById<Supplier>(id);
            if (supplier == null)
                throw new EntityNotFoundException(typeof(Supplier), id);
            supplier.State = State.Enable;
            RF.Save(supplier);
        }

        /// <summary>
        /// 禁用供应商
        /// </summary>
        /// <param name="id">供应商ID</param>
        public virtual void DisableSupplier(double id)
        {
            var supplier = GetById<Supplier>(id);
            if (supplier == null)
                throw new EntityNotFoundException(typeof(Supplier), id);
            supplier.State = State.Disable;
            RF.Save(supplier);
        }

        /// <summary>
        /// 查询供应商信息
        /// </summary>
        /// <param name="page">分页信息</param>
        /// <param name="SupplerCodeLike">供应商编码</param>
        /// <param name="State">状态</param>
        /// <returns>供应商信息</returns>
        public virtual EntityList<Supplier> SearchSupplierList(PagingInfo page, string SupplerCodeLike, State State)
        {
            var query = Query<Supplier>();
            query.Where(p => p.Code.Contains(SupplerCodeLike) && p.State == State);

            return query.ToList(page, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据配置项设置供应商默认值
        /// </summary>
        /// <returns></returns>
        public virtual SupplierConfigValue SetSupplierConfigValue()
        {
            var rst = new SupplierConfigValue();
            var config = ConfigService.GetConfig(new SupplierOutsourcingConfig(), typeof(Supplier));
            if (config.OutsourcingInLoc != null)
            {
                //rst.OutsourcingInLoc = config.OutsourcingInLoc;
                rst.OutsourcingInLocId = config.OutsourcingInLocId.Value;
                rst.OutsourcingInLocCode = config.OutsourcingInLoc.Code;
                rst.OutsourcingInLocName = config.OutsourcingInLoc.Name;
            }
            if (config.OutsourcingOutLoc != null)
            {
                //rst.OutsourcingOutLoc = config.OutsourcingOutLoc;
                rst.OutsourcingOutLocId = config.OutsourcingOutLocId.Value;
                rst.OutsourcingOutLocCode = config.OutsourcingOutLoc.Code;
                rst.OutsourcingOutLocName = config.OutsourcingOutLoc.Name;
            }
            rst.OutsourcingUseTime = config.OutsourcingUseTime;
            rst.OutsourcingReceiveType = config.OutsourcingReceive;
            rst.IsHasStorer = config.IsHasStorer;
            return rst;
        }
    }
}