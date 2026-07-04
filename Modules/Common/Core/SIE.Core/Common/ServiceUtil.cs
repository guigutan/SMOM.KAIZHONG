using SIE.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.Common
{
    /// <summary>
    /// 服务工具类
    /// </summary>
    public class ServiceUtil
    {
        /// <summary>
        /// Registrar assembly
        /// </summary>
        protected readonly static BasicConventionalRegistrar reg = new BasicConventionalRegistrar();
        /// <summary>
        /// 服务工具类
        /// </summary>
        public readonly static ServiceUtil Instance = new ServiceUtil();
        /// <summary>
        /// Gets an object from IOC container.
        /// </summary> 
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <returns>The instance object</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public T Resolve<T>()
        {
            var xx = reg.Resolve<T>();
            var cs = xx.GetType().GetConstructors().OrderBy(p => p.GetParameters().Length).LastOrDefault();
            if (cs == null)
            {
                return xx;
            }
            var ps = cs.GetParameters();
            List<object> args = new List<object>();
            foreach (var p in ps)
            {
                var method = this.GetType().GetMethod("Resolve").MakeGenericMethod(new Type[] { p.ParameterType });
                var pInst = method.Invoke(this, null);
                args.Add(pInst);
            }
            var _proxyGenerator = new Castle.DynamicProxy.ProxyGenerator();
            return (T)_proxyGenerator.CreateClassProxy(xx.GetType(), args.ToArray(), RemoteServiceInterceptor.Instance);
        }


        /// <summary>
        /// Checks whether given type is registered before.
        /// </summary>
        /// <param name="type">Type to check</param>
        public bool IsRegistered(Type type)
        {
            return reg.IsRegistered(type);
        }
    }
}
