using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Dock.DockGantt
{
    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("月台计划")]
    public partial class DockGantt : DataEntity
    {
    }
}
