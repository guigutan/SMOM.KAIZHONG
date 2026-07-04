using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using static SIE.XPCJ.Common.Snowflakes.SnowflakeExtension;

namespace SIE.XPCJ.Common.Snowflakes
{
    /// <summary>
    /// 表示一个 Twitter-Snowflake 算法的分布式编号的统一管理器。
    /// </summary>
    public static class SnowflakeHelper
    {
        private static long Seq = Process.GetCurrentProcess().StartTime.ConvertToJsTime();
        private static readonly IPAddress LocalIPAddress = Snowflake.GetLocalIPAddress();

        private static Snowflake _Instance;
        private static Snowflake Instance
        {
            get {
                if (_Instance == null)
                {
                    _Instance = Snowflake.Create(LocalIPAddress, Process.GetCurrentProcess().Id);
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 获取下一个唯一编号。
        /// </summary>
        /// <returns>一个在当前数据中心和工作机器中的唯一编号。</returns>
        public static long NextId() => Instance.NextId();

        /// <summary>
        /// 批量获取编号
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static List<long> BatchNextId(long batchSize)
        {
            var result = new List<long>();
            for (int i = 0; i < batchSize; i++)
            {
                result.Add(Instance.NextId());
            }
            return result;
        }

        /// <summary>
        /// 生成一个不可逆的十六位唯一字符串。
        /// </summary>
        /// <returns>一个在当前数据中心和工作机器中的唯一编号。</returns>
        public static string NextIdString() => Instance.NextIdString() + (System.Threading.Interlocked.Increment(ref Seq) % 10000L).ToString("0000");
    }
}
