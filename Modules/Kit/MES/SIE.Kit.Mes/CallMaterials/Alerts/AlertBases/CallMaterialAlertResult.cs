using SIE.Common.Alert;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 物料呼叫预警参数类
    /// </summary>
    [Serializable]
    public class CallMaterialAlertResult : AlertResultBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected CallMaterialAlertResult()
        {
            AlertTime = RF.Find<CallMaterialBill>().GetDbTime();
            MaterialAlerts = new List<MaterialAlert>();
        }

        /// <summary>
        /// 产线编码
        /// </summary>
        [Label("产线编码")]
        public string Line { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public string WorkOrderNO { get; set; }

        /// <summary>
        /// 预警物料集合
        /// </summary>
        public List<MaterialAlert> MaterialAlerts { get; set; }

        /// <summary>
        /// 预警时间
        /// </summary>
        public DateTime AlertTime { get; set; }
    }

    /// <summary>
    /// 预警物料信息
    /// </summary>
    [Serializable]
    public class MaterialAlert
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matrlCode">物料编码</param>
        /// <param name="matrlName">物料名称</param>
        public MaterialAlert(string matrlCode, string matrlName)
        {
            this.MaterialCode = matrlCode;
            this.MaterialName = matrlName;
            this.MaterialAlertValue = 0;
            this.MaterialWONo = string.Empty;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matrlCode">预警物料编码</param>
        /// <param name="matrlName">预警物料名称</param>
        /// <param name="alertValue">预警物料预警值</param>
        /// <param name="materialWONo">预警物料工单号</param>
        public MaterialAlert(string matrlCode, string matrlName, decimal alertValue, string materialWONo)
        {
            this.MaterialCode = matrlCode;
            this.MaterialName = matrlName;
            this.MaterialAlertValue = alertValue;
            this.MaterialWONo = materialWONo;
        }

        /// <summary>
        /// 预警物料编码
        /// </summary>
        [Label("物料编码")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 预警物料名称
        /// </summary>
        [Label("物料名称")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 预警物料预警值
        /// </summary>
        public decimal MaterialAlertValue { get; set; }

        /// <summary>
        /// 预警物料工单号
        /// </summary>
        public string MaterialWONo { get; set; }
    }

    /// <summary>
    /// 物料呼叫预警参数集合类
    /// </summary>
    [Serializable]
    public class CallMaterialAlertResultList : AlertResultListBase
    {
    }
}
