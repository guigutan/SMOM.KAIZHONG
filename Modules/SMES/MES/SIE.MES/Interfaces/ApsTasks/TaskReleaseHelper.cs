using SIE.Core.Common.Controllers;
using SIE.Core.Common.Models;
using SIE.Domain;
using SIE.EventMessages.Release;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 工单下达帮助类
    /// </summary>
    public static class TaskReleaseHelper
    {

        /// <summary>
        /// 设置下达结果属性值
        /// </summary>
        /// <param name="releasePlanResult">下达结果对象</param>
        /// <param name="resultFlag">下达结果布尔值</param>
        /// <param name="message">下达结果消息</param>        
        public static void SetReleasePlanMainResult(ReleasePlanResult releasePlanResult, bool resultFlag, string message)
        {
            if (releasePlanResult == null)
            {
                throw new ArgumentNullException(nameof(releasePlanResult));
            }

            releasePlanResult.IsSuccess = resultFlag;
            if (!message.IsNullOrWhiteSpace())
            {
                releasePlanResult.Message += message;
            }            
        }

        /// <summary>
        /// 创建下达结果明细对象
        /// </summary>
        /// <param name="detailId">明细Id</param>
        /// <param name="processTechOrderCode">工艺单编号</param>
        /// <param name="message">结果信息</param>
        /// <param name="workOrderNo">工单号</param>
        /// <returns>下达结果明细对象</returns>
        public static ReleaseDetailResult CreateReleaseDetailResult(string detailId, string processTechOrderCode, string message, string workOrderNo)
        {
            var curDetailResult = new ReleaseDetailResult();
            curDetailResult.DetailId = detailId;
            curDetailResult.ProcessTechOrderCode = processTechOrderCode;
            curDetailResult.Message = message;
            curDetailResult.WorkOrder = workOrderNo;
            return curDetailResult;
        }

        /// <summary>
        /// 批量设置实体的ID
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityList">实体列表</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void BatchSetIds<T>(IEnumerable<T> entityList) where T : DataEntity
        {
            if (entityList == null)
            {
                throw new ArgumentNullException(nameof(entityList));
            }

            if (!entityList.Any())
            {
                //无数据，返回
                return;
            }

            var ids = RT.Service.Resolve<CommonController>()
                .GetBatchIdList<T>(entityList.Count());

            var idGetter = new EntityIdGetter(ids);
            foreach (T entity in entityList)
            {
                entity.Id = idGetter.GetNextId();
            }
        }
    }
}
