using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.Utils;
using System;
using System.Reflection;
using System.Linq;

namespace SIE.DataAuth
{
    /// <summary>
    /// 数据权限拦截器
    /// </summary>
    public static class DataAuthInterceptor
    {/// <summary>
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
            var entityDataAuthAttributeList = dp.Repository.EntityType.GetCustomAttributes<EntityDataAuthAttribute>(); //支持多特性
            if (entityDataAuthAttributeList == null || !entityDataAuthAttributeList.Any()) return;
            foreach (var entityDataAuthAttribute in entityDataAuthAttributeList)
            {
                var mainId = dp.Repository.EntityMeta.ManagedProperties.FindProperty(entityDataAuthAttribute.AuthIdProperty);
                if (mainId == null)
                    throw new MissingMemberException("实体[{0}]找不到EntityDataAuthAttribute指定的AuthIdProperty属性[{1}]"
                        .FormatArgs(dp.Repository.EntityType.Name, entityDataAuthAttribute.AuthIdProperty));

                var mainTable = e.Args.Query.MainTable;
                var mainIdColumn = mainTable.FindColumn(mainId);

                Type authType = entityDataAuthAttribute.AuthType;

                var f = QueryFactory.Instance;
                IQuery subQuery = CreateSubQuery(mainIdColumn, authType, f);

                if (entityDataAuthAttribute.Nullable)
                {
                    e.Args.Query.Where = e.Args.Query.Where.And(f.Or(f.Constraint(mainIdColumn, f.Value(null)), f.Exists(subQuery)));
                }
                else
                {
                    e.Args.Query.Where = e.Args.Query.Where.And(f.Exists(subQuery));
                }
            }
        }

        private static IQuery CreateSubQuery(IColumnNode mainIdColumn, Type authType, QueryFactory f)
        {
            var authRepo = RF.Find(authType);
            var employeeAuthAttribute = authType.GetCustomAttribute<EmployeeAuthAttribute>();

            string employeeIdString = employeeAuthAttribute?.EmployeeIdProperty ?? "EmployeeId";
            string authIdString = employeeAuthAttribute?.AuthIdProperty ?? "AuthId";

            var employeeId = authRepo.EntityMeta.ManagedProperties.FindProperty(employeeIdString);
            if (employeeId == null)
                throw new MissingMemberException("实体[{0}]找不到属性[{1}],请检查EmployeeAuthAttribute配置"
                    .L10nFormat(authType.Name, employeeIdString));

            var authId = authRepo.EntityMeta.ManagedProperties.FindProperty(authIdString);
            if (authId == null)
                throw new MissingMemberException("实体[{0}]找不到属性[{1}],请检查EmployeeAuthAttribute配置"
                    .L10nFormat(authType.Name, authIdString));


            var subQuery = f.Query(authRepo);
            subQuery.Selection = f.Literal("1");
            var authIdColumn = subQuery.MainTable.FindColumn(authId);
            var employeeColumn = subQuery.MainTable.FindColumn(employeeId);
            subQuery.Where = f.And(f.Constraint(mainIdColumn, authIdColumn), f.Constraint(employeeColumn, RT.IdentityId));

            var isPhantomColumn = subQuery.MainTable.FindColumn(PhantomEntityExtension.IS_PHANTOMProperty);
            if (isPhantomColumn != null)
                subQuery.Where = f.And(subQuery.Where, isPhantomColumn.Equal(BooleanBoxes.False));
            return subQuery;
        }

        /// <summary>
        /// 关联查询使用数据权限查询数据
        /// </summary>
        /// <param name="query">查询</param>
        /// <param name="Nullable">控制业务权限的属性字段是否可以为空</param>        
        /// <param name="dataType">实体</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public static IQuery QueryWithDataAuth(this IQuery query, bool Nullable, Type dataType, string alias)
        {
            if (DataAuths.LoadALl)
            {
                return query;
            }

            var entityDataAuthAttribute = dataType.GetCustomAttribute<EntityDataAuthAttribute>();
            if (entityDataAuthAttribute == null)
            {
                return query;
            }

            var f = QueryFactory.Instance;

            var dataRepo = RF.Find(dataType);
            if (dataRepo == null)
            {
                throw new ValidationException("实体[{0}]找不到数据仓库".L10nFormat(dataType.Name));
            }

            var property = dataRepo.EntityMeta.ManagedProperties
                .FindProperty(entityDataAuthAttribute.AuthIdProperty);

            if (property == null)
            {
                throw new MissingMemberException("实体[{0}]找不到EntityDataAuthAttribute指定的AuthIdProperty属性[{1}]".L10nFormat(dataRepo.EntityMeta.EntityType.Name, entityDataAuthAttribute.AuthIdProperty));
            }

            var _mainTable = f.Table(dataRepo, alias);

            IColumnNode mainIdColumn = _mainTable.Column(property);

            Type authType = entityDataAuthAttribute.AuthType;
            var subQuery = CreateSubQuery(mainIdColumn, authType, f);

            if (Nullable)
            {
                query.Where = query.Where.And(f.Or(f.Constraint(mainIdColumn, f.Value(null)), f.Exists(subQuery)));
            }
            else
            {
                query.Where = query.Where.And(f.Exists(subQuery));
            }

            return query;
        }
    }
}
