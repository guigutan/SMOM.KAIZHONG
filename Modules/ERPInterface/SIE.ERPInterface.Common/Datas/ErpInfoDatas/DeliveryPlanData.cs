using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 发货计划分析数据
    /// </summary>
    [Serializable]
    public class DeliveryPlanData : ErpInfoData
    {
        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get;
            set;
        }

        /// <summary>
        /// 指定项目号
        /// </summary>
        public string ProjectNo
        {
            get;
            set;
        }

        /// <summary>
        /// 指定任务号
        /// </summary>
        public string TaskNo
        {
            get;
            set;
        }

        /// <summary>
        /// 指定批次号
        /// </summary>
        public string LotCode
        {
            get;
            set;
        }

        /// <summary>
        /// 指定生产批次
        /// </summary>
        public string ProductBatch
        {
            get;
            set;
        }

        /// <summary>
        /// 计划单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 计划明细行号
        /// </summary>
        public int LineNo { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string FromWhCode { get; set; }

        /// <summary>
        /// 目标仓库
        /// </summary>
        public string ToWhCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }        
    }
}
