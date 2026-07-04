using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 
    /// </summary>
    public enum OutputListType
    {
        /// <summary>
        /// 副产品
        /// </summary>
        [Label("副产品")]
        ByProducts,

        /// <summary>
        /// 联产品
        /// </summary>
        [Label("联产品")]
        JointProducts
    }
}
