using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签状态
    /// </summary>
    public enum ItemLabelState
    {
        /// <summary>
        /// 接收
        /// </summary>
        [Label("接收")]
        Receive = 1,

        /// <summary>
        /// 上料
        /// </summary>
        [Label("上料")]
        Feeding = 2,

        /// <summary>
        /// 下料
        /// </summary>
        [Label("下料")]
        Blanking = 3

    }
}
