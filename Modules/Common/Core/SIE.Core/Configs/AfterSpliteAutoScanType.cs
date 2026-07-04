using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Configs
{
    /// <summary>
    /// 拆分扫描选项
    /// </summary>
    public enum AfterSpliteAutoScanType
    {
        /// <summary>
        /// 不自动扫描
        /// </summary>
        [Label("不自动扫描")]
        NoScan = 0,

        /// <summary>
        /// 自动扫描新标签
        /// </summary>
        [Label("自动扫描新标签")]
        ScanNew = 1,

        /// <summary>
        /// 自动扫描旧标签
        /// </summary>
        [Label("自动扫描旧标签")]
        ScanOld = 2,
    }
}
