using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.EventMessages.EngineerPlans
{

    [Serializable]
    public class ScheduleAndCreateMITaskData
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }

        public double ItemId { get; set; }

        public string ItemRevision { get; set; }

        public string ItemExtPropName { get; set; }

        public string ItemEnableExtendProperty { get; set; }

        public double SaleOrderDetailId { get; set; }

        public string SaleOrderNo {  get; set; }

        public double CustomerId { get; set; }
        
        public DateTime ScheduleDay { get; set; }

        public DateTime RequireDelivery {  get; set; }


    }
}
