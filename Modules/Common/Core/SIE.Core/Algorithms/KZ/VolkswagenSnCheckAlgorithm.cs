using Newtonsoft.Json;
using SIE.Common.Algorithm;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.Algorithms.KZ
{
    /// <summary>
    /// 大众序列号校验码算法配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("大众序列号校验码算法配置")]
    public class VolkswagenSnConfig : SequenceConfig
    {
        #region 大众代码 VwCode
        /// <summary>
        /// 大众代码
        /// </summary>
        [Label("大众代码")]
        public static readonly Property<string> VwCodeProperty = P<VolkswagenSnConfig>.Register(e => e.VwCode);

        /// <summary>
        /// 大众代码
        /// </summary>
        public string VwCode
        {
            get { return this.GetProperty(VwCodeProperty); }
            set { this.SetProperty(VwCodeProperty, value); }
        }
        #endregion

        #region 公司代码 CompanyCode
        /// <summary>
        /// 公司代码
        /// </summary>
        [Label("公司代码")]
        public static readonly Property<string> CompanyCodeProperty = P<VolkswagenSnConfig>.Register(e => e.CompanyCode);

        /// <summary>
        /// 公司代码
        /// </summary>
        public string CompanyCode
        {
            get { return this.GetProperty(CompanyCodeProperty); }
            set { this.SetProperty(CompanyCodeProperty, value); }
        }
        #endregion

        #region 序列产线号 LineCode
        /// <summary>
        /// 序列产线号
        /// </summary>
        [Label("序列产线号")]
        public static readonly Property<string> LineCodeProperty = P<VolkswagenSnConfig>.Register(e => e.LineCode);

        /// <summary>
        /// 序列产线号
        /// </summary>
        public string LineCode
        {
            get { return this.GetProperty(LineCodeProperty); }
            set { this.SetProperty(LineCodeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="value">配置JSON字符串</param>
        public override void Initialize(string value)
        {
            base.Initialize(value);
            if (!string.IsNullOrEmpty(value))
            {
                var config = JsonConvert.DeserializeObject<VolkswagenSnConfig>(value);
                if (config != null)
                {
                    this.VwCode = config.VwCode;
                    this.CompanyCode = config.CompanyCode;
                    this.LineCode = config.LineCode;
                }
            }
        }
    }

    /// <summary>
    /// 大众序列号校验码算法
    /// 共15位字符，格式：大众代码(3位) + 公司代码(4位) + 日期(3位) + 流水号(3位) + 序列产线号(1位) + 校验码(1位)
    /// 流水号根据当天日期区分，每天从起始值重新开始
    /// </summary>
    [Algorithm("大众序列号校验码算法", typeof(VolkswagenSnConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    [Label("大众序列号校验码算法")]
    public class VolkswagenSnCheckAlgorithm : SequenceAlgorithm
    {
        #region 对照表

        /// <summary>
        /// 字符到数值的对照表（用于校验码计算）
        /// 0-9 → 0-9, A-Z → 10-35, - → 36, . → 37, 空格 → 38, $ → 39, / → 40, + → 41, % → 42
        /// </summary>
        protected static readonly Dictionary<char, int> CharToValue = new Dictionary<char, int>
        {
            {'0', 0},  {'1', 1},  {'2', 2},  {'3', 3},  {'4', 4},
            {'5', 5},  {'6', 6},  {'7', 7},  {'8', 8},  {'9', 9},
            {'A', 10}, {'B', 11}, {'C', 12}, {'D', 13}, {'E', 14},
            {'F', 15}, {'G', 16}, {'H', 17}, {'I', 18}, {'J', 19},
            {'K', 20}, {'L', 21}, {'M', 22}, {'N', 23}, {'O', 24},
            {'P', 25}, {'Q', 26}, {'R', 27}, {'S', 28}, {'T', 29},
            {'U', 30}, {'V', 31}, {'W', 32}, {'X', 33}, {'Y', 34},
            {'Z', 35}, {'-', 36}, {'.', 37}, {' ', 38}, {'$', 39},
            {'/', 40}, {'+', 41}, {'%', 42}
        };

        /// <summary>
        /// 数值到字符的对照表（用于校验码反查）
        /// </summary>
        protected static readonly Dictionary<int, char> ValueToChar = new Dictionary<int, char>
        {
            {0, '0'},  {1, '1'},  {2, '2'},  {3, '3'},  {4, '4'},
            {5, '5'},  {6, '6'},  {7, '7'},  {8, '8'},  {9, '9'},
            {10, 'A'}, {11, 'B'}, {12, 'C'}, {13, 'D'}, {14, 'E'},
            {15, 'F'}, {16, 'G'}, {17, 'H'}, {18, 'I'}, {19, 'J'},
            {20, 'K'}, {21, 'L'}, {22, 'M'}, {23, 'N'}, {24, 'O'},
            {25, 'P'}, {26, 'Q'}, {27, 'R'}, {28, 'S'}, {29, 'T'},
            {30, 'U'}, {31, 'V'}, {32, 'W'}, {33, 'X'}, {34, 'Y'},
            {35, 'Z'}, {36, '-'}, {37, '.'}, {38, ' '}, {39, '$'},
            {40, '/'}, {41, '+'}, {42, '%'}
        };

        /// <summary>
        /// 36进制字符表（用于流水号转换）
        /// </summary>
        protected static readonly char[] Base36Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        #endregion

        #region 日期编码
        /// <summary>
        /// 获取年份代码
        /// 从0-9，接着是A-Z，共36位，分别对应2017年到2052年
        /// 2017=0, 2027=A, 2052=Z
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>年份代码字符</returns>
        public static char GetYearCode(int year)
        {
            if (year < 2017 || year > 2052)
                return '0';

            var offset = year - 2017;
            if (offset < 10)
                return (char)('0' + offset);
            else
                return (char)('A' + (offset - 10));
        }

        /// <summary>
        /// 获取月份代码
        /// 从0-9，接着是A-B，共12位，分别对应1月到12月
        /// 1月=0, 10月=9, 11月=A, 12月=B
        /// </summary>
        /// <param name="month">月份(1-12)</param>
        /// <returns>月份代码字符</returns>
        public static char GetMonthCode(int month)
        {
            if (month < 1 || month > 12)
                return '0';

            var offset = month - 1;
            if (offset < 10)
                return (char)('0' + offset);
            else
                return (char)('A' + (offset - 10));
        }

        /// <summary>
        /// 获取日期代码
        /// 从1-9，接着是A-V，共31位，分别对应1号到31号
        /// 1号=1, 10号=A, 31号=V
        /// </summary>
        /// <param name="day">日期(1-31)</param>
        /// <returns>日期代码字符</returns>
        public static char GetDayCode(int day)
        {
            if (day < 1 || day > 31)
                return '0';

            var offset = day - 1;
            if (offset < 9)
                return (char)('1' + offset);
            else
                return (char)('A' + (offset - 9));
        }

        /// <summary>
        /// 获取日期编码（年月日各1位，共3位）
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>3位日期编码字符串</returns>
        public static string GetDateCode(DateTime date)
        {
            var yearCode = GetYearCode(date.Year);
            var monthCode = GetMonthCode(date.Month);
            var dayCode = GetDayCode(date.Day);
            return "{0}{1}{2}".FormatArgs(yearCode, monthCode, dayCode);
        }
        #endregion

        #region 流水号编码
        /// <summary>
        /// 将流水号转换为36进制的3位字符串
        /// 1=001, 9=009, 10=00A, 35=00Z, 36=010, 46655=ZZZ
        /// </summary>
        /// <param name="sequence">流水号(1-46655)</param>
        /// <returns>3位36进制字符串</returns>
        public static string GetSequenceCode(int sequence)
        {
            if (sequence < 1 || sequence > 46655)
                return "000";

            var result = new StringBuilder();
            var value = sequence;

            while (value > 0)
            {
                var remainder = value % 36;
                result.Insert(0, Base36Chars[remainder]);
                value /= 36;
            }

            // 补齐到3位
            while (result.Length < 3)
                result.Insert(0, '0');

            return result.ToString();
        }
        #endregion

        #region 校验码计算
        /// <summary>
        /// 计算校验码
        /// 前面14位分别对照获取数值，相加后除以43取余数，余数对照获取字符
        /// </summary>
        /// <param name="code">前14位编码</param>
        /// <returns>校验码字符，长度不足14位返回空字符串</returns>
        public static string ComputeCheckDigit(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length < 14)
                return string.Empty;

            var keyCode = code.Substring(0, 14).ToUpper();
            var sum = 0;

            foreach (var c in keyCode)
            {
                if (CharToValue.ContainsKey(c))
                {
                    sum += CharToValue[c];
                }
            }

            var remainder = sum % 43;
            return ValueToChar.ContainsKey(remainder) ? ValueToChar[remainder].ToString() : string.Empty;
        }
        #endregion

        #region 序列算法核心方法
        /// <summary>
        /// 查询当天的序列
        /// </summary>
        /// <param name="startValue">起始值</param>
        /// <returns>日期序列</returns>
        protected override SequenceBase GetSequenceBase(int startValue)
        {
            if (startValue == 0)
                startValue = 1;
            var dateNow = DateTime.Now;
            var date = dateNow.Date;
            return RT.Service.Resolve<AlgorithmController>().GetDateSequence(Context.DetailId, date, startValue);
        }

        /// <summary>
        /// 将序列值转换为完整的15位大众序列号编码
        /// </summary>
        /// <param name="value">当前序列值</param>
        /// <param name="sequenceBase">序列基类</param>
        /// <returns>15位编码字符串</returns>
        protected override string ConvertValue(int value, SequenceBase sequenceBase)
        {
            var config = Context.Config as VolkswagenSnConfig;
            if (config == null)
                return value.ToString();

            // 大众代码（第1-3位）
            var vwCode = (config.VwCode ?? string.Empty).PadRight(3).Substring(0, 3);

            // 公司代码（第4-7位，不足4位前面补空格）
            var companyCode = (config.CompanyCode ?? string.Empty).PadLeft(4).Substring(0, 4);

            // 日期编码（第8-10位）
            var dateCode = GetDateCode(DateTime.Now);

            // 流水号（第11-13位），value是当前序列值
            var sequenceCode = GetSequenceCode(value);

            // 序列产线号（第14位）
            var lineCode = (config.LineCode ?? string.Empty).PadRight(1).Substring(0, 1);

            // 前14位
            var code14 = "{0}{1}{2}{3}{4}".FormatArgs(vwCode, companyCode, dateCode, sequenceCode, lineCode);

            // 计算校验码（第15位）
            var checkDigit = ComputeCheckDigit(code14);

            return code14 + checkDigit;
        }
        #endregion
    }
}
