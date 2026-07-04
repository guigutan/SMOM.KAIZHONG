using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.DeviceIOTParas.Enums
{
    /// <summary>
    /// 来源
    /// </summary>
    [Label("来源")]
    public enum FromType
    {
        /// <summary>
        /// 自建
        /// </summary>
        [Label("自建")]
        MyCreate = 1,

        /// <summary>
        /// 接口
        /// </summary>
        [Label("接口")]
        Interface = 2,
    }
}
