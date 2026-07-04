using Newtonsoft.Json;
using SIE.Aop;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SIE.Core.Common
{
    /// <summary>
    /// 数据采集监控
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed  class DataCollectionAttribute : BaseInterceptorAttribute
    {
        /// <summary>
        /// |||
        /// </summary>
        public static readonly string Separator = "|||";

        /// <summary>
        /// (|)
        /// </summary>
        public static readonly string InputSeparator = "(|)";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public override Action Do(global::Castle.DynamicProxy.IInvocation invocation, Action action)
        {
            return () =>
            {
                action.Invoke();
#if DEBUG
                var data = invocation;
                string tipMsg = string.Empty;
                if (data.Arguments != null && data.Arguments.Length > 0)
                {
                    List<string> contentList = new List<string>();
                    for (int i = 0; i < data.Arguments.Length; i++)
                    {
                        contentList.Add(JsonConvert.SerializeObject(data.Arguments[i]));
                    }

                    string paraStr = string.Join(InputSeparator, contentList);
                    tipMsg += (paraStr + Separator);
                }

                if (data.ReturnValue != null)
                {
                    string returnValue = JsonConvert.SerializeObject(data.ReturnValue);
                    tipMsg += returnValue;
                }

                if (!string.IsNullOrEmpty(tipMsg))
                {
                    Debug.WriteLine(invocation.Method.Name + Separator + tipMsg);
                }
#endif
            };
        }
    }
}