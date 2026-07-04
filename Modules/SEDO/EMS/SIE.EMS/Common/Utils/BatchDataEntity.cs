using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Common.Utils
{
    /// <summary>
    /// 批量生成实体的ID帮助类
    /// </summary>
    public static class BatchDataEntity
    {        /// <summary>
             /// 批量生成实体ID
             /// </summary>
             /// <typeparam name="T">实体类型</typeparam>
             /// <param name="num">个数</param>
        public static List<double> GetBatchEntityId<T>(int num) where T : DataEntity
        {
            List<double> ids = new List<double>();
            var repo = RF.Find<T>();
            var id = repo.GetDoubleKeyNextId();
            double digits = 0;
            for (int i = 0; i < num; i++)
            {
                if (digits > 999)
                {
                    digits = 0;
                    id = repo.GetDoubleKeyNextId();
                }
                ids.Add(id + digits / 1000);
                digits++;
            }

            return ids;
        }
    }

    /// <summary>
    /// 快速生成实体ID
    /// </summary>
    public class QuickGenerateIdHelper
    {
        /// <summary>
        /// ID集合
        /// </summary>
        private Dictionary<Type, List<double>> NewIdDic { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuickGenerateIdHelper()
        {
            NewIdDic = new Dictionary<Type, List<double>>();
        }

        /// <summary>
        /// 生成ID
        /// </summary>
        /// <returns>返回ID</returns>
        public double GetNewId<T>() where T : DataEntity, new()
        {
            List<double> memoryIds = null;
            if (!NewIdDic.TryGetValue(typeof(T), out memoryIds))
            {
                memoryIds = new List<double>();
                NewIdDic[typeof(T)] = memoryIds;
            }

            if (memoryIds.Count == 0)
            {
                List<double> newIds = RT.Service.Resolve<ToolController>().GetBatchEntityId<T>(999);
                memoryIds.AddRange(newIds);
            }

            double newId = memoryIds[0];
            memoryIds.RemoveAt(0);

            return newId;
        }
    }
}
