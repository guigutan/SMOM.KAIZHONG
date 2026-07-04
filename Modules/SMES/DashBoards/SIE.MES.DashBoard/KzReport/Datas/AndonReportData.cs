using Org.BouncyCastle.Asn1.Mozilla;
using SIE.Andon.Andons.Enum;
using SIE.Items.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 安灯
    /// </summary>
    [Serializable]
    public class AndonReportData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode { get; set; }

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string AndonName { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDesc { get; set; }

        /// <summary>
        /// 故障发生时间
        /// </summary>
        public string FaultTime { get; set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public string LastTime { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public string ResponseTime { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public string HandleTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 四级负责人
        /// </summary>
        public string Level4 { get; set; }

        /// <summary>
        /// 三级负责人
        /// </summary>
        public string Level3 { get; set; }

        /// <summary>
        /// 二级负责人
        /// </summary>
        public string Level2 { get; set; }

        /// <summary>
        /// 一级负责人
        /// </summary>
        public string Level1 { get; set; }
    }

    /// <summary>
    /// 安灯报表状态下拉
    /// </summary>
    [Serializable]
    public class AndonReportStateData
    {
        /// <summary>
        /// 
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
    }

    /// <summary>
    /// 安灯异常柱形图
    /// </summary>
    [Serializable]
    public class AndonReportBarChartData
    {
        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 待响应
        /// </summary>
        public decimal Standby { get; set; }

        /// <summary>
        /// 处理中
        /// </summary>
        public decimal Processing { get; set; }

        /// <summary>
        /// 待验收
        /// </summary>
        public decimal ToAccepted { get; set; }
    }

    [Serializable]
    public class AndonReportBarChartDataFactory
    { 
        /// <summary>
        /// 类型
        /// </summary>
        public AndonManageState State { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
