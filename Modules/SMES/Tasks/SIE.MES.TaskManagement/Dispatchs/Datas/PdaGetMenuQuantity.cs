using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 主菜单数量显示
    /// </summary>
    [Serializable]
    public class PdaGetMenuQuantity
    {
        /// <summary>
        /// 安灯管理
        /// </summary>
        public decimal andon_manager { get; set; }

        /// <summary>
        /// 安灯响应
        /// </summary>
        public decimal andon_response { get; set; }

        /// <summary>
        /// 上料采集
        /// </summary>
        public decimal load_items_selection { get; set; }

        /// <summary>
        /// 产前准备
        /// </summary>
        public decimal production_prepare_index { get; set; }

        /// <summary>
        /// 可疑品处理
        /// </summary>
        public decimal suspect_product { get; set; }

        /// <summary>
        /// 装箱QC确认
        /// </summary>
        public decimal packing_qc_confirm { get; set; }

        /// <summary>
        /// 手动报工
        /// </summary>
        public decimal manual_report_index { get; set; }

        /// <summary>
        /// 扫码报工
        /// </summary>
        public decimal barcode_report_select { get; set; }

        /// <summary>
        /// 开机准备
        /// </summary>
        public decimal power_on_pre_index { get; set; }

        /// <summary>
        /// 副产品收货
        /// </summary>
        public decimal output_product { get; set; }
    }
}
