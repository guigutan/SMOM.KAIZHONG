using Newtonsoft.Json;
using System;

namespace SIE.Core.Common
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 字符串反序列化为实体对象(触发属性变更事件)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToJsonObjectCore<T>(this string json)
        {
            if (null == json)
            {
                throw new ArgumentNullException(nameof(json));
            }
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        /// <summary>
        /// 实体对象反序列化为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
