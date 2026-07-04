using SIE.Common.Algorithm;
using System;

namespace SIE.Core.Algorithms.KZ
{

    /// <summary>
    /// KZ-比亚迪生产日期算法
    /// </summary>
    [Algorithm("KZ-比亚迪生产日期算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class DateBYDSegmentAlgorithm : EntityCodeAlgorithm
    {
        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public override string GetCode()
        {
            var date = DateTime.Now;
            var yearCode = GetYearCode(date.Year);
            var monthCode = GetMonthCode(date.Month);
            var dateCode = date.ToString("dd");

            return "{0}{1}{2}".FormatArgs(yearCode, monthCode, dateCode);
        }

        /// <summary>
        /// 年份代码对照表(从2000年从A~Z按需编号)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetYearCode(int year)
        {
            if (year < 2000)
                return string.Empty;

            var i = (year - 2000) % 26 + 1;

            return i switch
            {
                1 => "A",
                2 => "B",
                3 => "C",
                4 => "D",
                5 => "E",
                6 => "F",
                7 => "G",
                8 => "H",
                9 => "I",
                10 => "J",
                11 => "K",
                12 => "L",
                13 => "M",
                14 => "N",
                15 => "O",
                16 => "P",
                17 => "Q",
                18 => "R",
                19 => "S",
                20 => "T",
                21 => "U",
                22 => "V",
                23 => "W",
                24 => "X",
                25 => "Y",
                26 => "Z",
                _ => string.Empty
            };
        }

        /// <summary>
        /// 月份代码对照表 A-L
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        string GetMonthCode(int month)
        {
            return month switch
            {
                1 => "A",
                2 => "B",
                3 => "C",
                4 => "D",
                5 => "E",
                6 => "F",
                7 => "G",
                8 => "H",
                9 => "I",
                10 => "J",
                11 => "K",
                12 => "L",
                _ => string.Empty
            };
        }
    }
}
