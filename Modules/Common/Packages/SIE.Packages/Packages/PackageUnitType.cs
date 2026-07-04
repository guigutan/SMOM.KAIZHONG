using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Packages.Packages
{
    /// <summary>
    /// 包装类型
    /// </summary>
    public enum PackageUnitType
    {
        /// <summary>
        /// 主单位
        /// </summary>
        [Label("主单位")]
        MasterUnit,

        /// <summary>
		/// 内包装
		/// </summary>
		[Label("内包装")]      
        InnerPack,

        /// <summary>
        /// 盒
        /// </summary>
        [Label("盒")]       
        Case,

        /// <summary>
        /// 箱
        /// </summary>
        [Label("箱")]       
        Box,

        /// <summary>
        /// 托
        /// </summary>
        [Label("托")]      
        Pallet,

        /// <summary>
        /// 多托
        /// </summary>
        [Label("多托")]       
        MultiPallet,

        /// <summary>
        /// 集装箱
        /// </summary>
        [Label("集装箱")]      
        Container,
    }
}
