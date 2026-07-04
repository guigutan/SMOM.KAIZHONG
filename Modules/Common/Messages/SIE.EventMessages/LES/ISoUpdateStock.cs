using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 发运单发货更新备料单信息
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultISoUpdateStock))]
    public interface ISoUpdateStock
    {
        /// <summary>
        /// 发运单发货更新备料单信息
        /// </summary>
        /// <param name="labelDatas"></param>
        void UpdateStockOrderBySo(List<SoLabelData> labelDatas);

        /// <summary>
        /// 发运单取消发货删除备料明细
        /// </summary>
        /// <param name="orderNo">发运单号</param>
        /// <param name="lineNos">发运单行号集合</param>
        void DeleteStockOrderSnBySo(string orderNo, List<string> lineNos);

        /// <summary>
        /// 更新备料单接收记录数据
        /// </summary>
        /// <param name="billNo">备料单</param>
        /// <param name="soNo">发运单</param>
        /// <param name="soLineNo">行号</param>
        /// <param name="labelNos">标签</param>
        void UpdateStockSn(string billNo, string soNo, string soLineNo, List<string> labelNos);
    }


    /// <summary>
    /// 发运单发货更新备料单信息
    /// </summary>
    class DefaultISoUpdateStock : ISoUpdateStock
    {
        public void UpdateStockOrderBySo(List<SoLabelData> labelDatas)
        {
            //无
        }

        /// <summary>
        /// 发运单取消发货删除备料明细
        /// </summary>
        /// <param name="orderNo">发运单号</param>
        /// <param name="lineNos">发运单行号集合</param>
        public void DeleteStockOrderSnBySo(string orderNo, List<string> lineNos)
        {
            //无
        }

        /// <summary>
        /// 更新备料单接收记录数据
        /// </summary>
        /// <param name="billNo">备料单</param>
        /// <param name="soNo">发运单</param>
        /// <param name="soLineNo">行号</param>
        /// <param name="labelNos">标签</param>
        public void UpdateStockSn(string billNo, string soNo, string soLineNo, List<string> labelNos)
        {
            ////无
        }
    }
}
