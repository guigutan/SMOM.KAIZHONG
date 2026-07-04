using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards
{
    /// <summary>
    /// 检验标准明细基类
    /// </summary>
    public class InspStandardDataDetailBase
    {
        /// <summary>
        /// 不对外公开构造函数
        /// </summary>
        protected InspStandardDataDetailBase() { }

        /// <summary>
        /// 检验类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 检验项目
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 检验工具
        /// </summary>
        public string TestTool { get; set; }

        /// <summary>
        /// 检验依据
        /// </summary>
        public string InspectionBasis { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public string LimitLow { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public string LimitMax { get; set; }

        /// <summary>
        /// 技术要求
        /// </summary>
        public string TechnicalRequirements { get; set; }

        /// <summary>
        /// 检验方式
        /// </summary>
        public string InspectionMode { get; set; }

        /// <summary>
        /// 是否必检
        /// </summary>
        public bool IsSuitable { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveStartTime { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? EffectiveEndTime { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string DefectGrade { get; set; }

        /// <summary>
        /// 检验标识
        /// </summary>
        public string CheckTag { get; set; }

        /// <summary>
        /// 抽样过程
        /// </summary>
        public string SamplingStep { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 间隔周期
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// 项目类别
        /// </summary>
        public string InspectionCategory { get; set; }

        /// <summary>
        /// 周期类型
        /// </summary>
        public string PeriodType { get; set; }
    }
}
