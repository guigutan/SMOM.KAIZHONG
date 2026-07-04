using SIE.Api;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Packages.QrCodeParseRules
{
    /// <summary>
    /// 二维码解析规则控制器 API接口
    /// </summary>
    public partial class QrCodeParseRuleController
    {
        /// <summary>
        /// 获取二维码解析数据
        /// </summary>
        /// <param name="parseData"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [ApiService("获取二维码解析数据")]
        [return: ApiReturn("返回二维码解析数据集合：List<QrCodeRstData>")]
        public virtual List<QrCodeRstData> GetQrCodeParseData([ApiParameter("被解析的内容")] string parseData, [ApiParameter("指定需解析的内容")] string keyWord)
        {
            List<QrCodeRstData> rstDatas = new List<QrCodeRstData>();
            EntityList<QrCodeParseRule> enableQrCodeDatas = GetEnabelQrCodeParseRuleData();

            enableQrCodeDatas.ForEach(p =>
            {
                if (p.InterceptWay == InterceptWay.InterceptDigit)
                {
                    rstDatas = GetInterceptDigitData(p, parseData);
                }
                else if (p.InterceptWay == InterceptWay.Separator)
                {
                    rstDatas = GetSeparator(p, parseData);
                }
                else
                {
                    //
                }
            });

            return rstDatas;
        }

        /// <summary>
        /// 获取截取位数据
        /// </summary>
        /// <param name="qrCode">二维码数据</param>
        /// <param name="parseData">解析数据</param>
        /// <returns>截取位数据</returns>
        public virtual List<QrCodeRstData> GetInterceptDigitData(QrCodeParseRule qrCode, string parseData)
        {
            List<QrCodeRstData> rstDatas = new List<QrCodeRstData>();

            if (qrCode.QrCodeParseRuleDetailList == null)
            {
                return rstDatas;
            }
            int interceptEnd = qrCode.QrCodeParseRuleDetailList.Max(t => t.InterceptEnd.Value);
            if (interceptEnd > parseData.Count())
            {
                return rstDatas;
            }
            foreach (var p in qrCode.QrCodeParseRuleDetailList.OrderBy(p => int.Parse(p.LineNo)))
            {
                // 1.截取开始位和截取结束位都小于等于被解析的内容长度的，将截取的内容直接返回
                if (p.InterceptStart.HasValue && p.InterceptStart.Value <= parseData.Length && p.InterceptEnd.HasValue && p.InterceptEnd.Value <= parseData.Length)
                {
                    rstDatas.Add(new QrCodeRstData()
                    {
                        QrCodeKey = p.ParseField.ToString(),
                        QrCodeName = p.ParseField.ToLabel(),
                        QrCodeKeyVal = (int)p.ParseField,
                        ParseField = p.ParseField,
                        QrCodeValue = parseData.Substring(p.InterceptStart.Value - 1, (p.InterceptEnd.Value - p.InterceptStart.Value + 1))
                    });
                }

                // 2.截取开始位小于等于被解析的内容长度、截取结束位大于被解析的内容长度，截取到多少内容就返回多少
                if (p.InterceptStart.HasValue && p.InterceptStart.Value <= parseData.Length && p.InterceptEnd.HasValue && p.InterceptEnd.Value > parseData.Length)
                {
                    rstDatas.Add(new QrCodeRstData()
                    {
                        QrCodeKey = p.ParseField.ToString(),
                        QrCodeName = p.ParseField.ToLabel(),
                        QrCodeKeyVal = (int)p.ParseField,
                        ParseField = p.ParseField,
                        QrCodeValue = parseData.Substring(p.InterceptStart.Value - 1, parseData.Length)
                    });
                }

                // 3.截取开始位大于被解析的内容长度，返回空值
                if (p.InterceptStart.HasValue && p.InterceptStart.Value > parseData.Length)
                {
                    rstDatas.Add(new QrCodeRstData()
                    {
                        QrCodeKey = p.ParseField.ToString(),
                        QrCodeName = p.ParseField.ToLabel(),
                        QrCodeKeyVal = (int)p.ParseField,
                        ParseField = p.ParseField,
                        QrCodeValue = null
                    });
                }
            }

            return rstDatas;
        }

        /// <summary>
        /// 获取分隔符数据
        /// </summary>
        /// <param name="qrCode">二维码解析规则</param>
        /// <param name="parseData">解析数据</param>
        public virtual List<QrCodeRstData> GetSeparator(QrCodeParseRule qrCode, string parseData)
        {
            List<QrCodeRstData> rstDatas = new List<QrCodeRstData>();
            if (qrCode.SeparatorType == "&amp;")
            {
                qrCode.SeparatorType = "&";
            } 
            if (parseData.IndexOf(qrCode.SeparatorType) < 0)
                return rstDatas;

            var parseDatas = parseData.Split(qrCode.SeparatorType.ToCharArray()).Where(f => f.IsNotEmpty()).ToList();
            var count = qrCode.QrCodeParseRuleDetailList.Count;
            for (int i = 0; i < count; i++)
            {
                var qrRulDtlData = qrCode.QrCodeParseRuleDetailList[i];
                var qrData = string.Empty;
                if (i == parseDatas.Count)
                {
                    qrData = parseData;
                }
                else if (i > parseDatas.Count)
                {
                    qrData = string.Empty;
                }
                else
                {
                    qrData = parseDatas[i];
                }
                rstDatas.Add(new QrCodeRstData()
                {
                    QrCodeKeyVal = (int)qrRulDtlData.ParseField,
                    QrCodeKey = qrRulDtlData.ParseField.ToString(),
                    QrCodeName = qrRulDtlData.ParseField.ToLabel(),
                    ParseField = qrRulDtlData.ParseField,
                    QrCodeValue = qrData
                });
            }
            return rstDatas;
        }
    }
}
