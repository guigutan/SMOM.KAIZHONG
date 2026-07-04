using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 开机准备：上模扫描标签
    /// </summary>
    [Serializable]
    public class Pda_PreStartupSetupScanEquipAccountInfo
    {

        /// <summary>
        ///模具编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 扫描时间
        /// </summary>
        public DateTime ScanTime { get; set; }

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 扫描时间
        /// </summary>
        public string ScanTimeStr { get { return ScanTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
    }
}
