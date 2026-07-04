namespace SIE.Fixtures.FixtureDemands.ViewModels
{
    /// <summary>
    /// 台账Id、仓库Id和库位Id字典Key
    /// </summary>
    public class DicUnloadKey
    {
        /// <summary>
        /// 台账Id
        /// </summary>
        public double AccountId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>true/false</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            if (!(obj is DicUnloadKey))
                return false;
            var dicconfikey = (DicUnloadKey)obj;
            return AccountId == dicconfikey.AccountId && WarehouseId == dicconfikey.WarehouseId && LocationId == dicconfikey.LocationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (AccountId + WarehouseId + LocationId).GetHashCode();
        }
    }
}
