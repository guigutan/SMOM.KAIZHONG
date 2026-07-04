using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Piles.Configs
{
    /// <summary>
    /// ASN启用配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [Label("移动端码盘创建LPN格式验证")]
    public class LpnRuleConfigValue : ConfigValue
    {

        #region LPN规则
        /// <summary>
        /// LPN规则
        /// </summary>
        [Label("LPN规则")]
        public static readonly Property<string> LpnRuleTextProperty = P<LpnRuleConfigValue>.Register(e => e.LpnRuleText);


        /// <summary>
        /// LPN规则文本
        /// </summary>
        public string LpnRuleText
        {
            get { return this.GetProperty(LpnRuleTextProperty); }
            set { this.SetProperty(LpnRuleTextProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示接口配置
        /// </summary>
        /// <returns>返回接口配置</returns>
        public override string Display()
        {
            return LpnRuleText;
        }

        /// <summary>
        /// lpn编码规则合法性校验
        /// </summary>
        /// <param name="strLpn">LPN编码</param>
        /// <param name="strErr">校验异常信息</param>
        /// <returns>返回是否校验合法</returns>
        public bool lpnCheck(string strLpn,out string strErr)
        {
            strErr = string.Empty;
            if (string.IsNullOrEmpty(LpnRuleText))
            {
                return true;
            }
            string[] strList = LpnRuleText.Split(new string[] { "_" }, StringSplitOptions.None);//允许为空，为空则不校验
            if (strList == null || strList.Length < 1)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(strList[0]) && !strLpn.StartsWith(strList[0]))
            {
                strErr = string.Format("LPN：[{0}]不符合规范，开始字符不包含：[{1}]", strLpn, strList[0]);
                return false;
            }
            int iCount = strList.Length;
            if (iCount > 1)
            {
                if (!string.IsNullOrEmpty(strList[iCount - 1]) && !strLpn.EndsWith(strList[iCount - 1]))
                {
                    strErr = string.Format("LPN：[{0}]不符合规范，结束字符不包含：[{1}]", strLpn, strList[iCount - 1]);
                    return false;
                }
            }
            if (iCount > 2)
            {
                for (int i = 1; i < iCount - 1; i++)
                {
                    if (!string.IsNullOrEmpty(strList[i]) && strLpn.IndexOf(strList[i], 1) < 1)
                    {
                        strErr = string.Format("LPN：[{0}]不符合规范，中间字符不包含：[{1}]", strLpn, strList[i]);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
