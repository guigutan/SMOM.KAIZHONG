using SIE.Domain.Validation;
using System;
using System.Collections.Generic;

namespace SIE.Core.Common
{
    /// <summary>
    /// 字典辅助类
    /// </summary>
    public static class DictionaryHelper
    {
        /// <summary>
        /// 获取字典中的值，不存在key时抛中文异常
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T2 GetValueWhenNoException<T1, T2>(Dictionary<T1, T2> dic, T1 key)
        {
            if (!dic.TryGetValue(key, out T2 result))
                throw new ValidationException("字典中不存在{0}".L10nFormat(key.ToString()));
            else
                return result;
        }
    }
}
