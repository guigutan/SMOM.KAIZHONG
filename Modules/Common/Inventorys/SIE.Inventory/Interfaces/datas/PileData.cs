using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory.Interfaces
{
    /// <summary>
    /// 垛表数据
    /// </summary>
    [Serializable]
    public class PileData
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 收货是否生成新的明细配置项
    /// </summary>
    public enum CollectAutoGenerateType
    {
        /// <summary>
        /// 不拆分明细
        /// </summary>
        [Label("不拆分明细")]
        NotGenerate,

        /// <summary>
        /// 自动拆分明细
        /// </summary>
        [Label("自动拆分明细")]
        AutoGenerate,

        /// <summary>
        /// 提醒是否拆分明细
        /// </summary>
        [Label("提醒是否拆分明细")]
        AskGenerate,

    }
}
