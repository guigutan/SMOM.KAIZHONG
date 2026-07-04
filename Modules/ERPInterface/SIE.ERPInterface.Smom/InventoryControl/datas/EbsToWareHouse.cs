using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.InventoryControl.datas
{
    /// <summary>
    /// ERP子库对照关系
    /// </summary>
    public enum EbsToWareHouse
    {
        /// <summary>
        /// 1:1
        /// </summary>
        [Label("1:1")]
        OneToOne = 1,

        /// <summary>
        /// 1:N
        /// </summary>
        [Label("1:N")]
        OneToN = 2,

        /// <summary>
        /// N:1
        /// </summary>
        [Label("N:1")]
        NToOne = 3,
    }
}
