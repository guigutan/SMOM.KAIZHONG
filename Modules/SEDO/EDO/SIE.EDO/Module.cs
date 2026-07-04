using SIE.EDO;
using SIE.Modules;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: Module(typeof(Module))]

namespace SIE.EDO
{
    /// <summary>
    /// 模块定义
    /// </summary>
    class Module : DomainModule
    {
        /// <summary>
        /// 模块的初始化方法
        /// </summary>
        /// <param name="app">应用程序对象</param>
        public override void Initialize(IApp app)
        {
        }

    }
}
