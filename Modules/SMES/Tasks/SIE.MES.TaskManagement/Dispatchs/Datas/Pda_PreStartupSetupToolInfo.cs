using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 获取检具信息
    /// </summary>
    [Serializable]
    public class Pda_PreStartupSetupToolInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
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
        /// 扫描时间
        /// </summary>
        public DateTime ScanTime { get; set; }


        /// <summary>
        /// 扫描时间
        /// </summary>
        public string ScanTimeStr { get { return ScanTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

    }
}
