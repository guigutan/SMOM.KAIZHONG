using SIE.Common.Algorithm;
using System;

namespace SIE.Core.Algorithms.KZ
{

    /// <summary>
    /// KZ-东风日期算法
    /// </summary>
    [Algorithm("KZ-东风日期算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class DateDFSegmentAlgorithm : EntityCodeAlgorithm
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
        /// 生产年月中年份码为1 位，用数字或字母表示，按表  规定的执行，30 年一循环 (从2001年从1~9 A~Y按需编号)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetYearCode(int year)
        {
            if (year < 2001)
                return string.Empty;

            var i = (year - 2001) % 30 + 1;

            return i switch
            {
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "4",
                5 => "5",
                6 => "6",
                7 => "7",
                8 => "8",
                9 => "9",
                10 => "A",
                11 => "B",
                12 => "C",
                13 => "D",
                14 => "E",
                15 => "F",
                16 => "G",
                17 => "H",
                18 => "J",
                19 => "K",
                20 => "L",
                21 => "M",
                22 => "N",
                23 => "P",
                24 => "R",
                25 => "S",
                26 => "T",
                27 => "V",
                28 => "W",
                29 => "X",
                30 => "Y",
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
