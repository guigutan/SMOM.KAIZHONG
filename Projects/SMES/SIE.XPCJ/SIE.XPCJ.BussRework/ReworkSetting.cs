using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP.Packing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussRework
{
    [Serializable]
    public class ReworkSetting : ConfigSettingBase
    {
        /// <summary>
        /// 置换后处理方式
        /// </summary>
        public ReplaceItemHandleMethod ReplaceItemHandleMethod { get; set; }
    }
}
