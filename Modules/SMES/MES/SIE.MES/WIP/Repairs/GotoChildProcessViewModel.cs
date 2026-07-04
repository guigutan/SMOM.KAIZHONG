using SIE.MES.WIP.Repairs;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Wip.Repairs
{
    /// <summary>
    /// 可选工序
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(PathName))]
    public class GotoChildProcessViewModel: GotoProcessViewModel
    {
    }
}
