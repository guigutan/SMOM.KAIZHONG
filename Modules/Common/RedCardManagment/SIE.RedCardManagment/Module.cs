using SIE.Modules;
using SIE.RedCardManagment;
using SIE.RedCardManagment.RedcardEvent;
using System;
using System.Collections.Generic;
using System.Text;
[assembly:Module(typeof(Module))]
namespace SIE.RedCardManagment
{
    internal class Module : DomainModule
    {
        public override void Initialize(IApp app)
        {
            app.StartupCompleted += App_StartupCompleted;
        }
        /// <summary>
        /// app启动后事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            RedcardEventListener.Instance.Start();
        }
    }
}
