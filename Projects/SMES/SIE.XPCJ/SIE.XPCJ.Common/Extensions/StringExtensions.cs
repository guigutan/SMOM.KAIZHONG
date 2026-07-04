using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Common.Settings;
using SIE.XPCJ.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// 本地化，Localization的缩写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string L10N(this string str)
        {
            if (Global.Language == null)
            {
                return str;
            }

            var languageValue = Global.Language.GetLanguageValue(str, out bool isExsitKey);
            if (Global.Language.Code == AppSettings.Instance.DevCulture)//当前界面语言等于开发语言的时候收集
            {
                Collection(isExsitKey, str);
            }
            return languageValue;
        }

        /// <summary>
        /// 收集 字典中不存在则 收集该key到本地化 且要求启用收集
        /// </summary>
        public static void Collection(bool isExsitKey, string key)
        {
            if (string.IsNullOrEmpty(key))
                return;
            if (!ContainsChinese(key))
                return;
            if (AppSettings.Instance.CollectionCulture && !isExsitKey)
            {
                Resource resource = new Resource();
                resource.CultureId = Global.Language.CultureId;
                resource.Key = key;
                resource.Value = key;
                resource.PersistenceStatus = PersistenceStatus.New;
                LanguageHelper.InQueue(resource);
            }
        }


        /// <summary>
        /// 本地化格式化
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string L10nFormat(this string str, params object[] args)
        {
            return str.L10N().FormatArgs(args);
        }

        /// <summary>
        /// 格式化字符串(已调整为安全兼容模式)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatArgs(this string str, params object[] args)
        {
            string ret = str;
            try
            {
                ret = string.Format(str, args);
            }
            catch
            {
                //log.ErrorFormat("格式化字符出错：模型{0}，参数个数：{1}，参数列表:{2}, 异常消息：{3}", str, args?.Length, string.Join(",", args), ex.ToString());
                for (int i = 0, j = args.Length; i < j; i++)
                {
                    str = str.Replace("{" + i + "}", args[i].ToString());
                }
                ret = str;
            }
            return ret;
        }

        /// <summary>
        /// 字符串是否包含中文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ContainsChinese(this string input)
        {
            string pattern = @"[\u4e00-\u9fa5]"; // 匹配中文字符的正则表达式
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key">32字节的密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(this string plainText, string key = "sie@normalKey")
        {
            string key32 = key;
            if (key.Length < 32)
            {
                for (int i = 0; i < 32 - key.Length; i++)
                    key32 += "A";
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key32);
                aesAlg.IV = new byte[16]; // 使用默认的 IV

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encrypted;

                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }

                return Convert.ToBase64String(encrypted);
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="key">32字节的密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(this string cipherText, string key = "sie@normalKey")
        {
            string key32 = key;
            if (key.Length < 32)
            {
                for (int i = 0; i < 32 - key.Length; i++)
                    key32 += "A";
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key32);
                aesAlg.IV = new byte[16]; // 使用默认的 IV

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                string plaintext = null;

                using (var msDecrypt = new System.IO.MemoryStream(cipherBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return plaintext;
            }
        }
    }
}
