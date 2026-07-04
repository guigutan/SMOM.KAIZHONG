using SIE.Domain;
using SIE.Packages.QrCodeParseRules;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Packages.QrCodeParseRules.DataQueryer
{
    /// <summary>
    /// 二维码解析
    /// </summary>
    public class QrCodeDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// QrCode测试
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="sn">序列号</param>
        /// <returns>二维码解析数据列表</returns>
        public List<QrCodeRstData> QrCodeTest(double billId, string sn)
        {
            var rule = RF.GetById<QrCodeParseRule>(billId);
            if (rule == null)
            {
                return new List<QrCodeRstData>();
            }

            if (rule.InterceptWay == InterceptWay.Separator)
            {
                return RT.Service.Resolve<QrCodeParseRuleController>().GetSeparator(rule, sn);
            }
            else
            {
                return RT.Service.Resolve<QrCodeParseRuleController>().GetInterceptDigitData(rule, sn);
            }
        }
    }
}
