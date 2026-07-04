using SIE.Common.ImportHelper;
using System;
using System.Collections.Generic;

namespace SIE.Defects.ImportInspection
{
    /// <summary>
    /// 导入功能业务数据的验证
    /// </summary>
    public static class BussinessDataValid
    {
        #region 模型

        #endregion

        #region 检验项目维护 验证

        /// <summary>
        /// 验证状态
        /// </summary>
        /// <param name="deckRange">状态范围</param>
        /// <param name="context">状态</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidQualityType(ref Dictionary<string, Enum> deckRange, string context, out string messageTip)
        {

            bool isValid = true;
            messageTip = string.Empty;
            if (deckRange == null)
            {
                deckRange = ImportExtension.GetEnumLabel(typeof(QualityType), string.Empty);
            }

            if (!deckRange.ContainsKey(context))
            {
                messageTip = string.Concat("只能选择".L10N(), string.Join(",", deckRange.Keys));
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证项目分类
        /// </summary>
        /// <param name="defectCategory"></param>
        /// <param name="context">项目分类编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidDefectCategory(ref Dictionary<string, double> defectCategory, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (defectCategory == null)
            {
                defectCategory = new Dictionary<string, double>();
            }

            if (!defectCategory.ContainsKey(context))
            {
                DefectCategory category = RT.Service.Resolve<DefectController>().GetDefectCategory(context);
                if (category != null)
                {
                    defectCategory.Add(context, category.Id);
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证缺陷等级
        /// </summary>
        /// <param name="defectGrade"></param>
        /// <param name="context">缺陷等级名称</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidDefectGrade(ref Dictionary<string, double> defectGrade, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (defectGrade == null)
            {
                defectGrade = new Dictionary<string, double>();
            }

            if (!defectGrade.ContainsKey(context))
            {
                DefectGrade category = RT.Service.Resolve<DefectController>().GetDefectGrade(context);
                if (category != null)
                {
                    defectGrade.Add(context, category.Id);
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
                    isValid = false;
                }
            }

            return isValid;
        }

        #endregion
    }
}
