using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.QMS.IQC
{

    /// <summary>
    /// 来料挑选接口
    /// </summary>
    [Services.Service(FallbackType = typeof(IqcPickDefault))]
    public interface IIqcPick
    {

        /// <summary>
        /// 检查是否存在编码生成规则
        /// </summary>
        /// <returns></returns>
        bool CheckIqcPickCodeConfig();
    }

    /// <summary>
    /// 接口的默认实现
    /// </summary>
    class IqcPickDefault : IIqcPick
    {
        /// <summary>
        /// 检查是否存在编码生成规则
        /// </summary>
        /// <returns></returns>
        public bool CheckIqcPickCodeConfig()
        {
            return false;
        }
    }
}
