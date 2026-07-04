using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ManagedProperty;
using SIE.Rbac.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.ERPInterface.Common.Controller
{
    /// <summary>
    /// 下载基础控制器
    /// </summary>
    public class DownloadBaseController : DomainController
    {
        #region 声明变量

        /// <summary>
        /// 最大重试次数(根据项目实际需要，可以做成配置项或配置文件)
        /// </summary>
        public const int MAX_RETYR_COUNT = 5;

        /// <summary>
        /// 最大批处理数量(根据项目实际需要，可以做成配置项或配置文件)
        /// </summary>
        public const int MAX_BATCH_QUANTITY = 1000;

        #endregion

        #region 通用

        /// <summary>
        /// 初始化API接口环境
        /// </summary>
        /// <param name="invOrg"></param>
        public virtual void InitEnvironment(int invOrg)
        {
            //库存组织赋值
            if (invOrg > 0)
                RT.InvOrg = invOrg;
        }

        /// <summary>
        /// 保存ERP下载记录
        /// </summary>
        /// <param name="type">下载类型</param>
        /// <param name="dataCount">处理数量</param>
        /// <param name="successCount">成功数量</param>
        /// <param name="failCount">失败数量</param>
        /// <param name="direction">下载方向</param>
        /// <param name="mode">任务模式</param>
        /// <param name="state">处理状态</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="remark">备注</param>
        /// <param name="infId">中间表ID</param>
        /// <param name="erpKey">ERP主键</param>
        /// <param name="sourceData">来源数据</param>
        public virtual void SaveDownloadERPLog(JobType type, int dataCount, int successCount, int failCount, JobDirection direction, JobMode mode,
            ProcessState state, DateTime beginDate, DateTime endDate, OperationType operationType, string remark = null, string infId = null, string erpKey = null, string sourceData = null)
        {
            if (state == ProcessState.Processed && successCount <= 0) return;     //没有成功处理数据不记录LOG

            var downloadLog = new DownloadJobLog();
            downloadLog.JobType = type;
            downloadLog.DataCount = dataCount;
            downloadLog.SuccessCount = successCount;
            downloadLog.FailCount = failCount;
            downloadLog.JobDirection = direction;
            downloadLog.JobMode = mode;
            downloadLog.State = state;
            downloadLog.BeginDate = beginDate;
            downloadLog.EndDate = endDate;
            downloadLog.Remark = remark;
            downloadLog.InfId = infId;
            downloadLog.ErpKey = erpKey;
            downloadLog.SourceData = sourceData;
            downloadLog.OperationType = operationType;
            RF.Save(downloadLog);
        }

        /// <summary>
        /// 获取中间表未处理数据
        /// 根据变量MAX_BATCH_QUANTITY确定获取数量，默认值1000
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">附件表达式(AND逻辑)</param>
        /// <returns>返回未处理数据</returns>
        public virtual IEnumerable<T> GetUnprocessedDatas<T>(Expression<Func<T, bool>> expression = null)
            where T : DownloadBaseEntity
        {
            var q = DB.Query<T>();

            if (expression != null)
                q.Where(expression);

            q.Where(p => p.State == ProcessState.Unprocessed || p.State == ProcessState.Retry).OrderBy(p => p.LastUpdateDate);
            return q.ToList(new PagingInfo(1, MAX_BATCH_QUANTITY));
        }

        /// <summary>
        /// 更新中间表数据状态
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="processState">处理状态</param>
        public virtual void UpdateCuxDataState<T>(T entity, ProcessState processState = ProcessState.Unprocessed)
            where T : DownloadBaseEntity
        {
            //如失败，标记重试
            if (processState == ProcessState.Failed && entity.RetryCount < MAX_RETYR_COUNT)
            {
                entity.State = ProcessState.Retry;
                entity.RetryCount++;
            }
            else
                entity.State = processState;
            entity.ProcessDate = DateTime.Now;
            RF.Save(entity);
        }

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <typeparam name="T">键值类型</typeparam>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="dict">实体字典</param>
        /// <param name="key">键值</param>
        /// <returns>实体</returns>
        public virtual E GenerateEntity<T, E>(Dictionary<T, E> dict, T key)
            where E : DataEntity, new()
        {
            E entity;
            if (!dict.TryGetValue(key, out entity))
            {
                entity = new E();
                entity.GenerateId();
                dict.Add(key, entity);
            }

            return entity;
        }

        /// <summary>
        /// 构建嵌套字典
        /// </summary>
        /// <typeparam name="K1">一级索引</typeparam>
        /// <typeparam name="K2">二级索引</typeparam>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="datas">数据源</param>
        /// <param name="keyProperty1">一级索引实体属性</param>
        /// <param name="keyProperty2">二级索引实体属性</param>
        /// <returns></returns>
        public virtual Dictionary<K1, Dictionary<K2, T>> GenerateDictionarys<K1, K2, T>(IEnumerable<T> datas, IManagedProperty keyProperty1, IManagedProperty keyProperty2) where T : DownloadBaseEntity
        {
            var dataDicts = new Dictionary<K1, Dictionary<K2, T>>();
            datas.ForEach(p =>
            {
                //获取Key值
                var key1 = (K1)p.GetProperty(keyProperty1);
                var key2 = (K2)p.GetProperty(keyProperty2);

                //重构集合格式
                Dictionary<K2, T> tmp;
                if (dataDicts.TryGetValue(key1, out tmp))
                    tmp.Add(key2, p);
                else
                {
                    tmp = new Dictionary<K2, T>();
                    tmp.Add(key2, p);
                    dataDicts.Add(key1, tmp);
                }
            });

            return dataDicts;
        }

        /// <summary>
        /// 构建嵌套字典
        /// </summary>
        /// <typeparam name="K">索引</typeparam>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="datas">数据源</param>
        /// <param name="keyProperty">索引实体属性</param>
        /// <returns></returns>
        public virtual Dictionary<K, List<T>> GenerateDictionarys<K, T>(IEnumerable<T> datas, IManagedProperty keyProperty) where T : DownloadBaseEntity
        {
            var dataDicts = new Dictionary<K, List<T>>();
            datas.ForEach(p =>
            {
                //获取Key值
                var key = (K)p.GetProperty(keyProperty);

                //重构集合格式
                List<T> tmp;
                if (dataDicts.TryGetValue(key, out tmp))
                    tmp.Add(p);
                else
                {
                    tmp = new List<T>();
                    tmp.Add(p);
                    dataDicts.Add(key, tmp);
                }
            });

            return dataDicts;
        }

        /// <summary>
        /// 构建中间表数据主从关联关系
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <param name="master">主实体</param>
        /// <param name="details">子实体集合</param>
        public virtual void GenerateChildren<M, D>(M master, IEnumerable<D> details) where M : DownloadBaseEntity where D : DownloadBaseEntity
        {
            master.Children = details.OfType<DownloadBaseEntity>().ToList();
        }
        #endregion
    }
}
