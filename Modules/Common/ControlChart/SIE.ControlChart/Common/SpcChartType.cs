using System.Collections.Generic;

namespace SIE.ControlChart.Common
{
    /// <summary>
    /// 控制图类型
    /// </summary> 
    public static class SpcChartType
    {
        /// <summary>
        /// Xbar_R
        /// </summary>
        public const string Xbar_R = "Xbar-R";
        /// <summary>
        /// Xbar_S
        /// </summary>
        public const string Xbar_S = "Xbar-S";
        /// <summary>
        /// Me_R
        /// </summary>
        public const string Me_R = "Me-R";
        /// <summary>
        /// I_MR
        /// </summary>
        public const string I_MR = "I-MR";
        /// <summary>
        /// P
        /// </summary>
        public const string P = "P";
        /// <summary>
        /// Np
        /// </summary>
        public const string Np = "np";
        /// <summary>
        /// C
        /// </summary>
        public const string C = "C";
        /// <summary>
        /// U
        /// </summary>
        public const string U = "U";

        /// <summary>
        /// 所有图表类型
        /// </summary>
        public static List<string> AllTypes { get; set; }

        /// <summary>
        /// 判断类型是否是计量型控制图
        /// </summary>
        /// <param name="type">控制图类型名称</param>
        /// <returns></returns>
        public static bool IsContinuousCharType(string type)
        {
            var result = false;
            switch (type)
            {
                case Xbar_R:
                case Xbar_S:
                case Me_R:
                case I_MR:
                    result = true;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 判断类型是否是计数型控制图
        /// </summary>
        /// <param name="type">控制图类型名称</param>
        /// <returns></returns>
        public static bool IsAttributeCharType(string type)
        {
            var result = false;
            switch (type)
            {
                case P:
                case Np:
                case C:
                case U:
                    result = true;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 判断类型是否是计数型Ng百分比控制图
        /// </summary>
        /// <param name="type">控制图类型名称</param>
        /// <returns></returns>
        public static bool IsNgPerCharType(string type)
        {
            var result = false;
            if (type == P || type == U)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 判断类型是否是计数型Ng数量控制图
        /// </summary>
        /// <param name="type">控制图类型名称</param>
        /// <returns></returns>
        public static bool IsNgNumCharType(string type)
        {
            var result = false;
            if (type == Np || type == C)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 判断类型是否计数型-不合格品数控制图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsFailQtyType(string type)
        {
            var result = false;
            if (type == Np || type == P)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 判断类型是否计数型-缺陷数控制图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDefectQtyType(string type)
        {
            var result = false;
            if (type == C || type == U)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取控制图所有类型
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllTypes()
        {
            if (AllTypes == null)
            {
                AllTypes = new List<string>() { Xbar_R, Xbar_S, Me_R, I_MR, P, Np, C, U };
            }
            return AllTypes;
        }
    }
}
