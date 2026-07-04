using SIE.XPCJ.Models.ConfigsSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussInspects
{
    [Serializable]
    public class InspectsSetting : ConfigSettingBase
    {
        /// <summary>
        /// 刷新时间
        /// </summary>
        public string ReflashTime { get; set; }
    }
}
