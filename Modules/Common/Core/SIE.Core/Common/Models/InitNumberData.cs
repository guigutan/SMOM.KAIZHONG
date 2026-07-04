using SIE.Common.Algorithm;
using SIE.Common.NumberRules;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Core.Common.Models
{
    /// <summary>
    /// 初始化编码规则
    /// </summary>
    [Serializable]
    public class InitNumberData
    {
        /// <summary>
        /// 编码规则编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public RuleType RuleType { get; set; }

        /// <summary>
        /// 编码规则明细
        /// </summary>
        private List<InitNumberDetailData> details;

        /// <summary>
        /// 编码规则明细
        /// </summary>
        public List<InitNumberDetailData> Details
        {
            get
            {
                if (details == null)
                {
                    details = new List<InitNumberDetailData>();
                }

                return details;
            }
        }
    }

    /// <summary>
    /// 编码规则明细
    /// </summary>
    public class InitNumberDetailData
    {
        /// <summary>
        /// 编码段算法类型
        /// </summary>
        public DetailType DetailType { get; set; }

        /// <summary>
        /// 固定编码值
        /// </summary>
        public string FixedValue { get; set; }

        /// <summary>
        /// 日期格式
        /// </summary>
        public DateFormat DateFormat { get; set; }

        /// <summary>
        /// 编码长度
        /// </summary>
        public int Length { get; set; }
    }

    /// <summary>
    /// 编码段算法类型（标签为算法名称）
    /// </summary>
    public enum DetailType
    {
        /// <summary>
        /// 固定编码算法
        /// </summary>
        [Label("固定编码算法")]
        FixedValue = 0,

        /// <summary>
        /// 时间编码算法
        /// </summary>
        [Label("时间编码算法")]
        Date = 1,

        /// <summary>
        /// 序列生成算法(区分当天日期)
        /// </summary>
        [Label("序列生成算法(区分当天日期)")]
        TodaySequence = 2,

        /// <summary>
        /// 普通序列生成算法
        /// </summary>
        [Label("普通序列生成算法")]
        Sequence = 3,

        /// <summary>
        /// 库存组织编码段
        /// </summary>
        [Label("库存组织编码段")]
        InvOrg = 4
    }
}
