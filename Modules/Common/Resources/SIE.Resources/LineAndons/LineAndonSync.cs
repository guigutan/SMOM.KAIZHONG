using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.LineAndons
{
    /// <summary>
    /// 产品与安灯区域同步
    /// </summary>
    [Label("产品与安灯区域")]
    public class LineAndonSync : WipResources.ISyncRsource
    {
        public string SyncResource()
        {

            return RT.Service.Resolve<LineAndonController>().SyncLineAndon();
        }
    }
}
