using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.InvOrg;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SIE.Core.Common.Controllers
{
    /// <summary>
    /// 通用数据查询控制器
    /// </summary>
    public partial class CommonController : DomainController
    {
        /// <summary>
        /// 查询超过5W条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual EntityList<T> CriteriaDatas<T>(Expression<Func<T, bool>> exp) where T : DataEntity
        {
            PagingInfo pagingInfo = new PagingInfo(1, 50000);
            EntityList<T> list = new EntityList<T>();
            var index = 50000;
            while (index == 50000)
            {
                var query = Query<T>();
                if (exp != null)
                    query.Where(exp);
                var des = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                list.AddRange(des);
                pagingInfo.PageNumber += 1;
                index = des.Count;
            }
            return list;
        }

        /// <summary>
        /// 获取实体数据集合（自动加载视图属性）
        /// Expression不支持序列号，仅支持控制器调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="exp">条件表达式</param> 
        /// <returns>实体数据集合</returns>
        public virtual EntityList<T> GetDatas<T>(Expression<Func<T, bool>> exp) where T : DataEntity
        {
            return GetDatas<T>(exp, null, new EagerLoadOptions().LoadWithViewProperty(), null);
        }

        /// <summary>
        /// 获取实体数据集合（自动加载视图属性）
        /// Expression不支持序列号，仅支持控制器调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="exp">条件表达式</param>
        /// <param name="pagingInfo">分页参数</param> 
        /// <returns>实体数据集合</returns>
        public virtual EntityList<T> GetDatas<T>(Expression<Func<T, bool>> exp, PagingInfo pagingInfo) where T : DataEntity
        {
            return GetDatas<T>(exp, pagingInfo, new EagerLoadOptions().LoadWithViewProperty(), null);
        }

        /// <summary>
        /// 获取实体数据集合
        /// Expression不支持序列号，仅支持控制器调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="exp">条件表达式</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="eagerLoad">贪婪加载参数</param> 
        /// <returns>实体数据集合</returns>
        public virtual EntityList<T> GetDatas<T>(Expression<Func<T, bool>> exp, PagingInfo pagingInfo, EagerLoadOptions eagerLoad) where T : DataEntity
        {
            return GetDatas<T>(exp, pagingInfo, eagerLoad, null);
        }

        /// <summary>
        /// 获取实体数据集合
        /// Expression不支持序列号，仅支持控制器调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="exp">条件表达式</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="eagerLoad">贪婪加载参数</param>
        /// <param name="orderInfos">排序参数</param>
        /// <returns>实体数据集合</returns>
        public virtual EntityList<T> GetDatas<T>(Expression<Func<T, bool>> exp, PagingInfo pagingInfo, EagerLoadOptions eagerLoad, List<OrderInfo> orderInfos) where T : DataEntity
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var query = Query<T>();
                if (exp != null)
                    query.Where(exp);
                if (orderInfos != null && orderInfos.Count > 0)
                    query.OrderBy(orderInfos);
                if (pagingInfo == null)
                    return query.ToList(null, eagerLoad);
                return query.ToList(pagingInfo, eagerLoad);
            }
        }

        /// <summary>
        /// 获取实体数据（自动加载视图属性）
        /// Expression不支持序列号，仅支持控制器调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="exp">条件表达式</param> 
        /// <returns>实体数据集合</returns>
        public virtual T GetData<T>(Expression<Func<T, bool>> exp) where T : DataEntity
        {
            return GetData<T>(exp, new EagerLoadOptions().LoadWithViewProperty(), null);
        }

        /// <summary>
        /// 获取实体数据
        /// Expression不支持序列号，仅支持控制器调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="exp">条件表达式</param>
        /// <param name="eagerLoad">贪婪加载</param>
        /// <returns>实体数据集合</returns>
        public virtual T GetData<T>(Expression<Func<T, bool>> exp, EagerLoadOptions eagerLoad) where T : DataEntity
        {
            return GetData<T>(exp, eagerLoad, null);
        }

        /// <summary>
        /// 获取实体数据
        /// Expression不支持序列号，仅支持控制器调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="exp">条件表达式</param>
        /// <param name="eagerLoad">贪婪加载参数</param>
        /// <param name="orderInfos">排序参数</param>
        /// <returns>实体数据集合</returns>
        public virtual T GetData<T>(Expression<Func<T, bool>> exp, EagerLoadOptions eagerLoad, List<OrderInfo> orderInfos) where T : DataEntity
        {
            var query = Query<T>();
            if (exp != null)
                query.Where(exp);
            if (orderInfos != null && orderInfos.Count > 0)
                query.OrderBy(orderInfos);
            return query.FirstOrDefault(eagerLoad);
        }

        /// <summary>
        /// 判断实体是否存在
        /// Expression不支持序列号，仅支持控制器调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="exp">条件表达式</param>
        /// <returns>实体数据集合</returns>
        public virtual bool IsExistData<T>(Expression<Func<T, bool>> exp) where T : DataEntity
        {
            var query = Query<T>();
            if (exp != null)
                query.Where(exp);
            return query.Count() > 0;
        }

        /// <summary>
        /// 获取单号
        /// example:   
        /// GetNo WorkOrder ("工单单号"); 
        /// 错误提示：未配置工单单号生成规则，请配置！
        /// 错误提示已做多语言翻译
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="msg">提示信息</param>  
        /// <exception cref="ValidationException">规则未找到或者未配置</exception>
        /// <returns>单号</returns>
        public virtual string GetNo<T>(string msg) where T : Entity
        {
            double ruleId = GetNoConfigRuleId<T>(msg);
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(ruleId, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取单号列表
        /// GetNo WorkOrder("工单单号");
        /// 错误提示：未配置工单单号生成规则，请配置！ 
        /// 错误提示已做多语言翻译
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="msg">提示信息</param>
        /// <param name="count">获取单号数量</param>
        /// <exception cref="ValidationException">规则未找到或者未配置</exception>
        /// <returns>单号列表</returns>
        public virtual List<string> GetNos<T>(int count, string msg) where T : Entity
        {
            double ruleId = GetNoConfigRuleId<T>(msg);
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(ruleId, count).ToList();
        }

        /// <summary>
        /// 获取单号配置规则ID
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="msg">提示信息</param>
        /// <exception cref="ValidationException">规则未找到或者未配置</exception>
        /// <returns>规则ID</returns>
        private double GetNoConfigRuleId<T>(string msg) where T : Entity
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(T));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未配置{0}生成规则，请配置！".L10nFormat(msg.L10N()));
            return config.BacodeRule.Id;
        }

        /// <summary>
        /// 批量保存：设置实体ID和创建人、组织数据並保存
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="entities">数据</param>
        public virtual void BatchSave<T>(EntityList<T> entities) where T : DataEntity
        {
            var setting = SIE.Domain.ORM.RdbDataProvider.Get(RF.Find<T>()).DbSetting;
            if (!setting.IsOracleDbServer())
            {
                RF.Save(entities);
                return;
            }
            var newEntitys = entities.Where(p => p.PersistenceStatus == PersistenceStatus.New).AsEntityList();
            SIE.Common.Domain.BulkSaver.SetBatchEntityId(newEntitys);
            newEntitys.ForEach(entity =>
            {
                if (entity.PersistenceStatus == PersistenceStatus.New)
                {
                    entity.CreateBy = RT.IdentityId;
                    entity.CreateDate = DateTime.Now;
                    entity.UpdateBy = RT.IdentityId;
                    entity.UpdateDate = DateTime.Now;
                    InvOrgIdExtension.SetInvOrgId(entity, RT.InvOrg);
                }
            });

            RF.BatchInsert(newEntitys);
            newEntitys.MarkSaved();
            RF.Save(entities);
        }

        /// <summary>
        /// 批量保存：设置实体ID和创建人、组织数据並保存
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="isSetBatchEntityId">是否批量生成ID</param>
        /// <param name="entities">数据</param>
        public virtual void BatchInsertSave<T>(EntityList<T> entities, bool isSetBatchEntityId = true) where T : DataEntity
        {
            var newEntitys = entities.AsEntityList();

            newEntitys.ForEach(entity =>
            {
                entity.CreateBy = RT.IdentityId;
                entity.CreateDate = DateTime.Now;
                entity.UpdateBy = RT.IdentityId;
                entity.UpdateDate = DateTime.Now;
                InvOrgIdExtension.SetInvOrgId(entity, RT.InvOrg);
            });

            var setting = SIE.Domain.ORM.RdbDataProvider.Get(RF.Find<T>()).DbSetting;
            if (newEntitys.Any())
            {
                if (isSetBatchEntityId && newEntitys.Any(f => f.Id == 0))
                    SIE.Common.Domain.BulkSaver.SetBatchEntityId(newEntitys);

                if (setting.IsSqlserverDbServer())
                {
                    int size = 100;
                    //对于SQLSERVER，框架最大支持数据表20个字段，100条数据一批
                    //如果字段比20多，批次数量要相对要减少，防止超过2100报错
                    var count = RF.Find<T>().TableInfo.Columns.Count;
                    if (count > 20)
                    {
                        size = 100 * 20 / count;
                    }
                    RF.BatchInsert(newEntitys, size);
                }
                else
                {                                     
                    RF.BatchInsert(newEntitys);
                }
            }
        }

        /// <summary>
        /// 批量获取实体的Id
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="count">Id数量</param>
        /// <returns>Id列表</returns>
        /// <remarks>循环生成数据时，不要用Id=GenerateId()</remarks>
        public virtual List<double> GetBatchIdList<T>(int count) where T : DataEntity
        {
            EntityRepository entityRepository = RepositoryFactory.Find<T>();
            return entityRepository.BatchGetDoubleKeyNextId(count);
        }

        /// <summary>
        /// 获取枚举类型
        /// </summary>
        /// <param name="category">单体工序标识</param>
        /// <returns></returns>
        public virtual List<T> GetEnumByCategory<T>(string category) where T : Enum
        {
            List<T> enums = new List<T>();
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<CategoryAttribute>();
                if (attribute?.Category == category)
                {
                    enums.Add((T)field.GetValue(null));
                }
            }
            return enums;
        }
    }
}