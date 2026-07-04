using SIE.Common.CheckAlgorithms;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;

namespace SIE.Core.CheckAlgorithms.ManufacturedSN
{
    /// <summary>
    /// 整机序列号有序列号校验码
    /// </summary>
    [CheckAlgorithm("整机序列号有序列号校验码算法", typeof(CheckAlgorithmConfig))]
    [RootEntity, Serializable]
    public class ManufacturedSnCheckAlgorithm : CheckAlgorithm
    {
        /// <summary>
        /// 编码对应值字典
        /// </summary>
        protected Dictionary<char, int> dicWordValue { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManufacturedSnCheckAlgorithm()
        {
            dicWordValue = new Dictionary<char, int>() {
                {'0',0 },
                {'1',1 },
                {'2',2 },
                {'3',3 },
                {'4',4 },
                {'5',5 },
                {'6',6 },
                {'7',7 },
                {'8',8 },
                {'9',9 },
                {'A',10 },
                {'B',11 },
                {'C',12 },
                {'D',14 },
                {'E',15 },
                {'F',16 },
                {'G',17 },
                {'H',18 },
                {'J',19 },
                {'K',20 },
                {'L',21 },
                {'M',22 },
                {'N',23 },
                {'P',24 },
                {'Q',25 },
                {'R',27 },
                {'S',28 },
                {'T',29 },
                {'U',30 },
                {'V',31 },
                {'W',32 },
                {'X',33 },
                {'Y',34 },
                {'Z',35 }
            };   //排除字 母 O、I，因为 13、26 不能对 13 取模数
        }

        /// <summary>
        /// 验证长度
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected override bool OnValidateCode(string code)
        {
            if (code.Length < 12)
            {
                throw new ValidationException("编码[{0}]长度不足12位，不能使用序列号校验码算法。".L10nFormat(code));
            }
            return base.OnValidateCode(code);
        }

        /// <summary>
        /// 获取校验码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected override string GetCheckBit(string code)
        {
            return Compute(code);
        }


        /// <summary>
        /// 计算校验码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string Compute(string code)
        {
            int sum = 0;
            if (code.Length < 12)
                return "";
            var keyCode = code.Substring(0, 12).ToUpper();
            for (int i = 1; i <= keyCode.Length; i++)
            {
                if (dicWordValue.ContainsKey(keyCode[i - 1]))
                    sum += (int)(dicWordValue[keyCode[i - 1]] * Math.Pow(2, i - 1));
            }
            return (sum % 13).ToString("X");    //转换成16进制
        }
    }
}
