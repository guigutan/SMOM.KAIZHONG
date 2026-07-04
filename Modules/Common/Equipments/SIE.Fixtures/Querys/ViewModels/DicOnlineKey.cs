namespace SIE.Fixtures.Querys.ViewModels
{
    /// <summary>
    /// 工单Id、产线Id、产线名称和工单号字典Key
    /// </summary>
    public class DicOnlineKey
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>true/false</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            if (!(obj is DicOnlineKey))
                return false;
            var dicconfikey = (DicOnlineKey)obj;
            return WorkOrderId == dicconfikey.WorkOrderId && ResourceId == dicconfikey.ResourceId && ResourceName == dicconfikey.ResourceName && WorkOrderNo == dicconfikey.WorkOrderNo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>工单Id、产线Id、产线名称和工单号字典Key</returns>
        public override int GetHashCode()
        {
            return (WorkOrderId + ResourceId + ResourceName + WorkOrderNo).GetHashCode();
        }
    }
}
