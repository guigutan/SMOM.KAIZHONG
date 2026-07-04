using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts.ApiModels
{
    /// <summary>
    /// 产前准备单据
    /// </summary>
    [Serializable]
    public class PrepareProductsBill
    {

        /// <summary>
        ///工单
        /// </summary>
        public string WONo { get; set; }

        /// <summary>
        ///工厂
        /// </summary>
        public string FactoryName { get; set; }
        /// <summary>
        ///工厂
        /// </summary>
        public double Factory { get; set; }
        /// <summary>
        ///资源
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 单据ID
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public string PlanBeginTime { get; set; }


        /// <summary>
        /// 状态显示值
        /// </summary>
        public string StateDisplay { get; set; }

       /// <summary>
       /// 
       /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProduceName { get; set; }
        
        /// <summary>
        /// 产品名称
        /// </summary>
        public double ProduceId { get; set; }

    }
}
