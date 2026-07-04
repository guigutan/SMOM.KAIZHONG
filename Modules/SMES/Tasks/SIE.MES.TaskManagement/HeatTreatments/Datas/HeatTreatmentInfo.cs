using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.HeatTreatments.Datas
{
    /// <summary>
    /// 热处理进出炉信息
    /// </summary>
    [Serializable]
    public class HeatTreatmentInfo
    {
        /// <summary>
        /// 条码号
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 作业类型(1=入炉，2=出炉)
        /// </summary>
        public int? OperationType { get; set; }

        /// <summary>
        /// 作业时间(入炉时间)
        /// </summary>
        public DateTime? EnableTime1 { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DevId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Count00 { get; set; }


        /// <summary>
        /// 存储时间
        /// </summary>
        public DateTime? SvTime { get; set; }

        /// <summary>
        /// 时间整形字
        /// </summary>
        public int? SvTimeMs { get; set; }

        /// <summary>
        /// 设备实际名称
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// 设备异常代码
        /// </summary>
        public int? ErrNum { get; set; }

        /// <summary>
        /// 运行段
        /// </summary>
        public int? RunPro { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 工艺代码
        /// </summary>
        public string Rec { get; set; }

        /// <summary>
        /// 保存号
        /// </summary>
        public string SvId { get; set; }

        /// <summary>
        /// 温度允许上限值
        /// </summary>
        public decimal? TmpH { get; set; }

        /// <summary>
        /// 温度允许下限值
        /// </summary>
        public decimal? TmpL { get; set; }

        /// <summary>
        /// 目标温度值
        /// </summary>
        public decimal? Tmp { get; set; }

        /// <summary>
        /// CH1温度值
        /// </summary>
        public decimal? Tmp1 { get; set; }

        /// <summary>
        /// CH2温度值
        /// </summary>
        public decimal? Tmp2 { get; set; }

        /// <summary>
        /// CH3温度值
        /// </summary>
        public decimal? Tmp3 { get; set; }

        /// <summary>
        /// CH4温度值
        /// </summary>
        public decimal? Tmp4 { get; set; }

        /// <summary>
        /// 运行时间
        /// </summary>
        public int? RunTime { get; set; }

        /// <summary>
        /// 运行号
        /// </summary>
        public string RunId { get; set; }

        /// <summary>
        /// 层号
        /// </summary>
        public int? Layer00 { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string Card00 { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Type00 { get; set; }

        /// <summary>
        /// 允许温度上偏差值
        /// </summary>
        public decimal? TmpH1 { get; set; }

        /// <summary>
        /// 允许温度上极限值
        /// </summary>
        public decimal? TmpH2 { get; set; }

        /// <summary>
        /// CH1校正值
        /// </summary>
        public decimal? Ch1 { get; set; }

        /// <summary>
        /// CH2校正值
        /// </summary>
        public decimal? Ch2 { get; set; }

        /// <summary>
        /// CH3校正值
        /// </summary>
        public decimal? Ch3 { get; set; }

        /// <summary>
        /// CH4校正值
        /// </summary>
        public decimal? Ch4 { get; set; }

        /// <summary>
        /// 标记字段
        /// </summary>
        public int Flag { get; set; } = 0;

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? EnableTime { get; set; }

        /// <summary>
        /// 计划号
        /// </summary>
        public string PlanNum { get; set; }

        /// <summary>
        /// 生产号
        /// </summary>
        public string ProductNum { get; set; }

        /// <summary>
        /// 产部名称
        /// </summary>
        public string WorkId { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }
    }

}
