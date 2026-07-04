using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EventMessages
{
    /// <summary>
    /// ERP通信共用数据类
    /// </summary>
    [Serializable]
    public class ErpInfoData
    {

        /// <summary>
        /// ErpKey(主键)
        /// </summary>
        public string ErpKey { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 中间表Key
        /// </summary>
        public string Infkey { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
    }

    /// <summary>
    /// 扩展
    /// </summary>
    public static class Collections
    {
        /// <summary>
        /// 以最后更新时间升序排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static List<T> OrderByLastUpdateDate<T>(this List<T> datas) where T : ErpInfoData
        {
            return datas.OrderBy(p => p.LastUpdateDate).ToList();
        }
    }
}
