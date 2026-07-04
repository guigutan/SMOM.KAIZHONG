using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussRepairs
{
    [Serializable]
    public class RepairsSetting : ConfigSettingBase
    {
        public ChangeItemHandleMethod ChangeItemHandleMethod { get; set; } = ChangeItemHandleMethod.Recycle;
    }
}
