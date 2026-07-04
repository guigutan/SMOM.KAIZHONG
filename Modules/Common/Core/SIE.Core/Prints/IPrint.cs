using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.Prints
{
    /// <summary>
    /// 打印接口
    /// </summary>
    public interface IPrint 
    {
        /// <summary>
        /// 标签补打
        /// </summary>
        /// <param name="logId"></param>
        void RePrintData(double logId);
    }
}
