using SIE.Domain.Validation;
using System;
using System.Text.RegularExpressions;

namespace SIE.Warehouses
{
    /// <summary>
    /// WCS地址控制器
    /// </summary>
    public partial class WcsAddressController : DomainController
    {
        #region WCS地址验证 适用于下一步地址是：巷道、站台、站台组的编码命名规范
        /// <summary>
        /// 地址验证
        /// </summary>
        /// <param name="addr">地址</param>
        /// <returns></returns>
        public virtual bool CheckAddr(string addr)
        {
            const int INVALID_VALUE = int.MinValue + 1;
            /// <summary>
            /// 排层列深度四个字的整形数组，默认为无效值
            /// </summary>
            int[] coordinates = new int[4]
            { INVALID_VALUE, INVALID_VALUE, INVALID_VALUE,INVALID_VALUE};

            string area = "";
            string warehouse = "";
            string _fullName = addr;
            string[] strArray1 = addr.Split(':');

            if (strArray1.Length != 2)
            {
                throw new ValidationException("无效地址请参考地址的命名规则（分号为英文分号）。错误的地址为：".L10N() + addr);
            }
            string type = !strArray1[1].EndsWith("_") ? strArray1[0] : throw new Exception("格式错误,地址不能以下划线（_）结尾,错误地址为：" + _fullName);
            string[] strArray2 = strArray1[1].Split('_');
            int iLength = strArray2.Length;
            int num = iLength <= 6 ? iLength : throw new Exception("无效地址,坐标无效。错误的地址为：" + addr);
            for (int index = 0; index < iLength; ++index)
            {
                if (this.checkArea(strArray2[index]))
                {
                    num = index;
                    area = strArray2[index];
                    break;
                }
                if (index >= coordinates.Length)
                    throw new ValidationException("无效地址,坐标维度超出范围。错误的地址为：".L10N() + _fullName);
                if (strArray2[index] != "")
                {
                    if (!int.TryParse(strArray2[index], out int result))
                        throw new ValidationException("无效地址,地址内的排/层/列/深度不是有效的数字字符串。错误的地址为：".L10N() + addr);
                    coordinates[index] = result;
                }
            }
            if (num == iLength - 2)
                warehouse = strArray2[iLength - 1];
            if (num < iLength - 2)
            {
                throw new ValidationException("无效地址,地址内的仓库/库区出现意外的位置。错误的地址为：".L10N() + _fullName);
            }
            this.paramHander(type, coordinates[0], coordinates[1], coordinates[2], coordinates[3], area, warehouse);
            return true;
        }

        private bool checkArea(string addr)
        {
            return Regex.IsMatch(addr, "^[a-zA-Z][0-9a-zA-Z]{0,2}");
        }

        private bool checkWarehouse(string addr)
        {
            return addr.Length > 0 && addr.Length <= 13;
        }

        private bool checkDepth(int depth)
        {
            const int INVALID_VALUE = int.MinValue + 1;
            return depth >= 0 || depth == INVALID_VALUE;
        }

        private void paramHander(string type, int range, int row, int column, int depth, string area, string warehouse)
        {
            const int INVALID_VALUE = int.MinValue + 1;
            const string str = "";
            if (string.IsNullOrEmpty(type))
                throw new ValidationException("无效地址,地址类型不能为空。".L10N() + type);
            if (!this.checkDepth(depth))
                throw new ValidationException("无效地址,地址内的深度不可小于0,当前的仓位深浅度为：".L10N() + (object)depth);
            if (!str.Equals(area) && !this.checkArea(area))
                throw new ValidationException("无效地址,库区地址格式错误。".L10N() + area);
            if (!str.Equals(warehouse) && !this.checkWarehouse(warehouse))
                throw new ValidationException("无效地址,库房地址格式错误,长度应在1到13之间,错误的库房地址为：".L10N() + warehouse);
            if (str.Equals(area) && !str.Equals(warehouse))
                throw new ValidationException("无效地址,指定库房时应同时指定库区。".L10N());
            if (range != INVALID_VALUE && range < 1)
                throw new ValidationException("无效排地址,排地址数组应该大于零。".L10N());
            if (row != INVALID_VALUE && row < 1)
                throw new ValidationException("无效层地址,层地址数组应该大于零。".L10N());
            if (column != INVALID_VALUE && column < 1)
                throw new ValidationException("无效列地址,列地址数组应该大于零。".L10N());
        }

        #endregion
    }
}
