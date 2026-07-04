using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SIE.Core.Common.IService;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using SIE.Services;

namespace SIE.Core.Common
{
    /// <summary>
    /// Registrar assembly
    /// </summary>
    public class BasicConventionalRegistrar
    {
        private static readonly WindsorContainer _container = new WindsorContainer();
        private bool IsAssemblyRegistered;
        private readonly object lockObj = new object();


        /// <summary>
        /// 注册程序集中满足约定的类
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public WindsorContainer RegisterAssembly(List<Assembly> assemblies)
        {

            _container.Register(Component.For<ServiceContainer, IServiceContainer, IServiceRegistrar, IServiceResolver>())
                .Register(Component.For<RemoteServiceInterceptor>());
            foreach (var assembly in assemblies)
            {
                //Transient
                _container.Register(
                    Classes.FromAssembly(assembly)
                           .IncludeNonPublicTypes()
                           .BasedOn<ITransientDependency>()
                           .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                           .WithService.Self()
                           .WithService.DefaultInterfaces()
                           .LifestyleTransient()
                );

                //Singleton
                _container.Register(
                    Classes.FromAssembly(assembly)
                           .IncludeNonPublicTypes()
                           .BasedOn<ISingletonDependency>()
                           .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                           .WithService.Self()
                           .WithService.DefaultInterfaces()
                           .LifestyleSingleton()
                );
            }
            return _container;
        }

        /// <summary>
        /// Gets an object from IOC container.
        /// </summary> 
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <returns>The instance object</returns>
        //[System.Diagnostics.DebuggerStepThrough]
        public T Resolve<T>()
        {
            TryRegisterFallback(typeof(T)); //发现时注册Fallback
            return _container.Resolve<T>();
        }

        void TryRegisterFallback(Type type)
        {
            lock (lockObj)
            {
                if (IsAssemblyRegistered)
                {
                    return;
                }
                IsAssemblyRegistered = true;

                var list = RT.GetAssemblies().ToList();
                RegisterAssembly(list);
            }
        }

        /// <summary>
        /// Checks whether given type is registered before.
        /// </summary>
        /// <param name="type">Type to check</param>
        public bool IsRegistered(Type type)
        {
            return _container.Kernel.HasComponent(type);
        }
    }
}
