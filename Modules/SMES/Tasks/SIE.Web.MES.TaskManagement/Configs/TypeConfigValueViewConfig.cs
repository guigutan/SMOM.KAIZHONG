using SIE.MES.TaskManagement.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Configs
{
    public class TypeConfigValueViewConfig:WebViewConfig<TypeConfigValue>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.Type).Show();
        }
    }
}
