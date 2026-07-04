using DocumentFormat.OpenXml.Office.CustomUI;
using ICSharpCode.SharpZipLib.Zip;
using SIE.Common;
using SIE.Common.Employees;
using SIE.Common.Users;
using SIE.Core.ApiModels;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.Resources.Enterprises;
using SIE.Resources.LineAndons;
using SIE.Resources.WipResources;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工控制器
    /// </summary>
    public partial class EmployeeController : DomainController, IEmployee
    {
        /// <summary>
        /// 获取员工列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>员工列表</returns>
        public virtual EntityList<Employee> GetEmployees(Expression<Func<Employee, bool>> exp, PagingInfo pagingInfo = null, string keyword = null)
        {
            var query = Query<Employee>();
            if (keyword != null)
                query.Where(p => p.Name.Contains(keyword));
            if (exp != null)
                query.Where(exp);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>

        public virtual EntityList<Employee> GetEmployees(PagingInfo pagingInfo = null, string keyword = null)
        {
            var query = Query<Employee>();
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 查询员工列表
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<SIE.Resources.Employee> GetEmployeeList(PagingInfo pagingInfo = null, string keyword = null)
        {
            var query = Query<SIE.Resources.Employee>();
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取员工集合
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="filterIds">过滤员工ID数组</param>
        /// <returns>员工集合</returns>
        public virtual EntityList<Employee> GetEmployees(string keyword, double[] filterIds)
        {
            var query = Query<Employee>();
            query.Where(p => !filterIds.Contains(p.Id));
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList();
        }


        /// <summary>
        /// 查询员工
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetEmployees(EmployeeSelectCriteria criteria)
        {
            var query = Query<Employee>();
            query.Where(p => p.Id != RT.IdentityId);
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    query.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    query.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.HireDate.HasValue)
                    query.Where(p => p.HireDate == criteria.HireDate.Value);
                if (criteria.UserId.HasValue)
                    query.Where(p => p.UserId == criteria.UserId.Value);
                if (criteria.WorkGroupId.HasValue)
                    query.Where(p => p.WorkGroupId == criteria.WorkGroupId.Value);
                if (criteria.EmployeeStatus.HasValue)
                {
                    query.Where(p => p.EmployeeStatus == criteria.EmployeeStatus.Value);
                }
                if (criteria.Sex.HasValue)
                    query.Where(p => p.Sex == criteria.Sex.Value);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }
        /// <summary>
        /// 根据编码名称获取用户
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">姓名</param>
        /// <returns>EntityList</returns>
        public virtual EntityList<User> GetUsersByCodeOrName(string code, string name)
        {
            var query = Query<User>();
            if (!code.IsNullOrEmpty())
            {
                query.Where(p => p.Code == code);
            }

            if (!name.IsNullOrEmpty())
            {
                query.Where(p => p.Employee.Name == name);
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取员工
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>员工</returns>
        public virtual Employee GetEmployeeByCode(string code)
        {
            return Query<Employee>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 获取员工
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns>员工</returns>
        public virtual Employee GetEmployeeByName(string name)
        {
            return Query<Employee>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Resource员工转换成Common员工类型
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public virtual SIE.Common.Employees.Employee ChangeToCommonEmployee(Employee employee)
        {
            SIE.Common.Employees.Employee result = new Common.Employees.Employee()
            {
                Id = employee.Id,
                Code = employee.Code,
                Name = employee.Name,
                UserId = employee.UserId,
            };
            return result;
        }

        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <param name="createAccount">是否创建用户</param>
        /// <param name="authenticateType">认证方式</param>
        /// <param name="userCode">用户编码</param>
        /// <exception cref="ArgumentNullException">参数空引用异常</exception>
        public virtual Employee SaveEmployee(Employee employee, bool createAccount, string authenticateType, string userCode)
        {
            Check.NotNull(employee, nameof(employee));

            using (var tran = DB.TransactionScope(CommonEntityDataProvider.ConnectionStringName))
            {
                if (createAccount && (employee.UserId == null || employee.UserId <= 0))
                {
                    var loginUser = new LoginUser
                    {
                        EmployeeId = employee.Id,
                        Name = employee.Name,
                        Code = userCode.IsNullOrEmpty() ? employee.Code : userCode,
                        AuthenticateType = authenticateType,
                        State = State.Enable
                    };
                    employee.UserId = RT.Service.Resolve<IUserService>().CreateUser(loginUser).Id;
                }
                RF.Save(employee);
                tran.Complete();
            }
            return employee;
        }

        /// <summary>
        /// 保存修改后的员工
        /// </summary>
        /// <returns></returns>
        public virtual Employee SaveEditedEmployee(Employee employee)
        {
            using (var tran = DB.TransactionScope(CommonEntityDataProvider.ConnectionStringName))
            {
                UnlinkLastUser(employee.Id);
                RF.Save(employee);
                if (employee.User != null)
                {
                    var user = employee.User;
                    user.EmployeeId = employee.Id;
                    //员工离职状态时，同步用户应该调整为“禁用”状态。
                    if (employee.EmployeeStatus == EmployeeStatus.UnJob && user.State == State.Enable)
                        user.State = State.Disable;
                    if (employee.EmployeeStatus == EmployeeStatus.Job && user.State == State.Disable)
                        user.State = State.Enable;
                    RF.Save(user);
                }
                tran.Complete();
            }
            return employee;
        }

        /// <summary>
        /// 关联用户
        /// </summary>
        /// <param name="employeeId">员工</param>
        /// <param name="userId">用户</param>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <exception cref="EntityNotFoundException">实体空引用异常</exception>
        public virtual void LinkUser(double employeeId, double userId)
        {
            var employee = GetById<Employee>(employeeId);
            if (employee == null)
                throw new EntityNotFoundException(typeof(Employee), employeeId);

            var user = GetById<User>(userId);
            if (user == null)
                throw new EntityNotFoundException(typeof(User), userId);

            if (user.EmployeeId != null)
                throw new ValidationException("被关联用户已存在关联员工[{0}]，请先取消关联".L10nFormat(user.Employee?.Name));

            if (employee.User != null)
                throw new ValidationException(
                    "员工[{0}]已经关联了用户[{1}],请先取消关联".L10nFormat(employee.Code, employee.User.Code));

            user.EmployeeId = employee.Id;
            RF.Save(user);

            employee.UserId = user.Id;
            RF.Save(employee);           
        }

        /// <summary>
        /// 解除关联用户
        /// </summary>
        /// <param name="employeeId">员工</param>
        /// <exception cref="EntityNotFoundException">实体空引用异常</exception>
        public virtual void UnlinkUser(double employeeId)
        {
            var employee = GetById<Employee>(employeeId);
            if (employee == null)
                throw new EntityNotFoundException(typeof(Employee), employeeId);

            var user = employee.User;
            if (user == null)
                throw new ValidationException("员工[{0}]与用户的关系已解除".L10nFormat(employee.Code));

            user.Employee = null;
            RF.Save(user);

            employee.User = null;
            RF.Save(employee);
        }

        /// <summary>
        /// 解除上一次关联的用户
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        public virtual void UnlinkLastUser(double employeeId)
        {
            var employee = GetById<Employee>(employeeId);
            if (employee != null)
            {
                var user = employee.User;
                if (user != null)
                {
                    user.Employee = null;
                    RF.Save(user);
                }
            }
        }

        /// <summary>        
        /// 获取所有关联人员的用户
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>用户列表</returns>
        public virtual EntityList<Resources.Employee> GetLinkedEmployees(string code, string name, PagingInfo pagingInfo)
        {
            var query = Query<Resources.Employee>().Join<User>((x, y) => x.Id == y.EmployeeId);
            query.Where(p => p.Code.Contains(code) || p.Name.Contains(name));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取所有未关联用户
        /// </summary>
        /// <param name="code">账号</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>分页信息</returns>
        public virtual EntityList<User> GetNotLinkedUser(string code, PagingInfo pagingInfo)
        {
            var query = Query<User>().Where(p => p.EmployeeId == null);
            if (code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains("%" + code + "%"));
            }
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据员工ID获取员工所对应的资源
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>EntityList</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<EmployeeResource> GetEmployeeResources(double employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            return Query<EmployeeResource>().Where(p => p.EmployeeId == employeeId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据员工ID和资源Id获取员工所对应的资源
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>EntityList</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<EmployeeResource> GetEmployeeResources(double employeeId,double resourceId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            return Query<EmployeeResource>().Where(p => p.EmployeeId == employeeId && p.ResourceId == resourceId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据员工Id列表获取员工资源列表
        /// </summary>
        /// <param name="employeeIds">员工Id列表</param>
        /// <returns>员工资源列表</returns>
        public virtual EntityList<EmployeeResource> GetEmployeeResources(List<double> employeeIds)
        {
            return Query<EmployeeResource>().Where(p => employeeIds.Contains(p.EmployeeId)).ToList();
        }

        /// <summary>
        /// 根据资源ID获取员工
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <returns>EntityList</returns>  
        public virtual EntityList<EmployeeResource> GetEmployeeResourcesByResId(double resourceId)
        {
            return Query<EmployeeResource>().Where(p => resourceId == p.ResourceId).ToList();
        }

        /// <summary>
        /// 根据资源ID获取员工
        /// </summary>
        /// <param name="workShopId">车间ID</param>
        /// <returns>EntityList</returns>  
        public virtual EntityList<EmployeeResource> GetEmployeeResourcesByShopId(double workShopId)
        {
            return Query<EmployeeResource>().Where(p => p.Resource.WorkShopId == workShopId && p.Resource.SourceType == SyncSourceType.Enterprise).ToList();
        }

        /// <summary>
        /// 此班组是否包含员工
        /// </summary>
        /// <param name="workGroupId">班组ID</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">参数空引用异常</exception>
        public virtual bool WorkGroupHasEmployee(double workGroupId)
        {
            if (workGroupId <= 0)
            {
                throw new ArgumentNullException(nameof(workGroupId));
            }

            var query = Query<Employee>();
            query.Where(p => p.WorkGroupId == workGroupId);
            return query.Count() > 0;
        }

        /// <summary>
        /// 此资源是否包含员工
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual bool ResourceHasEmployee(double resourceId)
        {
            if (resourceId <= 0)
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            var query = Query<EmployeeResource>();
            query.Where(p => p.ResourceId == resourceId);
            return query.Count() > 0;
        }

        /// <summary>
        /// 用户是否有资源
        /// </summary>
        /// <param name="employeeId">用户ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">参数空引用异常</exception>
        public virtual bool UserHasResource(double employeeId, double resourceId)
        {
            var query = Query<EmployeeResource>();
            query.Where(p => p.EmployeeId == employeeId && p.ResourceId == resourceId);
            return query.Count() > 0;
        }

        /// <summary>
        /// 根据班组ID查询员工信息
        /// </summary>
        /// <param name="workGroupId">班组ID</param>
        /// <param name="info">分页参数</param>
        /// <param name="key">搜索关键字</param>
        /// <returns>EntityList</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<Employee> GetEmployeeByWorkGroupId(double workGroupId, PagingInfo info = null, string key = "")
        {
            if (workGroupId <= 0)
                throw new ArgumentNullException(nameof(workGroupId));
            var query = Query<Employee>().Where(p => p.WorkGroupId == workGroupId);
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(p => p.Name.Contains(key) || p.Code.Contains(key));
            }
            query.OrderBy(p => new { p.Code, p.Name });
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据班组ID查询员工信息
        /// </summary>
        /// <param name="workGroupId">班组ID</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns>EntityList</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<Employee> GetEmployeeByWorkOrderGroupId(double workGroupId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            if (workGroupId <= 0)
                throw new ArgumentNullException(nameof(workGroupId));
            var query = Query<Employee>().Where(p => p.WorkGroupId == workGroupId);
          
            query.OrderBy(sortInfo);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据班组获取员工信息
        /// </summary>
        /// <param name="workGroupId">班组ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>员工信息</returns>
        public virtual EntityList<Employee> GetEmployeeByWorkGroupId(double workGroupId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Employee>().Where(p => p.WorkGroupId == workGroupId).WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取班组的班长信息
        /// </summary>
        /// <param name="workGroupId">班组Id</param>
        /// <returns>班组的班长信息</returns>
        public virtual Employee GetMonitor(double workGroupId)
        {
            Employee monitor = null;
            var employs = GetEmployeeByWorkGroupId(workGroupId);
            if (employs != null && employs.Any())
            {
                monitor = employs.FirstOrDefault(p => p.EmployeeType != null && p.EmployeeType == EmployeeType.Foreman);
            }
            return monitor;
        }

        /// <summary>
        /// 查询员工组
        /// </summary>
        /// <param name="criteria">员工组查询实体</param>
        /// <returns>EntityList</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<EmployeeGroup> GetEmployeeGroups(EmployeeGroupCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));
            var query = Query<EmployeeGroup>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            return query.ToList(criteria.PagingInfo);
        }

        /// <summary>
        /// 获取所有员工组
        /// </summary>        
        /// <returns>所有员工组</returns>        
        public virtual EntityList<EmployeeGroup> GetEmployeeGroupList()
        {
            return Query<EmployeeGroup>().ToList();
        }

        /// <summary>
        /// 根据员工组Id列表获取员工组列表
        /// </summary>
        /// <param name="ids">员工组Id列表</param>
        /// <returns>员工组列表</returns>
        public virtual EntityList<EmployeeGroup> GetEmployeeGroupList(List<double> ids)
        {
            return ids.SplitContains((tempIds) =>
            {
                return Query<EmployeeGroup>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 员工是否存在资源
        /// </summary>
        /// <param name="employeeId">用户ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual bool EmployeeHasResource(double employeeId, double resourceId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            if (resourceId <= 0)
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            var q = Query<EmployeeResource>();
            q.Where(p => p.ResourceId == resourceId && p.EmployeeId == employeeId);
            return q.Count() > 0;
        }

        /// <summary>
        /// 根据用户ID查找员工
        /// </summary>
        /// <param name="empId">员工Id</param>
        /// <returns>Employee</returns>
        public virtual Employee GetEmployeeByUserId(double empId)
        {
            var q = Query<Employee>().Where(p => p.Id == empId);
            return q.FirstOrDefault();
        }

        /// <summary>
        /// 获取所有员工
        /// </summary>        
        /// <returns>所有员工</returns>        
        public virtual EntityList<Employee> GetGetEmployeeList()
        {
            return Query<Employee>().ToList();
        }

        /// <summary>
        /// 通过员工Id列表获取员工列表
        /// </summary>
        /// <param name="ids">员工Id列表</param>
        /// <returns>员工列表</returns>
        public virtual EntityList<Employee> GetEmployeeList(List<double> ids)
        {
            return ids.SplitContains((tempIds) =>
            {
                return Query<Employee>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(Employee.WorkGroupProperty));
            });
        }

        /// <summary>
        /// 通过员工code列表获取员工列表
        /// </summary>
        /// <param name="codes">员工code列表</param>
        /// <returns>员工列表</returns>
        public virtual EntityList<Employee> GetEmployeeList(List<string> codes)
        {
            return codes.SplitContains(pCodes =>
            {
                return Query<Employee>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 通过用户code列表获取用户列表
        /// </summary>
        /// <param name="codes">用户code列表</param>
        /// <returns>员工列表</returns>
        public virtual EntityList<Rbac.Users.User> GetUserList(List<string> codes)
        {
            return codes.SplitContains(pCodes =>
            {
                return Query<Rbac.Users.User>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 通过班组Id列表获取员工列表
        /// </summary>
        /// <param name="workGroupIds">班组Id列表</param>
        /// <returns>员工列表</returns>
        public virtual EntityList<Employee> GetEmployeeListByWorkGroup(List<double?> workGroupIds)
        {
            return Query<Employee>().Where(p => workGroupIds.Contains(p.WorkGroupId)).ToList(null, new EagerLoadOptions().LoadWith(Employee.WorkGroupProperty));
        }

        /// <summary>
        /// 通过员工组Id列表获取员工列表
        /// </summary>
        /// <param name="employeeGroupIds">员工组Id列表</param>
        /// <returns>员工列表</returns>
        public virtual EntityList<Employee> GetEmployeeListByEmployeeGroup(List<double?> employeeGroupIds)
        {
            return Query<Employee>().Where(p => employeeGroupIds.Contains(p.EmployeeGroupId)).ToList(null, new EagerLoadOptions().LoadWith(Employee.WorkGroupProperty));
        }

        /// <summary>
        /// 获取所有在职员工
        /// </summary>        
        /// <returns>所有员工</returns>        
        public virtual EntityList<Employee> GetEmployeeListOnJob(List<double> ids)
        {
            return Query<Employee>().Join<WorkGroup>((x, y) => x.WorkGroupId == y.Id).Where(p => p.EmployeeStatus == EmployeeStatus.Job && p.WorkGroupId != null && !ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取所有在职员工
        /// </summary>        
        /// <returns>所有员工</returns>        
        public virtual EntityList<Employee> GetEmployeeListOnJob()
        {
            return Query<Employee>().Join<WorkGroup>((x, y) => x.WorkGroupId == y.Id).Where(p => p.EmployeeStatus == EmployeeStatus.Job && p.WorkGroupId != null).ToList();
        }

        /// <summary>
        ///  获取所有在职员工(根据工号,姓名)
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetEmployeeListOnJob(PagingInfo pagingInfo, string keyword = null)
        {
            var query = Query<Employee>();
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据引用判断是否可以删除企业模型和设备模型
        /// </summary>
        /// <param name="id">来源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns>true,false</returns>
        public virtual bool IsHasUsedResourse(double id, SyncSourceType sourceType)
        {
            var res = RT.Service.Resolve<WipResourceController>().GetWipResource(id, sourceType);
            if (res == null) return true;
            return Query<EmployeeResource>().Where(p => p.ResourceId == res.Id).FirstOrDefault() == null;
        }

        /// <summary>
        /// 判断员工资源是否引用指定的生产资源
        /// </summary>
        /// <param name="wipResourceId">生产资源Id</param>
        /// <returns></returns>
        public virtual bool EmployeeResourceHasUsedResource(double wipResourceId)
        {
            var employeeResource = Query<EmployeeResource>().Where(x => x.ResourceId == wipResourceId).FirstOrDefault();
            if (employeeResource == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 根据员工编号获取员工ID
        /// </summary>
        /// <param name="code">员工编号</param>
        /// <returns>员工ID，员工不存在返回null</returns>
        public virtual double? GetEmployeeId(string code)
        {
            return Query<Employee>().Where(p => p.Code == code).FirstOrDefault()?.Id;
        }

        /// <summary>
        /// 根据员工ID查找员工
        /// </summary>        
        /// <returns>员工</returns>        
        public virtual Employee GetEmployeeById(double employeeId)
        {
            return Query<Employee>().Where(p => p.Id == employeeId).FirstOrDefault(new EagerLoadOptions().LoadWith(Employee.WorkGroupProperty));
        }

        /// <summary>
        /// 根据员工ID查找员工编号
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <returns>员工编号</returns>
        public virtual string GetEmployeeCodeById(double employeeId)
        {
            return Query<Employee>().Where(p => p.Id == employeeId).Select(f => f.Code).FirstOrDefault<string>();
        }

        /// <summary>
        /// 根据员工ID集合查找员工
        /// </summary>        
        /// <returns>员工</returns>        
        public virtual EntityList<Employee> GetEmployeeByIds(List<double> employeeIds)
        {
            if (employeeIds.IsNullOrEmpty()) return new EntityList<Employee>();
            return Query<Employee>().Where(p => employeeIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(Employee.WorkGroupProperty));
        }

        /// <summary>
        /// 获取关联部门的员工列表
        /// </summary>
        /// <param name="keywork">员工编号/名称</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns>员工列表</returns>
        public virtual EntityList<Employee> GetExistDepartmentEmployees(string keywork, PagingInfo pagingInfo)
        {
            var query = Query<Employee>()
                             .Join<WorkGroup>((e, w) => e.WorkGroupId == w.Id)
                             .Join<WorkGroup, Enterprise>((e, d) => e.DepartmentId == d.Id);
            if (!keywork.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keywork) || p.Name.Contains(keywork));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取邮箱不为空的在职人员信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetEmailIsNotNullEmployee(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Employee>();
            query.Where(p => p.EmployeeStatus == EmployeeStatus.Job && !p.Email.IsNullOrEmpty());
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        #region 班组 
        /// <summary>
        /// 根据班组名称获取班组
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>班组</returns>
        public virtual WorkGroup GetWorkGroup(string name)
        {
            return Query<WorkGroup>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 获取不包含当前名称的班组
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>EntityList</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<WorkGroup> GetWorkGroupsExceptId(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(name));
            }

            var q = Query<WorkGroup>();
            q.Where(p => p.Name != name);
            return q.ToList();
        }

        /// <summary>
        /// 获取所有班组
        /// </summary>        
        /// <returns>所有班组</returns>        
        public virtual EntityList<WorkGroup> GetWorkGroupsList()
        {
            return Query<WorkGroup>().ToList();
        }

        /// <summary>
        /// 根据班组Id列表获取班组列表
        /// </summary>
        /// <param name="ids">班组Id列表</param>
        /// <returns>班组列表</returns>
        public virtual EntityList<WorkGroup> GetWorkGroupList(List<double> ids)
        {
            return ids.SplitContains((tempIds) =>
            {
                return Query<WorkGroup>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取班组列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>班组列表列表</returns>
        public virtual EntityList<WorkGroup> GetWorkGroups(Expression<Func<WorkGroup, bool>> exp, PagingInfo pagingInfo = null)
        {
            var query = Query<WorkGroup>();
            if (exp != null)
                query.Where(exp);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取部门下的所有班组
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="keywork">班组编码/名称</param>
        /// <returns>班组集合</returns>
        public virtual EntityList<WorkGroup> GetWorkGroups(double departmentId, string keywork)
        {
            var query = Query<WorkGroup>().Where(p => p.DepartmentId == departmentId);
            if (keywork.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keywork) || p.Name.Contains(keywork));
            return query.ToList();
        }
        #endregion


        /// <summary>
        /// 根据用户Id获取员工
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual Employee GetEmployeeByUId(double? userId)
        {
            var result = DB.Query<Employee>().Where(p => p.UserId == userId).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 删除员工与工厂关系数据
        /// </summary>
        /// <param name="ids">员工与工厂关系数据Id</param>
        public virtual void DeleteEmployeeEnterprise(List<double> ids)
        {
            DB.Delete<EmployeeEnterprise>().Where(p => ids.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 保存员工与工厂关系
        /// </summary>
        /// <param name="employeeEnterpriseList">员工与工厂关系</param>
        public virtual void SaveEmployeeEnterprise(List<EmployeeEnterprise> employeeEnterpriseList)
        {
            EntityList<EmployeeEnterprise> savedData = new EntityList<EmployeeEnterprise>();
            Check.NotNullOrEmpty(employeeEnterpriseList, nameof(employeeEnterpriseList));

            if (null == employeeEnterpriseList || employeeEnterpriseList.Count == 0)
                throw new ArgumentNullException(nameof(employeeEnterpriseList));

            foreach (var item in employeeEnterpriseList)
            {
                var employeeEnterprise = new EmployeeEnterprise();
                employeeEnterprise.EnterpriseId = item.EnterpriseId;
                employeeEnterprise.EmployeeId = item.EmployeeId;
                savedData.Add(employeeEnterprise);
            }
            RF.Save(savedData);
        }

        /// <summary>
        /// 获取WCS专用账号
        /// </summary>
        /// <returns></returns>
        public virtual Employee GetWcsEmployee()
        {
            var emp = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode("WCS_" + (RT.InvOrg.HasValue ? RT.InvOrg.Value : 0));
            if (emp == null)
            {
                emp = new Employee() { Code = "WCS_" + (RT.InvOrg.HasValue ? RT.InvOrg.Value : 0), Name = "WCS_" + (RT.InvOrg.HasValue ? RT.InvOrg.Value : 0), Remark = "立库专用任务领用账号" };
                RF.Save(emp);
            }
            return emp;
        }

        /// <summary>  
        /// 获取登录用户所关联的员工  
        /// </summary>  
        /// <returns></returns>  
        public virtual SIE.Resources.Employee GetLoginUserEmployee()
        {
            var employee = RF.GetById<SIE.Resources.Employee>(RT.IdentityId);
            return employee;
        }

        /// <summary>
        /// 根据工厂查询关联的员工
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="eagerLoadOptions"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetEmployeesByFactoryAuth(double? factoryId, string keyword = null, PagingInfo pagingInfo = null, EagerLoadOptions eagerLoadOptions = null)
        {
            var q = Query<Employee>();
            if (keyword != null)
            {
                q.Where(p => p.Name.Contains(keyword));
            }
            if (factoryId.HasValue)
            {
                q.Join<EmployeeEnterprise>((emp, ent) => emp.Id == ent.EmployeeId && ent.EnterpriseId == factoryId);
            }
            return q.ToList(pagingInfo, eagerLoadOptions);
        }

        /// <summary>
        /// 保存员工
        /// </summary>
        /// <param name="employeeInfo">员工信息</param>
        /// <returns>员工ID</returns>
        public double? SaveEmployeeInfos(EmployeeInfo employeeInfo)
        {
            if (employeeInfo == null)
            {
                return null;
            }

            Employee employee = new Employee();
            employee.Code = employeeInfo.EmpCode;
            employee.Name = employeeInfo.EmpName;
            employee.Phone = employeeInfo.EmpPhone;
            employee.Email = employeeInfo.EmpEmail;
            employee.EmployeeStatus = EmployeeStatus.Job;
            if (employeeInfo.EmpSex == Sex.Man.ToLabel())
            {
                employee.Sex = Sex.Man;
            }
            else
            {
                employee.Sex = Sex.Madam;
            }

            if (employeeInfo.EmpHireDate.IsNotEmpty())
            {
                if (DateTime.TryParse(employeeInfo.EmpHireDate, out DateTime dt))
                {
                    employee.HireDate = dt;
                }
                else
                {
                    throw new ValidationException("日期格式:[{0}]不正确".L10nFormat(employeeInfo.EmpHireDate));
                }
            }

            employee.UserId = employeeInfo.UserId;
            employee.Remark = employeeInfo.EmpRemark;

            RF.Save(employee);

            return employee.Id;
        }
        #region 签名
        /// <summary>
        /// 根据员工ID获取员工签名数据
        /// </summary>
        /// <param name="EmployeeId">员工ID</param>
        /// <returns>员工签名数据</returns>
        public virtual EntityList<EmployeeSign> GetEmployeeSign(double EmployeeId)
        {
            return Query<EmployeeSign>().Where(p => p.EmployeeId == EmployeeId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 批量保存上传的文件
        /// </summary>
        /// <param name="FileContent">压缩文件byte[]</param>

        public virtual void BatchSavePhoto(byte[] FileContent)
        {
            var fs = new MemoryStream(FileContent);
            ExtractZipFileAndSave(fs, "");
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="Id">员工与签名表主键ID</param>
        /// <returns></returns>
        public virtual string getSignPhoto(double Id)
        {
            var employeeSign = RF.GetById<EmployeeSign>(Id);
            if (employeeSign.SingPhoto == null)
            {
                throw new ValidationException("图片不存在".L10N());
            }
            string fileContent = "";
            string fileHead = $"data:image/{employeeSign.FileSuffix};base64,";
            string base64String = Convert.ToBase64String(employeeSign.SingPhoto);
            fileContent = fileHead + base64String;
            return fileContent;
        }

        /// <summary>
        /// 员工子页签单独上传图片/首页上传图片
        /// </summary>
        public virtual void SignleUploadImage(double Id, string fileContent, string fileName)
        {
            var employee = RF.GetById<Employee>(Id);
            var currentFileContent = fileContent.Split(',')[1];
            EntityList<EmployeeSign> employeeSigns = new EntityList<EmployeeSign>();
            var photoBytes = Convert.FromBase64String(currentFileContent);
            EmployeeSign employeeSign = SetEmployeeSignData(employee, employeeSigns, photoBytes, fileName);
            RF.Save(employeeSign);
        }
        /// <summary>
        ///  解压压缩文件验证并保存
        ///  文本文件
        /// </summary>
        /// <param name="fs">文件流对象</param>
        /// <param name="password">密码</param>
        /// <returns>文件的内容</returns>
        private void ExtractZipFileAndSave(Stream fs, string password)
        {
            EntityList<EmployeeSign> employeeSignList = new EntityList<EmployeeSign>();
            ZipFile zf = null;
            try
            {
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    // AES encrypted entries are handled automatically
                    zf.Password = password;
                }

                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        // Ignore directories
                        continue;
                    }
                    //验证文件格式
                    var employee = VerifySignPhoto(zipEntry);
                    Stream zipStream = zf.GetInputStream(zipEntry);
                    byte[] bytePhoto = StreamToByte(zipStream, zipEntry);
                    //byte[] bytePhoto = zipEntry.ExtraData;
                    var PhotoNameList = zipEntry.Name.Split('.').ToList();
                    var FileSuffix = PhotoNameList[PhotoNameList.Count - 1].ToLower();
                    var employeeSign = SetEmployeeSignData(employee, employeeSignList, bytePhoto, FileSuffix);
                    employeeSignList.Add(employeeSign);
                }
            }
            catch
            {
                throw new ValidationException("文件已损坏,请上传正确的文件".L10N());
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
                RF.Save(employeeSignList);
            }

        }

        /// <summary>
        /// 验证图片的信息
        /// </summary>
        /// <param name="zipEntry"></param>

        private Employee VerifySignPhoto(ZipEntry zipEntry)
        {
            List<string> photoSuffixs = new List<string>() { "jpg", "png", "jpeg" };
            const long fileSize = 1024 * 1024 * 10;
            //判断后缀名是否是jpg或者png
            var PhotoNameList = zipEntry.Name.Split('.').ToList();
            string photoSuffix = PhotoNameList[PhotoNameList.Count - 1].ToLower();
            string empNo = zipEntry.Name.Split('.')[0]; //规定图片的文件名要用员工号
            if (!photoSuffixs.Contains(photoSuffix))
            {
                throw new ValidationException("压缩包内请上传png或jpg的图片".L10N());
            }
            if (zipEntry.Size > fileSize)
            {
                throw new ValidationException("单张图片大小不能大于10M".L10N());
            }
            var employee = GetEmployeeByCode(empNo);
            if (employee == null)
            {
                throw new ValidationException("文件名不正确，请将图片名称修改为员工号".L10N());
            }
            return employee;
        }

        /// <summary>
        /// 文件流转byte
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="zipEntry">压缩文件内容</param>
        /// <returns></returns>
        private byte[] StreamToByte(Stream stream, ZipEntry zipEntry)
        {
            MemoryStream fileStream = new MemoryStream();
            byte[] data = new byte[zipEntry.Size];
            //int size = Convert.ToInt32(zipEntry.Size);
            int size = stream.Read(data, 0, data.Length);
            if (size == 0)
            {
                throw new ValidationException("图片文件无内容".L10N());
            }
            fileStream.Write(data, 0, size);
            fileStream.Close();
            return data;
        }

        /// <summary>
        /// 返回存储到员工签名表的数据
        /// </summary>
        /// <param name="employee">员工信息</param>
        /// <param name="employeeSignList">未保存签名图片信息</param>
        /// <param name="bytePhoto">文件字节流</param>
        /// <param name="FileSuffix">文件后缀名</param>
        /// <returns></returns>
        private EmployeeSign SetEmployeeSignData(Employee employee, EntityList<EmployeeSign> employeeSignList, byte[] bytePhoto, string FileSuffix)
        {
            var employeeSign = new EmployeeSign();
            employeeSign.EmployeeId = employee.Id;
            employeeSign.PhotoName = employee.Code;
            employeeSign.FileSuffix = FileSuffix;
            employeeSign.SingPhoto = bytePhoto;
            var SignData = GetEmployeeSignByEmpId(employee.Id);
            var noSaveSignList = employeeSignList.Where(p => p.EmployeeId == employee.Id).ToList();
            //获取当前操作人的姓名
            var operEmployee = GetEmployeeByUserId(RT.IdentityId);
            if (SignData == null)
            {
                //当员工签名表没有相关的员工的签名记录且之前未保存的图片列表也没有员工的签名数据则版本号为V001且状态为启用
                if (noSaveSignList.Count == 0)
                {
                    employeeSign.State = true;
                    employeeSign.VersionNo = "V001";
                    employeeSign.EnableBy = operEmployee.Name;
                    employeeSign.EnableDate = DateTime.Now;
                }
                else
                {
                    employeeSign.State = false;
                    employeeSign.VersionNo = GetNewVersionNo(noSaveSignList[noSaveSignList.Count - 1].VersionNo);
                }

            }
            else
            {
                employeeSign.State = false;
                if (noSaveSignList.Count == 0)
                {

                    employeeSign.VersionNo = GetNewVersionNo(SignData.VersionNo);
                }
                else
                {
                    employeeSign.VersionNo = GetNewVersionNo(noSaveSignList[noSaveSignList.Count - 1].VersionNo);
                }
            }
            return employeeSign;
        }

        /// <summary>
        /// 根据员工ID获取签名图片数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual EmployeeSign GetEmployeeSignByEmpId(double Id)
        {
            return Query<EmployeeSign>().Where(p => p.EmployeeId == Id).OrderByDescending(c => c.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// 员工签名关系表主键ID
        /// </summary>
        /// <param name="Id"></param>
        public virtual void DelteSignData(double Id)
        {
            DB.Delete<EmployeeSign>().Where(p => p.Id == Id).Execute();
        }
        /// <summary>
        /// 更新签名表的状态 -禁用变为 启用 启用变为禁用
        /// </summary>
        /// <param name="Id"></param>
        public virtual void EnableSignState(double Id)
        {
            //启用
            var enableSignData = RF.GetById<EmployeeSign>(Id);
            enableSignData.State = true;
            enableSignData.EnableDate = DateTime.Now;
            var employee = GetEmployeeByUserId(RT.IdentityId);
            enableSignData.EnableBy = employee.Name;
            enableSignData.StopBy = "";
            enableSignData.StopDate = null;
            //禁用
            var deenableSignData = Query<EmployeeSign>().Where(p => p.State && p.EmployeeId == enableSignData.EmployeeId).ToList().FirstOrDefault();
            if (deenableSignData != null)
            {
                deenableSignData.State = false;
                deenableSignData.StopBy = employee.Name;
                deenableSignData.StopDate = DateTime.Now;
                deenableSignData.EnableBy = "";
                deenableSignData.EnableDate = null;
                RF.Save(deenableSignData);
            }
            RF.Save(enableSignData);
        }
        /// <summary>
        /// 获取新的版本号
        /// </summary>
        /// <returns></returns>
        private string GetNewVersionNo(string oldVersion)
        {
            //版本号为V001开始 每次+1 最多为V999
            string Version = "V";
            var VersionArr = oldVersion.Split('V');
            var Num = Convert.ToInt32(VersionArr[1]);
            Num = Num + 1;
            if (Num > 999)
            {
                throw new ValidationException("版本已超过上限，最大版本号为999".L10N());
            }
            if (Num > 0 && Num < 10)
            {
                Version = $"{Version}00{Num}";
            }
            else if (Num >= 10 && Num < 100)
            {
                Version = $"{Version}0{Num}";
            }
            else if (Num >= 100)
            {
                Version = $"{Version}{Num}";
            }
            return Version;
        }
        #endregion

        /// <summary>
        /// 员工维护的资源批量删除命令
        /// </summary>
        /// <param name="selectIds"></param>
        public virtual void EmployeeResourceDelCommand(List<double> selectIds)
        {
            var resourceList = selectIds.SplitContains(tempId =>
            {
                return Query<EmployeeResource>().Where(p => tempId.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            if (resourceList.Count == 0)
            {
                throw new ValidationException("数据异常，删除失败".L10N());
            }
            resourceList.ForEach(item =>
            {
                item.PersistenceStatus = PersistenceStatus.Deleted;
            });
            RF.Save(resourceList);
        }

        /// <summary>
        /// 获取在职员工
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetJobEmployees(PagingInfo pagingInfo, string key)
        {
            return DB.Query<Employee>().Where(p => (p.Code.Contains(key) || p.Name.Contains(key)) && p.EmployeeStatus == EmployeeStatus.Job).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取员工基础信息key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetBaseEmployeeInfo(string key)
        {
            return Query<Employee>().Where(p => p.EmployeeStatus == EmployeeStatus.Job && (p.Code.Contains(key) || p.Name.Contains(key)))
                .Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                }).ToList<BaseDataInfo>().ToList();
        }


        /// <summary>
        /// 获取员工基础信息key
        /// </summary>
        /// <param name="empId"></param
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<EmployeeResource> GetEmployeeResourceByEmpId(double empId, string key)
        {
            var query = Query<EmployeeResource>().Where(p => p.EmployeeId == empId);
            if (!key.IsNullOrEmpty())
                query.Where(p => p.Resource.Code.Contains(key) || p.Resource.Name.Contains(key));
           return  query.ToList(null,new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 查询人员条码化数据
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual List<Employee> GetEmployeeDatasPrintDatas(List<double> Ids)
        {
            List<Employee> employeeDatas = new List<Employee>();
            Ids.SplitDataExecute(Ids =>
            {
                var list = Query<Employee>()
                    .Where(x => Ids.Contains(x.Id))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                employeeDatas.AddRange(list);
            });
            return employeeDatas;
        }

        public virtual string SyncAndEmpJob()
        {
            //查询安灯区域下有但区域员工下没有的产线资源数据
            // 直接查询AndonEmpResourceView视图实体
            var q = Query<AndonEmpResourceView>().ToList();
            //Check.NotNullOrEmpty(q, nameof(q));
            if (q.Count == 0)
            {
                return "没有要执行的数据，现有安灯区域下有的产线区域员工下都有";
            }
            else {
                List<double> EmployeeIds = q.Select(p => p.EmployeeId).Distinct().ToList();
                //删除安灯区域下有但区域员工下没有的产线资源数据
                DB.Delete<EmployeeResource>().Where(p => EmployeeIds.Contains(p.EmployeeId)).Execute();

                //新增-save
                EntityList<EmployeeResource> savedData = new EntityList<EmployeeResource>();
                foreach (var item in q)
                {
                    var EmployeeResource = new EmployeeResource();
                    EmployeeResource.EmployeeId = item.EmployeeId;
                    EmployeeResource.ResourceId = item.ResourceId;
                    savedData.Add(EmployeeResource);
                }
                RF.Save(savedData);

                return"执行成功";
                //return q.IsNullOrEmpty() ? string.Empty : $"执行失败，错误信息: {q}";
            }
        }
    }
}