using SIE.XPCJ.Models.ConfigsSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussMove
{
    [Serializable]
    public class MoveSetting: ConfigSettingBase
    {
        /// <summary>
        /// 是否打印外标签
        /// </summary>
        public bool IsPrintOutCode { get; set; }
    }
}
