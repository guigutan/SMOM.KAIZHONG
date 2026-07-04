using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS
{
    /// <summary>
    /// 获取序列号状态表
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIPackingLabelSn))]
    public interface IPackingLabelSn
    {
        /// <summary>
        /// 获取序列号状态表数据
        /// </summary>
        /// <param name="sns">条码</param>
        /// <returns>序列号状态表数据</returns>
        List<PackingLabelSnData> GetPackingLabelSnDatas(List<string> sns);

        /// <summary>
        /// 创建序列号状态表数据
        /// </summary>
        /// <param name="labelSnDatas">标签</param>
        void CreatePackingLabelSn(List<PackingLabelSnData> labelSnDatas);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultIPackingLabelSn : IPackingLabelSn
    {
        /// <summary>
        /// 创建序列号状态表数据
        /// </summary>
        /// <param name="labelSnDatas">标签</param>
        public void CreatePackingLabelSn(List<PackingLabelSnData> labelSnDatas)
        {
            ////无
        }

        /// <summary>
        /// 获取序列号状态表数据
        /// </summary>
        /// <param name="sns">条码</param>
        /// <returns>序列号状态表数据</returns>
        public List<PackingLabelSnData> GetPackingLabelSnDatas(List<string> sns)
        {
            return new List<PackingLabelSnData>();
        }
    }
}
