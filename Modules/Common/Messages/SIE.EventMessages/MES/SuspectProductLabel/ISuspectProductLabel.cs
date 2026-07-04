using SIE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.SuspectProductLabel
{
    [Service(FallbackType = typeof(DefaultISuspectProductLabel))]
    public interface ISuspectProductLabel
    {
        /// <summary>
        /// 获取标签处理结果
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        string GetSuspectProductLabelDetailResult(string label);
    }

    public class DefaultISuspectProductLabel : ISuspectProductLabel
    {
        /// <summary>
        /// 获取标签处理结果
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public string GetSuspectProductLabelDetailResult(string label)
        {
            return string.Empty;
        }
    }
}
