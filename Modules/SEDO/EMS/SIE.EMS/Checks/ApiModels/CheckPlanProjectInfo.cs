using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 设备点检信息
    /// </summary>
    [Serializable]
    public class CheckPlanProjectInfo
    {
        /// <summary>
        /// 点检计划Id
        /// </summary>
        public double CheckPlanId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 部位
        /// </summary>
        public string Part { get; set; }

        /// <summary>
        /// 项目耗材
        /// </summary>
        public string Consumable { get; set; }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParaCode { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParaName { get; set; }

        /// <summary>
        /// 设备参数
        /// </summary>
        public int EquipPara { get; set; }

        /// <summary>
        /// 设备参数名称
        /// </summary>
        public string EquipParaName { get; set; }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// 检验结果(0:NG,1:OK)
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 点检项目ID
        /// </summary>
        public double ProjectId { get; set; }

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId { get; set; }

        /// <summary>
        /// 周期类型 0-班 1-日
        /// </summary>
        public int CycleType { get; set; }

        /// <summary>
        /// 标准
        /// </summary>
        public string Standard { get; set; }

        /// <summary>
        /// 是否拍照 0-否 1-是
        /// </summary>
        public int IsPhoto { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 使用时长
        /// </summary>
        public decimal? UseTime { get; set; }
    }
}