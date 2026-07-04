using SIE.ERPInterface.Common.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ERPInterface.Parameters
{
    public class SyncFactoryItemLabelToFactoryParameterViewConfig : WebViewConfig<SyncFactoryItemLabelToFactoryParameter>
    {
        protected override void ConfigView()
        {
            View.AddBehavior("SIE.Web.ERPInterface.Parameters.Scripts.SyncFactoryItemLabelToFactoryParameterBehavior");
        }

        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.ERPInterface.Parameters.Scripts.SyncFactoryItemLabelToFactoryParameterBehavior");
        }
    }
}
