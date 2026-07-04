using System.Collections.Generic;

namespace SIE.Core.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static T DicToObject<T>(this Dictionary<string, object> dic) where T : class
        {
            //Type type = typeof(T);
            //var entity = Activator.CreateInstance(type) as T;

            //foreach (PropertyInfo pi in type.GetProperties())
            //{
            //    object value = DictionaryHelper.GetValueWhenNoException(dic, pi.Name);

            //    pi.SetValue(entity, (pi.GetType())value);

            //}
            //return entity;

            string str = Newtonsoft.Json.JsonConvert.SerializeObject(dic);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            return result;
        }
    }
}
