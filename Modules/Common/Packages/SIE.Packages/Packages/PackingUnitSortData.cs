using SIE.Packages.Packages;

namespace SIE.Packages
{
    /// <summary>
    /// 包装单位有序的数据
    /// </summary>
    public class PackingUnitSortData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackingUnit { get; set; }    
    }
}
