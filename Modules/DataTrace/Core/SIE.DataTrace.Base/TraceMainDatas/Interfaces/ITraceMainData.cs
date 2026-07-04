using SIE.DataTrace.Base.TraceMainDatas.Datas;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.Base.TraceMainDatas.Interfaces
{
    /// <summary>
    /// 追溯主数据接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultTraceMainData))]
    public interface ITraceMainData
    {
        /// <summary>
        /// 获取追溯主数据（根据ID）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        DataMainTraceDto GetTraceMainDatas(List<double> ids);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultTraceMainData : ITraceMainData
    {
        /// <summary>
        /// 获取追溯主数据（根据ID）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public DataMainTraceDto GetTraceMainDatas(List<double> ids)
        {
            return null;
        }
    }
    }
