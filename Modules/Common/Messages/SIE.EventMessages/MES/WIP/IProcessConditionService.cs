using System.Collections.Generic;

namespace SIE.EventMessages.MES.WIP
{
    /// <summary>
    /// 采集数据接口
    /// </summary>
    public interface IProcessConditionService
    {
        /// <summary>
        /// 获取采集数据
        /// </summary>
        /// <returns></returns>
        IList<KeyValuePair<string, string>> GetProcessConditionItems();
    }
}
