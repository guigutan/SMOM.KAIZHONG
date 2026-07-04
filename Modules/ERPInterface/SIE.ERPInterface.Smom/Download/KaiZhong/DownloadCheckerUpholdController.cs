using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Checker;
using SIE.MES.Fixture;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadCheckerUpholdController : DomainController
    {
        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveCheckerUpholdFactory(List<CheckerUpholdData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<CheckerUpholdData> list = new List<CheckerUpholdData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.CheckerUphold, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //工厂字典
                    Dictionary<string, Enterprise> dicFactory = new Dictionary<string, Enterprise>();

                    foreach (var data in datas)
                    {
                        try
                        {
                            OuterSystemRetVO result = new OuterSystemRetVO();
                            Valid(data, ref result);
                            if (!result.errorMsg.IsNullOrEmpty())
                                throw new ValidationException(result.errorMsg);

                            Enterprise factory = null;
                            if (dicFactory.ContainsKey(data.FactoryCode))
                            {
                                factory = dicFactory[data.FactoryCode];
                            }
                            else
                            {
                                factory = RT.Service.Resolve<EnterpriseController>().GetFactoriesList(null, data.FactoryCode).FirstOrDefault(p => p.Code == data.FactoryCode);
                                if (factory == null)
                                    throw new ValidationException("工厂{0}不存在".L10nFormat(data.FactoryCode));
                                dicFactory.Add(factory.Code, factory);
                            }

                            SaveCheckerUphold(data, factory);
                            apiResult.SuccessList.Add(data);
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            apiResult.ErrorList.Add(ex.GetBaseException().Message);
                            apiResult.ErrorObjList.Add(data);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<CheckerUpholdData>(erpDataInfLog, list, apiResult);
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<CheckerUpholdData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        public virtual void SaveCheckerUphold(CheckerUpholdData data, Enterprise factory)
        {
            try
            {
                CheckerUphold checkerUphold = new CheckerUphold();

                checkerUphold.PersistenceStatus = PersistenceStatus.New;
                checkerUphold.CheckerCode = data.CheckerCode;
                checkerUphold.CheckerName = data.CheckerName;
                checkerUphold.CheckerType = data.CheckerType;

                if (!data.EffectiveDate.IsNullOrEmpty())
                {

                    if (SafeConvertToDateTime(data.EffectiveDate, out DateTime result))
                    {
                        checkerUphold.EffectiveDate = result;
                    }
                }
                if (factory != null)
                    checkerUphold.FactoryId = factory.Id;

                checkerUphold.DrawingNo = data.DrawingNo;

                RF.Save(checkerUphold);
            }
            catch (Exception ex)
            {
                if (!ex.GetBaseException().Message.Contains("已存在相同"))
                    throw new ValidationException(ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="result"></param>
        public virtual void Valid(CheckerUpholdData data, ref OuterSystemRetVO result)
        {
            if (data.CheckerCode.IsNullOrEmpty())
            {
                var msg = "检具编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.CheckerName.IsNullOrEmpty())
            {
                var msg = "检具名称不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (!data.EffectiveDate.IsNullOrEmpty())
            {
                // 2. 格式归一化：统一替换分隔符为-，并提取日期+时间部分
                string normalizedStr = NormalizeDateFormat(data.EffectiveDate);
                if (normalizedStr.IsNullOrEmpty())
                {
                    var msg = "{0}日期格式不正确".L10nFormat(data.EffectiveDate);
                    result.success = false;
                    result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                    result.errorList.Add(data);
                    return;
                }
            }
            if (data.CheckerType.IsNullOrEmpty())
            {
                var msg = "检具类型不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.FactoryCode.IsNullOrEmpty())
            {
                var msg = "工厂编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
        }

        /// <summary>
        /// 通用字符串转DateTime（兼容多种格式+过滤非法日期）
        /// </summary>
        /// <param name="dateStr">待转换字符串（如2025-13-40、2025/13/40、20251340、2025-13-40 14:30:59等）</param>
        /// <param name="result">转换后的DateTime（失败则为DateTime.MinValue）</param>
        /// <returns>是否转换成功</returns>
        public virtual bool SafeConvertToDateTime(string dateStr, out DateTime result)
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
        public virtual bool IsValidDate(int year, int month, int day)
        {
            // 年份范围限制（可根据业务调整）
            if (year < 1900 || year > 2100) return false;

            // 月份必须1-12
            if (month < 1 || month > 12) return false;

            // 日期必须在当月合法范围内（自动处理2月/闰月）
            int maxDay = DateTime.DaysInMonth(year, month);
            return day >= 1 && day <= maxDay;
        }
    }
}
