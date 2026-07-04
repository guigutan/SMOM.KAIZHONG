using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    [RootEntity,Serializable]
    [ConditionQueryType(typeof(ProcessingOutboundSelectCriteria))]
    [Label("委外发货明细")]
    public class ProcessingOutboundSelect: ProcessingOutbound
    {
    }

    internal class ProcessingOutboundConfig : EntityConfig<ProcessingOutbound>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("PROC_OUT_OUTBOUND").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
