using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.ProcessTechs.ImportProcessTech
{
    /// <summary>
    /// 制程工艺导入引用验证
    /// </summary>
    public class ValidProcessTechDataHelper
    {
        /// <summary>
        /// 
        /// </summary>
        protected ValidProcessTechDataHelper() { }

        /// <summary>
        /// 验证制程类型
        /// </summary>
        /// <param name="ProcessTechTypeList">制程类型范围</param>
        /// <param name="context">上下文</param>
        /// <param name="messageTip">提示</param>
        /// <returns>true/false</returns>
        public static bool ValidProcessTechType(ref List<ProcessTechType> ProcessTechTypeList, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (ProcessTechTypeList.Count == 0)
            {
                ProcessTechTypeList = RT.Service.Resolve<ProcessTechTypeController>().GetProcessTechType().ToList();
            }

            var ProcessTechType = ProcessTechTypeList.FirstOrDefault(p => p.Code == context || p.Name == context);

            if (ProcessTechType == null)
            {
                messageTip = "[{0}]不存在于系统".L10nFormat(context);
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证工段
        /// </summary>
        /// <param name="ProcessSegmentList">工段范围</param>
        /// <param name="context">上下文</param>
        /// <param name="messageTip">提示</param>
        /// <returns>true/false</returns>
        public static bool ValidProcessSegment(ref List<ProcessSegment> ProcessSegmentList, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (string.IsNullOrEmpty(context)) return isValid;

            if (ProcessSegmentList.Count == 0)
            {
                ProcessSegmentList = RT.Service.Resolve<ProcessSegmentController>().GetProcessSegmentList().ToList();
            }

            var ProcessSegment = ProcessSegmentList.FirstOrDefault(p => p.Code == context || p.Name == context);

            if (ProcessSegment == null)
            {
                messageTip = "[{0}]不存在于系统".L10nFormat(context);
                isValid = false;
            }

            return isValid;
        }
    }
}
