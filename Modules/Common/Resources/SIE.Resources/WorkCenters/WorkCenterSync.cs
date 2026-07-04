using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.WorkCenters
{
    /// <summary>
    /// 工作中心同步
    /// </summary>
    [Label("工作中心")]
    public class WorkCenterSync : WipResources.ISyncRsource
    {
        public string SyncResource()
        {
            return RT.Service.Resolve<WorkCenterController>().SyncWorkCenter();
        }
    }
}
