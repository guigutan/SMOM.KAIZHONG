using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.WIP.Packing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussNewPackage
{
    [Serializable]
    public class NewPackageSetting : ConfigSettingBase
    {
        /// <summary>
        /// 打印方式
        /// </summary>
        public PrintMode PrintMode { get; set; }
    }
}
