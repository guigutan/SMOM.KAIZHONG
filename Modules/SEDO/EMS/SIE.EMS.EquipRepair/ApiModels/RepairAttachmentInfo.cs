using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 附件信息
    /// </summary>
    [Serializable]
    public class RepairAttachmentInfo
    {
        /// <summary>
        /// 附件ID
        /// </summary>
        public double? Id { get; set; }

        /// <summary>
        /// 文件名(需带扩展名)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 类型
        /// 0:报修
        /// 1:接单
        /// 2:派工
        /// 3:转派
        /// 4:开始维修
        /// 5:维修完成
        /// 6:交机确认
        /// 7:工程确认
        /// 8:维修暂停
        /// 9:继续维修
        /// 10:取消
        /// 11:强制关单
        /// 12:维修报告
        /// </summary>
        public int? Type { get; set; }
    }
}
