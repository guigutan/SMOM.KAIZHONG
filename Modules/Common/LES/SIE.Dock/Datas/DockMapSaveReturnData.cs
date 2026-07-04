using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.Datas
{
    /// <summary>
    /// 预约图保存数据返回
    /// </summary>
    [Serializable]
    public class DockMapSaveReturnData
    {       
        /// <summary>
        /// 报错信息（空代表是成功）
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 报错需要点亮的格子
        /// </summary>
        public List<string> Codes { get; set; } = new List<string>();
    }
}
