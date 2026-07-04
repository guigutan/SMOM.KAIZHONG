using SIE.Common.Configs;
using SIE.Common.InvOrg;
using SIE.Common.Prints;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.EMS.DevicePurs;
using SIE.EMS.DevicePurs.Configs;
using SIE.Equipments.DataAuth;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIE.EMS.DataAuth
{
    /// <summary>
    /// 数据权限拦截器
    /// </summary>
    public static class EmsAuthInterceptor
    {
        /// <summary>
        /// 拦截事件
        /// </summary>
        public static void Intercept()
        {
            RepositoryDataProvider.Querying += RepositoryDataProvider_Querying;
        }

        private static void RepositoryDataProvider_Querying(object sender, QueryingEventArgs e)
        {
            if (DataAuths.LoadALl) return;

            var dp = sender as RepositoryDataProvider;

            //预算部门的权限过滤 支持多特性
            var entityDataAuthAttributeList = dp.Repository.EntityType.GetCustomAttributes<BudgetDepartmentAuthAttribute>();

            if (entityDataAuthAttributeList != null && entityDataAuthAttributeList.Any())
            {
                FilterByBudgetDepartment(e, dp, entityDataAuthAttributeList);
            }

            //业务部门的权限过滤 支持多特性
            var bussinessDepartmentAuthAttribute = dp.Repository.EntityType.GetCustomAttributes<BussinessDepartmentAuthAttribute>();

            if (bussinessDepartmentAuthAttribute != null && bussinessDepartmentAuthAttribute.Any())
            {
                FilterByBussinessDepartmentAuthAttribute(e, dp, bussinessDepartmentAuthAttribute);
            }

            //支持多特性
            var equipAccountAuthAttributeList = dp.Repository.EntityType.GetCustomAttributes<EquipAccountAuthAttribute>();

            if (equipAccountAuthAttributeList != null && equipAccountAuthAttributeList.Any())
            {
                //获取是否启用设备权限的配置项
                YesNo enableDevicePermissions;

                var configValue = ConfigService.GetConfig(new EnableDevicePermissionsConfig(), typeof(DevicePur));

                if (configValue == null || configValue.EnableDevicePermissions == null)
                {
                    enableDevicePermissions = new EnableDevicePermissionsConfig().DefaultValue.EnableDevicePermissions.Value;
                }
                else
                {
                    enableDevicePermissions = configValue.EnableDevicePermissions.Value;
                }

                var equipAccountAuthAttribute = equipAccountAuthAttributeList.FirstOrDefault();

                //启用设备权限才按设备权限过滤
                if (enableDevicePermissions == YesNo.Yes)
                {
                    FilterByEquipAccountPermissions(e.Args.Query, dp.Repository, equipAccountAuthAttribute, e.Args.Query.MainTable);
                }
            }
        }

        /// <summary>
        /// 按预算部门过滤数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="dp"></param>
        /// <param name="entityDataAuthAttributeList"></param>
        /// <exception cref="MissingMemberException"></exception>
        private static void FilterByBudgetDepartment(QueryingEventArgs e, RepositoryDataProvider dp,
            IEnumerable<BudgetDepartmentAuthAttribute> entityDataAuthAttributeList)
        {
            foreach (var dataAuthAttribute in entityDataAuthAttributeList)
            {
                var mainId = dp.Repository.EntityMeta.ManagedProperties.FindProperty(dataAuthAttribute.AuthIdProperty);

                if (mainId == null)
                {
                    throw new MissingMemberException("实体[{0}]找不到BudgetDepartmentAuthAttribute指定的AuthIdProperty属性[{1}]"
                        .FormatArgs(dp.Repository.EntityType.Name, dataAuthAttribute.AuthIdProperty));
                }

                var mainTable = e.Args.Query.MainTable;
                var mainIdColumn = mainTable.FindColumn(mainId);

                var f = QueryFactory.Instance;
                IQuery subQuery = CreateSubQuery(mainIdColumn, f);

                if (dataAuthAttribute.Nullable)
                {
                    e.Args.Query.Where = e.Args.Query.Where.And(f.Or(f.Constraint(mainIdColumn, f.Value(null)), f.Exists(subQuery)));
                }
                else
                {
                    e.Args.Query.Where = e.Args.Query.Where.And(f.Exists(subQuery));
                }
            }
        }

        /// <summary>
        /// 按设备与人员权限中的业务部门列表过滤数据
        /// </summary>
        /// <param name="e">查询实体前的事件的参数</param>
        /// <param name="dp">通用的仓库数据层实现</param>
        /// <param name="entityDataAuthAttributeList">数据权限标记列表</param>
        /// <exception cref="MissingMemberException"></exception>
        private static void FilterByBussinessDepartmentAuthAttribute(QueryingEventArgs e, RepositoryDataProvider dp,
            IEnumerable<BussinessDepartmentAuthAttribute> entityDataAuthAttributeList)
        {
            foreach (var dataAuthAttribute in entityDataAuthAttributeList)
            {
                var mainId = dp.Repository.EntityMeta.ManagedProperties.FindProperty(dataAuthAttribute.AuthIdProperty);

                if (mainId == null)
                {
                    throw new MissingMemberException("实体[{0}]找不到BussinessDepartmentAuthAttribute指定的AuthIdProperty属性[{1}]"
                        .L10nFormat(dp.Repository.EntityType.Name, dataAuthAttribute.AuthIdProperty));
                }

                var mainTable = e.Args.Query.MainTable;
                var mainIdColumn = mainTable.FindColumn(mainId);

                var f = QueryFactory.Instance;
                IQuery subQuery = CreateSubQueryUseDepartment(mainIdColumn, f);

                if (dataAuthAttribute.Nullable)
                {
                    e.Args.Query.Where = e.Args.Query.Where.And(f.Or(f.Constraint(mainIdColumn, f.Value(null)), f.Exists(subQuery)));
                }
                else
                {
                    e.Args.Query.Where = e.Args.Query.Where.And(f.Exists(subQuery));
                }
            }
        }

        private static IQuery CreateSubQueryUseDepartment(IColumnNode mainIdColumn, QueryFactory f)
        {
            var subQuery = DB.Query<DeviceUseDepartment>("dbp")
                 .Join<DevicePur>("dp", (x, y) => x.DevicePurId == y.Id
                    && !y.GetProperty(PhantomEntityExtension.IS_PHANTOMProperty))
                 .LeftJoin<DevicePur, UserInUserGroup>("uig", (a, b) => a.UserGroupId == b.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == RT.Identity.UserId || c.UserId == RT.Identity.UserId))
                 .ToQuery();

            var dataRepo = RF.Find<DeviceUseDepartment>();
            var _mainTable = f.Table(dataRepo, "dbp");
            var property = dataRepo.EntityMeta.ManagedProperties
                .FindProperty(DeviceUseDepartment.EnterpriseIdProperty.Name);
            IColumnNode enterpriseIdColumn = _mainTable.Column(property);
            subQuery.Where = f.And(subQuery.Where, f.Constraint(mainIdColumn, enterpriseIdColumn));

            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));

            return subQuery;
        }

        private static IQuery CreateSubQuery(IColumnNode mainIdColumn, QueryFactory f)
        {
            var subQuery = DB.Query<DeviceBudgetDepartment>("dbp")
                 .Join<DevicePur>("dp", (x, y) => x.DevicePurId == y.Id
                    && !y.GetProperty(PhantomEntityExtension.IS_PHANTOMProperty))
                 .LeftJoin<DevicePur, UserInUserGroup>("uig", (a, b) => a.UserGroupId == b.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == RT.Identity.UserId || c.UserId == RT.Identity.UserId))
                 .ToQuery();

            var dataRepo = RF.Find<DeviceBudgetDepartment>();
            var _mainTable = f.Table(dataRepo, "dbp");
            var property = dataRepo.EntityMeta.ManagedProperties
                .FindProperty(DeviceBudgetDepartment.EnterpriseIdProperty.Name);
            IColumnNode enterpriseIdColumn = _mainTable.Column(property);
            subQuery.Where = f.And(subQuery.Where, f.Constraint(mainIdColumn, enterpriseIdColumn));

            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));

            return subQuery;
        }

        /// <summary>
        /// 按权限过滤设备台账
        /// </summary>
        /// <param name="query"></param>
        /// <param name="repository"></param>
        /// <param name="dataAuthAttribute"></param>
        /// <param name="mainTable"></param>
        /// <exception cref="MissingMemberException"></exception>
        private static void FilterByEquipAccountPermissions(IQuery query, EntityRepository repository,
            EquipAccountAuthAttribute dataAuthAttribute, ITableSource mainTable)
        {
            #region Plus
            var f = QueryFactory.Instance;
            // 假删除数据
            var isPhantomColumn = query.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
            {
                query.Where = f.And(query.Where, isPhantomColumn.Equal(BooleanBoxes.False));
            }

            var userId = RT.Identity.UserId;
            // 找到当前用户或当前用户所在用户组的设备与人员权限
            var purIds = DB.Query<DevicePur>("dp").LeftJoin<UserInUserGroup>("uig", (dp, uig) => dp.UserGroupId == uig.UserGroupId)
                .Where<UserInUserGroup>((dp, uig) => dp.UserId == userId || uig.UserId == userId).Select<UserInUserGroup>((dp, uig) => new { dp.Id }).ToList<double>().ToList();

            if (purIds.Count == 0) return;


            // 权限下通过所有维度找到的设备台账Id(扩展业务逻辑只需要参考CreateSubQueryOfUserDepartment的写法)
            // 部门字段
            var useDepartmentId = repository.EntityMeta.ManagedProperties.FindProperty(dataAuthAttribute.UseDepartmentIdProperty);
            var useDepartmentIdColumn = mainTable.FindColumn(useDepartmentId);
            // 业务部门
            var usrDeptSql = CreateSubQueryOfUserDepartment(purIds, useDepartmentIdColumn, f);


            // 设备台账表Id字段
            var idProperty = repository.EntityMeta.ManagedProperties.FindProperty("Id");
            var idColumn = mainTable.FindColumn(idProperty);
            // 设备清单
            var accountSql = CreateSubQueryOfDeviceBill(purIds, idColumn, f);


            // 后续扩展仿照上面的写法，必须用ToQuery否则ToList会递归执行拦截器，然后将新的子页签iQuery加在下面的f.Or中
            // 连接表
            query.Where = query.Where.And(f.Or(f.Exists(usrDeptSql), f.Exists(accountSql)));
            #endregion

        }

        /// <summary>
        /// 查找责任部门用于exists查询
        /// </summary>
        /// <param name="purIds">权限id</param>
        /// <param name="mainIdColumn">设备台账的部门字段</param>
        /// <param name="f">数据工厂</param>
        /// <returns></returns>
        private static IQuery CreateSubQueryOfUserDepartment(List<double> purIds, IColumnNode mainIdColumn, QueryFactory f)
        {
            // 查询设备权限业务部门子表
            var subQuery = DB.Query<DeviceUseDepartment>("dbp").Where(dbp => purIds.Contains(dbp.DevicePurId)).ToQuery();
            var dataRepo = RF.Find<DeviceUseDepartment>();
            var _mainTable = f.Table(dataRepo, "dbp");// 业务部门子表

            // f.Constraint(mainIdColumn, enterpriseIdColumn)连接业务部门的部门ID和设备台账的使用部门ID作为Exist的条件
            IColumnNode enterpriseIdColumn = _mainTable.Column(DeviceUseDepartment.EnterpriseIdProperty);  // 业务部门的部门字段
            subQuery.Where = f.And(subQuery.Where, f.Constraint(mainIdColumn, enterpriseIdColumn));

            // 启用假删除
            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));

            return subQuery;
        }

        /// <summary>
        /// 按设备类别和设备型号过滤
        /// </summary>
        /// <param name="mainIdColumn"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        private static IQuery CreateSubQueryOfEquipType(IColumnNode mainIdColumn, QueryFactory f)
        {
            var subQuery = DB.Query<EquipModel>("em")
                .Join<EquipType>("et", (x, y) => x.EquipTypeId == y.Id && !y.GetProperty(PhantomEntityExtension.IS_PHANTOMProperty))
                .Join<DeviceType>("dt", (x, y) => ((y.TypeCategory == x.TypeCategory && y.EquipTypeId == null && x.GetProperty(InvOrgIdExtension.INV_ORG_IDProperty) == y.GetProperty(InvOrgIdExtension.INV_ORG_IDProperty))
                     || (y.EquipTypeId == x.Id && (string.IsNullOrEmpty(y.TypeCategory) || y.TypeCategory == x.TypeCategory) && x.GetProperty(InvOrgIdExtension.INV_ORG_IDProperty) == y.GetProperty(InvOrgIdExtension.INV_ORG_IDProperty)))
                     && !y.GetProperty(PhantomEntityExtension.IS_PHANTOMProperty))
                .Join<DeviceType, DevicePur>("dp", (x, y) => x.DevicePurId == y.Id
                     && !y.GetProperty(PhantomEntityExtension.IS_PHANTOMProperty))
                .LeftJoin<DevicePur, UserInUserGroup>("uig", (a, b) => a.UserGroupId == b.UserGroupId)
               .Where<DevicePur, UserInUserGroup>((x, y, z) => (y.UserId == RT.Identity.UserId || z.UserId == RT.Identity.UserId))
               .ToQuery();

            var dataRepo = RF.Find<EquipModel>();
            var _mainTable = f.Table(dataRepo, "em");
            IColumnNode idColumn = _mainTable.Column(EquipModel.IdProperty);

            subQuery.Where = subQuery.Where.And(f.Constraint(mainIdColumn, idColumn));

            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));

            return subQuery;
        }

        /// <summary>
        /// 按设备台账过滤
        /// </summary>
        /// <param name="purIds">权限id</param>
        /// <param name="mainIdColumn">设备台账id字段</param>
        /// <param name="f">数据工厂</param>
        /// <returns></returns>
        private static IQuery CreateSubQueryOfDeviceBill(List<double> purIds, IColumnNode mainIdColumn, QueryFactory f)
        {
            // 查找设备清单
            var subQuery = DB.Query<DeviceBill>("db").Where(db => purIds.Contains(db.DevicePurId)).ToQuery();
            var dataRepo = RF.Find<DeviceBill>();
            var _mainTable = f.Table(dataRepo, "db"); // 设备清单子表

            // f.Constraint(mainIdColumn, equipAccountIdColumn)连接设备清单的部门ID和设备台账的使用部门ID作为Exist的条件
            IColumnNode equipAccountIdColumn = _mainTable.Column(DeviceBill.EquipAccountIdProperty); // 设备清单子表的设备台账Id字段
            subQuery.Where = f.And(subQuery.Where, f.Constraint(mainIdColumn, equipAccountIdColumn));

            // 启用了假删除
            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));

            return subQuery;
        }


        /// <summary>
        /// 关联查询使用数据权限查询数据
        /// </summary>
        /// <param name="query">查询</param>
        /// <param name="equipAcountIdFieldName">设备台账ID字段名</param>        
        /// <returns></returns>
        public static IQuery QueryWithEquipAccountPermissions(this IQuery query, string equipAcountIdFieldName)
        {
            if (DataAuths.LoadALl)
            {
                return query;
            }

            //获取是否启用设备权限的配置项
            YesNo enableDevicePermissions;

            var configValue = ConfigService.GetConfig(new EnableDevicePermissionsConfig(), typeof(DevicePur));
            if (configValue == null || configValue.EnableDevicePermissions == null)
            {
                enableDevicePermissions = new EnableDevicePermissionsConfig().DefaultValue.EnableDevicePermissions.Value;
            }
            else
            {
                enableDevicePermissions = configValue.EnableDevicePermissions.Value;
            }

            //启用设备权限才按设备权限过滤
            if (enableDevicePermissions != YesNo.Yes)
            {
                return query;
            }

            var dataType = typeof(EquipAccount);

            //支持多特性
            var equipAccountAuthAttributeList = dataType.GetCustomAttributes<EquipAccountAuthAttribute>();

            if (equipAccountAuthAttributeList == null || !equipAccountAuthAttributeList.Any())
            {
                return query;
            }

            var equipAccountAuthAttribute = equipAccountAuthAttributeList.FirstOrDefault();

            //主表设备台账ID字段属性
            var equipAcountIdProperty = query.MainTable.EntityRepository.EntityMeta.ManagedProperties
               .FindProperty(equipAcountIdFieldName);

            if (equipAcountIdProperty == null)
            {
                throw new MissingMemberException("实体[{0}]找不到EquipAccountAuthAttribute指定的EquipModelIdProperty属性[{1}]"
                    .FormatArgs(query.MainTable.EntityRepository.EntityType.Name, equipAcountIdFieldName));
            }
            
            var equipAcountIdColumn = query.MainTable.FindColumn(equipAcountIdProperty);

            var f = QueryFactory.Instance;
            
            //设备台账子查询(按工厂权限一起过滤)
            var equipAcountSubQuery = CreateSubQueryOfEquipAccount(equipAcountIdColumn, f);

            var dataRepo = RF.Find(dataType);
            if (dataRepo == null)
            {
                throw new ValidationException("实体[{0}]找不到数据仓库".L10nFormat(dataType.Name));
            }

            var _mainTable = equipAcountSubQuery.MainTable;

            //设备
            FilterByEquipAccountPermissions(equipAcountSubQuery, dataRepo, equipAccountAuthAttribute, _mainTable);

            query.Where = query.Where.And(f.Exists(equipAcountSubQuery));

            return query;
        }

        /// <summary>
        /// Exists台账来过滤EDO业务数据
        /// </summary>
        /// <param name="mainIdColumn"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        private static IQuery CreateSubQueryOfEquipAccount(IColumnNode mainIdColumn, QueryFactory f)
        {
            // 做一次设备台账子查询，来帮助业务逻辑exist实现台账权限过滤(同时根据工厂来过滤)
            var subQuery = DB.Query<EquipAccount>("ea").Exists<EmployeeEnterprise>((ea, ee) => ee.Where(p => p.EnterpriseId == ea.FactoryId && p.EmployeeId == RT.IdentityId))
               .ToQuery();

            var dataRepo = RF.Find<EquipAccount>();
            var _mainTable = f.Table(dataRepo, "ea");

            IColumnNode idColumn = _mainTable.Column(Entity.IdProperty);

            subQuery.Where = subQuery.Where.And(f.Constraint(mainIdColumn, idColumn));

            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);

            if (isPhantomColumn != null)
            {
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));
            }

            return subQuery;
        }

    }
}
