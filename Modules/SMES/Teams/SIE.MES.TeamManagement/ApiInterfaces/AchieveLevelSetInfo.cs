using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 个人评分绩效等级配置API信息类
    /// </summary>
    [Serializable]
    public class AchieveLevelSetInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="achiLevel">绩效等级</param>
        /// <param name="levelOperator">绩效运算符</param>
        public AchieveLevelSetInfo(int rowIndex, decimal? minValue, decimal? maxValue, int achiLevel, int levelOperator)
        {
            this.RowIndex = rowIndex;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.AchiLevel = achiLevel;
            this.LevelOperator = levelOperator;
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 绩效等级 [0:优秀，1:良好,2:及格,3:不良]
        /// </summary>
        public int AchiLevel { get; set; }

        /// <summary>
        /// 绩效运算符
        /// 0:小于, 1:大于, 2:介于(闭合), 
        /// 3:小于等于, 4:大于等于
        /// </summary>
        public int LevelOperator { get; set; }
    }
}
