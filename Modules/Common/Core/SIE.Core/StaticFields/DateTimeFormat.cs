using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SIE.Core
{
    /// <summary>
    /// 日期-字符串 格式字段
    /// </summary>
    public static class DateTimeFormat
    {
        /// <summary>
        /// yyyy/MM/dd HH:mm:ss
        /// </summary>
        public static readonly string LongDateString1 = "yyyy/MM/dd HH:mm:ss";

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static readonly string LongDateString2 = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// yyyy-MM-dd hh:mm
        /// </summary>
        public static readonly string LongDateString3 = "yyyy-MM-dd hh:mm";

        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        public static readonly string LongDateString4 = "yyyyMMddHHmmss";

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        public static readonly string YYYMMdd1 = "yyyy/MM/dd";

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public static readonly string YYYMMdd2 = "yyyy-MM-dd";

        /// <summary>
        /// yyyy-MM
        /// </summary>
        public static readonly string YYYYMM = "yyyy-MM";

        /// <summary>
        /// MM/dd
        /// </summary>
        public static readonly string MMdd1 = "MM/dd";

        /// <summary>
        /// MM:mm
        /// </summary>
        public static readonly string HHmm = "HH:mm";

        /// <summary>
        /// 通用字符串转DateTime（兼容多种格式+过滤非法日期）
        /// </summary>
        /// <param name="dateStr">待转换字符串（如2025-13-40、2025/13/40、20251340、2025-13-40 14:30:59等）</param>
        /// <param name="result">转换后的DateTime（失败则为DateTime.MinValue）</param>
        /// <returns>是否转换成功</returns>
        public static bool SafeConvertToDateTime(string dateStr, out DateTime result)
        {
            result = DateTime.MinValue;

            // 1. 空值/空白校验
            if (string.IsNullOrWhiteSpace(dateStr))
            {
                Console.WriteLine("错误：字符串为空");
                return false;
            }

            // 2. 格式归一化：统一替换分隔符为-，并提取日期+时间部分
            string normalizedStr = NormalizeDateFormat(dateStr);
            if (string.IsNullOrEmpty(normalizedStr))
            {
                Console.WriteLine($"错误：{dateStr} 不是有效的日期格式");
                return false;
            }

            // 3. 定义所有可能的日期格式（覆盖无分隔符/有分隔符、带/不带时分秒）
            string[] dateFormats = new[]
            {
            "yyyy-MM-dd",        // 2025-12-18（标准）
            "yyyyMMdd",          // 20251218（无分隔符）
            "yyyy-MM-dd HH:mm:ss",// 2025-12-18 14:30:59
            "yyyyMMddHHmmss",    // 20251218143059
            "yyyy-MM-dd HH:mm",  // 2025-12-18 14:30
            "yyyyMMddHHmm",      // 202512181430
            "yyyyMMddHHmmss"     //20251218143010
        };

            // 4. 安全转换（遍历所有格式，匹配则尝试转换，同时校验日期合法性）
            foreach (var format in dateFormats)
            {
                if (DateTime.TryParseExact(
                    normalizedStr,
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime tempDate))
                {
                    // 额外校验：确保年/月/日在合法范围（避免13月、40日等）
                    if (IsValidDate(tempDate.Year, tempDate.Month, tempDate.Day))
                    {
                        result = tempDate;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 格式归一化：替换/为-，移除多余字符，分离日期和时间
        /// </summary>
        private static string NormalizeDateFormat(string input)
        {
            // 移除所有非数字、非-、非:的字符（保留日期时间核心字符）
            string cleaned = Regex.Replace(input, @"[^0-9\-:]", "");

            // 替换/为-，统一分隔符
            cleaned = cleaned.Replace("/", "-");

            // 优化正则：支持以下格式
            // 1. 8位纯日期：20251218
            // 2. 带-的日期：2025-12-18
            // 3. 14位无分隔符日期+时间：20251218143059（yyyyMMddHHmmss）
            // 4. 12位无分隔符日期+时间：202512181430（yyyyMMddHHmm）
            // 5. 带-的日期+带:的时间：2025-12-18 14:30:59 / 2025-12-18 14:30
            // 6. 纯日期+带:的时间：20251218 14:30:59 / 20251218 14:30
            string pattern = @"^
        (?:\d{8}|\d{4}-\d{2}-\d{2})          # 基础日期（8位无分隔符 或 带-的日期）
        |                                   # 或
        \d{12}|\d{14}                       # 无分隔符日期+时间（12位=yyyyMMddHHmm | 14位=yyyyMMddHHmmss）
        |                                   # 或
        (?:\d{8}|\d{4}-\d{2}-\d{2})         # 基础日期
        \s*\d{2}:\d{2}(:\d{2})?             # 可选时间（带:分隔）
    $";
            // 忽略正则中的空白符（RegexOptions.IgnorePatternWhitespace），方便阅读
            Match match = Regex.Match(cleaned, pattern, RegexOptions.IgnorePatternWhitespace);

            // 对14位/12位无分隔符串，无需修改；对其他格式，返回匹配结果
            return match.Success ? match.Value : null;
        }

        /// <summary>
        /// 校验年月日是否合法（避免13月、40日等非法值）
        /// </summary>
        public static bool IsValidDate(int year, int month, int day)
        {
            // 年份范围限制（可根据业务调整）
            if (year < 1900 || year > 9999) return false;

            // 月份必须1-12
            if (month < 1 || month > 12) return false;

            // 日期必须在当月合法范围内（自动处理2月/闰月）
            int maxDay = DateTime.DaysInMonth(year, month);
            return day >= 1 && day <= maxDay;
        }

        /// <summary>
        /// 日期转字符串 yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToLongFormat1(DateTime dt)
        {
            return dt.ToString(DateTimeFormat.LongDateString1);
        }

        /// <summary>
        /// 日期转字符串 yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToLongFormat1(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(DateTimeFormat.LongDateString1);
            }

            return string.Empty;
        }

        /// <summary>
        /// MM-dd HH:mm
        /// </summary>
        public static readonly string MMddHHmm = "MM-dd HH:mm";

        /// <summary>
        /// 日期转字符串 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToLongFormat2(DateTime dt)
        {
            return dt.ToString(DateTimeFormat.LongDateString2);
        }

        /// <summary>
        /// 日期转字符串 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToLongFormat2(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(DateTimeFormat.LongDateString2);
            }

            return string.Empty;
        }

        /// <summary>
        /// 日期转字符串 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToLongFormat4(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(DateTimeFormat.LongDateString4);
            }

            return string.Empty;
        }

        /// <summary>
        /// 转化时间类型为字符串类型   yyyy/MM/dd 
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToShortFormat1(DateTime dt)
        {
            return dt.ToString(DateTimeFormat.YYYMMdd1);
        }

        /// <summary>
        /// 转化时间类型为字符串类型   yyyy/MM/dd 
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToShortFormat1(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(DateTimeFormat.YYYMMdd1);
            }

            return string.Empty;
        }

        /// <summary>
        /// 转化时间类型为字符串类型   yyyy-MM-dd 
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToShortFormat2(this DateTime dt)
        {
            return dt.ToString(DateTimeFormat.YYYMMdd2);
        }

        /// <summary>
        /// 转化时间类型为字符串类型   yyyy-MM-dd 
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToShortFormat2(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(DateTimeFormat.YYYMMdd2);
            }

            return string.Empty;
        }

        /// <summary>
        /// 转化时间类型为字符串类型   yyyyMMddHHmmssfff
        /// </summary>
        /// <param name="dt">日期对象</param>
        /// <returns>返回日期转字符串</returns>
        public static string ToLongCodeFormat(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmssfff");
        }
    }
}