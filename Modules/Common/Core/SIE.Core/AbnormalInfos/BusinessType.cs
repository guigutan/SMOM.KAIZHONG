using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.AbnormalInfos
{

    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BusinessType
    {
      
        /// <summary>
        /// SPC
        /// </summary>

        [Label("SPC")]
        SPC = 0,

        /// <summary>
        /// 红牌管理
        /// </summary>
        [Label("红牌管理")]
        RedCard = 1,
    }
}
